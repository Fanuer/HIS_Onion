using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HIS.WebApi.Auth.Data.Interfaces.Repository;
using HIS.WebApi.Auth.V2.Exceptions;
using HIS.WebApi.Auth.V2.Models;
using Microsoft.AspNetCore.Identity;
using Onion.Client;

namespace HIS.WebApi.Auth.V2.Service
{
    public class OnionUserStore : IUserRoleStore<OnionUser>
    {
        #region FIELDS

        private bool _disposed = false;
        private IOnionSession _masterSession;

        #endregion

        #region CTOR

        public OnionUserStore(Uri informationServerUri, string username, string userpassword)
        {
            if (informationServerUri == null)
            {
                throw new ArgumentNullException(nameof(informationServerUri));
            }
            if (String.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (String.IsNullOrWhiteSpace(userpassword))
            {
                throw new ArgumentNullException(nameof(userpassword));
            }
            this.OnionMasterSession = new OnionSession(informationServerUri, username, userpassword);
        }

        public OnionUserStore(IOnionSession masterSession)
        {
            if (masterSession == null)
            {
                throw new ArgumentNullException(nameof(masterSession));
            }
            this.OnionMasterSession = masterSession;
        }

        #endregion

        #region METHODS

        #region Members IUserStore

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object) this);
        }

        public async Task<OnionUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return
                await
                    Task.Run(
                        () =>
                            Mapper.Instance.Map<OnionUser>(
                                this.OnionMasterSession.UserManagement.Users.TryGetUser(userId)));
        }

        public async Task<OnionUser> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            return
                await
                    Task.Run(
                        () =>
                            Mapper.Instance.Map<OnionUser>(
                                this.OnionMasterSession.UserManagement.Users.TryGetUser(userName)));
        }

        public async Task<IdentityResult> UpdateAsync(OnionUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            await Task.Run(() =>
            {
                var onionUser = this.OnionMasterSession.UserManagement.Users.TryGetUser(user.Id);
                onionUser.DisplayName = user.DisplayName;
                onionUser.Name = user.UserName;
            }, cancellationToken);

            return new IdentityResult();
        }

        public async Task<IdentityResult> CreateAsync(OnionUser user, CancellationToken cancellationToken)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            await Task.Run(async () =>
            {
                this.OnionMasterSession.UserManagement.Users.Create(user.UserName, user.Password);
                await this.UpdateAsync(user, cancellationToken);
            }, cancellationToken);

            return new IdentityResult();
        }

        public async Task DeleteAsync(OnionUser user, CancellationToken cancellationToken)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            var onionUser = Mapper.Instance.Map<Onion.Client.IUser>(user);
            await Task.Run(() => this.OnionMasterSession.UserManagement.Users.Delete(onionUser), cancellationToken);
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
        public async Task<IList<string>> GetRolesAsync(OnionUser user, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var onionUser = this.OnionMasterSession.UserManagement.Users.GetUser(user.Id);
                return onionUser.Groups
                          .Cast<IGroup>()
                          .Select(x => x.Name)
                          .Union(GetOnionUserRoles(onionUser))
                          .ToList();
            }, cancellationToken);
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

        public async Task<bool> IsInRoleAsync(OnionUser user, string roleName, CancellationToken cancellationToken)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            var roles = await this.GetRolesAsync(user, cancellationToken);
            return roles.Contains(roleName);
        }

        public async Task RemoveFromRoleAsync(OnionUser user, string roleName, CancellationToken cancellationToken)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (String.IsNullOrWhiteSpace(roleName)) { throw new ArgumentNullException(nameof(roleName)); }
            await Task.Run(() => JoinLeaveGroup(user.Id, roleName, false), cancellationToken);
        }

        public async Task AddToRoleAsync(OnionUser user, string roleName, CancellationToken cancellationToken)
        {
            if (user == null) { throw new ArgumentNullException(nameof(user)); }
            if (String.IsNullOrWhiteSpace(roleName)) { throw new ArgumentNullException(nameof(roleName)); }
            await Task.Run(() => JoinLeaveGroup(user.Id, roleName, true), cancellationToken);
        }

        private void JoinLeaveGroup(int userId, string roleName, bool join)
        {
            var onionUser = this.OnionMasterSession.UserManagement.Users.GetUser(userId);

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

        public async Task<IList<OnionUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            if (String.IsNullOrWhiteSpace(roleName)) { throw new ArgumentNullException(nameof(roleName)); }
            return await Task.Run(() =>
            {
                var role = this.OnionMasterSession.UserManagement.Groups.GetGroup(roleName);
                // TODO: Mappen
                return role.Members.Cast<OnionUser>().ToList();
            }, cancellationToken);
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

        public IQueryable<OnionUser> Users => (IQueryable<OnionUser>)OnionMasterSession?.UserManagement.Users.AsQueryable();

        #endregion

        public Task<string> GetUserIdAsync(OnionUser user, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => user.Id.ToString(), cancellationToken);
        }

        public async Task<string> GetUserNameAsync(OnionUser user, CancellationToken cancellationToken)
        {
            return await Task.Factory.StartNew(() =>
            {
                var onionUser = this.OnionMasterSession.UserManagement.Users.GetUser(user.Id.ToString());
                return onionUser.Name;
            }, cancellationToken);
        }

        public Task SetUserNameAsync(OnionUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task GetNormalizedUserNameAsync(OnionUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(OnionUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task GetClaimsAsync(OnionUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AddClaimsAsync(OnionUser user, IEnumerable claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(OnionUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(OnionUser user, IEnumerable claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}