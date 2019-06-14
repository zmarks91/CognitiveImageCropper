using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Services.Implement;

namespace AdageCognitiveImageHandler
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class SubscribeToMediaSavedEventComposer : ComponentComposer<SubscribeToMediaSavedEventComponent>
    {
    }

    public class SubscribeToMediaSavedEventComponent : IComponent
    {
        public void Initialize()
        {
            MediaService.Saving += MediaService_Saving;

            DataTypeService.Saving += DataTypeService_Saving;
            ContentService.Saving += ContentService_Saving;

        }

        private void ContentService_Saving(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentSavingEventArgs e)
        {
            foreach (var mediaItem in e.SavedEntities.OfType<Umbraco.Core.Models.IMedia>())
            {
                if (mediaItem.ContentType.Alias == "Image" && mediaItem.VersionId <= 1)
                {
                    //perhaps send to Azure for AI analysis of image contents or something...
                    AdageMediaService service = new AdageMediaService();
                    service.HandleImageUpload(mediaItem);
                }
            }
        }

        private void DataTypeService_Saving(Umbraco.Core.Services.IDataTypeService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IDataType> e)
        {
            foreach (var mediaItem in e.SavedEntities.OfType<Umbraco.Core.Models.IMedia>())
            {
                if (mediaItem.ContentType.Alias == "Image" && mediaItem.VersionId <= 1)
                {
                    //perhaps send to Azure for AI analysis of image contents or something...
                    AdageMediaService service = new AdageMediaService();
                    service.HandleImageUpload(mediaItem);
                }
            }
        }

        private void MediaService_Saving(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (var mediaItem in e.SavedEntities)
            {
                if (mediaItem.ContentType.Alias == "Image" && mediaItem.VersionId <= 1)
                {
                    //perhaps send to Azure for AI analysis of image contents or something...
                    AdageMediaService service = new AdageMediaService();
                    service.HandleImageUpload(mediaItem);
                }
            }
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }
    }
}
