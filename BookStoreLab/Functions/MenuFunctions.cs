using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookStoreLab.Models;

namespace BookStoreLab.Functions
{
    public static class MenuFunctions
    {
        public static int CheckStock(MyBookStoreContext context)
        {
            Console.WriteLine("Please select which store to check stock status for.");
            var stores = context.Stores.ToList();
            foreach (var store in stores)
            {
                Console.WriteLine($"Store: {store.StoreName}\nStoreID: {store.StoreId}");
            }
            int output = Convert.ToInt32(Console.ReadLine());
            return output;
        }

        public static void AddData(MyBookStoreContext context)
        {
            Console.WriteLine("Add Book or Author?");
            Console.WriteLine("1: Book\n2: Author\n3:Abort");
            var menuChoice = InputReader.SingleKey(3);
            switch (menuChoice)
            {
                case '1':
                    
                    string author = Console.ReadLine();
                    break;
                case '2':
                    break;
                case '3':
                    break;
            }

        }

        public static void RemoveData(MyBookStoreContext context)
        {

        }

        public static void UpdateStock(MyBookStoreContext context)
        {
            int? storeIdOut;
            Fetcher.StockStatus(context, "Please select which store to update stock for.", out storeIdOut);
            if (storeIdOut != null)
            {
                var currentStore = context.Stores.SingleOrDefault(s => s.StoreId == storeIdOut);
                Console.WriteLine("Please enter the ISBN you wish to update stock for");
                string input = Console.ReadLine();
                var selectedStock = context.StockStatuses
                    .SingleOrDefault(stock => stock.Isbn13 == input && stock.StoreId == currentStore.StoreId);

                if (selectedStock != null)
                {
                    Console.WriteLine($"Found ISBN: {selectedStock.Isbn13}, Current Stock: {selectedStock.CurrentStock}");
                    Console.WriteLine("Please enter the new stock quantity:");

                    string stockInput = Console.ReadLine();

                    
                    if (int.TryParse(stockInput, out int newStock))
                    {
                        if(stockInput == "0")
                        {
                            Console.WriteLine("Do you want to remove this book from this store's stock-list entirely?\n1: Yes\n2: No");
                            var choice = InputReader.SingleKey(2);
                            if(choice == '1')
                            {
                                DatabaseManager.Remove(selectedStock, context);
                                Console.WriteLine("Book has been removed");
                            }
                        }
                        else if(stockInput != "0")
                        {
                            selectedStock.CurrentStock = newStock;
                            context.SaveChanges();
                            Console.WriteLine("Stock updated successfully!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number for the stock quantity.");
                    }
                }
                else
                {
                    Console.WriteLine("The entered ISBN is not found for the selected store.");
                }
            }
        }
    }
}
