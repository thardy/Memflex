using System.Web.Mvc;
using FlexProviders.Aspnet;
using FlexProviders.Membership;
using FlexProviders.Roles;
using LogMeIn.Models;
using StructureMap;
namespace LogMeIn {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });
                            x.For<IFilterProvider>().Use<SmFilterProvider>();

                            x.For<IFlexMembershipProvider>().HttpContextScoped().Use<FlexMembershipProvider>();
                            x.For<IFlexRoleProvider>().HttpContextScoped().Use<FlexRoleProvider>();
                            x.For<IFlexUserStore>().HttpContextScoped().Use<UserStore>();
                            x.For<IFlexRoleStore>().HttpContextScoped().Use<RoleStore>();
                            x.SetAllProperties(p => p.OfType<IFlexRoleProvider>());
                            x.Forward<IFlexMembershipProvider, IFlexOAuthProvider>();

                            x.For<IApplicationEnvironment>().Singleton().Use<AspnetEnvironment>();
                            x.For<ISecurityEncoder>().Singleton().Use<DefaultSecurityEncoder>();

                            
                            //x.For<MoviesDb>().HybridHttpOrThreadLocalScoped().Use<MoviesDb>();
							x.For<IFlexDataStore>().HybridHttpOrThreadLocalScoped().Use<MoviesDb>();
                            x.SelectConstructor(() => new MoviesDb());                            
                        });
            return ObjectFactory.Container;
        }
    }
}