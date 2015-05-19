using System.Collections.Generic;
using System.IO;
using System.Threading;
using SecondLanguage;

namespace IdentityServer3.Contrib.Localization.UI
{
    public class TranslatorHelper
    {
        private static TranslatorHelper translatorHelper;
        private static readonly object lockObject = new object();

        private readonly Dictionary<string, Translator> translators = new Dictionary<string, Translator>();

        private TranslatorHelper()
        {
        }

        public static TranslatorHelper Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (translatorHelper == null)
                    {
                        translatorHelper = new TranslatorHelper();
                        translatorHelper.Init();
                    }
                    return translatorHelper;
                }
            }
        }

        private void Init()
        {
            var files = Directory.GetFiles("Resources", "*.po", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                var translator = new Translator();
                translator.RegisterTranslation(file);

                translators.Add(Path.GetFileNameWithoutExtension(file), translator);
            }
        }

        public Translator GetTranslatorForCurrentCulture()
        {
            var currentUICulture = Thread.CurrentThread.CurrentUICulture;
            Translator translator = null;

            if (translators.ContainsKey(currentUICulture.ToString()))
            {
                translator = translators[currentUICulture.ToString()];
            }
            if (translator == null && translators.ContainsKey(currentUICulture.TwoLetterISOLanguageName))
            {
                translator = translators[currentUICulture.TwoLetterISOLanguageName];
            }
            if (translator == null && translators.ContainsKey("en"))
            {
                translator = translators["en"];
            }

            return translator;
        }
    }
}
