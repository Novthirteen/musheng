//------------------------------------------------------------------------------
// Copyright (c) 2004 Whidsoft Corporation. All Rights Reserved.
//------------------------------------------------------------------------------

namespace Whidsoft.WebControls
{
    using System;
	using System.Collections ;
    using System.ComponentModel;
    using System.Drawing.Design;

    /// <summary>
    /// Collection of OrgNodes within a TreeView.
    /// </summary>
    //[Editor(typeof(Microsoft.Web.UI.WebControls.Design.OrgNodeCollectionEditor), typeof(UITypeEditor))]
    public class OrgNodeCollection : CollectionBase
    {
        private IOrgNode _Parent;

        /// <summary>
        /// Initializes a new instance of a OrgNodeCollection.
        /// </summary>
        /// <param name="parent">The parent OrgNode of this collection.</param>
        public OrgNodeCollection( IOrgNode parent) : base()
        {
            _Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of a OrgNodeCollection.
        /// </summary>
        public OrgNodeCollection() : base ()
        {
            _Parent = null;
        }

        /// <summary>
        /// The parent object of this collection.
        /// </summary>
        public IOrgNode Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

		/*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">The index of the item being inserted.</param>
        /// <param name="value">The item being inserted.</param>
        protected override void OnInsert(int index, object value)
        {
            if (((OrgNode)value)._Parent != null)
            {
                throw new Exception( Util.GetStringResource("OrgNodeAlreadyInCollection"));
            }

            SetItemProperties((OrgNode)value);

            if (!Reloading)
            {
                TreeView tv;
                if (Parent is OrgNode)
                    tv = ((OrgNode)Parent).ParentTreeView;
                else
                    tv = (TreeView)Parent;

                if ((tv != null) && tv.IsInitialized)
                {
                    _tnSelected = tv.GetNodeFromIndex(tv.SelectedNodeIndex);
                }
            }

            base.OnInsert(index, value);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">The index of the item being inserted.</param>
        /// <param name="value">The item being inserted.</param>
        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);

            if (!Reloading)
            {
                TreeView tv;
                if (Parent is OrgNode)
                    tv = ((OrgNode)Parent).ParentTreeView;
                else
                    tv = (TreeView)Parent;

                if ((tv != null) && tv.IsInitialized)
                {
                    if (_tnSelected != null)
                        tv.SelectedNodeIndex = _tnSelected.GetNodeIndex();
                    else
                        tv.SelectedNodeIndex = "0";
                }
            }
        }
		*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">The index of the item being changed.</param>
        /// <param name="oldValue">The old item.</param>
        /// <param name="newValue">The new item.</param>
        protected override void OnSet(int index, object oldValue, object newValue)
        {
            SetItemProperties((OrgNode)newValue);

            base.OnSet(index, oldValue, newValue);
        }

        /// <summary>
        /// Sets properties of the OrgNode before being added.
        /// </summary>
        /// <param name="item">The OrgNode to be set.</param>
        private void SetItemProperties( IOrgNode item)
        {
            item.Parent = Parent;
        }

        /// <summary>
        /// Adds a OrgNode to the collection.
        /// </summary>
        /// <param name="item">The OrgNode to add.</param>
        public void Add(OrgNode item)
        {
            List.Add(item);
        }

        /// <summary>
        /// Adds a OrgNode to the collection at a specific index.
        /// </summary>
        /// <param name="index">The index at which to add the item.</param>
        /// <param name="item">The OrgNode to add.</param>
        public void AddAt(int index, OrgNode item)
        {
            List.Insert(index, item);
        }

        /// <summary>
        /// Determines if a OrgNode is in the collection.
        /// </summary>
        /// <param name="item">The OrgNode to search for.</param>
        /// <returns>true if the OrgNode exists within the collection. false otherwise.</returns>
        public bool Contains(OrgNode item)
        {
            return List.Contains(item);
        }

        /// <summary>
        /// Determines zero-based index of a OrgNode within the collection.
        /// </summary>
        /// <param name="item">The OrgNode to locate within the collection.</param>
        /// <returns>The zero-based index.</returns>
        public int IndexOf(OrgNode item)
        {
            return List.IndexOf(item);
        }

        /// <summary>
        /// Removes a OrgNode from the collection.
        /// </summary>
        /// <param name="item">The OrgNode to remove.</param>
        public void Remove(OrgNode item)
        {           
            List.Remove(item);
        }

		/*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">The index of the item being removed.</param>
        /// <param name="value">The item being removed.</param>
        protected override void OnRemove(int index, object value)
        {
            if (!Reloading)
            {
                OrgNode node = (OrgNode)value;
                TreeView tv = node.ParentTreeView;
                if (tv != null)
                {
                    if (tv.SelectedNodeIndex.IndexOf(node.GetNodeIndex()) == 0)
                    {
                        // The node being removed is the selected node or one of its parents
                        OrgNode newNode = null;
                        if (Count > 1)
                        {
                            // Set the new selected node index to the next node
                            // or the previous one if the node is the last node
                            if (index == (Count - 1))
                            {
                                newNode = this[index - 1];
                            }
                            else
                            {
                                newNode = this[index + 1];
                            }
                        }
                        else if ((Parent != null) && (Parent is OrgNode))
                        {
                            // There are no other nodes in this collection, so
                            // try setting to its parent
                            newNode = (OrgNode)Parent;
                        }

                        _tnSelected = newNode;
                    }
                    else
                    {
                        // The selected node does not need to change, but its
                        // index may be affected by this removal.
                        _tnSelected = tv.GetNodeFromIndex(tv.SelectedNodeIndex);
                    }
                }
            }

            base.OnRemove(index, value);
        }
		*/


		/*

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">The index of the item being removed.</param>
        /// <param name="value">The item being removed.</param>
        protected override void OnRemoveComplete(int index, object value)
        {          
            base.OnRemoveComplete(index, value);

            OrgNode node = (OrgNode)value;
            node.ParentTreeView = null;
            node._Parent = null;

            TreeView tv = (Parent is OrgNode) ? ((OrgNode)Parent).ParentTreeView : (TreeView)Parent;
            if (!Reloading && (tv != null))
            {
                if (_tnSelected != null)
                {
                    tv.SelectedNodeIndex = _tnSelected.GetNodeIndex();
                }
                else
                {
                    tv.SelectedNodeIndex = null;
                }

                if (tv.HoverNodeIndex != null && tv.GetNodeFromIndex(tv.HoverNodeIndex) == null)
                {
                    tv.HoverNodeIndex = "";
                }
            }
        }

        /// <summary>
        /// Adjusts the SelectedNodeIndex of the TreeView when a clear is performed.
        /// </summary>
        protected override void OnClear()
        {
            if (!Reloading && (Parent != null))
            {
                if (Parent is TreeView)
                {
                    ((TreeView)Parent).SelectedNodeIndex = null;
                }
                else
                {
                    TreeView tv = ((OrgNode)Parent).ParentTreeView;
                    if (tv != null)
                    {
                        string parentIndex = ((OrgNode)Parent).GetNodeIndex();
                        if (tv.SelectedNodeIndex.IndexOf(parentIndex) == 0)
                        {
                            tv.SelectedNodeIndex = parentIndex;
                        }
                    }
                }
            }

            base.OnClear();
        }
		*/

        /// <summary>
        /// Indexer into the collection.
        /// </summary>
        public OrgNode this[int index]
        {
            get { return (OrgNode)List[index]; }
        }
    }
}
