using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using FlexProviders.Membership;

namespace LogMeIn.Models
{
    public class MoviesDb : DbContext, IFlexDataStore
    {
        public MoviesDb()
        {
        }

        public MoviesDb(string connectionStringOrName)
            : base(connectionStringOrName)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlexOAuthAccount>().HasKey(a => new { a.Provider, a.ProviderUserId });
            base.OnModelCreating(modelBuilder);
        }

		#region IFlexDataStore implementation
		public int CommitChanges() {
			return SaveChanges();
		}

		public void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new() {
			var query = All<T>().Where(expression);
			foreach (T item in query) {
				Delete(item);
			}
		}

		public void Delete<T>(T item) where T : class, new() {
			Set<T>().Remove(item);
		}

		public void DeleteAll<T>() where T : class, new() {
			var query = All<T>();
			foreach (var item in query) {
				Delete(item);
			}
		}

		public T Single<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new() {
			return All<T>().FirstOrDefault(expression);
		}

		public IQueryable<T> All<T>() where T : class, new() {
			return Set<T>().AsQueryable<T>();
		}

		public T Add<T>(T item) where T : class, new() {
			return Set<T>().Add(item);
		}

		public List<T> Add<T>(IEnumerable<T> items) where T : class, new() {
			return items.Select(Add).ToList();
		}
		#endregion IDatastore implementation
    }
}