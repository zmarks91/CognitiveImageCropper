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
        public int Left { get; set; }
        public int Top { get; set; }

        public virtual void Populate(BoundingRect areaOfInterest)
        {
            Left = areaOfInterest.X + (areaOfInterest.W / 2);
            Top = areaOfInterest.Y + (areaOfInterest.H / 2);
        }

    }
}
