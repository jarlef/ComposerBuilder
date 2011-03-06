using Dropit.Extension.SpecializedProperties;
using PageTypeBuilder;

namespace ComposerBuilder
{
    public class FunctionData : TypedPageData
    {
        [ComposerProperty(Type = typeof(ExtensionFunctionProperty), DisplayInEditMode = false, Searchable = false, UniqueValuePerLanguage = true)]
        public virtual string ExtensionContentFunctionProperty { get; set; }

        [ComposerProperty(UniqueValuePerLanguage = false, Searchable = false, DisplayInEditMode = false)]
        public virtual bool NeverUsedProperty { get; set; }  
    }
}