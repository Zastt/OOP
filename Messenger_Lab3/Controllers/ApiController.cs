using MessengerApp.Data;
using MessengerApp.Models;
using MessengerApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MessengerApp.Controllers;

[ApiController]
[Route("")]
public class ApiController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly MessageService _msgService;
    private readonly ModerationService _modService;

    public ApiController(AppDbContext db, MessageService msgService, ModerationService modService)
    {
        _db = db;
        _msgService = msgService;
        _modService = modService;
    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
    {
        var user = new User { Name = dto.Name };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPost("conversations")]
    public async Task<IActionResult> CreateConversation([FromBody] ConversationCreateDto dto)
    {
        var conv = new Conversation { Type = dto.Type };
        _db.Conversations.Add(conv);
        await _db.SaveChangesAsync();
        return Ok(conv);
    }

    [HttpPost("messages")]
    public async Task<IActionResult> SendMessage([FromBody] MessageCreateDto dto)
    {
        try
        {
            var msg = await _msgService.SendMessageAsync(dto);
            return Ok(msg);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpGet("conversations/{id}/messages")]
    public async Task<IActionResult> GetMessages(int id)
    {
        var messages = await _msgService.GetActiveMessagesAsync(id);
        return Ok(messages);
    }

    [HttpPost("messages/{id}/report")]
    public async Task<IActionResult> ReportMessage(int id, [FromBody] ReportCreateDto dto)
    {
        var report = await _modService.CreateReportAsync(id, dto);
        return Ok(report);
    }

    [HttpPost("moderation/action")]
    public async Task<IActionResult> ModerationAction([FromBody] ModerationActionDto dto)
    {
        var msg = await _modService.ProcessActionAsync(dto);
        if (msg == null) return NotFound("Message not found");
        return Ok(new { msg.Id, msg.VisibilityStatus });
    }
}