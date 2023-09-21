using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BD_EF_Core
{
    public class BankSystem
    {
        private int accountHolderID;
        private string AccountHolderName;


        public void HandleLoggedInUser(string email)
        {
            

            UserAccounts();

            while (true)
            {
                UserAccounts();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" +-+-+-+-+-+-+-+-+-+\r\n | TRANSACTION MENUE |\r\n +-+-+-+-+-+-+-+-+-+");
                Console.ResetColor();


                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("1. Create Bank Account");
                Console.WriteLine("2. Delete Account");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n$ $$ Operations $$ $\n");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("3. Deposit");
                Console.WriteLine("4. Withdraw");
                Console.WriteLine("5. Transfer Money");
                Console.WriteLine("6. Account history");
                Console.ResetColor();
                Console.WriteLine("7. Logout");
                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        
                        CreateAccount();
                        break;
                    case "2":
                        
                        DeleteAccount();
                        break;
                    case "3":
                        Console.Clear();
                        Deposit();
                        break;
                    case "4":
                        Withdraw();
                        break;
                    case "5":
                        Transfer();
                        break;
                    case "6":
                        GetAccountHistory();
                        break;
                    case "7":
                        MainMenue mainMenue = new MainMenue();
                        mainMenue.mainMenu();
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public bool RegisterUser(string name, string email, string password)
        {


            try
            {
               var userinfo = new User { Name = name, Email = email, Password = password };
                var _context = new ApplicationDBContext();

                _context.Users.Add(userinfo);
                _context.SaveChanges();

               return true;

            }
            catch (Exception e)
            {
                // 12 catch the exception message if any occurs
                Console.WriteLine(e.Message);
            }
            
            return false;

        }

        public bool Login(string email, string password)
        {
            try
            {
                using (var context = new ApplicationDBContext()) 
                {
                    var user = context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);

                    if (user != null)
                    {
                        accountHolderID = user.Id; //save it in global variable
                        AccountHolderName = user.Name;

                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid email or password. Please try again.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false; // Login failed.
        }

        public void CreateAccount()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" +-+-+-+-+-+-+-+-+-+\r\n | CREATING ACCOUNT |\r\n +-+-+-+-+-+-+-+-+-+");
            Console.ResetColor();
            try
            {
                
                Console.Write("Enter initial balance: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal initialBalance))
                {
                    using (var context = new ApplicationDBContext()) 
                    {
                        // Create a new Account object
                        var newAccount = new Account{ Balance = initialBalance,Id = accountHolderID,AccountHolderName = AccountHolderName };// Glopal user id created when ever the user login from function info

                        // Add the new account to the context and save changes to the database
                        context.Accounts.Add(newAccount);
                        int rowsAffected = context.SaveChanges();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Bank account created successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Error creating bank account.");
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid initial balance.");
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void DeleteAccount()
        {
            UserAccounts();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" +-+-+-+-+-+-+-+-+-+\r\n | DELETING ACCOUNT |\r\n +-+-+-+-+-+-+-+-+-+");
            Console.ResetColor();

            Console.WriteLine("Please enter the account number you want to delete");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }

            try
            {
                using (var context = new ApplicationDBContext()) 
                {
                    var accountToDelete = context.Accounts.Where(x=>x.Id == accountHolderID).SingleOrDefault(a => a.AccountNumber == accountNumber);

                    if (accountToDelete != null)
                    {
                        context.Accounts.Remove(accountToDelete);
                        int rowsAffected = context.SaveChanges();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Account {accountNumber} deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"Error deleting account {accountNumber}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Account not found.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void UserAccounts()
        {
            //Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  YOUR ACCOUNTS INFO ");
            Console.ResetColor();
            try
            {
                using (var context = new ApplicationDBContext()) 
                {
                    var userAccounts = context.Accounts

                        .Where(a => a.Id == accountHolderID)
                        .ToList();

                    foreach (var account in userAccounts)
                    {
                        Console.WriteLine($"UserID: {account.Id} Account Number: {account.AccountNumber}, Balance: {account.Balance}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        public void Deposit()
        {
            UserAccounts();

            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" +-+-+-+-+-+-+-+-+-+\r\n | DEPOSITTING |\r\n +-+-+-+-+-+-+-+-+-+");
            Console.ResetColor();

            Console.Write("Enter the account number to deposit into: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }

            Console.Write("Enter the amount to deposit: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid deposit amount.");
                return;
            }

            try
            {
                using (var context = new ApplicationDBContext()) 
                {
                    var account = context.Accounts.SingleOrDefault(a => a.AccountNumber == accountNumber);

                    if (account != null)
                    {
                        account.Balance += amount; // Update the balance in memory
                        int Rowaffected = context.SaveChanges();

                        if(Rowaffected > 0)
                        {
                            Console.WriteLine("Depositing successful.");

                            Transaction transaction = new Transaction();
                            var depositTransaction = new Transaction
                            {
                                Amount = amount,
                                Type = TransactionType.Deposit,
                                AccountNumber = account.AccountNumber,
                                Timestamp = DateTime.UtcNow
                            };

                            context.Transactions.Add(depositTransaction);
                            int rowsAffected = context.SaveChanges();
                            if (rowsAffected > 0)
                            {

                                Console.WriteLine("Transaction regesteried.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Account not found. Or a problem happend while depositting ");
                        return;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public void Withdraw()
        {
            UserAccounts();

            Console.Write("Enter the account number to withdraw from: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }

            Console.Write("Enter the amount to withdraw: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            try
            {
                using (var context = new ApplicationDBContext())
                {
                    var account = context.Accounts.SingleOrDefault(a => a.AccountNumber == accountNumber && a.Id == accountHolderID);

                    if (account != null)
                    {
                        if (account.Balance >= amount)
                        {
                            account.Balance -= amount; // Update the balance in memory
                            int rowsAffected = context.SaveChanges();

                            if (rowsAffected > 0)
                            {
                                Console.WriteLine("Withdrawal successful.");

                                var withdrawalTransaction = new Transaction
                                {
                                    Amount = amount,
                                    Type = TransactionType.Withdrawal,
                                    AccountNumber = accountNumber,
                                    Timestamp = DateTime.UtcNow
                                };

                                context.Transactions.Add(withdrawalTransaction);
                                int transactionRowsAffected = context.SaveChanges();

                                if (transactionRowsAffected > 0)
                                {
                                    Console.WriteLine("Transaction registered.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error updating account balance.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Account not found.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }

        public void Transfer()
        {
            UserAccounts();

            Console.Write("Enter the account number to transfer from: ");
            if (!int.TryParse(Console.ReadLine(), out int sourceAccountNumber))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }

            Console.Write("Enter the account number to transfer to: ");
            if (!int.TryParse(Console.ReadLine(), out int targetAccountNumber))
            {
                Console.WriteLine("Invalid target account number.");
                return;
            }

            Console.Write("Enter the amount to transfer: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid transfer amount.");
                return;
            }

            using (var context = new ApplicationDBContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sourceAccount = context.Accounts.SingleOrDefault(a => a.AccountNumber == sourceAccountNumber && a.Id == accountHolderID);
                        var targetAccount = context.Accounts.SingleOrDefault(a => a.AccountNumber == targetAccountNumber);

                        if (sourceAccount == null || targetAccount == null)
                        {
                            Console.WriteLine("One of the accounts doesn't exist.");
                            return;
                        }

                        if (sourceAccount.Balance >= amount)
                        {
                            sourceAccount.Balance -= amount;
                            targetAccount.Balance += amount;

                            context.SaveChanges();

                            var transferTransaction = new Transaction
                            {
                                Amount = amount,
                                Type = TransactionType.Transfer,
                                SourceAccountNumber = sourceAccountNumber,
                                TargetAccountNumber = targetAccountNumber,
                                AccountNumber = sourceAccountNumber,
                                Timestamp = DateTime.Now
                            };
                            var transferTargetTransaction = new Transaction
                            {
                                Amount = amount,
                                Type = TransactionType.Transfer,
                                SourceAccountNumber = sourceAccountNumber,
                                TargetAccountNumber = targetAccountNumber,
                                AccountNumber = targetAccountNumber,
                                Timestamp = DateTime.Now
                            };
                            context.Transactions.Add(transferTransaction);
                            context.Transactions.Add(transferTargetTransaction);
                            context.SaveChanges();

                            transaction.Commit();

                            Console.WriteLine($"Transferred {amount} OMR from account {sourceAccountNumber} to account {targetAccountNumber}.");
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds in the source account.");
                        }
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"An error occurred: {e.Message}");
                    }
                }
            }
        }



        public void GetAccountHistory()
        {
            UserAccounts();

            Console.Write("Enter the account number to show the history: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }

            using (var context = new ApplicationDBContext())
            {
                var account = context.Accounts.SingleOrDefault(a => a.AccountNumber == accountNumber && a.Id == accountHolderID);

                if (account == null)
                {
                    Console.WriteLine("Account not found.");
                    return;
                }

                var transactions = context.Transactions
                    .Where(t => t.AccountNumber == accountNumber)
                    .ToList();

                if (transactions.Any())
                {
                    Console.WriteLine($"Transaction History for Account {accountNumber}:");

                    foreach (var transaction in transactions)
                    {
                        Console.WriteLine($"TransactionID: {transaction.TransactionId}");
                        Console.WriteLine($"Type: {transaction.Type}");
                        Console.WriteLine($"Amount: {transaction.Amount}");
                        Console.WriteLine($"Source Account Number: {transaction.SourceAccountNumber}");
                        Console.WriteLine($"Target Account Number: {transaction.TargetAccountNumber}");
                        Console.WriteLine($"Account Number: {transaction.AccountNumber}");
                        Console.WriteLine($"Timestamp: {transaction.Timestamp}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                else
                {
                    Console.WriteLine("No transaction history found for this account.");
                }
            }
        }




    }
}
