namespace API.DTOs.Users
{
    public class UpdateUserProfileRequest
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string DoB { get; set; }
        public string Avatar { get; set; }
        public int Sex { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string BusinessLicense { get; set; }
        public string TaxId { get; set; }
        public bool IsActive { get; set; }
    }
}
