using System;
using System.Collections.Generic;
using System.Linq;
using ComposerBuilder.Helpers;
using Dropit.Extension.Controllers;
using Dropit.Extension.Core;
using EPiServer.DataAbstraction;

namespace ComposerBuilder.Synchronization
{
    public class ComposerFunctionUpdater
    {
        private readonly IList<Type> _functions;
        private readonly IList<PageType> _contentTypes;
        private readonly IList<PageType> _pageTypes;

        public ComposerFunctionUpdater()
        {
            _functions = ReflectionHelper.GetTypesWithAttribute(typeof(FunctionTypeAttribute)).ToList();
            _contentTypes = PageTypeHelper.GetPageTypes(ContentFunctionTypeManager.List());
            _pageTypes = PageTypeManager.List();
        }


        public void SyncFunctions()
        {
            foreach (var function in _functions)
            {
                UpdateFunction(function);
            }
        }

        private void UpdateFunction(Type function)
        {
            var functionAttribute = AttributeHelper.GetFunctionAttribute(function);
            var functionType = PageTypeHelper.GetFunctionPageType(function);

            if(functionType == null)
            {
                return;
            }

            UpdateRules(functionAttribute, functionType);
            UpdateAccess(functionAttribute, functionType);
            UpdateContentAreas(functionType);
        }

        private void UpdateRules(FunctionTypeAttribute functionTypeAttribute, PageType functionType)
        {
            int functionTypeID = functionType.ID;
         
            RuleContentAreaCollection ruleCaCollection = RuleContentAreaManager.XMLRuleContentAreaList();
            ruleCaCollection.RemoveRuleContentFunction(functionTypeID);

            var filteredPageTypes = _pageTypes;
            var filteredContetTypes = _contentTypes;

            if(functionTypeAttribute.AvailableOnPages == AvailableOn.Specific)
            {
                filteredPageTypes = PageTypeHelper.GetPageTypes(functionTypeAttribute.AvailableOnPageTypes);
            }

            if (functionTypeAttribute.AvailableOnContent == AvailableOn.Specific)
            {
                filteredContetTypes = PageTypeHelper.GetFunctionPageTypes(functionTypeAttribute.AvailableOnContentTypes);
            }

            foreach (var pageType in filteredPageTypes)
            {
                List<ContentAreaData> pageAreas = ContentAreaManager.GetAvailableContentAreasViaProperty(pageType);

                foreach(var pageArea in pageAreas)
                {
                    ruleCaCollection.AddRule(pageType.ID, pageArea.ID, functionTypeID);
                }
            }

            foreach (var contentType in filteredContetTypes)
            {
                if(contentType.ID == functionTypeID)
                {
                    continue;
                }

                List<ContentAreaData> contentAreas = ContentAreaManager.GetAvailableContentAreasViaProperty(contentType.ID);

                foreach (var contentArea in contentAreas)
                {
                    ruleCaCollection.AddRule(contentType.ID, contentArea.ID, functionTypeID);
                }
            }

            RuleContentAreaManager.SaveRuleContentAreaCollection(ruleCaCollection);
        }

        private void UpdateAccess(FunctionTypeAttribute functionTypeAttribute, PageType functionType)
        {
            int functionTypeID = functionType.ID;

            var functionAccessControlList = ManageAccessHandler.GetACLOfFunctionType(functionTypeID);
            ContentFunctionType.Load(functionTypeID);

            var defaultAccessControlList = ManageAccessHandler.LoadDefaultAccessFunctionType();

            foreach (var accessEntry in defaultAccessControlList.Where(a => functionAccessControlList.Exists(a.Key) == false).ToList())
            {
                functionAccessControlList.Add(accessEntry.Value);
            }

            ManageAccessHandler.SaveAccessFunctionType(functionAccessControlList, null, functionTypeID);
        }

        private void UpdateContentAreas(PageType functionType)
        {
            ContentFunctionTypeManager.ReRegisterCA(functionType);
        }
    }
}