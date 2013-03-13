using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using FlexProviders.Membership;
using FlexProviders.Roles;

namespace FlexProviders.EF
{
    public class FlexRoleStore<TRole, TUser> : IFlexRoleStore
        where TRole : class, IFlexRole<TUser>, new()
        where TUser : class, IFlexMembershipUser, new()
    {
		private readonly IFlexDataStore _db;

		public FlexRoleStore(IFlexDataStore db)
        {
            _db = db;
        }

        public void CreateRole(string roleName)
        {
            var role = new TRole {Name = roleName};
            _db.Add(role);
            _db.CommitChanges();
        }

        public string[] GetRolesForUser(string username)
        {
//            return _db.Set<TRole>().Where(role => role.Users.Any(u => u.Email.Equals(username)))
//                           .Select(role => role.Name).ToArray();
			return _db.All<TRole>().Where(role => role.Users.Any(u => u.Email.Equals(username)))
						   .Select(role => role.Name).ToArray();
        }

        public string[] GetUsersInRole(string roleName)
        {
            return _db.All<TRole>().Where(role => role.Name.Equals(roleName))
							.SelectMany(role => role.Users).Select(user => user.Email)
							.ToArray();

        }

        public string[] GetAllRoles()
        {
            return _db.All<TRole>().Select(role => role.Name).ToArray();
        }

        public string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return _db.All<TRole>().Where(role => role.Name.Equals(roleName))
							.SelectMany(role => role.Users).Where(user => user.Email.StartsWith(usernameToMatch)).Select(user => user.Email)
							.ToArray();
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            var users = _db.All<TUser>().Where(u => usernames.Contains(u.Email)).ToList();

            foreach (var roleName in roleNames)
            {
                var role = _db.All<TRole>().Include(r=>r.Users).SingleOrDefault(r => r.Name == roleName);
                if (role != null)
                {
                    foreach (var user in users)
                    {
                        role.Users.Remove(user);
                    }
                }
            }
            _db.CommitChanges();
        }

        public void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var users = _db.All<TUser>().Where(u => usernames.Contains(u.Email)).ToList();

            foreach (var roleName in roleNames)
            {
                var role = _db.Single<TRole>(r => r.Name == roleName);
                if (role != null)
                {
                    if(role.Users == null)
                    {
                        role.Users = new Collection<TUser>();
                    }
                    foreach (var user in users)
                    {
                        role.Users.Add(user);
                    }
                }
            }
            _db.CommitChanges();
        }

        public bool RoleExists(string roleName)
        {
            return _db.All<TRole>().Any(r => r.Name == roleName);
        }

        public bool DeleteRole(string roleName)
        {
            var role = _db.All<TRole>().Include(r=>r.Users).SingleOrDefault(r => r.Name == roleName);
            if (role != null)
            {
                role.Users.Clear();
                _db.Delete(role);
                _db.CommitChanges();
                return true;
            }
            return false;
        }
    }
}