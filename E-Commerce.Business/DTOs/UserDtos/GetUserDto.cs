using E_Commerce.Domain.Enums;

namespace E_Commerce.Business.DTOs.UserDtos
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public UserRole Role { get; set; }
    }
}
