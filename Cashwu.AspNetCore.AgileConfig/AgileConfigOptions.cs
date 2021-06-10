namespace Cashwu.AspNetCore.AgileConfig
{
    public class AgileConfigOptions
    {
        /// <summary>
        /// default name is DefaultConnection
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// default interval is 5 second
        /// </summary>
        public int PollingInterval { get; set; } = 5;

        public bool DebugLog { get; set; }
    }
}