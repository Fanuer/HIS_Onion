using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HIS.Helpers.IoC;
using HIS.WebApi.SecretStore.Repositories;
using Microsoft.Practices.Unity;
using Swashbuckle.Application;

namespace HIS.WebApi.SecretStore
{
    /// <summary>
    /// Web Application Settings
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the Web Application Settings
        /// </summary>
        /// <param name="config">http configuration</param>
        public static void Register(HttpConfiguration config)
        {
            UnityConfig.RegisterComponents();
            SwaggerConfig.RegisterSwagger();
            // Web API-Routen
            config.MapHttpAttributeRoutes();
        }
        
    }
}
