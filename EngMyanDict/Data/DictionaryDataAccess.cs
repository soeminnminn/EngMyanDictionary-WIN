using System;
using System.Text;
using System.IO;
#if ANYCPU
using Microsoft.Data.SQLite;
#else
using System.Data.SQLite;
#endif
using System.Data;
using System.Collections.Generic;

namespace EngMyanDict
{
    internal class DictionaryDataAccess
    {
#region Variables
        private SQLiteConnection m_connection = null;
        private Exception m_exception = null;
#endregion

#region Contructor
        public DictionaryDataAccess(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            if (!File.Exists(filePath))
            {
                SQLiteConnection.CreateFile(filePath);
            }

            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = filePath;

            this.m_connection = new SQLiteConnection(builder.ConnectionString);
        }
#endregion

#region Methods
        public bool CreateTables()
        {
            if (this.m_connection == null) return false;
            this.m_exception = null;

            bool retValue = false;
            try
            {
                StringBuilder stringBuilder = new StringBuilder();

                // Default Tables
                stringBuilder.Append(" CREATE TABLE IF NOT EXISTS \"android_metadata\" (\"locale\" TEXT DEFAULT 'en_US'); ");
                stringBuilder.Append(" INSERT INTO \"android_metadata\" (\"locale\") VALUES (\"en_US\"); ");

                // Custom Tables
                stringBuilder.Append(" CREATE TABLE IF NOT EXISTS \"dictionary\" (\"_id\" INTEGER PRIMARY KEY  NOT NULL  UNIQUE , " +
                    "\"word\" VARCHAR , \"stripword\" VARCHAR , \"title\" TEXT, \"definition\" TEXT, \"keywords\" TEXT, \"synonym\" TEXT, " +
                    "\"filename\" VARCHAR, \"picture\" BOOLEAN, \"sound\" BOOLEAN); ");

                SQLiteCommand command = this.m_connection.CreateCommand();
                command.CommandText = stringBuilder.ToString();

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();

                command.ExecuteNonQuery();
                retValue = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                this.m_exception = ex;
            }
            finally
            {
                if ((this.m_connection != null) && (this.m_connection.State == ConnectionState.Open))
                    this.m_connection.Close();
            }

            return retValue;
        }

        public bool CreateDictionaryIndexs()
        {
            bool result = false;
            if (this.m_connection == null) return result;
            this.m_exception = null;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" CREATE INDEX IF NOT EXISTS \"idx_dictionary_word\" ON \"dictionary\" (\"word\" ASC); ");
                stringBuilder.Append(" CREATE INDEX IF NOT EXISTS \"idx_dictionary_stripword\" ON \"dictionary\" (\"stripword\" ASC); ");

                SQLiteCommand command = this.m_connection.CreateCommand();
                command.CommandText = stringBuilder.ToString();

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();

                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                this.m_exception = ex;
            }
            finally
            {
                if ((this.m_connection != null) && (this.m_connection.State == ConnectionState.Open))
                    this.m_connection.Close();
            }

            return result;
        }

        public bool RunDictionaryIndexs()
        {
            bool result = false;
            if (this.m_connection == null) return result;
            this.m_exception = null;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" REINDEX \"main\".\"idx_dictionary_word\"; ");
                stringBuilder.Append(" REINDEX \"main\".\"idx_dictionary_stripword\"; ");

                SQLiteCommand command = this.m_connection.CreateCommand();
                command.CommandText = stringBuilder.ToString();

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();

                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                this.m_exception = ex;
            }
            finally
            {
                if ((this.m_connection != null) && (this.m_connection.State == ConnectionState.Open))
                    this.m_connection.Close();
            }

            return result;
        }

        public DataTable ReadDictionary()
        {
            DataTable dataTable = null;
            if (this.m_connection == null) return dataTable;
            this.m_exception = null;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" SELECT [_id], [word], [stripword], [title], [definition], ");
                stringBuilder.Append(" [keywords], [synonym], [filename], [picture], [sound] ");
                stringBuilder.Append(" FROM [dictionary] ");

                SQLiteCommand command = this.m_connection.CreateCommand();
                command.CommandText = stringBuilder.ToString();

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();

                SQLiteDataReader dataReader = command.ExecuteReader();

                dataTable = new DataTable("dictionary");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    DataColumn column = new DataColumn(dataReader.GetName(i), dataReader.GetFieldType(i));
                    if (column.ColumnName == "_id")
                    {
                        column.Unique = true;
                        column.AllowDBNull = false;
                        dataTable.Columns.Add(column);
                        dataTable.PrimaryKey = new DataColumn[] { column };
                    }
                    else
                    {
                        dataTable.Columns.Add(column);
                    }
                }
                dataTable.Load(dataReader);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                this.m_exception = ex;
            }
            finally
            {
                if ((this.m_connection != null) && (this.m_connection.State == ConnectionState.Open))
                    this.m_connection.Close();
            }

            return dataTable;
        }

        public List<DictionaryItem> QuerySuggestWord(int limit)
        {
            List<DictionaryItem> dataTable = null;
            if (this.m_connection == null) return dataTable;
            this.m_exception = null;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" SELECT [_id], [word], [stripword] FROM [dictionary] ");
                stringBuilder.Append(" ORDER BY [stripword] ASC ");
                stringBuilder.AppendFormat(" LIMIT {0} ", limit);

                SQLiteCommand command = this.m_connection.CreateCommand();
                command.CommandText = stringBuilder.ToString();

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();

                SQLiteDataReader dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataTable = new List<DictionaryItem>();
                    while (dataReader.Read())
                    {
                        DictionaryItem item = DictionaryItem.FromDataReader(dataReader);
                        if (item != null)
                        {
                            dataTable.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                this.m_exception = ex;
            }
            finally
            {
                if ((this.m_connection != null) && (this.m_connection.State == ConnectionState.Open))
                    this.m_connection.Close();
            }

            return dataTable;
        }

        public List<DictionaryItem> QueryWord(string searchword)
        {
            List<DictionaryItem> dataTable = null;
            if (string.IsNullOrEmpty(searchword)) return dataTable;
            if (this.m_connection == null) return dataTable;
            this.m_exception = null;

            searchword = searchword.Replace("'", "''");
            searchword = searchword.Replace("%", "");
            searchword = searchword.Replace("_", "");
            searchword = searchword.Trim();
            if ((searchword.IndexOf('*') > -1) || (searchword.IndexOf('?') > -1))
            {
                searchword = searchword.Replace('?', '_');
                searchword = searchword.Replace('*', '%');
            }
            else
            {
                searchword = searchword + "%";
            }

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" SELECT [_id], [word], [stripword] FROM [dictionary] ");
                stringBuilder.AppendFormat(" WHERE [stripword] LIKE '{0}' ", searchword);
                stringBuilder.Append(" ORDER BY [stripword] ASC ");

                SQLiteCommand command = this.m_connection.CreateCommand();
                command.CommandText = stringBuilder.ToString();

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();

                SQLiteDataReader dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataTable = new List<DictionaryItem>();
                    while (dataReader.Read())
                    {
                        DictionaryItem item = DictionaryItem.FromDataReader(dataReader);
                        if (item != null)
                        {
                            dataTable.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                this.m_exception = ex;
            }
            finally
            {
                if ((this.m_connection != null) && (this.m_connection.State == ConnectionState.Open))
                    this.m_connection.Close();
            }

            return dataTable;
        }

        public List<DictionaryItem> StripQuery(string searchword)
        {
            List<DictionaryItem> dataTable = null;
            if (string.IsNullOrEmpty(searchword)) return dataTable;
            if (this.m_connection == null) return dataTable;
            this.m_exception = null;

            searchword = searchword.Replace("'", "''");
            searchword = searchword.Replace("%", "");
            searchword = searchword.Replace("_", "");
            searchword = searchword.Trim();

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" SELECT [_id], [word], [stripword] FROM [dictionary] ");
                stringBuilder.AppendFormat(" WHERE [stripword] LIKE '{0}' ", searchword);
                stringBuilder.Append(" ORDER BY [stripword] ASC ");

                SQLiteCommand command = this.m_connection.CreateCommand();
                command.CommandText = stringBuilder.ToString();

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();

                SQLiteDataReader dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataTable = new List<DictionaryItem>();
                    while (dataReader.Read())
                    {
                        DictionaryItem item = DictionaryItem.FromDataReader(dataReader);
                        if (item != null)
                        {
                            dataTable.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                this.m_exception = ex;
            }
            finally
            {
                if ((this.m_connection != null) && (this.m_connection.State == ConnectionState.Open))
                    this.m_connection.Close();
            }

            return dataTable;
        }

        public List<DictionaryItem> ExactQuery(string searchword)
        {
            List<DictionaryItem> dataTable = null;
            if (string.IsNullOrEmpty(searchword)) return dataTable;
            if (this.m_connection == null) return dataTable;
            this.m_exception = null;

            searchword = searchword.Replace("'", "''");
            searchword = searchword.Replace("%", "");
            searchword = searchword.Replace("_", "");
            searchword = searchword.Trim().ToLower();

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" SELECT [_id], [word], [stripword] FROM [dictionary] ");
                stringBuilder.AppendFormat(" WHERE [stripword] IS '{0}' ", searchword);
                stringBuilder.Append(" ORDER BY [stripword] ASC ");

                SQLiteCommand command = this.m_connection.CreateCommand();
                command.CommandText = stringBuilder.ToString();

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();

                SQLiteDataReader dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataTable = new List<DictionaryItem>();
                    while (dataReader.Read())
                    {
                        DictionaryItem item = DictionaryItem.FromDataReader(dataReader);
                        if (item != null)
                        {
                            dataTable.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                this.m_exception = ex;
            }
            finally
            {
                if ((this.m_connection != null) && (this.m_connection.State == ConnectionState.Open))
                    this.m_connection.Close();
            }

            return dataTable;
        }

        public List<DictionaryItem> QueryDefinition(long id)
        {
            List<DictionaryItem> dataTable = null;
            if (id < 1) return dataTable;
            if (this.m_connection == null) return dataTable;
            this.m_exception = null;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" SELECT [_id], [word], [stripword], [title], [definition], ");
                stringBuilder.Append(" [keywords], [synonym], [filename], [picture], [sound] ");
                stringBuilder.Append(" FROM [dictionary] ");
                stringBuilder.AppendFormat(" WHERE [_id] IS {0} ", id);
                stringBuilder.Append(" ORDER BY [stripword] ASC ");
                stringBuilder.Append(" LIMIT 1 ");

                SQLiteCommand command = this.m_connection.CreateCommand();
                command.CommandText = stringBuilder.ToString();

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();

                SQLiteDataReader dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dataTable = new List<DictionaryItem>();
                    while (dataReader.Read())
                    {
                        DictionaryItem item = DictionaryItem.FromDataReader(dataReader);
                        if (item != null)
                        {
                            dataTable.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                this.m_exception = ex;
            }
            finally
            {
                if ((this.m_connection != null) && (this.m_connection.State == ConnectionState.Open))
                    this.m_connection.Close();
            }

            return dataTable;
        }

        public int Save(DataTable table)
        {
            int result = 0;
            if (table == null) return result;
            if (this.m_connection == null) return result;
            this.m_exception = null;

            StringBuilder selectBuilder = new StringBuilder();
            selectBuilder.Append("SELECT ");
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (i > 0) selectBuilder.Append(",");
                string column = string.Format("[{0}]", table.Columns[i].ColumnName);
                selectBuilder.Append(column);
            }
            selectBuilder.AppendFormat("FROM [{0}]", table.TableName);

            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(selectBuilder.ToString(), this.m_connection);
            try
            {
                if (this.m_connection.State == ConnectionState.Closed)
                    this.m_connection.Open();

                SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(dataAdapter);
                dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                dataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();

                result = dataAdapter.Update(table);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                this.m_exception = ex;
            }
            finally
            {
                if ((this.m_connection != null) && (this.m_connection.State == ConnectionState.Open))
                    this.m_connection.Close();
            }

            return result;
        }
#endregion
    }
}
