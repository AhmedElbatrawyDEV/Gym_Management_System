using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutAPI.Application.DTOs;
using WorkoutAPI.Application.Services;

namespace WorkoutAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TrainersController : ControllerBase {
    private readonly ITrainerService _trainerService;
    private readonly ILogger<TrainersController> _logger;

    public TrainersController(ITrainerService trainerService, ILogger<TrainersController> logger) {
        _trainerService = trainerService;
        _logger = logger;
    }

    /// <summary>
    /// Get all available trainers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainerResponse>>> GetTrainers() {
        try
        {
            var trainers = await _trainerService.GetAvailableTrainersAsync();
            return Ok(trainers);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting trainers");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get trainer by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TrainerResponse>> GetTrainer(Guid id) {
        try
        {
            var trainer = await _trainerService.GetTrainerByIdAsync(id);
            if (trainer == null)
            {
                return NotFound($"Trainer with ID {id} not found");
            }

            return Ok(trainer);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting trainer {TrainerId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get trainer by user ID
    /// </summary>
    [HttpGet("by-user/{userId:guid}")]
    public async Task<ActionResult<TrainerResponse>> GetTrainerByUserId(Guid userId) {
        try
        {
            var trainer = await _trainerService.GetTrainerByUserIdAsync(userId);
            if (trainer == null)
            {
                return NotFound($"Trainer for user ID {userId} not found");
            }

            return Ok(trainer);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting trainer by user ID {UserId}", userId);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Get trainers by specialization
    /// </summary>
    [HttpGet("by-specialization/{specialization}")]
    public async Task<ActionResult<IEnumerable<TrainerResponse>>> GetTrainersBySpecialization(string specialization) {
        try
        {
            var trainers = await _trainerService.GetTrainersBySpecializationAsync(specialization);
            return Ok(trainers);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting trainers by specialization {Specialization}", specialization);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Create a new trainer
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TrainerResponse>> CreateTrainer([FromBody] CreateTrainerRequest request) {
        try
        {
            var trainer = await _trainerService.CreateTrainerAsync(request);
            return CreatedAtAction(nameof(GetTrainer), new { id = trainer.Id }, trainer);
        } catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument while creating trainer");
            return BadRequest(ex.Message);
        } catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation while creating trainer");
            return Conflict(ex.Message);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating trainer");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Update an existing trainer
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TrainerResponse>> UpdateTrainer(Guid id, [FromBody] UpdateTrainerRequest request) {
        try
        {
            var trainer = await _trainerService.UpdateTrainerAsync(id, request);
            return Ok(trainer);
        } catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Trainer not found for update: {TrainerId}", id);
            return NotFound(ex.Message);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating trainer {TrainerId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    /// <summary>
    /// Delete a trainer (soft delete)
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteTrainer(Guid id) {
        try
        {
            var result = await _trainerService.DeleteTrainerAsync(id);
            if (!result)
            {
                return NotFound($"Trainer with ID {id} not found");
            }

            return NoContent();
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting trainer {TrainerId}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }
}

