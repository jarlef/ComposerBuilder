namespace ComposerBuilder
{
    public class ExtensionPageTypeAttribute : PageTypeBuilder.PageTypeAttribute
    {
        public ExtensionPageTypeAttribute()
        {
        }

        public ExtensionPageTypeAttribute(string guid) : base(guid)
        {
        }
        
        public new string Name
        {
            get { return base.Name; }
            set { base.Name = "[ExtensionSys] " + value; }
        }

    }
}