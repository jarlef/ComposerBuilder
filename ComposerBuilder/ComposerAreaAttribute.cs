using Dropit.Extension.SpecializedProperties;

namespace ComposerBuilder
{
    public class ComposerAreaAttribute : ComposerPropertyAttribute
    {
        public ComposerAreaAttribute()
        {
            Type = typeof(ExtensionContentAreaProperty);
            DisplayInEditMode = false;
            Searchable = true;
            UniqueValuePerLanguage = false;
        }
    }
}
