using System.ComponentModel.DataAnnotations;

namespace WebStore.ServiceContracts.DTO.Account
{
    public class LoginResponse
    {
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password can't be blank!")]
        public string Password { get; set; } = string.Empty;
    }
}
