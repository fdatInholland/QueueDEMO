using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;
using QueueDEMO.QueueInterfaces;

namespace QueueDEMO
{
    public class HttpFunctionQueueInsert
    {
        private readonly IAzureStorageQueue _messageQueue;

        public HttpFunctionQueueInsert(IAzureStorageQueue storageQueue)
        {
            _messageQueue = storageQueue;
        }

        [Function("HttpFunctionQueueInsert")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            //PREF map to valid object!
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string message = data?.message;

            if (string.IsNullOrWhiteSpace(message))
            {
                return new BadRequestObjectResult("Please pass a 'message' in the request body.");
            }

            await _messageQueue.CreateMessage(message);

            return new OkObjectResult($"Message added to queue: {message}");
        }
    }
}
