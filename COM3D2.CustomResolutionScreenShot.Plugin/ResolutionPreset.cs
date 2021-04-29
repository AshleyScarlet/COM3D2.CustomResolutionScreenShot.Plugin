namespace COM3D2.CustomResolutionScreenShot.Plugin
{
    internal struct ResolutionPreset
    {
        public int Width;
        public int Height;
        public int DepthBuffer;

        public ResolutionPreset(int width, int height) : this(width, height, 24) { }

        public ResolutionPreset(int width, int height, int depthBuffer)
        {
            Width = width;
            Height = height;
            DepthBuffer = depthBuffer;
        }
    }
}
