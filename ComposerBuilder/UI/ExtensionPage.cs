using System;
using System.Web.UI.HtmlControls;
using Dropit.Extension.Core;

namespace ComposerBuilder.UI
{
    public class ExtensionPage<T> : PageTypeBuilder.UI.TemplatePage<T>
        where T : PageTypeBuilder.TypedPageData
    {
        public ExtensionPage()
            : base(0, 0)
        {
            CurrentExtensionPageHandler = new ExtensionPageHandler();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CurrentExtensionPageHandler.Initialize(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // Add meta tag to set default content compatible mode in IE 8
            var httpEquiv = new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=EmulateIE7" };
            Header.Controls.Add(httpEquiv);
        }

        protected ExtensionPageHandler CurrentExtensionPageHandler
        {
            get;
            private set;
        }

    }
}