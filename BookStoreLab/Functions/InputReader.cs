using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLab.Functions
{
    public static class InputReader
    {
        public static char SingleKey(int maxValidNumber)
        {
            ConsoleKeyInfo cki;
            char keyChar;

            do
            {
                cki = Console.ReadKey(true);
                keyChar = cki.KeyChar;
            }
            while (!IsValidKey(keyChar, maxValidNumber));

            return keyChar;
        }

        private static bool IsValidKey(char keyChar, int maxValidNumber)
        {
            // Check if the character is between '1' and the max number in char form
            return keyChar >= '1' && keyChar <= '0' + maxValidNumber;
        }
    }
}
