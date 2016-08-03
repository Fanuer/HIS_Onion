using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Web;
using Onion.Server;

namespace HIS.WebApi.Auth.Exceptions
{
  public class OnionSessionNotInitialisedException: OnionDataRepositoryException
  {
    #region FIELDS
    #endregion

    #region CTOR

    public OnionSessionNotInitialisedException(string message ="Server session must be initialised to use it")
      :base(message)
    {
    }
    #endregion

    #region METHODS
    #endregion

    #region PROPERTIES
    #endregion
  }
}