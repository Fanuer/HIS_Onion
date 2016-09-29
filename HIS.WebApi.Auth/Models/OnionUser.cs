using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace HIS.WebApi.Auth.Models
{
    public class OnionUser: Data.Models.User
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public OnionUser(int id) : base(id)
        {
            this.AdditionalClaims = new Collection<Claim>();
        }

        public OnionUser(int id, string userName, string displayName = "", Collection<Claim> claims = null)
            : base(id, userName, displayName)
        {
            this.AdditionalClaims = claims ?? new Collection<Claim>();
        }

        #endregion

        #region METHODS
        #endregion

        #region PROPERTIES
        internal string Password { get; set; }
        public Collection<Claim> AdditionalClaims { get; set; }
        #endregion
    }
}