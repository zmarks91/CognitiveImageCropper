using System.Collections.Generic;
using Umbraco.Core.PropertyEditors;

namespace UmbracoCognitive.App_Plugins.CognitiveImageCropper
{
    internal class CognitiveImageCropperConfigurationEditor : ConfigurationEditor<CognitiveImageCropperConfiguration>
    {

        /// <inheritdoc />
        public override IDictionary<string, object> ToValueEditor(object configuration)
        {
            var d = base.ToValueEditor(configuration);
            if (!d.ContainsKey("focalPoint")) d["focalPoint"] = new { left = 0.5, top = 0.5 };
            if (!d.ContainsKey("src")) d["src"] = "";
            return d;
        }
    }
}