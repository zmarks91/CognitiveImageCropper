using Newtonsoft.Json;
using Umbraco.Core.PropertyEditors;

namespace UmbracoCognitive.App_Plugins.CognitiveImageCropper
{
    internal class CognitiveImageCropperConfiguration : ImageCropperConfiguration
    {
        [ConfigurationField("apiKey", "API Key", "textstring")]
        public string ApiKey { get; set; }
        
    }
}