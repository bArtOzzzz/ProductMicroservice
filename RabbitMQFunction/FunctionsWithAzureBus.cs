using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using System.Text;
using System.IO;

namespace Functions
{
    public static class FunctionsWithAzureBus
    {
        [FunctionName("SendMessage")]
        [return: ServiceBus("az-queue", EntityType.Queue)]
        public static async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("SendMessage function requested");
            string body = string.Empty;
            using (var reader = new StreamReader(req.Body, Encoding.UTF8))
            {
                body = await reader.ReadToEndAsync();
                log.LogInformation($"Message body : {body}");
            }
            log.LogInformation($"SendMessage processed.");
            return body;
        }
    }
}   
    