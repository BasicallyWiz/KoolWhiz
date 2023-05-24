using WizBotLibrary;

public class WizBotExecutable
{
  public static void Main(string[] args)
  {
    WizBot Bot = new WizBot(args);
    if (Bot.IsDebugMode) Bot.logger.OnDebug += OnDebug;
    Bot.logger.OnWarning += OnWarn;
    Bot.logger.OnInfo += OnInfo;
    Bot.logger.OnSetup += OnSetup;
    Bot.MainAsync(args).Wait();
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
  public static async Task OnSetup(string text)
  {
    Console.WriteLine(text);
  }
}