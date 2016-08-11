using System;
using System.Collections;
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
using IUser = Onion.Client.IUser;

namespace HIS.WebApi.Auth.Repositories
{
  public class OnionUserStore
    : IUserStore<User, int>,
     IUserRoleStore<User, int>,
     IQueryableUserStore<User, int>,
     IUserClaimStore<User, int>
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

    public async Task<User> FindByIdAsync(int userId)
    {
      return await Task.Run(() => Mapper.Instance.Map<User>(this.OnionMasterSession.UserManagement.Users.TryGetUser(userId)));
    }

    public async Task<User> FindByNameAsync(string userName)
    {
      return await Task.Run(() => Mapper.Instance.Map<User>(this.OnionMasterSession.UserManagement.Users.TryGetUser(userName)));
    }
    public async Task UpdateAsync(User user)
    {
      if (user==null){throw new ArgumentNullException(nameof(user));}
      await Task.Run(() =>
      {
        var onionUser = this.OnionMasterSession.UserManagement.Users.TryGetUser(user.Id);
        onionUser.DisplayName = user.DisplayName;
        onionUser.Name = user.UserName;
      });
    }

    public async Task CreateAsync(User user)
    {
      if (user == null) { throw new ArgumentNullException(nameof(user)); }

      await Task.Run(async () =>
      {
        this.OnionMasterSession.UserManagement.Users.Create(user.UserName, user.Password);
        await this.UpdateAsync(user);
      });
    }

    public async Task DeleteAsync(User user)
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
    public async Task<IList<string>> GetRolesAsync(User user)
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

    public async Task<bool> IsInRoleAsync(User user, string roleName)
    {
      if (user == null) { throw new ArgumentNullException(nameof(user)); }

      var roles = await this.GetRolesAsync(user);
      return roles.Contains(roleName);
    }

    public async Task RemoveFromRoleAsync(User user, string roleName)
    {
      if (user == null) { throw new ArgumentNullException(nameof(user)); }
      if (String.IsNullOrWhiteSpace(roleName)) { throw new ArgumentNullException(nameof(roleName)); }
      await Task.Run(() => JoinLeaveGroup(user.Id, roleName, false));
    }

    public async Task AddToRoleAsync(User user, string roleName)
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

    public async Task<IList<Claim>> GetClaimsAsync(User user)
    {
      return await Task.Run(() => user.Claims.ToList());
    }

    public async Task AddClaimAsync(User user, Claim claim)
    {
      if (user == null) { throw new ArgumentNullException(nameof(user)); }
      if (claim == null) { throw new ArgumentNullException(nameof(claim)); }

      await Task.Run(() => user.Claims.Add(claim));
    }

    public async Task RemoveClaimAsync(User user, Claim claim)
    {
      if (user == null) { throw new ArgumentNullException(nameof(user)); }
      if (claim == null) { throw new ArgumentNullException(nameof(claim)); }

      await Task.Run(() => user.Claims.Remove(claim));
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

    public IQueryable<User> Users => (IQueryable<User>) OnionMasterSession?.UserManagement.Users.AsQueryable();

    #endregion
  }
}