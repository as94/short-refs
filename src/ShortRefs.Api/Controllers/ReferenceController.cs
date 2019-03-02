namespace ShortRefs.Api.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ShortRefs.Api.ClientModels;

    [Authorize]
    [Route("")]
    [Route("references")]
    public class ReferenceController : Controller
    {
        private static readonly string[] TestRefs =
        {
            "abc",
            "bcd",
            "cde"
        };

        [HttpGet]
        public async Task<IActionResult> GetMyReferenceStatAsync()
        {
            var host = this.HttpContext.Request.Host.Value;
        
            var viewModels = new ReferenceStatList
            {
                Items =
                    TestRefs.Select(
                        r => new ReferenceStatItem
                        {
                            Original = $"{host}/{r}{r}{r}",
                            Short = $"{host}/{r}",
                            RedirectsCount = 0
                        })
                    .ToArray()
            };
        
            return this.Ok(viewModels);
        }

        // TODO: redirect
        [HttpGet]
        [Route("{shortReference}")]
        public async Task<IActionResult> GetReferenceAsync([Required]string shortReference)
        {
            // increment count

            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReferenceAsync([Required][FromBody]ReferenceCreate reference)
        {
            return this.Ok();
        }
    }
}
