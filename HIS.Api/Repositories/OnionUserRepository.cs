using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HIS.WebApi.Auth.Base.Interfaces;
using HIS.WebApi.Auth.Base.Interfaces.Repository;
using Onion.Client;

namespace HIS.WebApi.Auth.Repositories
{
  public class OnionUserRepository:IUserRepository<int>
  {
    #region FIELDS

    private const string APPSETTINGS_INFORMATIONSERVER = "informationServerUrl";
    private readonly string informationServerUrl;
    private string _token = "";
    
    #endregion

    #region CTOR

    public OnionUserRepository(string username, string password) : this()
    {
      if (String.IsNullOrWhiteSpace(username)) { throw new ArgumentNullException(nameof(username)); }
      if (String.IsNullOrWhiteSpace(password)) { throw new ArgumentNullException(nameof(password)); }

      using (var session = new OnionSession(this.informationServerUrl, username, password))
      {
        this._token = session.SessionToken;
      }
    }

    public OnionUserRepository(string token):this()
    {
      if (String.IsNullOrWhiteSpace(token)){ throw new ArgumentNullException(nameof(token));}
      using (var session = new OnionSession(this.informationServerUrl, token))
      {
        this._token = session.SessionToken;
      }
    }

    private  OnionUserRepository()
    {
      var url = ConfigurationManager.AppSettings.Get(APPSETTINGS_INFORMATIONSERVER);
      if (String.IsNullOrWhiteSpace(url))
      {
        throw new ArgumentNullException(APPSETTINGS_INFORMATIONSERVER, "No InformationServer-Url set in Appsettings");
      }
      informationServerUrl = url;
    }
    #endregion

    #region METHODS
    public async Task<IUser<int>> FindAsync(int id)
    {
      return await Task.Run(() =>
      {
        using (var session = new OnionSession())
        {
          var onionUser = session.UserManagement.Users.GetUser(id);
          return Mapper.Instance.Map<IUser<int>>(onionUser);
        }
      });
    }

    public Task<bool> AddAsync(IUser<int> model)
    {
      throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(int id)
    {
      throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(IUser<int> model)
    {
      throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(int id)
    {
      throw new NotImplementedException();
    }

    public Task<IQueryable<IUser<int>>> GetAllAsync()
    {
      throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(IUser<int> model)
    {
      throw new NotImplementedException();
    }

    #endregion

    #region PROPERTIES
    #endregion
  }
}