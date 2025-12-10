namespace WebStore.ServiceContracts.DTO.Account
{
    public class TokenModel
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
