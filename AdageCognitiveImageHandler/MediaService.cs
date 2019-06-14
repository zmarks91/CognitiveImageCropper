using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace AdageCognitiveImageHandler
{
    public class AdageMediaService
    {
        public virtual void HandleImageUpload(IMedia mediaFile)
        {
            var imageCropperProperties = mediaFile.Properties.Where(prop => prop.PropertyType.PropertyEditorAlias.ToLower() == "imagecropper");
        }
    }
}
