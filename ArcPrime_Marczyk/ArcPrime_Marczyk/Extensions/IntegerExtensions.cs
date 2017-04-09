using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcPrime_Marczyk.Extensions
{
     internal static class IntegerExtensions
     {
          internal static bool IsWithin(this int value, int minimum, int maximum)
          {
               return value >= minimum && value <= maximum;
          }
     }
}
