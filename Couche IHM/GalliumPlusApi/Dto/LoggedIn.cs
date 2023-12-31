namespace GalliumPlusApi.Dto
{
    public class LoggedIn
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public UserDetails? User { get; set; }
        public uint Permissions { get; set; }

        public LoggedIn()
        {
            Token = "";
            Expiration = new(0);
            User = new();
            Permissions = 0;
        }
    }
}