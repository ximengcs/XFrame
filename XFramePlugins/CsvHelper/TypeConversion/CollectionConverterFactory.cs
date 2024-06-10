﻿// Copyright 2009-2022 Josh Close
// This file is a part of CsvHelper and is dual licensed under MS-PL and Apache 2.0.
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html for MS-PL and http://opensource.org/licenses/Apache-2.0 for Apache 2.0.
// https://github.com/JoshClose/CsvHelper
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsvHelper.TypeConversion
{
	/// <inheritdoc />
	public class CollectionConverterFactory : ITypeConverterFactory
	{
		private int dictionaryTypeHashCode = typeof(IDictionary).GetHashCode();
		private List<int> enumerableTypeHashCodes = new List<int>
		{
			typeof(IList).GetHashCode(),
			typeof(ICollection).GetHashCode(),
			typeof(IEnumerable).GetHashCode(),
		};

		/// <inheritdoc />
		public bool CanCreate(Type type)
		{
			if (type is IList) return true;
			if (type is IDictionary) return true;
			if (type is ICollection) return true;
			if (type is IEnumerable) return true;

			if (type.IsArray)
			{
				// ArrayConverter
				return true;
			}

			if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
			{
				// IDictionaryGenericConverter
				return true;
			}

			if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
			{
				// IDictionaryGenericConverter
				return true;
			}

			if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
			{
				// CollectionGenericConverter
				return true;
			}

			if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Collection<>))
			{
				// CollectionGenericConverter
				return true;
			}

			if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
			{
				// IEnumerableGenericConverter
				return true;
			}

			if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>))
			{
				// IEnumerableGenericConverter
				return true;
			}

			if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			{
				// IEnumerableGenericConverter
				return true;
			}

			// A specific IEnumerable converter doesn't exist.
			if (typeof(IEnumerable).IsAssignableFrom(type))
			{
				// EnumerableConverter
				return true;
			}

			return false;
		}

		/// <inheritdoc />
		public bool Create(Type type, TypeConverterCache cache, out ITypeConverter typeConverter)
		{
			var typeHashCode = type.GetHashCode();

			if (typeHashCode == dictionaryTypeHashCode)
			{
				typeConverter = new IDictionaryConverter();
				return true;
			}

			if (enumerableTypeHashCodes.Contains(typeHashCode))
			{
				typeConverter = new IEnumerableConverter();
				return true;
			}

			if (type.IsArray)
			{
				typeConverter = new ArrayConverter();
				return true;
			}

			var isGenericType = type.GetTypeInfo().IsGenericType;
			var genericTypeDefinition = type.GetGenericTypeDefinition();

			if (isGenericType && genericTypeDefinition == typeof(Dictionary<,>))
			{
				typeConverter = new IDictionaryGenericConverter();
				return true;
			}

			if (isGenericType && genericTypeDefinition == typeof(IDictionary<,>))
			{
				typeConverter = new IDictionaryGenericConverter();
				return true;
			}

			if (isGenericType && genericTypeDefinition == typeof(List<>))
			{
				typeConverter = new CollectionGenericConverter();
				return true;
			}

			if (isGenericType && genericTypeDefinition == typeof(Collection<>))
			{
				typeConverter = new CollectionGenericConverter();
				return true;
			}

			if (isGenericType && genericTypeDefinition == typeof(IList<>))
			{
				typeConverter = new IEnumerableGenericConverter();
				return true;
			}

			if (isGenericType && genericTypeDefinition == typeof(ICollection<>))
			{
				typeConverter = new IEnumerableGenericConverter();
				return true;
			}

			if (isGenericType && genericTypeDefinition == typeof(IEnumerable<>))
			{
				typeConverter = new IEnumerableGenericConverter();
				return true;
			}

			// A specific IEnumerable converter doesn't exist.
			if (typeof(IEnumerable).IsAssignableFrom(type))
			{
				typeConverter = new EnumerableConverter();
				return true;
			}

			throw new InvalidOperationException($"Cannot create collection converter for type '{type.FullName}'.");
		}
	}
}
