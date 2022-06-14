using Microsoft.WindowsAzure.ResourceStack.Common.BackgroundJobs;

namespace BJSNoAffMultiDispatch.Controllers
{
    [JobCallback(Name = "JobCallback7min")]
    public class JobCallback7min: JobDelegate
    {
        public async override Task<JobExecutionResult> ExecuteAsync(JobExecutionContext context, CancellationToken token)
        {
            Console.WriteLine("I am CallBack for 7 mins: " + DateTime.Now);
            System.Diagnostics.Debug.WriteLine("I am CallBack for 7 mins: " + DateTime.Now);
            string fullPath = String.Format("C:\\Users\\akritijain\\Desktop\\Outputs\\7min.txt");
            using (StreamWriter writer = File.AppendText(fullPath))
            {
                writer.WriteLine("I am CallBack for 7 mins: " + DateTime.Now);
            }

            return await Task.FromResult(new JobExecutionResult { Status = JobExecutionStatus.Succeeded });
        }
    }
}
