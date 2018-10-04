// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SQLite.Properties;
using SQLitePCL;

namespace Microsoft.Data.SQLite
{
    /// <summary>
    /// Represents a SQL statement to be executed against a SQLite database.
    /// </summary>
    public class SQLiteCommand : DbCommand
    {
        private readonly Lazy<SQLiteParameterCollection> _parameters = new Lazy<SQLiteParameterCollection>(
            () => new SQLiteParameterCollection());
        private readonly ICollection<sqlite3_stmt> _preparedStatements = new List<sqlite3_stmt>();
        private SQLiteConnection _connection;
        private string _commandText;

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteCommand" /> class.
        /// </summary>
        public SQLiteCommand()
        {
            CommandTimeout = 30;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteCommand" /> class.
        /// </summary>
        /// <param name="commandText">The SQL to execute against the database.</param>
        public SQLiteCommand(string commandText)
            : this()
        {
            CommandText = commandText;
        }
            

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteCommand" /> class.
        /// </summary>
        /// <param name="commandText">The SQL to execute against the database.</param>
        /// <param name="connection">The connection used by the command.</param>
        public SQLiteCommand(string commandText, SQLiteConnection connection)
            : this(commandText)
        {
            Connection = connection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteCommand" /> class.
        /// </summary>
        /// <param name="commandText">The SQL to execute against the database.</param>
        /// <param name="connection">The connection used by the command.</param>
        /// <param name="transaction">The transaction within which the command executes.</param>
        public SQLiteCommand(string commandText, SQLiteConnection connection, SQLiteTransaction transaction)
            : this(commandText, connection)
        {
            Transaction = transaction;
        }

        /// <summary>
        /// Gets or sets a value indicating how <see cref="CommandText" /> is interpreted. Only
        /// <see cref="CommandType.Text" /> is supported.
        /// </summary>
        /// <value>A value indicating how <see cref="CommandText" /> is interpreted.</value>
        public override CommandType CommandType
        {
            get { return CommandType.Text; }
            set
            {
                if (value != CommandType.Text)
                {
                    throw new ArgumentException(Resources.InvalidCommandType(value));
                }
            }
        }

        /// <summary>
        /// Gets or sets the SQL to execute against the database.
        /// </summary>
        /// <value>The SQL to execute against the database.</value>
        public override string CommandText
        {
            get { return _commandText; }
            set
            {
                if (DataReader != null)
                {
                    throw new InvalidOperationException(Resources.SetRequiresNoOpenReader(nameof(CommandText)));
                }

                if (value != _commandText)
                {
                    DisposePreparedStatements();
                    _commandText = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the connection used by the command.
        /// </summary>
        /// <value>The connection used by the command.</value>
        public new virtual SQLiteConnection Connection
        {
            get { return _connection; }
            set
            {
                if (DataReader != null)
                {
                    throw new InvalidOperationException(Resources.SetRequiresNoOpenReader(nameof(Connection)));
                }

                if (value != _connection)
                {
                    DisposePreparedStatements();

                    _connection?.RemoveCommand(this);
                    _connection = value;
                    value?.AddCommand(this);
                }
            }
        }

        /// <summary>
        /// Gets or sets the connection used by the command. Must be a <see cref="SQLiteConnection" />.
        /// </summary>
        /// <value>The connection used by the command.</value>
        protected override DbConnection DbConnection
        {
            get { return Connection; }
            set { Connection = (SQLiteConnection)value; }
        }

        /// <summary>
        /// Gets or sets the transaction within which the command executes.
        /// </summary>
        /// <value>The transaction within which the command executes.</value>
        public new virtual SQLiteTransaction Transaction { get; set; }

        /// <summary>
        /// Gets or sets the transaction within which the command executes. Must be a <see cref="SQLiteTransaction" />.
        /// </summary>
        /// <value>The transaction within which the command executes.</value>
        protected override DbTransaction DbTransaction
        {
            get { return Transaction; }
            set { Transaction = (SQLiteTransaction)value; }
        }

        /// <summary>
        /// Gets the collection of parameters used by the command.
        /// </summary>
        /// <value>The collection of parameters used by the command.</value>
        public new virtual SQLiteParameterCollection Parameters
        {
            get { return _parameters.Value; }
        }

        /// <summary>
        /// Gets the collection of parameters used by the command.
        /// </summary>
        /// <value>The collection of parameters used by the command.</value>
        protected override DbParameterCollection DbParameterCollection
        {
            get { return Parameters; }
        }

        /// <summary>
        /// Gets or sets the number of seconds to wait before terminating the attempt to execute the command. Defaults to 30.
        /// </summary>
        /// <value>The number of seconds to wait before terminating the attempt to execute the command.</value>
        /// <remarks>
        /// The timeout is used when the command is waiting to obtain a lock on the table.
        /// </remarks>
        public override int CommandTimeout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the command should be visible in an interface control.
        /// </summary>
        /// <value>A value indicating whether the command should be visible in an interface control.</value>
        public override bool DesignTimeVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how the results are applied to the row being updated.
        /// </summary>
        /// <value>A value indicating how the results are applied to the row being updated.</value>
        public override UpdateRowSource UpdatedRowSource { get; set; }

        /// <summary>
        /// Gets or sets the data reader currently being used by the command, or null if none.
        /// </summary>
        /// <value>The data reader currently being used by the command.</value>
        protected internal virtual SQLiteDataReader DataReader { get; set; }

        /// <summary>
        /// Releases any resources used by the connection and closes it.
        /// </summary>
        /// <param name="disposing">
        /// true to release managed and unmanaged resources; false to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposePreparedStatements();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates a new parameter.
        /// </summary>
        /// <returns>The new parameter.</returns>
        public new virtual SQLiteParameter CreateParameter()
        {
            return new SQLiteParameter();
        }

        /// <summary>
        /// Creates a new parameter.
        /// </summary>
        /// <returns>The new parameter.</returns>
        protected override DbParameter CreateDbParameter()
        {
            return CreateParameter();
        }

        /// <summary>
        /// Creates a prepared version of the command on the database. This has no effect.
        /// </summary>
        public override void Prepare()
        {
            if (_connection?.State != ConnectionState.Open)
            {
                throw new InvalidOperationException(Resources.CallRequiresOpenConnection(nameof(Prepare)));
            }

            if (string.IsNullOrEmpty(_commandText))
            {
                throw new InvalidOperationException(Resources.CallRequiresSetCommandText(nameof(Prepare)));
            }

            if (_preparedStatements.Count != 0)
            {
                return;
            }

            try
            {
                using (var enumerator = PrepareAndEnumerateStatements().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                    }
                }
            }
            catch
            {
                DisposePreparedStatements();

                throw;
            }
        }

        /// <summary>
        /// Executes the <see cref="CommandText" /> against the database and returns a data reader.
        /// </summary>
        /// <returns>The data reader.</returns>
        /// <exception cref="SQLiteException">A SQLite error occurs during execution.</exception>
        public new virtual SQLiteDataReader ExecuteReader()
        {
            return ExecuteReader(CommandBehavior.Default);
        }

        /// <summary>
        /// Executes the <see cref="CommandText" /> against the database and returns a data reader.
        /// </summary>
        /// <param name="behavior">
        /// A description of the results of the query and its effect on the database.
        /// <para>Only <see cref="CommandBehavior.Default" />, <see cref="CommandBehavior.SequentialAccess" />,
        /// <see cref="CommandBehavior.SingleResult" />, <see cref="CommandBehavior.SingleRow" />, and
        /// <see cref="CommandBehavior.CloseConnection" /> are supported.</para>
        /// </param>
        /// <returns>The data reader.</returns>
        /// <exception cref="SQLiteException">A SQLite error occurs during execution.</exception>
        public new virtual SQLiteDataReader ExecuteReader(CommandBehavior behavior)
        {
            if ((behavior & ~(CommandBehavior.Default | CommandBehavior.SequentialAccess | CommandBehavior.SingleResult
                | CommandBehavior.SingleRow | CommandBehavior.CloseConnection)) != 0)
            {
                throw new ArgumentException(Resources.InvalidCommandBehavior(behavior));
            }

            if (DataReader != null)
            {
                throw new InvalidOperationException(Resources.DataReaderOpen);
            }

            if (_connection?.State != ConnectionState.Open)
            {
                throw new InvalidOperationException(Resources.CallRequiresOpenConnection(nameof(ExecuteReader)));
            }

            if (string.IsNullOrEmpty(_commandText))
            {
                throw new InvalidOperationException(Resources.CallRequiresSetCommandText(nameof(ExecuteReader)));
            }

            if (Transaction != _connection.Transaction)
            {
                throw new InvalidOperationException(
                    Transaction == null
                        ? Resources.TransactionRequired
                        : Resources.TransactionConnectionMismatch);
            }

            // This is not a guarantee. SQLITE_BUSY can still be thrown before the command timeout.
            // This sets a timeout handler but this can be cleared by concurrent commands.
            raw.sqlite3_busy_timeout(_connection.Handle, CommandTimeout * 1000);

            var hasChanges = false;
            var changes = 0;
            int rc;
            var stmts = new Queue<KeyValuePair<sqlite3_stmt, bool>>();
            var unprepared = _preparedStatements.Count == 0;

            try
            {
                foreach (var stmt in unprepared
                    ? PrepareAndEnumerateStatements()
                    : _preparedStatements)
                {
                    var boundParams = 0;

                    if (_parameters.IsValueCreated)
                    {
                        boundParams = _parameters.Value.Bind(stmt);
                    }

                    var expectedParams = raw.sqlite3_bind_parameter_count(stmt);
                    if (expectedParams != boundParams)
                    {
                        var unboundParams = new List<string>();
                        for (var i = 1; i <= expectedParams; i++)
                        {
                            var name = raw.sqlite3_bind_parameter_name(stmt, i);

                            if (_parameters.IsValueCreated
                                ||
                                !_parameters.Value.Cast<SQLiteParameter>().Any(p => p.ParameterName == name))
                            {
                                unboundParams.Add(name);
                            }
                        }

                        throw new InvalidOperationException(Resources.MissingParameters(string.Join(", ", unboundParams)));
                    }

                    var timer = Stopwatch.StartNew();
                    while (raw.SQLITE_LOCKED == (rc = raw.sqlite3_step(stmt)) || rc == raw.SQLITE_BUSY)
                    {
                        if (timer.ElapsedMilliseconds >= CommandTimeout * 1000)
                        {
                            break;
                        }

                        raw.sqlite3_reset(stmt);

                        // TODO: Consider having an async path that uses Task.Delay()
                        Thread.Sleep(150);
                    }

                    SQLiteException.ThrowExceptionForRC(rc, _connection.Handle);

                    if (rc == raw.SQLITE_ROW
                        // NB: This is only a heuristic to separate SELECT statements from INSERT/UPDATE/DELETE statements.
                        //     It will result in false positives, but it's the best we can do without re-parsing SQL
                        || raw.sqlite3_stmt_readonly(stmt) != 0)
                    {
                        stmts.Enqueue(new KeyValuePair<sqlite3_stmt, bool>(stmt, rc != raw.SQLITE_DONE));
                    }
                    else
                    {
                        raw.sqlite3_reset(stmt);
                        hasChanges = true;
                        changes += raw.sqlite3_changes(_connection.Handle);
                    }
                }
            }
            catch when (unprepared)
            {
                DisposePreparedStatements();

                throw;
            }

            var closeConnection = (behavior & CommandBehavior.CloseConnection) != 0;

            return DataReader = new SQLiteDataReader(this, stmts, hasChanges ? changes : -1, closeConnection);
        }

        /// <summary>
        /// Executes the <see cref="CommandText" /> against the database and returns a data reader.
        /// </summary>
        /// <param name="behavior">A description of query's results and its effect on the database.</param>
        /// <returns>The data reader.</returns>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return ExecuteReader(behavior);
        }

        /// <summary>
        /// Executes the <see cref="CommandText" /> asynchronously against the database and returns a data reader.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// SQLite does not support asynchronous execution. Use write-ahead logging instead.
        /// </remarks>
        /// <seealso href="http://sqlite.org/wal.html">Write-Ahead Logging</seealso>
        public new virtual Task<SQLiteDataReader> ExecuteReaderAsync()
        {
            return ExecuteReaderAsync(CommandBehavior.Default, CancellationToken.None);
        }

        /// <summary>
        /// Executes the <see cref="CommandText" /> asynchronously against the database and returns a data reader.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// SQLite does not support asynchronous execution. Use write-ahead logging instead.
        /// </remarks>
        /// <seealso href="http://sqlite.org/wal.html">Write-Ahead Logging</seealso>
        public new virtual Task<SQLiteDataReader> ExecuteReaderAsync(CancellationToken cancellationToken)
        {
            return ExecuteReaderAsync(CommandBehavior.Default, cancellationToken);
        }

        /// <summary>
        /// Executes the <see cref="CommandText" /> asynchronously against the database and returns a data reader.
        /// </summary>
        /// <param name="behavior">A description of query's results and its effect on the database.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// SQLite does not support asynchronous execution. Use write-ahead logging instead.
        /// </remarks>
        /// <seealso href="http://sqlite.org/wal.html">Write-Ahead Logging</seealso>
        public new virtual Task<SQLiteDataReader> ExecuteReaderAsync(CommandBehavior behavior)
        {
            return ExecuteReaderAsync(behavior, CancellationToken.None);
        }

        /// <summary>
        /// Executes the <see cref="CommandText" /> asynchronously against the database and returns a data reader.
        /// </summary>
        /// <param name="behavior">A description of query's results and its effect on the database.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// SQLite does not support asynchronous execution. Use write-ahead logging instead.
        /// </remarks>
        /// <seealso href="http://sqlite.org/wal.html">Write-Ahead Logging</seealso>
        public new virtual Task<SQLiteDataReader> ExecuteReaderAsync(
            CommandBehavior behavior,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(ExecuteReader(behavior));
        }

        /// <summary>
        /// Executes the <see cref="CommandText" /> asynchronously against the database and returns a data reader.
        /// </summary>
        /// <param name="behavior">A description of query's results and its effect on the database.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(
            CommandBehavior behavior,
            CancellationToken cancellationToken)
        {
            return await ExecuteReaderAsync(behavior, cancellationToken);
        }

        /// <summary>
        /// Executes the <see cref="CommandText" /> against the database.
        /// </summary>
        /// <returns>The number of rows inserted, updated, or deleted. -1 for SELECT statements.</returns>
        /// <exception cref="SQLiteException">A SQLite error occurs during execution.</exception>
        public override int ExecuteNonQuery()
        {
            if (_connection?.State != ConnectionState.Open)
            {
                throw new InvalidOperationException(Resources.CallRequiresOpenConnection(nameof(ExecuteNonQuery)));
            }
            if (_commandText == null)
            {
                throw new InvalidOperationException(Resources.CallRequiresSetCommandText(nameof(ExecuteNonQuery)));
            }

            var reader = ExecuteReader();
            reader.Dispose();

            return reader.RecordsAffected;
        }

        /// <summary>
        /// Executes the <see cref="CommandText" /> against the database and returns the result.
        /// </summary>
        /// <returns>The first column of the first row of the results, or null if no results.</returns>
        /// <exception cref="SQLiteException">A SQLite error occurs during execution.</exception>
        public override object ExecuteScalar()
        {
            if (_connection?.State != ConnectionState.Open)
            {
                throw new InvalidOperationException(Resources.CallRequiresOpenConnection(nameof(ExecuteScalar)));
            }
            if (_commandText == null)
            {
                throw new InvalidOperationException(Resources.CallRequiresSetCommandText(nameof(ExecuteScalar)));
            }

            using (var reader = ExecuteReader())
            {
                return reader.Read()
                    ? reader.GetValue(0)
                    : null;
            }
        }

        /// <summary>
        /// Attempts to cancel the execution of the command. Does nothing.
        /// </summary>
        public override void Cancel()
        {
        }

        private IEnumerable<sqlite3_stmt> PrepareAndEnumerateStatements()
        {
            var tail = _commandText;
            sqlite3_stmt stmt = null;
            do
            {
                var rc = raw.sqlite3_prepare_v2(
                    _connection.Handle,
                    tail,
                    out stmt,
                    out tail);
                SQLiteException.ThrowExceptionForRC(rc, _connection.Handle);

                // Statement was empty, white space, or a comment
                if (stmt.ptr == IntPtr.Zero)
                {
                    if (!string.IsNullOrEmpty(tail))
                    {
                        continue;
                    }

                    break;
                }

                _preparedStatements.Add(stmt);

                yield return stmt;
            }
            while (!string.IsNullOrEmpty(tail));
        }

        private void DisposePreparedStatements()
        {
            if (DataReader != null)
            {
                DataReader.Dispose();
                DataReader = null;
            }

            foreach (var stmt in _preparedStatements)
            {
                stmt.Dispose();
            }

            _preparedStatements.Clear();
        }
    }
}
