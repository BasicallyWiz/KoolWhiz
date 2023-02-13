using WizBotLibrary;

public class WizBotExecutable
{
  public static void Main()
  {
    WizBot Bot = new WizBot();
    Bot.logger.OnDebug += OnDebug;
    Bot.logger.OnWarning += OnWarn;
    Bot.MainAsync().Wait();
  }
  public static async Task OnInfo(string text)
  {
    Console.WriteLine($"{text}");
  }
  public static async Task OnDebug(string text)
  {
    Console.WriteLine(text);
  }
  public static async Task OnWarn(string text) {
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(text);
    Console.ForegroundColor = ConsoleColor.White;
  }
}