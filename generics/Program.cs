using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generics
{
    class Program
    {
        static void Main(string[] args)
        {
            Office<int> office1 = new Office<int>(123);
            Office<string> office2 = new Office<string>("124");

            Console.WriteLine("oficce 1 number = " + office1.Number);
            Console.WriteLine("oficce 2 number = " + office2.Number);

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

            Console.ReadLine();
        }
    }
    public class Office<T>
    {
        public T Number { get; set; }

        public Office(T number)
        {
            Number = number;
        }
    }

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
