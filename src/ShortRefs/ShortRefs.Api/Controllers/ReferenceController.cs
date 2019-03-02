namespace ShortRefs.Api.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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
        [Route("")]
        [Route("my")]
        public async Task<IActionResult> GetUserReferencesAsync()
        {
            var host = this.HttpContext.Request.Host.Value;
        
            var result = TestRefs.Select(r => $"{host}/{r}");
        
            return this.Ok(result);
        }
    }
}
