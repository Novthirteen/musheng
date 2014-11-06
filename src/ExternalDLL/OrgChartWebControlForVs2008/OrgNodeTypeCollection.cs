//------------------------------------------------------------------------------
// Copyright (c) 2004 Whidsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Whidsoft.WebControls
{
    using System;
	using System.Collections;


    /// <summary>
    /// Collection of TreeNodes within a TreeView.
    /// </summary>
    public class OrgNodeTypeCollection : CollectionBase
    {
        private Object _Parent;

        /// <summary>
        /// Initializes a new instance of a TreeNodeCollection.
        /// </summary>
        public OrgNodeTypeCollection() : base()
        {
            _Parent = null;
        }

        /// <summary>
        /// Initializes a new instance of a TreeNodeCollection.
        /// </summary>
        /// <param name="parent">The parent OrgNodeType of this collection.</param>
        public OrgNodeTypeCollection(Object parent) : base()
        {
            _Parent = parent;
        }

        /// <summary>
        /// The parent object of this collection.
        /// </summary>
        public Object Parent
        {
            get { return _Parent; }
            set
            {
                _Parent = value;

                if (value != null)
                {
                    foreach (OrgNodeType item in this)
                    {
                        SetItemProperties(item);
                    }
                }
            }
        }

        private void SetItemProperties(OrgNodeType item)
        {
            item._Parent = Parent;
        }

        /// <summary>
        /// Sets item properties on being inserted.
        /// </summary>
        /// <param name="index">The index of the item being inserted.</param>
        /// <param name="value">The item being inserted.</param>
        protected override void OnInsert(int index, object value)
        {
            SetItemProperties((OrgNodeType)value);

            base.OnInsert(index, value);
        }

        /// <summary>
        /// Sets item properties on being set.
        /// </summary>
        /// <param name="index">The index of the item being changed.</param>
        /// <param name="oldValue">The old item.</param>
        /// <param name="newValue">The new item.</param>
        protected override void OnSet(int index, object oldValue, object newValue)
        {
            SetItemProperties((OrgNodeType)newValue);

            base.OnSet(index, oldValue, newValue);
        }

        /// <summary>
        /// Adds a OrgNodeType to the collection.
        /// </summary>
        /// <param name="item">The OrgNodeType to add.</param>
        public void Add(OrgNodeType item)
        {
            List.Add(item);
        }

        /// <summary>
        /// Adds a OrgNodeType to the collection at a specific index.
        /// </summary>
        /// <param name="index">The index at which to add the item.</param>
        /// <param name="item">The OrgNodeType to add.</param>
        public void AddAt(int index, OrgNodeType item)
        {
            List.Insert(index, item);
        }

        /// <summary>
        /// Determines if a OrgNodeType is in the collection.
        /// </summary>
        /// <param name="item">The OrgNodeType to search for.</param>
        /// <returns>true if the OrgNodeType exists within the collection. false otherwise.</returns>
        public bool Contains(OrgNodeType item)
        {
            return List.Contains(item);
        }

        /// <summary>
        /// Determines zero-based index of a OrgNodeType within the collection.
        /// </summary>
        /// <param name="item">The OrgNodeType to locate within the collection.</param>
        /// <returns>The zero-based index.</returns>
        public int IndexOf(OrgNodeType item)
        {
            return List.IndexOf(item);
        }

        /// <summary>
        /// Removes a OrgNodeType from the collection.
        /// </summary>
        /// <param name="item">The OrgNodeType to remove.</param>
        public void Remove(OrgNodeType item)
        {
            List.Remove(item);
        }

        /// <summary>
        /// Indexer into the collection.
        /// </summary>
        public OrgNodeType this[int index]
        {
            get { return (OrgNodeType)List[index]; }
        }
    }
}
