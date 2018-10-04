// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using SQLite.Properties;
using SQLitePCL;

namespace Microsoft.Data.SQLite
{
    // TODO: Truncate to specified size
    // TODO: Infer type and size from value
    /// <summary>
    /// Represents a parameter and its value in a <see cref="SQLiteCommand" />.
    /// </summary>
    /// <remarks>Due to SQLite's dynamic type system, parameter values are not converted.</remarks>
    /// <seealso href="http://sqlite.org/datatype3.html">Datatypes In SQLite Version 3</seealso>
    public class SQLiteParameter : DbParameter
    {
        private string _parameterName = string.Empty;
        private object _value;
        private Action<sqlite3_stmt, int> _bindAction;
        private bool _bindActionValid;

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteParameter" /> class.
        /// </summary>
        public SQLiteParameter()
        {
            DbType = DbType.String;
            SqliteType = SQLiteType.Text;
            SourceColumn = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteParameter" /> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Can be null.</param>
        public SQLiteParameter(string name, object value)
            : this()
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            _parameterName = name;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteParameter" /> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The type of the parameter.</param>
        public SQLiteParameter(string name, SQLiteType type)
            : this()
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            _parameterName = name;
            SqliteType = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteParameter" /> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The type of the parameter.</param>
        /// <param name="size">The maximum size, in bytes, of the parameter.</param>
        public SQLiteParameter(string name, SQLiteType type, int size)
            : this(name, type)
        {
            Size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteParameter" /> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="type">The type of the parameter.</param>
        /// <param name="size">The maximum size, in bytes, of the parameter.</param>
        /// <param name="sourceColumn">The source column used for loading the value. Can be null.</param>
        public SQLiteParameter(string name, SQLiteType type, int size, string sourceColumn)
            : this(name, type, size)
        {
            SourceColumn = sourceColumn;
        }

        /// <summary>
        /// Gets or sets the type of the parameter.
        /// </summary>
        /// <value>The type of the parameter.</value>
        /// <remarks>Due to SQLite's dynamic type system, parameter values are not converted.</remarks>
        /// <seealso href="http://sqlite.org/datatype3.html">Datatypes In SQLite Version 3</seealso>
        public override DbType DbType { get; set; }

        /// <summary>
        /// Gets or sets the SQLite type of the parameter.
        /// </summary>
        /// <value>The SQLite type of the parameter.</value>
        /// <remarks>Due to SQLite's dynamic type system, parameter values are not converted.</remarks>
        /// <seealso href="http://sqlite.org/datatype3.html">Datatypes In SQLite Version 3</seealso>
        public virtual SQLiteType SqliteType { get; set; }

        /// <summary>
        /// Gets or sets the direction of the parameter. Only <see cref="ParameterDirection.Input" /> is supported.
        /// </summary>
        /// <value>The direction of the parameter.</value>
        public override ParameterDirection Direction
        {
            get { return ParameterDirection.Input; }
            set
            {
                if (value != ParameterDirection.Input)
                {
                    throw new ArgumentException(Resources.InvalidParameterDirection(value));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the parameter is nullable.
        /// </summary>
        /// <value>A value indicating whether the parameter is nullable.</value>
        public override bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public override string ParameterName
        {
            get { return _parameterName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _parameterName = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum size, in bytes, of the parameter.
        /// </summary>
        /// <value>The maximum size, in bytes, of the parameter.</value>
        public override int Size { get; set; }

        /// <summary>
        /// Gets or sets the source column used for loading the value.
        /// </summary>
        /// <value>The source column used for loading the value.</value>
        public override string SourceColumn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the source column is nullable.
        /// </summary>
        /// <value>A value indicating whether the source column is nullable.</value>
        public override bool SourceColumnNullMapping { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        /// <value>The value of the parameter.</value>
        /// <remarks>Due to SQLite's dynamic type system, parameter values are not converted.</remarks>
        /// <seealso href="http://sqlite.org/datatype3.html">Datatypes In SQLite Version 3</seealso>
        public override object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                _bindActionValid = false;
            }
        }

        /// <summary>
        /// Gets or sets the System.Data.DataRowVersion to use when you load System.Data.Common.DbParameter.Value.
        /// </summary>
        /// <value>One of the System.Data.DataRowVersion values. The default is Current.</value>
        public override DataRowVersion SourceVersion { get; set; }

        /// <summary>
        /// Resets the <see cref="DbType" /> property to its original value.
        /// </summary>
        public override void ResetDbType()
        {
            ResetSqliteType();
        }

        /// <summary>
        /// Resets the <see cref="SqliteType" /> property to its original value.
        /// </summary>
        public virtual void ResetSqliteType()
        {
            DbType = DbType.String;
            SqliteType = SQLiteType.Text;
        }

        internal bool Bind(sqlite3_stmt stmt)
        {
            if (_parameterName.Length == 0)
            {
                throw new InvalidOperationException(Resources.RequiresSet(nameof(ParameterName)));
            }

            var index = raw.sqlite3_bind_parameter_index(stmt, _parameterName);
            if (index == 0 &&
                (index = FindPrefixedParameter(stmt)) == 0)
            {
                return false;
            }

            if (_value == null)
            {
                throw new InvalidOperationException(Resources.RequiresSet(nameof(Value)));
            }

            if (!_bindActionValid)
            {
                var type = Value.GetType().UnwrapNullableType().UnwrapEnumType();
                if (type == typeof(bool))
                {
                    var value = (bool)_value ? 1L : 0;
                    _bindAction = (s, i) => raw.sqlite3_bind_int64(s, i, value);
                }
                else if (type == typeof(byte))
                {
                    var value = (long)(byte)_value;
                    _bindAction = (s, i) => raw.sqlite3_bind_int64(s, i, value);
                }
                else if (type == typeof(byte[]))
                {
                    var value = (byte[])_value;
                    _bindAction = (s, i) => BindBlob(s, i, value);
                }
                else if (type == typeof(char))
                {
                    var value = (long)(char)_value;
                    _bindAction = (s, i) => raw.sqlite3_bind_int64(s, i, value);
                }
                else if (type == typeof(DateTime))
                {
                    var value = ((DateTime)_value).ToString(@"yyyy\-MM\-dd HH\:mm\:ss.FFFFFFF");
                    _bindAction = (s, i) => BindText(s, i, value);
                }
                else if (type == typeof(DateTimeOffset))
                {
                    var value = ((DateTimeOffset)_value).ToString(@"yyyy\-MM\-dd HH\:mm\:ss.FFFFFFFzzz");
                    _bindAction = (s, i) => BindText(s, i, value);
                }
                else if (type == typeof(DBNull))
                {
                    _bindAction = (s, i) => raw.sqlite3_bind_null(s, i);
                }
                else if (type == typeof(decimal))
                {
                    var value = ((decimal)_value).ToString("0.0###########################", CultureInfo.InvariantCulture);
                    _bindAction = (s, i) => BindText(s, i, value);
                }
                else if (type == typeof(double))
                {
                    var value = (double)_value;
                    _bindAction = (s, i) => BindDouble(s, i, value);
                }
                else if (type == typeof(float))
                {
                    var value = (double)(float)_value;
                    _bindAction = (s, i) => BindDouble(s, i, value);
                }
                else if (type == typeof(Guid))
                {
                    var value = ((Guid)_value).ToByteArray();
                    _bindAction = (s, i) => BindBlob(s, i, value);
                }
                else if (type == typeof(int))
                {
                    var value = (long)(int)_value;
                    _bindAction = (s, i) => raw.sqlite3_bind_int64(s, i, value);
                }
                else if (type == typeof(long))
                {
                    var value = (long)_value;
                    _bindAction = (s, i) => raw.sqlite3_bind_int64(s, i, value);
                }
                else if (type == typeof(sbyte))
                {
                    var value = (long)(sbyte)_value;
                    _bindAction = (s, i) => raw.sqlite3_bind_int64(s, i, value);
                }
                else if (type == typeof(short))
                {
                    var value = (long)(short)_value;
                    _bindAction = (s, i) => raw.sqlite3_bind_int64(s, i, value);
                }
                else if (type == typeof(string))
                {
                    var value = (string)_value;
                    _bindAction = (s, i) => BindText(s, i, value);
                }
                else if (type == typeof(TimeSpan))
                {
                    var value = ((TimeSpan)_value).ToString("c");
                    _bindAction = (s, i) => BindText(s, i, value);
                }
                else if (type == typeof(uint))
                {
                    var value = (long)(uint)_value;
                    _bindAction = (s, i) => raw.sqlite3_bind_int64(s, i, value);
                }
                else if (type == typeof(ulong))
                {
                    var value = (long)(ulong)_value;
                    _bindAction = (s, i) => raw.sqlite3_bind_int64(s, i, value);
                }
                else if (type == typeof(ushort))
                {
                    var value = (long)(ushort)_value;
                    _bindAction = (s, i) => raw.sqlite3_bind_int64(s, i, value);
                }
                else
                {
                    throw new InvalidOperationException(Resources.UnknownDataType(type));
                }

                _bindActionValid = true;
            }

            _bindAction(stmt, index);

            return true;
        }

        private static void BindBlob(sqlite3_stmt stmt, int index, byte[] value)
        {
            raw.sqlite3_bind_blob(stmt, index, value);
        }

        private static void BindText(sqlite3_stmt stmt, int index, string value)
        {
            raw.sqlite3_bind_text(stmt, index, value);
        }

        private static void BindDouble(sqlite3_stmt stmt, int index, double value)
        {
            if (double.IsNaN(value))
            {
                throw new InvalidOperationException(Resources.CannotStoreNaN);
            }

            raw.sqlite3_bind_double(stmt, index, value);
        }

        private readonly static char[] _parameterPrefixes = { '@', '$', ':' };

        private int FindPrefixedParameter(sqlite3_stmt stmt)
        {
            var index = 0;
            int nextIndex;

            foreach (var prefix in _parameterPrefixes)
            {
                if (_parameterName[0] == prefix)
                {
                    // If name already has a prefix characters, the first call to sqlite3_bind_parameter_index
                    // would have worked if the parameter name was in the statement
                    return 0;
                }

                nextIndex = raw.sqlite3_bind_parameter_index(stmt, prefix + _parameterName);

                if (nextIndex == 0)
                {
                    continue;
                }

                if (index != 0)
                {
                    throw new InvalidOperationException(Resources.AmbiguousParameterName(_parameterName));
                }

                index = nextIndex;
            }

            return index;
        }
    }
}
