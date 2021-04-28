using System;

namespace JanHafner.AspNet.CorsSettings
{
    public class CorsPolicySettings
    {
        public bool IsDefaultPolicy { get; set; }

        public string Name { get; set; }

        public string[] ExposedHeaders { get; set; } = Array.Empty<string>();
                     
        public string[] Headers { get; set; } = Array.Empty<string>();

        public string[] Methods { get; set; } = Array.Empty<string>();

        public string[] Origins { get; set; } = Array.Empty<string>();

        public bool SupportsCredentials { get; set; }
    }
}
