using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionPractic
{
    class Program
    {

        public delegate int[] MyDelegate();
        public static int[] number = new int[] { 2, 4, 6, 4, 7, 3, 8, 3, 1, 9 };

        static void Main(string[] args)
        {
            //Reflection / attributes
            Console.WriteLine("----------------------------\nReflection / attributes\n----------------------------");
            var shopping = new Shopping(500);
            var buyer = new Buyer("User1");

            PropertyInfo shopInfo = typeof(Shopping).GetProperties().Where(i => i.Name.Equals("Summa")).FirstOrDefault();
            Console.WriteLine(shopInfo.Name + " = " + shopInfo.GetValue(shopping));
            Type type = shopInfo.CustomAttributes.FirstOrDefault().AttributeType;
            PropertyInfo userName = type.GetProperties().Where(i => i.Name.Equals("Name")).FirstOrDefault();
            var test = userName.GetValue(buyer).ToString();
            Console.WriteLine(userName.Name + " = " + test);

            var type1 = typeof(Shopping);
            var attributes = type1.GetCustomAttributes(false);
            foreach(var attribute in attributes)
            {
                Console.WriteLine(attribute);
            }

            var properties = type1.GetProperties();
            foreach(var prop in properties)
            {
                Console.WriteLine(prop.PropertyType + " " + prop.Name);

                var attrs = prop.GetCustomAttributes(false);
                foreach(var a in attrs)
                {
                    Console.WriteLine(a);
                }
            }

            //Generics
            Console.WriteLine(" \n----------------------------\nGenerics\n----------------------------");
            Office<int> office1 = new Office<int>(123);
            Office<string> office2 = new Office<string>("124");

            Console.WriteLine("oficce 1 number = " + office1.Number);
            Console.WriteLine("oficce 2 number = " + office2.Number);

            //Delegats
            Console.WriteLine(" \n----------------------------\nDelegats\n----------------------------");
            MyDelegate myDelegate = new MyDelegate(LeniviyPodstchet);
            ChtotoHz(myDelegate);

            //Variance
            Console.WriteLine(" \n----------------------------\nVariance\n----------------------------");
            Console.WriteLine("out"); //Ковариантность
            var b = new CheckOut<BMW>();
            Console.WriteLine(b is iCheckOut<BMW>);
            Console.WriteLine(b is iCheckOut<Car>);
            Console.WriteLine(b is iCheckOut<Series8>); 
            Console.WriteLine("\nin"); //Контрвариантность
            var c = new CheckIn<BMW>();
            Console.WriteLine(c is iCheckIn<BMW>);
            Console.WriteLine(c is iCheckIn<Car>);
            Console.WriteLine(c is iCheckIn<Series8>);

            //Func and etc.
            Console.WriteLine(" \n----------------------------\nFunc and etc.\n----------------------------");
            Console.WriteLine("Func");
            Func<int, int, int> degr = Degree;
            int result = GetInt(3, 5, degr);
            Console.WriteLine(result);

            Console.WriteLine("\nAction");
            Action<int, int> sum = (num1,num2) => Console.WriteLine(num1 + num2);
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


            //End
            Console.ReadLine();
        }

        //Delegats
        public static int[] LeniviyPodstchet()
        {
            return number.Where(i => i % 2 == 0).ToArray();
        }
        public static void ChtotoHz(MyDelegate myDelegate)
        {
            myDelegate += myDelegate;
            foreach (var a in myDelegate())
            {
                Console.WriteLine(a);
            }
        }

        //Func and etc.
        private static int GetInt(int v1, int v2, Func<int, int, int> degr)
        {
            int result = -1;
            if (v1 > 0 && v2 > 0)
                result = degr(v1, v2);
            return result;
        }
        static int Degree(int number, int degree)
        {
            if (degree <= 0)
                return -1;

            int result = number;
            do
            {
                degree--;
                result *= number;
            } while (degree > 1);

            return result;
        }
    }

    //Reflection / attributes
    [Test]
    public class Shopping
    {
        [Buyer] public int Summa { get; set; }

        public Shopping(int summa)
        {
            this.Summa = summa;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Buyer : Attribute
    {
        public string Name { get; set; }

        public Buyer()
        { }
        public Buyer(string name)
        {
            this.Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class Test : Attribute
    {
        string TestName { get; set; }

        public Test()
        {
            this.TestName = "Test";
        }

    }

    //Generics
    public class Office<T>
    {
        public T Number { get; set; }

        public Office(T number)
        {
            Number = number;
        }
    }

    //variance
    public class Car { }
    public class BMW : Car { }
    public class Series8 : BMW { }
    public interface iCheckOut<out T>
    {
        T GetId(int ID);
    }
    public class CheckOut<T> : iCheckOut<T>
    {
        public T GetId(int ID) { throw new NotImplementedException(); }
    }
    public interface iCheckIn<in T>
    {
        void Save(T ID);
    }
    public class CheckIn<T> : iCheckIn<T>
    {
        public void Save(T ID) { throw new NotImplementedException(); }
    }
}
