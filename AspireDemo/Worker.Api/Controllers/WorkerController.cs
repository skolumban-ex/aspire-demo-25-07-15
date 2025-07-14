using Microsoft.AspNetCore.Mvc;

namespace Worker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly ILogger<WorkerController> _logger;

        private readonly AppDbContext _appDbContext;

        public WorkerController(ILogger<WorkerController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _appDbContext = dbContext;
        }

        [HttpPost(Name = "Perform work")]
        public WorkResultDto PerformWork([FromBody] WorkUnit workUnit)
        {
            // if there are no workers, create one
            if (_appDbContext.Workers.Count() == 0)
            {
                _appDbContext.Workers.Add(new Worker() { FinishedWorkCount = 0 });
                _appDbContext.SaveChanges();
            }

            // get the first worker
            var firstWorker = _appDbContext.Workers.First();

            int startCount = firstWorker.FinishedWorkCount;

            // wait a second
            Thread.Sleep(1000);

            firstWorker.FinishedWorkCount++;

            _appDbContext.SaveChanges();

            int endCount = firstWorker.FinishedWorkCount;

            return new WorkResultDto
            {
                EndCount = endCount,
                StartCount = startCount,
                Text = workUnit.Text,
            };
        }
    }
}
