namespace HIS.WebApi.Auth.Base.Provider
{
    public class OnionOAuthProvider:CustomOAuthProvider
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public OnionOAuthProvider(IBearerTokenUserService<IUser<string>> repository) : base(repository)
        {
        }
        #endregion

        #region METHODS

        public override void AddAdditionalClaims(ClaimsIdentity authClaims, OAuthGrantResourceOwnerCredentialsContext context)
        {
            base.AddAdditionalClaims(authClaims, context);
            if (authClaims != null && context != null)
            {
                var session = new OnionSession(EnvironmentSettings.Instance.OnionData.InformationServerUri, context.UserName, context.Password);
                authClaims.AddClaim(new Claim("onionSession", session.SessionToken));
            }
        }

        #endregion

        #region PROPERTIES
        #endregion


    }
}
