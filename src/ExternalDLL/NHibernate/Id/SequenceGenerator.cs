using System;
using System.Collections;
using System.Data;
using log4net;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref="IIdentifierGenerator" /> that generates <c>Int64</c> values using an 
	/// oracle-style sequence. A higher performance algorithm is 
	/// <see cref="SequenceHiLoGenerator"/>.
	/// </summary>
	/// <remarks>
	/// <p>
	///	This id generation strategy is specified in the mapping file as 
	///	<code>
	///	&lt;generator class="sequence"&gt;
	///		&lt;param name="sequence"&gt;uid_sequence&lt;/param&gt;
	///		&lt;param name="schema"&gt;db_schema&lt;/param&gt;
	///	&lt;/generator&gt;
	///	</code>
	/// </p>
	/// <p>
	/// The <c>sequence</c> parameter is required while the <c>schema</c> is optional.
	/// </p>
	/// </remarks>
	public class SequenceGenerator : IPersistentIdentifierGenerator, IConfigurable
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(SequenceGenerator));

		/// <summary>
		/// The name of the sequence parameter.
		/// </summary>
		public const string Sequence = "sequence";

		/// <summary>
		/// The name of the schema parameter.
		/// </summary>
		public const string Schema = "schema";

		private string sequenceName;
		private IType type;
		private SqlString sql;

		#region IConfigurable Members

		/// <summary>
		/// Configures the SequenceGenerator by reading the value of <c>sequence</c> and
		/// <c>schema</c> from the <c>parms</c> parameter.
		/// </summary>
		/// <param name="type">The <see cref="IType"/> the identifier should be.</param>
		/// <param name="parms">An <see cref="IDictionary"/> of Param values that are keyed by parameter name.</param>
		/// <param name="dialect">The <see cref="Dialect.Dialect"/> to help with Configuration.</param>
		public virtual void Configure(IType type, IDictionary parms, Dialect.Dialect dialect)
		{
			this.sequenceName = PropertiesHelper.GetString(Sequence, parms, "hibernate_sequence");
			string schemaName = (string) parms[Schema];
			if (schemaName != null && sequenceName.IndexOf(StringHelper.Dot) < 0)
			{
				sequenceName = schemaName + '.' + sequenceName;
			}
			this.type = type;
			sql = new SqlString(dialect.GetSequenceNextValString(sequenceName));
		}

		#endregion

		#region IIdentifierGenerator Members

		/// <summary>
		/// Generate an <see cref="Int16"/>, <see cref="Int32"/>, or <see cref="Int64"/> 
		/// for the identifier by using a database sequence.
		/// </summary>
		/// <param name="session">The <see cref="ISessionImplementor"/> this id is being generated in.</param>
		/// <param name="obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier as a <see cref="Int16"/>, <see cref="Int32"/>, or <see cref="Int64"/>.</returns>
		public virtual object Generate(ISessionImplementor session, object obj)
		{
			IDbCommand cmd = session.Batcher.PrepareCommand(CommandType.Text, sql, SqlTypeFactory.NoTypes);
			IDataReader reader = null;
			try
			{
				reader = session.Batcher.ExecuteReader(cmd);
				reader.Read();
				object result = IdentifierGeneratorFactory.Get(reader, type, session);

				log.Debug("sequence ID generated: " + result);
				return result;
			} 
				// TODO: change to SQLException
			catch (Exception e)
			{
				// TODO: add code to log the sql exception
				log.Error("error generating sequence", e);
				throw;
			}
			finally
			{
				session.Batcher.CloseCommand(cmd, reader);
			}
		}

		#endregion

		#region IPersistentIdentifierGenerator Members

		/// <summary>
		/// The SQL required to create the database objects for a SequenceGenerator.
		/// </summary>
		/// <param name="dialect">The <see cref="Dialect.Dialect"/> to help with creating the sql.</param>
		/// <returns>
		/// An array of <see cref="String"/> objects that contain the Dialect specific sql to 
		/// create the necessary database objects for the SequenceGenerator.
		/// </returns>
		public string[] SqlCreateStrings(Dialect.Dialect dialect)
		{
			return new string[]
				{
					dialect.GetCreateSequenceString(sequenceName)
				};
		}

		/// <summary>
		/// The SQL required to remove the underlying database objects for a SequenceGenerator.
		/// </summary>
		/// <param name="dialect">The <see cref="Dialect.Dialect"/> to help with creating the sql.</param>
		/// <returns>
		/// A <see cref="String"/> that will drop the database objects for the SequenceGenerator.
		/// </returns>
		public string SqlDropString(Dialect.Dialect dialect)
		{
			return dialect.GetDropSequenceString(sequenceName);
		}

		/// <summary>
		/// Return a key unique to the underlying database objects for a SequenceGenerator.
		/// </summary>
		/// <returns>
		/// The configured sequence name.
		/// </returns>
		public object GeneratorKey()
		{
			return sequenceName;
		}

		#endregion
	}
}