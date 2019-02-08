using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphModelling
{
    static class ExtensionMethods
    {
        const double epsilon = 0;
    

   
        public static bool EqualTo(this double a, double b)
        {
            return EqualTo(a, b, epsilon);
        }

        public static bool EqualTo(this double a, double b, double maximumDifferenceAllowed)
        {
            // Handle comparisons of floating point values that may not be exactly the same
            return (Math.Abs(a - b) < maximumDifferenceAllowed);
        }
        public static bool IsNotEqual(this double a, double b)
        {
            return (a != b) && !(Math.Abs(a - b) < epsilon);
        }
        public static bool LT(this double a, double b) { return IsNotEqual(a, b) && (a < b); }
        public static bool GT(this double a, double b) { return IsNotEqual(a, b) && (a > b); }
    }
}
