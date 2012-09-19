namespace FlexProviders
{
    public interface IFlexUserStore
    {        
        IFlexMembershipUser GetUserByUsername(string username);
        IFlexMembershipUser Add(IFlexMembershipUser user);
        IFlexMembershipUser Save(IFlexMembershipUser user);        
    }    
}