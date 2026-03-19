namespace BibliotecaApi.Application.Abstractions
{
    public interface IPasswordHasher
    {
        public string Hash(string password);
        bool Verificar(string passwordRequest, string passwordHash);
    }
}
