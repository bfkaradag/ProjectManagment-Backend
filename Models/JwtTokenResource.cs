namespace DSPro.Models;

public class JwtTokenResource
{
    public string Token { get; set; }
    public long Expiry { get; set; }
}