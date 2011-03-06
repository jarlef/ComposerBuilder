using System;
using PageTypeBuilder;

namespace ComposerBuilder.Helpers
{
    public static class AttributeHelper
    {
        public static FunctionTypeAttribute GetFunctionAttribute(Type functionType)
        {
            return ReflectionHelper.GetAttribute(functionType, typeof(FunctionTypeAttribute)) as FunctionTypeAttribute;
        }

        public static PageTypeAttribute GetPageTypeAttribute(Type functionType)
        {
            return ReflectionHelper.GetAttribute(functionType, typeof(PageTypeAttribute)) as PageTypeAttribute;
        }
    }
}
