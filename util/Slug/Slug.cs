namespace Projeto.Utilities
{
    public static class SlugGenerator
    {
        public static string GenerateSlug(string nome)
        {
            return nome.ToLower().Replace(" ", "-");
        }
    }
}
