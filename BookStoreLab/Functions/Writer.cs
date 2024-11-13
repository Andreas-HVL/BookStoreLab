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
       
        public static void AnyKeyReturn()
        {
            Console.WriteLine("Press any key to return to Main Menu");
            Console.ReadKey();
        }
    }
}
