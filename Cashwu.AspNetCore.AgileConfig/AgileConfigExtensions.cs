using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashwu.AspNetCore.AgileConfig
{
    public static class AgileConfigExtensions
    {
        public static IConfigurationBuilder AddEntityFrameworkValues(this IConfigurationBuilder builder, Action<AgileConfigOptions> optionsAction = null)
        {
            var connectionStringConfig = builder.Build();

            var configOptions = new AgileConfigOptions
            {
                ConnectionStringName = "DefaultConnection",
                PollingInterval = 5,
            };

            optionsAction?.Invoke(configOptions);

            var dbOptions = new DbContextOptionsBuilder<AgileConfigContext>();
            dbOptions = dbOptions.UseSqlServer(connectionStringConfig.GetConnectionString(configOptions.ConnectionStringName));

            return builder.Add(new AgileConfigSource(dbOptions.Options, configOptions));
        }
    }
}