using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizBotExecutable.Modules
{
    public class Logging
    {
        public static async Task OnInfo(string text)
        {
            Console.WriteLine($"{text}");
        }
        public static async Task OnDebug(string text)
        {
            Console.WriteLine(text);
        }
        public static async Task OnWarn(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static async Task OnSetup(string text)
        {
            Console.WriteLine(text);
        }
    }
}
