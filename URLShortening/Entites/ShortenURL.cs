namespace URLShortening.Entites
{
    public class ShortenURL
    {

        public Guid Id { get; set; }
        public string LongURL { get; set; } = string.Empty;
        public string ShortURL { get; set; } = string.Empty;
        public string Code  { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }
    }
}
