using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookStoreLab.Models;
using System.Reflection;

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
            Console.Clear();
            Console.WriteLine("Add Book or Author?");
            Console.WriteLine("1: Book\n2: Author\n3: Abort");
            var menuChoice = InputReader.SingleKey(3);
            switch (menuChoice)
            {
                case '1':
                    while (true)
                    {
                        Fetcher.ListAuthors(context);
                        int? authorId = InputReader.GetValidIntegerInput("Please write the Author ID of the Author you'd like to add a book for, or write 'Q' to leave: ");
                        if (!authorId.HasValue)
                        {
                            Writer.AnyKeyReturn();
                            Console.Clear();
                            break;
                        }

                        var chosenAuthor = context.Authors.FirstOrDefault(a => a.AuthorId == authorId);
                        if (chosenAuthor == null)
                        {
                            Console.Clear();
                            Console.WriteLine("Author not found. Please enter a valid Author ID.");
                            continue; 
                        }

                        while (true)
                        {
                            Fetcher.ListPublishers(context);
                            int? publisherId = InputReader.GetValidIntegerInput("Please write the Publisher ID of the Publisher you'd like to add a book for, or write 'Q' to leave: ");
                            if (!publisherId.HasValue)
                            {
                                Writer.AnyKeyReturn();
                                Console.Clear();
                                break;
                            }

                            var chosenPublisher = context.Publishers.FirstOrDefault(p => p.PublisherId == publisherId);
                            if (chosenPublisher == null)
                            {
                                Console.Clear();
                                Console.WriteLine("Publisher not found. Please enter a valid Publisher ID.");
                                continue; 
                            }
                            Console.Clear();
                            Console.WriteLine($"{chosenAuthor.FirstName} {chosenAuthor.LastName}, published by: {chosenPublisher.PublisherName}");


                            Console.WriteLine("Please enter the ISBN (Cannot be empty):");
                            string isbn;
                            while (string.IsNullOrWhiteSpace(isbn = Console.ReadLine()))
                            {
                                Console.WriteLine("ISBN cannot be empty. Please enter a valid ISBN:");
                            }

                            Console.WriteLine("Please enter the book title (Cannot be empty):");
                            string title;
                            while (string.IsNullOrWhiteSpace(title = Console.ReadLine()))
                            {
                                Console.WriteLine("Title cannot be empty. Please enter a valid title:");
                            }

                            Console.WriteLine("Please enter the book's language (Cannot be empty):");
                            string language;
                            while (string.IsNullOrWhiteSpace(language = Console.ReadLine()))
                            {
                                Console.WriteLine("Language cannot be empty. Please enter a valid language:");
                            }

                            Console.WriteLine("Please enter the price (e.g., 19.99):");
                            decimal price = InputReader.GetValidDecimalInput("Please enter a valid price:");

                            Console.WriteLine("Please enter the release date (format: YYYY-MM-DD):");
                            DateOnly releaseDate;
                            while (!DateOnly.TryParse(Console.ReadLine(), out releaseDate))
                            {
                                Console.WriteLine("Invalid date format. Please enter a valid release date (format: YYYY-MM-DD):");
                            }

                            Console.WriteLine("Please enter the genre (optional, press Enter to skip):");
                            string? genre = Console.ReadLine();
                            genre = string.IsNullOrWhiteSpace(genre) ? null : genre;

                            int? pages = InputReader.GetValidIntegerInput("Please enter the number of pages (optional, press Enter to skip):");
                            
                            var newBook = new Book
                            {
                                Isbn13 = isbn,
                                Title = title,
                                Language = language,
                                Price = price,
                                ReleaseDate = releaseDate,
                                Genre = genre,
                                Pages = pages,
                                AuthorId = authorId.Value,
                                PublisherId = publisherId.Value
                            };

                            context.Books.Add(newBook);
                            context.SaveChanges();

                            Console.WriteLine($"Book '{title}' successfully added to the database.");
                        }
                    }
                    break;

                case '2':
                    Console.Clear();
                    Console.WriteLine("Please enter the author's first name (Cannot be empty):");
                    string firstName;
                    while (string.IsNullOrWhiteSpace(firstName = Console.ReadLine()))
                    {
                        Console.WriteLine("First name cannot be empty. Please enter a valid first name:");
                    }

                    Console.WriteLine("Please enter the author's last name (Cannot be empty):");
                    string lastName;
                    while (string.IsNullOrWhiteSpace(lastName = Console.ReadLine()))
                    {
                        Console.WriteLine("Last name cannot be empty. Please enter a valid last name:");
                    }

                    var existingAuthor = context.Authors.FirstOrDefault(a => a.FirstName == firstName && a.LastName == lastName);
                    if (existingAuthor != null)
                    {
                        Console.WriteLine($"An author with the name '{firstName} {lastName}' already exists.");
                        Writer.AnyKeyReturn();
                        Console.Clear();
                        return; 
                    }
                    
                    Console.WriteLine("Please enter the author's date of birth (format: YYYY-MM-DD):");
                    DateOnly dateOfBirth;
                    while (!DateOnly.TryParse(Console.ReadLine(), out dateOfBirth))
                    {
                        Console.WriteLine("Invalid date format. Please enter a valid date of birth (format: YYYY-MM-DD):");
                    }

                    var newAuthor = new Author
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        DateOfBirth = dateOfBirth
                    };

                    context.Authors.Add(newAuthor);
                    context.SaveChanges();

                    Console.WriteLine($"Author '{firstName} {lastName}' successfully added to the database.");
                    Writer.AnyKeyReturn();
                    Console.Clear();
                    break;
                case '3':
                    break;
            }

        }

        public static void RemoveData(MyBookStoreContext context)
        {
            Console.Clear();
            Console.WriteLine("Remove Book or Author?");
            Console.WriteLine("1: Book\n2: Author\n3: Abort");
            var menuChoice = InputReader.SingleKey(3);
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
