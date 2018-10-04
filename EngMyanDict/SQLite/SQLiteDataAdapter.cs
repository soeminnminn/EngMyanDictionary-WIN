// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel;
using System.Data.Common;
using System.Text;

namespace Microsoft.Data.SQLite
{
    /// <summary>
    /// SQLite implementation of DbDataAdapter.
    /// </summary>
    [DefaultEvent("RowUpdated")]
    [Designer("Microsoft.VSDesigner.Data.VS.SqlDataAdapterDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public class SQLiteDataAdapter : DbDataAdapter
    {
        private bool disposeSelect = true;

        private static object _updatingEventPH = new object();
        private static object _updatedEventPH = new object();

        #region Public Constructors
        /// <overloads>
        /// This class is just a shell around the DbDataAdapter.  Nothing from
        /// DbDataAdapter is overridden here, just a few constructors are defined.
        /// </overloads>
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SQLiteDataAdapter()
        {
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Constructs a data adapter using the specified select command.
        /// </summary>
        /// <param name="cmd">
        /// The select command to associate with the adapter.
        /// </param>
        public SQLiteDataAdapter(SQLiteCommand cmd)
        {
            SelectCommand = cmd;
            disposeSelect = false;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Constructs a data adapter with the supplied select command text and
        /// associated with the specified connection.
        /// </summary>
        /// <param name="commandText">
        /// The select command text to associate with the data adapter.
        /// </param>
        /// <param name="connection">
        /// The connection to associate with the select command.
        /// </param>
        public SQLiteDataAdapter(string commandText, SQLiteConnection connection)
        {
            SelectCommand = new SQLiteCommand(commandText, connection);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Constructs a data adapter with the specified select command text,
        /// and using the specified database connection string.
        /// </summary>
        /// <param name="commandText">
        /// The select command text to use to construct a select command.
        /// </param>
        /// <param name="connectionString">
        /// A connection string suitable for passing to a new SQLiteConnection,
        /// which is associated with the select command.
        /// </param>
        public SQLiteDataAdapter(
            string commandText,
            string connectionString
            )
        {
            SQLiteConnection cnn = new SQLiteConnection(connectionString);

            SelectCommand = new SQLiteCommand(commandText, cnn);
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Cleans up resources (native and managed) associated with the current instance.
        /// </summary>
        /// <param name="disposing">
        /// Zero when being disposed via garbage collection; otherwise, non-zero.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            try
            {
                ////////////////////////////////////
                // dispose managed resources here...
                ////////////////////////////////////

                if (disposeSelect && (SelectCommand != null))
                {
                    SelectCommand.Dispose();
                    SelectCommand = null;
                }

                if (InsertCommand != null)
                {
                    InsertCommand.Dispose();
                    InsertCommand = null;
                }

                if (UpdateCommand != null)
                {
                    UpdateCommand.Dispose();
                    UpdateCommand = null;
                }

                if (DeleteCommand != null)
                {
                    DeleteCommand.Dispose();
                    DeleteCommand = null;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Row updating event handler
        /// </summary>
        public event EventHandler<RowUpdatingEventArgs> RowUpdating
        {
            add
            {
                EventHandler<RowUpdatingEventArgs> previous = (EventHandler<RowUpdatingEventArgs>)base.Events[_updatingEventPH];
                if ((previous != null) && (value.Target is DbCommandBuilder))
                {
                    EventHandler<RowUpdatingEventArgs> handler = (EventHandler<RowUpdatingEventArgs>)FindBuilder(previous);
                    if (handler != null)
                    {
                        base.Events.RemoveHandler(_updatingEventPH, handler);
                    }
                }
                base.Events.AddHandler(_updatingEventPH, value);
            }
            remove { base.Events.RemoveHandler(_updatingEventPH, value); }
        }

        internal static Delegate FindBuilder(MulticastDelegate mcd)
        {
            if (mcd != null)
            {
                Delegate[] invocationList = mcd.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    if (invocationList[i].Target is DbCommandBuilder)
                    {
                        return invocationList[i];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Row updated event handler
        /// </summary>
        public event EventHandler<RowUpdatedEventArgs> RowUpdated
        {
            add { base.Events.AddHandler(_updatedEventPH, value); }
            remove { base.Events.RemoveHandler(_updatedEventPH, value); }
        }

        /// <summary>
        /// Raised by the underlying DbDataAdapter when a row is being updated
        /// </summary>
        /// <param name="value">The event's specifics</param>
        protected override void OnRowUpdating(RowUpdatingEventArgs value)
        {
            EventHandler<RowUpdatingEventArgs> handler = base.Events[_updatingEventPH] as EventHandler<RowUpdatingEventArgs>;

            if (handler != null)
                handler(this, value);
        }

        /// <summary>
        /// Raised by DbDataAdapter after a row is updated
        /// </summary>
        /// <param name="value">The event's specifics</param>
        protected override void OnRowUpdated(RowUpdatedEventArgs value)
        {
            EventHandler<RowUpdatedEventArgs> handler = base.Events[_updatedEventPH] as EventHandler<RowUpdatedEventArgs>;

            if (handler != null)
                handler(this, value);
        }

        /// <summary>
        /// Gets/sets the select command for this DataAdapter
        /// </summary>
        [DefaultValue((string)null), Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public new SQLiteCommand SelectCommand
        {
            get { return (SQLiteCommand)base.SelectCommand; }
            set { base.SelectCommand = value; }
        }

        /// <summary>
        /// Gets/sets the insert command for this DataAdapter
        /// </summary>
        [DefaultValue((string)null), Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public new SQLiteCommand InsertCommand
        {
            get { return (SQLiteCommand)base.InsertCommand; }
            set { base.InsertCommand = value; }
        }

        /// <summary>
        /// Gets/sets the update command for this DataAdapter
        /// </summary>
        [DefaultValue((string)null), Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public new SQLiteCommand UpdateCommand
        {
            get { return (SQLiteCommand)base.UpdateCommand; }
            set { base.UpdateCommand = value; }
        }

        /// <summary>
        /// Gets/sets the delete command for this DataAdapter
        /// </summary>
        [DefaultValue((string)null), Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public new SQLiteCommand DeleteCommand
        {
            get { return (SQLiteCommand)base.DeleteCommand; }
            set { base.DeleteCommand = value; }
        }
    }
}
