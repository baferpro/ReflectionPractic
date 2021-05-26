using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionPractic
{
    class Program
    {
        static void Main(string[] args)
        {
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
            Console.ReadLine();


            Office<int> office1 = new Office<int>(123);
            Office<string> office2 = new Office<string>("124");

            Console.WriteLine("oficce 1 number = " + office1.Number);
            Console.WriteLine("oficce 2 number = " + office2.Number);
            Console.ReadLine();
        }
    }

    [Test]
    class Shopping
    {
        public T Id { get; set; }
        [Buyer] public int Summa { get; set; }

        public Shopping(int summa)
        {
            this.Summa = summa;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    class Buyer : Attribute
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
    class Test : Attribute
    {
        string TestName { get; set; }

        public Test()
        {
            this.TestName = "Test";
        }

    }
    class Office<T>
    {
        public T Number { get; set; }

        public Office(T number)
        {
            Number = number;
        }
    }
}
