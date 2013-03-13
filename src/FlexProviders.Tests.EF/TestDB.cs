using System;
using System.Data.Entity;
using System.Linq;
using FlexProviders.Tests.Integration.EF.Migrations;

namespace FlexProviders.Tests.Integration.EF
{
    public class TestDb 
    {
        public TestDb()
        {
            _db = _initializer.Value;
        }

        static dynamic Initialize()
        {
            UseEntityFrameworkToCreateDatabase();
            return Simple.Data.Database.OpenNamedConnection("Default");
        }

        static void UseEntityFrameworkToCreateDatabase()
        {
            Database.Delete("Default");
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SomeDb, Configuration>("Default"));
            var context = new SomeDb("name=Default");
            var users = context.Users.ToList();           
            var roles = context.Roles.ToList();
            foreach(var role in roles)
            {
                context.Roles.Remove(role);
            }
            foreach(var user in users)
            {
                context.Users.Remove(user);
            }
        }

        public bool CanFindEmail(string email)
        {
            return _db.Users.FindByEmail(email) != null;
        }

        public string GetPassword(string email)
        {
            return _db.Users.FindByEmail(email).Password;
        }

        public int GetCountOfOAuthAccounts(string email)
        {
			var userId = _db.Users.FindByEmail(email).Id;
            return _db.FlexOAuthAccounts.FindAll(_db.FlexOAuthAccounts.User_Id == userId).Count();
        }

        public bool CanFindRole(string name)
        {
            return _db.Roles.FindByName(name) != null;
        }

        public bool UserIsInRole(string username, string rolename)
        {
            var userId = _db.Users.FindByEmail(username).Id;
            var roleId = _db.Roles.FindByName(rolename).Id;

            return _db.RoleUsers.FindBy(User_Id: userId, Role_Id: roleId) != null;       
        }

        private readonly dynamic _db;
        private readonly static Lazy<dynamic> _initializer = new Lazy<dynamic>(Initialize);
    }
}