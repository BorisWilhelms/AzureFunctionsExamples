using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Threading.Tasks;

namespace Examples
{
    public static class CosmosDb
    {
        [FunctionName("AddToShoppingCart")]
        public static async Task<IActionResult> Run(
             [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ShopingCart/{productId}")]HttpRequest req,
             [CosmosDB("store", "products", ConnectionStringSetting = "CosmosDbConnectionString", Id = "{productId}")] Product product,
             [CosmosDB("store", "shoppingcart", ConnectionStringSetting = "CosmosDbConnectionString")] IAsyncCollector<ShoppingCartItem> shoppingCart
         )
        {
            var userId = GetUserId(req);
            var item = new ShoppingCartItem()
            {
                UserId = userId.Value,
                Product = product
            };
            await shoppingCart.AddAsync(item);
            return new OkResult();
        }

        private static Guid? GetUserId(HttpRequest request) =>
            // User ID aus request extrahieren.
            null;
    }
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class ShoppingCartItem
    {
        public Guid UserId { get; set; }
        public Product Product { get; set; }
    }
}
