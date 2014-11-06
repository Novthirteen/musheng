using System;
using System.Collections;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace NHibernate.Loader
{
	public sealed class OuterJoinableAssociation
	{
		private readonly IAssociationType joinableType;
		private readonly IJoinable joinable;
		private readonly string lhsAlias; // belong to other persister
		private readonly string[] lhsColumns; // belong to other persister
		private readonly string rhsAlias;
		private readonly string[] rhsColumns;
		private readonly JoinType joinType;
		private readonly string on;
		private readonly IDictionary enabledFilters;

		public OuterJoinableAssociation(
			IAssociationType joinableType,
			String lhsAlias,
			String[] lhsColumns,
			String rhsAlias,
			JoinType joinType,
			ISessionFactoryImplementor factory,
			IDictionary enabledFilters)
		{
			this.joinableType = joinableType;
			this.lhsAlias = lhsAlias;
			this.lhsColumns = lhsColumns;
			this.rhsAlias = rhsAlias;
			this.joinType = joinType;
			this.joinable = joinableType.GetAssociatedJoinable(factory);
			this.rhsColumns = JoinHelper.GetRHSColumnNames(joinableType, factory);
			this.on = joinableType.GetOnCondition(rhsAlias, factory, enabledFilters);
			this.enabledFilters = enabledFilters; // needed later for many-to-many/filter application
		}

		public JoinType JoinType
		{
			get { return joinType; }
		}

		public string RHSAlias
		{
			get { return rhsAlias; }
		}

		private bool IsOneToOne
		{
			get
			{
				if (joinableType.IsEntityType)
				{
					EntityType etype = (EntityType) joinableType;
					return etype.IsOneToOne;
				}
				else
				{
					return false;
				}
			}
		}

		public IAssociationType JoinableType
		{
			get { return joinableType; }
		}

		public string RHSUniqueKeyName
		{
			get { return joinableType.RHSUniqueKeyPropertyName; }
		}

		public bool IsCollection
		{
			get { return joinableType.IsCollectionType; }
		}

		public IJoinable Joinable
		{
			get { return joinable; }
		}

		public int GetOwner(IList associations)
		{
			if (IsOneToOne || IsCollection)
			{
				return GetPosition(lhsAlias, associations);
			}
			else
			{
				return -1;
			}
		}

		/// <summary>
		/// Get the position of the join with the given alias in the
		/// list of joins
		/// </summary>
		private static int GetPosition(string lhsAlias, IList associations)
		{
			int result = 0;
			for (int i = 0; i < associations.Count; i++)
			{
				OuterJoinableAssociation oj = (OuterJoinableAssociation) associations[i];
				if (oj.Joinable.ConsumesEntityAlias() /*|| oj.getJoinable().consumesCollectionAlias() */)
				{
					if (oj.rhsAlias.Equals(lhsAlias))
					{
						return result;
					}
					result++;
				}
			}
			return -1;
		}

		public void AddJoins(JoinFragment outerjoin)
		{
			outerjoin.AddJoin(
				joinable.TableName,
				rhsAlias,
				lhsColumns,
				rhsColumns,
				joinType,
				on
				);
			outerjoin.AddJoins(
				joinable.FromJoinFragment(rhsAlias, false, true),
				joinable.WhereJoinFragment(rhsAlias, false, true)
				);
		}

		public void ValidateJoin(String path)
		{
			if (
				rhsColumns == null ||
				lhsColumns == null ||
				lhsColumns.Length != rhsColumns.Length ||
				lhsColumns.Length == 0
				)
			{
				throw new MappingException("invalid join columns for association: " + path);
			}
		}

		public bool IsManyToManyWith(OuterJoinableAssociation other)
		{
			if (joinable.IsCollection)
			{
				IQueryableCollection persister = (IQueryableCollection) joinable;
				if (persister.IsManyToMany)
				{
					return persister.ElementType == other.JoinableType;
				}
			}
			return false;
		}

		public void AddManyToManyJoin(JoinFragment outerjoin, IQueryableCollection collection)
		{
			String manyToManyFilter = collection.GetManyToManyFilterFragment(rhsAlias, enabledFilters);
			String condition = "".Equals(manyToManyFilter)
			                   	? on
			                   	: "".Equals(on)
			                   	  	? manyToManyFilter
			                   	  	: on + " and " + manyToManyFilter;
			outerjoin.AddJoin(
				joinable.TableName,
				rhsAlias,
				lhsColumns,
				rhsColumns,
				joinType,
				condition
				);
			outerjoin.AddJoins(
				joinable.FromJoinFragment(rhsAlias, false, true),
				joinable.WhereJoinFragment(rhsAlias, false, true)
				);
		}
	}
}