using Locoom.Application.Common.Interfaces.Persistence;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Locoom.API.Controllers
{
    [Route("users")]
    public class UsersController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UsersController(IMapper mapper, ISender mediator, IUserRepository userRepository)
        {
            _mapper = mapper;
            _mediator = mediator;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }
    }
}

