
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThisNetWorks.OrchardCore.OpenApi.SampleModels.Models;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models;
using CreateFooDto = ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models.CreateFooDto;
using UpdateFooDto = ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models.UpdateFooDto;

namespace ThisNetWorks.OrchardCore.OpenApi.SampleModule.Controllers
{
    [Route("api/foo")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken, AllowAnonymous]
    public class FooController: Controller
    {
        private readonly IContentManager _contentManager;

        public FooController(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateFooDto createDto)
        {
            var newContentItem = await _contentManager.NewAsync("FooText");
            var dto = newContentItem.ToDto<FooTextItemDto>();
            dto.FooText.FooField = new TextFieldDto
            {
                Text = createDto.Text
            };

            newContentItem.FromDto(dto);
            await _contentManager.UpdateValidateAndCreateAsync(newContentItem, VersionOptions.Published);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(UpdateFooDto updateDto)
        {
            //var newContentItem = await _contentManager.NewAsync("FooText");
            //var dto = newContentItem.ToDto<FooTextItemDto>();
            //dto.FooText.FooField = new TextFieldDto
            //{
            //    Text = createDto.Text
            //};

            //newContentItem.FromDto(dto);
            //await _contentManager.UpdateValidateAndCreateAsync(newContentItem, VersionOptions.Published);

            return Ok();
        }
    }
}
