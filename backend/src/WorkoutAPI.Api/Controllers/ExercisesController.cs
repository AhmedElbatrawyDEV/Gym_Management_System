using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Domain.Enums;
using WorkoutAPI.Domain.Interfaces;
using Mapster;

namespace WorkoutAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly ILogger<ExercisesController> _logger;

    public ExercisesController(IExerciseRepository exerciseRepository, ILogger<ExercisesController> logger)
    {
        _exerciseRepository = exerciseRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all exercises with translations
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExerciseResponse>>> GetExercises([FromQuery] Language language = Language.Arabic)
    {
        try
        {
            var exercises = await _exerciseRepository.GetExercisesWithTranslationsAsync(language);
            var response = exercises.Adapt<IEnumerable<ExerciseResponse>>();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting exercises");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get exercise by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExerciseResponse>> GetExercise(Guid id, [FromQuery] Language language = Language.Arabic)
    {
        try
        {
            var exercise = await _exerciseRepository.GetWithTranslationsAsync(id);
            if (exercise == null)
            {
                return NotFound($"Exercise with ID {id} not found");
            }

            var response = exercise.Adapt<ExerciseResponse>();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting exercise {ExerciseId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get exercises by type (Push, Pull, Legs)
    /// </summary>
    [HttpGet("by-type/{type}")]
    public async Task<ActionResult<IEnumerable<ExerciseResponse>>> GetExercisesByType(ExerciseType type, [FromQuery] Language language = Language.Arabic)
    {
        try
        {
            var exercises = await _exerciseRepository.GetByTypeAsync(type);
            var response = exercises.Adapt<IEnumerable<ExerciseResponse>>();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting exercises by type {ExerciseType}", type);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get exercises by muscle group
    /// </summary>
    [HttpGet("by-muscle-group/{muscleGroup}")]
    public async Task<ActionResult<IEnumerable<ExerciseResponse>>> GetExercisesByMuscleGroup(MuscleGroup muscleGroup, [FromQuery] Language language = Language.Arabic)
    {
        try
        {
            var exercises = await _exerciseRepository.GetByMuscleGroupAsync(muscleGroup);
            var response = exercises.Adapt<IEnumerable<ExerciseResponse>>();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting exercises by muscle group {MuscleGroup}", muscleGroup);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get exercise by code
    /// </summary>
    [HttpGet("by-code/{code}")]
    public async Task<ActionResult<ExerciseResponse>> GetExerciseByCode(string code, [FromQuery] Language language = Language.Arabic)
    {
        try
        {
            var exercise = await _exerciseRepository.GetByCodeAsync(code);
            if (exercise == null)
            {
                return NotFound($"Exercise with code {code} not found");
            }

            var response = exercise.Adapt<ExerciseResponse>();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting exercise by code {Code}", code);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}

