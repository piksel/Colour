using System.IO;
using System.Text;

namespace Piksel.Graphics.Utilities
{
    internal class StreamParser
    {
        byte[] buffer = new byte[1000];
        readonly char[] chars = new char[1024];
        int head = 0;
        int br;
        StringBuilder sb = new StringBuilder();
        private Stream stream;
        readonly Encoding encoding;
        readonly Decoder decoder;
        private readonly bool eatWhiteSpace;

        public bool ReachedEnd { get; internal set; }

        public StreamParser(Stream stream, bool eatWhiteSpace = false)
            : this(stream, new UTF8Encoding(false), eatWhiteSpace) { }

        public StreamParser(Stream stream, Encoding encoding, bool eatWhiteSpace = false)
        {
            this.stream = stream;
            this.encoding = encoding;
            this.decoder = encoding.GetDecoder();
            this.eatWhiteSpace = eatWhiteSpace;

            // Read the first byte immediately 
            br = stream.ReadByte();
        }

        private void Flush(bool final)
        {
            decoder.Convert(buffer, 0, head, chars, 0, 1024, final, out int bytesUsed, out int charsUsed, out bool completed);
            sb.Append(chars, 0, charsUsed);
            head = 0;
        }

        private bool IsWhiteSpace(char suspect)
            => suspect == ' ' || suspect == '\t';

        private bool IsTarget(char suspect, char target)
            => suspect == target || (target == '\n' && suspect == '\r');

        public StreamParser ReadUntil(char target, out string content)
        {
            while (br >= 0)
            {
                if (IsTarget((char)br, target)) break;
                buffer[head++] = (byte)br;
                if (head > buffer.Length)
                {
                    Flush(false);
                }
                br = stream.ReadByte();
            }
            ReachedEnd = br < 0;

            Flush(true);
            content = sb.ToString();
            sb.Clear();
            return this;

        }

        public StreamParser EatUntil(char target)
        {
            while (br >= 0)
            {
                if (IsTarget((char)br, target))
                {
                    break;
                }
                br = stream.ReadByte();
            }
            ReachedEnd = br < 0;
            return this;
        }

        public StreamParser Eat(params char[] targets)
        {
            foreach (var target in targets)
            {
                Eat(target);
            }
            return this;
        }

        public StreamParser Eat(char target)
        {
            while (br >= 0)
            {
                var cr = (char)br;
                if (!(IsTarget(cr, target) || (eatWhiteSpace && IsWhiteSpace(cr))))
                {
                    break;
                }
                br = stream.ReadByte();
            }
            ReachedEnd = br < 0;
            return this;
        }
    }
}
