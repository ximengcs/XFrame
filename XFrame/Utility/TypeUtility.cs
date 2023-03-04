using System;

namespace XFrame.Utility
{
    public class TypeUtility
    {
        public static string GetSimpleName(string fullName)
        {
            string[] names = fullName.Split('.');
            return names[names.Length - 1];
        }

        public static bool HasAttribute<T>(object inst) where T : Attribute
        {
            return HasAttribute<T>(inst.GetType());
        }

        public static bool HasAttribute<T>(Type type) where T : Attribute
        {
            return Attribute.GetCustomAttribute(type, typeof(T), true) != null;
        }

        public static T GetAttribute<T>(object inst) where T : Attribute
        {
            return Attribute.GetCustomAttribute(inst.GetType(), typeof(T), true) as T;
        }

        public static T GetAttribute<T>(Type type) where T : Attribute
        {
            return Attribute.GetCustomAttribute(type, typeof(T), true) as T;
        }
    }
}
