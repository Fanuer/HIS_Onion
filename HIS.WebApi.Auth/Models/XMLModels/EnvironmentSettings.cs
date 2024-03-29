﻿using System;
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
  internal class EnvironmentSettings
  {
    #region Field

    private static EnvironmentSettings _instance;
    #endregion

    #region Constructor
    private EnvironmentSettings()
    {
      
    }
    #endregion

    #region Methods

    private static EnvironmentSettings Create()
    {
      EnvironmentSettings result = null;
      string filename = Filename("App_Data", "EnvironmentSettings.xml");
      if (!File.Exists(filename))
      {
        throw new FileNotFoundException("EnvironmentSettings not found", filename);
      }

      using (var reader = new XmlTextReader(filename))
      {
        var serializer = new XmlSerializer(typeof(EnvironmentSettings));
        result = (EnvironmentSettings) serializer.Deserialize(reader);
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

    public static EnvironmentSettings Instance
    {
      get
      {
        if (_instance == null)
        {
          lock (typeof(EnvironmentSettings))
          {
            if (_instance == null)
            {
              _instance = EnvironmentSettings.Create();
            }
          }
        }
        return _instance;
      }
    }
    #endregion
  }
}
