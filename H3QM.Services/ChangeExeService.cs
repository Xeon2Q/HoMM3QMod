using System;
using System.IO;
using System.Linq;
using System.Text;
using H3QM.Interfaces.Services;

namespace H3QM.Services
{
    public class ChangeExeService : IChangeExeService
    {
        #region Private fields

        private static readonly Encoding Encoding = Encoding.GetEncoding(1251);

        #endregion

        #region IChangeExeService implementation

        public bool ChangeHero(string exePath, string marker, byte[] originalHero, byte[] modifiedHero)
        {
            if (string.IsNullOrWhiteSpace(marker)) throw new ArgumentNullException(nameof(marker));
            if (originalHero == null || !originalHero.Any()) throw new ArgumentNullException(nameof(originalHero));
            if (modifiedHero == null) throw new ArgumentNullException(nameof(modifiedHero));
            if (originalHero.Length != modifiedHero.Length) throw new Exception("Hero templates are different length");

            var markerBytes = Encoding.GetBytes(marker);
            var buffer = new byte[markerBytes.Length * 100];
            using (var stream = File.Open(exePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                stream.Seek(0, SeekOrigin.Begin);

                long markerPosition = -1;
                long heroPosition = -1;
                while (true)
                {
                    var readBytes = stream.Read(buffer, 0, buffer.Length);
                    if (readBytes < 1) break;

                    if (markerPosition < 0)
                    {
                        markerPosition = GetBytesPosition(stream.Position, buffer, markerBytes);
                        // marker not found
                        if (markerPosition < 0)
                        {
                            // end of stream
                            if (stream.Position == stream.Length) break;

                            stream.Seek(-markerBytes.Length * 2, SeekOrigin.Current);
                            continue;
                        }
                    }

                    heroPosition = GetBytesPosition(stream.Position, buffer, originalHero);
                    if (heroPosition >= 0) break;
                }

                // hero not found
                if (heroPosition < 0) return false;

                stream.Seek(heroPosition, SeekOrigin.Begin);
                stream.Write(modifiedHero, 0, modifiedHero.Length);

                return true;
            }
        }

        #endregion

        #region Private methods

        private static long GetBytesPosition(long position, byte[] buffer, byte[] bytes)
        {
            if (buffer == null || !buffer.Any()) throw new ArgumentNullException(nameof(buffer));
            if (bytes == null || !bytes.Any()) throw new ArgumentNullException(nameof(bytes));

            long x = -1;
            for (var i = 0; i < buffer.Length - bytes.Length + 1; i++)
            {
                for (var j = 0; j < bytes.Length; j++)
                {
                    if (buffer[i + j] != bytes[j]) break;
                    if (j == bytes.Length - 1) x = i;
                }
                if (x >= 0) break;
            }

            return x < 0
                ? x
                : position - buffer.Length + x;
        }

        #endregion
    }
}