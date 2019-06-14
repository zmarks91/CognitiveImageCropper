using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Microsoft.Rest;

namespace AdageCognitiveImageHandler
{
    public class AdageMediaService
    {
        public async Task<FocalPoint> GetFocalPointByBytes(Byte[] bytes)
        {
            FaceApiService service = new FaceApiService();
            var response = await service.MakeAnalysisRequestWithBytes(bytes);
            return response;
        }

        public async Task<FocalPoint> GetFocalPointByLocalPath(string localPath)
        {
            var fullLocalPath = HostingEnvironment.MapPath(localPath);
            if (System.IO.File.Exists(fullLocalPath))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (Stream input = System.IO.File.OpenRead(fullLocalPath))
                    {
                        input.CopyTo(memoryStream);                        
                    }

                    memoryStream.Position = 0;

                    var bytes = memoryStream.GetBuffer();
                    return await GetFocalPointByBytes(bytes);
                }
            }
            else
            {
                throw new ApplicationException("File does not exist.");
            }
        }

        public virtual void HandleImageUpload(IMedia mediaFile)
        {
            var umbracoFile = mediaFile.Properties["umbracoFile"];
            if (umbracoFile != null)
            {                
                var umbracoFileValue = umbracoFile.GetValue();
                var umbracoFileJson = JObject.Parse(umbracoFileValue.ToString());
                var focalPoint = umbracoFileJson["focalPoint"];

                string localFilePath = umbracoFileJson["src"].ToString();

                FocalPoint autoFocalPoint = Task.Run<FocalPoint>(async () => await GetFocalPointByLocalPath(localFilePath)).Result;

                var jsonString = string.Format(@"{{""left"": ""{0}"", ""top"": ""{1}""}}", autoFocalPoint.Left, autoFocalPoint.Top);

                if (focalPoint == null)
                {   
                    umbracoFileJson.First.AddAfterSelf(new JProperty("focalPoint", JObject.Parse(jsonString)));
                    focalPoint = umbracoFileJson["focalPoint"];
                }
                else
                {
                    focalPoint["left"] = autoFocalPoint.Left.ToString();
                    focalPoint["top"] = autoFocalPoint.Top.ToString();
                }

                umbracoFile.SetValue(umbracoFileJson.ToString());
            }            
        }
    }
}
