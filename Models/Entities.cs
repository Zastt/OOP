namespace MessengerApp.Models;

public enum VisibilityStatus { Active, HiddenByModerator }
public enum ReportStatus { Pending, Resolved }

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class Conversation
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty; // "direct" або "group"
}

public class Message
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public int SenderId { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public VisibilityStatus VisibilityStatus { get; set; } = VisibilityStatus.Active;
}

public class Report
{
    public int Id { get; set; }
    public int MessageId { get; set; }
    public int ReporterId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public ReportStatus Status { get; set; } = ReportStatus.Pending;
}