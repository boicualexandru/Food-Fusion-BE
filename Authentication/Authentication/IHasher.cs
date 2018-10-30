namespace Services.Authentication
{
    public interface IHasher
    {
        string GetHash(string plainText);
        bool VerifyHashedText(string hashedText, string plainText);
    }
}
