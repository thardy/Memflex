using Xunit;
using Xunit.Extensions;

namespace FlexProviders.Tests.Integration.EF.Roles
{
    public class QueryRoles : IntegrationTest
    {
        [Fact]
        [AutoRollback]
        public void Can_Find_User_In_Role()
        {
            var user = new User { Email = "sallen", Password = "123" };
            RoleProvider.CreateRole("admin");
            MembershipProvider.CreateAccount(user);

            RoleProvider.AddUsersToRoles(new[] { "sallen" }, new[] { "admin" });

            Assert.True(RoleProvider.IsUserInRole("sallen", "admin"));
        }

        [Fact]
        [AutoRollback]
        public void Doesnt_Find_User_Not_In_Role()
        {
            var user = new User { Email = "sallen", Password = "123" };
            RoleProvider.CreateRole("admin");
            MembershipProvider.CreateAccount(user);

            RoleProvider.AddUsersToRoles(new[] { "sallen" }, new[] { "admin" });

            Assert.False(RoleProvider.IsUserInRole("sallen", "sales"));
        }

        [Fact]
        [AutoRollback]
        public void Can_Get_Roles_For_User()
        {
            var user = new User { Email = "sallen", Password = "123" };
            MembershipProvider.CreateAccount(user);

            RoleProvider.CreateRole("admin");
            RoleProvider.CreateRole("sales");
            RoleProvider.CreateRole("engineering");

            RoleProvider.AddUsersToRoles(new[] { "sallen" }, new[] { "admin", "engineering" });

            Assert.True(RoleProvider.GetRolesForUser("sallen").Length == 2);
        }

        [Fact]
        [AutoRollback]
        public void Can_Tell_Role_Exists()
        {
            RoleProvider.CreateRole("admin");
    
            Assert.True(RoleProvider.RoleExists("admin"));
        }

        [Fact]
        [AutoRollback]
        public void Can_Tell_Role_Doesnt_Exists()
        {
            RoleProvider.CreateRole("sales");

            Assert.False(RoleProvider.RoleExists("admin"));
        }

        [Fact]
        [AutoRollback]
        public void Can_Get_All_Roles()
        {
            RoleProvider.CreateRole("admin");
            RoleProvider.CreateRole("engineering");
            RoleProvider.CreateRole("sales");

            var result = RoleProvider.GetAllRoles();

            Assert.Equal(3, result.Length);
        }

        [Fact]
        [AutoRollback]
        public void Can_Get_Users_In_Role()
        {
            var user1 = new User { Email = "sallen", Password = "123" };
            var user2 = new User { Email = "missmm", Password = "123" };

            RoleProvider.CreateRole("admin");
            RoleProvider.CreateRole("engineering");
            RoleProvider.CreateRole("sales");

            MembershipProvider.CreateAccount(user1);
            MembershipProvider.CreateAccount(user2);

            RoleProvider.AddUsersToRoles(new[] { "sallen", "missmm" }, new[] { "admin", "engineering" }); 
  

            Assert.Equal(2, RoleProvider.GetUsersInRole("admin").Length);
        }
    }
}