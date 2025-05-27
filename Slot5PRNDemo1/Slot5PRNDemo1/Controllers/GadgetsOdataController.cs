using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Slot5PRNDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GadgetsOdataController : ControllerBase
    {
        private readonly MyWorldDbContext _myWorldDbContext;

        public GadgetsOdataController(MyWorldDbContext myWorldDbContext)
        {
            _myWorldDbContext = myWorldDbContext;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_myWorldDbContext.Gadgets.AsQueryable());
        }
    }
}
