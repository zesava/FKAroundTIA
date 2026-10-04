using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FKAroundTIA.Services
{
    public class ProjectTextService
    {
        private const string ProjectTextNamespace =
            "http://www.siemens.com/Industry/2009/10/01/Automation/FormattedText";

        public bool TryGetPlainText(string value, out string plainText)
        {
            plainText = null;
            if (!TryParse(value, out var document))
            {
                return false;
            }

            XNamespace xmlNamespace = ProjectTextNamespace;
            var paragraphs = document
                .Descendants(xmlNamespace + "p")
                .Select(paragraph => paragraph.Value)
                .ToList();

            plainText = string.Join(Environment.NewLine, paragraphs);
            return true;
        }

        public string UpdatePlainText(string originalValue, string plainText)
        {
            if (!TryParse(originalValue, out var document))
            {
                throw new FormatException("The value is not a valid Siemens ProjectText document.");
            }

            XNamespace xmlNamespace = ProjectTextNamespace;
            var body = document.Root?.Element(xmlNamespace + "body");
            if (body == null)
            {
                throw new FormatException("The Siemens ProjectText document does not contain a body element.");
            }

            body.RemoveNodes();
            string normalizedText = (plainText ?? "").Replace("\r\n", "\n").Replace('\r', '\n');
            foreach (string line in normalizedText.Split(new[] { '\n' }, StringSplitOptions.None))
            {
                body.Add(new XElement(xmlNamespace + "p", line));
            }

            using (var writer = new Utf8StringWriter())
            {
                document.Save(writer, SaveOptions.DisableFormatting);
                return writer.ToString();
            }
        }

        private static bool TryParse(string value, out XDocument document)
        {
            document = null;
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            try
            {
                document = XDocument.Parse(value, LoadOptions.PreserveWhitespace);
                return document.Root != null
                    && document.Root.Name.LocalName == "ProjectText"
                    && document.Root.Name.NamespaceName == ProjectTextNamespace;
            }
            catch
            {
                return false;
            }
        }

        private sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
