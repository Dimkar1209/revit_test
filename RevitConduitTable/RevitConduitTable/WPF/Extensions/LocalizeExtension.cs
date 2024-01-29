using Prism.Ioc;

using RevitConduitTable.WPF.Services;

using System;
using System.Windows.Markup;

namespace RevitConduitTable.WPF.Extensions
{
    public class LocalizeExtension : MarkupExtension
    {
        public string ResourceKey { get; set; }

        public LocalizeExtension(string resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var container = Prism.Ioc.ContainerLocator.Container;
            var localizationService = container.Resolve<ILocalizationService>();

            return localizationService?.GetString(ResourceKey) ?? $"[{ResourceKey}]";
        }
    }

}
