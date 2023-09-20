namespace BD_EF_Core
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("      Welcome To    ");
            Console.WriteLine(" +-+-+-+-+-+-+-+-+-+\r\n |A|h|m|e|d|h|h|3|3|\r\n +-+-+-+-+-+-+-+-+-+");
            Console.WriteLine("    Banking System      \n");
            Console.ResetColor();


            MainMenue mainMenue = new MainMenue();

            mainMenue.mainMenu();

        }
    }
}