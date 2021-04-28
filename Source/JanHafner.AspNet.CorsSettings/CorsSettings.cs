using System;

namespace JanHafner.AspNet.CorsSettings
{
    public class CorsSettings
    {
        public const string DEFAULT_KEY = "Cors";

        public CorsPolicySettings[] Policies { get; set; } = Array.Empty<CorsPolicySettings>();
    }
}
