using FlexProviders.EF;
using FlexProviders.Membership;

namespace LogMeIn.Models
{
    public class UserStore : FlexMembershipUserStore<User>
    {
        public UserStore(IFlexDataStore db) : base(db)
        {
            
        }
    }
}