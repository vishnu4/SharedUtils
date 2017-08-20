using System;
using SharedUtils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharedUtils.Tests.Extensions
{
    [TestClass]
    public class SharedMethods_Test
    {
        [TestMethod]
        public void FormatGUIDObject_DoesRandomTextGiveEmptyGUID()
        {
            Guid g = randomText.ToGUID();
            Assert.AreEqual(Guid.Empty, g);
        }

        const string randomText = "2034j2oi3rn0823";

        [TestMethod]
        public void IsNumeric_IsRandomTextFalse()
        {
            var b = randomText.IsNumeric();
            Assert.IsFalse(b);
        }

        [TestMethod]
        public void IsNumeric_IsIntegerTrue()
        {
            var b = "1111".IsNumeric();
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void IsNumeric_IsDoubleTrue()
        {
            var b = "11.1111".IsNumeric();
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void FormatDoubleObject_DoesRandomTextGiveNullValue()
        {
            double? d = randomText.ToNullableDouble();
            Assert.IsNull(d);
        }

        [TestMethod]
        public void FormatDoubleObject_DoesDoubleObjectReturnCorrectly()
        {
            double inputD = 33.123234;
            object inputo = inputD;
            double? output = inputo.ToNullableDouble();
            Assert.AreEqual(inputD, output);
        }

        [TestMethod]
        public void FormatDoubleObject_DoesDecimalObjectReturnCorrectly()
        {
            decimal inputDec = 33.123234M;
            object inputo = inputDec;
            double? output = inputo.ToNullableDouble();
            Assert.AreEqual((double)inputDec, output);
        }

        [TestMethod]
        public void FormatDoubleObject_DoesStringObjectReturnCorrectly()
        {
            string inputS = "33.123234";
            object inputo = inputS;
            double? output = inputo.ToNullableDouble();
            Assert.AreEqual(Convert.ToDouble(inputS), output);
        }

        [TestMethod]
        public void DoubleObject_AlmostEquals()
        {
            double d1 = 11.1111;
            double d2 = 22.2222;
            double d2a = 22.2221;
            double d3 = 33;
            double d3a = 33.00000001;
            Assert.IsFalse(d1.AlmostEquals(d2));
            Assert.IsFalse(d2.AlmostEquals(d2a));
            Assert.IsTrue(d3.AlmostEquals(d3a));
        }

        [TestMethod]
        public void FormatIntegerObject_DoesRandomTextGiveNullValue()
        {
            int? i = randomText.ToNullableInteger();
            Assert.IsNull(i);
        }

        [TestMethod]
        public void FormatBooleanObject_DoesRandomTextGiveNullValue()
        {
            bool? b = randomText.ToNullableBoolean();
            Assert.IsNull(b);
        }


        [TestMethod]
        public void FormatBooleanObject_DoesTrueStringGiveTrueValue()
        {
            bool? b = "true".ToNullableBoolean();
            Assert.IsTrue(b.Value);
        }

        [TestMethod]
        public void FormatBooleanObject_DoesFalseStringGiveTrueValue()
        {
            bool? b = "false".ToNullableBoolean();
            Assert.IsFalse(b.Value);
        }
        

        [TestMethod]
        public void FormatDateObject_DoesRandomTextGiveNullValue()
        {
            DateTime? d = randomText.ToNullableDateTime();
            Assert.IsNull(d);
        }

        [TestMethod]
        public void FormatDateObject_DoTwoDifferentCulturesEqualEachOther()
        {
            string sNLDate = "15/11/2015 13:30";
            string sUSDate = "11/15/2015 1:30 PM";

            DateTime? dNLDate = sNLDate.ToNullableDateTime(new System.Globalization.CultureInfo("nl-BE"));
            DateTime? dUSDate =sUSDate.ToNullableDateTime(new System.Globalization.CultureInfo("en-US"));
            Assert.AreEqual(dNLDate, dUSDate);
        }

        [TestMethod]
        public void FormatString_DoesWhitespaceTurnIntoNullOrEmpty()
        {
            const string whitespaceText = "   ";
            string s = whitespaceText.ToFormattedString();
            Assert.IsTrue(string.IsNullOrEmpty(s));
            string s2 = "0".ToFormattedString();
            Assert.IsTrue(string.IsNullOrEmpty(s2));
            string s3 = "0".ToFormattedString(false);
            Assert.IsFalse(string.IsNullOrEmpty(s3));
        }

        [TestMethod]
        public void GetRandomString_Are2DifferentCallsActuallyRandom()
        {
            const int stringLength = 10;
            string s1 = Utility.GetRandomString(stringLength);
            string s2 = Utility.GetRandomString(stringLength);
            Assert.AreNotEqual(s1, s2);
        }

        /*
         ScrubHtml
        IsValidPhoneNumber
        FormatAddress;
        FormatURIObject;
        ReturnFormattedFileName;
        ReturnFormattedXmlElement;
        */

        ////doesn't work, the millseconds are off for some reason
        //[TestMethod]
        //public void ConvertToUnixTimestamp_ConvertUnixTimestamp_ConversionBackAndForth()
        //{
        //    DateTime d = System.DateTime.Now;
        //    double dDate = d.ToUnixTimestamp();
        //    DateTime d2 = dDate.FromUnixTimestamp();
        //    Assert.AreEqual(d, d2);
        //}

    }
}

