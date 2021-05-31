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
        public static ShopBuyTestEntities db = new ShopBuyTestEntities();

        public delegate int[] MyDelegate();
        public static int[] number = new int[] { 2, 4, 6, 4, 7, 3, 8, 3, 1, 9 };
        static void Main(string[] args)
        {
            //Reflection / attributes
            Console.WriteLine("----------------------------\nReflection / attributes\n----------------------------");
            var shopping = new Shoppings(500);
            var buyer = new Buyers("User1");

            PropertyInfo shopInfo = typeof(Shoppings).GetProperties().Where(i => i.Name.Equals("Summa")).FirstOrDefault();
            Console.WriteLine(shopInfo.Name + " = " + shopInfo.GetValue(shopping));
            Type type = shopInfo.CustomAttributes.FirstOrDefault().AttributeType;
            PropertyInfo userName = type.GetProperties().Where(i => i.Name.Equals("Name")).FirstOrDefault();
            var test = userName.GetValue(buyer).ToString();
            Console.WriteLine(userName.Name + " = " + test);

            var type1 = typeof(Shoppings);
            var attributes = type1.GetCustomAttributes(false);
            foreach (var attribute in attributes)
            {
                Console.WriteLine(attribute);
            }

            var properties = type1.GetProperties();
            foreach (var prop in properties)
            {
                Console.WriteLine(prop.PropertyType + " " + prop.Name);

                var attrs = prop.GetCustomAttributes(false);
                foreach (var a in attrs)
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

            //Events
            Console.WriteLine(" \n----------------------------\nEvents\n----------------------------");
            Account acc = new Account(100);
            acc.Notify += DisplayMessage;
            acc.Put(20);
            acc.Take(70);
            acc.Take(150);

            //Linq
            Console.WriteLine(" \n----------------------------\nLinq\n----------------------------");
            string bestBuyer = db.Shopping.OrderByDescending(i => i.Summa).Select(i => i.Buyer.Name).FirstOrDefault().ToString();
            Console.WriteLine(bestBuyer);

            //iEnumerable
            Console.WriteLine(" \n----------------------------\niEnumerable\n----------------------------");
            var startTime = System.Diagnostics.Stopwatch.StartNew();
            IEnumerable<Shopping> shoppings1 = db.Shopping.Where(i => i.Buyer.Name.Contains("й")).ToList();
            foreach (var j in shoppings1)
            {
                Console.WriteLine(j.Buyer.Name);
            }
            startTime.Stop();
            var resultTime = startTime.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                resultTime.Hours,
                resultTime.Minutes,
                resultTime.Seconds,
                resultTime.Milliseconds);
            Console.WriteLine("Время выполнения: " + elapsedTime);

            //iEnumerable
            Console.WriteLine(" \n----------------------------\nIQueryable\n----------------------------");
            var startTime1 = System.Diagnostics.Stopwatch.StartNew();
            IQueryable<Shopping> shoppings2 = db.Shopping.Where(i => i.Buyer.Name.Contains("й"));
            List<Shopping> list = shoppings2.ToList();
            foreach (var j in list)
            {
                Console.WriteLine(j.Buyer.Name);
            }

            startTime1.Stop();
            var resultTime1 = startTime1.Elapsed;
            var elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                resultTime1.Hours,
                resultTime1.Minutes,
                resultTime1.Seconds,
                resultTime1.Milliseconds);
            Console.WriteLine("Время выполнения: " + elapsedTime1);

            //End
            Console.ReadLine();
        }

        //Delegats and events
        public static int[] LeniviyPodstchet()
        {
            return number.Where(i => i % 2 == 0).ToArray();
        }

        public delegate void MessageHandler(string message);
        public event MessageHandler Notify;
        public static void ChtotoHz(MyDelegate myDelegate)
        {
            myDelegate += myDelegate;

            foreach (var a in myDelegate())
            {
                Console.WriteLine(a);
            }
        }

        //Func and etc.
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
            else if(degree == 0)
            {
                result = 1;
            }
            else
            {
                result = -1;
            }

            return result;
        }

        //Events
        private static void DisplayMessage(object sender, AccountEventArgs e)
        {
            Console.WriteLine($"Сумма транзакции: {e.Sum}");
            Console.WriteLine(e.Message);
        }
    }

    //Events
    class Account
    {
        public delegate void AccountHandler(object sender, AccountEventArgs e);
        public event AccountHandler Notify;
        public Account(int sum)
        {
            Sum = sum;
        }
        public int Sum { get; private set; }
        public void Put(int sum)
        {
            Sum += sum;
            Notify?.Invoke(this, new AccountEventArgs($"На счет поступило {sum}", sum));
        }
        public void Take(int sum)
        {
            if (Sum >= sum)
            {
                Sum -= sum;
                Notify?.Invoke(this, new AccountEventArgs($"Сумма {sum} снята со счета", sum));
            }
            else
            {
                Notify?.Invoke(this, new AccountEventArgs("Недостаточно денег на счете", sum)); ;
            }
        }
    }
    class AccountEventArgs
    {
        // Сообщение
        public string Message { get; }
        // Сумма, на которую изменился счет
        public int Sum { get; }

        public AccountEventArgs(string mes, int sum)
        {
            Message = mes;
            Sum = sum;
        }
    }

    //Reflection / attributes
    [Test]
    public class Shoppings
    {
        [Buyers] public int Summa { get; set; }

        public Shoppings(int summa)
        {
            this.Summa = summa;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Buyers : Attribute
    {
        public string Name { get; set; }

        public Buyers()
        { }
        public Buyers(string name)
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