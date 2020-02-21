using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace CryptoUtil.Version1
{

    /// <summary>
    ///     This implementation first encrypts the message with a random 128 bit IV and an 256 bit Key using AES.
    ///     The signature is made using HMAC-SHA512 with an extra key.
    ///     Final Message = HMAC-SHA512(IV || AES(Message, EncryptionKey), SignatureKey) || IV || AES(Message, EncryptionKey
    /// </summary>
    internal class CryptoHelperV1 : ICryptoHelper
    {

        internal CryptoHelperV1(IKeyStorage keyStorage)
        {
            KeyStorage = keyStorage ?? throw new ArgumentNullException(nameof(keyStorage));
        }

        public IKeyStorage KeyStorage { get; }

        public byte[] Decrypt(string keyId, string stringToDecrypt)
        {
            var key = LoadKey(keyId);

            try
            {
                var decodedString = Utilities.DecodeHex(stringToDecrypt);

                if (decodedString.Length <= 512 / 8 + 128 / 8)
                {
                    // Signature + IV
                    return null;
                }

                var signature = new byte[512 / 8]; // SHA512
                var signedData = new byte[decodedString.Length - signature.Length];
                Array.Copy(decodedString, signature, signature.Length);
                Array.Copy(decodedString,
                    signature.Length,
                    signedData,
                    0,
                    signedData.Length);

                using (var hmac = new HMACSHA512(key.SignatureKey))
                {
                    var calculatedSignature = hmac.ComputeHash(signedData);

                    if (!Utilities.SafeCompare(calculatedSignature, signature))
                    {
                        return null;
                    }
                }

                var iv = new byte[128 / 8];
                var encryptedData = new byte[signedData.Length - iv.Length];
                Array.Copy(signedData, iv, iv.Length);
                Array.Copy(signedData,
                    iv.Length,
                    encryptedData,
                    0,
                    encryptedData.Length);

                using var aes = Aes.Create();

                aes.Mode = CipherMode.CBC;
                aes.IV = iv;
                aes.Key = key.EncryptionKey;

                using var output = new MemoryStream();

                using (var cryptoStream =
                    new CryptoStream(output, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(encryptedData, 0, encryptedData.Length);
                }

                return output.ToArray();

            }
            catch (Exception)
            {
                return null;
            }
        }

        public string DecryptString(string keyId, string stringToDecrypt)
        {
            var decryptedData = Decrypt(keyId, stringToDecrypt);

            return decryptedData == null ? null : new string(Encoding.UTF8.GetChars(decryptedData));
        }

        public string Encrypt(string keyId, byte[] dataToEncrypt)
        {
            if (dataToEncrypt == null || dataToEncrypt.Length == 0)
            {
                throw new ArgumentException($"{nameof(dataToEncrypt)} must be non null and must have contents.");
            }

            var key = LoadKey(keyId);

            byte[] encryptedData;
            byte[] iv;

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.GenerateIV();
                aes.Key = key.EncryptionKey;

                using var output = new MemoryStream();

                using (var cryptoStream =
                    new CryptoStream(output, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                }

                encryptedData = output.ToArray();
                iv = aes.IV;
            }

            var dataToSign = new byte[iv.Length + encryptedData.Length];
            Array.Copy(iv, dataToSign, iv.Length);
            Array.Copy(encryptedData,
                0,
                dataToSign,
                iv.Length,
                encryptedData.Length);

            byte[] signature;

            using (var hmac = new HMACSHA512(key.SignatureKey))
            {
                signature = hmac.ComputeHash(dataToSign);
            }

            var finalData = new byte[signature.Length + dataToSign.Length];
            Array.Copy(signature, finalData, signature.Length);
            Array.Copy(dataToSign,
                0,
                finalData,
                signature.Length,
                dataToSign.Length);

            return Utilities.EncodeHex(finalData);
        }

        public string EncryptString(string keyId, string stringToEncrypt)
        {
            return Encrypt(keyId, Encoding.UTF8.GetBytes(stringToEncrypt));
        }

        private KeyPairV1 LoadKey(string keyId)
        {
            var keyData = KeyStorage.RetrieveKey(keyId);

            return keyData == null ? GenerateKey(keyId) : JsonConvert.DeserializeObject<KeyPairV1>(keyData);
        }

        private KeyPairV1 GenerateKey(string keyId)
        {
            var key = new KeyPairV1
            {
                EncryptionKey = new byte[32],
                SignatureKey = new byte[64]
            };

            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(key.EncryptionKey);
                random.GetBytes(key.SignatureKey);
            }

            KeyStorage.SaveKey(keyId, JsonConvert.SerializeObject(key));

            return key;
        }

    }

}