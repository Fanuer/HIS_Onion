using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using HIS.WebApi.Auth.Exceptions;
using HIS.WebApi.Auth.Models;
using Microsoft.AspNet.Identity;
using Onion.Client;

namespace HIS.WebApi.Auth.Repositories
{
  public class OnionUserStore
    : IUserStore<User, int>,
     IUserRoleStore<User, int>,
     IQueryableUserStore<User, int>,
     IUserClaimStore<User, int>,
     IUserEmailStore<User, int>
  {
    #region FIELDS
    private bool _disposed = false;
    private OnionSession _session;
    private readonly Uri _informationServerUri;
    #endregion

    #region CTOR

    public OnionUserStore(Uri informationServerUri)
    {
      if (informationServerUri == null) { throw new ArgumentNullException(nameof(informationServerUri)); }
      this._informationServerUri = informationServerUri;
    }
    #endregion

    #region METHODS
    #region Members IUserStore
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object)this);
    }

    public async Task<User> FindByIdAsync(int userId)
    {
      return await Task.Run(() => Mapper.Instance.Map<User>(this.OnionSession.UserManagement.Users.TryGetUser(userId)));
    }

    public async Task<User> FindByNameAsync(string userName)
    {
      return await Task.Run(() => Mapper.Instance.Map<User>(this.OnionSession.UserManagement.Users.TryGetUser(userName)));
    }
    public async Task UpdateAsync(User user)
    {
      // TODO: Update User implementieren
      throw new NotImplementedException();
      //return await Task.Run(() => Mapper.Instance.Map<User>(this.OnionSession));
    }

    public Task CreateAsync(User user)
    {
      // TODO: Create User implementieren
      throw new NotImplementedException();
    }

    public async Task DeleteAsync(User user)
    {
      var onionUser = Mapper.Instance.Map<Onion.Client.IUser>(user);
      await Task.Run(() => this.OnionSession.UserManagement.Users.Delete(onionUser));
    }

    /// <summary>
    ///     If disposing, calls dispose on the Context.  Always nulls out the Context
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
        this._session.Dispose();
      this._disposed = true;
    }

    #endregion

    #region Member IUserRoleStore
    public async Task<IList<string>> GetRolesAsync(User user)
    {
      return await Task.Run(() =>
      {
        var onionUser = this.OnionSession.UserManagement.Users.GetUser(user.Id);
        return onionUser.Groups
                        .Cast<IGroup>()
                        .Select(x => x.Name)
                        .Union(GetOnionUserRoles(onionUser))
                        .ToList();
      });
    }

    private IEnumerable<string> GetOnionUserRoles(Onion.Client.IUser user)
    {
      var result = new List<string>();

      if (user == null) return result;
      if (user.IsAdministrator) result.Add(UserRoles.Administrator.ToString());
      if (user.IsEditor) result.Add(UserRoles.Editor.ToString());
      if (user.IsLiveEditor) result.Add(UserRoles.LiveEditor.ToString());
      if (user.IsSchemaManager) result.Add(UserRoles.SchemaManager.ToString());
      if (user.IsUserManager) result.Add(UserRoles.UserManager.ToString());

      return result;
    }

    public async Task<bool> IsInRoleAsync(User user, string roleName)
    {
      if (user == null) { throw new ArgumentNullException(nameof(user)); }

      var roles = await this.GetRolesAsync(user);
      return roles.Contains(roleName);
    }


    public Task RemoveFromRoleAsync(User user, string roleName)
    {
      throw new NotImplementedException();
    }

    public Task AddToRoleAsync(User user, string roleName)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region Member IUserClaimStore

    public Task<IList<Claim>> GetClaimsAsync(User user)
    {
      throw new NotImplementedException();
    }

    public Task AddClaimAsync(User user, Claim claim)
    {
      throw new NotImplementedException();
    }

    public Task RemoveClaimAsync(User user, Claim claim)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region Member IUserEmailStore

    public Task SetEmailAsync(User user, string email)
    {
      throw new NotImplementedException();
    }

    public Task<string> GetEmailAsync(User user)
    {
      throw new NotImplementedException();
    }

    public Task<bool> GetEmailConfirmedAsync(User user)
    {
      throw new NotImplementedException();
    }

    public Task SetEmailConfirmedAsync(User user, bool confirmed)
    {
      throw new NotImplementedException();
    }

    public Task<User> FindByEmailAsync(string email)
    {
      throw new NotImplementedException();
    }

    #endregion

    public void UpdateSession(string username, string password)
    {
      this._session = new OnionSession(this._informationServerUri, username, password);
    }

    public void UpdateSession(string token)
    {
      this._session = new OnionSession(this._informationServerUri, token);
    }

    #endregion

    #region PROPERTIES

    private OnionSession OnionSession
    {
      get
      {
        if (this._session == null || this._session.IsInterrupted)
        {
          throw new OnionSessionNotInitialisedException();
        }
        return this._session;
      }
      set { this._session = value; }
    }

    public IQueryable<User> Users => (IQueryable<User>) _session?.UserManagement.Users.AsQueryable();

    #endregion

    #region Nested

    publ

    #endregion}
  }