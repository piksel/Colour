using Piksel.Graphics.ColourPicker.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;

namespace Piksel.Graphics.ColourPicker.ColourSwatches
{

    /// <summary>
    /// Reads and writes Colour swatch data.
    /// </summary>

    internal class ColourSwatchXml
    {

        /// <summary>
        /// Private constructor to prevent the compiler from automatically 
        /// creating a default public constructor.
        /// </summary>

        private ColourSwatchXml() { }

        /// <summary>
        /// Reads Colour swatches.
        /// </summary>
        /// <param name="file">The path to the XML file containing the swatch 
        /// data.</param>
        /// <param name="isResourceFile">A boolean value indicating whether or 
        /// not the file is to be loaded from the assembly.</param>
        /// <returns>A list of Colour swatches.</returns>

        internal static IEnumerable<ColourPreset> ReadPresets(string file, bool isResourceFile)
        {


            using (var xmlReader = isResourceFile
                ? new XmlTextReader(Resources.GetFileResource<ColourSwatchXml>(file))
                : new XmlTextReader(file))
            {

                byte r = 0;
                byte g = 0;
                byte b = 0;
                bool insideColourElement = false;

                while (xmlReader.Read())
                {

                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals("Colour", StringComparison.InvariantCultureIgnoreCase))
                    {

                        byte.TryParse(xmlReader.GetAttribute("red"), out r);
                        byte.TryParse(xmlReader.GetAttribute("green"), out g);
                        byte.TryParse(xmlReader.GetAttribute("blue"), out b);
                        insideColourElement = true;

                    }
                    else if (xmlReader.NodeType == XmlNodeType.Text && insideColourElement)
                    {

                        Colour c = Color.FromArgb(r, g, b);
                        string description = xmlReader.ReadString();
                        yield return new ColourPreset(c, description);
                        insideColourElement = false;

                    }


                }

            }


        }

        /// <summary>
        /// Writes custom Colour swatches to the file system.
        /// </summary>
        /// <param name="file">The name of the file.</param>
        /// <param name="Colours">A list of the custom Colour 
        /// swatches.</param>

        internal static void WriteSwatches(string file, IEnumerable<ColourPreset> colourSwatches)
        {

            using (var xmlWriter = new XmlTextWriter(file, Encoding.UTF8))
            {


                xmlWriter.Formatting = Formatting.Indented;

                xmlWriter.WriteStartDocument(false);
                xmlWriter.WriteStartElement("swatches");
                xmlWriter.WriteStartElement("swatch");
                xmlWriter.WriteAttributeString("id", "CustomSwatches");
                xmlWriter.WriteStartElement("Colours");

                foreach (var cs in colourSwatches)
                {

                    xmlWriter.WriteStartElement("Colour");
                    xmlWriter.WriteAttributeString("red", cs.Colour.Red.ToString());
                    xmlWriter.WriteAttributeString("green", cs.Colour.Green.ToString());
                    xmlWriter.WriteAttributeString("blue", cs.Colour.Blue.ToString());
                    xmlWriter.WriteString(cs.Name);
                    xmlWriter.WriteEndElement();

                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndDocument();
            }

        }

    }

}
