namespace ShortRefs.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.Ok("Hello from short-refs");
        }
    }
}
