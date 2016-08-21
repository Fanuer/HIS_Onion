using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Security.Claims;
using System.Web;
using HIS.WebApi.Auth.Base.Models;

namespace HIS.WebApi.Auth.Models
{
    public class OnionUser:User
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public OnionUser(int id) : base(id)
        {
        }

        public OnionUser(int id, string userName, string displayName = "", Collection<Claim> claims = null)
            : base(id, userName, displayName, claims)
        {
        }

        #endregion

        #region METHODS
        #endregion

        #region PROPERTIES
        internal string Password { get; set; }
        #endregion

    }
}