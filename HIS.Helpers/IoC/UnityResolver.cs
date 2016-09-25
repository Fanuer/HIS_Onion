using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace HIS.Helpers.IoC
{
    /// <summary>
    /// Creates an IoC Resolver that uses the Framework Unity
    /// </summary>
    public class UnityResolver : IDependencyResolver
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public UnityResolver(IUnityContainer container)
        {
            if (container == null){throw new ArgumentNullException(nameof(container));}
            this.Container = container;
        }
        #endregion

        #region METHODS
        /// <summary>
        /// Creates one instance of a type
        /// </summary>
        /// <param name="serviceType">Type of created Service</param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return Container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a collection of objects of a specified type
        /// </summary>
        /// <param name="serviceType">Type of created Service</param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return Container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        /// <summary>
        /// Is used to create a Child scrope for each Controller
        /// </summary>
        /// <returns></returns>
        public IDependencyScope BeginScope()
        {
            var child = Container.CreateChildContainer();
            return new UnityResolver(child);
        }

        /// <summary>
        /// Releases all unused resources
        /// </summary>
        public void Dispose()
        {
            Container.Dispose();
        }

        #endregion

        #region PROPERTIES
        /// <summary>
        /// IoC Container
        /// </summary>
        protected IUnityContainer Container { get; set; }

        #endregion
    }
}
