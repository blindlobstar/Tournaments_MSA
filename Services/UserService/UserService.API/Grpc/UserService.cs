using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcUserService;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Core.Data;
using static GrpcUserService.UserService;

namespace UserService.API.Grpc
{
    public class UserService : UserServiceBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public override async Task<User> Get(GetRequest request, ServerCallContext context)
        {
            var user = await _userRepository.Get(request.Id);
            return _mapper.Map<User>(user);
        }

        public override async Task<GetAllResponse> GetAll(Empty request, ServerCallContext context)
        {
            var response = new GetAllResponse();

            var users = await _userRepository.GetAll();
            response.Users.AddRange(_mapper.Map<IEnumerable<User>>(users));

            return response;
        }
    }
}
