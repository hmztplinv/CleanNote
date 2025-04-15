using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApp.Application.Features.Notes.Commands;
using NoteApp.Application.Features.Notes.Queries;
using NoteApp.Application.Wrappers;

namespace NoteApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Yetkilendirme eklendi
public class NotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<object>>>> Get()
    {
        var response = await _mediator.Send(new GetAllNotesQuery());
        return Ok(response);
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResponse<List<object>>>> GetPaged([FromQuery] GetPagedNotesQuery query)
    {
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> GetById(Guid id)
    {
        var query = new GetNoteByIdQuery { Id = id };
        var response = await _mediator.Send(query);
        
        if (!response.Success)
            return NotFound(response);
            
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult<ApiResponse<Guid>>> Create([FromBody] CreateNoteCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Guid>>> Update(Guid id, [FromBody] UpdateNoteCommand command)
    {
        command.Id = id;
        var response = await _mediator.Send(command);
        
        if (!response.Success)
            return NotFound(response);
            
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
    {
        var command = new DeleteNoteCommand { Id = id };
        var response = await _mediator.Send(command);
        
        if (!response.Success)
            return NotFound(response);
            
        return Ok(response);
    }

    [HttpPatch("{id}/toggle-publish")]
    public async Task<ActionResult<ApiResponse<bool>>> TogglePublishState(Guid id)
    {
        var command = new ToggleNotePublishStateCommand { Id = id };
        var response = await _mediator.Send(command);
        
        if (!response.Success)
            return NotFound(response);
            
        return Ok(response);
    }
}