// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using SQLite.Properties;
using SQLitePCL;

namespace Microsoft.Data.SQLite
{
    /// <summary>
    /// Represents a collection of SQLite parameters.
    /// </summary>
    public class SQLiteParameterCollection : DbParameterCollection
    {
        private readonly List<SQLiteParameter> _parameters = new List<SQLiteParameter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteParameterCollection" /> class.
        /// </summary>
        protected internal SQLiteParameterCollection()
        {
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        /// <value>The number of items in the collection.</value>
        public override int Count
        {
            get { return _parameters.Count; }
        }

        /// <summary>
        /// Gets the object used to synchronize access to the collection.
        /// </summary>
        /// <value>The object used to synchronize access to the collection.</value>
        public override object SyncRoot
        {
            get { return ((ICollection)_parameters).SyncRoot; }
        }

        /// <summary>
        /// Specifies whether the collection is a fixed size.
        /// </summary>
        /// <value>true if the collection is a fixed size; otherwise false.</value>
        public override bool IsFixedSize
        {
            get { return ((IList)_parameters).IsFixedSize; }
        }

        /// <summary>
        /// Specifies whether the collection is read-only.
        /// </summary>
        /// <value>true if the collection is read-only; otherwise false.</value>
        public override bool IsReadOnly
        {
            get { return ((IList)_parameters).IsReadOnly; }
        }

        /// <summary>
        /// Specifies whether the collection is synchronized.
        /// </summary>
        /// <value>true if the collection is synchronized; otherwise false.</value>
        public override bool IsSynchronized
        {
            get { return ((ICollection)_parameters).IsSynchronized; }
        }

        /// <summary>
        /// Gets or sets the parameter at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter.</param>
        /// <returns>The parameter.</returns>
        public new virtual SQLiteParameter this[int index]
        {
            get { return _parameters[index]; }
            set
            {
                if (_parameters[index] == value)
                {
                    return;
                }

                _parameters[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the parameter with the specified name.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The parameter.</returns>
        public new virtual SQLiteParameter this[string parameterName]
        {
            get { return this[IndexOfChecked(parameterName)]; }
            set { this[IndexOfChecked(parameterName)] = value; }
        }

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="value">The parameter to add. Must be a <see cref="SQLiteParameter" />.</param>
        /// <returns>The zero-based index of the parameter that was added.</returns>
        public override int Add(object value)
        {
            _parameters.Add((SQLiteParameter)value);

            return Count - 1;
        }

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="value">The parameter to add.</param>
        /// <returns>The parameter that was added.</returns>
        public virtual SQLiteParameter Add(SQLiteParameter value)
        {
            _parameters.Add(value);

            return value;
        }

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="type">The SQLite type of the parameter.</param>
        /// <returns>The parameter that was added.</returns>
        public virtual SQLiteParameter Add(string parameterName, SQLiteType type)
        {
            return Add(new SQLiteParameter(parameterName, type));
        }

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="type">The SQLite type of the parameter.</param>
        /// <param name="size">The maximum size, in bytes, of the parameter.</param>
        /// <returns>The parameter that was added.</returns>
        public virtual SQLiteParameter Add(string parameterName, SQLiteType type, int size)
        {
            return Add(new SQLiteParameter(parameterName, type, size));
        }

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="type">The SQLite type of the parameter.</param>
        /// <param name="size">The maximum size, in bytes, of the parameter.</param>
        /// <param name="sourceColumn">
        /// The source column used for loading the value of the parameter. Can be null.
        /// </param>
        /// <returns>The parameter that was added.</returns>
        public virtual SQLiteParameter Add(string parameterName, SQLiteType type, int size, string sourceColumn)
        {
            return Add(new SQLiteParameter(parameterName, type, size, sourceColumn));
        }

        /// <summary>
        /// Adds multiple parameters to the collection.
        /// </summary>
        /// <param name="values">
        /// An array of parameters to add. They must be <see cref="SQLiteParameter" /> objects.
        /// </param>
        public override void AddRange(Array values)
        {
            Add(values.Cast<SQLiteParameter>());
        }

        /// <summary>
        /// Adds multiple parameters to the collection.
        /// </summary>
        /// <param name="values">The parameters to add.</param>
        public virtual void AddRange(IEnumerable<SQLiteParameter> values)
        {
            _parameters.AddRange(values);
        }

        /// <summary>
        /// Adds a parameter to the collection.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter. Can be null.</param>
        /// <returns>The parameter that was added.</returns>
        public virtual SQLiteParameter AddWithValue(string parameterName, object value)
        {
            var parameter = new SQLiteParameter(parameterName, value);
            Add(parameter);

            return parameter;
        }

        /// <summary>
        /// Removes all parameters from the collection.
        /// </summary>
        public override void Clear()
        {
            _parameters.Clear();
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified parameter.
        /// </summary>
        /// <param name="value">The parameter to look for. Must be a <see cref="SQLiteParameter" />.</param>
        /// <returns>true if the collection contains the parameter; otherwise, false.</returns>
        public override bool Contains(object value)
        {
            return Contains((SQLiteParameter)value);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified parameter.
        /// </summary>
        /// <param name="value">The parameter to look for.</param>
        /// <returns>true if the collection contains the parameter; otherwise, false.</returns>
        public virtual bool Contains(SQLiteParameter value)
        {
            return _parameters.Contains(value);
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains a parameter with the specified name.
        /// </summary>
        /// <param name="value">The name of the parameter.</param>
        /// <returns>true if the collection contains the parameter; otherwise, false.</returns>
        public override bool Contains(string value)
        {
            return IndexOf(value) != -1;
        }

        /// <summary>
        /// Copies the collection to an array of parameters.
        /// </summary>
        /// <param name="array">
        /// The array into which the parameters are copied. Must be an array of <see cref="SQLiteParameter" /> objects.
        /// </param>
        /// <param name="index">The zero-based index to which the parameters are copied.</param>
        public override void CopyTo(Array array, int index)
        {
            CopyTo((SQLiteParameter[])array, index);
        }

        /// <summary>
        /// Copies the collection to an array of parameters.
        /// </summary>
        /// <param name="array">The array into which the parameters are copied.</param>
        /// <param name="index">The zero-based index to which the parameters are copied.</param>
        public virtual void CopyTo(SQLiteParameter[] array, int index)
        {
            _parameters.CopyTo(array, index);
        }

        /// <summary>
        /// Gets an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public override IEnumerator GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        /// <summary>
        /// Gets a parameter at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter.</param>
        /// <returns>The parameter.</returns>
        protected override DbParameter GetParameter(int index)
        {
            return this[index];
        }

        /// <summary>
        /// Gets a parameter with the specified name.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The parameter.</returns>
        protected override DbParameter GetParameter(string parameterName)
        {
            return GetParameter(IndexOfChecked(parameterName));
        }

        /// <summary>
        /// Gets the index of the specified parameter.
        /// </summary>
        /// <param name="value">The parameter. Must be a <see cref="SQLiteParameter" />.</param>
        /// <returns>The zero-based index of the parameter.</returns>
        public override int IndexOf(object value)
        {
            return IndexOf((SQLiteParameter)value);
        }

        /// <summary>
        /// Gets the index of the specified parameter.
        /// </summary>
        /// <param name="value">The parameter.</param>
        /// <returns>The zero-based index of the parameter.</returns>
        public virtual int IndexOf(SQLiteParameter value)
        {
            return _parameters.IndexOf(value);
        }

        /// <summary>
        /// Gets the index of the parameter with the specified name.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The zero-based index of the parameter or -1 if not found.</returns>
        public override int IndexOf(string parameterName)
        {
            for (var index = 0; index < _parameters.Count; index++)
            {
                if (_parameters[index].ParameterName == parameterName)
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Inserts a parameter into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the parameter should be inserted.</param>
        /// <param name="value">The parameter to insert. Must be a <see cref="SQLiteParameter" />.</param>
        public override void Insert(int index, object value)
        {
            Insert(index, (SQLiteParameter)value);
        }

        /// <summary>
        /// Inserts a parameter into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the parameter should be inserted.</param>
        /// <param name="value">The parameter to insert.</param>
        public virtual void Insert(int index, SQLiteParameter value)
        {
            _parameters.Insert(index, value);
        }

        /// <summary>
        /// Removes a parameter from the collection.
        /// </summary>
        /// <param name="value">The parameter to remove. Must be a <see cref="SQLiteParameter" />.</param>
        public override void Remove(object value)
        {
            Remove((SQLiteParameter)value);
        }

        /// <summary>
        /// Removes a parameter from the collection.
        /// </summary>
        /// <param name="value">The parameter to remove.</param>
        public virtual void Remove(SQLiteParameter value)
        {
            _parameters.Remove(value);
        }

        /// <summary>
        /// Removes a parameter from the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter to remove.</param>
        public override void RemoveAt(int index)
        {
            _parameters.RemoveAt(index);
        }

        /// <summary>
        /// Removes a parameter with the specified name from the collection.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to remove.</param>
        public override void RemoveAt(string parameterName)
        {
            RemoveAt(IndexOfChecked(parameterName));
        }

        /// <summary>
        /// Sets the parameter at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter to set.</param>
        /// <param name="value">The parameter. Must be a <see cref="SQLiteParameter" />.</param>
        protected override void SetParameter(int index, DbParameter value)
        {
            this[index] = (SQLiteParameter)value;
        }

        /// <summary>
        /// Sets the parameter with the specified name.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to set.</param>
        /// <param name="value">The parameter. Must be a <see cref="SQLiteParameter" />.</param>
        protected override void SetParameter(string parameterName, DbParameter value)
        {
            SetParameter(IndexOfChecked(parameterName), value);
        }

        internal int Bind(sqlite3_stmt stmt)
        {
            var bound = 0;
            foreach (var parameter in _parameters)
            {
                if (parameter.Bind(stmt))
                {
                    bound++;
                }
            }
            return bound;
        }

        private int IndexOfChecked(string parameterName)
        {
            var index = IndexOf(parameterName);
            if (index == -1)
            {
                throw new IndexOutOfRangeException(Resources.ParameterNotFound(parameterName));
            }

            return index;
        }
    }
}
