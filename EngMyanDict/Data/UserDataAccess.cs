using System;
using System.Data;
using System.IO;
using System.Text;
#if ANYCPU
using Microsoft.Data.SQLite;
#else
using System.Data.SQLite;
#endif

using System.Collections.Generic;

namespace EngMyanDict
{
    internal class UserDataAccess
    {
        #region Variables
        private SQLiteConnection m_connection = null;
        private Exception m_exception = null;
        #endregion

        #region Constructor
        public UserDataAccess(string filePath)
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
                stringBuilder.Append(" CREATE TABLE IF NOT EXISTS \"histories\" (\"_id\" INTEGER PRIMARY KEY  NOT NULL  UNIQUE , " +
                    "\"word\" VARCHAR , \"refrence_id\" INTEGER , \"timestamp\" INTEGER); ");

                stringBuilder.Append(" CREATE TABLE IF NOT EXISTS \"favorites\" (\"_id\" INTEGER PRIMARY KEY  NOT NULL  UNIQUE , " +
                    "\"word\" VARCHAR , \"refrence_id\" INTEGER , \"timestamp\" INTEGER); ");

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

        public bool CreateTriggers()
        {
            if (this.m_connection == null) return false;
            this.m_exception = null;

            bool retValue = false;
            try
            {
                StringBuilder stringBuilder = new StringBuilder();

                // Triggers
                stringBuilder.Append(" CREATE TRIGGER IF NOT EXISTS \"limit_histories\" AFTER INSERT ON \"histories\"" +
                    " FOR EACH ROW" +
                    " WHEN (SELECT COUNT(\"_id\") FROM \"histories\") > 100" +
                    " BEGIN " +
                    " DELETE FROM \"histories\"" +
                    " WHERE \"timestamp\" IS (SELECT MIN(\"timestamp\") FROM \"histories\");" +
                    " END; ");

                stringBuilder.Append(" CREATE TRIGGER IF NOT EXISTS \"limit_favorites\" AFTER INSERT ON \"favorites\"" +
                    " FOR EACH ROW" +
                    " WHEN (SELECT COUNT(\"_id\") FROM \"favorites\") > 100" +
                    " BEGIN " +
                    " DELETE FROM \"favorites\"" +
                    " WHERE \"timestamp\" IS (SELECT MIN(\"timestamp\") FROM \"favorites\");" +
                    " END; ");

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

        public bool DropTriggers()
        {
            if (this.m_connection == null) return false;
            this.m_exception = null;

            bool retValue = false;
            try
            {
                StringBuilder stringBuilder = new StringBuilder();

                // Drop Triggers
                stringBuilder.Append(" DROP TRIGGER IF EXISTS \"limit_histories\"; ");
                stringBuilder.Append(" DROP TRIGGER IF EXISTS \"limit_favorites\"; ");

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
        
        public bool IsFavorited(long refId)
        {
            bool retValue = false;
            if (refId < 1) return retValue;
            if (this.m_connection == null) return retValue;
            this.m_exception = null;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" SELECT COUNT([_id]) FROM [favorites] ");
                stringBuilder.AppendFormat(" WHERE [refrence_id] IS {0}", refId);

                SQLiteCommand command = this.m_connection.CreateCommand();
                command.CommandText = stringBuilder.ToString();

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();

                object result = command.ExecuteScalar();
                retValue = result != null && (long)result > 0;
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

        public List<DictionaryItem> GetAllFavorites()
        {
            List<DictionaryItem> dataTable = null;
            if (this.m_connection == null) return dataTable;
            this.m_exception = null;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" SELECT [_id], [word], [refrence_id], [timestamp] FROM [favorites] ");

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

        public int CreateFavorite(string word, long refId)
        {
            int retValue = 0;
            if (refId < 1) return retValue;
            if (string.IsNullOrEmpty(word)) return retValue;
            if (this.m_connection == null) return retValue;
            this.m_exception = null;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" INSERT INTO [favorites] ([word], [refrence_id], [timestamp]) ");
                stringBuilder.Append(" VALUES (@word, @refrence_id, @timestamp) ");

                SQLiteCommand command = this.m_connection.CreateCommand();
                command.CommandText = stringBuilder.ToString();
                command.Parameters.AddWithValue("@word", word);
                command.Parameters.AddWithValue("@refrence_id", refId);
                command.Parameters.AddWithValue("@timestamp", DateTime.Now.Ticks);

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();

                retValue = command.ExecuteNonQuery();
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

        public int RemoveFavorite(params long[] ids)
        {
            int retValue = 0;
            if (ids == null || ids.Length < 1) return retValue;
            if (this.m_connection == null) return retValue;
            this.m_exception = null;

            SQLiteTransaction transaction = null;

            try
            {
                string[] queries = new string[ids.Length];
                for (int i = 0; i < ids.Length; i++)
                {
                    long id = ids[i];
                    queries[i] = string.Format(" DELETE FROM [favorites] WHERE [_id] = {0} ", id);
                }

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();


                transaction = this.m_connection.BeginTransaction();
                for (int i = 0; i < ids.Length; i++)
                {
                    SQLiteCommand command = this.m_connection.CreateCommand();
                    command.CommandText = queries[i];
                    retValue += command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();

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

        public int RemoveFavoriteByRef(params long[] ids)
        {
            int retValue = 0;
            if (ids == null || ids.Length < 1) return retValue;
            if (this.m_connection == null) return retValue;
            this.m_exception = null;

            SQLiteTransaction transaction = null;

            try
            {
                string[] queries = new string[ids.Length];
                for(int i = 0; i < ids.Length; i++)
                {
                    long id = ids[i];
                    queries[i] = string.Format(" DELETE FROM [favorites] WHERE [refrence_id] = {0} ", id);
                }

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();


                transaction = this.m_connection.BeginTransaction();
                for (int i = 0; i < ids.Length; i++)
                {
                    SQLiteCommand command = this.m_connection.CreateCommand();
                    command.CommandText = queries[i];
                    retValue += command.ExecuteNonQuery();
                }
                transaction.Commit();

            }
            catch (Exception ex)
            {
                if (transaction != null)
                    transaction.Rollback();

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

        public List<DictionaryItem> GetAllHistories()
        {
            List<DictionaryItem> dataTable = null;
            if (this.m_connection == null) return dataTable;
            this.m_exception = null;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" SELECT [_id], [word], [refrence_id], [timestamp] FROM [histories] ");

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

        public int CreateHistory(string word, long refId)
        {
            int retValue = 0;
            if (refId < 1) return retValue;
            if (string.IsNullOrEmpty(word)) return retValue;
            if (this.m_connection == null) return retValue;
            this.m_exception = null;

            try
            {
                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append(" SELECT COUNT([_id]) FROM [histories] ");
                stringBuilder.AppendFormat(" WHERE [refrence_id] = {0} ", refId);

                SQLiteCommand selectCommand = this.m_connection.CreateCommand();
                selectCommand.CommandText = stringBuilder.ToString();
                object selectResult = selectCommand.ExecuteScalar();
                if (selectResult != null && (long)selectResult == 0)
                {
                    stringBuilder.Clear();
                    stringBuilder.Append(" INSERT INTO [histories] ([word], [refrence_id], [timestamp]) ");
                    stringBuilder.Append(" VALUES (@word, @refrence_id, @timestamp) ");

                    SQLiteCommand insertCommand = this.m_connection.CreateCommand();
                    insertCommand.CommandText = stringBuilder.ToString();
                    insertCommand.Parameters.AddWithValue("@word", word);
                    insertCommand.Parameters.AddWithValue("@refrence_id", refId);
                    insertCommand.Parameters.AddWithValue("@timestamp", DateTime.Now.Ticks);

                    retValue = insertCommand.ExecuteNonQuery();
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

            return retValue;
        }

        public int RemoveAllHistory()
        {
            int retValue = 0;
            if (this.m_connection == null) return retValue;
            this.m_exception = null;

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(" DELETE FROM [histories] ");

                SQLiteCommand command = this.m_connection.CreateCommand();
                command.CommandText = stringBuilder.ToString();

                if (this.m_connection.State != ConnectionState.Open)
                    this.m_connection.Open();

                retValue = command.ExecuteNonQuery();
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
        #endregion
    }
}
