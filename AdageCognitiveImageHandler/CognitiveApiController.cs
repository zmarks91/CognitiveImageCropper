using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Umbraco.Web.Mvc;

namespace AdageCognitiveImageHandler
{
    /// <summary>
    /// URL: /umbraco/cognitiveimagecropper/cognitiveapi/
    /// </summary>
    [PluginController("CognitiveImageCropper")]
    public class CognitiveApiController : UmbracoApiController
    {   
        public async Task<FocalPoint> GetFocalPoint(string imageUrl, string subscriptionKey, string region)
        {
            //TODO: remove defaults
            if (string.IsNullOrEmpty(region))
                region = "northcentralus";

            //TODO: remove defaults
            if (string.IsNullOrEmpty(subscriptionKey))
                subscriptionKey = "04ede2d9fa174f61899dce82b1a6fbf4";

            ComputerVisionClient computerVision = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(subscriptionKey),
                new System.Net.Http.DelegatingHandler[] { });

            computerVision.Endpoint = $"https://{region}.api.cognitive.microsoft.com";
            AreaOfInterestResult result = await computerVision.GetAreaOfInterestAsync(imageUrl);
            FocalPoint point = new FocalPoint()
            {
                Left = result.AreaOfInterest.X + (result.AreaOfInterest.W / 2),
                Top = result.AreaOfInterest.Y + (result.AreaOfInterest.H / 2)
            };
            return point;
        }
    }
    
    public class FocalPoint
    {
        public int Left { get; set; }
        public int Top { get; set; }

    }
}
