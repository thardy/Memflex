using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects.DataClasses;
using System.Linq;
using FlexProviders.Membership;
using Microsoft.Web.WebPages.OAuth;

namespace FlexProviders.EF
{
    public class FlexMembershipUserStore<TUser> 
        : IFlexUserStore
            where TUser: class, IFlexMembershipUser, new()             
    {
        //private readonly DbContext _context;
	    private readonly IFlexDataStore _db;

		public FlexMembershipUserStore (IFlexDataStore db) {
	        _db = db;
        }
                    
        public IFlexMembershipUser GetUserByEmail(string username)
        {
			return _db.Single<TUser>(u => u.Email == username);//.Set<TUser>().SingleOrDefault(u => u.Email == username);
        }

        public IFlexMembershipUser Add(IFlexMembershipUser user)
        {
			user = _db.Add((TUser)user);//.Set<TUser>().Add((TUser)user);
			_db.CommitChanges();//.SaveChanges();
			return user;
        }

        public IFlexMembershipUser Save(IFlexMembershipUser user) {
			//_db.Entry(user).State = EntityState.Modified;
			_db.CommitChanges();
           return user;
        }

        public IFlexMembershipUser CreateOAuthAccount(string provider, string providerUserId, IFlexMembershipUser user)
        {
			user = _db.Single<TUser>(u => u.Email == user.Email);//.Set<TUser>().Single(u => u.Email == user.Email);
            if(user.OAuthAccounts == null)
            {
                user.OAuthAccounts = new EntityCollection<FlexOAuthAccount>();
            }
            user.OAuthAccounts.Add(new FlexOAuthAccount() { Provider = provider, ProviderUserId = providerUserId});
			_db.CommitChanges();
            return user;
        }

        public IFlexMembershipUser GetUserByOAuthProvider(string provider, string providerUserId)
        {
			//var user = _db.Set<TUser>().SingleOrDefault(u => u.OAuthAccounts.Any(a => a.Provider == provider && a.ProviderUserId == providerUserId));
	        var user = _db.Single<TUser>(u => u.OAuthAccounts.Any(a => a.Provider == provider && a.ProviderUserId == providerUserId));
            return user;
        }

        public bool DeleteOAuthAccount(string provider, string providerUserId) {
	        //var account = _db.Set<FlexOAuthAccount>().Find(provider, providerUserId);
	        var account = _db.Single<FlexOAuthAccount>(x => x.Provider == provider && x.ProviderUserId == providerUserId);
            if(account != null)
            {
				//_db.Set<FlexOAuthAccount>().Remove(account);
				_db.Delete(account);
				//_db.SaveChanges();
	            _db.CommitChanges();
                return true;
            }            
            return false;
        }

        public IFlexMembershipUser GetUserByPasswordResetToken(string passwordResetToken)
        {
			//var user = _db.Set<TUser>().SingleOrDefault(u => u.PasswordResetToken == passwordResetToken);
	        var user = _db.Single<TUser>(u => u.PasswordResetToken == passwordResetToken);
            return user;
        }

        public IEnumerable<OAuthAccount> GetOAuthAccountsForUser(string username)
        {
			//var user = _db.Set<TUser>().Single(u => u.Email == username);
	        var user = _db.Single<TUser>(u => u.Email == username);
            return user.OAuthAccounts.Select(account => new OAuthAccount(account.Provider, account.ProviderUserId));
        }
    }
}