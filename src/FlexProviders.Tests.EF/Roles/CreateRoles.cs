using System;
using Xunit;
using Xunit.Extensions;

namespace FlexProviders.Tests.Integration.EF.Roles
{
    public class CreateRoles : IntegrationTest
    {
        [Fact]
        [AutoRollback]
        public void Can_Create_Role()
        {
            RoleProvider.CreateRole("admin");

            Assert.True(_db.CanFindRole("admin"));
        }

        [Fact]
        [AutoRollback]
        public void Can_Add_Users_To_Role()
        {
            var user = new User {Email = "sallen", Password="123", PasswordResetTokenExpiration = DateTime.Now};
            RoleProvider.CreateRole("admin");
            MembershipProvider.CreateAccount(user);

            RoleProvider.AddUsersToRoles(new [] { "sallen"}, new [] { "admin"});

            Assert.True(_db.UserIsInRole("sallen", "admin"));
        }

     
    }
}