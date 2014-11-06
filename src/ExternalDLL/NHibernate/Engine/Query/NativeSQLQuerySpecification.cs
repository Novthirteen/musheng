using System;
using System.Collections;
using Iesi.Collections;
using NHibernate.Loader.Custom;
using NHibernate.Util;

namespace NHibernate.Engine.Query
{
	public class NativeSQLQuerySpecification
	{
		private readonly string queryString;
		private readonly ISQLQueryReturn[] sqlQueryReturns;
		private readonly ISet querySpaces;
		private readonly int hashCode;

		public NativeSQLQuerySpecification(
			string queryString,
			ISQLQueryReturn[] sqlQueryReturns,
			ICollection querySpaces)
		{
			this.queryString = queryString;
			this.sqlQueryReturns = sqlQueryReturns;

			if (querySpaces == null)
			{
				this.querySpaces = new HashedSet();
			}
			else
			{
				ISet tmp = new HashedSet();
				tmp.AddAll(querySpaces);
				// Can't use ImmutableSet here because it doesn't implement GetHashCode properly.
				this.querySpaces = tmp;
			}

			// pre-determine and cache the hashcode
			int hashCode = queryString.GetHashCode();
			unchecked
			{
				hashCode = 29 * hashCode + this.querySpaces.GetHashCode();
				if (this.sqlQueryReturns != null)
				{
					hashCode = 29 * hashCode + sqlQueryReturns.Length;
				}
			}

			this.hashCode = hashCode;
		}

		public string QueryString
		{
			get { return queryString; }
		}

		public ISQLQueryReturn[] SqlQueryReturns
		{
			get { return sqlQueryReturns; }
		}

		public ISet QuerySpaces
		{
			get { return querySpaces; }
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}

			if (o == null || GetType() != o.GetType())
			{
				return false;
			}

			NativeSQLQuerySpecification that = (NativeSQLQuerySpecification) o;

			return hashCode == that.hashCode &&
			       CollectionHelper.CollectionEquals(querySpaces, that.querySpaces) &&
			       querySpaces.Equals(that.querySpaces) &&
			       queryString.Equals(that.queryString) &&
			       CollectionHelper.CollectionEquals(sqlQueryReturns, that.sqlQueryReturns);
		}

		public override int GetHashCode()
		{
			return hashCode;
		}
	}
}