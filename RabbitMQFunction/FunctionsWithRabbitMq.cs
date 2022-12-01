using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs;

namespace Functions
{
    public static class FunctionsWithRabbitMq
    {
        [FunctionName("RabbitMQTriggerCSharp")]
        public static void RabbitMQTrigger_BasicDeliverEventArgs(
        [RabbitMQTrigger("RabbitMqListener", ConnectionStringSetting = "connectionString")] string message, ILogger logger)
        {
            logger.LogInformation($"RabbitMQ queue trigger function processed message: {message}");
        }
    }
}
 