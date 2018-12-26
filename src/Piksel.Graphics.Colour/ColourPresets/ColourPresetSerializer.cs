using Piksel.Graphics.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Piksel.Graphics.ColourPicker.ColourSwatches
{
    public static class ColourPresetSerializer
    {
        static Encoding Encoding => new UTF8Encoding(false);

        public static IEnumerable<ColourPreset> LoadPresets(string fileName)
        {
            using (var fs = File.Open(fileName, FileMode.Open))
            {
                foreach (var preset in LoadPresets(fs))
                    yield return preset;
            }
        }

        public static IEnumerable<ColourPreset> LoadPresets(Stream stream)
        {
            var parser = new StreamParser(stream, eatWhiteSpace: true);
            parser.EatUntil('#').ReadUntil('\n', out string header).Eat('\n');
            if (header != "# ColourPreset v1") yield break;

            while (!parser.ReachedEnd)
            {
                parser.Eat('"')
                    .ReadUntil('"', out string colour)
                    .Eat('"', '=', '"')
                    .ReadUntil('"', out string name)
                    .Eat('"', '\n');

                yield return new ColourPreset(Colour.FromString(colour), name);
            }
        }

        public static void SavePresets(IEnumerable<ColourPreset> presets, string fileName)
        {
            using (var fs = File.Open(fileName, FileMode.Create))
            {
                SavePresets(presets, fs);
            }
        }

        public static void SavePresets(IEnumerable<ColourPreset> presets, Stream stream)
        {
            using (var sr = new StreamWriter(stream, Encoding, 1024, true))
            {
                sr.WriteLine("# ColourPreset v1");
                foreach (var preset in presets)
                {
                    sr.WriteLine($"\"{preset.Colour.ToHex(HexPrefix.Hash, HexFormatAlpha.Auto)}\" = \"{preset.Name}\"");
                }
            }
        }

    }
}
