using System;

namespace Infrasturcture.Global.Helpers.Events
{
    public class DataEventArgs: EventArgs
    {
        public object Data { get; set; }

        public DataEventArgs()
        {
            
        }

        public DataEventArgs(object data)
        {
            Data = data;
        }
    }
}
