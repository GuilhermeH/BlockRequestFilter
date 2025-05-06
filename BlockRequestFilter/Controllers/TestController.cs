using Microsoft.AspNetCore.Mvc;

namespace BlockRequestFilter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [ServiceFilter(typeof(BlockImmediateRequestFilter))]
        [HttpGet]
        public string Get()
        {
            return "Request Accepted";
        }
    }
}
