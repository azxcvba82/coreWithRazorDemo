namespace WebApplicationDemo.Model
{
    public class JwtBlacklistService
    {
        private static readonly HashSet<string> _blacklistedTokens = new HashSet<string>();

        public void BlacklistToken(string token)
        {
            _blacklistedTokens.Add(token);
        }

        public bool IsTokenBlacklisted(string token)
        {
            return _blacklistedTokens.Contains(token);
        }
    }
}
