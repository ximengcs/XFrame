using System;

namespace XFrame.Utility
{
    public class TypeUtility
    {
        public static string GetSimpleName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return fullName;
            string[] names = fullName.Split('.');
            return names[names.Length - 1];
        }

        public static bool HasAttribute(object inst, Type type)
        {
            return HasAttribute(inst.GetType(), type);
        }

        public static bool HasAttribute(Type instType, Type type)
        {
            return Attribute.GetCustomAttribute(instType, type, true) != null;
        }

        public static bool HasAttribute<T>(object inst) where T : Attribute
        {
            return HasAttribute(inst.GetType(), typeof(T));
        }

        public static bool HasAttribute<T>(Type type) where T : Attribute
        {
            return HasAttribute(type, typeof(T));
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
