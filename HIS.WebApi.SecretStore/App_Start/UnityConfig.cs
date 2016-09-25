using Microsoft.Practices.Unity;
using System.Web.Http;
using HIS.WebApi.SecretStore.Repositories;
using Unity.WebApi;

namespace HIS.WebApi.SecretStore
{
    /// <summary>
    /// Config Class for Unity-IoC-Component
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Specifies the IoC-Mappings
        /// </summary>
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ISecretRepository, MongoDbSecretRepository>(new HierarchicalLifetimeManager(), new InjectionFactory(c => new MongoDbSecretRepository()));
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}