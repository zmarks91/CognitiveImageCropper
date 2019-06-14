using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using Umbraco.Core.Media;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.PropertyEditors.ValueConverters;
using Umbraco.Core.Services;
using Umbraco.Web.Media;

namespace UmbracoCognitive.App_Plugins.CognitiveImageCropper
{
    /// <summary>
    /// Represents an image cropper property editor.
    /// </summary>
    [DataEditor("Adage.CognitiveImageCropper", "Adage Cognitive Image Cropper", "~/app_plugins/CognitiveImageCropper/cognitiveimagecropper.html", ValueType = ValueTypes.Json, HideLabel = false, Group = "media", Icon = "icon-crop")]
    public class CognitiveImageCropperPropertyEditor : Umbraco.Web.PropertyEditors.ImageCropperPropertyEditor
    {
        public CognitiveImageCropperPropertyEditor(ILogger logger, IMediaFileSystem mediaFileSystem, IContentSection contentSettings, IDataTypeService dataTypeService)
            : base(logger, mediaFileSystem, contentSettings, dataTypeService)
        {
            
        }

        /// <summary>
        /// Creates the corresponding preValue editor.
        /// </summary>
        /// <returns>The corresponding preValue editor.</returns>
        protected override IConfigurationEditor CreateConfigurationEditor() => new CognitiveImageCropperConfigurationEditor();
    }
}