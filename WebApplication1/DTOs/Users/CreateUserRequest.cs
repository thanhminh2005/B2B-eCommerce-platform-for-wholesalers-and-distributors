namespace API.DTOs.Users
{
    public class CreateUserRequest
    {
        public string RoleId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string DoB { get; set; }
        public string Avatar { get; set; }
        public int Sex { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
