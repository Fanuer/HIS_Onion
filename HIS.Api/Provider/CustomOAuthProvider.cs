using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using HIS.Helpers.Crypto;
using HIS.WebApi.Auth.Base.Interfaces.Repository;
using HIS.WebApi.Auth.Base.Models;
using HIS.WebApi.Auth.Base.Models.Enums;
using HIS.WebApi.Auth.Base.Repositories;
using HIS.WebApi.Auth.Models;
using HIS.WebApi.Auth.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Onion.Client;

namespace HIS.WebApi.Auth.Provider
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        #region CONST
        #endregion

        #region FIELDS

        private IBearerTokenUserManagementRepository<Base.Interfaces.IUser<string>> _repository;
        #endregion

        #region CTOR

        public CustomOAuthProvider(IBearerTokenUserManagementRepository<Base.Interfaces.IUser<string>> repository)
        {
            this._repository = repository;
        }

        ~CustomOAuthProvider()
        {
            this._repository?.Dispose();
            this._repository = null;
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Validates a client thats tries to access the server.
        /// Exepts all clients, the api is the onlyclient available and do not allow adding additional clients
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (String.IsNullOrEmpty(context.ClientId))
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                
                context.SetError("invalid_clientId", "ClientId should be sent.");
                return;
            }
            
            client = await _repository.Clients.FindAsync(context.ClientId);
            if (client == null)
            {
                context.SetError("invalid_clientId", $"Client '{context.ClientId}' is not registered in the system.");
                return;
            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return;
                }
                if (!Hasher.Current.ValidatePassword(clientSecret, client.Secret))
                {
                    context.SetError("invalid_clientId", "Client secret is invalid.");
                    return;
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return;
            }

            context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
        }

        /// <summary>
        /// receiving the username and password from the request and validate them against our ASP.NET 2.1 Identity system
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //Allowing cross domain resources for external logins
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            //Search user by username and password
            var userManager = context.OwinContext.GetUserManager<UserManager<HIS.WebApi.Auth.Base.Interfaces.IUser<string>>>();

            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var oldtokens = (await _repository.RefreshTokens.GetAllAsync()).Where(x => x.ExpiresUtc < DateTime.UtcNow || x.Subject.Equals(user.UserName)).ToList();
            foreach (var token in oldtokens)
            {
                await _repository.RefreshTokens.RemoveAsync(token);
            }

            // Generate claim and JWT-Ticket 
            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");
            oAuthIdentity.AddClaim(new Claim("userId", oAuthIdentity.GetUserId()));

            AddAdditionalClaims(oAuthIdentity, context);

            var ticket = new AuthenticationTicket(oAuthIdentity, null);

            //Transfer this identity to an OAuth 2.0 Bearer access ticket
            context.Validated(ticket);

        }

        public virtual void AddAdditionalClaims(ClaimsIdentity authClaims, OAuthGrantResourceOwnerCredentialsContext context)
        {
            
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            /*if (!context.AdditionalResponseParameters.ContainsKey("UserId"))
            {
                context.AdditionalResponseParameters.Add("UserId", context.Identity.GetUserId());
            }
            else
            {
                context.AdditionalResponseParameters["UserId"] = context.Identity.GetUserId();
            }*/

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary.ContainsKey("as:client_id") ? context.Ticket.Properties.Dictionary["as:client_id"] : "";
            var currentClient = context.ClientId ?? "";

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        #endregion

        #region PROPERTIES
        #endregion
    }
}