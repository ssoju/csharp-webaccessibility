/****************************************************************************\
>
>
>	 CHtmlText Class
>
>
>
>
\*****************************************************************************/

// CHtmlText.cs: implementation of the CHtmlText class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;

namespace Cloud9.Parser.Html
{
	/// <summary>
	/// The CHtmlText node represents a simple piece of text from the document.
	/// </summary>
    public sealed class CHtmlText : CHtmlNode, Cloud9.Parser.Html.Base.IDiagnosisable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region 기본

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public CHtmlText()
        {
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// This constructs a new node with the given text content.
		/// </summary>
		/// <param name="text"></param>
		public CHtmlText(string text)
		{
            System.Diagnostics.Debug.Assert(text != null);  
			m_text = text;
		}

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 생성자
        /// </summary>
        public CHtmlText(CHtmlText obj) 
            : base(obj)
        {
            System.Diagnostics.Debug.Assert(obj != null);

            obj.AssertValid();
            m_text = obj.m_text;
        }

        /////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new CHtmlText(this);
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

			if(m_text.Length == 0)
				buffer.Append(prefix + "Text content is empty\n");
			else if(this.IsWhiteSpace)
                buffer.Append(prefix + "Text content is white space\n");
            else
                buffer.Append(prefix + "Text content: \"" + m_text + "\"\n");
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(Cloud9.Parser.Html.Base.IBaseVisitor visitor)
        {
            Cloud9.Parser.Html.Base.IVisitor<CHtmlText> textVisitor = visitor as Cloud9.Parser.Html.Base.IVisitor<CHtmlText>;
            if(textVisitor != null) textVisitor.Visit(this);
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
                return "";
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
		/// This is the text associated with this node.
		/// </summary>
		public string Text
		{
			get
			{
				return m_text;
			}
			set
			{
				System.Diagnostics.Debug.Assert(value != null);
                m_text = value;
			}
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public bool IsWhiteSpace
        {
            get
            {
                return m_text.Length == 1 && CHtmlUtil.IsWhiteSpaceChar(m_text[0]);
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
            writer.Append(CHtmlUtil.TranStrToHtmlText(m_text));
		}

    #endregion	

	/////////////////////////////////////////////////////////////////////////////////
	#region	멤버변수

		/// <summary>
		/// 
		/// </summary>
		private string m_text = "";

    #endregion	

	}
}
