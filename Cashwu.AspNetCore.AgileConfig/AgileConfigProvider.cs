using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashwu.AspNetCore.AgileConfig
{
    public class AgileConfigProvider : ConfigurationProvider, IDisposable
    {
        private static readonly object LockObject = new();
        private static DateTime _lastRequested;

        private readonly DbContextOptions<AgileConfigContext> _dbOptions;
        private readonly AgileConfigOptions _configOptions;
        private readonly CancellationTokenSource _cancellationToken;
        private Task _backgroundWorker;

        public AgileConfigProvider(DbContextOptions<AgileConfigContext> dbOptions,
                                   AgileConfigOptions configOptions)
        {
            _cancellationToken = new CancellationTokenSource();
            _dbOptions = dbOptions;
            _configOptions = configOptions;
        }

        private bool HasChanged
        {
            get
            {
                try
                {
                    using var context = new AgileConfigContext(_dbOptions);

                    var now = DateTime.UtcNow;

                    var lastUpdated = context.ConfigValue
                                             .Where(c => c.LastUpdatedOn <= now)
                                             .OrderByDescending(v => v.LastUpdatedOn)
                                             .Select(v => v.LastUpdatedOn)
                                             .FirstOrDefault();

                    var hasChanged = lastUpdated > _lastRequested;

                    _lastRequested = lastUpdated;

                    return hasChanged;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        public void Dispose()
        {
            _cancellationToken.Cancel();

            _backgroundWorker?.Dispose();
            _backgroundWorker = null;
        }

        public override void Load()
        {
            using (var dbContext = new AgileConfigContext(_dbOptions))
            {
                _lastRequested = DateTime.UtcNow;
                Data = GetDataFromDatabase(dbContext);
            }

            _backgroundWorker = Task.Factory.StartNew(token =>
            {
                while (!((CancellationToken)token).IsCancellationRequested)
                {
                    if (HasChanged)
                    {
                        UpdateFromDatabase();
                    }

                    Thread.Sleep(_configOptions.PollingInterval * 1_000);
                }
            }, _cancellationToken.Token, _cancellationToken.Token);
        }

        private void UpdateFromDatabase()
        {
            using var dbContext = new AgileConfigContext(_dbOptions);

            var configData = GetDataFromDatabase(dbContext);

            if (_configOptions.DebugLog)
            {
                var value = string.Join(", ", configData.Select(a => $"{a.Key} - {a.Value}"));
                Console.WriteLine($" config value change - {value}");
            }

            lock (LockObject)
            {
                Data = configData;
            }

            OnReload();
        }

        private IDictionary<string, string> GetDataFromDatabase(AgileConfigContext dbContext)
        {
            try
            {
                return dbContext.ConfigValue.ToDictionary(c => c.Key, c => c.Value);
            }
            catch (SqlException)
            {
                return new Dictionary<string, string>();
            }
        }
    }
}