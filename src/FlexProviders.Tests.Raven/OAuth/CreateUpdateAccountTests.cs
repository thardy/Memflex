using System.Linq;
using Xunit;

namespace FlexProviders.Tests.Integration.Raven.OAuth
{
    public class CreateUpdateAccountTests : IntegrationTest
    {
        [Fact]
        public void Can_Create_OAuth_Account()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", new User { Email="sallen"});

            Assert.Equal(1, Verifier.Query<User>().Single(u=>u.Email == "sallen").OAuthAccounts.Count());
        }

        [Fact]
        public void Can_Update_OAuth_Account()
        {
            MembershipProvider.CreateOAuthAccount("Microsoft", "bitmask", new User { Email = "sallen" });
            MembershipProvider.CreateOAuthAccount("Yahoo", "bitmask", new User { Email = "sallen" });

            Assert.Equal(2, Verifier.Query<User>().Single(u => u.Email == "sallen").OAuthAccounts.Count());
        }
    }
}