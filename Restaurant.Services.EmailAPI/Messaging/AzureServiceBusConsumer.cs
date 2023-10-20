using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Restaurant.Services.EmailAPI.Message;
using Restaurant.Services.EmailAPI.Models.Dto;
using Restaurant.Services.EmailAPI.Service;
using System.Text;

namespace Restaurant.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;

        private readonly string emailCartQueue;

        private readonly string registeredUserQueue;

        private readonly string orderCreated_Topic;

        private readonly string orderCreated_Email_Subscription;

        private readonly IConfiguration _configuration;

        private readonly EmailService _emailService;

        private ServiceBusProcessor _emailBusProcessor;

        private ServiceBusProcessor _registeredUserProcessor;

        private ServiceBusProcessor _emailOrderPlacedProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;

            _emailService = emailService;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");

            registeredUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisteredUserQueue");

            orderCreated_Topic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreated_Email_Subscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Email_Subscription");

            var client = new ServiceBusClient(serviceBusConnectionString);

            _emailBusProcessor = client.CreateProcessor(emailCartQueue);

            _registeredUserProcessor = client.CreateProcessor(registeredUserQueue);

            _emailOrderPlacedProcessor = client.CreateProcessor(orderCreated_Topic, orderCreated_Email_Subscription);

        }

        public async Task Start()
        {
            _emailBusProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailBusProcessor.ProcessErrorAsync += ErrorHandler;
            _emailBusProcessor.StartProcessingAsync();

            _registeredUserProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
            _registeredUserProcessor.ProcessErrorAsync += ErrorHandler;
            await _registeredUserProcessor.StartProcessingAsync();

            _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedRequestReceived;
            _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlacedProcessor.StartProcessingAsync();
        }

        private async Task OnOrderPlacedRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            RewardsMessage objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
                //TODO - try to log email
                await _emailService.LogOrderPlaced(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task Stop()
        {
            await _emailBusProcessor.StopProcessingAsync();
            await _emailBusProcessor.DisposeAsync();

            await _registeredUserProcessor.StopProcessingAsync();
            await _registeredUserProcessor.DisposeAsync();

            await _emailOrderPlacedProcessor.StopProcessingAsync();
            await _emailOrderPlacedProcessor.DisposeAsync();
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                //TODO - try to log email
                await _emailService.EmailCartAndLog(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            string email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                //TODO - try to log email
                await _emailService.RegisterUserEmailAndLog(email);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
