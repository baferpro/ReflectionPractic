using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedDelegates_
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Func");
            Func<int, int, int> degr = Degree;
            int result = degr(3, 5);
            Console.WriteLine(result);

            Console.WriteLine("\nAction");
            Action<int, int> sum = (num1, num2) => Console.WriteLine(num1 + num2);
            sum(5, 6);

            Console.WriteLine("\nPredicate");
            Predicate<int> check = (num3) => num3 > 0;
            Console.WriteLine(check(45));
            Console.WriteLine(check(-10));

            Console.WriteLine("\nExpression");
            ParameterExpression param1 = Expression.Parameter(typeof(int), "a");
            ParameterExpression param2 = Expression.Parameter(typeof(int), "b");
            Expression add = Expression.Add(param1, param2);
            LambdaExpression expressionlambda = Expression.Lambda(add, param1, param2);
            var newLambda = (Func<int, int, int>)expressionlambda.Compile();
            Console.WriteLine(newLambda(10, 5));

            Console.ReadLine();
        }
        static int Degree(int number, int degree)
        {
            int result = number;

            if (degree > 0)
            {
                do
                {
                    degree--;
                    result *= number;
                } while (degree > 1);
            }
            else if (degree == 0)
            {
                result = 1;
            }
            else
            {
                result = -1;
            }

            return result;
        }
    }
}
