using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArcPrime_Marczyk.Model;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Web;

namespace ArcPrime_Marczyk.Communication
{
     public class ArcStateReceiver
     {
          RestClient client;

          public ArcStateReceiver(string responseClientBaseUrl)
          {
               this.client = new RestClient(responseClientBaseUrl);
          }
          
          public ArcologyState GetState()
          {
               var request = new RestRequest(Method.GET);
               request.AddHeader("cache-control", "no-cache");
               request.AddHeader("content-type", "application/json");
               IRestResponse response = client.Execute(request);

               return JsonConvert.DeserializeObject<ArcologyState>(response.Content);
          } 
     }
}
