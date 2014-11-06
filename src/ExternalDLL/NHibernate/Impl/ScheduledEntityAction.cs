using System;
using System.IO;
using System.Runtime.Serialization;
using NHibernate.Engine;
using NHibernate.Persister.Entity;

namespace NHibernate.Impl
{
	/// <summary>
	/// The base class for a scheduled action to perform on an entity during a
	/// flush.
	/// </summary>
	[Serializable]
	internal abstract class ScheduledEntityAction : IExecutable, IDeserializationCallback
	{
		private readonly ISessionImplementor session;
		private readonly object id;

		[NonSerialized]
		private IEntityPersister persister;

		private readonly object instance;

		/// <summary>
		/// Initializes a new instance of <see cref="ScheduledEntityAction"/>.
		/// </summary>
		/// <param name="session">The <see cref="ISessionImplementor"/> that the Action is occuring in.</param>
		/// <param name="id">The identifier of the object.</param>
		/// <param name="instance">The actual object instance.</param>
		/// <param name="persister">The <see cref="IEntityPersister"/> that is responsible for the persisting the object.</param>
		protected ScheduledEntityAction(ISessionImplementor session, object id, object instance, IEntityPersister persister)
		{
			this.session = session;
			this.id = id;
			this.persister = persister;
			this.instance = instance;
		}


		/// <summary>
		/// Gets the <see cref="ISessionImplementor"/> the action is executing in.
		/// </summary>
		protected ISessionImplementor Session
		{
			get { return session; }
		}

		/// <summary>
		/// Gets the identifier of the object.
		/// </summary>
		protected object Id
		{
			get { return id; }
		}

		/// <summary>
		/// Gets the <see cref="IEntityPersister"/> that is responsible for persisting the object.
		/// </summary>
		protected IEntityPersister Persister
		{
			get { return persister; }
		}

		/// <summary>
		/// Gets the object that is having the scheduled action performed against it.
		/// </summary>
		protected object Instance
		{
			get { return instance; }
		}

		#region SessionImpl.IExecutable Members

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>Not supported for a non-collection entity</remarks>
		public void BeforeExecutions()
		{
			throw new NotSupportedException("BeforeExecutions() called for non-collection method");
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual bool HasAfterTransactionCompletion
		{
			get { return persister.HasCache; }
		}

		/// <summary>
		/// Called when the Transaction this action occurred in has completed.
		/// </summary>
		/// <param name="success"></param>
		public abstract void AfterTransactionCompletion(bool success);

		/// <summary>
		/// Execute the action using the <see cref="IEntityPersister"/>.
		/// </summary>
		public abstract void Execute();

		/// <summary></summary>
		public object[] PropertySpaces
		{
			get { return persister.PropertySpaces; }
		}

		#endregion

		#region IDeserializationCallback member

		void IDeserializationCallback.OnDeserialization(object sender)
		{
			try
			{
				persister = session.GetEntityPersister(instance);
			}
			catch (MappingException e)
			{
				throw new IOException("Unable to resolve class persister on deserialization", e);
			}
		}

		#endregion
	}
}