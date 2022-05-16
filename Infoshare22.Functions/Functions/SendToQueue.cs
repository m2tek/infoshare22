using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infoshare22.Functions;
public static class SendToQueue
{
    [FunctionName("SendToQueue")]
    [return: Queue("inputs")]
    public static async Task<string> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);

        dynamic result = new
        {
            id = data.id,
            amount = data.amount,
            currency = data.currency
        };

        return JsonConvert.SerializeObject(result);
    }
}
