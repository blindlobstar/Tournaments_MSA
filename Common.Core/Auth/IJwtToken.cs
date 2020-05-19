namespace Common.Core.Auth
{
    public interface IJwtToken
    {
        string AccessToken { get; set; }
        string RefreshToken { get; set; }
        string Id { get; set; }
    }
}