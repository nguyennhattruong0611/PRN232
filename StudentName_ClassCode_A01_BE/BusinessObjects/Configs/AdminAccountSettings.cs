namespace BusinessObjects.Configs
{
    public class AdminAccountSettings
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string AccountName { get; set; } = "Administrator"; 
        public string Role { get; set; } = "Admin";
    }
}
