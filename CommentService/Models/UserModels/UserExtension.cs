using CommentService.Domain.Enteties;


namespace CommentService.Models.UserModels
{
    public static class UserExtension
    {
        public static UserResponseModel ToDTO(this User user)
        {
            return new UserResponseModel()
            {
                Id = user.Id,
                NickName = user.NickName,
                Email = user.Email,
                BirthYear = user.BirthYear,
                RoleName = ""
            };
        }
    }
}
