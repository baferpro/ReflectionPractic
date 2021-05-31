using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegatsAndEvents
{
    class Program
    {
        public delegate int[] MyDelegate();
        public static int[] number = new int[] { 2, 4, 6, 4, 7, 3, 8, 3, 1, 9 };
        static void Main(string[] args)
        {

            //Delegats
            Console.WriteLine(" \n----------------------------\nDelegats\n----------------------------");
            MyDelegate myDelegate = new MyDelegate(LeniviyPodstchet);
            ChtotoHz(myDelegate);

            //Events
            Console.WriteLine(" \n----------------------------\nEvents\n----------------------------");
            Account acc = new Account(100);
            acc.Notify += DisplayMessage;
            acc.Put(20);
            acc.Take(70);
            acc.Take(150);

            Console.ReadLine();
        }


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
        private static void DisplayMessage(object sender, AccountEventArgs e)
        {
            Console.WriteLine($"Сумма транзакции: {e.Sum}");
            Console.WriteLine(e.Message);
        }
    }
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
}
