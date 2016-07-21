﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HIS.WebApi.Helper.Models
{
  [XmlRoot("data")]
  public class Keys
  {
    #region Field

    #endregion

    #region Properties

    [XmlElement("connectionString", typeof(ConnectionStringModel))]
    public List<ConnectionStringModel> ConnectionStrings { get; set; }
    #endregion

    #region Nested
    public class ConnectionStringModel
    {
      [XmlAttribute("name")]
      public string Name { get; set; }
      [XmlAttribute("connectionString")]
      public string ConnectionString { get; set; }
      [XmlAttribute("isProductive")]
      public bool IsProductive{ get; set; }
      [XmlAttribute("providerName")]
      public string ProviderName { get; set; }
    }
    #endregion
  }
}
