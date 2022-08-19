using ATM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.UI
{
    class AppScreen
    {
        internal const string cur = "N ";
        public static void Welcome()
        {
            //Cleas console screen
            Console.Clear();
            //Set the title console
            Console.Title = "My ATM application";
            //Set colors
            Console.ForegroundColor = ConsoleColor.White;
            //Set the message welcom
            Console.WriteLine("\n\n---------------Welcome to My ATM Application---------------\n\n");

            //Prompt the user know what they have to do
            Console.WriteLine("Please insert your ATM card");
            Console.WriteLine("NOTE: Actual ATM machine will accept and validate a physical ATM card, read the card number and validate it.");
            Utility.PressEnterToContinue();
        }

        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();
            tempUserAccount.CardNumber = Validator.Converter<long>("your card number.");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecrectInput("Enter your card PIN here!"));
            return tempUserAccount;
        }
        internal static void LoginProgress()
        {
            Console.WriteLine("\nChecking card number and PIN...");
            Utility.DotnetAnimation();
        }
        internal static void PrintLockScreen()
        {
            Console.Clear();
            Utility.PrintMessage("Your account is locked .Please go to the nearest branch to unlock your account.Thank you.", true);
            Utility.PressEnterToContinue();
            Environment.Exit(1);
        }
        internal static void WelcomeCustomer(string fullname)
        {
            Console.WriteLine($"Welcome Back,{fullname} ");
            Utility.PressEnterToContinue();
        }
        internal static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("----------My ATM App Menu----------");
            Console.WriteLine(":                                  :");
            Console.WriteLine("0. Account Balance                 :");
            Console.WriteLine("1.Cash Deposit                     :");
            Console.WriteLine("2.Withdrawal                       :");
            Console.WriteLine("3.Transfer                         :");
            Console.WriteLine("4.Transactions                     :");
            Console.WriteLine("5.Logout                           :");
        }
        internal static void LogoutProgress()
        {
            Console.WriteLine("Thanks for using My ATM App.");
            Utility.DotnetAnimation();
            Console.Clear();
        }
        internal static int SelecAmount()
        {
            Console.WriteLine("");
            Console.WriteLine($":1.{cur}500      5.{cur}10,000");
            Console.WriteLine($":2.{cur}1000     6.{cur}15,000");
            Console.WriteLine($":3.{cur}2000     7.{cur}20,000");
            Console.WriteLine($":4.{cur}5000     8.{cur}40,000");
            Console.WriteLine($":0.Other");
            Console.WriteLine("");
            int selectAmount = Validator.Converter<int>("option: ");
            switch (selectAmount)
            {
                case 1:
                    return 500;
                    break;
                case 2:
                    return 1000;
                    break;
                case 3:
                    return 2000;
                    break;
                case 4:
                    return 5000;
                    break;
                case 5:
                    return 10000;
                    break;
                case 6:
                    return 15000;
                    break;
                case 7:
                    return 20000;
                    break;
                case 8:
                    return 40000;
                    break;
                case 0:
                    return 0;
                    break;
                default:
                    Utility.PrintMessage("Invalid input.Try again.",false);
                    return -1;
                    break;
            }
        }
        internal static InternalTransfer InternalTransferForm()
        {
            var internalTransfer = new InternalTransfer();
            internalTransfer.ReciepeinAccountNumber = Validator.Converter<long>("recipient's account number: ");
            internalTransfer.TransferAmount = Validator.Converter<decimal>($"amount {cur}");
            internalTransfer.ReciepeinAccountName = Utility.GetUserInput("recipient's name: ");
            return internalTransfer;
        }
    }
}
