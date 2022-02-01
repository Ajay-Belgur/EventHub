using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventhubController : ControllerBase
    {
        private static string connectionString = "";
        private static string eventhubName = "";
        private EventHubProducerClient producerClient;
        private EventHubConsumerClient consumerClient;

        public EventhubController()
        {
            producerClient = new EventHubProducerClient(connectionString, eventhubName);
            consumerClient = new EventHubConsumerClient("$Default", connectionString, eventhubName);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> SendEvent(string item, int numberOfUnits)
        {
            using (var eventBatch = await producerClient.CreateBatchAsync())
            {
                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(new Order(item, numberOfUnits).ToString())));
                await producerClient.SendAsync(eventBatch);
            }

            return Ok();
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> ReceiveEvent(string item, int numberOfUnits)
        {
            await foreach (PartitionEvent partitionEvent in consumerClient.ReadEventsAsync(new System.Threading.CancellationToken()))
            {
                EventData data = partitionEvent.Data;
                return Ok(Encoding.UTF8.GetString(data.EventBody.ToArray()));
            }

            return Ok("No data");

        }

    }
}
