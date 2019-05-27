using CryptoUtil.Version1;

namespace CryptoUtil
{

    /// <summary>
    ///     Factory helper for the CryptoHelpers.
    /// </summary>
    public static class CryptoHelper
    {

        /// <summary>
        ///     Creates a version 1 crypto string.
        /// </summary>
        /// <param name="keyStorage">The key storage to be used.</param>
        /// <returns></returns>
        public static ICryptoHelper CreateV1(IKeyStorage keyStorage)
        {
            return new CryptoHelperV1(keyStorage);
        }

    }

}