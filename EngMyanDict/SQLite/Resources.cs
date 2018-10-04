using System;
using System.Reflection;
using System.Resources;

namespace SQLite.Properties
{
    internal static class Resources
    {
        private static readonly ResourceManager _resourceManager;

        static Resources()
        {
            _resourceManager = EngMyanDict.SQLite.SR.ResourceManager;
        }

        /// <summary>
        /// {methodName} can only be called when the connection is open.
        /// </summary>
        public static string CallRequiresOpenConnection(object methodName)
        {
            return string.Format(
                GetString("CallRequiresOpenConnection", nameof(methodName)),
                methodName);
        }

        /// <summary>
        /// CommandText must be set before {methodName} can be called.
        /// </summary>
        public static string CallRequiresSetCommandText(object methodName)
        {
            return string.Format(
                GetString("CallRequiresSetCommandText", nameof(methodName)),
                methodName);
        }

        /// <summary>
        /// ConnectionString cannot be set when the connection is open.
        /// </summary>
        public static string ConnectionStringRequiresClosedConnection
        {
            get { return GetString("ConnectionStringRequiresClosedConnection"); }
        }

        /// <summary>
        /// Invalid attempt to call {operation} when reader is closed.
        /// </summary>
        public static string DataReaderClosed(object operation)
        {
            return string.Format(
                GetString("DataReaderClosed", nameof(operation)),
                operation);
        }

        /// <summary>
        /// The cache mode '{mode}' is invalid.
        /// </summary>
        public static string InvalidCacheMode(object mode)
        {
            return string.Format(
                GetString("InvalidCacheMode", nameof(mode)),
                mode);
        }

        /// <summary>
        /// The CommandBehavior '{behavior}' is invalid.
        /// </summary>
        public static string InvalidCommandBehavior(object behavior)
        {
            return string.Format(
                GetString("InvalidCommandBehavior", nameof(behavior)),
                behavior);
        }

        /// <summary>
        /// The CommandType '{commandType}' is invalid.
        /// </summary>
        public static string InvalidCommandType(object commandType)
        {
            return string.Format(
                GetString("InvalidCommandType", nameof(commandType)),
                commandType);
        }

        /// <summary>
        /// The IsolationLevel '{isolationLevel}' is invalid.
        /// </summary>
        public static string InvalidIsolationLevel(object isolationLevel)
        {
            return string.Format(
                GetString("InvalidIsolationLevel", nameof(isolationLevel)),
                isolationLevel);
        }

        /// <summary>
        /// The ParameterDirection '{direction}' is invalid.
        /// </summary>
        public static string InvalidParameterDirection(object direction)
        {
            return string.Format(
                GetString("InvalidParameterDirection", nameof(direction)),
                direction);
        }

        /// <summary>
        /// Keyword not supported: '{keyword}'.
        /// </summary>
        public static string KeywordNotSupported(object keyword)
        {
            return string.Format(
                GetString("KeywordNotSupported", nameof(keyword)),
                keyword);
        }

        /// <summary>
        /// Must add values for the following parameters: {parameters}
        /// </summary>
        public static string MissingParameters(object parameters)
        {
            return string.Format(
                GetString("MissingParameters", nameof(parameters)),
                parameters);
        }

        /// <summary>
        /// No data exists for the row/column.
        /// </summary>
        public static string NoData
        {
            get { return GetString("NoData"); }
        }

        /// <summary>
        /// ConnectionString must be set before Open can be called.
        /// </summary>
        public static string OpenRequiresSetConnectionString
        {
            get { return GetString("OpenRequiresSetConnectionString"); }
        }

        /// <summary>
        /// SqliteConnection does not support nested transactions.
        /// </summary>
        public static string ParallelTransactionsNotSupported
        {
            get { return GetString("ParallelTransactionsNotSupported"); }
        }

        /// <summary>
        /// A SqliteParameter with ParameterName '{parameterName}' is not contained by this SqliteParameterCollection.
        /// </summary>
        public static string ParameterNotFound(object parameterName)
        {
            return string.Format(
                GetString("ParameterNotFound", nameof(parameterName)),
                parameterName);
        }

        /// <summary>
        /// {propertyName} must be set.
        /// </summary>
        public static string RequiresSet(object propertyName)
        {
            return string.Format(
                GetString("RequiresSet", nameof(propertyName)),
                propertyName);
        }

        /// <summary>
        /// This SqliteTransaction has completed; it is no longer usable.
        /// </summary>
        public static string TransactionCompleted
        {
            get { return GetString("TransactionCompleted"); }
        }

        /// <summary>
        /// The transaction object is not associated with the connection object.
        /// </summary>
        public static string TransactionConnectionMismatch
        {
            get { return GetString("TransactionConnectionMismatch"); }
        }

        /// <summary>
        /// Execute requires the command to have a transaction object when the connection assigned to the command is in a pending local transaction.  The Transaction property of the command has not been initialized.
        /// </summary>
        public static string TransactionRequired
        {
            get { return GetString("TransactionRequired"); }
        }

        /// <summary>
        /// No mapping exists from object type {typeName} to a known managed provider native type.
        /// </summary>
        public static string UnknownDataType(object typeName)
        {
            return string.Format(
                GetString("UnknownDataType", nameof(typeName)),
                typeName);
        }

        /// <summary>
        /// SQLite Error {errorCode}: '{message}'.
        /// </summary>
        public static string SqliteNativeError(object errorCode, object message)
        {
            return string.Format(
                GetString("SqliteNativeError", nameof(errorCode), nameof(message)),
                errorCode, message);
        }

        /// <summary>
        /// For more information on this error code see http://sqlite.org/rescode.html
        /// </summary>
        public static string DefaultNativeError
        {
            get { return GetString("DefaultNativeError"); }
        }

        /// <summary>
        /// Cannot bind the value for parameter '{parameterName}' because multiple matching parameters were found in the command text. Specify the parameter name with the symbol prefix, e.g. '@{parameterName}'.
        /// </summary>
        public static string AmbiguousParameterName(object parameterName)
        {
            return string.Format(
                GetString("AmbiguousParameterName", nameof(parameterName)),
                parameterName);
        }

        /// <summary>
        /// The SQLite library is already loaded. UseWinSqlite3 must be called before using SQLite.
        /// </summary>
        public static string AlreadyLoaded
        {
            get { return GetString("AlreadyLoaded"); }
        }

        /// <summary>
        /// The {enumType} enumeration value, {value}, is invalid.
        /// </summary>
        public static string InvalidEnumValue(object enumType, object value)
        {
            return string.Format(
                GetString("InvalidEnumValue", nameof(enumType), nameof(value)),
                enumType, value);
        }

        /// <summary>
        /// Cannot convert object of type '{sourceType}' to object of type '{targetType}'.
        /// </summary>
        public static string ConvertFailed(object sourceType, object targetType)
        {
            return string.Format(
                GetString("ConvertFailed", nameof(sourceType), nameof(targetType)),
                sourceType, targetType);
        }

        /// <summary>
        /// Cannot store 'NaN' values.
        /// </summary>
        public static string CannotStoreNaN
        {
            get { return GetString("CannotStoreNaN"); }
        }

        /// <summary>
        /// An open reader is already associated with this command. Close it before opening a new one.
        /// </summary>
        public static string DataReaderOpen
        {
            get { return GetString("DataReaderOpen"); }
        }

        /// <summary>
        /// An open reader is associated with this command. Close it before changing the {propertyName} property.
        /// </summary>
        public static string SetRequiresNoOpenReader(object propertyName)
        {
            return string.Format(
                GetString("SetRequiresNoOpenReader", nameof(propertyName)),
                propertyName);
        }

        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name);
            for (var i = 0; i < formatterNames.Length; i++)
            {
                value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
            }

            return value;
        }
    }
}
