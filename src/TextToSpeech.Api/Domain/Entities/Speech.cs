using System.Xml.Linq;

namespace TextToSpeech.Api.Domain.Entities
{
    public class Speech
    {
        public string Locale { get; set; }
        public Gender Gender { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public AudioOutputFormat OutputFormat { get; set; }

        public string SSML
        {
            get
            {
                var ssmlDoc = new XDocument(
                            new XElement("speak",
                                new XAttribute("version", "1.0"),
                                new XAttribute(XNamespace.Xml + "lang", "en-US"),
                                new XElement("voice",
                                    new XAttribute(XNamespace.Xml + "lang", this.Locale),
                                    new XAttribute(XNamespace.Xml + "gender", this.Gender.ToString()),
                                    new XAttribute("name", this.Name),
                                    this.Text)));
                return ssmlDoc.ToString();
            }
        }
    }
}
