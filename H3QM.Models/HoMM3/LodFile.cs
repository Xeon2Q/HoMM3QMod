using System;
using System.Linq;
using System.Text;

namespace H3QM.Models.HoMM3
{
    public class LodFile
    {
        #region C-tor & Properties

        private readonly Encoding _encoding;
        private readonly byte[] _name = new byte[16];
        private readonly byte[] _type = new byte[4];
        private readonly byte[] _offset = new byte[4];
        private readonly byte[] _originalSize = new byte[4];
        private readonly byte[] _compressedSize = new byte[4];
        private byte[] _originalContent = new byte[0];
        private byte[] _compressedContent = new byte[0];

        public LodFile(Encoding encoding, long position, byte[] name, byte[] type, byte[] offset, byte[] originalSize, byte[] compressedSize)
        {
            IsChanged = false;

            _encoding = encoding;
            Position = position;

            CopyBytes(name, _name, true);
            CopyBytes(type, _type);
            CopyBytes(offset, _offset);
            CopyBytes(originalSize, _originalSize);
            CopyBytes(compressedSize, _compressedSize);
        }

        #endregion

        #region Properties

        public bool IsChanged { get; private set; }

        public long Position { get; }

        public string Name
        {
            get => _encoding.GetString(_name.Where(q => q > 0).ToArray());
            set => CopyBytes(_encoding.GetBytes(value), _name);
        }

        public uint Type
        {
            get => BitConverter.ToUInt32(_type, 0);
            set => CopyBytes(BitConverter.GetBytes(value), _type);
        }

        public uint Offset
        {
            get => BitConverter.ToUInt32(_offset, 0);
            set => CopyBytes(BitConverter.GetBytes(value), _offset);
        }

        public uint OriginalSize
        {
            get => BitConverter.ToUInt32(_originalSize, 0);
            private set => CopyBytes(BitConverter.GetBytes(value), _originalSize);
        }

        public uint CompressedSize
        {
            get => BitConverter.ToUInt32(_compressedSize, 0);
            private set => CopyBytes(BitConverter.GetBytes(value), _compressedSize);
        }

        public string OriginalContent => _encoding.GetString(_originalContent);

        public string CompressedContent => _encoding.GetString(_compressedContent);

        #endregion

        #region Methods

        public byte[] GetNameBytes() => _name;

        public byte[] GetTypeBytes() => _type;

        public byte[] GetOffsetBytes() => _offset;

        public byte[] GetOriginalSizeBytes() => _originalSize;

        public byte[] GetOriginalContentBytes() => _originalContent;

        public byte[] GetCompressedSizeBytes() => _compressedSize;

        public byte[] GetCompressedContentBytes() => _compressedContent;

        public void SetContent(string originalContent, string compressedContent)
        {
            SetContent(_encoding.GetBytes(originalContent), _encoding.GetBytes(compressedContent));
        }

        public void SetContent(byte[] originalContent, byte[] compressedContent)
        {
            IsChanged = _compressedContent.Length > 0 && (!ArraysAreSame(_originalContent, originalContent) || !ArraysAreSame(compressedContent, _compressedContent));

            _originalContent = originalContent;
            _compressedContent = compressedContent;

            OriginalSize = (uint) originalContent.Length;
            CompressedSize = (uint) compressedContent.Length;

            // special case
            if (ArraysAreSame(originalContent, compressedContent)) CompressedSize = 0;
        }

        #endregion

        #region Private methods

        private static void CopyBytes(byte[] from, byte[] to, bool textData = false)
        {
            Array.Clear(to, 0, to.Length);
            var eof = textData ? Array.IndexOf(from, (byte)0) : from.Length;
            if (eof < 0) eof = from.Length;
            for (var i = 0; i < eof; i++) to[i] = from[i];
        }

        private bool ArraysAreSame(byte[] array1, byte[] array2)
        {
            if (array1 == null) throw new ArgumentNullException(nameof(array1));
            if (array2 == null) throw new ArgumentNullException(nameof(array2));
            if (array1.Length != array2.Length) return false;

            for (var i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i]) return false;
            }

            return true;
        }

        #endregion
    }
}