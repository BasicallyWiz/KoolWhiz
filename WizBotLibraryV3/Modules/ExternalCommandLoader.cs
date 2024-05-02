using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using WizBotLibraryV3.SlashCommands;
using System.Reflection;
using Microsoft.CodeAnalysis.Emit;
using Discord;
using Discord.WebSocket;

namespace WizBotLibraryV3.Modules
{
  public class ExternalCommandLoader
  {
    WizBot Bot { get; init; }
    List<SlashCommand> SlashCommands { get; set; } = new();
    public List<ApplicationCommandProperties> AppCommands { get; set; } = new();

    string codeFolderPath = $"{Directory.GetCurrentDirectory()}/Commands/";

    public ExternalCommandLoader(WizBot Bot)
    {
      this.Bot = Bot;
      Bot.Client.SlashCommandExecuted += HandleCommand;
    }

    private async Task HandleCommand(SocketSlashCommand cmd)
    {
      _ = Bot.Logger.Debug($"Slash command executed: {cmd.Data.Name}");
      await SlashCommands.Where(x => x.Command().Name.Value == cmd.Data.Name).First().Execute(cmd);
    }
    public void LoadCommands()
    {
      Bot.Logger?.Info("Flushing Commands...");
      SlashCommands.Clear();
      AppCommands.Clear();

      Bot.Logger?.Info("Loading Files...");
      // Get all the C# files in the code folder
      string[] csFiles = Directory.GetFiles(codeFolderPath, "*.cs");

      Bot.Logger?.Info("Creating Compiler...");
      // Create a compilation object
      var compilation = CSharpCompilation.Create("MyCompilation")
          .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
          .AddReferences(MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location))
          .AddReferences(MetadataReference.CreateFromFile(Assembly.Load("System.Private.CoreLib").Location))
          .AddReferences(MetadataReference.CreateFromFile(Assembly.Load("System.Collections").Location))
          .AddReferences(MetadataReference.CreateFromFile(typeof(WizBot).Assembly.Location))
          .AddReferences(MetadataReference.CreateFromFile(Assembly.Load("Discord.Net.Commands").Location))
          .AddReferences(MetadataReference.CreateFromFile(Assembly.Load("Discord.Net.Core").Location))
          .AddReferences(MetadataReference.CreateFromFile(Assembly.Load("Discord.Net.WebSocket").Location))
          .AddReferences(MetadataReference.CreateFromFile(Assembly.Load("Discord.Net.Rest").Location));

      Bot.Logger?.Info("Adding Syntax Trees...");
      // Add the C# files to the compilation
      foreach (var csFile in csFiles)
      {
        Bot.Logger?.Info($"  {csFile}");
        var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(csFile));
        compilation = compilation.AddSyntaxTrees(syntaxTree);
      }

      Bot.Logger?.Info("Emitting Assembly...");
      // Emit the compiled assembly
      using (var ms = new MemoryStream())
      {
        EmitResult result = compilation.Emit(ms);

        if (!result.Success)
        {
          // Handle compilation errors
          IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
              diagnostic.IsWarningAsError ||
              diagnostic.Severity == DiagnosticSeverity.Error);

          foreach (Diagnostic diagnostic in failures)
          {
            Bot.Logger?.Warn(diagnostic.GetMessage());
          }
        }
        else
        {
          Bot.Logger?.Info("Loading Assembly...");
          // Load the compiled assembly
          ms.Seek(0, SeekOrigin.Begin);
          Assembly assembly = Assembly.Load(ms.ToArray());

          Bot.Logger?.Info("Getting Types...");
          var typesWithMyAttribute = assembly.GetTypes()
            .Where(t => t.BaseType == typeof(SlashCommand));

          foreach (var type in typesWithMyAttribute)
          {
            var instance = Activator.CreateInstance(type, args: [Bot]) as SlashCommand; // Create an instance of the type
            if (instance is null) continue;
            Bot.Logger?.Info($"Adding Type: {type.Name}");
            SlashCommands.Add(instance);
            instance!.Setup();
            AppCommands.Add(instance!.Command());
          }
          Bot.Logger?.Info($"Finished loading commands successfully, with {SlashCommands.Count()} new commands");
        }
      }
    }
    public void WriteCommands()
    {
      if (Bot.IsDebug)
      {
        Bot.Logger?.Info("Debug mode enabled, writing commands to management server");
        Bot.Client.BulkOverwriteGlobalApplicationCommandsAsync(null);
        Bot.Client.GetGuild(Bot.StartupData.ManagementServer).BulkOverwriteApplicationCommandAsync(AppCommands.ToArray());
      }
      else
      {
        Bot.Logger?.Info("Writing global commands");
        Bot.Client.BulkOverwriteGlobalApplicationCommandsAsync(AppCommands.ToArray());
      }
    }
  }
}
