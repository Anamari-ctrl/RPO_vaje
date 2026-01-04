using System.ComponentModel.DataAnnotations;

namespace WebStore.ServiceContracts.DTO.AuthDTO
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? ClientUri { get; set; }
    }
}
