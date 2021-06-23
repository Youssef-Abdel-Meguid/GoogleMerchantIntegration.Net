using Google.Apis.Auth.OAuth2;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.ShoppingContent.v2_1;
using Google.Apis.ShoppingContent.v2_1.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMerchant
{
    public class GoogleMerchant
    {

        private UserCredential Credential;

        private ShoppingContentService Service;

        public ulong MerchantId { get; set; }

        public GoogleMerchant()
        {

        }

        public GoogleMerchant(string clientId, string clientSecret, string applicationName, ulong merchantId)
        {

            MerchantId = merchantId;

            Credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets() 
                { 
                    ClientId = clientId, 
                    ClientSecret = clientSecret 
                },
                new string[] { ShoppingContentService.Scope.Content },
                "user",
                CancellationToken.None
                ).Result;

            Service = new ShoppingContentService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
                ApplicationName = applicationName
            });

        }

        public async Task InsertProductBatch(List<Product> products)
        {
            BatchRequest request = new BatchRequest(Service);

            foreach (var p in products)
            {
                ProductsResource.InsertRequest insertRequest = Service.Products.Insert(p, MerchantId);

                request.Queue<Product>(insertRequest, (content, error, index, message) => 
                {
                    if(content != null)
                        Console.WriteLine(String.Format("Product inserted with id {0}", ((Product)content).Id));

                    if(error != null)
                        Console.WriteLine(error.Message);

                    if(message != null)
                        Console.WriteLine(message.StatusCode);
                });
            }

            await request.ExecuteAsync(CancellationToken.None);

        }

        public async Task<bool> InsertProduct(Product product)
        {
            ProductsResource.InsertRequest insertRequest = Service.Products.Insert(product, MerchantId);
            bool isInserted = false;
            try
            {
                var insertedProduct = await insertRequest.ExecuteAsync(CancellationToken.None);
                isInserted = true;
                Console.WriteLine(string.Format("Product inserted with id {0}", insertedProduct.Id));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }

            return isInserted;

        }

        public async Task<bool> DeleteProduct(string id)
        {

            bool isDelete = false;

            ProductsResource.DeleteRequest deleteRequest = Service.Products.Delete(MerchantId, id);

            try
            {
                await deleteRequest.ExecuteAsync(CancellationToken.None);
                isDelete = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }

            return isDelete;
        }

        public async Task<Product> GetProduct(string id)
        {
            ProductsResource.GetRequest getRequest = Service.Products.Get(MerchantId, id);
            Product product = new Product();

            try
            {
                product = await getRequest.ExecuteAsync(CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }

            return product;
        }

        public async Task<List<Product>> GetProducts()
        {
            ProductsResource.ListRequest listRequest = Service.Products.List(MerchantId);
            List<Product> products = new List<Product>();

            try
            {
                ProductsListResponse response = await listRequest.ExecuteAsync(CancellationToken.None);
                foreach (var p in response.Resources)
                    products.Add(p);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }

            return products;
        }

        public async Task<bool> UpdateProduct(Product product, string id)
        {
            ProductsResource.UpdateRequest updateRequest = Service.Products.Update(product, MerchantId, id);
            bool isUpdated = false;
            try
            {
                var updatedProducted = await updateRequest.ExecuteAsync(CancellationToken.None);
                Console.WriteLine("producte with id {0} has been updated", updatedProducted.Id);
                isUpdated = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }

            return isUpdated;

        }

        public async Task<bool> CreateOrder(OrdersCreateTestOrderRequest order)
        {
            bool isOrderCreated = false;

            OrdersResource.CreatetestorderRequest request = Service.Orders.Createtestorder(order, MerchantId);

            try
            {
                OrdersCreateTestOrderResponse response = await request.ExecuteAsync(CancellationToken.None);
                isOrderCreated = true;
                Console.WriteLine("Order created with id {0}", response.OrderId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }


            return isOrderCreated;
        }

    }
}
