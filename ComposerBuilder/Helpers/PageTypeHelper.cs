using System;
using System.Collections.Generic;
using System.Linq;
using Dropit.Extension.Core;
using EPiServer.DataAbstraction;
using PageTypeBuilder;
using PageTypeBuilder.Abstractions;

namespace ComposerBuilder.Helpers
{
    public static class PageTypeHelper
    {
        static PageTypeHelper()
        {
            PageTypeFactory = new PageTypeFactory();
        }

        private static PageTypeFactory PageTypeFactory { get; set; }
       
   
        public static PageType GetFunctionPageType(Type functionType)
        {
            var functionAttribute = AttributeHelper.GetFunctionAttribute(functionType);

            if(functionAttribute == null)
            {
                return null;
            }
            return ConvertToPageType(functionType);
        }

        public static PageType GetPageType(Type typedPage)
        {
            var pageTypeAttribute = AttributeHelper.GetPageTypeAttribute(typedPage);

            if(pageTypeAttribute == null)
            {
                return null;
            }

            return ConvertToPageType(typedPage);
        }

        public static PageType GetPageType(int pageTypeId)
        {
            return PageTypeFactory.Load(pageTypeId);
        }
        
        private static PageType ConvertToPageType(Type type)
        {
            int? pageTypeID = PageTypeResolver.Instance.GetPageTypeID(type);

            if(pageTypeID == null)
            {
                return null;
            }

            return PageTypeFactory.Load(pageTypeID.Value);
        }

        public static List<PageType> GetFunctionPageTypes(IList<Type> types)
        {
            if (types == null)
            {
                return new List<PageType>();
            }

            return types.Select(GetFunctionPageType).Where(pt => pt != null).ToList();
        }

        public static List<PageType> GetPageTypes(IList<Type> types)
        {
            if(types == null)
            {
                return new List<PageType>();
            }

            return types.Select(GetPageType).Where(pt => pt != null).ToList();
        }


        public static List<PageType> GetPageTypes(IList<ContentFunctionType> types)
        {
            if (types == null)
            {
                return new List<PageType>();
            }

            return types.Select(t => GetPageType(t.FunctionTypeID)).Where(pt => pt != null).ToList();
        }
    }
}