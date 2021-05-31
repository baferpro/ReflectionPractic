using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ReflectionPractic
{
    class Program
    {
        static void Main(string[] args)
        {
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

            Console.ReadLine();
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
}