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
using System.Net.Http;

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

        [HttpPost]
        public async Task<FocalPoint> GetFocalPoint()
        {

            AdageMediaService service = new AdageMediaService();
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                StringBuilder sb = new StringBuilder(); // Holds the response body

                // Read the form data and return an async task.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names for uploaded files.
                foreach (var file in provider.FileData)
                {
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));


                    byte[] outputFileBytes = System.IO.File.ReadAllBytes(file.LocalFileName);
                    return await service.GetFocalPointByBytes(outputFileBytes);
                }

                throw new ApplicationException("Expected file");
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
    
}
