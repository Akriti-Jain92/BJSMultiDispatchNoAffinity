using Microsoft.WindowsAzure.ResourceStack.Common.BackgroundJobs;

namespace BJSNoAffMultiDispatch.Controllers
{
    [JobCallback(Name = "JobCallback5min")]
    public class JobCallback5min: JobDelegate
    {
        public async override Task<JobExecutionResult> ExecuteAsync(JobExecutionContext context, CancellationToken token)
        {
            Console.WriteLine("I am CallBack for 5 mins: " + DateTime.Now);
            System.Diagnostics.Debug.WriteLine("I am CallBack for 5 mins: " + DateTime.Now);
            string fullPath = String.Format("C:\\Users\\akritijain\\Desktop\\Outputs\\5min.txt");
            using (StreamWriter writer = File.AppendText(fullPath))
            {
                writer.WriteLine("I am CallBack for 5 mins: " + DateTime.Now);
            }

            return await Task.FromResult(new JobExecutionResult { Status = JobExecutionStatus.Succeeded });
        }
    }
}
