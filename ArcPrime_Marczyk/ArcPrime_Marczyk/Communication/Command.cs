using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcPrime_Marczyk.Extensions;

namespace ArcPrime_Marczyk.Communication
{
     public enum Command
     {
          ImportFood, 
          Produce,
          Propaganda,
          Clean,
          BuildArcology,
          ExpandPopulationCapacity,
          ExpandFoodCapacity,
          WeAreReady,
          Restart
     }

     public static class CommandExtensions
     {
          public static string GetNameToJson(this Command command)
          {
               return command.ToString();                                      // Works as long as all enum fields are named the same way that command is named in JSON object
          }

          public static bool IsParamValid(this Command command, string param)
          {
               bool isValid;
               if (command == Command.Restart || command == Command.Produce)   // Params are ignored in these commands, for the rest of them, param values are in the same interval <1,200>
                   isValid = true;
               else
               {
                    int paramAsInt;
                    if (int.TryParse(param, out paramAsInt))
                        isValid = paramAsInt.IsWithin(1, 200);
                    else
                        isValid = false;
               }
               
               return isValid;
          }
     }

}