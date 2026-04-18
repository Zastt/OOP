namespace MessengerApp.Models;

public record UserCreateDto(string Name);
public record ConversationCreateDto(string Type);
public record MessageCreateDto(int ConversationId, int SenderId, string Text);
public record ReportCreateDto(int ReporterId, string Reason);
public record ModerationActionDto(int MessageId, VisibilityStatus Action);