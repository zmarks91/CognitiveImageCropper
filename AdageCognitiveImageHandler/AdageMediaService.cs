using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Newtonsoft.Json.Linq;

namespace AdageCognitiveImageHandler
{
    public class AdageMediaService
    {
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
