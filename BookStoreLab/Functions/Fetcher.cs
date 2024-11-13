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
        public static void StockStatus(MyBookStoreContext context)
        {
            
            Console.WriteLine("Please select which store to check stock status for.");
            var stores = context.Stores.ToList();
            foreach (var store in stores)
            {
                Console.WriteLine($"Store: {store.StoreName}\nStoreID: {store.StoreId}");
            }
            int output = Convert.ToInt32(Console.ReadLine());
            
            Console.Clear();
            var storeList = context.Stores
                        .Include(s => s.StockStatuses)
                            .ThenInclude(ss => ss.Isbn13Navigation)
                                .ThenInclude(sss => sss.Author)
                        .FirstOrDefault(s => s.StoreId == output);
            if (storeList != null)
            {
                Console.WriteLine($"{storeList.StoreName} stock info:");

                foreach (var stockStatus in storeList.StockStatuses)
                {
                    var bookTitle = stockStatus.Isbn13Navigation.Title;
                    var bookAuthor = stockStatus.Isbn13Navigation.Author.FirstName + " " + stockStatus.Isbn13Navigation.Author.LastName;
                    Console.WriteLine($"Author: {bookAuthor}\nTitle: {bookTitle}\nISBN: {stockStatus.Isbn13}, Quantity: {stockStatus.CurrentStock}\n");
                }
            }
            else
            {
                Console.WriteLine("Store not found.");
            }
            Console.WriteLine("Press any key to return.");
            Console.ReadKey();
        }
    }
}
