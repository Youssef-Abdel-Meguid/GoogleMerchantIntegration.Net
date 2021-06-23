using Google.Apis.Auth.OAuth2;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.ShoppingContent.v2_1;
using Google.Apis.ShoppingContent.v2_1.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GoogleMerchant;
using System.Configuration;

namespace GoogleMerchant
{
    class Program
    {
        static async Task Main(string[] args)
        {


            string clientId = ConfigurationManager.AppSettings["ClientID"].ToString();
            string clientSecret = ConfigurationManager.AppSettings["ClientSecret"].ToString();
            string appName = ConfigurationManager.AppSettings["ApplicationName"].ToString();
            ulong merchantId = ulong.Parse(ConfigurationManager.AppSettings["MerchantID"].ToString());

            GoogleMerchant googleMerchant = new GoogleMerchant(
                clientId,
                clientSecret, 
                appName, 
                merchantId);


            List<string> fullAddress = new List<string>();
            fullAddress.Add("Youssef");
            fullAddress.Add("AbdelMeguid");
            fullAddress.Add("3");
            fullAddress.Add("haha");
            fullAddress.Add("ainshams");
            fullAddress.Add("cairo");
            fullAddress.Add("egypt");

            List<TestOrderPickupDetailsPickupPerson> p = new List<TestOrderPickupDetailsPickupPerson>();
            p.Add(new TestOrderPickupDetailsPickupPerson()
            {
                Name = "Youssef AbdelMeguid",
                PhoneNumber = "+201555954750"
            });

            List<TestOrderLineItem> items = new List<TestOrderLineItem>();
            items.Add(new TestOrderLineItem()
            {
                QuantityOrdered = 1,
                ShippingDetails = new OrderLineItemShippingDetails()
                {
                    Type = "pickup",
                    DeliverByDate = "2021-07-16T19:20:30+01:00",
                    ShipByDate = "2021-07-16T19:20:30+01:00",
                    Method = new OrderLineItemShippingDetailsMethod()
                    {
                        MaxDaysInTransit = 12,
                        MinDaysInTransit = 8,
                        MethodName = "otlob",
                        Carrier = "otlob"
                    }
                },
                ReturnInfo = new OrderLineItemReturnInfo()
                {
                    DaysToReturn = 0,
                    IsReturnable = false,
                    PolicyUrl = ""
                },
                Product = new TestOrderLineItemProduct()
                {
                    Brand = "nity",
                    Condition = "new",
                    ContentLanguage = "en",
                    OfferId = "2",
                    Price = new Price()
                    {
                        Currency = "EGP",
                        Value = "122.0"
                    },
                    TargetCountry = "EG",
                    Title = "catsss",
                    ImageLink = "https://encrypted-tbn1.gstatic.com/shopping?q=tbn:ANd9GcTYxr4iHCIhk5UBruy4B3jG-AjcWK-GDYQZabs-UTqoxjFJdrU0RVJBVfQ8BS-Ck3BJaqtqPJvPgw"
                }
            });

            OrdersCreateTestOrderRequest order = new OrdersCreateTestOrderRequest()
            {
                Country = "Egypt",
                TemplateName = "template1",
                TestOrder = new TestOrder()
                {
                    PredefinedDeliveryAddress = "jim",
                    PredefinedBillingAddress = "jim",
                    PredefinedEmail = "pog.dwight.schrute@gmail.com",
                    DeliveryDetails = new TestOrderDeliveryDetails()
                    {
                        Address = new TestOrderAddress() 
                        {
                            Country = "Egypt",
                            FullAddress = fullAddress,
                            IsPostOfficeBox = false,
                            Locality = "ainshams",
                            PostalCode = "113322",
                            RecipientName = "Youssef AbdelMeguid",
                            Region = "cairo",
                            StreetAddress = fullAddress
                        },
                        IsScheduledDelivery = true,
                        PhoneNumber = "+201555954750"
                    },
                    PredefinedPickupDetails = "jim",
                    ShippingCost = new Price()
                    {
                        Value = "1002.2",
                        Currency = "EGP"
                    },
                    ShippingOption = "standard",
                    PickupDetails = new TestOrderPickupDetails()
                    {
                        LocationCode = "033",
                        PickupLocationType = "store",
                        PickupLocationAddress = new TestOrderAddress()
                        {
                            Country = "Egypt",
                            FullAddress = fullAddress,
                            IsPostOfficeBox = false,
                            Locality = "ainshams",
                            PostalCode = "113322",
                            RecipientName = "Youssef AbdelMeguid",
                            Region = "cairo",
                            StreetAddress = fullAddress
                        },
                        PickupPersons = p
                    },
                    EnableOrderinvoices = false,
                    LineItems = items
                }
            };

            var createOrder = await googleMerchant.CreateOrder(order);

            var product = await googleMerchant.GetProducts();

            foreach (var po in product)
            {
                Console.WriteLine(po.Id);
            }

        }

    }
}




//List<Product> products = new();
//products.Add(new Product()
//{
//    Title = "catsss"

//});


//var update = await googleMerchant.UpdateProduct(products[0], "online:en:EG:2");
//Console.WriteLine(update.ToString());

//await googleMerchant.InsertProduct(products[0]);

//googleMerchant.InsertProductBatch(products).Wait();

//var d = await googleMerchant.DeleteProduct("online:en:EG:2");

//Console.WriteLine(d.ToString());

//var products = await googleMerchant.GetProducts();

//foreach (var p in products)
//    Console.WriteLine(p.Id);

//var product = await googleMerchant.GetProducts();

//foreach (var p in product)
//{
//    Console.WriteLine(p.Id);
//}