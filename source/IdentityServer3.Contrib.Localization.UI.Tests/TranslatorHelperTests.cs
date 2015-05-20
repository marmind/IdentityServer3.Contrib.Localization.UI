using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdentityServer3.Contrib.Localization.UI.Tests
{
    [TestClass]
    public class TranslatorHelperTests
    {
        

        [TestInitialize]
        public void Initialize()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
        }

        [TestMethod]
        public void GetTranslatorForCurrentCultuer_ShouldBeOk()
        {
            var translator = TranslatorHelper.Instance.GetTranslatorForCurrentCulture();
            Assert.IsNotNull(translator);
        }

        [TestMethod]
        public void TranslateSample_ShouldBeOk()
        {
            const string toTranslate = "[[common.error]]: [[login.username]] and [[login.password]] not set!";
            const string expected = "Error: Username and Password not set!";

            var translated = TranslatorHelper.Instance.Translate(toTranslate);

            Assert.AreEqual<string>(expected, translated);
        }

        [TestMethod]
        public void TranslateStringWithNoGivenId_ShouldReturnId()
        {
            const string toTranslate = "[[this.is.no.existing.translation.id]]";
            const string expected = "this.is.no.existing.translation.id";

            var translated = TranslatorHelper.Instance.Translate(toTranslate);

            Assert.AreEqual<string>(expected, translated);
        }

        [TestMethod]
        public void TranslateMoreComplicated_ShouldBeOk()
        {
            const string toTranslate = "[[logout.click]]<a ng-href=\"{{model.redirectUrl}}\">[[logout.here]]</a>[[logout.return]]";
            const string expected = "Click <a ng-href=\"{{model.redirectUrl}}\">here</a> to return to the";

            var translated = TranslatorHelper.Instance.Translate(toTranslate);

            Assert.AreEqual<string>(expected, translated);
        }
    }
}
