using FluentAssertions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace JanHafner.AspNet.CorsSettings.Tests.CorsMiddlewareExtensionsTests
{
    public sealed class AddCors
    {
        [Fact]
        public void ThrowsArgumentNullExceptionIfServiceCollectionIsNull()
        {
            // Arrange
            IServiceCollection serviceCollection = null;
            var configurationMock = new Mock<IConfiguration>();
            const string key = "Cors";

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => serviceCollection.AddCors(configurationMock.Object, key));
        }

        [Fact]
        public void ThrowsArgumentNullExceptionIfConfigurationIsNull()
        {
            // Arrange
            var serviceCollectionMock = new Mock<IServiceCollection>();
            IConfiguration configuration = null;
            const string key = "Cors";

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => serviceCollectionMock.Object.AddCors(configuration, key));
        }

        [Fact]
        public void ThrowsArgumentExceptionIfKeyIsNull()
        {
            // Arrange
            var serviceCollectionMock = new Mock<IServiceCollection>();
            var configurationMock = new Mock<IConfiguration>();
            const string key = null;

            // Act, Assert
            Assert.Throws<ArgumentException>(() => serviceCollectionMock.Object.AddCors(configurationMock.Object, key));
        }

        [Fact]
        public void ThrowsExceptionIfTheConfigurationSectionWasNotFound()
        {
            // Arrange
            var serviceCollectionMock = new Mock<IServiceCollection>();
            var configuration = new ConfigurationBuilder().AddJsonStream(new MemoryStream(TestAppSettings.Default))
                                                              .Build();
            var key = Guid.NewGuid().ToString();

            // Act, Assert
            Assert.Throws<KeyNotFoundException>(() => serviceCollectionMock.Object.AddCors(configuration, key));
        }

        [Fact]
        public async Task ProvidesThePoliciesCorrectly()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder().AddJsonStream(new MemoryStream(TestAppSettings.Default))
                                                              .Build();
            const string key = "Cors";
            const string myDefaultPolicyName = "MyDefaultPolicy";
            const string myPolicyName = "MyPolicy";

            // Act
            serviceCollection.AddCors(configuration, key);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var corsOptions = serviceProvider.GetRequiredService<IOptions<CorsOptions>>().Value;

            // Assert
            var myDefaultPolicy = corsOptions.GetPolicy(myDefaultPolicyName);
            myDefaultPolicy.Should().NotBeNull();
            myDefaultPolicy.ExposedHeaders.Should().NotBeNull().And.BeEmpty();
            myDefaultPolicy.AllowAnyHeader.Should().BeTrue();
            myDefaultPolicy.Headers.Should().NotBeNullOrEmpty().And.ContainSingle("*");
            myDefaultPolicy.AllowAnyMethod.Should().BeTrue();
            myDefaultPolicy.Methods.Should().NotBeNullOrEmpty().And.ContainSingle("*");
            myDefaultPolicy.AllowAnyOrigin.Should().BeFalse();
            myDefaultPolicy.Origins.Should().NotBeNullOrEmpty().And.ContainSingle("https://localhost:4200");
            myDefaultPolicy.SupportsCredentials.Should().BeTrue();

            var myPolicy = corsOptions.GetPolicy(myPolicyName);
            myPolicy.Should().NotBeNull();
            myPolicy.ExposedHeaders.Should().NotBeNullOrEmpty().And.ContainSingle("GET");
            myPolicy.AllowAnyHeader.Should().BeFalse();
            myPolicy.Headers.Should().NotBeNullOrEmpty().And.BeEquivalentTo("Authorization", "Content-Type");
            myPolicy.AllowAnyMethod.Should().BeFalse();
            myPolicy.Methods.Should().NotBeNullOrEmpty().And.BeEquivalentTo("PUT", "DELETE", "PATCH");
            myPolicy.AllowAnyOrigin.Should().BeTrue();
            myPolicy.Origins.Should().NotBeNull().And.ContainSingle("*");
            myPolicy.SupportsCredentials.Should().BeFalse();
        }
    }
}
