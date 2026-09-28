using Microsoft.AspNetCore.Mvc;

namespace learnova.LearningService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { status = "Learnova LearningService OK" });
    }
}
