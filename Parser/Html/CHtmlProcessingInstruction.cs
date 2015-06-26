/*****************************************************************************\
>
>
>	 CHtmlProcessingInstruction Class
>
>
>
>
\*****************************************************************************/

// CHtmlProcessingInstruction: implementation of the CHtmlProcessingInstruction class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;

namespace Cloud9.Parser.Html
{
	/// <summary>
    /// The CHtmlProcessingInstruction node represents a simple piece of processing instruction from the document.
	/// </summary>
    public sealed class CHtmlProcessingInstruction : CHtmlNode, Cloud9.Parser.Html.Base.IDiagnosisable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region 기본

        /////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
        public CHtmlProcessingInstruction()
        {
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
        public CHtmlProcessingInstruction(string value)
		{
            System.Diagnostics.Debug.Assert(value != null);
            m_value = value;
		}

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 생성자
        /// </summary>
        public CHtmlProcessingInstruction(CHtmlProcessingInstruction obj) 
            : base(obj)
        {
            System.Diagnostics.Debug.Assert(obj != null) ;

            obj.AssertValid();
            m_value = obj.m_value;
        }

        /////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new CHtmlProcessingInstruction(this);
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

            if(m_value.Length == 0)
                buffer.Append(prefix + "Processing Instruction is empty\n");
            else
            {
                buffer.Append(prefix + "Processing Instruction content:");
                buffer.Append("\n====================================================================================================================================\n");
                buffer.Append(m_value);
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
            Cloud9.Parser.Html.Base.IVisitor<CHtmlProcessingInstruction> piVisitor = visitor as Cloud9.Parser.Html.Base.IVisitor<CHtmlProcessingInstruction>;
            if(piVisitor != null) piVisitor.Visit(this);
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
        /// This is the name associated with this node.
        /// </summary>
        public string Value
        {
            get
            {
                return m_value;
            }
            set
            {
                System.Diagnostics.Debug.Assert(value != null) ;
                m_value = value;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This is the text associated with this node.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "<?" + m_value + "?>";
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

            for(int count = 0; count < indentDepth; ++count)
                writer.Append("\t");

            writer.Append("<?");
            writer.Append(m_value);
            writer.Append("?>\n");
        }

    #endregion	

	/////////////////////////////////////////////////////////////////////////////////
	#region	멤버변수

		/// <summary>
		/// 
		/// </summary>
        private string m_value = "";

    #endregion	

	}
}
