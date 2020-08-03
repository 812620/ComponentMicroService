using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComponentProcessingMicroservice.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using ComponentProcessingMicroservice.Processing;

namespace ComponentProcessingMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentProcessingMicroserviceController : ControllerBase
    {
        readonly int Limit = 50000;
        
        ProcessRequest RequestObject = new ProcessRequest();

        ProcessResponse ResponseObject = new ProcessResponse();     
       
        // GET: api/ComponentProcessingMicroservice/5
       [HttpGet("ProcessDetail")]
        public string GetRequest(string json)
        {
            var RequestObject = JsonConvert.DeserializeObject<ProcessRequest>(json);

       RequestObject = new ProcessRequest
            {
                Name = RequestObject.Name,
                ContactNumber = RequestObject.ContactNumber,
                CreditCardNumber = RequestObject.CreditCardNumber,
                ComponentType = RequestObject.ComponentType,
                ComponentName = RequestObject.ComponentName,
                Quantity = RequestObject.Quantity,
                IsPriorityRequest = RequestObject.IsPriorityRequest

            };
            int Processing = ProcessId();



            ResponseObject = new ProcessResponse
            {
                RequestId = Processing,
                ProcessingCharge = ProcessingCharge(RequestObject.ComponentType),
                PackagingAndDeliveryCharge = PackagingDelivery(RequestObject.ComponentType, RequestObject.Quantity),
                DateOfDelivery = DeliveryDate()

            };

         string ResponseString= JsonConvert.SerializeObject(ResponseObject);
            return ResponseString;

        }
        public DateTime DeliveryDate()
        {
            DateTime date = DateTime.Now;
            if (RequestObject.IsPriorityRequest == true)
            {
                return date.AddDays(2).Date;
            }
            else
            {
                return date.AddDays(5).Date;
            }
        }
        public int PackagingDelivery(string Item, int Count)
        {
            var PackigingDeliveryCharge = "";
            var query = "?item="+Item+"&count="+Count;
            HttpClient client = new HttpClient();
            HttpResponseMessage result = client.GetAsync("https://localhost:44348/GetPackagingDeliveryCharge" + query).Result;


            if (result.IsSuccessStatusCode)
            {
                PackigingDeliveryCharge = result.Content.ReadAsStringAsync().Result;
            }
            else
            {
                PackigingDeliveryCharge = "0";
            }
            int charge = int.Parse(PackigingDeliveryCharge);
            return charge;
        }

        public string CardDetails(CardDetails details)
        {
            PaymentDetails Response;
            var data = JsonConvert.SerializeObject(details);
            var value = new StringContent(data, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
              var response = client.PostAsync("https://localhost:44360/api/ProcessPayment", value).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    Response = JsonConvert.DeserializeObject<PaymentDetails>(result);
                    if(Response.Message == "Successful")
                    {
                        return "Successful";
                    }
                    else
                    {
                        return "Failed";
                    }
                }
                else
                {
                    return "Failed";
                }
            }
        }
        public int ProcessId()
        {
            int n = 0;
            Random _random = new Random();
            n = _random.Next(500,1000);
            return n;
        }

        public int ProcessingCharge(string Workflow)
        {
            int ProcessingCharge = 0;
            Workflow = RequestObject.ComponentType;
            if (Workflow == "Integral")
            {
                IntegralWorkflow integral = new IntegralWorkflow();
                ProcessingCharge = integral.ProcessingCharge(RequestObject.IsPriorityRequest);
            }
            else
            {
                AccessoryWorkflow accessory = new AccessoryWorkflow();
                ProcessingCharge = accessory.ProcessingCharge(RequestObject.IsPriorityRequest);
            }
            return ProcessingCharge;
        }

        [HttpPost]
        public string GetUserMessage(string message)
        {
            if (message == "True")
            {
                CardDetails detail = new CardDetails()
                {
                    CreditCardNumber = RequestObject.CreditCardNumber,
                    CreditLimit = Limit,
                    ProcessingCharge = ResponseObject.ProcessingCharge + ResponseObject.PackagingAndDeliveryCharge
                };
                var result = CardDetails(detail);
                if(result == "Successful")
                {
                    return "Success";
                }
                else
                {
                    return "Failed";
                }
            }
            else
            {
                return "Payment not initiated";
            }
        }
    }
}
