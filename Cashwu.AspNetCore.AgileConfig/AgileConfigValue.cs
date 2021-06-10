using System;

namespace Cashwu.AspNetCore.AgileConfig
{
    public sealed class AgileConfigValue
    {
        public string Key { get; set; }

        public string Value { get; set; }

#pragma warning disable 649
        private DateTime? _lastUpdated;
#pragma warning restore 649

        public DateTime LastUpdatedOn => _lastUpdated ?? DateTime.UtcNow;

        public string LastUpdatedBy { get; set; }
    }
}