using System;
using System.Collections.Generic;
using System.Text;

namespace EngMyanDict
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        #region Variables
        private string mName = null;
        #endregion

        #region Constructor
        public ColumnNameAttribute(string name)
        {
            this.mName = name;
        }
        #endregion

        #region Properties
        public string Name
        {
            get { return this.mName; }
            set { this.mName = value; }
        }
        #endregion

        #region Methods
        #endregion
    }
}
