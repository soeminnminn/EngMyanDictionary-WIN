// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Data.Common;

namespace Microsoft.Data.SQLite
{
    /// <summary>
    /// Creates instances of various Microsoft.Data.SQLite classes.
    /// </summary>
    public class SQLiteFactory : DbProviderFactory
    {
        private SQLiteFactory()
        {
        }

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public readonly static SQLiteFactory Instance = new SQLiteFactory();

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <returns>The new command.</returns>
        public override DbCommand CreateCommand()
        {
            return new SQLiteCommand();
        }

        /// <summary>
        /// Creates a new connection.
        /// </summary>
        /// <returns>The new connection.</returns>
        public override DbConnection CreateConnection()
        {
            return new SQLiteConnection();
        }

        /// <summary>
        /// Creates a new connection string builder.
        /// </summary>
        /// <returns>The new connection string builder.</returns>
        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new SQLiteConnectionStringBuilder();
        }

        /// <summary>
        /// Creates a new parameter.
        /// </summary>
        /// <returns>The new parameter.</returns>
        public override DbParameter CreateParameter()
        {
            return new SQLiteParameter();
        }
    }
}
