using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Owin;

namespace HIS.WebApi.SecretStore
{
    public class Startup
    {
        /// <summary>
        /// Get's fired when the applications is started by the host
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();
            UnityConfig.RegisterComponents();
            SwaggerConfig.RegisterSwagger();
            app.UseWebApi(httpConfig);
        }
    }
}