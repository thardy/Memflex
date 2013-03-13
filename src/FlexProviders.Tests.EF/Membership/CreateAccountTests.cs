using FlexProviders.Membership;
using Xunit;
using Xunit.Extensions;

namespace FlexProviders.Tests.Integration.EF.Membership
{
    public class CreateAccountTests : IntegrationTest
    {
        [Fact]         
        [AutoRollback]
        public void Can_Create_Account()
        {
            var user = new User() {Email = "sallen", Password = "12345678"};

            MembershipProvider.CreateAccount(user);

            Assert.True(_db.CanFindEmail("sallen"));
        }

        [Fact]
        [AutoRollback]
        public void Fails_If_Duplicate_Username()
        {
            var user = new User() { Email = "sallen", Password = "12345678" };

            MembershipProvider.CreateAccount(user);

            Assert.Throws<FlexMembershipException>(() => MembershipProvider.CreateAccount(user));
        }

        [Fact]
        [AutoRollback]
        public void Account_Created_As_Local_Account()
        {
            var user = new User() { Email = "sallen", Password = "12345678" };
            
            MembershipProvider.CreateAccount(user);
            
            Assert.True(MembershipProvider.HasLocalAccount("sallen"));
        }
    }
}