using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Common;
using MySql.Data;
using MySql.Data.MySqlClient;
using Amazon.Lambda.Core;
namespace BrickBridge.Lambda.MySql
{
    public class MySqlQueryHandler
    {
        private ILambdaContext _context;
        private string _connStr = System.Environment.GetEnvironmentVariable("PODIO_DB_CONNECTION_STRING");
        private MySqlConnection _conn;

        public MySqlQueryHandler(ILambdaContext context, string connectionString = null)
        {
            _context = context;
            _conn = new MySqlConnection(connectionString ?? _connStr);
        }

        private async Task ExecuteNonQuery(MySqlCommand command)
        {
            try
            {
                _context.Logger.LogLine("Trying to connect to MySQL...");
                _conn.Open();
                _context.Logger.LogLine($"Executing command: {command.CommandText}");
                var affected = await command.ExecuteNonQueryAsync();
                _context.Logger.LogLine($"{affected} rows affected");
            }
            catch (Exception e)
            {
                _context.Logger.LogLine(e.ToString());
            }
            finally
            {
                await _conn.CloseAsync();
            }
        }

        private async Task<DbDataReader> ExecuteQuery(MySqlCommand command)
        {
            DbDataReader result = null;
            try
            {
                _context.Logger.LogLine("Trying to connect to MySQL...");
                _conn.Open();
                _context.Logger.LogLine($"Executing command: {command.CommandText}");
                var reader = await command.ExecuteReaderAsync();
            }
            catch (Exception e)
            {
                _context.Logger.LogLine(e.ToString());
            }
            finally
            {
                await _conn.CloseAsync();
            }

            return result;
        }

        private async Task<T> ExecuteScalar<T>(MySqlCommand command)
        {
            object result = null;
            try
            {
                _context.Logger.LogLine("Trying to connect to MySQL...");
                _conn.Open();
                _context.Logger.LogLine($"Executing command: {command.CommandText}");
                result = await command.ExecuteScalarAsync();
            }
            catch (Exception e)
            {
                _context.Logger.LogLine(e.ToString());
            }
            finally
            {
                await _conn.CloseAsync();
            }

            return (T)result;
        }

        public async Task<int> GetPodioAppId(string bbcApp, string version, string spaceName, string appName)
        {
            var cmd = new MySqlCommand(MySqlQueries.SELECT_APP_ID + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?bbcAppId", MySqlDbType.VarChar).Value = bbcApp;
            cmd.Parameters.Add("?version", MySqlDbType.VarChar).Value = version;
            cmd.Parameters.Add("?appName", MySqlDbType.VarChar).Value = appName;
            cmd.Parameters.Add("?spaceName", MySqlDbType.VarChar).Value = spaceName;
            var podioAppId = await ExecuteScalar<Int32>(cmd);
            _context.Logger.LogLine($"PodioAppId = {podioAppId}");
            return podioAppId;
        }

        public async Task<int> InsertPodioItem(int podioAppId, int itemId, int revision, string clientId, string envId)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_ITEM + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?podioAppId", MySqlDbType.Int32).Value = podioAppId;
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?revision", MySqlDbType.Int32).Value = revision;
            cmd.Parameters.Add("?clientId", MySqlDbType.VarChar).Value = clientId;
            cmd.Parameters.Add("?envId", MySqlDbType.VarChar).Value = envId;
            var newItemId = await ExecuteScalar<Int32>(cmd);
            _context.Logger.LogLine($"PodioItemId = {newItemId}");
            return newItemId;
        }

        public async Task<List<MySqlPodioField>> SelectAppFields(int podioAppId)
        {
            var cmd = new MySqlCommand(MySqlQueries.SELECT_APP_FIELDS, _conn);
            cmd.Parameters.Add("?podioAppId", MySqlDbType.Int32).Value = podioAppId;
            var reader = await ExecuteQuery(cmd);
            var result = new List<MySqlPodioField>();
            while(reader.Read())
            {
                result.Add(new MySqlPodioField
                {
                    PodioFieldId = reader.GetInt32(0),
                    ExternalId = reader.GetString(1),
                    Type = reader.GetString(2)
                });
            }
            _context.Logger.LogLine($"App has {result.Count} fields.");
            return result;
        }

        public async Task<int> InsertCategoryField(int itemId, int fieldId, string optionText, int optionId)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_CATEGORY_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?optionText", MySqlDbType.VarChar).Value = optionText;
            cmd.Parameters.Add("?optionId", MySqlDbType.Int32).Value = optionId;
            var fieldDataId = await ExecuteScalar<Int32>(cmd);
            return fieldDataId;
        }

        public async Task<int> InsertContactField(int itemId, int fieldId, int contactId)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_CONTACT_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?contactId", MySqlDbType.Int32).Value = contactId;
            var fieldDataId = await ExecuteScalar<Int32>(cmd);
            return fieldDataId;
        }

        public async Task<int> InsertDateField(int itemId, int fieldId, DateTime? start, DateTime? end)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_DATE_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?start", MySqlDbType.DateTime).Value = start;
            cmd.Parameters.Add("?end", MySqlDbType.DateTime).Value = end;
            var fieldDataId = await ExecuteScalar<Int32>(cmd);
            return fieldDataId;
        }

        public async Task<int> InsertDurationField(int itemId, int fieldId, int duration)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_DURATION_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?duration", MySqlDbType.Int32).Value = duration;
            var fieldDataId = await ExecuteScalar<Int32>(cmd);
            return fieldDataId;
        }

        public async Task<int> InsertLocationField(int itemId, int fieldId, string location)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_LOCATION_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?location", MySqlDbType.VarChar).Value = location;
            var fieldDataId = await ExecuteScalar<Int32>(cmd);
            return fieldDataId;
        }

        public async Task<int> InsertMemberField(int itemId, int fieldId, int memberId)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_MEMBER_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?memberId", MySqlDbType.Int32).Value = memberId;
            var fieldDataId = await ExecuteScalar<Int32>(cmd);
            return fieldDataId;
        }

        public async Task<int> InsertMoneyField(int itemId, int fieldId, decimal amount, string currency)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_MONEY_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?amount", MySqlDbType.Decimal).Value = amount;
            cmd.Parameters.Add("?currency", MySqlDbType.VarChar).Value = currency;
            var fieldDataId = await ExecuteScalar<Int32>(cmd);
            return fieldDataId;
        }

        public async Task<int> InsertNumberField(int itemId, int fieldId, double value)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_NUMBER_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?value", MySqlDbType.Double).Value = value;
            var fieldDataId = await ExecuteScalar<Int32>(cmd);
            return fieldDataId;
        }

        public async Task<int> InsertPhoneField(int itemId, int fieldId, string type, string value)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_PHONEEMAIL_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?type", MySqlDbType.VarChar).Value = type;
            cmd.Parameters.Add("?value", MySqlDbType.VarChar).Value = value;
            cmd.Parameters.Add("?phoneOrEmail", MySqlDbType.VarChar).Value = "P";
            var fieldDataId = await ExecuteScalar<Int32>(cmd);
            return fieldDataId;
        }

        public async Task<int> InsertEmailField(int itemId, int fieldId, string type, string value)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_PHONEEMAIL_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?type", MySqlDbType.VarChar).Value = type;
            cmd.Parameters.Add("?value", MySqlDbType.VarChar).Value = value;
            cmd.Parameters.Add("?phoneOrEmail", MySqlDbType.VarChar).Value = "E";
            var fieldDataId = await ExecuteScalar<Int32>(cmd);
            return fieldDataId;
        }

        public async Task<int> InsertProgressField(int itemId, int fieldId, int progress)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_PROGRESS_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?progress", MySqlDbType.Int32).Value = progress;
            var fieldDataId = await ExecuteScalar<Int32>(cmd);
            return fieldDataId;
        }

        public async Task<int> InsertRelationField(int itemId, int fieldId, int refId)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_RELATION_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?ref", MySqlDbType.Int32).Value = refId;
            var fieldDataId = await ExecuteScalar<Int32>(cmd);
            return fieldDataId;
        }

        public async Task<int> InsertTextField(int itemId, int fieldId, string text)
        {
            var cmd = new MySqlCommand(MySqlQueries.INSERT_TEXT_DATA + MySqlQueries.GET_ID, _conn);
            cmd.Parameters.Add("?itemId", MySqlDbType.Int32).Value = itemId;
            cmd.Parameters.Add("?fieldId", MySqlDbType.Int32).Value = fieldId;
            cmd.Parameters.Add("?text", MySqlDbType.Int32).Value = text;
            var fieldDataId = await ExecuteScalar<Int32>(cmd);
            return fieldDataId;
        }
    }
}
