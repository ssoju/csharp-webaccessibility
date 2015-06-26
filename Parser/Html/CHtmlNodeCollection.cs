/*****************************************************************************\
>
>
>	 CHtmlNodeCollection Class
>
>
>
>
\*****************************************************************************/

// CHtmlNodeCollection.cs: implementation of the CHtmlNodeCollection class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cloud9.Parser.Html;

namespace Cloud9.Parser.Html
{    

	/// <summary>
	/// This object represents a collection of CHtmlNodes. The order in which the
    /// nodes occur directly corresponds to the order in which they appear in the
    /// original HTML document.
	/// </summary>
    public sealed class CHtmlNodeCollection : Cloud9.Parser.Html.Base.IDiagnosisable, IEnumerable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region ±âº»	

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Public constructor to create an empty collection.
		/// </summary> 
		public CHtmlNodeCollection() : base()
		{
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        public CHtmlNodeCollection(int capacity)
        {
            m_nodeList.Capacity = capacity;
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// A collection is usually associated with a parent node (an CHtmlElement, actually)
		/// but you can pass null to implement an abstracted collection.
		/// </summary>
		/// <param name="element"></param>
		internal CHtmlNodeCollection(CHtmlElement element) : base()
		{
            m_ownerNode = element;
		}

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 
		/// </summary>
		public void AssertValid()
		{          	
           for(int index = 0; index < m_nodeList.Count; ++index) // ©Ò¦³ª«¥ó´M³X?¦¸©I¥s Dump
               m_nodeList[index].AssertValid();
		}

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// »ý¼ºÀÚ
		/// </summary>
		public void Dump(StringBuilder buffer, string prefix)
		{
			AssertValid();
		}

    #endregion

    /////////////////////////////////////////////////////////////////////////////////
	#region 

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return m_nodeList.GetEnumerator();
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public int Capacity
        {
            get
            {
                return m_nodeList.Capacity;
            }
            set
            {
                m_nodeList.Capacity = value;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return m_nodeList.Count;
            }
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// This will add a node to the collection.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public void Add(CHtmlNode node)
		{
            System.Diagnostics.Debug.Assert(node != null);

            // set node.Parent
            if(m_ownerNode != null) 
            {
                if(node.Parent != null)
                {
                    int index = node.Parent.Nodes.IndexOf(node);
                    node.Parent.Nodes.m_nodeList.RemoveAt(index);
                }

                node.Parent = m_ownerNode;
            }

            m_nodeList.Add(node); 
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This will insert a node at the given position
        /// </summary>
        /// <param name="index">The position at which to insert the node.</param>
        /// <param name="node">The node to insert.</param>
        public void Insert(int index, CHtmlNode node)
        {
            System.Diagnostics.Debug.Assert(index >= 0 && index <= m_nodeList.Count);
            System.Diagnostics.Debug.Assert(node != null);

            // set node.Parent
            if(m_ownerNode != null)
            {
                if(node.Parent != null)
                    node.Parent.Nodes.RemoveAt(node.Parent.Nodes.IndexOf(node));

                node.Parent = m_ownerNode;
            }

            m_nodeList.Insert(index, node);
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            // set node.Parent
            if(m_ownerNode != null)
            {
                for(int index = 0, count = m_nodeList.Count; index < count; ++index)
                {
                    CHtmlNode node = m_nodeList[index];
                    System.Diagnostics.Debug.Assert(m_ownerNode == node.Parent);
                    node.Parent = null;
                }
            }

            m_nodeList.Clear();            
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            System.Diagnostics.Debug.Assert(index >= 0 && index < m_nodeList.Count);

            CHtmlNode node = m_nodeList[index];

            // set node.Parent
            if(m_ownerNode != null)
            {
                System.Diagnostics.Debug.Assert(m_ownerNode == node.Parent);
                node.Parent = null;
            }

            m_nodeList.RemoveAt(index);
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public void RemoveRange(int index, int count)
        {
            System.Diagnostics.Debug.Assert(index >= 0 && index < m_nodeList.Count);
            System.Diagnostics.Debug.Assert(count > 0);
            System.Diagnostics.Debug.Assert(index + count <= m_nodeList.Count);

            for(int scanIndex = index; scanIndex < index + count; ++scanIndex)
            {
                CHtmlNode node = m_nodeList[scanIndex];

                // set node.Parent
                if(m_ownerNode != null)
                {
                    System.Diagnostics.Debug.Assert(m_ownerNode == node.Parent);
                    node.Parent = null;
                }
            }

            m_nodeList.RemoveRange(index, count);
        }

    #endregion
	
    /////////////////////////////////////////////////////////////////////////////////
	#region 

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This property allows you to change the node at a particular position in the
        /// collection.
        /// </summary>
        public CHtmlNode this[int index]
        {
            get
            {
                System.Diagnostics.Debug.Assert(index >= 0 && index < m_nodeList.Count);
                return m_nodeList[index];
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This allows you to directly access the first element in this colleciton with the given name.
        /// If the node does not exist, this will return null.
        /// </summary>
        public CHtmlNode this[string name]
        {
            get
            {
                System.Diagnostics.Debug.Assert(name != null);

                name = name.Trim().ToLower();

                CHtmlNode result = null;

                for(int index = 0, count = m_nodeList.Count; index < count; ++index)
                {
                    CHtmlNode node = m_nodeList[index];
                    if(node.NodeName.Equals(name))
                    {
                        result = node;
                        break;
                    }
                }

                return result;
            }
        }
        
        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This will return the index of the element with the specified name. If it is
        /// not found, this method will return -1.
        /// </summary>
        /// <param name="name">The name of the element to find.</param>
        /// <returns>The zero-based index, or -1.</returns>
        public int IndexOf(string name)
        {
            System.Diagnostics.Debug.Assert(name != null);

            name = name.Trim().ToLower();

            int result = -1;
            for(int index = 0; index < m_nodeList.Count; ++index)
                if(this[index].NodeName.Equals(name))
                    result = index;

            return result;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This is used to identify the index of this node as it appears in the collection.
        /// </summary>
        /// <param name="node">The node to test</param>
        /// <returns>The index of the node, or -1 if it is not in this collection</returns>
        public int IndexOf(CHtmlNode node)
        {
            System.Diagnostics.Debug.Assert(node != null);

            return m_nodeList.IndexOf(node);
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CHtmlNodeCollection GetDescendent()
        {
            CHtmlNodeCollection result = new CHtmlNodeCollection(64);
            GetDescendent(result);

            return result;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public void GetDescendent(CHtmlNodeCollection result)
        {
            System.Diagnostics.Debug.Assert(result != null);

            for(int index = 0, count = m_nodeList.Count; index < count; ++index)
            {
                CHtmlNode node = m_nodeList[index];
                result.Add(node);
                if(node is CHtmlElement)
                    ((CHtmlElement)node).Nodes.GetDescendent(result);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="searchChildren"></param>
        /// <returns></returns>
        public CHtmlNode FindByNameFirstNode(string name, bool searchChildren)
        {
            System.Diagnostics.Debug.Assert(name != null);

            CHtmlNode result = null;
            name = name.Trim().ToLower();

            for(int index = 0, count = m_nodeList.Count; index < count; ++index)
            {
                CHtmlNode node = m_nodeList[index];
                if(node.NodeName.Equals(name))
                {
                    result = node;
                    if(result != null) break;
                }

                if(searchChildren == true && node is CHtmlElement)
                {
                    result = ((CHtmlElement)node).Nodes.FindByNameFirstNode(name, searchChildren);
                    if(result != null) break;
                }
            }

            return result;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public CHtmlNode FindByNameFirstNode(string name, int depth)
        {
            System.Diagnostics.Debug.Assert(depth >= 0);

            CHtmlNode result = null;

            if(depth > 0)
            {
                name = name.Trim().ToLower();

                for(int index = 0, count = m_nodeList.Count; index < count; ++index)
                {
                    CHtmlNode node = m_nodeList[index];
                    if(node.NodeName.Equals(name))
                    {
                        result = node;
                        if(result != null) break;
                    }

                    if(depth >  1 && node is CHtmlElement)
                    {
                        result = ((CHtmlElement)node).Nodes.FindByNameFirstNode(name, depth - 1);
                        if(result != null) break;
                    }
                }
            }

            return result;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This will search though this collection of nodes for all elements with the
        /// specified name. If you want to search the subnodes recursively, you should
        /// pass True as the parameter in searchChildren. This search is guaranteed to
        /// return nodes in the order in which they are found in the document.
        /// </summary>
        /// <param name="name">The name of the element to find</param>
        /// <returns>A collection of all the nodes that macth.</returns>
        public CHtmlNodeCollection FindByName(string name)
        {
            System.Diagnostics.Debug.Assert(name != null);

            name = name.Trim().ToLower();

            return FindByName(name, false);
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// This will search though this collection of nodes for all elements with the
		/// specified name. If you want to search the subnodes recursively, you should
		/// pass True as the parameter in searchChildren. This search is guaranteed to
		/// return nodes in the order in which they are found in the document.
		/// </summary>
		/// <param name="name">The name of the element to find</param>
		/// <param name="searchChildren">True if you want to search sub-nodes, False to
		/// only search this collection.</param>
		/// <returns>A collection of all the nodes that macth.</returns>
		public CHtmlNodeCollection FindByName(string name, bool searchChildren)
		{
            System.Diagnostics.Debug.Assert(name != null);

            name = name.Trim().ToLower();

			CHtmlNodeCollection result = new CHtmlNodeCollection(64);
            FindByName(result, name, searchChildren);

			return result;
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="name"></param>
        /// <param name="searchChildren"></param>
        public void FindByName(CHtmlNodeCollection result, string name, bool searchChildren)
        {
            System.Diagnostics.Debug.Assert(result != null);
            System.Diagnostics.Debug.Assert(name != null);

            name = name.Trim().ToLower();

            for(int index = 0, count = m_nodeList.Count; index < count; ++index)
            {
                CHtmlNode node = m_nodeList[index];
                if(node.NodeName.Equals(name))
                    result.Add(node);

                if(searchChildren == true && node is CHtmlElement)
                    ((CHtmlElement)node).Nodes.FindByName(result, name, searchChildren);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="name"></param>
        /// <param name="searchChildren"></param>
        public void FindByName(CHtmlNodeCollection result, string name, int depth)
        {
            System.Diagnostics.Debug.Assert(result != null);
            System.Diagnostics.Debug.Assert(name != null);
            System.Diagnostics.Debug.Assert(depth >= 0);

            if(depth > 0)
            {
                name = name.Trim().ToLower();

                for(int index = 0, count = m_nodeList.Count; index < count; ++index)
                {
                    CHtmlNode node = m_nodeList[index];
                    if(node.NodeName.Equals(name))
                        result.Add(node);

                    if(depth > 1 && node is CHtmlElement)
                        ((CHtmlElement)node).Nodes.FindByName(result, name, depth - 1);
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="name"></param>
        /// <param name="attributeName"></param>
        /// <param name="searchChildren"></param>
        public void FindByNameAttribute(CHtmlNodeCollection result, string name, string attributeName, bool searchChildren)
        {
            System.Diagnostics.Debug.Assert(result != null);
            System.Diagnostics.Debug.Assert(attributeName != null);

            attributeName = attributeName.Trim().ToLower();

            for(int index = 0, count = m_nodeList.Count; index < count; ++index)
            {
                CHtmlNode node = m_nodeList[index];

                if(node.NodeName.Equals(name) &&
                   node is IHtmlNodeHasAttribute && ((IHtmlNodeHasAttribute)node).Attributes.HasAttribute(attributeName))
                    result.Add(node);

                if(searchChildren && node is CHtmlElement)
                    ((CHtmlElement)node).Nodes.FindByNameAttribute(result, name, attributeName, searchChildren);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="name"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <param name="searchChildren"></param>
        public void FindByNameAttributeValue(CHtmlNodeCollection result, string name, string attributeName, string attributeValue, bool searchChildren)
        {
            System.Diagnostics.Debug.Assert(result != null);
            System.Diagnostics.Debug.Assert(attributeName != null);

            attributeName = attributeName.Trim().ToLower();

            for(int index = 0, count = m_nodeList.Count; index < count; ++index)
            {
                CHtmlNode node = m_nodeList[index];

                if(node.NodeName.Equals(name) && node is IHtmlNodeHasAttribute)
                {
                    CHtmlAttribute attribute = ((IHtmlNodeHasAttribute)node).Attributes[attributeName];
                    if(attribute != null && attribute.Value == attributeValue)
                        result.Add(node);
                }

                if(searchChildren && node is CHtmlElement)
                    ((CHtmlElement)node).Nodes.FindByNameAttributeValue(result, name, attributeName, attributeValue, searchChildren);
            }
        }

    #endregion

	/////////////////////////////////////////////////////////////////////////////////
	#region ¸â¹öº¯¼ö
        
		/// <summary>
		/// 
		/// </summary>
        private CHtmlElement m_ownerNode = null;
        /// <summary>
        /// 
        /// </summary>
        private List<CHtmlNode> m_nodeList = new List<CHtmlNode>();	

    #endregion

	}
}
