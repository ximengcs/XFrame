using System;
using System.Reflection;

namespace XFrame.Core
{
    public class Pair
    {
        public static Pair<K, V> Create<K, V>(K key, V value)
        {
            return new Pair<K, V>(key, value);
        }
    }

    public partial struct Pair<K, V>
    {
        public K Key { get; private set; }
        public V Value { get; private set; }

        public Pair(K key, V value)
        {
            Key = key;
            Value = value;
        }

        public static bool operator ==(Pair<K, V> src, Pair<K, V> tar)
        {
            return src.Equals(tar);
        }

        public static bool operator !=(Pair<K, V> src, Pair<K, V> tar)
        {
            return !src.Equals(tar);
        }

        public override bool Equals(object obj)
        {
            if (obj is Pair<K, V> other)
            {
                return Key.Equals(other.Key) && Value.Equals(other.Value);
            }
            else
            {
                Type type = obj.GetType();
                if (s_GenericType == type.GetGenericTypeDefinition())
                {
                    PropertyInfo key = type.GetProperty(nameof(Key));
                    PropertyInfo value = type.GetProperty(nameof(Value));
                    return Key.Equals(key.GetValue(obj)) && Value.Equals(value.GetValue(obj));
                }
                else
                {
                    return base.Equals(obj);
                }
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"[{Key},{Value}]";
        }
    }
}
