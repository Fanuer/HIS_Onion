using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace HIS.WebApi.Auth.Models.XMLModels
{
  public class OnionModel
  {
    #region PROPERTIES
    /// <summary>
    /// Uri of the Onion Information Server
    /// </summary>
    [XmlAttribute("informationServerUrl")]
    public XmlUri InformationServerUri { get; set; }
    /// <summary>
    /// Onion Master User Name
    /// </summary>
    [XmlAttribute("username")]
    public string OnionUser { get; set; }
    /// <summary>
    /// Onion Master User Password
    /// </summary>
    [XmlAttribute("password")]
    public string OnionPassword { get; set; }
    #endregion
  }
}