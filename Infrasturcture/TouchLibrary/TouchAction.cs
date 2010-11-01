namespace Infrasturcture.TouchLibrary
{
    /// <summary>
    /// Bitwise enum to allow simple storage of multi-options in one var
    /// and easy comparison of supported actions.
    /// </summary>
    public enum TouchAction
    {
        None = 1,
        Move = 2,
        Resize = 4,
        Rotate = 8,
        Flick = 16,
        Spin = 32,
        SelectToFront = 64,
        Tap = 128,
        Slide = 256,
        ScrollX = 512,
        ScrollY = 1024,
        Drag = 2048,
        BoundsCheck = 4096
    }
}