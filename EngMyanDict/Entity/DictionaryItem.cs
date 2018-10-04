using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace EngMyanDict
{
    internal class DictionaryItem
    {
        #region Variables
        private long mId = 0;
        private long mRefrenceId = 0;
        private string mWord = null;
        private string mStripWord = null;
        private string mTitle = null;
        private string mDefinition = null;
        private string mKeywords = null;
        private string mSynonym = null;
        private string mFileName = null;
        private bool mPicture = false;
        private bool mSound = false;
        #endregion

        #region Constructor
        public DictionaryItem()
        { }
        #endregion

        #region Properties
        [ColumnName("_id")]
        public long Id
        {
            get { return this.mId; }
            set { this.mId = value; }
        }

        [ColumnName("refrence_id")]
        public long RefrenceId
        {
            get { return this.mRefrenceId; }
            set { this.mRefrenceId = value; }
        }

        [ColumnName("word")]
        public string Word
        {
            get { return this.mWord; }
            set { this.mWord = value; }
        }

        [ColumnName("stripword")]
        public string StripWord
        {
            get { return this.mStripWord; }
            set { this.mStripWord = value; }
        }

        [ColumnName("title")]
        public string Title
        {
            get { return this.mTitle; }
            set { this.mTitle = value; }
        }

        [ColumnName("definition")]
        public string Definition
        {
            get { return this.mDefinition; }
            set { this.mDefinition = value; }
        }

        [ColumnName("keywords")]
        public string Keywords
        {
            get { return this.mKeywords; }
            set { this.mKeywords = value; }
        }

        [ColumnName("synonym")]
        public string Synonym
        {
            get { return this.mSynonym; }
            set { this.mSynonym = value; }
        }

        [ColumnName("filename")]
        public string FileName
        {
            get { return this.mFileName; }
            set { this.mFileName = value; }
        }

        [ColumnName("picture")]
        public bool Picture
        {
            get { return this.mPicture; }
            set { this.mPicture = value; }
        }

        [ColumnName("sound")]
        public bool Sound
        {
            get { return this.mSound; }
            set { this.mSound = value; }
        }
        #endregion

        #region Methods
        public static DictionaryItem FromDataRow(DataRow row)
        {
            if (row != null)
            {
                DictionaryItem item = new DictionaryItem();

                PropertyInfo[] properties = item.GetType().GetProperties();
                foreach(PropertyInfo property in properties)
                {
                    object[] attribs = property.GetCustomAttributes(typeof(ColumnNameAttribute), false);
                    if (attribs != null && attribs.Length > 0)
                    {
                        ColumnNameAttribute columnName = attribs[0] as ColumnNameAttribute;
                        int colIdx = row.Table.Columns.IndexOf(columnName.Name);
                        if (colIdx != -1 && !row.IsNull(colIdx))
                        {
                            object value = row.ItemArray[colIdx];
                            try
                            {
                                property.SetValue(item, value, null);
                            }
                            catch (ArgumentException ex)
                            {
                                if (typeof(bool) == property.PropertyType)
                                {
                                    string str = value == null ? "false" : value.ToString().ToLower();
                                    property.SetValue(item, (str != "0" && str != "false"), null);
                                }
                                else if (typeof(string) == property.PropertyType)
                                {
                                    string str = value == null ? null : value.ToString();
                                    property.SetValue(item, str, null);
                                }
                                else
                                {
                                    System.Diagnostics.Trace.WriteLine(ex.Message);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Trace.WriteLine(ex.Message);
                            }
                        }
                    }
                }

                return item;
            }
            return null;
        }

        private static Dictionary<DbDataReader, string[]> sColumnsCache = new Dictionary<DbDataReader, string[]>();
        public static DictionaryItem FromDataReader(DbDataReader reader)
        {
            if (reader != null)
            {
                DictionaryItem item = new DictionaryItem();
                string[] columns = null;
                if (sColumnsCache.ContainsKey(reader))
                {
                    columns = sColumnsCache[reader];
                }
                else
                {
                    columns = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        columns[i] = reader.GetName(i);
                    }
                    sColumnsCache.Add(reader, columns);
                }

                PropertyInfo[] properties = item.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    object[] attribs = property.GetCustomAttributes(typeof(ColumnNameAttribute), false);
                    if (attribs != null && attribs.Length > 0)
                    {
                        ColumnNameAttribute columnName = attribs[0] as ColumnNameAttribute;
                        int colIdx = Array.IndexOf(columns, columnName.Name);
                        if (colIdx != -1 && !reader.IsDBNull(colIdx))
                        {
                            object value = reader.GetValue(colIdx);
                            try
                            {
                                property.SetValue(item, value, null);
                            }
                            catch (ArgumentException ex)
                            {
                                if (typeof(bool) == property.PropertyType)
                                {
                                    string str = value == null ? "false" : value.ToString().ToLower();
                                    property.SetValue(item, (str != "0" && str != "false"), null);
                                }
                                else if (typeof(string) == property.PropertyType)
                                {
                                    string str = value == null ? null : value.ToString();
                                    property.SetValue(item, str, null);
                                }
                                else
                                {
                                    System.Diagnostics.Trace.WriteLine(ex.Message);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Trace.WriteLine(ex.Message);
                            }
                            
                        }
                    }
                }
                return item;
            }
            return null;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(this.mWord)) return this.mWord;
            return base.ToString();
        }
        #endregion
    }
}
