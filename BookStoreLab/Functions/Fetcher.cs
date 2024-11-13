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
        public static void StockStatus(MyBookStoreContext context, int storeId)
        {
            Console.Clear();
            var store = context.Stores
                        .Include(s => s.StockStatuses)
                            .ThenInclude(ss => ss.Isbn13Navigation)
                                .ThenInclude(sss => sss.Author)
                        .FirstOrDefault(s => s.StoreId == storeId);
            if (store != null)
            {
                Console.WriteLine($"{store.StoreName} stock info:");

                foreach (var stockStatus in store.StockStatuses)
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
