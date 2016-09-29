using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HIS.WebApi.Auth.Exceptions;
using HIS.WebApi.Auth.Models;
using Microsoft.AspNet.Identity;
using Onion.Client;

namespace HIS.WebApi.Auth.Repositories
{
    public class OnionUserStore
    : IUserStore<OnionUser, int>,
     IUserRoleStore<OnionUser, int>,
     IQueryableUserStore<OnionUser, int>,
     IUserClaimStore<OnionUser, int>
  {
    #region FIELDS
    private bool _disposed = false;
    private IOnionSession _masterSession;
    #endregion

    #region CTOR

    public OnionUserStore(Uri informationServerUri, string username, string userpassword)
    {
      if (informationServerUri == null) { throw new ArgumentNullException(nameof(informationServerUri)); }
      if (String.IsNullOrWhiteSpace(username)) { throw new ArgumentNullException(nameof(username)); }
      if (String.IsNullOrWhiteSpace(userpassword)) { throw new ArgumentNullException(nameof(userpassword)); }
      this.OnionMasterSession = new OnionSession(informationServerUri, username, userpassword);
    }

    public OnionUserStore(IOnionSession masterSession)
    {
      if (masterSession == null) { throw new ArgumentNullException(nameof(masterSession)); }
      this.OnionMasterSession = masterSession;
    }
    #endregion

    #region METHODS
    #region Members IUserStore
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object)this);
    }

    public async Task<OnionUser> FindByIdAsync(int userId)
    {
      return await Task.Run(() => Mapper.Instance.Map<OnionUser>(this.OnionMasterSession.UserManagement.Users.TryGetUser(userId)));
    }

    public async Task<OnionUser> FindByNameAsync(string userName)
    {
      return await Task.Run(() => Mapper.Instance.Map<OnionUser>(this.OnionMasterSession.UserManagement.Users.TryGetUser(userName)));
    }
    public async Task UpdateAsync(OnionUser user)
    {
      if (user==null){throw new ArgumentNullException(nameof(user));}
      await Task.Run(() =>
      {
        var onionUser = this.OnionMasterSession.UserManagement.Users.TryGetUser(user.Id);
        onionUser.DisplayName = user.DisplayName;
        onionUser.Name = user.UserName;
      });
    }

    public async Task CreateAsync(OnionUser user)
    {
      if (user == null) { throw new ArgumentNullException(nameof(user)); }

      await Task.Run(async () =>
      {
        this.OnionMasterSession.UserManagement.Users.Create(user.UserName, user.Password);
        await this.UpdateAsync(user);
      });
    }

    public async Task DeleteAsync(OnionUser user)
    {
      if (user == null) { throw new ArgumentNullException(nameof(user)); }

      var onionUser = Mapper.Instance.Map<Onion.Client.IUser>(user);
      await Task.Run(() => this.OnionMasterSession.UserManagement.Users.Delete(onionUser));
    }

    /// <summary>
    ///     If disposing, calls dispose on the Context.  Always nulls out the Context
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
        this._masterSession.Dispose();
      this._disposed = true;
    }

    #endregion

    #region Member IUserRoleStore
    public async Task<IList<string>> GetRolesAsync(OnionUser user)
    {
      return await Task.Run(() =>
      {
        var onionUser = this.OnionMasterSession.UserManagement.Users.GetUser(user.Id);
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

    public async Task<bool> IsInRoleAsync(OnionUser user, string roleName)
    {
      if (user == null) { throw new ArgumentNullException(nameof(user)); }

      var roles = await this.GetRolesAsync(user);
      return roles.Contains(roleName);
    }

    public async Task RemoveFromRoleAsync(OnionUser user, string roleName)
    {
      if (user == null) { throw new ArgumentNullException(nameof(user)); }
      if (String.IsNullOrWhiteSpace(roleName)) { throw new ArgumentNullException(nameof(roleName)); }
      await Task.Run(() => JoinLeaveGroup(user.Id, roleName, false));
    }

    public async Task AddToRoleAsync(OnionUser user, string roleName)
    {
      if (user == null) { throw new ArgumentNullException(nameof(user)); }
      if (String.IsNullOrWhiteSpace(roleName)) { throw new ArgumentNullException(nameof(roleName)); }
      await Task.Run(() => JoinLeaveGroup(user.Id, roleName, true));

    }

    private void JoinLeaveGroup(int userID, string roleName, bool join)
    {
      var onionUser = this.OnionMasterSession.UserManagement.Users.GetUser(userID);

      if (onionUser == null) { throw new ArgumentNullException(nameof(onionUser)); }
      if (String.IsNullOrWhiteSpace(roleName)) { throw new ArgumentNullException(nameof(roleName)); }

      var role = UserRoles.None;

      if (UserRoles.TryParse(roleName, true, out role))
      {
        switch (role)
        {
          case UserRoles.Administrator:
            onionUser.IsAdministrator = join;
            break;
          case UserRoles.SchemaManager:
            onionUser.IsSchemaManager = join;
            break;
          case UserRoles.UserManager:
            onionUser.IsUserManager = join;
            break;
          case UserRoles.Editor:
            onionUser.IsEditor = join;
            break;
          case UserRoles.LiveEditor:
            onionUser.IsLiveEditor = join;
            break;
        }
      }
      else
      {
        var group = OnionMasterSession.UserManagement.Groups.TryGetGroup(roleName);
        if (group != null)
        {
          if (join)
          {
            onionUser.Groups.Join(group);
          }
          else
          {
            onionUser.Groups.Leave(group);
          }
          
        }
      }

    }

    #endregion

    #region Member IUserClaimStore

    public async Task<IList<Claim>> GetClaimsAsync(OnionUser user)
    {
      return await Task.Run(() =>
      {
          var addClaims = user.AdditionalClaims;
          var identityClaims = user.Claims.Select(x => new Claim(x.ClaimType, x.ClaimValue));
          return addClaims.Union(identityClaims).ToList();
      });
    }

    public async Task AddClaimAsync(OnionUser user, Claim claim)
    {
      if (user == null) { throw new ArgumentNullException(nameof(user)); }
      if (claim == null) { throw new ArgumentNullException(nameof(claim)); }

      await Task.Run(() => user.AdditionalClaims.Add(claim));
    }

    public async Task RemoveClaimAsync(OnionUser user, Claim claim)
    {
      if (user == null) { throw new ArgumentNullException(nameof(user)); }
      if (claim == null) { throw new ArgumentNullException(nameof(claim)); }

      await Task.Run(() => user.AdditionalClaims.Remove(claim));
    }

    #endregion

    #endregion

    #region PROPERTIES

    private IOnionSession OnionMasterSession
    {
      get
      {
        if (this._masterSession == null || this._masterSession.IsInterrupted)
        {
          throw new OnionSessionNotInitialisedException();
        }
        return this._masterSession;
      }
      set { this._masterSession = value; }
    }

    public IQueryable<OnionUser> Users => (IQueryable<OnionUser>) OnionMasterSession?.UserManagement.Users.AsQueryable();

    #endregion
  }
}