using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using HIS.Api.Provider;
using HIS.Api.Repositories;
using HIS.WebApi.Helper.Repositories;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Onion.Client;
using Owin;

[assembly: OwinStartup(typeof(HIS.Api.Startup))]

namespace HIS.Api
{
  public partial class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      HttpConfiguration httpConfig = new HttpConfiguration();

      //ConfigureAuth(app);
    }

    private ConfigureAutomapper(IAppBuilder app)
    {
      app.CreatePerOwinContext(AutoMapperConfigFactory.Instance.Create().CreateMapper);
    }

    /// <summary>
    /// Configure the db context and user manager to use a single instance per request
    /// </summary>
    /// <param name="app"></param>
    private void ConfigureOAuthTokenGeneration(IAppBuilder app)
    {
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
    }
  }
}
