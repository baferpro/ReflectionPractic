using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatsWTF
{
    class Program
    {
        public delegate int[] MyDelegate();
        public static int[] number = new int[] { 2, 4, 6, 4, 7, 3, 8, 3, 1, 9 };
        static void Main(string[] args)
        {
            MyDelegate myDelegate = new MyDelegate(LeniviyPodstchet);
            ChtotoHz(myDelegate);
        }
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
            Console.ReadLine();
        }
    }
}
