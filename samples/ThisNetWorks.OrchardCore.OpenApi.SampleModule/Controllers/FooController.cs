
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThisNetWorks.OrchardCore.OpenApi.SampleModels;

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
        public async Task<IActionResult> Post(string textField)
        {
            var newContentItem = await _contentManager.NewAsync("FooText");
            var dto = newContentItem.ToDto<FooTextItemDto>();
            dto.FooText.FooField = new TextFieldDto
            {
                Text = textField
            };

            newContentItem.FromDto(dto);
            await _contentManager.UpdateValidateAndCreateAsync(newContentItem, VersionOptions.Published);

            return Ok();
        }
    }
}
