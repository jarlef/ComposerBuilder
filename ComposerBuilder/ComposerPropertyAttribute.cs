namespace ComposerBuilder
{

    public class ComposerPropertyAttribute : PageTypeBuilder.PageTypePropertyAttribute
    {
        public ComposerPropertyAttribute()
        {
            Tab = typeof (ComposerTab);
        }
    }
}