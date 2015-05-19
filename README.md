# IdentityServer3.Contrib.Localization.UI

Per default all views of [IdentityServer3](https://github.com/IdentityServer/IdentityServer3 "IdentityServer3") are not localized and in English. This solution provides a `LocalizedViewLoader` that can be used to load all views with translations. This implementation is based on the `FileSystemWithEmbeddedFallbackViewLoader` but with translation capabilities.

## Install

Build the solution and include the resulting assembly. Place the directories `assets` and `resources` into the root directory of your application. Be sure to set `Copy to Output Directory` to `Copy if newer`. All files are loaded into an internal cache after startup, so restart after changing views or language files.

    var factory = new IdentityServerServiceFactory();
    factory.ViewService = new Registration<IViewService>(
        resolver => new DefaultViewService(embeddedViewServiceConfig, new LocalizedViewLoader()));

The `LocalizedViewLoader` takes a paramter `directory` to override the default view directory which is `assets`.

## Language files

We use PO files as language files. The name of the file defines the culture:

    de.po -> German
    de-AT -> German, Austria

If the requested language could not be found, the culture `en` will be searched, so be sure that this one exists.

For further information how IdentityServer3 works, see [Customizing Views](https://identityserver.github.io/Documentation/docs/advanced/customizingViews.html "Customizing Views").
