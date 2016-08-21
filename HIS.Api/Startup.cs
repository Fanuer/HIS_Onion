using System;
using System.Configuration;
using System.Web.Http;
using AutoMapper;
using HIS.WebApi.Auth;
using HIS.WebApi.Auth.Provider;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace HIS.WebApi.Auth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();

            //ConfigureAuth(app);
        }

        /// <summary>
        /// Configure the db context and user manager to use a single instance per request
        /// </summary>
        /// <param name="app"></param>
        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            /*
      app.CreatePerOwinContext(ContextFactory.Instance.CreateRepository);

      OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
      {
#warning For Dev enviroment only (on production should be AllowInsecureHttp = false)
        AllowInsecureHttp = true,
        TokenEndpointPath = new PathString("/oauth2/token"),
        AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
        Provider = new CustomOAuthProvider(), // specify, how to validate the Resource Owner
        AccessTokenFormat = new CustomJwtFormat(ConfigurationManager.AppSettings["as:Issuer"]), //Specifies the implementation, how to generate the access token
        RefreshTokenProvider = new CustomRefreshTokenProvider()
      };

      // OAuth 2.0 Bearer Access Token Generation
      app.UseOAuthAuthorizationServer(OAuthServerOptions);
      */
        }
    }
}
