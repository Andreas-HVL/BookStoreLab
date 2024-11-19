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
                    Console.Clear();
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
                            break;
                        }
                        break;
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
            switch (menuChoice)
            {
                case '1':
                    Console.Clear();
                    Fetcher.ListBooks(context);
                    Console.WriteLine("Select a book (ISBN) to remove from the store.");
                    Console.WriteLine("NOTE: THIS WILL REMOVE ANY RELATIONS TO THIS BOOK FROM THE DATABASE, INCLUDING ORDER-DATA, USE SPARINGLY");

                    string bookChoice;
                    var chosenBook = default(Book);
                    do
                    {
                        Console.Write("Enter the ISBN of the Book");
                        bookChoice = Console.ReadLine()?.Trim();
                        if(string.IsNullOrEmpty(bookChoice))
                        {
                            Console.WriteLine("ISBN cannot be empty. Please try again.");
                            continue;
                        }
                        chosenBook = context.Books.FirstOrDefault(b => b.Isbn13 == bookChoice);

                        if (chosenBook == null)
                        {
                            Console.WriteLine("Invalid ISBN, please try again.");
                        }
                    }
                    while (chosenBook == null);

                    Console.WriteLine($"Book '{chosenBook.Title}' with ISBN {chosenBook.Isbn13} selected for removal.");
                    Console.WriteLine("Please confirm deletion. THIS CANNOT BE UNDONE!");
                    Console.WriteLine("1: Confirm\n2: Abort");
                    var confirm = InputReader.SingleKey(2);
                    switch (confirm)
                    {
                        case '1':
                            try
                            {
                                context.Books.Remove(chosenBook);
                                context.SaveChanges();
                                Console.WriteLine("Book removed successfully");
                            }
                            catch (Exception ex) { Console.WriteLine($"An error occurred while removing the book: {ex.Message}"); }
                            break;
                        case '2':
                            Console.WriteLine("Book removal aborted");
                            break;
                    }
                    break;

                case '2':
                    Console.Clear();
                    Fetcher.ListAuthors(context);
                    Console.WriteLine("Select an author (Author ID) to remove from the database.");
                    Console.WriteLine("NOTE: THIS WILL REMOVE ANY BOOK ASSOCIATED WITH THIS AUTHOR (AND ANY ASSOCIATIONS TO THOSE BOOKS). USE SPARINGLY.");

                    string authorChoice;
                    var chosenAuthor = default(Author);
                    do
                    {
                        Console.Write("Enter the Author ID: ");
                        authorChoice = Console.ReadLine()?.Trim();
                        if (string.IsNullOrEmpty(authorChoice))
                        {
                            Console.WriteLine("Author ID cannot be empty. Please try again.");
                            continue;
                        }

                        if (!int.TryParse(authorChoice, out int authorId))
                        {
                            Console.WriteLine("Invalid input. Please enter a valid numeric Author ID.");
                            continue;
                        }

                        chosenAuthor = context.Authors.FirstOrDefault(a => a.AuthorId == authorId);

                        if (chosenAuthor == null)
                        {
                            Console.WriteLine("Invalid Author ID, please try again.");
                        }
                    }
                    while (chosenAuthor == null);

                    Console.WriteLine($"Author '{chosenAuthor.FirstName} {chosenAuthor.LastName}' (ID: {chosenAuthor.AuthorId}) selected for removal.");
                    Console.WriteLine("Please confirm deletion. THIS CANNOT BE UNDONE!");
                    Console.WriteLine("1: Confirm\n2: Abort");
                    var authorConfirm = InputReader.SingleKey(2);
                    switch (authorConfirm)
                    {
                        case '1':
                            try
                            {
                                context.Authors.Remove(chosenAuthor);
                                context.SaveChanges();
                                Console.WriteLine("Author and all associated books removed successfully.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred while removing the author: {ex.Message}");
                            }
                            break;
                        case '2':
                            Console.WriteLine("Author removal aborted.");
                            break;
                    }
                    break;
                case '3':
                    break;
            }
        }

        public static void UpdateStock(MyBookStoreContext context)
        {
            int? storeIdOut;
            Fetcher.StockStatus(context, "Please select which store to update stock for.", out storeIdOut);

            if (storeIdOut != null)
            {
                Fetcher.ListBooks(context);
                var currentStore = context.Stores.SingleOrDefault(s => s.StoreId == storeIdOut);
                Console.WriteLine("");
                Console.WriteLine("Please enter the ISBN you wish to update stock for:");
                string input = Console.ReadLine()?.Trim();

                var selectedStock = context.StockStatuses
                    .SingleOrDefault(stock => stock.Isbn13 == input && stock.StoreId == currentStore.StoreId);

                if (selectedStock != null)
                {
                    // Stock already exists for this ISBN in the selected store
                    Console.WriteLine($"Found ISBN: {selectedStock.Isbn13}, Current Stock: {selectedStock.CurrentStock}");
                    Console.WriteLine("Please enter the new stock quantity:");

                    string stockInput = Console.ReadLine();

                    if (int.TryParse(stockInput, out int newStock))
                    {
                        if (newStock == 0)
                        {
                            Console.WriteLine("Do you want to remove this book from this store's stock-list entirely?\n1: Yes\n2: No");
                            var choice = InputReader.SingleKey(2);
                            if (choice == '1')
                            {
                                DatabaseManager.Remove(selectedStock, context);
                                Console.WriteLine("Book has been removed from the store.");
                            }
                        }
                        else
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
                    // Stock does not exist for the selected ISBN in the selected store
                    Console.WriteLine("The entered ISBN is not found in this store. Would you like to add it? \n1: Yes\n2: No");
                    var choice = InputReader.SingleKey(2);
                    switch (choice)
                    {
                        case '1':
                            // Check if the book exists in the Books table
                            var book = context.Books.SingleOrDefault(b => b.Isbn13 == input);
                            if (book != null)
                            {
                                Console.WriteLine("Please enter the initial stock quantity:");
                                string stockInput = Console.ReadLine();

                                if (int.TryParse(stockInput, out int newStock) && newStock > 0)
                                {
                                    var newStockEntry = new StockStatus
                                    {
                                        StoreId = currentStore.StoreId,
                                        Isbn13 = book.Isbn13,
                                        CurrentStock = newStock
                                    };

                                    context.StockStatuses.Add(newStockEntry);
                                    context.SaveChanges();
                                    Console.WriteLine($"Book '{book.Title}' has been added to the store with an initial stock of {newStock}.");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid stock quantity. Operation canceled.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("The entered ISBN does not exist in the catalog. Operation canceled.");
                            }
                            break;
                        case '2':
                            Console.WriteLine("Operation canceled.");
                            break;
                    }
                }
            }
        }

        public static void UpdateData(MyBookStoreContext context)
        {
            Console.Clear();
            Console.WriteLine("Update Book or Author?");
            Console.WriteLine("1: Book\n2: Author\n3: Abort");
            var menuChoice = InputReader.SingleKey(3);
            switch (menuChoice)
            {
                case '1':
                    Fetcher.ListBooks(context);
                    Book chosenBook = null;
                    while (true)
                    {
                        Console.Write("Enter the ISBN of the book you'd like to edit or write 'Q' to leave: ");
                        string bookChoice = Console.ReadLine()?.Trim();

                        if (bookChoice?.ToUpper() == "Q")
                        {
                            Console.Clear();
                            return; // Exit the function completely if the user chooses to quit
                        }

                        if (string.IsNullOrEmpty(bookChoice))
                        {
                            Console.WriteLine("ISBN cannot be empty. Please try again.");
                            continue;
                        }

                        chosenBook = context.Books.FirstOrDefault(b => b.Isbn13 == bookChoice);
                        if (chosenBook == null)
                        {
                            Console.WriteLine("Invalid ISBN, please try again.");
                        }
                        else
                        {
                            break; // Exit the loop when a valid book is selected
                        }
                    }


                    Console.WriteLine("Please write the new title of the book, leave empty if no changes:");
                    string title = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        chosenBook.Title = title; // Update the book's title directly
                    }

                    Console.WriteLine("Please write the new language of the book, leave empty if no changes:");
                    string language = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        chosenBook.Language = language; // Update the book's language directly
                    }

                    Console.WriteLine("Please write the new Release Date of the book, leave empty if no changes (format: YYYY-MM-DD):");
                    string dateInput = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(dateInput))
                    {
                        if (DateOnly.TryParse(dateInput, out DateOnly releaseDate))
                        {
                            chosenBook.ReleaseDate = releaseDate; // Update the release date if valid
                        }
                        else
                        {
                            Console.WriteLine("Invalid date format. No changes were made to the release date.");
                        }
                    }

                    Console.WriteLine("Please write the new amount of pages, leave empty if no changes (if not a valid integer, no changes will be made)");
                    string pagesInput = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(pagesInput) && int.TryParse(pagesInput, out int pages))
                    {
                        chosenBook.Pages = pages;
                    }
                    else if (!string.IsNullOrWhiteSpace(pagesInput))
                    {
                        Console.WriteLine("Invalid page count. No changes were made.");
                    }

                    Console.WriteLine("Please write the new price (e.g. 19,99), leave empty if no changes (If not a valid decimal, no changes will be made)");
                    string priceInput = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(priceInput) && decimal.TryParse(priceInput, out decimal price))
                    {
                        chosenBook.Price = price;
                    }
                    else if (!string.IsNullOrWhiteSpace(priceInput))
                    {
                        Console.WriteLine("Invalid price format. No changes were made.");
                    }

                    Console.WriteLine("Please write the new genre, leave empty if no changes");
                    string genre = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(genre))
                    {
                        chosenBook.Genre = genre;
                    }
                    context.SaveChanges();
                    Console.WriteLine($"Book updated, the following is the data from the database:\nTitle: {chosenBook.Title}\nLanguage: {chosenBook.Language}\nRelease Date: {chosenBook.ReleaseDate}\nNumber of Pages: {chosenBook.Pages}\nPrice: {chosenBook.Price}\nGenre: {chosenBook.Genre}");
                    break;

                case '2':
                    while (true)
                    {
                        Fetcher.ListAuthors(context);
                        int? authorId = InputReader.GetValidIntegerInput("Please write the Author ID of the Author you'd like to edit, or write 'Q' to leave: ");

                        if (!authorId.HasValue)
                        {
                            Console.Clear();
                            return;
                        }

                        var chosenAuthor = context.Authors.FirstOrDefault(a => a.AuthorId == authorId);
                        if (chosenAuthor == null)
                        {
                            Console.Clear();
                            Console.WriteLine("Author not found. Please enter a valid Author ID.");
                            continue;
                        }

                        Console.WriteLine("Please enter the new first name for the author (If no changes, leave blank)");
                        string firstName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(firstName))
                        {
                            chosenAuthor.FirstName = firstName;
                        }

                        Console.WriteLine("Please enter the new last name for the author (if no changes, leave blank)");
                        string lastName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(lastName))
                        {
                            chosenAuthor.LastName = lastName;
                        }

                        Console.WriteLine("Enter the new date of birth for the author (format: YYYY-MM-DD) (if no changes, leave blank, and if wrong format is used, no changes will be made)");
                        string birthDateInput = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(birthDateInput))
                        {
                            if (DateOnly.TryParse(birthDateInput, out  DateOnly dateOfBirth))
                            {
                                chosenAuthor.DateOfBirth = dateOfBirth; // Update only if valid
                            }
                            else
                            {
                                Console.WriteLine("Invalid date format. No changes were made to the date of birth.");
                            }
                        }
                        context.SaveChanges();
                        Console.WriteLine($"Author updated to the following info:\nFull Name: {chosenAuthor.FirstName} {chosenAuthor.LastName}\nDate of Birth: {chosenAuthor.DateOfBirth}");
                        break;
                    }
                    break;
            }
        }
    }
}
