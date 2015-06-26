/*****************************************************************************\
>
>
>	 CHtmlElement Class
>
>
>
>
\*****************************************************************************/

// CHtmlElement.cs: implementation of the CHtmlElement class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;

namespace Cloud9.Parser.Html
{
	/// <summary>
	/// The CHtmlElement object represents any HTML element. An element has a name
	/// and zero or more attributes.
	/// </summary>
    public sealed class CHtmlElement : CHtmlNode, IHtmlNodeHasAttribute, Cloud9.Parser.Html.Base.IDiagnosisable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region 

        public enum EndTagType
        {
            HasChild,
            ExplicitlyTerminated,
            Terminated,
            NonTerminated,
        }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////
	#region 기본

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This constructs a new HTML element with the specified tag name.
        /// </summary>
        public CHtmlElement(string name)
        {
            System.Diagnostics.Debug.Assert(name != null && CHtmlUtil.ExistWhiteSpaceChar(name) == false);

            m_nodes = new CHtmlNodeCollection(this);
            m_name = name.Trim().ToLower();
        }

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 생성자
        /// </summary>
        public CHtmlElement(CHtmlElement obj) : base(obj)
        {
            System.Diagnostics.Debug.Assert(obj != null);

            obj.AssertValid();

            m_name = obj.m_name;
            m_nodes = new CHtmlNodeCollection(this);

            int count = obj.m_nodes.Count;
            m_nodes.Capacity = count;
            for(int index = 0; index < count; ++index)
                m_nodes.Add((CHtmlNode)obj.m_nodes[index].Clone());

            count = obj.m_attributes.Count;
            m_attributes.Capacity = count;
            for(int index = 0; index < count; ++index)
                m_attributes.Add((CHtmlAttribute)obj.m_attributes[index].Clone());
        }

        /////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new CHtmlElement(this);
        }

		/////////////////////////////////////////////////////////////////////////////        
		/// <summary>
		/// 
		/// </summary>
		public override void AssertValid()
		{
            //System.Diagnostics.Debug.Assert(m_previousWithSameNameNode == null);
            //System.Diagnostics.Debug.Assert(m_close == true);

			m_nodes.AssertValid();
			m_attributes.AssertValid();

            if(m_bindObject is Cloud9.Parser.Html.Base.IDiagnosisable)
                ((Cloud9.Parser.Html.Base.IDiagnosisable)m_bindObject).AssertValid();
		}

		/////////////////////////////////////////////////////////////////////////////       
		/// <summary>
		/// 생성자
		/// </summary>
		public override void Dump(StringBuilder buffer, string prefix)
		{
			AssertValid();
			string old = prefix;
			buffer.Append(old + "쥂Object " + GetType().Name + " Dump : \n");

            prefix += " ";
            buffer.Append(prefix + "Node ID: " + this.NodeID + " Parse Close: " + m_close + "\n");	
            buffer.Append(prefix + "HTML Tag: " + this.Tag + "\n");

            if(m_bindObject is Cloud9.Parser.Html.Base.IDiagnosisable)
            {
                buffer.Append(prefix + "Bind Object:\n");
                buffer.Append(prefix + " n");
                ((Cloud9.Parser.Html.Base.IDiagnosisable)m_bindObject).Dump(buffer, prefix);
                buffer.Append(prefix + " n");
            }

			if(m_nodes.Count != 0)
			{
                buffer.Append(prefix + "CHtmlNode number: " + m_nodes.Count + "\n");
                buffer.Append(prefix + "Child Object deep dump in the following:\n");

                for(int index = 0, count = m_nodes.Count; index < count; ++index) // ⒡┳かτ퀾쿦?┯쯍쩾 Dump
                {
					buffer.Append(prefix + " n");
					m_nodes[index].Dump(buffer, prefix);
				}               
			}
            else if(this.TerminatedType == EndTagType.ExplicitlyTerminated) buffer.Append(prefix + "Element is explicitly terminated\n");
            else if(this.TerminatedType == EndTagType.Terminated) buffer.Append(prefix + "Element is terminated\n");
            else if(this.TerminatedType == EndTagType.NonTerminated) buffer.Append(prefix + "Element is non-terminated\n");
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(Cloud9.Parser.Html.Base.IBaseVisitor visitor)
        {
            Cloud9.Parser.Html.Base.IVisitor<CHtmlElement> elementVisitor = visitor as Cloud9.Parser.Html.Base.IVisitor<CHtmlElement>;
            if(elementVisitor != null) elementVisitor.Visit(this);
        }

    #endregion

	/////////////////////////////////////////////////////////////////////////////////
	#region 

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public override string NodeName
        {
            get
            {
                return m_name;
            }
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 태그명 반환.
		/// </summary>
		public string Name
		{
			get
			{
				return m_name;
			}
            set
            {
                System.Diagnostics.Debug.Assert(value != null);
                System.Diagnostics.Debug.Assert(CHtmlUtil.ExistWhiteSpaceChar(value) == false);

                m_name = value.Trim().ToLower();
            }
		}

        /////////////////////////////////////////////////////////////////////////////////
        public EndTagType TerminatedType
        {
            get
            {
                if(m_nodes.Count > 0)
                    m_terminatedType = EndTagType.HasChild;
                return m_terminatedType;
            }
            set
            {
                if(m_nodes.Count > 0)
                    m_terminatedType = EndTagType.HasChild;
                else m_terminatedType = value;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public override bool IsLeaf
        {
            get
            {
                return (m_nodes.Count == 0);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 자식노드 반환
        /// </summary>
        public CHtmlNodeCollection Nodes
        {
            get
            {
                return m_nodes;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 해당 앨리먼트의 속성리스트 반환
        /// </summary>
        public CHtmlAttributeCollection Attributes
        {
            get
            {
                return m_attributes;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 엘리먼트의 태그 반환.
        /// </summary>
        /// <returns></returns>		
        public string Tag
        {
            get
            {
                string temp = "<" + m_name;

                for(int index = 0, count = m_attributes.Count; index < count; ++index)
                    temp += " " + m_attributes[index].HTML;

                temp += ">";
                return temp;
            }
        }

		/////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
		public string InnerText
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();

                for(int index = 0, count = m_nodes.Count; index < count; ++index)
                {
                    CHtmlText text = m_nodes[index] as CHtmlText;
                    if(text != null)
                        stringBuilder.Append(text.Text);
                }

				return stringBuilder.ToString();
			}
		}

    #endregion

	/////////////////////////////////////////////////////////////////////////////////
	#region  

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This will return the full HTML to represent this node (and all child nodes)
        /// </summary>
        internal override void TransformHTML(StringBuilder writer, int indentDepth)
        {
            System.Diagnostics.Debug.Assert(writer != null);
            writer.Append("<" + m_name);

            for(int index = 0, count = m_attributes.Count; index < count; ++index)
            {
                writer.Append(" ");
                m_attributes[index].TransformHTML(writer);
            }

            if(m_nodes.Count > 0)
            {
                writer.Append(">");
                for(int index = 0, count = m_nodes.Count; index < count; ++index)
                    m_nodes[index].TransformHTML(writer, indentDepth + 1);
                writer.Append("</" + m_name + ">");
            }
            else if(this.TerminatedType == EndTagType.ExplicitlyTerminated)
                 writer.Append("></" + m_name + ">");
            else if(this.TerminatedType == EndTagType.Terminated)
                writer.Append("/>");
            else if(this.TerminatedType == EndTagType.NonTerminated)
                writer.Append(">");
        }

    #endregion

	/////////////////////////////////////////////////////////////////////////////////
	#region 멤버변수

		/// <summary>
		/// 
		/// </summary>
		private string m_name = "";
        /// <summary>
        /// 
        /// </summary>
        private EndTagType m_terminatedType = EndTagType.NonTerminated;
		/// <summary>
		/// 
		/// </summary>
		private CHtmlNodeCollection m_nodes = null; 
		/// <summary>
		/// 
		/// </summary>
        private CHtmlAttributeCollection m_attributes = new CHtmlAttributeCollection();

    #endregion

    /////////////////////////////////////////////////////////////////////////////////
    #region 파싱한 메타

        /// <summary>
        /// 
        /// </summary>
        internal bool m_close = false;
        /// <summary>
        /// 
        /// </summary>
        internal CHtmlElement m_previousWithSameNameNode = null;

    #endregion

	}
}
