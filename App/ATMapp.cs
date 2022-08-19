using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using ATM.UI;
using ATM.Domain.Entities;
using ATM.Domain.Interface;
using ATM.Domain.Enums;

namespace ATM.App
{
    class ATMapp: IUserLogin,IUserAccountActions,ITransaction
    {
        private List<UserAccount> userAccountList;
        private UserAccount selectAccount;
        private List<Transaction> _listTransaction;
        private const decimal minimumKeptAmount = 500;
        private readonly AppScreen screen;
        public  ATMapp()
        {
            screen = new AppScreen();
        }

        public void CheckUserCardNumberAndPassword()
        {
            bool isCorrectLogin = false;
            while(isCorrectLogin==false)
            {
                UserAccount inputUserAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach(UserAccount account in userAccountList)
                {
                    selectAccount = account;
                    if (inputUserAccount.CardNumber.Equals(selectAccount.CardNumber))
                    {
                        selectAccount.TotalLogin++;
                        if(inputUserAccount.CardPin.Equals(selectAccount.CardPin))
                        {
                            selectAccount = account;
                            if(selectAccount.IsLocked || selectAccount.TotalLogin >3)
                            {
                                AppScreen.PrintLockScreen();
                            }
                            else
                            {
                                selectAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                    }
                }
                if(isCorrectLogin==false)
                {
                    Utility.PrintMessage("\n Invalid card number or PIN.",false);
                    selectAccount.IsLocked = selectAccount.TotalLogin == 3;
                    if(selectAccount.IsLocked)
                    {
                        AppScreen.PrintLockScreen();
                    }
                }
                Console.Clear();    

            }

        }
        
        public void Run()
        {
            AppScreen.Welcome();
            CheckUserCardNumberAndPassword();
            AppScreen.WelcomeCustomer(selectAccount.FullName);
            AppScreen.DisplayMenu();
            ProcessMenuOptions();
        }
        public void InitializeData()
        {
            userAccountList = new List<UserAccount>
            {
                new UserAccount{Id=1,FullName="Hoang gia huy1",CardNumber=2,CardPin=333333,AccountBalance=50000.00m,IsLocked=true },
                new UserAccount{Id=2,FullName="Hoang gia huy2",CardNumber=22,CardPin=444444,AccountBalance=40000.00m,IsLocked=false },
                new UserAccount{Id=3,FullName="Hoang gia huy3",CardNumber=222,CardPin=55,AccountBalance=30000.00m,IsLocked=false },

            };
            _listTransaction = new List<Transaction>();
        }
        private void ProcessMenuOptions()
        {
            switch (Validator.Converter<int>("an option: "))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithDrawal:
                    MakeWithDrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                    var internalTransfer = AppScreen.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;
                case (int)AppMenu.ViewTransaction:
                    Console.WriteLine("Viewing transaction");
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogoutProgress();
                    Utility.PrintMessage("You have successfully logged out.Please collect your ATM card.");
                    Run();
                    break;
                default:
                    Utility.PrintMessage("Invalid Option",false);
                    break;
            }
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your account balance is: {Utility.FormatAmount(selectAccount.AccountBalance)}");
        }
        //Place deposit
        public void PlaceDeposit()
        {
            Console.WriteLine("\nOnly multiple of 500 and 1000 naria allowed.\n");
            var transaction_amt = Validator.Converter<int>($"amount: {AppScreen.cur}");

            //simulate counting
            Console.WriteLine("\nChecking and Counting bank notes.");
            Utility.DotnetAnimation();
            Console.WriteLine("");
            //Some
            if(transaction_amt<=0)
            {
                Utility.PrintMessage("Amount need to be greater than zero.Try again.", false);
                return;
            }
            if(transaction_amt%500 != 0)
            {
                Utility.PrintMessage($"Enter deposit amount in multiples of 500 or 1000.Try again.",false);
                return;
            }
            if(PreviewBankNotesAccount(transaction_amt)==false)
            {
                Utility.PrintMessage($"You have cancelled your action.",false);
                return;
            }

            //blin transaction details to transaction object
            InsertTransaction(selectAccount.Id,TransactionType.Deposit,transaction_amt,"");
            //update account balance
            selectAccount.AccountBalance += transaction_amt;
            //print success update
            Utility.PrintMessage($"Your deposit of {Utility.FormatAmount(transaction_amt)} was successful.",true);
        }

        public void MakeWithDrawal()
        {
            var transaction_amt = 0;
            int selectAmount = AppScreen.SelecAmount();
            if (selectAmount == -1)
            {
                selectAmount = AppScreen.SelecAmount();
            }else if(selectAmount != 0)
            {
                transaction_amt = selectAmount;
            }
            else
            {
                transaction_amt = Validator.Converter<int>($"amount: {AppScreen.cur}");
            }
            //input validation
            if(transaction_amt <= 0)
            {
                Utility.PrintMessage("Amount need to be greater than zero.Try again ", false);
                return;
            }
            if(transaction_amt % 500 != 0)
            {
                Utility.PrintMessage("You can only withdraw amount in multiples of 500 or 1000 naira.Try again.",false );
                return;

            }
            //business logic validation
            if(transaction_amt > selectAccount.AccountBalance)
            {
                Utility.PrintMessage($"Withdraw failed. Your balence is too low to withdraw {Utility.FormatAmount(transaction_amt)}",false);
                return;
            }
            if(selectAccount.AccountBalance - transaction_amt <minimumKeptAmount)
            {
                Utility.PrintMessage($"Withdraw failed. Your account have to have minimum{Utility.FormatAmount(minimumKeptAmount)}",false);
                return;
            }
            //blind withdraw details to transaction object
            InsertTransaction(selectAccount.Id, TransactionType.WithDrawal, transaction_amt, "");
            //update account balance
            selectAccount.AccountBalance -= transaction_amt;
            //succes message
            Utility.PrintMessage($"You have successfully withdrawn  {Utility.FormatAmount(transaction_amt)}.",true);
        }
        private bool PreviewBankNotesAccount(int amout)
        {
            int thousandNoteCount = amout / 1000;
            int fivehundredNoteCount = (amout % 1000) / 500;

            Console.WriteLine("\nSummary");
            Console.WriteLine("---------");
            Console.WriteLine($"{AppScreen.cur}1000 x {thousandNoteCount} = {1000 * thousandNoteCount}");
            Console.WriteLine($"{AppScreen.cur}500 x {fivehundredNoteCount} = {500 * fivehundredNoteCount}");
            Console.WriteLine($"Total amount: {Utility.FormatAmount(amout)}\n\n");
            int otp = Validator.Converter<int>("1 to confirm");
            return otp.Equals(1);
        }

        public void InsertTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc)
        {
            var transaction = new Transaction()
            {
                TransactionId =Utility.GetTransactionId(),
                UserBankAccountId = _UserBankAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _tranType,
                Description = _desc
            };
            _listTransaction.Add(transaction);
        }

        public void ViewTransaction()
        {
            throw new NotImplementedException();
        }
        //Itransaction
        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            if(internalTransfer.TransferAmount<=0)
            {
                Utility.PrintMessage("Amount needs to be more than zero.Try again.", false);
                return;
            }
            if(internalTransfer.TransferAmount > selectAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed.You don't have enough balance to transfer {Utility.FormatAmount(internalTransfer.TransferAmount)}");
                return;
            }
            //check the minimums kept amount 
            if((selectAccount.AccountBalance - minimumKeptAmount)< minimumKeptAmount)
            {
                Utility.PrintMessage($"Transfer faile. Your account needs to have minimum {Utility.FormatAmount(minimumKeptAmount)}", false);
                return ;
            }
            //check reciever's number is valid
            var selectedBankAccountReciever = (from userAcc in userAccountList
                                               where userAcc.AccountNumber == internalTransfer.ReciepeinAccountNumber
                                               select userAcc).FirstOrDefault();
            if(selectedBankAccountReciever == null)
            {
                Utility.PrintMessage("Transfer failed ,Recieber bank account number is valid. ",false);
                return;
            }
            //checl reciever's name is valid
            if (selectAccount.FullName != internalTransfer.ReciepeinAccountName)
            {
                Utility.PrintMessage("Transfer failed,Recipient's bank account does not match");
                return;
            }
            //add transaction 
            InsertTransaction(selectAccount.Id, TransactionType.Transfer, -internalTransfer.TransferAmount, "Transfer" + $"to {selectedBankAccountReciever.AccountNumber} ({selectedBankAccountReciever.FullName})");
            //update sender's account balance
            selectAccount.AccountBalance -= internalTransfer.TransferAmount;
            //add transaction record-reciever
            InsertTransaction(selectedBankAccountReciever.Id, TransactionType.Transfer, -internalTransfer.TransferAmount, "Transfer" + $"{selectAccount.AccountNumber}({selectAccount.FullName})");
            //update reciever's account balance
            selectedBankAccountReciever.AccountBalance += internalTransfer.TransferAmount;
            //print succes message
            Utility.PrintMessage($"You have successfully transfered" + $"{Utility.FormatAmount(internalTransfer.TransferAmount)} to " + $"{internalTransfer.ReciepeinAccountName}",true);
        }

    }
}