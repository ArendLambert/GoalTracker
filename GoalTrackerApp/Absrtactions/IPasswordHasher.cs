namespace Abstractions
{
    public interface IPasswordHasher
    {
        static abstract string Generate(string password);
        static abstract bool Verify(string password, string hashedPassword);
    }
}