using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace HIS.WebApi.Auth.Models.XMLModels
{
  public class XmlUri : IXmlSerializable
  {
    private Uri _Value;

    public XmlUri() { }
    public XmlUri(Uri source) { _Value = source; }

    public static implicit operator Uri(XmlUri o)
    {
      return o?._Value;
    }

    public static implicit operator XmlUri(Uri o)
    {
      return o == null ? null : new XmlUri(o);
    }

    public XmlSchema GetSchema()
    {
      return null;
    }

    public void ReadXml(XmlReader reader)
    {
      _Value = new Uri(reader.ReadElementContentAsString());
    }

    public void WriteXml(XmlWriter writer)
    {
      writer.WriteValue(_Value.ToString());
    }
  }
}