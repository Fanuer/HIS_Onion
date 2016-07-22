using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Serialization;
using HIS.Api.Models;
using HIS.WebApi.Helper.Interfaces.Repository;
using HIS.WebApi.Helper.Models;

namespace HIS.Api.Repositories
{
  public class ContextFactory
  {
    #region Field

    private static ContextFactory _instance;
    #endregion

    #region Ctor
    private ContextFactory()
    {
      
    }
    #endregion

    #region Methods
    public AuthContext CreateContext()
    {
      var filename = this.Filename("App_Data", "Keys.xml");
      if (!File.Exists(filename))
      {
        throw new FileNotFoundException("Keys.xml was not found", filename);
      }
      using (var reader = XmlReader.Create(new StreamReader(filename)))
      {
        var serializer = new XmlSerializer(typeof(Keys));
        var key = (Keys)serializer.Deserialize(reader);
        var connString = key.ConnectionStrings.First(x => x.Name.Equals("AuthContext")).ConnectionString;
        return new AuthContext(connString);
      }
    }

    

    private string Filename(params string[] names)
    {
      return HostingEnvironment.ApplicationPhysicalPath + String.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), names);
    }
    #endregion

    #region Properties

    public static ContextFactory Instance
    {
      get
      {
        if (ContextFactory._instance == null)
        {
          lock (typeof(ContextFactory))
          {
            if (_instance == null)
            {
              _instance = new ContextFactory();
            }
          }
        }
        return ContextFactory._instance;
      }
    }
    #endregion
  }
}
