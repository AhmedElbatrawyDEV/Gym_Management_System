using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Application.Abstractions;
using WorkoutAPI.Application.DTOs;

namespace WorkoutAPI.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly IExerciseService _service;
    public ExercisesController(IExerciseService service){ _service = service; }

    [HttpGet] public Task<List<ExerciseResponse>> List() => _service.ListAsync();

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExerciseResponse>> Get(Guid id)
    {
        var e = await _service.GetAsync(id);
        return e is null ? NotFound() : Ok(e);
    }

    [Authorize(Roles = "Admin,Trainer")]
    [HttpPost]
    public Task<ExerciseResponse> Create(CreateExerciseRequest req) => _service.CreateAsync(req);

    [Authorize(Roles = "Admin,Trainer")]
    [HttpPut("{id:guid}")]
    public Task<ExerciseResponse> Update(Guid id, CreateExerciseRequest req) => _service.UpdateAsync(id, req);

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id){ await _service.DeleteAsync(id); return NoContent(); }
}