using Microsoft.Azure.WebJobs;
using System;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Examples
{
    public static class AzureFunctions
    {
        [FunctionName("OrderSmsSender")]
        // Deklarative Verbindung zu Twilio
        [return: TwilioSms(AccountSidSetting = "AccontSid", AuthTokenSetting = "AuthToken", From = "Telephonnummer")]
        public static CreateMessageOptions Run([QueueTrigger("%OrderQueue%", Connection = "OrderQueueConnection")] Order data)
        {
            var message = new CreateMessageOptions(new PhoneNumber(data.MobileNumber))
            {
                Body = $"Hallo {data.Name}, danke fuer Ihre Bestellung. Sie wird nun von uns bearbeitet!",
            };
            return message;
        }
    }
    public class Order
    {
        public Guid Id { get; set; }
        public string TrackingNumber { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public Product[] Products { get; set; }
    }
}
