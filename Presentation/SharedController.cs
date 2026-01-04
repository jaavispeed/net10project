using Microsoft.AspNetCore.Mvc;

namespace PulseTrain.Presentation;

[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[Route("api/[controller]")]
[ApiController]
public class SharedController : ControllerBase { }
