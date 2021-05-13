using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionPractic
{
    class Program
    {
        public static db g_db = new db();
        static void Main(string[] args)
        {
            string select = g_db.Shopping.OrderByDescending(i => i.Summa).Select(i => i.Buyer.Name).FirstOrDefault().ToString();
            Console.WriteLine(select);
            Console.ReadLine();
        }
    }
}
