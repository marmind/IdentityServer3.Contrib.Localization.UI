using System;
using System.IO;
using IdentityServer3.Core.Services.Default;

namespace IdentityServer3.Contrib.Localization.UI
{
    public class LocalizedViewLoader : IViewLoader
    {
        readonly FileSystemViewLoader file;
        readonly EmbeddedAssetsViewLoader embedded;

        public LocalizedViewLoader()
            : this(GetDefaultDirectory())
        {
        }

        public LocalizedViewLoader(string directory)
        {
            file = new FileSystemViewLoader(directory);
            embedded = new EmbeddedAssetsViewLoader();
        }

        static string GetDefaultDirectory()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.Combine(path, "assets");
            return path;
        }

        public async System.Threading.Tasks.Task<string> LoadAsync(string name)
        {            
            string value = null;
            value = await file.LoadAsync(name);
            if (value == null)
            {
                value = await embedded.LoadAsync(name);
            }
            return TranslatorHelper.Instance.Translate(value);
        }
    }
}
