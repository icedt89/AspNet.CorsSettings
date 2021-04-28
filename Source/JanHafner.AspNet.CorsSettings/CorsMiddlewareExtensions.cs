using JanHafner.AspNet.CorsSettings;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JanHafner.AspNet.CorsSettings
{
    public static class CorsMiddlewareExtensions
    {
        public static IServiceCollection AddCors(this IServiceCollection serviceCollection, IConfiguration configuration, string key = CorsSettings.DEFAULT_KEY)
        {
            if (serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            var corsPoliciesSettings = configuration.GetSection(key).Get<CorsSettings>();
            if(corsPoliciesSettings == null)
            {
                throw new KeyNotFoundException($"The configuration section with key '{key}' was not found");
            }

            return serviceCollection.AddCors(options =>
            {
                foreach (var corsPolicySettings in corsPoliciesSettings.Policies)
                {
                    if (corsPolicySettings.IsDefaultPolicy)
                    {
                        options.DefaultPolicyName = corsPolicySettings.Name;
                        options.AddDefaultPolicy(builder => ApplyToPolicyBuilder(corsPolicySettings, builder));
                    }
                    else
                    {
                        options.AddPolicy(corsPolicySettings.Name, builder => ApplyToPolicyBuilder(corsPolicySettings, builder));
                    }
                }
            });
        }

        private static void ApplyToPolicyBuilder(CorsPolicySettings corsPolicySettigs, CorsPolicyBuilder builder)
        {
            builder.WithHeaders(corsPolicySettigs.Headers.ToArray())
                   .WithMethods(corsPolicySettigs.Methods.ToArray())
                   .WithOrigins(corsPolicySettigs.Origins.ToArray())
                   .WithExposedHeaders(corsPolicySettigs.ExposedHeaders.ToArray());

            if (corsPolicySettigs.SupportsCredentials)
            {
                builder.AllowCredentials();
            }
        }
    }
}
