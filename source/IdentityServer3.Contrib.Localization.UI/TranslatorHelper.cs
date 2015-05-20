using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
            var resPath = Path.Combine(AssemblyDirectory, "resources");

            var files = Directory.GetFiles(resPath, "*.po", SearchOption.AllDirectories);
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

        private string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public string Translate(string html)
        {
            var translator = GetTranslatorForCurrentCulture();
            const string pattern = @"\[\[([a-zA-Z0-9\.]+)\]\]";
            var matches =
                from Match match in Regex.Matches(html, pattern)
                select match.Groups[1].Value;

            foreach (var match in matches)
            {
                var translated = translator.Translate(match);
                html = html.Replace("[[" + match + "]]", translated);
            }
            return html;
        }
    }
}
