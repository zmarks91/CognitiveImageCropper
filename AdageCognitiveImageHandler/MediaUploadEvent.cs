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
            MediaService.Saved += MediaService_Saved;
        }

        private void MediaService_Saved(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            foreach (var mediaItem in e.SavedEntities)
            {
                if (mediaItem.ContentType.Alias == "Image")
                {
                    //perhaps send to Azure for AI analysis of image contents or something...
                    AdageMediaService service = new AdageMediaService();
                    service.HandleImageUpload(mediaItem);
                }
            }
        }

        public void Terminate()
        {
            //throw new NotImplementedException();
        }
    }
}
