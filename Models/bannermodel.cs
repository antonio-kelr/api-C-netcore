namespace banner.Models
{
    public record PersonRequest(string? Titulo, string? Url, int? UserId);

    public class BannerModel
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public int? UserId { get; set; }

        public string? Url { get; set; }
    }
}
