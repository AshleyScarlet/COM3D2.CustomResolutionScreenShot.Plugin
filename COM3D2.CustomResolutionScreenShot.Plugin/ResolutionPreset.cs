using System;

namespace COM3D2.CustomResolutionScreenShot.Plugin
{
    internal struct ResolutionPreset
    {
        public int Width;
        public int Height;
        public int DepthBuffer;
        public string Name;

        public ResolutionPreset(int width, int height, string name) : this(width, height, 24, name) { }

        public ResolutionPreset(int width, int height, int depthBuffer, string name)
        {
            Width = width;
            Height = height;
            DepthBuffer = depthBuffer;
            Name = name;
        }

        public unsafe string GetResolutionText()
        {
            int widthCharCount = Util.CountDigits((uint)Width);
            int heightCharCount = Util.CountDigits((uint)Height);
            var buffer = stackalloc char[widthCharCount + 3 + heightCharCount + 1];
            var ptr = buffer;
            WriteInt32(ptr, widthCharCount, Width);
            ptr += widthCharCount;
            *ptr++ = ' ';
            *ptr++ = 'x';
            *ptr++ = ' ';
            WriteInt32(ptr, heightCharCount, Height);
            return new string(buffer);
        }

        private unsafe static void WriteInt32(char* buffer, int length, int value)
        {
            char* p = buffer + length;
            do
            {
                int remainder;
                value = Math.DivRem(value, 10, out remainder);
                *(--p) = (char)(remainder + '0');
            }
            while (value != 0);
        }
    }
}
