using System;
using System.Collections.Generic;
using System.Linq;
using ComposerBuilder.Helpers;
using Dropit.Extension.Controllers;
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
                //Cannot update areas cause this causes page requests + use http context that does not exist in this context
                //foreach (ContentAreaData data in ContentAreaManager.GetAvailableContentAreas(pageType))
                //{
                //    PageTypeManager.AddContentAreaProperty(pageType, data.ID, data.Description);
                //}
                //AddExtensionPageProperty(pageType);

                if (updateSortOrder)
                {
                    PageTypeCollection source = PageTypeManager.List();
                    if ((source != null) && (source.Count > 0))
                    {
                        pageType.SortOrder = source.Last().SortOrder + 10;
                    }
                }
                pageType.IsAvailable = true;
                pageType.Save();

                //private methods for access and rules
                //PageTypeManager.SetDefaultAccessPageType(pageType.ID);
                //PageTypeManager.SetDefaultRulePageType(pageType.ID);
            }
        }

 

    }
}