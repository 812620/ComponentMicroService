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
        
        ProcessRequest RequestObject = new ProcessRequest();

        ProcessResponse ResponseObject = new ProcessResponse();
/*      
       
        // GET: api/ComponentProcessingMicroservice/5
       [HttpGet("ProcessDetail")]
        public string GetRequest(string json)
        {
            string ComponentType = "";int ComponentQuantity = 0;

            int Processing = 0;
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
            ComponentType = RequestObject.ComponentType;
            ComponentQuantity = RequestObject.Quantity;
            Processing = ProcessId();



            ResponseObject = new ProcessResponse
            {
                RequestId = Processing,
                ProcessingCharge = ProcessingCharge(RequestObject.ComponentType),
                PackagingAndDeliveryCharge = PackagingDelivery(ComponentType, ComponentQuantity),
                DateOfDelivery = DeliveryDate()

            };

         string ResponseString= JsonConvert.SerializeObject(ResponseObject);
            return ResponseString;

        }
        private DateTime DeliveryDate()
        {
            DateTime date = DateTime.Now;
            if (RequestObject.IsPriorityRequest == true)
            {
                return date.AddDays(2);
            }
            else
            {
                return date.AddDays(5);
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

*/

        [HttpPost]
        private string CardDetails(CardDetails obj)
        {
            PaymentDetails Response;
            var data = JsonConvert.SerializeObject(obj);
            var value = new StringContent(data, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
              var response = client.PostAsync("", value).Result;

                if (response.IsSuccessStatusCode)
                {
                    Response = JsonConvert.DeserializeObject<PaymentDetails>(CardDetails);
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
/*
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
                IntegralWorkflow ob = new IntegralWorkflow();
                ProcessingCharge= ob.ProcessingCharge(RequestObject);
            }
            else
            {
                AccessoryWorkflow ob1 = new AccessoryWorkflow();
                ProcessingCharge=ob1.ProcessingCharge(RequestObject);
            }
            return ProcessingCharge;
        }

        public string GetMessageUser(Boolean message)
        {
            string FinalMessage = "";
            string ResponseMessage ="";
            
            if (message == true)
            {
             var Response=CardDetails();
                ResponseMessage= Response.Message;
                if(ResponseMessage!="sucessfull")
                {
                    FinalMessage = "Payment Failed";
                }
                else
                {
                    FinalMessage = "Payment Sucessfull";
                }   
                
            }
            return FinalMessage;
        }
*/
        public int GetCardLimit(int CardCardNumber)
        {
            int CardLimit = 0;
            Dictionary<long, int> CreditCard = new Dictionary<long, int>();
            CreditCard.Add(100,500 );
            CreditCard.Add(101,700);
            CreditCard.Add(102, 1000);
            CreditCard.Add(105, 400);
            foreach (KeyValuePair<long,int> ele1 in CreditCard)
            {
                if (ele1.Key == CardCardNumber)
                {
                    CardLimit = ele1.Value;
                }
                else
                {
                    CardLimit = 0;
                }
          
            }
            return CardLimit;
        }

        /*    
             public int RecieveResponse()
        {

C:\Users\Namit Pundir\source\repos\ComponentProcessingMicroservice\Controllers\ComponentProcessingMicroserviceController.cs
        }  
        */
    }
}
