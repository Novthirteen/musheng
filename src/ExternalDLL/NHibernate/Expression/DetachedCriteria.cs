using System;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace NHibernate.Expression
{
	/// <summary>
	/// Some applications need to create criteria queries in "detached
	/// mode", where the Hibernate session is not available. This class
	/// may be instantiated anywhere, and then a <c>ICriteria</c>
	/// may be obtained by passing a session to 
	/// <c>GetExecutableCriteria()</c>. All methods have the
	/// same semantics and behavior as the corresponding methods of the
	/// <c>ICriteria</c> interface.
	/// </summary>
	[Serializable]
	public class DetachedCriteria
	{
		private CriteriaImpl impl;
		private ICriteria criteria;

		protected DetachedCriteria(System.Type entityType)
		{
			impl = new CriteriaImpl(entityType, null);
			criteria = impl;
		}

		protected DetachedCriteria(System.Type entityType, string alias)
		{
			impl = new CriteriaImpl(entityType, alias, null);
			criteria = impl;
		}

		protected DetachedCriteria(CriteriaImpl impl, ICriteria criteria)
		{
			this.impl = impl;
			this.criteria = criteria;
		}

		/// <summary>
		/// Get an executable instance of <c>Criteria</c>,
		/// to actually run the query.</summary>
		public ICriteria GetExecutableCriteria(ISession session)
		{
			impl.Session = session.GetSessionImplementation();
			return impl;
		}

		public static DetachedCriteria For(System.Type entityType)
		{
			return new DetachedCriteria(entityType);
		}


		public static DetachedCriteria For<T>()
		{
			return new DetachedCriteria(typeof(T));
		}

		public static DetachedCriteria For(System.Type entityType, string alias)
		{
			return new DetachedCriteria(entityType, alias);
		}


		public DetachedCriteria Add(ICriterion criterion)
		{
			criteria.Add(criterion);
			return this;
		}

		public DetachedCriteria AddOrder(Order order)
		{
			criteria.AddOrder(order);
			return this;
		}

		public DetachedCriteria CreateAlias(string associationPath, string alias)
		{
			criteria.CreateAlias(associationPath, alias);
			return this;
		}

		public DetachedCriteria CreateAlias(string associationPath, string alias, JoinType joinType)
		{
			criteria.CreateAlias(associationPath, alias, joinType);
			return this;
		}

		public DetachedCriteria CreateCriteria(string associationPath, string alias)
		{
			return new DetachedCriteria(impl, criteria.CreateCriteria(associationPath, alias));
		}

		public DetachedCriteria CreateCriteria(string associationPath)
		{
			return new DetachedCriteria(impl, criteria.CreateCriteria(associationPath));
		}

		public DetachedCriteria CreateCriteria(string associationPath, JoinType joinType)
		{
			return new DetachedCriteria(impl, criteria.CreateCriteria(associationPath, joinType));
		}

		public DetachedCriteria CreateCriteria(string associationPath, string alias, JoinType joinType)
		{
			return new DetachedCriteria(impl, criteria.CreateCriteria(associationPath, alias, joinType));
		}

		public string Alias
		{
			get { return criteria.Alias; }
		}

		public DetachedCriteria SetFetchMode(string associationPath, FetchMode mode)
		{
			criteria.SetFetchMode(associationPath, mode);
			return this;
		}

		public DetachedCriteria SetProjection(IProjection projection)
		{
			criteria.SetProjection(projection);
			return this;
		}

		public DetachedCriteria SetResultTransformer(IResultTransformer resultTransformer)
		{
			criteria.SetResultTransformer(resultTransformer);
			return this;
		}

		public override string ToString()
		{
			return "DetachableCriteria(" + criteria.ToString() + ')';
		}

		protected internal CriteriaImpl GetCriteriaImpl()
		{
			return impl;
		}
	}
}