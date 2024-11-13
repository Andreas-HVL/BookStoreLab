using BookStoreLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLab.Functions
{
    public static class Writer
    {
        public static void Welcome()
        {
            Console.WriteLine("Welcome\nWhat changes are you looking to make?");
            Console.WriteLine("1: Check stock");
            Console.WriteLine("2: Update stock");
            Console.WriteLine("3: Add data");
            Console.WriteLine("4: Remove data");
            Console.WriteLine("5: Close program");
        }

       
        public static void LoggedInMenu()
        {
            Console.WriteLine("Welcome");
            Console.WriteLine("Select from the below options");
            Console.WriteLine("1: View user info");
            Console.WriteLine("2: View cart");
            Console.WriteLine("3: Add items to cart");
            Console.WriteLine("4: Logout");
            Console.WriteLine("5: Checkout");
        }
        public static void AnyKeyReturn()
        {
            Console.WriteLine("Press any key to return to Main Menu");
            Console.ReadKey();
        }
        public static void ExitMenu()
        {
            Console.WriteLine("Processing Payment");
            foreach (char c in ".......")
            {
                Console.Write(c);
                Thread.Sleep(200);
            }
            Console.WriteLine("\nThanks for shopping, goodbye!");
            Console.WriteLine("Please press any key to close the store");
            Console.ReadKey();
        }
    }
}
