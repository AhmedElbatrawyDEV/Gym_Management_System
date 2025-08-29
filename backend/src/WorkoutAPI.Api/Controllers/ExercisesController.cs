using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Api.Common;
using WorkoutAPI.Api.Models;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Application.Queries.GetExercises;
using WorkoutAPI.Domain.Enums;

namespace WorkoutAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : BaseController
{
    /// <summary>
    /// Get exercises with pagination and optional filtering
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <param name="type">Exercise type filter</param>
    /// <param name="muscleGroup">Muscle group filter</param>
    /// <param name="difficulty">Difficulty level filter</param>
    /// <param name="activeOnly">Filter for active exercises only</param>
    /// <param name="language">Language for exercise translations</param>
    /// <returns>Paginated list of exercises</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<ExerciseDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetExercises(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] ExerciseType? type = null,
        [FromQuery] MuscleGroup? muscleGroup = null,
        [FromQuery] DifficultyLevel? difficulty = null,
        [FromQuery] bool? activeOnly = true,
        [FromQuery] Language language = Language.English)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = new GetExercisesQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            Type = type,
            MuscleGroup = muscleGroup,
            Difficulty = difficulty,
            ActiveOnly = activeOnly,
            Language = language
        };

        var result = await Mediator.Send(query);
        return HandlePaginatedResult(result);
    }
}
