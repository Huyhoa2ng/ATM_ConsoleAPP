using ATM.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.App
{
    public class Entry
    {
        public static void Main(string[] args)
        {
            //int name = Validator.Converter<int>($"your card number");
            //Console.WriteLine($"Your card number is {name}");
            ATMapp atmAPP = new ATMapp();
            atmAPP.InitializeData();
            atmAPP.Run();      

        }
    }
}
