using System.ComponentModel.DataAnnotations;

namespace WebStore.ServiceContracts.DTO.Auth
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "Password is required!")]
        public string? Token { get; set; }

        public string? NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match!")]
        public string? ConfirmPassword { get; set; }

        public string? Email { get; set; }
    }
}
