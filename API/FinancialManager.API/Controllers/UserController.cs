using System.Security.Claims;
using FinancialManager.Application.DTOs;
using FinancialManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserProfileService _userProfileService;
    private readonly ILogger<UserController> _logger;

    public UserController(UserProfileService userProfileService, ILogger<UserController> logger)
    {
        _userProfileService = userProfileService;
        _logger = logger;
    }

    /// <summary>
    /// Obtém o perfil do usuário autenticado
    /// </summary>
    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileResponse>> GetProfile()
    {
        var userId = GetCurrentUserId();
        _logger.LogInformation("Obtendo perfil do usuário {UserId}", userId);

        var profile = await _userProfileService.GetProfileAsync(userId);
        return Ok(profile);
    }

    /// <summary>
    /// Atualiza o perfil do usuário autenticado
    /// </summary>
    [HttpPut("profile")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserProfileResponse>> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = GetCurrentUserId();
        _logger.LogInformation("Atualizando perfil do usuário {UserId}", userId);

        var profile = await _userProfileService.UpdateProfileAsync(userId, request);
        
        _logger.LogInformation("Perfil do usuário {UserId} atualizado com sucesso", userId);
        return Ok(profile);
    }

    /// <summary>
    /// Altera a senha do usuário autenticado
    /// </summary>
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = GetCurrentUserId();
        _logger.LogInformation("Alterando senha do usuário {UserId}", userId);

        await _userProfileService.ChangePasswordAsync(userId, request);
        
        _logger.LogInformation("Senha do usuário {UserId} alterada com sucesso", userId);
        return NoContent();
    }

    /// <summary>
    /// Desativa a conta do usuário autenticado
    /// </summary>
    [HttpDelete("deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateAccount()
    {
        var userId = GetCurrentUserId();
        _logger.LogWarning("Desativando conta do usuário {UserId}", userId);

        await _userProfileService.DeactivateAccountAsync(userId);
        
        _logger.LogWarning("Conta do usuário {UserId} desativada", userId);
        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("userId")?.Value 
            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Usuário não autenticado");
        }

        return userId;
    }
}
