using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace EngMyanDict
{
    internal class Definition
    {
        #region Variables
        private DictionaryItem mItem = null;
        private bool mIsFavorited = false;
        private string mHtml = string.Empty;
        private Image mImage = null;
        #endregion

        #region Constructor
        public Definition()
        { }

        public Definition(DictionaryItem item)
        {
            this.mItem = item;
            this.buildHtml();
        }
        #endregion

        #region Methods
        private void buildHtml()
        {
            DictionaryItem item = this.Item;
            StringBuilder builder = new StringBuilder();
            builder.Append("<html>");
            builder.Append("<head>");
            builder.Append("<meta name=\"viewport\" content=\"initial-scale=1.0, user-scalable=yes, width=device-width\" />");
            builder.Append("<meta content=\"text/html; charset=utf-8\" http-equiv=\"content-type\">");
            builder.AppendFormat("<title>{0}</title>", item.Title);
            builder.Append("</head>");

            builder.Append("<body>");
            builder.Append(item.Definition);

            if (!string.IsNullOrEmpty(item.Synonym))
            {
                builder.Append("<hr />");
                builder.Append("<h3>Synonym</h3>");
                builder.Append("<p class=\"synonym\">");
                builder.Append(item.Synonym);
                builder.Append("</p>");
            }

            builder.Append("</body>");
            builder.Append("</html>");

            this.mHtml = builder.ToString();
        }
        #endregion

        #region Properties
        public DictionaryItem Item
        {
            get { return this.mItem; }
        }

        public bool IsFavorited
        {
            get { return this.mIsFavorited; }
            set { this.mIsFavorited = value; }
        }

        public string Html
        {
            get { return this.mHtml; }
            set { this.mHtml = value; }
        }
        
        public Image Image
        {
            get
            {
                return this.mImage;
            }
            set
            {
                this.mImage = value;
            }
        }
        #endregion
    }
}
