using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using System;

namespace Functions
{
    public static class FunctionsWithRabbitMq
    {
        [FunctionName("RabbitMQTriggerCSharp")]
        public static void RabbitMQTrigger_BasicDeliverEventArgs(
            [RabbitMQTrigger(queueName: "queue", ConnectionStringSetting = "rabbitMQConnectionAppSetting")] string message, ILogger logger)
            {
                logger.LogInformation($"C# RabbitMQ queue trigger function processed message: {message}");
                Console.WriteLine($"C# RabbitMQ queue trigger function processed message: {message}");
            }
    }
}
