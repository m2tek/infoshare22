using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infoshare22.Functions;
public class Convert
{
    private HttpClient _httpClient;
    public Convert(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://infoshare22-apim.azure-api.net/");
        _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("SubscriptionKey"));
    }

    [FunctionName("Convert")]
    [return: Queue("results")]
    public async Task<string> Run([QueueTrigger("inputs", Connection = "AzureWebJobsStorage")]string queueMessage, ILogger log)
    {
        dynamic data = JsonConvert.DeserializeObject(queueMessage);
        var result = await _httpClient.GetAsync($"/convert?currency={data.currency}&amount={data.amount.ToString("#.##", CultureInfo.InvariantCulture)}");
            
        result.EnsureSuccessStatusCode();
            
        var amountPLN = await result.Content.ReadAsStringAsync();

        return JsonConvert.SerializeObject(new
        {
            id = data.id,
            amountPLN = amountPLN
        });
    }
}
