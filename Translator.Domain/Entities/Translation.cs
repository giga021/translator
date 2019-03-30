using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Translator.Domain.Entities
{
	[XmlRoot("translation")]
	public class Translation
	{
		[XmlAttribute("id")]
		public long Id { get; set; }
		[XmlAttribute("timestamp")]
		public DateTime TimeStamp { get; set; }
		[XmlIgnore]
		public string FromLanguage { get; set; }
		[XmlIgnore]
		public string ToLanguage { get; set; }
		[XmlElement("from")]
		public string From { get; set; }
		[XmlElement("to")]
		public string To { get; set; }
	}
}
