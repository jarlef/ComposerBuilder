using System;

namespace ComposerBuilder
{
    public class FunctionTypeAttribute : PageTypeBuilder.PageTypeAttribute
    {
        public FunctionTypeAttribute()
        {
            Initialize();
        }
        
        public FunctionTypeAttribute(string guid) : base(guid)
        {
            Initialize();
        }

        private void Initialize()
        {
            SortOrder = 50000;
            AvailableOnPages = AvailableOn.Specific;
            AvailableOnContent = AvailableOn.Specific;
            AvailableInEditMode = false;
        }

        public new string Name
        {
            get { return base.Name; } 
            set { base.Name = "[ExtensionSys] " + value; }
        }

        public AvailableOn AvailableOnPages { get; set; }
        public Type[] AvailableOnPageTypes { get; set; }
        public AvailableOn AvailableOnContent { get; set; }
        public Type[] AvailableOnContentTypes { get; set; }
    }
}