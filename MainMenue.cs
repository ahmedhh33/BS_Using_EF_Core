using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_EF_Core
{
    public class MainMenue
    {
        public void mainMenu()
        {
            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" +-+-+-+-+-+-+-+-+-+\r\n | MAIN MENUE |\r\n +-+-+-+-+-+-+-+-+-+");
            Console.ResetColor();
            //BankSystem bankSystem = new BankSystem();
            User user = new User();

            try
            {

                while (true)
                {
                    Console.WriteLine("1. ViewExchangeRates");
                    Console.WriteLine("2. CurrencyConverter");
                    Console.WriteLine("3. Register");
                    Console.WriteLine("4. Login");
                    Console.WriteLine("5. Exit");
                    Console.Write("Select an option: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            ViewExchangeRates();
                            break;
                        case "2":
                            CurrencyConverter();
                            break;
                        case "3":
                            regestringuser();
                            break;
                        case "4":
                            Loging();
                           
                            break;

                        case "5":
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Are you sure you want to exit? (y/n) "); // Check if the user want to exit the application
                            string ExitInput = Console.ReadLine();
                            ExitInput.ToLower();
                            Console.ResetColor();
                            if (ExitInput.Equals("y", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.Write("Thank You");
                                Environment.Exit(0);
                            }
                            else
                            {
                                mainMenu();
                            }
                            break;

                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                // 12 catch the exception message if any occurs
                Console.WriteLine(e.Message);
            }
            //finally
            //{
            //    // 13 after all we need to close the connection with database
            //    sqlConnection.Close();
            //}
        }
        private static void Loging()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("      Logging To    ");
            Console.WriteLine(" +-+-+-+-+-+-+-+-+-+\r\n | YOUR || BAGE |\r\n +-+-+-+-+-+-+-+-+-+");
            Console.WriteLine("    Banking System      \n");
            Console.ResetColor();
            BankSystem bankSystem = new BankSystem();

            Console.Write("Enter your email: ");
            string loginEmail = Console.ReadLine();
            Console.Write("Enter your password: ");
            string loginPassword = Console.ReadLine();

            if (bankSystem.Login(loginEmail, loginPassword))
            {
                Console.WriteLine("Login successful.");
                Console.WriteLine("Welcom To Ahmedhh33 Bank : ");
                //bankSystem.UserInfo(loginEmail, loginPassword);
                bankSystem.HandleLoggedInUser(loginEmail);

            }
            else
            {
                Console.WriteLine("Login failed. Invalid email or password.");
            }
        }
        private static void regestringuser()
        {

            BankSystem bankSystem = new BankSystem();
            User user = new User();

            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.Write("Enter your email: ");
            string email = Console.ReadLine();
            Console.Write("Please enter a password that meets the criteria : ");
            //Console.Write("Password must be at least 8 characters long and contain uppercase letter,lowercase letter,number, and symbol : ");
            string password = Console.ReadLine();
            if (user.IsStrongPassword(password))
            {
                if (bankSystem.RegisterUser(name, email, password))
                {
                    Console.WriteLine("Registration successful.");
                    //Console.WriteLine("Your data information are : ");
                    //bankSystem.UserInfo(email, password);
                    bankSystem.HandleLoggedInUser(email);
                }
                else
                {
                    Console.WriteLine("Registration failed. User with this email already exists.");
                }
            }
            else
            {
                Console.WriteLine("Password does not meet the criteria. Please try again.");
            }
        }
        public async Task ViewExchangeRates()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" +-+-+-+-+-+-+-+-+-+\r\n | View Exchange Rates |\r\n +-+-+-+-+-+-+-+-+-+");
            Console.ResetColor();

            ExchangeRateService exchangeRateService = new ExchangeRateService();
            ExchangeRateData exchangeRates = await exchangeRateService.GetExchangeRatesAsync();

            if (exchangeRates != null)
            {
                Console.WriteLine($"Base Currency: {exchangeRates.base_code}");
                Console.WriteLine("Exchange Rates:");
                foreach (var conversion_rates in exchangeRates.conversion_rates)
                {
                    Console.WriteLine($"{conversion_rates.Key}: {conversion_rates.Value}");
                }
            }
            return;
        }
        public async Task CurrencyConverter()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" +-+-+-+-+-+-+-+-+-+\r\n | Currency Converter |\r\n +-+-+-+-+-+-+-+-+-+");
            Console.ResetColor();

            CurrencyConverter currencyConverter = new CurrencyConverter();

            Console.Write("Enter the currency you want to convert from (e.g., USD): ");
            string fromCurrency = Console.ReadLine().ToUpper();

            Console.Write("Enter the currency you want to convert to (e.g., EUR): ");
            string toCurrency = Console.ReadLine().ToUpper();

            Console.Write("Enter the amount to convert: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                decimal convertedAmount = await currencyConverter.ConvertCurrencyAsync(fromCurrency, toCurrency, amount);
                if (convertedAmount >= 0)
                {
                    Console.WriteLine($"Converted amount: {convertedAmount} {toCurrency}");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }
    }
}
