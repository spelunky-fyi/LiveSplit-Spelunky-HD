

using System.Xml;

namespace LiveSplit.Spelunky
{
  public static class XMLSettings
  {
    public static XmlElement ToElement<T>(XmlDocument doc, string name, T value)
    {
      XmlElement element = doc.CreateElement(name);
      element.InnerText = value.ToString();
      return element;
    }

    public static T Parse<T>(XmlElement element, T defaultValue, XMLSettings.ExceptingParser<T> p) => element != null ? p(element.InnerText) : defaultValue;

    public delegate T ExceptingParser<T>(string str);
  }
}
