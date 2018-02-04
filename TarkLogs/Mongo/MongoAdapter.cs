using MongoDB.Driver;
using System;
using TarkLogs.Constants;
using TarkUtils;

namespace TarkLogs.Mongo
{
    public class MongoAdapter
    {
        MongoClient _db;

        public MongoAdapter()
        {
            var timeout = AppSettingsManager.Get<double>(AppSettingsConstants.KEY_TIMEOUT, DatabaseConstants.DEFAULT_TIMEOUT);
            var host = AppSettingsManager.Get<string>(AppSettingsConstants.KEY_HOST);
            var port = AppSettingsManager.Get<int>(AppSettingsConstants.KEY_PORT, DatabaseConstants.DEFAULT_PORT);

            _db = new MongoClient(new MongoClientSettings()
            {
                ConnectTimeout = TimeSpan.FromSeconds(timeout),
                Server = new MongoServerAddress(host, port)
            });
        }

        public void Persist<T>(string database, string collection, T obj)
        {
            _db.GetDatabase(database)
                .GetCollection<T>(collection)
                .InsertOneAsync(obj);
        }
    }
}
