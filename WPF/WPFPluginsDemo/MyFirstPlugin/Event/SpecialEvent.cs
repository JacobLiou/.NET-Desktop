namespace MyFirstPlugin.Event;

public class SpecialEvent
{
  public string Content { get; set; }

  public SpecialEvent(string content)
  {
    Content = content;
  }
}
