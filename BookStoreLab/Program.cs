using BookStoreLab.Functions;
using BookStoreLab.Models;

namespace BookStoreLab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo cki;
            bool stayInMenu = true;
            bool programRunning = true;
            while (programRunning)
            {
                Console.Clear();
                Writer.Welcome();
                var menuChoice = InputReader.SingleKey(5);
                switch (menuChoice)
                {
                    //1: Check stock 2: Update stock 3: Add data 4: Remove data 5: Close program"
                    case '1':
                        Console.Clear();
                        using (var context = new MyBookStoreContext())
                        {
                            Fetcher.StockStatus(context, MenuFunctions.CheckStock(context));
                        }
                        break;
                    case '2':
                            
                        break;
                    case '3':
                            
                        break;
                    case '4':
                        
                        break;
                    case '5':
                        programRunning = false;
                        break;
                }
        }
                

            
            
           
        }
    }
}
