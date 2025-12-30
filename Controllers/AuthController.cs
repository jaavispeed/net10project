using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PulseTrain.Models.Dtos;
using PulseTrain.Repository.IRepository;

namespace PulseTrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AuthController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetUser(int id)
        {
            var user = _userRepository.GetUser(id);
            if(user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UserRegisterDto>(user);
            return Ok(userDto);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
           if(createUserDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(string.IsNullOrWhiteSpace(createUserDto.Email))
            {
                return BadRequest("El email es requerido");
            }
            if(string.IsNullOrWhiteSpace(createUserDto.Password))
            {
                return BadRequest("La contraseña es requerida");
            }
            if(!_userRepository.IsUniqueUser(createUserDto.Email))
            {
                return BadRequest("El usuario ya existe");
            }
            var result = await _userRepository.Register(createUserDto);
            if(result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al registrar el usuario");
            }
            return CreatedAtRoute("GetUser", new { id = result.Id }, result);
        }

    [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
        if(userLoginDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

        var user = await _userRepository.Login(userLoginDto);
        if(user == null)
            {
                return Unauthorized();
            }
        return Ok(user);
        }

    }
}
