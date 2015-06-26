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
	/// 모든 노드의 부모 클래스.
	/// </summary>
    public abstract class CHtmlNode : Cloud9.Parser.Html.Base.IDiagnosisable, ICloneable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region 기본	

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
		/// 생성자
		/// </summary>
		/// <param name="buffer">  </param>
		/// <param name="prefix"></param>
		public abstract void Dump(StringBuilder buffer, string prefix);

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// html 반환
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
		/// 자식노드를 포함한 전체 html 반환.
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
        /// 해당노드의 html태그로 변환하는 가상함수
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
        /// 현재 노드가 속한 부모 엘리먼트.
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
        /// 현재노드의 이전노드.
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
        /// 현재노드의 다음 노드.
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
		/// 최상위 노드여부.
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
		/// node의 부모여부
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
		/// node의 자식여부.
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
    #region 멤버변수
		
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
