using System.Globalization;
using Dropit.Extension.Core;
using EPiServer;
using EPiServer.Core;
using PageTypeBuilder;
using PageTypeBuilder.UI;

namespace ComposerBuilder.UI
{
    public class ContentFunction<T> : BaseContentFunction
            where T : FunctionData
    {
        // Fields
        private T _currentPage;

        // Methods
        protected ContentFunction()
        {
        }

        // Properties
        public new T CurrentPage
        {
            get
            {
                if (_currentPage == null)
                {
                    PageData page = DataFactory.Instance.GetPage(ContentFunctionLink);
                    if (!page.InheritsFromType<T>())
                    {
                        throw new PageTypeBuilderException(string.Format(CultureInfo.InvariantCulture, "The function is not of type {0}.", new object[] { typeof(T).Name }));
                    }
                    _currentPage = (T)page;
                }
                return _currentPage;
            }
        }
    }
}

 
