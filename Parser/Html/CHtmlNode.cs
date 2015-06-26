/*****************************************************************************\
>
>
>	 CHtmlNode Class
>
>
>
>
\*****************************************************************************/

// CHtmlNode.cs: implementation of the CHtmlNode class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;

namespace Cloud9.Parser.Html
{
	/// <summary>
	/// ��� ����� �θ� Ŭ����.
	/// </summary>
    public abstract class CHtmlNode : Cloud9.Parser.Html.Base.IDiagnosisable, ICloneable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region �⺻	

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 
		/// </summary>
		protected CHtmlNode()
		{
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        protected CHtmlNode(CHtmlNode obj)
        {
            System.Diagnostics.Debug.Assert(obj != null);

            obj.AssertValid();

            m_parent = obj.m_parent;
            m_nodeID = obj.m_nodeID;
            m_bindObject = obj.m_bindObject;
        }

        /////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

		/////////////////////////////////////////////////////////////////////////////        
		/// <summary>
		/// 
		/// </summary>
		public abstract void AssertValid();

		/////////////////////////////////////////////////////////////////////////////       
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="buffer">  </param>
		/// <param name="prefix"></param>
		public abstract void Dump(StringBuilder buffer, string prefix);

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// html ��ȯ
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.HTML;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m_nodeID;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitor"></param>
        public abstract void Accept(Cloud9.Parser.Html.Base.IBaseVisitor visitor);

    #endregion

    /////////////////////////////////////////////////////////////////////////////////
    #region  

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// �ڽĳ�带 ������ ��ü html ��ȯ.
		/// </summary>
        public string HTML 
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                TransformHTML(stringBuilder, 0);

                return stringBuilder.ToString();
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �ش����� html�±׷� ��ȯ�ϴ� �����Լ�
        /// </summary>
        internal abstract void TransformHTML(StringBuilder writer, int indentDepth);

    #endregion

    /////////////////////////////////////////////////////////////////////////////////
    #region 

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public int NodeID
        {
            get
            {
                return m_nodeID;
            }
            set
            {
                m_nodeID = value;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public abstract string NodeName
        {
            get;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public Object BindObject
        {
            get
            {
                return m_bindObject;
            }
            set
            {
                m_bindObject = value;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ���� ��尡 ���� �θ� ������Ʈ.
        /// </summary>
        public CHtmlElement Parent
        {
            get
            {
                return m_parent;
            }
            internal set
            {
                m_parent = value;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �������� �������.
        /// </summary>
        public CHtmlNode PreviousSibling
        {
            get
            {
                CHtmlNode previousSibling = null;
                if(m_parent != null)
                {
                    int index = m_parent.Nodes.IndexOf(this);
                    System.Diagnostics.Debug.Assert(index != -1);                    
                    if(index - 1 >= 0)
                        previousSibling = m_parent.Nodes[index - 1];
                }

                return previousSibling;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �������� ���� ���.
        /// </summary>
        public CHtmlNode NextSibling
        {
            get
            {
                CHtmlNode nextSibling = null;
                if(m_parent != null)
                {
                    int index = m_parent.Nodes.IndexOf(this);
                    System.Diagnostics.Debug.Assert(index != -1); 
                    if(index + 1 < m_parent.Nodes.Count)
                        nextSibling = m_parent.Nodes[index + 1];
                }

                return nextSibling;
            }
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// �ֻ��� ��忩��.
		/// </summary>
		public bool IsRoot
		{
			get
			{
				return m_parent == null;
			}
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public abstract bool IsLeaf { get; }
       
		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// node�� �θ𿩺�
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool IsDescendentOf(CHtmlNode node)
		{
            System.Diagnostics.Debug.Assert(node != null);

            bool reault = false;

			CHtmlNode parent = m_parent;
			while(parent != null)
			{
                if (parent == node)
                {
                    reault = true;
                    break;
                }

				parent = parent.m_parent;
			}

            return reault;
		}

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// node�� �ڽĿ���.
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool IsAncestorOf(CHtmlNode node)
		{
            System.Diagnostics.Debug.Assert(node != null);
			return node.IsDescendentOf(this);
		}

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public CHtmlNode GetCommonAncestor(CHtmlNode node)
		{
            System.Diagnostics.Debug.Assert(node != null);

            CHtmlNode commonAncestor = null;

			CHtmlNode thisParent = this;
            while(commonAncestor != null && thisParent != null)
			{
				CHtmlNode thatParent = node;
                while(commonAncestor != null && thatParent != null)
				{
                    if(thisParent == thatParent)
                      commonAncestor = thisParent;
                   
					thatParent = thatParent.Parent;
				}
				thisParent = thisParent.Parent;
			}

            return commonAncestor;
		}

    #endregion

    /////////////////////////////////////////////////////////////////////////////////
    #region �������
		
		/// <summary>
		/// 
		/// </summary>
        protected CHtmlElement m_parent = null;
        /// <summary>
        /// 
        /// </summary>
        protected int m_nodeID = 0;
        /// <summary>
        /// 
        /// </summary>
        protected Object m_bindObject = null;

    #endregion

	}
}
