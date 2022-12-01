using Microsoft.Azure.WebJobs;
using System;

namespace Functions
{
    public static class FunctionsWithRabbitMq
    {
        [FunctionName("RabbitMQTriggerCSharp")]
        public static void RabbitMQTrigger_BasicDeliverEventArgs(
        [RabbitMQTrigger("queueName", ConnectionStringSetting = "connectionString")] string message)
        {
            Console.WriteLine($"C# RabbitMQ queue trigger function processed message: {message}");
        }
    }
}
 