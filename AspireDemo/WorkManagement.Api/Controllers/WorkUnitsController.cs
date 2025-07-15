using Microsoft.AspNetCore.Mvc;

namespace WorkManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WorkUnitsController : ControllerBase
{
    private readonly ILogger<WorkUnitsController> _logger;

    private readonly AppDbContext _appDbContext;

    private readonly WorkerApiClient _workerClient;

    public WorkUnitsController(ILogger<WorkUnitsController> logger, AppDbContext dbContext, WorkerApiClient workerClient)
    {
        _logger = logger;
        _appDbContext = dbContext;
        _workerClient = workerClient;
    }

    [HttpPost(Name = "Post work unit")]
    public IActionResult PostWork([FromBody] WorkUnitPostDto workUnitDto)
    {
        WorkUnit workUnit = PerformWork(workUnitDto);

        _appDbContext.WorkUnits.Add(workUnit);

        _appDbContext.SaveChanges();

        return new OkResult();
    }

    private WorkUnit PerformWork(WorkUnitPostDto workUnitDto)
    {
        WorkResult responseObject = _workerClient.GetWorkResult(workUnitDto);

        return new WorkUnit()
        {
            Text = responseObject.Text,
            StartCount = responseObject.StartCount,
            EndCount = responseObject.EndCount,
        };
    }

    

    [HttpGet(Name = "Get work units")]
    public IEnumerable<WorkUnit> Get()
    {
        return _appDbContext.WorkUnits.ToArray();
    }
}
