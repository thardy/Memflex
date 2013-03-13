using FlexProviders.EF;
using FlexProviders.Membership;

namespace LogMeIn.Models
{
    public class RoleStore : FlexRoleStore<Role,User>
    {
		public RoleStore(IFlexDataStore db)
			: base(db)
        {
            
        }
    }
}