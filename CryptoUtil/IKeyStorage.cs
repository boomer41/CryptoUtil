namespace CryptoUtil
{

    /// <summary>
    ///     Interface for the accessing application to store generated keys.
    /// </summary>
    public interface IKeyStorage
    {

        /// <summary>
        ///     Retrieves a previously generated key pair from the storage.
        ///     Must return null when the key was not found.
        /// </summary>
        /// <param name="keyId">The key identifier.</param>
        /// <returns>The key data as given by <see cref="SaveKey" />. Null when not found.</returns>
        string RetrieveKey(string keyId);

        /// <summary>
        ///     Persists a key to the storage.
        ///     When the key already exists, this method shall override an existing key.
        /// </summary>
        /// <param name="keyId">The key identifier.</param>
        /// <param name="keyData">The key data to be saved.</param>
        void SaveKey(string keyId, string keyData);

    }

}