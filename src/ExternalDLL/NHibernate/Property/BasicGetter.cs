using System;
using System.Reflection;
using System.Reflection.Emit;

namespace NHibernate.Property
{
	/// <summary>
	/// An <see cref="IGetter"/> for a Property <c>get</c>.
	/// </summary>
    [Serializable]
    public sealed class BasicGetter : IGetter, IOptimizableGetter
	{
		private System.Type clazz;
		private PropertyInfo property;
		private string propertyName;

		/// <summary>
		/// Initializes a new instance of <see cref="BasicGetter"/>.
		/// </summary>
		/// <param name="clazz">The <see cref="System.Type"/> that contains the Property <c>get</c>.</param>
		/// <param name="property">The <see cref="PropertyInfo"/> for reflection.</param>
		/// <param name="propertyName">The name of the Property.</param>
		public BasicGetter(System.Type clazz, PropertyInfo property, string propertyName)
		{
			this.clazz = clazz;
			this.property = property;
			this.propertyName = propertyName;
		}

		public PropertyInfo Property
		{
			get { return property; }
		}

		#region IGetter Members

		/// <summary>
		/// Gets the value of the Property from the object.
		/// </summary>
		/// <param name="target">The object to get the Property value from.</param>
		/// <returns>
		/// The value of the Property for the target.
		/// </returns>
		public object Get(object target)
		{
			try
			{
				return property.GetValue(target, new object[0]);
			}
			catch (Exception e)
			{
				throw new PropertyAccessException(e, "Exception occurred", false, clazz, propertyName);
			}
		}

		/// <summary>
		/// Gets the <see cref="System.Type"/> that the Property returns.
		/// </summary>
		/// <value>The <see cref="System.Type"/> that the Property returns.</value>
		public System.Type ReturnType
		{
			get { return property.PropertyType; }
		}

		/// <summary>
		/// Gets the name of the Property.
		/// </summary>
		/// <value>The name of the Property.</value>
		public string PropertyName
		{
			get { return property.Name; }
		}

		/// <summary>
		/// Gets the <see cref="PropertyInfo"/> for the Property.
		/// </summary>
		/// <value>
		/// The <see cref="PropertyInfo"/> for the Property.
		/// </value>
		public MethodInfo Method
		{
			get { return property.GetGetMethod(true); }
		}

		#endregion

		public void Emit(ILGenerator il)
		{
			MethodInfo method = Method;
			if (method == null)
			{
				throw new PropertyNotFoundException(clazz, property.Name, "getter");
			}
			il.EmitCall(OpCodes.Callvirt, method, null);
		}
	}
}