using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
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

        private readonly IConfiguration _configuration;

        private ServiceBusProcessor _emailBusProcessor;

        private ServiceBusProcessor _registeredUserProcessor;

        private readonly EmailService _emailService;

        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;

            _emailService = emailService;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");

            registeredUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisteredUserQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);

            _emailBusProcessor = client.CreateProcessor(emailCartQueue);

            _registeredUserProcessor = client.CreateProcessor(registeredUserQueue);

        }

        public async Task Start()
        {
            _emailBusProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailBusProcessor.ProcessErrorAsync += ErrorHandler;
            _emailBusProcessor.StartProcessingAsync();

            _registeredUserProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
            _registeredUserProcessor.ProcessErrorAsync += ErrorHandler;
            await _registeredUserProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _emailBusProcessor.StopProcessingAsync();
            await _emailBusProcessor.DisposeAsync();

            await _registeredUserProcessor.StopProcessingAsync();
            await _registeredUserProcessor.DisposeAsync();
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
