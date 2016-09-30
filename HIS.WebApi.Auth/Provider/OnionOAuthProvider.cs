using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Data.Interfaces.Models;
using HIS.WebApi.Auth.Data.Interfaces.Repository;
using HIS.WebApi.Auth.Models.XMLModels;
using Microsoft.Owin.Security.OAuth;
using Onion.Client;

namespace HIS.WebApi.Auth.Provider
{
    public class OnionOAuthProvider:CustomOAuthProvider
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public OnionOAuthProvider(IBearerTokenUserService<IUser<string>> repository) : base(repository)
        {
        }
        #endregion

        #region METHODS

        public override void AddAdditionalClaims(ClaimsIdentity authClaims, OAuthGrantResourceOwnerCredentialsContext context)
        {
            base.AddAdditionalClaims(authClaims, context);
            if (authClaims != null && context != null)
            {
                var session = new OnionSession(EnvironmentSettings.Instance.OnionData.InformationServerUri, context.UserName, context.Password);
                authClaims.AddClaim(new Claim("onionSession", session.SessionToken));
            }
        }

        #endregion

        #region PROPERTIES
        #endregion


    }
}
