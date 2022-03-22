using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using MultiTenant.Core.Settings;
using MultiTenant.Infrastructure.Services;
using MultiTenant.UnitTests.Mockers.Services;
using NUnit.Framework;
using System;

namespace MultiTenant.UnitTests.Services
{
    public class TenantServiceUnitTests
    {
        private TenantService _sut;

        [Test]
        public void GetTenant_Returns_Correct_Tenant()
        {
            //Arrange
            _sut = new TenantService(
                GetTenantSettings(),
                GetHttpContextAccessor("Tenant1"));

            //Act
            var tenant = _sut.GetTenant();

            //Assert
            Assert.AreEqual("Tenant1", tenant.Id);
            Assert.AreEqual("Tenant1Name", tenant.Name);
            Assert.AreEqual("Tenant1ConnectionString", tenant.ConnectionString);
        }

        [Test]
        public void GetTenant_Returns_Correct_Default_Tenant_Settings()
        {
            //Arrange
            _sut = new TenantService(
                GetTenantSettings(),
                GetHttpContextAccessor("Tenant3"));

            //Act
            var tenant = _sut.GetTenant();

            //Assert
            Assert.AreEqual("Tenant3", tenant.Id);
            Assert.AreEqual("Tenant3Name", tenant.Name);
            Assert.AreEqual("DefaultConnectionString", tenant.ConnectionString);
        }

        [Test]
        public void GetTenant_Throws_Exceptions_When_Tenant_Header_Not_Found()
        {
            Assert.Throws<Exception>(() => new TenantService(
                            GetTenantSettings(),
                            GetHttpContextAccessor(null)));
        }

        [Test]
        public void GetTenant_Throws_Exceptions_When_Tenant_Header_Not_Valid()
        {
            Assert.Throws<Exception>(() => new TenantService(
                            GetTenantSettings(),
                            GetHttpContextAccessor("InvalidTenant")));
        }

        private IOptions<TenantSettings> GetTenantSettings()
        {
            return TenantSettingsData.CreateTenantSettings();
        }

        private IHttpContextAccessor GetHttpContextAccessor(string tenantId)
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();

            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                context.Request.Headers["tenant"] = tenantId;
            }

            mockHttpContextAccessor
                .Setup(_ => _.HttpContext)
                .Returns(context);

            return mockHttpContextAccessor.Object;
        }
    }
}
