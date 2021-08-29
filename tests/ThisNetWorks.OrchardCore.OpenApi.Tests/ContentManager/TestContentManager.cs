using OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.ContentManager
{

    public class TestContentManager : IContentManager
    {
        public static TestContentManager ContentManager = new TestContentManager();
       
        public Task<ContentValidateResult> RestoreAsync(ContentItem contentItem)
        {
            throw new NotImplementedException();
        }

        public Task<ContentItem> CloneAsync(ContentItem contentItem)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(ContentItem contentItem, VersionOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<ContentValidateResult> CreateContentItemVersionAsync(ContentItem contentItem)
        {
            throw new NotImplementedException();
        }

        public Task DiscardDraftAsync(ContentItem contentItem)
        {
            throw new NotImplementedException();
        }

        public Task SaveDraftAsync(ContentItem contentItem)
        {
            throw new NotImplementedException();
        }

        public Task<ContentItem> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ContentItem> GetAsync(string id, VersionOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ContentItem>> GetAsync(IEnumerable<string> contentItemIds, bool latest = false)
        {
            throw new NotImplementedException();
        }

        public Task<ContentItem> GetVersionAsync(string contentItemVersionId)
        {
            throw new NotImplementedException();
        }

        public Task ImportAsync(IEnumerable<ContentItem> contentItems)
        {
            throw new NotImplementedException();
        }

        public Task<ContentItem> LoadAsync(ContentItem contentItem)
        {
            throw new NotImplementedException();
        }

        public Task<ContentItem> NewAsync(string contentType)
        {
            var contentItem = new ContentItem()
            {
                ContentItemId = IdGenerator.GenerateId(),
                ContentItemVersionId = IdGenerator.GenerateId(),
                ContentType = contentType
            };

            return Task.FromResult(contentItem);
            
        }

        public Task<TAspect> PopulateAspectAsync<TAspect>(IContent content, TAspect aspect)
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync(ContentItem contentItem)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(ContentItem contentItem)
        {
            throw new NotImplementedException();
        }

        public Task UnpublishAsync(ContentItem contentItem)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ContentItem contentItem)
        {
            throw new NotImplementedException();
        }

        public Task<ContentValidateResult> UpdateContentItemVersionAsync(ContentItem updatingVersion, ContentItem updatedVersion)
        {
            throw new NotImplementedException();
        }

        public Task<ContentValidateResult> ValidateAsync(ContentItem contentItem)
        {
            throw new NotImplementedException();
        }
    }
}
