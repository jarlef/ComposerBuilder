using EPiServer.Security;
using PageTypeBuilder;

namespace ComposerBuilder
{
    public class ComposerTab : Tab
    {
        #region Overrides of Tab

        public override string Name
        {
            get { return "Extension"; }
        }

        public override AccessLevel RequiredAccess
        {
            get { return AccessLevel.Edit; }
        }

        public override int SortIndex
        {
            get { return 1000; }
        }

        #endregion
    } 
}