using Microsoft.AspNetCore.Mvc;
using RatisbonaBackend.Business.Services;
using RatisbonaBackend.Presentation.Contracts.Users;

namespace RatisbonaBackend.Presentation.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(UsersService usersService) : ControllerBase
{
    /// <summary>Listet alle Benutzer.</summary>
    /// <response code="200">Liste der Benutzer.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserDto>>> List(CancellationToken cancellationToken)
    {
        var users = await usersService.ListAsync(cancellationToken);
        return Ok(users);
    }

    /// <summary>Liefert einen Benutzer per ID.</summary>
    /// <param name="id">Benutzer-ID.</param>
    /// <param name="cancellationToken">Abbruch-Token.</param>
    /// <response code="200">Benutzer gefunden.</response>
    /// <response code="404">Benutzer nicht gefunden.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await usersService.GetByIdAsync(id, cancellationToken);
        if (result.StatusCode == StatusCodes.Status404NotFound)
        {
            return NotFound(new { message = result.Error });
        }

        return Ok(result.Value);
    }

    /// <summary>Erstellt einen neuen Benutzer.</summary>
    /// <param name="request">Benutzerdaten.</param>
    /// <param name="cancellationToken">Abbruch-Token.</param>
    /// <response code="201">Benutzer erstellt.</response>
    /// <response code="400">Ungültige Anfrage.</response>
    /// <response code="409">E-Mail bereits vergeben.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserDto>> Create([FromBody] UserCreateRequest request, CancellationToken cancellationToken)
    {
        var result = await usersService.CreateAsync(request, cancellationToken);
        if (result.StatusCode == StatusCodes.Status409Conflict)
        {
            return Conflict(new { message = result.Error });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value?.Id }, result.Value);
    }

    /// <summary>Aktualisiert einen Benutzer.</summary>
    /// <param name="id">Benutzer-ID.</param>
    /// <param name="request">Benutzerdaten.</param>
    /// <param name="cancellationToken">Abbruch-Token.</param>
    /// <response code="204">Benutzer aktualisiert.</response>
    /// <response code="400">Ungültige Anfrage.</response>
    /// <response code="404">Benutzer nicht gefunden.</response>
    /// <response code="409">E-Mail bereits vergeben.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(Guid id, [FromBody] UserUpdateRequest request, CancellationToken cancellationToken)
    {
        var result = await usersService.UpdateAsync(id, request, cancellationToken);
        if (result.StatusCode == StatusCodes.Status404NotFound)
        {
            return NotFound(new { message = result.Error });
        }

        if (result.StatusCode == StatusCodes.Status409Conflict)
        {
            return Conflict(new { message = result.Error });
        }

        return NoContent();
    }

    /// <summary>Löscht einen Benutzer.</summary>
    /// <param name="id">Benutzer-ID.</param>
    /// <param name="cancellationToken">Abbruch-Token.</param>
    /// <response code="204">Benutzer gelöscht.</response>
    /// <response code="404">Benutzer nicht gefunden.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await usersService.DeleteAsync(id, cancellationToken);
        if (result.StatusCode == StatusCodes.Status404NotFound)
        {
            return NotFound(new { message = result.Error });
        }

        return NoContent();
    }
}
