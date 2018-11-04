namespace Services.Authentication.Models
{
    public class JwtSettings
    {
        public string SymetricSecurityKey { get; set; }
        public int DaysBeforeExpiration { get; set; }
    }

}
