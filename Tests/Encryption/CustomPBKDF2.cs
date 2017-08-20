using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedUtils.Tests.Encryption
{
    [TestClass]
    public class CustomPBKDF2
    {

        [TestMethod]
        public void TwoRandomSaltsDoNotEqualEachOther()
        {
            byte[] salt1 = SharedUtils.Encryption.CustomPBKDF2.PBKDF2GetRandomSalt();
            byte[] salt2 = SharedUtils.Encryption.CustomPBKDF2.PBKDF2GetRandomSalt();
            Assert.AreNotEqual(salt1, salt2);
        }

        [TestMethod]
        public void PBKDF2Hash_DoTwoHashesOfSameTextEqualEachOther()
        {
            string randomText = SharedUtils.Extensions.Utility.GetRandomString(12);
            byte[] salt = SharedUtils.Encryption.CustomPBKDF2.PBKDF2GetRandomSalt();
            string afterHash1 = SharedUtils.Encryption.CustomPBKDF2.PBKDF2HashedPassword(randomText, salt, 0);
            string afterHash2 = SharedUtils.Encryption.CustomPBKDF2.PBKDF2HashedPassword(randomText, salt, 0);
            Assert.AreEqual(afterHash1, afterHash2);
        }

        [TestMethod]
        public void PBKDF2Hash_DoTwoHashesOfSameTextDifferentIterationsNotEqualEachOther()
        {
            string randomText = SharedUtils.Extensions.Utility.GetRandomString(12);
            byte[] salt = SharedUtils.Encryption.CustomPBKDF2.PBKDF2GetRandomSalt();
            string afterHash1 = SharedUtils.Encryption.CustomPBKDF2.PBKDF2HashedPassword(randomText, salt, 1000);
            string afterHash2 = SharedUtils.Encryption.CustomPBKDF2.PBKDF2HashedPassword(randomText, salt, 2000);
            Assert.AreNotEqual(afterHash1, afterHash2);
        }

        [TestMethod]
        public void PBKDF2Hash_DoTwoHashesOfSameTextDifferentSaltsNotEqualEachOther()
        {
            string randomText = SharedUtils.Extensions.Utility.GetRandomString(12);
            byte[] salt1 = SharedUtils.Encryption.CustomPBKDF2.PBKDF2GetRandomSalt();
            byte[] salt2 = SharedUtils.Encryption.CustomPBKDF2.PBKDF2GetRandomSalt();
            string afterHash1 = SharedUtils.Encryption.CustomPBKDF2.PBKDF2HashedPassword(randomText, salt1, 0);
            string afterHash2 = SharedUtils.Encryption.CustomPBKDF2.PBKDF2HashedPassword(randomText, salt2, 0);
            Assert.AreNotEqual(afterHash1, afterHash2);
        }

    }
}
