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
using System.IO;
using System.Web.Hosting;

namespace AdageCognitiveImageHandler
{
    /// <summary>
    /// URL: /umbraco/cognitiveimagecropper/cognitiveapi/
    /// </summary>
    [PluginController("CognitiveImageCropper")]
    public class CognitiveApiController : UmbracoApiController
    {   
        

        public async Task<string> GetImageDescription(string imageUrl, string subscriptionKey, string region)
        {
            return "Not implemented";
        }

        public async Task<FocalPoint> GetFocalPoint(string imageUrl)
        {
            AdageMediaService service = new AdageMediaService();
            return await service.GetFocalPoint(imageUrl);
        }

        [HttpPost]
        public async Task<FocalPoint> GetFocalPoint(HttpPostedFile file)
        {
            AdageMediaService service = new AdageMediaService();
            var asyncInputStream = await Request.Content.ReadAsStreamAsync();
            var base64Encoded = await Request.Content.ReadAsStringAsync();
            byte[] imageData = System.Convert.FromBase64String(base64Encoded);
            return await service.GetFocalPoint(asyncInputStream);
        }
    }
    
}
