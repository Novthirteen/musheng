using System;
using System.Collections;
using Iesi.Collections;

namespace NHibernate.Type
{
	/// <summary>
	/// Extends the <see cref="SetType" /> to provide sorting.
	/// </summary>
	[Serializable]
	public class SortedSetType : SetType
	{
		private IComparer comparer;

		/// <summary>
		/// Initializes a new instance of a <see cref="SortedSetType"/> class for
		/// a specific role using the <see cref="IComparer"/> to do the sorting.
		/// </summary>
		/// <param name="role">The role the persistent collection is in.</param>
		/// <param name="propertyRef">The name of the property in the
		/// owner object containing the collection ID, or <see langword="null" /> if it is
		/// the primary key.</param>
		/// <param name="comparer">The <see cref="IComparer"/> to use for the sorting.</param>
		public SortedSetType(string role, string propertyRef, IComparer comparer)
			: base(role, propertyRef)
		{
			this.comparer = comparer;
		}

		public IComparer Comparer
		{
			get { return comparer; }
		}

		public override object Instantiate()
		{
			return new SortedSet(comparer);
		}
	}
}