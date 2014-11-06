using System;
using System.Collections;
using NHibernate.Engine;
using NHibernate.Property;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.Mapping
{
	/// <summary>
	/// Mapping for a property of a .NET class (entity
	/// or component).
	/// </summary>
	public class Property
	{
		private string name;
		private IValue propertyValue;
		private string cascade;
		private bool updateable = true;
		private bool insertable = true;
		private string propertyAccessorName;
		private IDictionary metaAttributes;
		private PersistentClass persistentClass;
		private bool isOptimisticLocked;
		private PropertyGeneration generation = PropertyGeneration.Never;

		public Property()
		{
		}

		public Property(IValue propertyValue)
		{
			this.propertyValue = propertyValue;
		}

		public IType Type
		{
			get { return propertyValue.Type; }
		}

		/// <summary>
		/// Gets the number of columns this property uses in the db.
		/// </summary>
		public int ColumnSpan
		{
			get { return propertyValue.ColumnSpan; }
		}

		/// <summary>
		/// Gets an <see cref="ICollection"/> of <see cref="Column"/>s.
		/// </summary>
		public ICollection ColumnCollection
		{
			get { return propertyValue.ColumnCollection; }
		}

		/// <summary>
		/// Gets or Sets the name of the Property in the class.
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public bool IsComposite
		{
			get { return propertyValue is Component; }
		}

		public IValue Value
		{
			get { return propertyValue; }
			set { this.propertyValue = value; }
		}

		public Cascades.CascadeStyle CascadeStyle
		{
			get
			{
				IType type = propertyValue.Type;
				if (type.IsComponentType && !type.IsAnyType)
				{
					IAbstractComponentType actype = (IAbstractComponentType) propertyValue.Type;
					int length = actype.Subtypes.Length;
					for (int i = 0; i < length; i++)
					{
						if (actype.GetCascadeStyle(i) != Cascades.CascadeStyle.StyleNone)
						{
							return Cascades.CascadeStyle.StyleAll;
						}
					}

					return Cascades.CascadeStyle.StyleNone;
				}
				else
				{
					if (cascade.Equals("all"))
					{
						return Cascades.CascadeStyle.StyleAll;
					}
					else if (cascade.Equals("all-delete-orphan"))
					{
						return Cascades.CascadeStyle.StyleAllDeleteOrphan;
					}
					else if (cascade.Equals("none"))
					{
						return Cascades.CascadeStyle.StyleNone;
					}
					else if (cascade.Equals("save-update"))
					{
						return Cascades.CascadeStyle.StyleSaveUpdate;
					}
					else if (cascade.Equals("delete"))
					{
						return Cascades.CascadeStyle.StyleOnlyDelete;
					}
					else if (cascade.Equals("delete-orphan"))
					{
						return Cascades.CascadeStyle.StyleDeleteOrphan;
					}
					else
					{
						throw new MappingException("Unspported cascade style: " + cascade);
					}
				}
			}
		}

		public string Cascade
		{
			get { return cascade; }
			set { cascade = value; }
		}

		public bool IsUpdateable
		{
			get
			{
				bool[] columnUpdateability = propertyValue.ColumnUpdateability;
				return updateable &&
				       (
				       	// columnUpdateability.Length == 0 ||
				       !ArrayHelper.IsAllFalse(columnUpdateability)
				       );
			}
			set { updateable = value; }
		}

		public bool IsInsertable
		{
			get
			{
				bool[] columnInsertability = propertyValue.ColumnInsertability;
				return insertable &&
				       (
				       	columnInsertability.Length == 0 ||
				       	!ArrayHelper.IsAllFalse(columnInsertability)
				       );
			}
			set { insertable = value; }
		}

		/// <summary></summary>
		public bool IsNullable
		{
			get { return propertyValue == null || propertyValue.IsNullable; }
		}

		public bool IsOptional
		{
			get { return IsNullable; }
		}

		public string PropertyAccessorName
		{
			get { return propertyAccessorName; }
			set { propertyAccessorName = value; }
		}

		public IGetter GetGetter(System.Type clazz)
		{
			return PropertyAccessor.GetGetter(clazz, name);
		}

		public ISetter GetSetter(System.Type clazz)
		{
			return PropertyAccessor.GetSetter(clazz, name);
		}

		protected IPropertyAccessor PropertyAccessor
		{
			get { return PropertyAccessorFactory.GetPropertyAccessor(PropertyAccessorName); }
		}

		public bool IsBasicPropertyAccessor
		{
			get { return propertyAccessorName == null || propertyAccessorName.Equals("property"); }
		}

		public IDictionary MetaAttributes
		{
			get { return metaAttributes; }
			set { metaAttributes = value; }
		}

		public MetaAttribute GetMetaAttribute(string name)
		{
			return (MetaAttribute) metaAttributes[name];
		}

		public bool IsValid(IMapping mapping)
		{
			return Value.IsValid(mapping);
		}

		public string NullValue
		{
			get
			{
				if (propertyValue is SimpleValue)
				{
					return ((SimpleValue) propertyValue).NullValue;
				}
				else
					return null;
			}
		}

		public PersistentClass PersistentClass
		{
			get { return persistentClass; }
			set { persistentClass = value; }
		}

		public bool IsOptimisticLocked
		{
			get { return isOptimisticLocked; }
			set { isOptimisticLocked = value; }
		}

		public PropertyGeneration Generation
		{
			get { return generation; }
			set { generation = value; }
		}
	}
}