using Microsoft.WindowsAzure.ResourceStack.Common.BackgroundJobs;

namespace BJSNoAffMultiDispatch.Controllers
{
    public class ScheduleJobClass
    {
        public async void scheduleAJob(int count, int time, Type callback)
        {
            var jobBuilder3 = JobBuilder.Create(jobPartition: time + "mins2", jobId: "id" + time + "mins2")
              .WithCallback(callback)
              .WithRepeatStrategy(count, TimeSpan.FromMinutes(time))
              .WithRetryStrategy(2, TimeSpan.FromMinutes(1));

            var managementClient3 = new JobManagementClient(
              connectionString: String.Format("DefaultEndpointsProtocol=https;AccountName=akritibjsmultijobtest;AccountKey=grfgM2LMP+u3eENp1AdMIiuablkMaDGUKASj707z3j3xz8j74Nwa+1PotXmvNt3NNdNunQOBlBpr+ASttH+BcQ==;EndpointSuffix=core.windows.net"),
              executionAffinity: String.Empty,
              eventSource: new EventSourceTest(),
              queueNamePrefix: "queue" + time + "mins2",
              tableName: "table" + time + "mins2",
              encryptionUtility: null);

            await managementClient3
              .CreateOrUpdateJob(jobBuilder3)
              .ConfigureAwait(continueOnCapturedContext: false);

            var dispatcherClient3 = new JobDispatcherClient(
              connectionString: String.Format("DefaultEndpointsProtocol=https;AccountName=akritibjsmultijobtest;AccountKey=grfgM2LMP+u3eENp1AdMIiuablkMaDGUKASj707z3j3xz8j74Nwa+1PotXmvNt3NNdNunQOBlBpr+ASttH+BcQ==;EndpointSuffix=core.windows.net"),
              executionAffinity: String.Empty,
              tableName: "table" + time + "mins2",
              queueNamePrefix: "queue" + time + "mins2",
              eventSource: new EventSourceTest(),
              encryptionUtility: null);

            dispatcherClient3.RegisterJobCallback(callback);

            dispatcherClient3.ProvisionSystemConsistencyJob().Wait();

            dispatcherClient3.Start();
        }
    }
}
