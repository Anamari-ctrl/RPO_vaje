using System.ComponentModel.DataAnnotations;

namespace WebStore.ServiceContracts.DTO.Account
{
    public class RegisterResponse
    {
        [Required(ErrorMessage = "Username can't be black!")]
        [EmailAddress(ErrorMessage = "Email must be in correct format!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password can't be blank!")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword can't be blank!")]
        [Compare(nameof(Password), ErrorMessage = "Password and confirm password do not match!")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "FullName can't be black!")]
        public string? FullName { get; set; }
    }
}
