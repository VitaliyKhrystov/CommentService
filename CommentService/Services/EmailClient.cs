namespace CommentService.Services
{
    public class EmailClient
    {
        public static string Address { get; set; }
        public static string Password { get; set; }
        public static string Host { get; set; }
        public static int Port { get; set; }
        public static bool EnableSsl { get; set; }
    }
}
