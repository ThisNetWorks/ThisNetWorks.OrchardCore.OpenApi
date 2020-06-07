
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Contents;
using OrchardCore.Mvc.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThisNetWorks.OrchardCore.OpenApi.SampleModels.Models;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models;
using CreateFooDto = ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models.CreateFooDto;
using GetFooDto = ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models.GetFooDto;
using UpdateFooDto = ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models.UpdateFooDto;

namespace ThisNetWorks.OrchardCore.OpenApi.SampleModule.Controllers
{
    [Route("api/foo")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken, AllowAnonymous]
    public class FooController: Controller
    {
        private readonly IContentManager _contentManager;
        private readonly IAuthorizationService _authorizationService;

        public FooController(
            IContentManager contentManager,
            IAuthorizationService authorizationService)
        {
            _contentManager = contentManager;
            _authorizationService = authorizationService;
        }

        //[HttpGet]
        ////TODO get an exception here when using an itemdto.
        //public ActionResult<GetFooDto> Get()
        //{
        //    return Ok(new GetFooDto
        //    {
        //        Text = "foo"
        //    });
        //}

        [HttpGet]
        //TODO get an exception here when using an itemdto.
        public ActionResult<List<GetFooDto>> Get()
        {
            return Ok(new List<GetFooDto>
            {
                new GetFooDto
                {
                    Text = "foo"
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateFooDto createDto)
        {
            var newContentItem = await _contentManager.NewAsync("FooText");
            if (!await _authorizationService.AuthorizeAsync(User, CommonPermissions.PublishContent))
            {
                return this.ChallengeOrForbid();
            }

            var dto = newContentItem.ToDto<FooTextItemDto>();
            dto.FooText.FooField = new TextFieldDto
            {
                Text = createDto.Text
            };
            // In this example we assume that these will always be added to the same list
            // which is created by a migration, so we are able to hard code the content item id.
            // In real life you might use a CustomSettings with a content picker to select
            // the appropriate list for these items to go into.
            dto.ContainedPart = new ContainedPartDto
            {
                ListContentItemId = "45kn6x1x58cg91cqrpm99f475p"
            };

            newContentItem.FromDto(dto);
            await _contentManager.UpdateValidateAndCreateAsync(newContentItem, VersionOptions.Published);

            return Ok();
        }

        [HttpPut("{reference}")]
        public IActionResult Put(string reference, UpdateFooDto updateDto)
        {
            // TODO
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
