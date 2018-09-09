// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using System.Collections.Generic;
using System.Linq;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Examples
{
    public static class EventGrid
    {

        [FunctionName("EventGrid")]
        [return: TwilioSms(AccountSidSetting = "AccontSid", AuthTokenSetting = "AuthToken", From = "Telephonnummer")]
        public static CreateMessageOptions Run([EventGridTrigger]EventGridEvent eventGridEvent,
            [CosmosDB("store", "orders", ConnectionStringSetting = "CosmosDbConnectionString")] IEnumerable<Order> orders
            )
        {
            var shippingEvent = eventGridEvent.Data as ShippingEvent;
            var order = orders.FirstOrDefault(o => o.TrackingNumber == shippingEvent.TrackingNumber);
            var message = new CreateMessageOptions(new PhoneNumber(order.MobileNumber))
            {
                Body = $"Ihre Bestellung wurde {order.Id} verschickt",
            };
            return message;
        }
    }

    public class ShippingEvent
    {
        public string TrackingNumber { get; set; }
        public string State { get; set; }
    }

}
