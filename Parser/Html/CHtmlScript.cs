/*****************************************************************************\
>
>
>	 CHtmlScript Class
>
>
>
>
\*****************************************************************************/

// CHtmlScript.cs: implementation of the CHtmlScript class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;

namespace Cloud9.Parser.Html
{
	/// <summary>
    /// The CHtmlScript node represents a simple piece of script from the document.
	/// </summary>
    public sealed class CHtmlScript : CHtmlNode, IHtmlNodeHasAttribute, Cloud9.Parser.Html.Base.IDiagnosisable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region 기본

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 
		/// </summary>
        public CHtmlScript()
		{
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public CHtmlScript(string script)
        {
            System.Diagnostics.Debug.Assert(script != null);
            m_script = script;
        }

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 생성자
        /// </summary>
        public CHtmlScript(CHtmlScript obj) 
            : base(obj)
        {
            System.Diagnostics.Debug.Assert(obj != null);

            obj.AssertValid();
            m_script = obj.m_script;

            int count = obj.m_attributes.Count;
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
            return new CHtmlScript(this);
        }

		/////////////////////////////////////////////////////////////////////////////        
		/// <summary>
		/// 
		/// </summary>
		public override void AssertValid()
		{          			
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
            buffer.Append(prefix + "Node ID: " + this.NodeID + "\n");	
            buffer.Append(prefix + "HTML Tag: " + this.Tag + "\n");	

            if(m_script.Length == 0)
                buffer.Append(prefix + "Script content is empty\n");
            else
            {
                buffer.Append(prefix + "Script content:");
                buffer.Append("\n====================================================================================================================================\n");
                buffer.Append(m_script);
                buffer.Append("\n====================================================================================================================================\n");
            }
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(Cloud9.Parser.Html.Base.IBaseVisitor visitor)
        {
            Cloud9.Parser.Html.Base.IVisitor<CHtmlScript> scriptVisitor = visitor as Cloud9.Parser.Html.Base.IVisitor<CHtmlScript>;
            if(scriptVisitor != null) scriptVisitor.Visit(this);
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
                return "script";
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
                return true;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This will return the HTML representation of this element.
        /// </summary>
        /// <returns></returns>		
        public string Tag
        {
            get
            {
                string temp = "<script";
                for(int index = 0, count = m_attributes.Count; index < count; ++index)
                    temp += " " + m_attributes[index].HTML;

                temp += ">";
                return temp;
            }
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// This is the Script associated with this node.
		/// </summary>
		public string Script
		{
			get
			{
                return m_script;
			}
			set
			{
				System.Diagnostics.Debug.Assert(value != null);
                m_script = value;
			}
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This is the collection of attributes associated with this element.
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
        /// This is the text associated with this node.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.HTML;
        }

    #endregion	

    /////////////////////////////////////////////////////////////////////////////////
    #region  

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///   ┬ HTML 쫞?
		/// </summary>
        /// 
        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This will return the full HTML to represent this node (and all child nodes)
        /// </summary>
        internal override void TransformHTML(StringBuilder writer, int indentDepth)
        {
            System.Diagnostics.Debug.Assert(writer != null);		

            writer.Append("<script");
            for(int index = 0, count = m_attributes.Count; index < count; ++index)
            {
                writer.Append(" ");
                m_attributes[index].TransformHTML(writer);
            }
            writer.Append(">");

            writer.Append(m_script);
            writer.Append("</script>");
        }

#endregion	

	/////////////////////////////////////////////////////////////////////////////////
	#region	멤버변수

		/// <summary>
		/// 
		/// </summary>
		private string m_script = "";
        /// <summary>
        /// 
        /// </summary>
        private CHtmlAttributeCollection m_attributes = new CHtmlAttributeCollection();

    #endregion	

	}
}
