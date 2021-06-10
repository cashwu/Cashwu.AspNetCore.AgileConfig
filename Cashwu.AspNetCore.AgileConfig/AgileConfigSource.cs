using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashwu.AspNetCore.AgileConfig
{
    public class AgileConfigSource : IConfigurationSource
    {
        private readonly DbContextOptions<AgileConfigContext> _dbOptions;
        private readonly AgileConfigOptions _configOptions;

        public AgileConfigSource(DbContextOptions<AgileConfigContext> dbOptions, AgileConfigOptions configOptions)
        {
            _dbOptions = dbOptions;
            _configOptions = configOptions;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AgileConfigProvider(_dbOptions, _configOptions);
        }
    }
}