using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ComposerBuilder.Helpers
{
    /// <summary>
    /// Based on AttributedTypeUtility from PageTypeBuilder
    /// </summary>
    public static class ReflectionHelper
    {
        // Methods

        public static Attribute GetAttribute(Type type, Type attributeType)
        {
            Attribute attribute = null;
            foreach (object obj2 in type.GetCustomAttributes(true))
            {
                if (attributeType.IsAssignableFrom(obj2.GetType()))
                {
                    attribute = (Attribute)obj2;
                }
            }
            return attribute;
        }

        private static object[] GetAttributes(Type type)
        {
            return type.GetCustomAttributes(true);
        }

        public static List<Type> GetTypesWithAttribute(Type attributeType)
        {
            Func<AssemblyName, bool> predicate = null;
            string attributeAssemblyName = attributeType.Assembly.GetName().Name;
            var list = new List<Type>();

            //IEnumerable<Type> typesWithAttributeInAssembly = GetTypesWithAttributeInAssembly(attributeType.Assembly, attributeType);
            //list.AddRange(typesWithAttributeInAssembly);
            
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (predicate == null)
                {
                    predicate = delegate(AssemblyName a)
                    {
                        return a.Name == attributeAssemblyName;
                    };
                }
                if (assembly.GetReferencedAssemblies().Count(predicate) != 0)
                {
                    IEnumerable<Type> typesWithAttributeInAssembly = GetTypesWithAttributeInAssembly(assembly, attributeType);
                    list.AddRange(typesWithAttributeInAssembly);
                }
            }
            return list;
        }

        private static IEnumerable<Type> GetTypesWithAttributeInAssembly(Assembly assembly, Type attributeType)
        {
            return assembly.GetTypes().Where(type => TypeHasAttribute(type, attributeType)).ToList();
        }

        private static bool TypeHasAttribute(Type type, Type attributeType)
        {
            bool flag = false;
            foreach (object obj2 in GetAttributes(type))
            {
                if (attributeType.IsAssignableFrom(obj2.GetType()))
                {
                    flag = true;
                }
            }
            return flag;
        }
    }


}