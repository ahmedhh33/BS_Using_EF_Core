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
            //Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" +-+-+-+-+-+-+-+-+-+\r\n | TRANSACTION MENUE |\r\n +-+-+-+-+-+-+-+-+-+");
            Console.ResetColor();

            while (true)
            {
                //UserAccounts();

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
                        UserAccounts();
                        //Deposit();
                        break;
                    case "4":
                        Console.Clear();
                        //Withdraw();
                        break;
                    case "5":
                        Console.Clear();
                        //Transferr();
                        //Transfer();
                        break;
                    case "6":
                        Console.Clear();
                        //GetAccountHistory();
                        break;
                    case "7":
                        //loggedInUser = null; // Logout the user.
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
                using (var context = new ApplicationDBContext()) // Replace YourDbContext with the actual name of your DbContext class
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
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" +-+-+-+-+-+-+-+-+-+\r\n | YOUR ACCOUNTS INFO |\r\n +-+-+-+-+-+-+-+-+-+");
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




    }
}
