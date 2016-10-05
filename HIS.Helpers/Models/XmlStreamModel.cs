using System.Xml;

namespace HIS.Helpers.Models
{
    public class XmlStreamModel : StreamModel
    {
        public XmlReader CreateXmlReader()
        {
            return XmlReader.Create(Stream);
        }
    }
}
