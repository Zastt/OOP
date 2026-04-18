using MessengerApp.Data;
using MessengerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Services;

public class MessageService
{
    private readonly AppDbContext _db;

    public MessageService(AppDbContext db) => _db = db;

    public async Task<Message> SendMessageAsync(MessageCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Text)) throw new ArgumentException("Text is empty");
        
        var msg = new Message
        {
            ConversationId = dto.ConversationId,
            SenderId = dto.SenderId,
            Text = dto.Text
        };
        
        _db.Messages.Add(msg);
        await _db.SaveChangesAsync();
        return msg;
    }

    public async Task<List<Message>> GetActiveMessagesAsync(int conversationId)
    {
        return await _db.Messages
            .Where(m => m.ConversationId == conversationId && m.VisibilityStatus == VisibilityStatus.Active)
            .ToListAsync();
    }
}

public class ModerationService
{
    private readonly AppDbContext _db;

    public ModerationService(AppDbContext db) => _db = db;

    public async Task<Report> CreateReportAsync(int messageId, ReportCreateDto dto)
    {
        var report = new Report
        {
            MessageId = messageId,
            ReporterId = dto.ReporterId,
            Reason = dto.Reason
        };
        _db.Reports.Add(report);
        await _db.SaveChangesAsync();
        return report;
    }

    public async Task<Message?> ProcessActionAsync(ModerationActionDto dto)
    {
        var msg = await _db.Messages.FindAsync(dto.MessageId);
        if (msg == null) return null;

        msg.VisibilityStatus = dto.Action;

        var reports = await _db.Reports.Where(r => r.MessageId == msg.Id).ToListAsync();
        foreach (var r in reports)
        {
            r.Status = ReportStatus.Resolved;
        }

        await _db.SaveChangesAsync();
        return msg;
    }
}