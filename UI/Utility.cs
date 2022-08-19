using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.UI
{
    public static class Utility
    {
        private static long tranId;
        private static CultureInfo culture =  new CultureInfo("IG-NG");
        public static long GetTransactionId()
        {
            return ++tranId;
        }
        public static string GetSecrectInput(string prompt)
        {
            bool isPrompt = true;
            string asterics = "";
            StringBuilder input = new StringBuilder();
            while(true)
            {
                if(isPrompt)
                {
                    Console.WriteLine(prompt);
                    isPrompt = false;
                }
                ConsoleKeyInfo inputKey = Console.ReadKey(true);

                if(inputKey.Key == ConsoleKey.Enter)
                {
                    if (input.Length == 6)
                    {
                        break;
                    }
                    else
                    {
                        PrintMessage("\n\nPlease enter 6 digits.",false);
                        isPrompt = true;
                        input.Clear();
                        continue;
                    }
                }
                if(inputKey.Key==ConsoleKey.Backspace && input.Length >0)
                {
                    input.Remove(input.Length -1, 1);
                }else if(inputKey.Key != ConsoleKey.Backspace)
                {
                    input.Append(inputKey.KeyChar);
                    Console.Write(asterics + "*");
                }
            }
            return input.ToString();
        }
        public static void PrintMessage(string msg, bool succes = true)
        {
            if (succes)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(msg);
            Console.ResetColor();
            PressEnterToContinue();

        }
        //Get User
        public static string GetUserInput(string _prompt)
        {
            Console.WriteLine($"Enter {_prompt}");
            return Console.ReadLine();
        }

        //Press
        public static void PressEnterToContinue()
        {
            Console.WriteLine("\n\n\nPress enter to continue...");
            Console.ReadLine();
        }
        public static void DotnetAnimation(int timer = 10)
        {
           
            for (int i = 0; i < timer; i++)
            {
                Console.Write(".");
                Thread.Sleep(200);
            }
            Console.Clear();
        }
        public static string FormatAmount(decimal amt)
        {
            return String.Format(culture, "{0:C2}", amt);
        }
    }
}
