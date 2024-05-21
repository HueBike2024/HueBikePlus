using Core.Common;
using Microsoft.Extensions.Configuration;
using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL.ColumnWriters;

namespace Core.Helpers
{
    public class LogHelper
    {
        public static ILogger Logger;
        public static ILogger ErrorLogger;
        public static ILogger InternalLogger;
        public static ILogger UserActionLogger;

        public static ILogger CreateLogger(IConfiguration configuration, string tableName)
        {
            //Used columns (Key is a column name) 
            //Column type is writer's constructor parameter
            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                {$"Message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                //{$"MessageTemplate", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                {$"Level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {$"Exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                {$"TimeStamp", new TimestampColumnWriter(NpgsqlDbType.TimestampTz)},
                {$"Path", new StringColumnWriter("Path")},
                {$"Method", new StringColumnWriter("Method")},
                {$"StatusCode", new StringColumnWriter("StatusCode")},
                {$"UserName", new StringColumnWriter("UserName")},
            };

            return new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.PostgreSQL(configuration.GetConnectionString(AppConstants.MainConnectionString), tableName, columnWriters)
                    .CreateLogger();
        }

        public static ILogger CreateErrorLogger(IConfiguration configuration, string tableName)
        {
            //Used columns (Key is a column name) 
            //Column type is writer's constructor parameter
            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                {$"Id", new RenderedMessageColumnWriter(NpgsqlDbType.Bigint) },
                {$"Message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                {$"MessageTemplate", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                {$"Level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {$"Exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                {$"TimeStamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp) }
            };


            return new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .WriteTo.PostgreSQL(configuration.GetConnectionString(AppConstants.MainConnectionString), tableName, columnWriters)
                    .CreateLogger();
        }

        public static ILogger CreateUserActionLog(IConfiguration configuration, string tableName)
        {
            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                {$"Id", new RenderedMessageColumnWriter(NpgsqlDbType.Bigint) },
                {$"UserName", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                {$"TimeStamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp) }
            };


            return new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .WriteTo.PostgreSQL(configuration.GetConnectionString(AppConstants.MainConnectionString), tableName, columnWriters)
                    .CreateLogger();
        }

        public static ILogger CreateInternalLogger(IConfiguration configuration, string tableName)
        {
            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                {$"Id", new RenderedMessageColumnWriter(NpgsqlDbType.Bigint) },
                {$"LogUrl", new RenderedMessageColumnWriter(NpgsqlDbType.Varchar) },
                {$"Path", new RenderedMessageColumnWriter(NpgsqlDbType.Varchar) },
                {$"Method", new MessageTemplateColumnWriter(NpgsqlDbType.Varchar) },
                {$"StatusCode", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {$"Authorization", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {$"ResponseBody", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {$"Username", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {$"UserAgent", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {$"TimeStamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp) }
            };


            return new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .WriteTo.PostgreSQL(configuration.GetConnectionString(AppConstants.MainConnectionString), tableName, columnWriters)
                    .CreateLogger();
        }

        public class StringColumnWriter : ColumnWriterBase
        {
            private readonly string _keyName;

            public StringColumnWriter(string keyName) : base(NpgsqlDbType.Varchar)
            {
                _keyName = keyName;
            }

            public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
            {
                var (username, value) = logEvent.Properties.FirstOrDefault(p => p.Key == _keyName);
                string result = value?.ToString()?.Trim('"');
                return result == "null" ? null : result;
            }
        }
    }
}
