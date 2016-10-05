using Onion.Server;

namespace HIS.WebApi.Auth.V2.Exceptions
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