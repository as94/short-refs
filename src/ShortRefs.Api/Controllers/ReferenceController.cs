namespace ShortRefs.Api.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ShortRefs.Api.ClientModels;

    [Authorize]
    [Route("")]
    [Route("references")]
    public class ReferenceController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetMyReferenceStatAsync()
        {
            //var host = this.HttpContext.Request.Host.Value;
        
            var viewModels = new ReferenceStatList
            {
                Items = Array.Empty<ReferenceStatItem>()
            };
        
            return this.Ok(viewModels);
        }

        [HttpGet]
        [Route("{shortReference}")]
        public async Task<IActionResult> RedirectByOriginalReferenceAsync([Required]string shortReference)
        {
            // var originalReference = await this.userReferenceService.GetOriginalReferenceAsync(shortReference);
            //
            // if (originalReference == null)
            // {
            //     return this.BadRequest($"Bad short reference = '{shortReference}'");
            // }

            var originalReference = shortReference;

            return this.LocalRedirect(originalReference);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReferenceAsync([Required][FromBody]ReferenceCreate reference)
        {
            return this.Ok();
        }
    }
}
