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
     public class CommandExecutor
     {
          RestClient client;
          string emailAddress;
          string token;

          public CommandExecutor(string requestClientBaseUrl, string emailAddress, string token)
          {
               this.client = new RestClient(requestClientBaseUrl);
               this.emailAddress = emailAddress;
               this.token = token;
          }

          public bool TryExecuting(Command command, string paramValue)
          {
               var request = new RestRequest(Method.POST);
               var commandJson = createJsonObjOfCommand(command.GetNameToJson(), paramValue);

               configureRequest(request);
               addCommandJsonToRequestParam(commandJson, request);
               IRestResponse response = client.Execute(request);

               return wasExecutedProperly(response);
          }

          private string createJsonObjOfCommand(string commandName, string paramValue)
          {
              return "{\"Command\": \""  + commandName  + "\", "    /* {"Command": "someCommandValue", "Something": "someValue", etc ...} */
                   + "\"Login\": \""     + emailAddress + "\", "
                   + "\"Token\": \""     + token        + "\", "
                   + "\"Parameter\": \"" + paramValue   + "\"}";
          }

          private void configureRequest(RestRequest request)
          {
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
          }

          private void addCommandJsonToRequestParam(string commandJson, RestRequest request)
          {
            request.AddParameter("application/json", commandJson, ParameterType.RequestBody);
          }

          private bool wasExecutedProperly(IRestResponse response)
          {
               if (response.ErrorException != null)                 // It should help, but in any case, it's rather harmless.
                    return false;
               if (response.StatusCode != HttpStatusCode.OK)
                    return false;

               return true; 
          }
     }
}
