using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Onion.Client;
using IUser = Onion.Client.IUser;

namespace HIS.WebApi.Auth.Provider
{
  public class CustomOAuthProvider : OAuthAuthorizationServerProvider
  {
    #region FIELDS

    private readonly string informationServerUri = "tcp://127.0.0.1:8087/onion/server";
    private readonly string roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    private readonly string userIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    private readonly string userNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
    #endregion

    #region CTOR
    #endregion

    #region METHODS
    /// <summary>
    /// responsible for validating if the Resource server (audience) is already registered in our Authorization server by reading the client_id value from the request
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
      string clientId;
      string clientSecret;

      if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
      {
        context.TryGetFormCredentials(out clientId, out clientSecret);
      }

      if (context.ClientId == null && String.IsNullOrWhiteSpace(clientId))
      {
        context.SetError("invalid_clientId", "client_Id is not set");
      }
      else if (!context.HasError)
      {
        var audience = await Repository.Instance.Clients.FindAsync(context.ClientId);
        if (audience == null)
        {
          context.SetError("invalid_clientId", $"Client '{context.ClientId}' is not registered in the system.");
        }
        else
        {
          context.OwinContext.Set("as:clientId", clientId);
          context.OwinContext.Set("as:clientAllowedOrigin", audience.AllowedOrigin);
          context.Validated();
        }
      }
    }

    /// <summary>
    /// validating the resource owner (user) credentials
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
      //Allowing cross domain resources for external logins
      var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";
      context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

      //Search user by username and password
      bool isSessionCreated = false;
      OnionSession session = null;
      try
      {
        session = new OnionSession(informationServerUri, context.UserName, context.Password);
        isSessionCreated = true;
        if (!isSessionCreated)
        {
          context.SetError("invalid_grant", "The user name or password is incorrect.");
          return;
        }

        var repo = Repository.Instance.RefreshTokens;
        var allTokens = await repo.GetAllAsync();
        var oldtokens = allTokens.Where(x => x.ExpiresUtc < DateTime.UtcNow || x.Subject.Equals(context.UserName)).ToList();
        foreach (var token in oldtokens)
        {
          await repo.RemoveAsync(token);
        }


        var clientId = context.OwinContext.Get<string>("as:clientId");
        var identity = await user.GenerateUserIdentityAsync(userManager, "JWT", clientId);
        
        var ticket = new AuthenticationTicket(identity, props);
        context.Validated(ticket);
      }
      catch (Exception)
      {

      }
      finally
      {
        session?.Dispose();
      }


      session.Dispose();
    }

    public override Task TokenEndpoint(OAuthTokenEndpointContext context)
    {
      foreach (var property in context.Properties.Dictionary)
      {
        context.AdditionalResponseParameters.Add(property.Key, property.Value);
      }
      if (!context.AdditionalResponseParameters.ContainsKey("UserId"))
      {
        context.AdditionalResponseParameters.Add("UserId", context.Identity.GetUserId());
      }
      else
      {
        context.AdditionalResponseParameters["UserId"] = context.Identity.GetUserId();
      }

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

    private ClaimsIdentity GenerateClaims(string username, OnionSession session, string authenticationType)
    {
      if (String.IsNullOrWhiteSpace(username)) { throw new ArgumentNullException(nameof(username)); }
      if (session == null) { throw new ArgumentNullException(nameof(session)); }
      if (String.IsNullOrWhiteSpace(authenticationType)) { throw new ArgumentNullException(nameof(authenticationType)); }


      var factory = new ClaimsIdentityFactory<Microsoft.AspNet.Identity.IUser>();
      ClaimsIdentity id = new ClaimsIdentity(authenticationType);

      id.AddClaim(new Claim(this.userIdClaimType, user.Id.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
      id.AddClaim(new Claim(this.userNameClaimType, username, "http://www.w3.org/2001/XMLSchema#string"));
      id.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"));
      id.AddClaims(this.GetOnionRoles(user));

      return id;
    }

    private IEnumerable<Claim> GetOnionRoles(IUser user)
    {
      IEnumerable<Claim> result = new List<Claim>();
      if (user == null){ throw new ArgumentNullException(nameof(user));}
      return result;
    }



    #endregion

      #region PROPERTIES
      #endregion
    }
}