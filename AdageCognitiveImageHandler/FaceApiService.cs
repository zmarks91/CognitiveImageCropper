using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AdageCognitiveImageHandler
{
    public class FaceApiService
    {
        private string _region;
        public virtual string Region
        {
            get
            {
                if (_region == null)
                    return ConfigurationManager.AppSettings["CognitiveImageCropper.AzureRegion"];
                else
                    return _region;
            }
            set { _region = value; }
        }

        private string _subscriptionKey;
        public virtual string SubscriptionKey
        {

            get
            {
                if (_subscriptionKey == null)
                    return ConfigurationManager.AppSettings["CognitiveImageCropper.AzureFaceApiKey"];
                else
                    return _subscriptionKey;
            }
            set
            {
                _subscriptionKey = value;
            }
        }

        // NOTE: You must use the same region in your REST call as you used to
        // obtain your subscription keys. For example, if you obtained your
        // subscription keys from westus, replace "westcentralus" in the URL
        // below with "westus".
        //
        // Free trial subscription keys are generated in the "westus" region.
        // If you use a free trial subscription key, you shouldn't need to change
        // this region.
        public virtual string uriBase =>
            $"https://{Region}.api.cognitive.microsoft.com/face/v1.0/detect";

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        public virtual async Task<FocalPoint> GetFacePoint(JArray faceArray, int imageWidth, int imageHeight)
        {
            //TODO: get face with highest ["faceattributes"]["smile"] score

            var face = faceArray.First();
            int top = int.Parse(face["faceRectangle"]["top"].ToString());
            int left = int.Parse(face["faceRectangle"]["left"].ToString());
            int width = int.Parse(face["faceRectangle"]["width"].ToString());
            int height = int.Parse(face["faceRectangle"]["height"].ToString());
            FocalPoint point = new FocalPoint();
            point.Populate(left, top, width, height, imageWidth, imageHeight);
            return point;
        }

        // Gets the analysis of the specified image by using the Face REST API.
        public virtual async Task<FocalPoint> MakeAnalysisRequest(string imageFilePath)
        {
            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            return await MakeAnalysisRequestWithBytes(byteData);
        }

        public virtual async Task<FocalPoint> MakeAnalysisRequestWithBytes(byte[] byteData)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", SubscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
                "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;
            
            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();
                JArray jArray = JArray.Parse(contentString);

                using (MemoryStream memoryStream = new MemoryStream(byteData))
                {
                    using (System.Drawing.Image image = new System.Drawing.Bitmap(memoryStream))
                    {
                        return await GetFacePoint(jArray, image.Width, image.Height);
                    }
                 }
            }
        }
    }
}
