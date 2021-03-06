using System.Linq;
using Xunit;

namespace FlexProviders.Tests.Integration.Raven.Membership
{
    public class ChangePasswordTests : IntegrationTest
    {
        [Fact]
        public void Can_Change_Password()
        {
            var username = "sallen";
            var password = "12345678";
            var user = new User { Email = username, Password = password };
            MembershipProvider.CreateAccount(user);

            var firstEncodedPassword = Verifier.Query<User>().Single(u => u.Email == "sallen").Password;
            MembershipProvider.ChangePassword(username, password, "foo");
            var secondEncodedPassword = Verifier.Query<User>().Single(u => u.Email == "sallen").Password;

            Assert.NotEqual(firstEncodedPassword, secondEncodedPassword);
        }
    }
}