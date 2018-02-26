using System;
using System.Linq;

namespace WUDownloader
{
    public static class ConsoleInput
    {
        public static bool YesOrNo()
        {
            string _val = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && _val.Length == 0 &&
                    (key.KeyChar.Equals('Y') || key.KeyChar.Equals('y') ||
                    key.KeyChar.Equals('N') || key.KeyChar.Equals('n')))
                {
                    _val += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && _val.Length > 0)
                    {
                        _val = _val.Substring(0, (_val.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter || _val.Length == 0);

            if (_val.Equals("Y") || _val.Equals("y"))
            {
                Console.WriteLine("");
                return true;
            }
            else
            {
                Console.WriteLine("");
                return false;
            }
        }
        public static string StringInput()
        {
            string _val = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                //Accepts all characters that aren't Backspace or Enter
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    _val += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && _val.Length > 0)
                    {
                        _val = _val.Substring(0, (_val.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter || _val.Length == 0);
            Console.WriteLine("");
            return _val;
        }
        public static string StringInputAllowEmpty()
        {
            string _val = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                //Accepts all characters that aren't Backspace or Enter
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    _val += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && _val.Length > 0)
                    {
                        _val = _val.Substring(0, (_val.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");
            return _val;
        }
        public static int PositiveInteger()
        {
            string _val = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                //Only allow numbers 0-9
                if (key.Key != ConsoleKey.Backspace && (key.KeyChar == '0' || key.KeyChar == '1' ||
                    key.KeyChar == '2' || key.KeyChar == '3' || key.KeyChar == '4' || key.KeyChar == '5' ||
                    key.KeyChar == '6' || key.KeyChar == '7' || key.KeyChar == '8' || key.KeyChar == '9'))
                {
                    //double val = 0;
                    bool _x = Int32.TryParse(key.KeyChar.ToString(), out int val);
                    if (_x)
                    {
                        _val += key.KeyChar;
                        Console.Write(key.KeyChar);
                    }
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && _val.Length > 0)
                    {
                        _val = _val.Substring(0, (_val.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter || _val.Length == 0);

            int input = Int32.Parse(_val);
            Console.WriteLine("");
            return input;
        }
        public static int PositiveIntegerAllowEmpty()
        {
            string _val = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                //Only allow numbers 0-9
                if (key.Key != ConsoleKey.Backspace && (key.KeyChar == '0' || key.KeyChar == '1' ||
                    key.KeyChar == '2' || key.KeyChar == '3' || key.KeyChar == '4' || key.KeyChar == '5' ||
                    key.KeyChar == '6' || key.KeyChar == '7' || key.KeyChar == '8' || key.KeyChar == '9'))
                {
                    //double val = 0;
                    bool _x = Int32.TryParse(key.KeyChar.ToString(), out int val);
                    if (_x)
                    {
                        _val += key.KeyChar;
                        Console.Write(key.KeyChar);
                    }
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && _val.Length > 0)
                    {
                        _val = _val.Substring(0, (_val.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);

            int input = -1;
            if (_val.Length > 0)
            {
                input = Int32.Parse(_val);
            }
            Console.WriteLine("");
            return input;
        }
        public static char MappingSelection()
        {
            char[] acceptedChars = new char[] { 'M', 'm', 'I', 'i', 'S', 's', 'F', 'f' };
            string _val = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && _val.Length == 0 &&
                    (acceptedChars.Contains(key.KeyChar)))
                {
                    _val += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && _val.Length > 0)
                    {
                        _val = _val.Substring(0, (_val.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter || _val.Length == 0);
            _val = _val.ToUpper();
            Console.WriteLine("");
            return _val[0];
        }
    }
}
