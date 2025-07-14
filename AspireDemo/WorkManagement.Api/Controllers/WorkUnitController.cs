using Microsoft.AspNetCore.Mvc;

namespace WorkManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkUnitsController : ControllerBase
{
    private readonly ILogger<WorkUnitsController> _logger;

    private readonly AppDbContext _appDbContext;

    public WorkUnitsController(ILogger<WorkUnitsController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _appDbContext = dbContext;
    }

    [HttpPost(Name = "Post work unit")]
    public IActionResult PostWork([FromBody] WorkUnitPostDto workUnitDto)
    {
        WorkUnit workUnit = new WorkUnit() { Text = workUnitDto.Text };

        _appDbContext.WorkUnits.Add(workUnit);

        _appDbContext.SaveChanges();

        return new OkResult();
    }

    [HttpGet(Name = "Get work units")]
    public IEnumerable<WorkUnit> Get()
    {
        return _appDbContext.WorkUnits.ToArray();
    }
}