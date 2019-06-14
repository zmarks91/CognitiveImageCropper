using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdageCognitiveImageHandler
{
    public class FocalPoint
    {
        public float Left { get; set; }
        public float Top { get; set; }

        public virtual void Populate(AreaOfInterestResult areaOfInterestResult)
        {
            var areaOfInterest = areaOfInterestResult.AreaOfInterest;
            var metadata = areaOfInterestResult.Metadata;

            float x = areaOfInterest.X + (areaOfInterest.W / 2);
            float y = areaOfInterest.Y + (areaOfInterest.H / 2);

            Left = x / metadata.Width;
            Top = y / metadata.Height;
        }

    }
}
