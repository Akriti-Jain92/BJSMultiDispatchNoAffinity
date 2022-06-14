using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.ResourceStack.Common.BackgroundJobs;

namespace BJSNoAffMultiDispatch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BJSMultiDispatch : ControllerBase
    {

        private readonly ILogger<BJSMultiDispatch> _logger;

        public BJSMultiDispatch(ILogger<BJSMultiDispatch> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<String> Get()
        {
            Console.WriteLine("I am for testing");
            System.Diagnostics.Debug.WriteLine("I am for testing");
            var jobBuilder1 = JobBuilder.Create(jobPartition: "5mins2", jobId: "id5mins2")
              .WithCallback(typeof(JobCallback5min))
              .WithRepeatStrategy(3, TimeSpan.FromMinutes(5))
              .WithRetryStrategy(2, TimeSpan.FromMinutes(1));

            var managementClient1 = new JobManagementClient(
              connectionString: String.Format("DefaultEndpointsProtocol=https;AccountName=akritibjsmultijobtest;AccountKey=grfgM2LMP+u3eENp1AdMIiuablkMaDGUKASj707z3j3xz8j74Nwa+1PotXmvNt3NNdNunQOBlBpr+ASttH+BcQ==;EndpointSuffix=core.windows.net"),
              executionAffinity: String.Empty,
              eventSource: new EventSourceTest(),
              queueNamePrefix: "queue5mins2",
              tableName: "table5mins2",
              encryptionUtility: null);

            await managementClient1
              .CreateOrUpdateJob(jobBuilder1)
              .ConfigureAwait(continueOnCapturedContext: false);

            var dispatcherClient1 = new JobDispatcherClient(
              connectionString: String.Format("DefaultEndpointsProtocol=https;AccountName=akritibjsmultijobtest;AccountKey=grfgM2LMP+u3eENp1AdMIiuablkMaDGUKASj707z3j3xz8j74Nwa+1PotXmvNt3NNdNunQOBlBpr+ASttH+BcQ==;EndpointSuffix=core.windows.net"),
              executionAffinity: String.Empty,
              eventSource: new EventSourceTest(),
              tableName: "table5mins2",
              queueNamePrefix: "queue5mins2",
              encryptionUtility: null);

            dispatcherClient1.RegisterJobCallback(typeof(JobCallback5min));

            dispatcherClient1.ProvisionSystemConsistencyJob().Wait();

            dispatcherClient1.Start();



            var jobBuilder2 = JobBuilder.Create(jobPartition: "10mins2", jobId: "id10mins2")
              .WithCallback(typeof(JobCallback10min))
              .WithRepeatStrategy(2, TimeSpan.FromMinutes(10))
              .WithRetryStrategy(2, TimeSpan.FromMinutes(1));

            var managementClient2 = new JobManagementClient(
              connectionString: String.Format("DefaultEndpointsProtocol=https;AccountName=akritibjsmultijobtest;AccountKey=grfgM2LMP+u3eENp1AdMIiuablkMaDGUKASj707z3j3xz8j74Nwa+1PotXmvNt3NNdNunQOBlBpr+ASttH+BcQ==;EndpointSuffix=core.windows.net"),
              executionAffinity: String.Empty,
              eventSource: new EventSourceTest(),
              queueNamePrefix: "queue10mins2",
              tableName: "table10mins2",
              encryptionUtility: null);

            await managementClient2
              .CreateOrUpdateJob(jobBuilder2)
              .ConfigureAwait(continueOnCapturedContext: false);

            var dispatcherClient2 = new JobDispatcherClient(
              connectionString: String.Format("DefaultEndpointsProtocol=https;AccountName=akritibjsmultijobtest;AccountKey=grfgM2LMP+u3eENp1AdMIiuablkMaDGUKASj707z3j3xz8j74Nwa+1PotXmvNt3NNdNunQOBlBpr+ASttH+BcQ==;EndpointSuffix=core.windows.net"),
              executionAffinity: String.Empty,
              tableName: "table10mins2",
              queueNamePrefix: "queue10mins2",
              eventSource: new EventSourceTest(),
              encryptionUtility: null);

            dispatcherClient2.RegisterJobCallback(typeof(JobCallback10min));

            dispatcherClient2.ProvisionSystemConsistencyJob().Wait();

            dispatcherClient2.Start();

            ScheduleJobClass scheduleJob = new ScheduleJobClass();
            scheduleJob.scheduleAJob(100, 7, typeof(JobCallback7min));
            scheduleJob.scheduleAJob(150, 12, typeof(JobCallback12min));
            scheduleJob.scheduleAJob(15, 15, typeof(JobCallback15min));
            scheduleJob.scheduleAJob(50, 18, typeof(JobCallback18min));
            scheduleJob.scheduleAJob(20, 20, typeof(JobCallback20min));
            scheduleJob.scheduleAJob(25, 25, typeof(JobCallback25min));

            return "BJS Execution: " + DateTime.Now;
        }

    }
}