using AutoMapper;
using CommentService.Domain.Enteties;
using CommentService.Models;

namespace CommentService
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRequestDTO, User>();
            CreateMap<User, UserResponseDTO>();
        }
    }
}
