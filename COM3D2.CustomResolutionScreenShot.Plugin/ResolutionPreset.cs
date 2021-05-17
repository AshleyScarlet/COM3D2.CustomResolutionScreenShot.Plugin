using System;

#pragma warning disable 0660
#pragma warning disable 0661

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
            var bufferLength = widthCharCount + 3 + heightCharCount;
            var buffer = stackalloc char[bufferLength];
            var ptr = buffer;
            WriteInt32(ptr, widthCharCount, Width);
            ptr += widthCharCount;
            *ptr++ = ' ';
            *ptr++ = 'x';
            *ptr++ = ' ';
            WriteInt32(ptr, heightCharCount, Height);
            return new string(buffer, 0, bufferLength);
        }

        public unsafe string GetAspectRatioText()
        {
            var width = Width;
            var height = Height;
            var gcd = Gcd(width, height);
            width /= gcd;
            height /= gcd;

            int widthCharCount = Util.CountDigits((uint)width);
            int heightCharCount = Util.CountDigits((uint)height);
            var bufferLength = widthCharCount + 1 + heightCharCount;
            var buffer = stackalloc char[bufferLength];
            var ptr = buffer;
            WriteInt32(ptr, widthCharCount, width);
            ptr += widthCharCount;
            *ptr++ = ':';
            WriteInt32(ptr, heightCharCount, height);
            return new string(buffer, 0, bufferLength);
        }

        private static int Gcd(int left, int right)
        {
            while (right != 0)
            {
                int temp = left % right;
                left = right;
                right = temp;
            }

            return left;
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

        public static bool operator ==(ResolutionPreset left, ResolutionPreset right) => 
            left.Width == right.Width && left.Height == right.Height &&
            left.DepthBuffer == right.DepthBuffer && left.Name == right.Name;

        public static bool operator !=(ResolutionPreset left, ResolutionPreset right) => !(left == right);
    }
}
