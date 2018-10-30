namespace Services.Authentication
{
    public class JwtSettings
    {
        public string SymetricSecurityKey { get; set; }
        public int DaysBeforeExpiration { get; set; }
    }

}
