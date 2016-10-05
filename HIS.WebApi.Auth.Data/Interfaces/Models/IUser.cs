using System.Collections.ObjectModel;
using System.Security.Claims;

namespace HIS.WebApi.Auth.Data.Interfaces.Models
{
    public interface IUser<TKey> : IEntity<TKey>
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR

        #endregion

        #region METHODS

        #endregion

        #region PROPERTIES
        string UserName { get; set; }
        string DisplayName { get; }
        Collection<Claim> AdditionalClaims { get; set; }

        #endregion

    }
}
