using System;
using NUnit.Framework;

namespace CryptoUtil.Tests
{

    [TestFixture]
    public class Version1Tests
    {

        private readonly ICryptoHelper _cryptoHelper;

        public Version1Tests()
        {
            var keyStorage = new RamKeyStorage();
            _cryptoHelper = CryptoHelper.CreateV1(keyStorage);
        }

        [TestCase("Test")]
        [TestCase("abcd")]
        [TestCase("11a31edb36865a0b74e1dd783da58e0558a223ee210cf279448cb4830af2b207ad61f878dec4ff8878e92e5f0fa68")]
        public void TestEncryptionAndDecryption(string value)
        {
            var encrypted = _cryptoHelper.EncryptString(
                "TestEncryptionAndDecryption_" + value,
                value);
            var decrypted = _cryptoHelper.DecryptString(
                "TestEncryptionAndDecryption_" + value,
                encrypted);
            Assert.AreEqual(value,
                decrypted,
                "Encrypting and Decrypting the same string must yield the same original string");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("Test")]
        [TestCase("ff")]
        [TestCase("ac6bcc0b9dca1d5d598a9ac9bd67c0205fa8214ac9633fd800ee0d4d3285190e4da612159d146302")]
        public void VerifyInvalidInputs(string value)
        {
            var result = _cryptoHelper.DecryptString("VerifyInvalidInputs", value);

            Assert.IsNull(result, $"{value} must return an null while decrypting");
        }

        [TestCase(null)]
        [TestCase("")]
        public void TestInvalidInputsForEncrypt(string value)
        {
            Assert.Catch(
                typeof(ArgumentException),
                () => _cryptoHelper.EncryptString("TestInvalidInputsForEncrypt", value),
                "Call to EncryptString must yield ArgumentException");
        }

    }

}