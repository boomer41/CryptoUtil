namespace CryptoUtil
{

    /// <summary>
    ///     Implements a generic CryptoHelper.
    /// </summary>
    public interface ICryptoHelper
    {

        /// <summary>
        ///     The key storage to be used.
        /// </summary>
        IKeyStorage KeyStorage { get; }

        /// <summary>
        ///     Decrypts raw binary data previously encrypted by <see cref="Encrypt" />.
        ///     Returns null when the given string is invalid.
        ///     Automatically generates and persists the key as necessary.
        /// </summary>
        /// <param name="keyId">The ID of the key to be used.</param>
        /// <param name="stringToDecrypt">The string to be decrypted.</param>
        /// <returns></returns>
        byte[] Decrypt(string keyId, string stringToDecrypt);

        /// <summary>
        ///     Decrypts a string previously encrypted by <see cref="EncryptString" />.
        ///     Returns null when the given string is invalid.
        ///     Automatically generates and persists the key as necessary.
        /// </summary>
        /// <param name="keyId">The ID of the key to be used.</param>
        /// <param name="stringToDecrypt">The string to be decrypted.</param>
        /// <returns></returns>
        string DecryptString(string keyId, string stringToDecrypt);

        /// <summary>
        ///     Encrypts binary data using the key <paramref name="keyId" />.
        ///     Automatically generates and persists the key as necessary.
        /// </summary>
        /// <param name="keyId">The ID of the key to be used.</param>
        /// <param name="dataToEncrypt">The data to be encrypted.</param>
        /// <returns>The encrypted data.</returns>
        string Encrypt(string keyId, byte[] dataToEncrypt);

        /// <summary>
        ///     Encrypts a string using the key <paramref name="keyId" />.
        ///     Automatically generates and persists the key as necessary.
        /// </summary>
        /// <param name="keyId">The ID of the key to be used.</param>
        /// <param name="stringToEncrypt">The string to be encrypted.</param>
        /// <returns>The encrypted data.</returns>
        string EncryptString(string keyId, string stringToEncrypt);

    }

}