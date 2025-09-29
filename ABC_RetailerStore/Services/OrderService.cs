using ABCRetail.Web.Models;
using Azure.Storage.Queues;
using System.Text.Json;

namespace ABCRetail.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly QueueClient _queueClient;

        public OrderService(QueueServiceClient queueServiceClient)
        {
            _queueClient = queueServiceClient.GetQueueClient("order-processing");
            _queueClient.CreateIfNotExists();
        }

        public async Task ProcessOrderAsync(OrderMessage orderMessage)
        {
            var messageContent = JsonSerializer.Serialize(orderMessage);
            await _queueClient.SendMessageAsync(messageContent);
        }

        public async Task<IEnumerable<string>> GetOrderMessagesAsync()
        {
            var messages = new List<string>();
            var response = await _queueClient.ReceiveMessagesAsync(maxMessages: 10);
            
            foreach (var message in response.Value)
            {
                messages.Add(message.MessageText);
                // In a real application, you would process the message and then delete it
                // await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
            }
            
            return messages;
        }
    }
}
