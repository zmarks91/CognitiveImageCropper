using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;
using Umbraco.Core.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Umbraco.Web.Mvc;
using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace AdageCognitiveImageHandler
{
    public class AdageMediaService
    {
        private string _region;
        public virtual string Region {
            get
            {
                if (_region == null)
                    return ConfigurationManager.AppSettings["CognitiveImageCropper.AzureRegion"];
                else
                    return _region;
            }
            set { _region = value; } }

        private string _subscriptionKey;
        public virtual string SubscriptionKey {

            get {
                if (_subscriptionKey == null)
                    return ConfigurationManager.AppSettings["CognitiveImageCropper.AzureKey"];                    
                else
                    return _subscriptionKey;
            }
            set {
                _subscriptionKey = value;
            }
        }

        protected virtual ComputerVisionClient GetClient(string subscriptionKey, string region)
        {
            ComputerVisionClient computerVision = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(subscriptionKey),
                new System.Net.Http.DelegatingHandler[] { });

            computerVision.Endpoint = $"https://{region}.api.cognitive.microsoft.com";

            return computerVision;
        }

        public async Task<FocalPoint> GetFocalPoint(string imageUrl)
        {
            ComputerVisionClient computerVision = GetClient(SubscriptionKey, Region);
            AreaOfInterestResult result = await computerVision.GetAreaOfInterestAsync(imageUrl);

            FocalPoint point = new FocalPoint();
            point.Populate(result.AreaOfInterest);
            return point;
        }

        public async Task<FocalPoint> GetFocalPoint(Stream imageStream)
        {
            ComputerVisionClient computerVision = GetClient(SubscriptionKey, Region);
            AreaOfInterestResult result = await computerVision.GetAreaOfInterestInStreamAsync(imageStream);

            FocalPoint point = new FocalPoint();
            point.Populate(result.AreaOfInterest);
            return point;
        }

        public async Task<FocalPoint> GetFocalPoint(Umbraco.Core.Models.IMedia mediaFile)
        {
            //string localPath = HostingEnvironment.MapPath(url)
            //string path = Server.MapPath(mediaFile.Url);
            string path = "localpath";
            if (System.IO.File.Exists(path))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (Stream input = System.IO.File.OpenRead(path))
                    {
                        input.CopyTo(memoryStream);
                    }

                    memoryStream.Position = 0;

                    return await GetFocalPoint(memoryStream);
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
                if (focalPoint == null)
                {
                    umbracoFileJson.First.AddAfterSelf(new JProperty("focalPoint", JObject.Parse(@"{""left"": ""1"", ""top"": ""0.5""}")));
                    focalPoint = umbracoFileJson["focalPoint"];
                }

                focalPoint["left"] = "1";

                umbracoFile.SetValue(umbracoFileJson.ToString());
            }
        }
    }
}
