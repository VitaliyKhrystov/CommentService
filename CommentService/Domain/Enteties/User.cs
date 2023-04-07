namespace CommentService.Domain.Enteties
{
    public class User
    {
        public string Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int BirthYear { get; set; }
        public string RoleId { get; set; }
        public Role Role { get; set; }
    }
}
