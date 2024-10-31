using DevGuide.Models.Models;
using DevGuide.Models;
using Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillController : ControllerBase
    {
        private SkillManager skillManager;
        public SkillController(SkillManager _skillManager)
        {
            this.skillManager = _skillManager;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var res =skillManager.GetAll().Select(i=> new { Name = i.Name, Id = i.Id });
            return new JsonResult(res);
        }
    }
}
