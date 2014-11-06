using System;
using System.Collections;
using Iesi.Collections;

namespace NHibernate.Util
{
	public sealed class CollectionHelper
	{
		private class EmptyEnumerator : IDictionaryEnumerator
		{
			public object Key
			{
				get { throw new InvalidOperationException("EmptyEnumerator.get_Key"); }
			}

			public object Value
			{
				get { throw new InvalidOperationException("EmptyEnumerator.get_Value"); }
			}

			public DictionaryEntry Entry
			{
				get { throw new InvalidOperationException("EmptyEnumerator.get_Entry"); }
			}

			public void Reset()
			{
			}

			public object Current
			{
				get { throw new InvalidOperationException("EmptyEnumerator.get_Current"); }
			}

			public bool MoveNext()
			{
				return false;
			}
		}

		/// <summary>
		/// A read-only dictionary that is always empty and permits lookup by <see langword="null" /> key.
		/// </summary>
		private class EmptyMapClass : IDictionary
		{
			private static readonly EmptyEnumerator EmptyEnumerator = new EmptyEnumerator();

			public bool Contains(object key)
			{
				return false;
			}

			public void Add(object key, object value)
			{
				throw new NotSupportedException("EmptyMap.Add");
			}

			public void Clear()
			{
				throw new NotSupportedException("EmptyMap.Clear");
			}

			IDictionaryEnumerator IDictionary.GetEnumerator()
			{
				return EmptyEnumerator;
			}

			public void Remove(object key)
			{
				throw new NotSupportedException("EmptyMap.Remove");
			}

			public object this[object key]
			{
				get { return null; }
				set { throw new NotSupportedException("EmptyMap.set_Item"); }
			}

			public ICollection Keys
			{
				get { return this; }
			}

			public ICollection Values
			{
				get { return this; }
			}

			public bool IsReadOnly
			{
				get { return true; }
			}

			public bool IsFixedSize
			{
				get { return true; }
			}

			public void CopyTo(Array array, int index)
			{
			}

			public int Count
			{
				get { return 0; }
			}

			public object SyncRoot
			{
				get { return this; }
			}

			public bool IsSynchronized
			{
				get { return false; }
			}

			public IEnumerator GetEnumerator()
			{
				return EmptyEnumerator;
			}
		}

		public static readonly IDictionary EmptyMap = new EmptyMapClass();
		public static readonly ICollection EmptyCollection = EmptyMap;

		public static bool CollectionEquals(ICollection c1, ICollection c2)
		{
			if (c1 == c2)
			{
				return true;
			}

			if(c1==null || c2==null)
			{
				return false;
			}

			if (c1.Count != c2.Count)
			{
				return false;
			}

			IEnumerator e1 = c1.GetEnumerator();
			IEnumerator e2 = c2.GetEnumerator();

			while (e1.MoveNext())
			{
				e2.MoveNext();
				if (!Equals(e1.Current, e2.Current))
				{
					return false;
				}
			}

			return true;
		}

		public static bool DictionaryEquals(IDictionary a, IDictionary b)
		{
			if (Equals(a, b))
			{
				return true;
			}

			if (a == null || b == null)
			{
				return false;
			}

			if (a.Count != b.Count)
			{
				return false;
			}

			foreach (object key in a.Keys)
			{
				if (!Equals(a[key], b[key]))
				{
					return false;
				}
			}

			return true;
		}

		public static bool SetEquals(ISet a, ISet b)
		{
			if (Equals(a, b))
			{
				return true;
			}

			if (a == null || b == null)
			{
				return false;
			}

			if (a.Count != b.Count)
			{
				return false;
			}

			foreach (object obj in a)
			{
				if (!b.Contains(obj))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Computes a hash code for <paramref name="coll"/>.
		/// </summary>
		/// <remarks>The hash code is computed as the sum of hash codes of
		/// individual elements, so that the value is independent of the
		/// collection iteration order.
		/// </remarks>
		public static int GetHashCode(ICollection coll)
		{
			unchecked
			{
				int result = 0;

				foreach (object obj in coll)
				{
					if (obj != null)
					{
						result += obj.GetHashCode();
					}
				}

				return result;
			}
		}

		/// <summary>
		/// Creates a <see cref="Hashtable" /> that uses case-insensitive string comparison
		/// associated with invariant culture.
		/// </summary>
		/// <remarks>
		/// This is different from the method in <see cref="System.Collections.Specialized.CollectionsUtil" />
		/// in that the latter uses the current culture and is thus vulnerable to the "Turkish I" problem.
		/// </remarks>
		public static Hashtable CreateCaseInsensitiveHashtable()
		{
			return new Hashtable(
				CaseInsensitiveHashCodeProvider.DefaultInvariant,
				CaseInsensitiveComparer.DefaultInvariant
				);
		}

		/// <summary>
		/// Creates a <see cref="Hashtable" /> that uses case-insensitive string comparison
		/// associated with invariant culture.
		/// </summary>
		/// <remarks>
		/// This is different from the method in <see cref="System.Collections.Specialized.CollectionsUtil" />
		/// in that the latter uses the current culture and is thus vulnerable to the "Turkish I" problem.
		/// </remarks>
		public static Hashtable CreateCaseInsensitiveHashtable(IDictionary dictionary)
		{
			return new Hashtable(
				dictionary,
				CaseInsensitiveHashCodeProvider.DefaultInvariant,
				CaseInsensitiveComparer.DefaultInvariant
				);
		}

		private CollectionHelper()
		{
		}
	}
}