using Discord;
using System.Xml.Serialization;
using static WizBotLibraryV3.Modules.Logger;

namespace WizBotLibraryV3.Modules
{
  public class GogArchiver
  {
    WizBot Bot {  get; set; }
    public List<GogRecord> Gogs { get; set; }

    public GogArchiver(WizBot Bot) {
      this.Bot = Bot;
      Gogs = [];
    }
    public void Add(GogRecord record) => Gogs.Add(record);
    public void Add(ulong MessageId, string FilePath, string RemoteResource, ulong GogNum) => Gogs.Add(new GogRecord(MessageId, FilePath, RemoteResource, GogNum));

    public async Task Archive(IAsyncEnumerable<IMessage> messages) {
      XmlSerializer xml = new(typeof(GogXml));
      List<GogRecord> Downloaded;
      if (File.Exists($"{Directory.GetCurrentDirectory()}/Gogs/Gogs.xml"))
      {
        var readStream = File.OpenRead($"{Directory.GetCurrentDirectory()}/Gogs/Gogs.xml");
        Downloaded = ((GogXml)xml.Deserialize(readStream))?.Gogs;
        readStream.Close();
      }
      List<GogRecord> GogsBuffer = new();

      await foreach(var message in messages)
      {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
      var random = new Random();
        var record = new GogRecord()
        {
          MessageId = message.Id,
          FilePath = new string(Enumerable.Repeat(chars, 20).Select(s => s[random.Next(s.Length)]).ToArray())
        };

        if (message.Content.StartsWith("https://"))
        {
          record.RemoteResource = message.Content;
        }
        if (message.Attachments.Count() > 0) {  
          record.FilePath += "_" + message.Attachments.First().Filename;
          record.RemoteResource = message.Attachments.First().Url;
        }

        if (record.RemoteResource != null && record.MessageId != null && record.FilePath != null)
        {
          GogsBuffer.Add(record);
        }
      }
      _ = Bot.Logger.Debug($"We have {GogsBuffer.Count()}, and there are {await messages.CountAsync()} messages. Attempting to download...");

      HttpClient client = new();
      foreach (var gog in GogsBuffer) {
        try
        {
          _= Bot.Logger.Debug($"Downloading gog from url: \"{gog.RemoteResource}\"");
          var stream = File.OpenWrite($"{Directory.GetCurrentDirectory()}/Gogs/{gog.FilePath!}");
          var filestream = await client.GetStreamAsync(gog.RemoteResource!);
          filestream.CopyTo(stream);
          stream.Close();
          Gogs.Add(gog);

        } catch ( Exception e ) {
          gog.GogDownloadFailed = true;
        }
      }

      var stream2 = File.OpenWrite($"{Directory.GetCurrentDirectory()}/Gogs/Gogs.xml");
      xml.Serialize(stream2, new GogXml(Gogs));
      stream2.Close();
    }
  }

  [XmlRoot]
  public class GogXml {
    public GogXml() { }
    public GogXml(List<GogRecord> Gogs) { this.Gogs = Gogs; }

    [XmlElement]
    public List<GogRecord>? Gogs { get; set; }
  }
  public class GogRecord
  {
    [XmlAttribute]
    public ulong MessageId { get; set; } = ulong.MinValue;
    [XmlAttribute]
    public string FilePath { get; set; } = string.Empty;
    [XmlAttribute]
    public string RemoteResource { get; set; } = string.Empty;
    [XmlAttribute]
    public ulong GogNum { get; set; } = ulong.MinValue;
    [XmlAttribute]
    public bool GogDownloadFailed { get; set; } = false;

    public GogRecord() { }
    public GogRecord(ulong MessageId, string FilePath, string RemoteResource, ulong GogNum) {
      this.MessageId = MessageId;
      this.FilePath = FilePath;
      this.RemoteResource = RemoteResource;
      this.GogNum = GogNum;
    }
  }
}
