namespace CommentService.Services
{
    public class JWTconfig
    {
        public static string ValidIssuer { get; set; }
        public static string ValidAudience { get; set; }
        public static string Key { get; set; }
        public static int TokenExpirationSeconds { get; set; }
        public static int RefreshTokenExpirationDays { get; set; }

    }
}
