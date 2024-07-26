using Microsoft.EntityFrameworkCore;

namespace URLShortening.Services
{
    public class URLShorteningServices
    {
        public const int NumberOfcharshort = 7;
        private const string Alphabet =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private readonly Random _random = new() ;
        private readonly ApplicationDBContext _dbContext;
        public URLShorteningServices(ApplicationDBContext dbContext)
        {
            _dbContext=dbContext;
        }

        public async Task<string> GenerateUniqueCode()
        {
            var codechars = new char[NumberOfcharshort];
            while (true)
            {
                for (var i = 0; i < NumberOfcharshort; i++)
                {
                    int randomIndex = _random.Next(Alphabet.Length - 1);
                    codechars[i] = Alphabet[randomIndex];
                }
                var code = new string(codechars);

                if (!await _dbContext.ShortenURL.AnyAsync(s => s.Code == code))
                {
                    return code;
                }
            }
            
        }
    }
}
