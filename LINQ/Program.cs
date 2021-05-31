using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ
{
    class Program
    {
        public static ShopBuyTestEntities db = new ShopBuyTestEntities();
        static void Main(string[] args)
        {
            //Linq
            Console.WriteLine(" \n----------------------------\nLinq\n----------------------------");
            int bestBuyer = db.Shopping.GroupBy(i => i.BuyerId)
                                        .Select(g => new { Summa = g.Sum( i => i.Summa), BuyerId = g.Key })
                                        .OrderByDescending(i => i.Summa)
                                        .Select(i => i.BuyerId).FirstOrDefault();
            Console.WriteLine(db.Buyer.Where(i => i.Id == bestBuyer).Select(i => i.Name).FirstOrDefault());

            //iEnumerable
            Console.WriteLine(" \n----------------------------\niEnumerable\n----------------------------");
            var startTime = System.Diagnostics.Stopwatch.StartNew();
            IEnumerable<Buyer> buyers = db.Buyer.Where(i => i.Name.Contains("а")).ToList();
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
            IQueryable<Buyer> buyers1 = db.Buyer.Where(i => i.Name.Contains("а"));
            List<Buyer> list = buyers1.ToList();
            startTime1.Stop();
            var resultTime1 = startTime1.Elapsed;
            var elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                resultTime1.Hours,
                resultTime1.Minutes,
                resultTime1.Seconds,
                resultTime1.Milliseconds);
            Console.WriteLine("Время выполнения: " + elapsedTime1);

            Console.ReadLine();
        }
    }
}
