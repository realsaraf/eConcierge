using System.Collections.Generic;

namespace Helpers
{
    public class DirectionSteps
    {
        public string TotalDuration { get; set; }

        public string TotalDistance { get; set; }

        public string OriginAddress { get; set; }

        public string DestinationAddress { get; set; }

        public List<DirectionStep> Steps { get; set; }
    }
}