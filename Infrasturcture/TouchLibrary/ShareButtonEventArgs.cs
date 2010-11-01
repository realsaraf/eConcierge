using System;

namespace Infrasturcture.TouchLibrary
{
    public class ShareButtonEventArgs : EventArgs
    {
        public string FilePath { get; set; }
        public MediaType MediaType { get; set; }
    }
}
