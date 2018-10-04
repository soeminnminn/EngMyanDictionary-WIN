// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using SQLite.Properties;
using Microsoft.Data.SQLite.Utilities;
using SQLitePCL;

namespace Microsoft.Data.SQLite
{
    /// <summary>
    /// Represents a connection to a SQLite database.
    /// </summary>
    public partial class SQLiteConnection : DbConnection
    {
        private const string MainDatabaseName = "main";

        private readonly IList<WeakReference<SQLiteCommand>> _commands = new List<WeakReference<SQLiteCommand>>();

        private string _connectionString;
        private ConnectionState _state;
        private sqlite3 _db;

        static SQLiteConnection()
        {
            BundleInitializer.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteConnection" /> class.
        /// </summary>
        public SQLiteConnection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteConnection" /> class.
        /// </summary>
        /// <param name="connectionString">The string used to open the connection.</param>
        /// <seealso cref="SQLiteConnectionStringBuilder" />
        public SQLiteConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Creates a database file.  This just creates a zero-byte file which SQLite
        /// will turn into a database when the file is opened properly.
        /// </summary>
        /// <param name="databaseFileName">The file to create</param>
        static public void CreateFile(string databaseFileName)
        {
            FileStream fs = File.Create(databaseFileName);
            fs.Close();
        }

        /// <summary>
        /// Gets a handle to underlying database connection.
        /// </summary>
        /// <value>A handle to underlying database connection.</value>
        /// <seealso href="http://sqlite.org/c3ref/sqlite3.html">Database Connection Handle</seealso>
        public virtual sqlite3 Handle
        {
            get { return _db; }
        }

        /// <summary>
        /// Gets or sets a string used to open the connection.
        /// </summary>
        /// <value>A string used to open the connection.</value>
        /// <seealso cref="SQLiteConnectionStringBuilder" />
        public override string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                if (State != ConnectionState.Closed)
                {
                    throw new InvalidOperationException(Resources.ConnectionStringRequiresClosedConnection);
                }

                _connectionString = value;
                ConnectionStringBuilder = new SQLiteConnectionStringBuilder(value);
            }
        }

        internal SQLiteConnectionStringBuilder ConnectionStringBuilder { get; set; }

        /// <summary>
        /// Gets the name of the current database. Always 'main'.
        /// </summary>
        /// <value>The name of the current database.</value>
        public override string Database
        {
            get { return MainDatabaseName; }
        }

        /// <summary>
        /// Gets the path to the database file. Will be absolute for open connections.
        /// </summary>
        /// <value>The path to the database file.</value>
        public override string DataSource
        {
            get
            {
                string dataSource = null;
                if (State == ConnectionState.Open)
                {
                    dataSource = raw.sqlite3_db_filename(_db, MainDatabaseName);
                }

                return dataSource ?? ConnectionStringBuilder.DataSource;
            }
        }

        /// <summary>
        /// Gets the version of SQLite used by the connection.
        /// </summary>
        /// <value>The version of SQLite used by the connection.</value>
        public override string ServerVersion
        {
            get { return raw.sqlite3_libversion(); }
        }

        /// <summary>
        /// Gets the current state of the connection.
        /// </summary>
        /// <value>The current state of the connection.</value>
        public override ConnectionState State
        {
            get { return _state; }
        }

        /// <summary>
        /// Gets or sets the transaction currently being used by the connection, or null if none.
        /// </summary>
        /// <value>The transaction currently being used by the connection.</value>
        protected internal virtual SQLiteTransaction Transaction { get; set; }

        private void SetState(ConnectionState value)
        {
            var originalState = _state;
            if (originalState != value)
            {
                _state = value;
                OnStateChange(new StateChangeEventArgs(originalState, value));
            }
        }

        /// <summary>
        /// Opens a connection to the database using the value of <see cref="ConnectionString" />.
        /// </summary>
        /// <exception cref="SQLiteException">A SQLite error occurs while opening the connection.</exception>
        public override void Open()
        {
            if (State == ConnectionState.Open)
            {
                return;
            }
            if (ConnectionString == null)
            {
                throw new InvalidOperationException(Resources.OpenRequiresSetConnectionString);
            }

            var filename = ConnectionStringBuilder.DataSource;
            var flags = 0;

            if (filename.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
            {
                flags |= raw.SQLITE_OPEN_URI;
            }

            switch (ConnectionStringBuilder.Mode)
            {
                case SQLiteOpenMode.ReadOnly:
                    flags |= raw.SQLITE_OPEN_READONLY;
                    break;

                case SQLiteOpenMode.ReadWrite:
                    flags |= raw.SQLITE_OPEN_READWRITE;
                    break;

                case SQLiteOpenMode.Memory:
                    flags |= raw.SQLITE_OPEN_READWRITE | raw.SQLITE_OPEN_CREATE | raw.SQLITE_OPEN_MEMORY;
                    if ((flags & raw.SQLITE_OPEN_URI) == 0)
                    {
                        flags |= raw.SQLITE_OPEN_URI;
                        filename = "file:" + filename;
                    }
                    break;

                default:
                    Debug.Assert(
                        ConnectionStringBuilder.Mode == SQLiteOpenMode.ReadWriteCreate,
                        "ConnectionStringBuilder.Mode is not ReadWriteCreate");
                    flags |= raw.SQLITE_OPEN_READWRITE | raw.SQLITE_OPEN_CREATE;
                    break;
            }

            switch (ConnectionStringBuilder.Cache)
            {
                case SQLiteCacheMode.Shared:
                    flags |= raw.SQLITE_OPEN_SHAREDCACHE;
                    break;

                case SQLiteCacheMode.Private:
                    flags |= raw.SQLITE_OPEN_PRIVATECACHE;
                    break;

                default:
                    Debug.Assert(
                        ConnectionStringBuilder.Cache == SQLiteCacheMode.Default,
                        "ConnectionStringBuilder.Cache is not Default.");
                    break;
            }

            var dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
            if (!string.IsNullOrEmpty(dataDirectory)
                && (flags & raw.SQLITE_OPEN_URI) == 0
                && !filename.Equals(":memory:", StringComparison.OrdinalIgnoreCase)
                && !Path.IsPathRooted(filename))
            {
                filename = Path.Combine(dataDirectory, filename);
            }

            var rc = raw.sqlite3_open_v2(filename, out _db, flags, vfs: null);
            SQLiteException.ThrowExceptionForRC(rc, _db);

            SetState(ConnectionState.Open);
        }

        /// <summary>
        /// Closes the connection to the database. Open transactions are rolled back.
        /// </summary>
        public override void Close()
        {
            if (_db == null
                || _db.ptr == IntPtr.Zero)
            {
                return;
            }

            Transaction?.Dispose();

            foreach (var reference in _commands)
            {
                SQLiteCommand command = null;
                if (reference.TryGetTarget(out command))
                {
                    command.Dispose();
                }
            }

            _commands.Clear();

            _db.Dispose2();
            _db = null;
            SetState(ConnectionState.Closed);
        }

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
                Close();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates a new command associated with the connection.
        /// </summary>
        /// <returns>The new command.</returns>
        /// <remarks>
        /// The command's <seealso cref="SQLiteCommand.Transaction" /> property will also be set to the current
        /// transaction.
        /// </remarks>
        public new virtual SQLiteCommand CreateCommand()
        {
            return new SQLiteCommand { Connection = this, Transaction = Transaction };
        }

        /// <summary>
        /// Creates a new command associated with the connection.
        /// </summary>
        /// <returns>The new command.</returns>
        protected override DbCommand CreateDbCommand()
        {
            return CreateCommand();
        }

        internal void AddCommand(SQLiteCommand command)
        {
            _commands.Add(new WeakReference<SQLiteCommand>(command));
        }

        internal void RemoveCommand(SQLiteCommand command)
        {
            for (var i = _commands.Count - 1; i >= 0; i--)
            {
                SQLiteCommand item = null;
                if (!_commands[i].TryGetTarget(out item) || item == command)
                {
                    _commands.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Create custom collation.
        /// </summary>
        /// <param name="name">Name of the collation.</param>
        /// <param name="comparison">Method that compares two strings.</param>
        public virtual void CreateCollation(string name, Comparison<string> comparison)
        {
            CreateCollation<object>(name, null, comparison != null ? (_, s1, s2) => comparison(s1, s2) : (Func<object, string, string, int>)null);
        }

        /// <summary>
        /// Create custom collation.
        /// </summary>
        /// <typeparam name="T">The type of the state object.</typeparam>
        /// <param name="name">Name of the collation.</param>
        /// <param name="state">State object passed to each invocation of the collation.</param>
        /// <param name="comparison">Method that compares two strings, using additional state.</param>
        public virtual void CreateCollation<T>(string name, T state, Func<T, string, string, int> comparison)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (State != ConnectionState.Open)
            {
                throw new InvalidOperationException(Resources.CallRequiresOpenConnection(nameof(CreateCollation)));
            }

            delegate_collation collation = comparison != null ? (v, s1, s2) => comparison((T)v, s1, s2) : (delegate_collation)null;
            var rc = raw.sqlite3_create_collation(_db, name, state, collation);
            SQLiteException.ThrowExceptionForRC(rc, _db);
        }

        /// <summary>
        /// Begins a transaction on the connection.
        /// </summary>
        /// <returns>The transaction.</returns>
        public new virtual SQLiteTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.Unspecified);
        }

        /// <summary>
        /// Begins a transaction on the connection.
        /// </summary>
        /// <param name="isolationLevel">The isolation level of the transaction.</param>
        /// <returns>The transaction.</returns>
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// Begins a transaction on the connection.
        /// </summary>
        /// <param name="isolationLevel">The isolation level of the transaction.</param>
        /// <returns>The transaction.</returns>
        public new virtual SQLiteTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            if (State != ConnectionState.Open)
            {
                throw new InvalidOperationException(Resources.CallRequiresOpenConnection(nameof(BeginTransaction)));
            }
            if (Transaction != null)
            {
                throw new InvalidOperationException(Resources.ParallelTransactionsNotSupported);
            }

            return Transaction = new SQLiteTransaction(this, isolationLevel);
        }

        /// <summary>
        /// Changes the current database. Not supported.
        /// </summary>
        /// <param name="databaseName">The name of the database to use.</param>
        /// <exception cref="NotSupportedException">Always.</exception>
        public override void ChangeDatabase(string databaseName)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Enables extension loading on the connection.
        /// </summary>
        /// <param name="enable">true to enable; false to disable</param>
        /// <seealso href="http://sqlite.org/loadext.html">Run-Time Loadable Extensions</seealso>
        public virtual void EnableExtensions(bool enable = true)
        {
            if (_db == null
                || _db.ptr == IntPtr.Zero)
            {
                throw new InvalidOperationException(Resources.CallRequiresOpenConnection(nameof(EnableExtensions)));
            }

            var rc = raw.sqlite3_enable_load_extension(_db, enable ? 1 : 0);
            SQLiteException.ThrowExceptionForRC(rc, _db);
        }
    }
}
