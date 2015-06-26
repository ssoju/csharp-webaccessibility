/*****************************************************************************\
>
>
>	 CHtmlComment Class
>
>
>
>
\*****************************************************************************/

// CHtmlComment: implementation of the CHtmlComment class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;

namespace Cloud9.Parser.Html
{
	/// <summary>
	/// ��ť��Ʈ�� �����ϴ� �ڸ�Ʈ ��� ����.
	/// </summary>
    public sealed class CHtmlComment : CHtmlNode, Cloud9.Parser.Html.Base.IDiagnosisable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region 

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public CHtmlComment()
        {
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 
		/// </summary>
		/// <param name="comment"></param>
		public CHtmlComment(string comment)
		{
            System.Diagnostics.Debug.Assert(comment != null);
            m_comment = comment;
		}

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ������
        /// </summary>
        public CHtmlComment(CHtmlComment obj) 
            : base(obj)
        {
            System.Diagnostics.Debug.Assert(obj != null);

            obj.AssertValid();
            m_comment = obj.m_comment;
        }

        /////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new CHtmlComment(this);
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
		/// ������
		/// </summary>
		public override void Dump(StringBuilder buffer, string prefix)
		{
			AssertValid();
			string old = prefix;
			buffer.Append(old + "�uObject " + GetType().Name + " Dump : \n");							

			prefix += " ";
            buffer.Append(prefix + "Node ID: " + this.NodeID + "\n");	

            if (m_comment.Length == 0)
				buffer.Append(prefix + "Comment content is empty\n");
            else
            {
                buffer.Append(prefix + "Comment content:");
                buffer.Append("\n====================================================================================================================================\n");
                buffer.Append(m_comment);
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
            Cloud9.Parser.Html.Base.IVisitor<CHtmlComment> commentVisitor = visitor as Cloud9.Parser.Html.Base.IVisitor<CHtmlComment>;
            if(commentVisitor != null) commentVisitor.Visit(this);
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
        /// 
        /// </summary>
        public bool SGMLComment
        {
            get
            {
                return m_SGMLComment;
            }
            set
            {
                m_SGMLComment = value;
            }
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// �ڸ�Ʈ ����.
		/// </summary>
		public string Comment
		{
			get
			{
                return m_comment;
			}
			set
			{
				System.Diagnostics.Debug.Assert(value != null);
                m_comment = value;
			}
		}

    #endregion	

    /////////////////////////////////////////////////////////////////////////////////
    #region  

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ����� ��ü html ��ȯ
        /// </summary>
        internal override void TransformHTML(StringBuilder writer, int indentDepth)
        {
            System.Diagnostics.Debug.Assert(writer != null);

            for(int count = 0; count < indentDepth; ++count)
                writer.Append("\t");

            if(m_SGMLComment == false)
                writer.Append("<!--");
            else writer.Append("<!");
            
            writer.Append(m_comment);

            if(m_SGMLComment == false)
                writer.Append("-->\n");
            else writer.Append(">\n");
        }

    #endregion	

	/////////////////////////////////////////////////////////////////////////////////
	#region	�������

		/// <summary>
		/// 
		/// </summary>
        private string m_comment = "";
        /// <summary>
        /// 
        /// </summary>
        private bool m_SGMLComment = false;

    #endregion	

	}
}
