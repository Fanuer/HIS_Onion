using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using log4net.Util;

namespace HIS.WebApi.Auth.Models.XMLModels
{
  [XmlRoot("enviromentSettings")]
  internal class EnviromentSettings
  {
    #region Field

    private static EnviromentSettings _instance;
    #endregion

    #region Constructor
    private EnviromentSettings()
    {
      
    }
    #endregion

    #region Methods

    private static EnviromentSettings Create()
    {
      EnviromentSettings result = null;
      string filename = Filename("App_Data", "EnviromentSettings.xml");
      if (!File.Exists(filename))
      {
        throw new FileNotFoundException("EnvironmentSettings not fount", filename);
      }

      using (var reader = new XmlTextReader(filename))
      {
        var serializer = new XmlSerializer(typeof(EnviromentSettings));
        result = (EnviromentSettings) serializer.Deserialize(reader);
      }

      return result;
    }

    private static string Filename(params string[] names)
    {
      return HostingEnvironment.ApplicationPhysicalPath + String.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), names);
    }
    #endregion

    #region Properties

    [XmlElement("onion", typeof(OnionModel))]
    public OnionModel OnionData { get; set; }

    public EnviromentSettings Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (typeof(EnviromentSettings))
          {
            if (_instance == null)
            {
              _instance = EnviromentSettings.Create();
            }
          }
        }
        return _instance;
      }
    }
    #endregion
  }
}
