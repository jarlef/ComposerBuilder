using System;
using System.Collections.Generic;
using System.Linq;
using ComposerBuilder.Helpers;
using Dropit.Extension.Controllers;
using Dropit.Extension.Core;
using EPiServer.DataAbstraction;

namespace ComposerBuilder.Synchronization
{
    public class ComposerPageUpdater
    {
        private readonly List<Type> _pages;

        public ComposerPageUpdater()
        {
            _pages = ReflectionHelper.GetTypesWithAttribute(typeof(ExtensionPageTypeAttribute)).ToList();
        }

        public void SyncPages()
        {
            foreach (var page in _pages)
            {
                UpdatePage(page);
            }
        }

        private void UpdatePage(Type page)
        {
            try
            {
                var pageAttribute = AttributeHelper.GetPageTypeAttribute(page) as ExtensionPageTypeAttribute;
                var pageType = PageTypeHelper.GetPageType(page);

                if (pageAttribute == null || pageType == null)
                {
                    return;
                }

                UpdatePageRegistration(pageAttribute, pageType);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private void UpdatePageRegistration(ExtensionPageTypeAttribute pageAttribute, PageType pageType)
        {
            if (!PageTypeManager.UpdateNameAndDescription(pageAttribute.Name, pageAttribute.Description, pageType.ID))
            {
                return;
            }

            ConvertToExtensionPageType(pageType.ID, true);
        }

        public static void ConvertToExtensionPageType(int pageTypeID, bool updateSortOrder)
        {
            var pageType = PageType.Load(pageTypeID);
            
            if (!PageTypeManager.IsExtensionPageType(pageType))
            {
                PageTypeHelper.AddExtensionPageProperty(pageType);

                if (updateSortOrder)
                {
                    var source = PageTypeManager.List();
                    if ((source != null) && (source.Count > 0))
                    {
                        pageType.SortOrder = source.Last().SortOrder + 10;
                    }
                }

                pageType.IsAvailable = true;
                pageType.Save();

                //private methods for access and rules
                PageTypeHelper.SetDefaultAccessPageType(pageType.ID);
                PageTypeHelper.SetDefaultRulePageType(pageType.ID);
            }
        }
    }
}