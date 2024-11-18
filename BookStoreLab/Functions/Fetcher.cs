using BookStoreLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookStoreLab.Functions
{
    public static class Fetcher
    {
        public static void StockStatus(MyBookStoreContext context, string prompt, out int? storeIdOut)
        {
            Console.WriteLine(prompt);
            storeIdOut = null;
            var stores = context.Stores.ToList();
            foreach (var store in stores)
            {
                Console.WriteLine($"Store: {store.StoreName}\nStoreID: {store.StoreId}");
            }

            while (true)
            {
                int? storeId = InputReader.GetValidIntegerInput("Enter the Store ID, or input 'Q' to leave: ");

                if (!storeId.HasValue)
                {
                    Writer.AnyKeyReturn();
                    Console.Clear();
                    break;
                }

                var storeList = context.Stores
                    .Include(s => s.StockStatuses)
                        .ThenInclude(ss => ss.Isbn13Navigation)
                            .ThenInclude(sss => sss.Author)
                    .FirstOrDefault(s => s.StoreId == storeId);
                
                if (storeList != null)
                {
                    Console.Clear();
                    Console.WriteLine($"{storeList.StoreName} stock info:");
    
                    foreach (var stockStatus in storeList.StockStatuses)
                    {
                        var stock = stockStatus.CurrentStock == 0 ? "Out of Stock" : stockStatus.CurrentStock.ToString();
                        var bookTitle = stockStatus.Isbn13Navigation.Title;
                        var bookAuthor = stockStatus.Isbn13Navigation.Author.FirstName + " " + stockStatus.Isbn13Navigation.Author.LastName;
                        Console.WriteLine($"Author: {bookAuthor}\nTitle: {bookTitle}\nISBN: {stockStatus.Isbn13}, Quantity: {stock}\n");
                    }
                    storeIdOut = storeId;
                    break; 
                }
                else
                {
                    Console.WriteLine("Store not found. Please enter a valid Store ID.");
                }
            }
        }
    }
}
