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
                            int? storeIdOut;
                            Fetcher.StockStatus(context, "Please select which store to check stock status for.", out storeIdOut);
                            Writer.AnyKeyReturn();
                        }
                        break;
                    case '2':
                        Console.Clear();
                        using (var context = new MyBookStoreContext())
                        {
                            MenuFunctions.UpdateStock(context);
                            Writer.AnyKeyReturn();
                        }
                        break;
                    case '3':
                        Console.Clear();
                        using (var context = new MyBookStoreContext())
                        {
                            MenuFunctions.AddData(context);
                            Writer.AnyKeyReturn();
                        }
                        break;
                    case '4':
                        Console.Clear();
                        //MenuFunctions.RemoveData();
                        break;
                    case '5':
                        programRunning = false;
                        break;
                }
        }
                

            
            
           
        }
    }
}
