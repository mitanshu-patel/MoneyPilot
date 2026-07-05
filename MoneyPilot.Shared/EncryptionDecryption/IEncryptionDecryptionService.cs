namespace MoneyPilot.Shared.EncryptionDecryption;

public interface IEncryptionDecryptionService
{
    string EncryptSecret(string value);

    string DecryptSecret(string value);

    bool TryDecryptValue(string value, out string? decryptedValue);
}
