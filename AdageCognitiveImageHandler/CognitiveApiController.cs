using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;
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

        public async Task<FocalPoint> GetFocalPoint(Byte[] bytes)
        {
            AdageMediaService service = new AdageMediaService();
            return await service.GetFocalPointByBytes(bytes);
        }
    }
    
}
