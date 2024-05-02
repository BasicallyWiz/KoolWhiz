using System.Xml.Serialization;

namespace WizBotLibraryV3.Modules
{
  [XmlRoot]
  public class BotData
  {
    [XmlAttribute]
    public string ClientToken { get; set; } = "Client Token";
    [XmlElement]
    public GogcordData GogcordData { get; set; } = new();
    [XmlAttribute]
    public ulong ManagementServer { get; set; } = ulong.MaxValue;
    [XmlAttribute]
    public string CommandsDirectory { get; set; } = "/Directory/";
    [XmlAttribute]
    public ulong OwnerId { get; set; } = 324588568951390220;
  }

  public class GogcordData {
    [XmlAttribute]
    public ulong GogCountChannel { get; set; } = ulong.MaxValue;
    [XmlAttribute]
    public ulong ImmediatelyChannel { get; set; } = ulong.MaxValue;
    [XmlAttribute]
    public ulong GogSeriesChannel { get; set; } = ulong.MaxValue;
    [XmlAttribute]
    public string SuccessReaction { get; set; } = ":gogHappy:831979949036273715";
  }
}
