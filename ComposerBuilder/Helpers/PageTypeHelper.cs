using System;
using System.Collections.Generic;
using System.Linq;
using Dropit.Extension.Controllers;
using Dropit.Extension.Core;
using EPiServer.DataAbstraction;
using EPiServer.Security;
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

        public static void AddContentAreaProperty(PageType pageType, string name, string description)
        {
            Predicate<PageDefinition> match = null;
            if (pageType.Definitions.Find(def => def.Name == name) == null)
            {
                match = delegate(PageDefinition def)
                {
                    return def.Name == name;
                };

                if (pageType.Definitions.Find(match) == null)
                {
                    var item = new PageDefinition();
                    item.PageTypeID = pageType.ID;
                    item.Name = name;
                    item.EditCaption = name;
                    item.HelpText = description;
                    item.DefaultValueType = DefaultValueType.None;
                    item.DefaultValue = "";
                    item.Type = PageTypeManager.ContentAreaPropertyType;
                    item.ID = 0;
                    item.Searchable = true;
                    item.Tab = PageTypeManager.LoadExtensionTab();
                    item.DisplayEditUI = false;
                    item.LanguageSpecific = false;
                    item.LongStringSettings = 0;
                    item.Save();
                    item.ClearCache();
                    pageType.Definitions.Add(item);
                    PageDefinitionType.ClearCache();
                }
            }
        }

        public static void AddExtensionPageProperty(PageType pageType)
        {
            if (pageType.Definitions.Find(def => def.Name == "ExtensionPageProperty") == null)
            {
                if (pageType.Definitions.Find(def => def.Name == "ExtensionPageProperty") == null)
                {
                    var item = new PageDefinition();
                    item.PageTypeID = pageType.ID;
                    item.Name = "ExtensionPageProperty";
                    item.EditCaption = "ExtensionPageProperty";
                    item.HelpText = "Specialized For Extension Added By Extension (Do not remove)";
                    item.DefaultValueType = DefaultValueType.None;
                    item.DefaultValue = "";
                    item.Type = PageTypeManager.ExtensionPageDefinitionType;
                    item.ID = 0;
                    item.Searchable = true;
                    item.Tab = PageTypeManager.LoadExtensionTab();
                    item.DisplayEditUI = true;
                    item.LanguageSpecific = true;
                    item.LongStringSettings = 0;
                    item.Save();
                    item.ClearCache();
                    pageType.Definitions.Add(item);
                }
            }

            PageDefinitionType.ClearCache();
        }

 

 


        public static void SetDefaultAccessPageType(int pageTypeID)
        {
            ManageAccessHandler.SaveAccessPageType(ManageAccessHandler.LoadDefaultAccessPageType(), new AccessControlList(), pageTypeID);
        }

        public static void SetDefaultRulePageType(int pageTypeID)
        {
            RuleContentAreaCollection ruleCaCollection = RuleContentAreaManager.XMLRuleContentAreaList();
            PageType pageType = PageType.Load(pageTypeID);
            if (pageType != null)
            {
                List<ContentAreaData> availableContentAreasViaProperty = ContentAreaManager.GetAvailableContentAreasViaProperty(pageType);
                IList<ContentFunctionType> list2 = ContentFunctionTypeManager.List();
                foreach (ContentAreaData data in availableContentAreasViaProperty)
                {
                    ruleCaCollection.RemoveRuleContentArea(pageType.ID, data.ID);
                    foreach (ContentFunctionType type2 in list2)
                    {
                        ruleCaCollection.AddRule(pageTypeID, data.ID, type2.FunctionTypeID);
                    }
                }
                RuleContentAreaManager.SaveRuleContentAreaCollection(ruleCaCollection);
            }
        }
 


 

 


    }
}