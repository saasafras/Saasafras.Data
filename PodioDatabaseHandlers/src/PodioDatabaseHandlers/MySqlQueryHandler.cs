using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Common;
using MySql.Data;
using MySql.Data.MySqlClient;
using Amazon.Lambda.Core;
namespace BrickBridge.Lambda.MySql
{
    public class MySqlQueryHandler : IDisposable
    {
        private ILambdaContext _context;
        private string _connStr = System.Environment.GetEnvironmentVariable("PODIO_DB_CONNECTION_STRING");
        private MySqlConnection _conn;

        public MySqlQueryHandler(ILambdaContext context, string connectionString = null)
        {
            _context = context;
            _conn = new MySqlConnection(connectionString ?? _connStr);
            _conn.Open();
        }

        private async Task ExecuteNonQuery(MySqlCommand command)
        {
            try
            {
                _context.Logger.LogLine("Trying to connect to MySQL...");
                _context.Logger.LogLine($"Executing command: {command.CommandText}");
                var affected = await command.ExecuteNonQueryAsync();
                _context.Logger.LogLine($"{affected} rows affected");
            }
            catch (Exception e)
            {
                _conn.Close();
                _context.Logger.LogLine($"Exception in ExecuteNonQuery: {e.ToString()}");
                throw e;
            }
        }

        private async Task<DbDataReader> ExecuteQuery(MySqlCommand command)
        {
            DbDataReader result = null;
            try
            {
                _context.Logger.LogLine("Trying to connect to MySQL...");
                _context.Logger.LogLine($"Executing command: {command.CommandText}");
                result = await command.ExecuteReaderAsync();
                //if (result.HasRows)
                    //_context.Logger.LogLine($"reader returned rows");
            }
            catch (Exception e)
            {
                _conn.Close();
                _context.Logger.LogLine($"Exception in ExecuteQuery: {e.ToString()}");
                throw e;
            }
            return result;
        }

        private async Task<object> ExecuteScalar(MySqlCommand command)
        {
            object result = null;
            try
            {
                _context.Logger.LogLine("Trying to connect to MySQL...");
                _context.Logger.LogLine($"Executing command: {command.CommandText}");
                result = await command.ExecuteScalarAsync();
            }
            catch (Exception e)
            {
                _conn.Close();
                _context.Logger.LogLine($"Exception in ExecuteScalar: {e.ToString()}");
                throw e;
            }
            return result;
        }

        private async Task<UInt64> ExecuteUInt64(MySqlCommand cmd)
        {
            var result = await ExecuteScalar(cmd);
            return (UInt64)result;
        }

        private async Task<Int32> ExecuteInt32(MySqlCommand cmd)
        {
            var result = await ExecuteScalar(cmd);
            return (Int32)result;
        }

        public async Task<Int32> GetPodioAppId(string bbcApp, string version, string spaceName, string appName)
        {
            var cmd = new MySqlCommand(MySqlQueries.SELECT_APP_ID, _conn);
            cmd.Parameters.Add("?bbcAppId", MySqlDbType.VarChar).Value = bbcApp;
            cmd.Parameters.Add("?version", MySqlDbType.VarChar).Value = version;
            cmd.Parameters.Add("?appName", MySqlDbType.VarChar).Value = appName;
            cmd.Parameters.Add("?spaceName", MySqlDbType.VarChar).Value = spaceName;
            var podioAppId = await ExecuteInt32(cmd);
            _context.Logger.LogLine($"PodioAppId = {podioAppId}");
            return podioAppId;
        }

        public async Task<UInt64> InsertPodioItem(Int32 podioAppId, int itemId, int revision, string clientId, string envId)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_ITEM + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?podioAppId", MySqlDbType.Int32).Value = podioAppId;
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?revision", MySqlDbType.Int32).Value = revision;
            cmd.Parameters.Add("?clientId", MySqlDbType.VarChar).Value = clientId;
            cmd.Parameters.Add("?envId", MySqlDbType.VarChar).Value = envId;
            var newItemId = await ExecuteUInt64(cmd);
            _context.Logger.LogLine($"PodioItemId = {newItemId}");
            return newItemId;
        }

        public async Task<List<MySqlPodioField>> SelectAppFields(Int32 podioAppId)
        {
            var cmd = new MySqlCommand(MySqlQueries.SELECT_APP_FIELDS, _conn);

            cmd.Parameters.Add("?podioAppId", MySqlDbType.Int32).Value = podioAppId;
            var reader = await ExecuteQuery(cmd);
            var result = new List<MySqlPodioField>();
            _context.Logger.LogLine($"Reader has rows: {reader.HasRows}");
            while (reader.Read())
            {
                result.Add(new MySqlPodioField
                {
                    PodioFieldId = reader.GetInt32(0),
                    ExternalId = reader.GetString(1),
                    Type = reader.GetString(2)
                });
            }
            reader.Close();
            _context.Logger.LogLine($"App has {result.Count} fields.");
            return result;
        }

        public async Task<UInt64> InsertCategoryField(ulong itemId, int fieldId, string optionText, int optionId)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_CATEGORY_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?optionText", MySqlDbType.VarChar).Value = optionText;
            cmd.Parameters.Add("?optionId", MySqlDbType.Int32).Value = optionId;
            var fieldDataId = await ExecuteUInt64(cmd);
            return fieldDataId;
        }

        public async Task<UInt64> InsertContactField(ulong itemId, int fieldId, int contactId)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_CONTACT_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?contactId", MySqlDbType.Int32).Value = contactId;
            var fieldDataId = await ExecuteUInt64(cmd);
            return fieldDataId;
        }

        public async Task<UInt64> InsertDateField(ulong itemId, int fieldId, DateTime? start, DateTime? end)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_DATE_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?start", MySqlDbType.DateTime).Value = start;
            cmd.Parameters.Add("?end", MySqlDbType.DateTime).Value = end;
            var fieldDataId = await ExecuteUInt64(cmd);
            return fieldDataId;
        }

        public async Task<UInt64> InsertDurationField(ulong itemId, int fieldId, int duration)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_DURATION_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?duration", MySqlDbType.Int32).Value = duration;
            var fieldDataId = await ExecuteUInt64(cmd);
            return fieldDataId;
        }

        public async Task<UInt64> InsertLocationField(ulong itemId, int fieldId, string location)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_LOCATION_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?location", MySqlDbType.VarChar).Value = location;
            var fieldDataId = await ExecuteUInt64(cmd);
            return fieldDataId;
        }

        public async Task<UInt64> InsertMemberField(ulong itemId, int fieldId, int memberId)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_MEMBER_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?memberId", MySqlDbType.Int32).Value = memberId;
            var fieldDataId = await ExecuteUInt64(cmd);
            return fieldDataId;
        }

        public async Task<UInt64> InsertMoneyField(ulong itemId, int fieldId, decimal amount, string currency)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_MONEY_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?amount", MySqlDbType.Decimal).Value = amount;
            cmd.Parameters.Add("?currency", MySqlDbType.VarChar).Value = currency;
            var fieldDataId = await ExecuteUInt64(cmd);
            return fieldDataId;
        }

        public async Task<UInt64> InsertNumberField(ulong itemId, int fieldId, double value)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_NUMBER_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?value", MySqlDbType.Double).Value = value;
            var fieldDataId = await ExecuteUInt64(cmd);
            return fieldDataId;
        }

        public async Task<UInt64> InsertPhoneField(ulong itemId, int fieldId, string type, string value)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_PHONEEMAIL_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?type", MySqlDbType.VarChar).Value = type;
            cmd.Parameters.Add("?value", MySqlDbType.VarChar).Value = value;
            cmd.Parameters.Add("?phoneOrEmail", MySqlDbType.VarChar).Value = "P";
            var fieldDataId = await ExecuteUInt64(cmd);
            return fieldDataId;
        }

        public async Task<UInt64> InsertEmailField(ulong itemId, int fieldId, string type, string value)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_PHONEEMAIL_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?type", MySqlDbType.VarChar).Value = type;
            cmd.Parameters.Add("?value", MySqlDbType.VarChar).Value = value;
            cmd.Parameters.Add("?phoneOrEmail", MySqlDbType.VarChar).Value = "E";
            var fieldDataId = await ExecuteUInt64(cmd);
            return fieldDataId;
        }

        public async Task<UInt64> InsertProgressField(ulong itemId, int fieldId, int progress)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_PROGRESS_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?progress", MySqlDbType.Int32).Value = progress;
            var fieldDataId = await ExecuteUInt64(cmd);
            return fieldDataId;
        }

        public async Task<UInt64> InsertRelationField(ulong itemId, int fieldId, int refId)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_RELATION_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?ref", MySqlDbType.Int32).Value = refId;
            var fieldDataId = await ExecuteUInt64(cmd);
            return fieldDataId;
        }

        public async Task<UInt64> InsertTextField(ulong itemId, int fieldId, string text)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_TEXT_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?text", MySqlDbType.VarChar).Value = text;
            var fieldDataId = await ExecuteUInt64(cmd);
            return fieldDataId;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _conn.Close();
                    _conn = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MySqlQueryHandler() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
