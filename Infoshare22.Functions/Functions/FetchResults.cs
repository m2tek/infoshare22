using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Queues;
using System;

namespace Infoshare22.Functions;
public static class FetchResults
{
    [FunctionName("FetchResults")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
    {
        var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        var queue = new QueueClient(connectionString, "results");
        var response = await queue.ReceiveMessageAsync();
        var msg = response.Value;

        if (msg == null)
        {
            return new NoContentResult();
        }
        else
        {
            await queue.DeleteMessageAsync(msg.MessageId, msg.PopReceipt);
            var r = msg.Body.ToString().FromBase64();
            return new OkObjectResult(r);
        }
    }
}
