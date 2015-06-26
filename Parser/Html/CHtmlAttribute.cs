/*****************************************************************************\
>
>
>	 CHtmlAttribute Class
>
>
>
>
\*****************************************************************************/

// CHtmlAttribute.cs: implementation of the CHtmlAttribute class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;

namespace Cloud9.Parser.Html
{
	/// <summary>
    /// CHTmlElement�� ���Ե� �Ӽ��� �����ϴ� Ŭ����
	/// </summary>
    public sealed class CHtmlAttribute : Cloud9.Parser.Html.Base.IDiagnosisable, ICloneable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region �⺻

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// ������
		/// </summary>
		public CHtmlAttribute()
		{
		}

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ������
        /// </summary>
        public CHtmlAttribute(string name)
        {
            System.Diagnostics.Debug.Assert(name != null && CHtmlUtil.ExistWhiteSpaceChar(name) == false);

            m_attributeName = name.Trim().ToLower();
        }

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ������
        /// </summary>
        public CHtmlAttribute(string name, string value)
        {
            System.Diagnostics.Debug.Assert(name != null && CHtmlUtil.ExistWhiteSpaceChar(name) == false);
            System.Diagnostics.Debug.Assert(value != null);

            m_attributeName = name.Trim().ToLower();
            m_attributeValue = value;
        }

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ������
        /// </summary>
        public CHtmlAttribute(CHtmlAttribute obj)
        {
            System.Diagnostics.Debug.Assert(obj != null);

            obj.AssertValid();
            m_attributeName = obj.m_attributeName;
            m_attributeValue = obj.m_attributeValue;
        }

        /////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new CHtmlAttribute(this);
        }

		/////////////////////////////////////////////////////////////////////////////        
		/// <summary>
		/// ����� ���� ���
		/// </summary>
		public void AssertValid()
		{
            System.Diagnostics.Debug.Assert(m_attributeName != null && CHtmlUtil.ExistWhiteSpaceChar(m_attributeName) == false, "Member variable, m_attributeName is invalid");
            System.Diagnostics.Debug.Assert(m_attributeValue != null, "Member variable, m_attributeValue is invalid");       	
		}

		/////////////////////////////////////////////////////////////////////////////       
		/// <summary>
		/// �������ڿ� ����
		/// </summary>
		public void Dump(StringBuilder buffer, string prefix)
		{
            AssertValid();
            string old = prefix;
            buffer.Append(old + "�uObject " + GetType().Name + " Dump : \n");

            prefix += " ";

            if(m_attributeName.Length == 0)
                buffer.Append(prefix + "Attribute is empty\n");
            else
                buffer.Append(prefix + "Attribute: \"" + this.HTML + "\"\n");
		}

    #endregion	

    /////////////////////////////////////////////////////////////////////////////////
    #region �Ӽ�

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// �ݩʦW��
		/// </summary>
		public string Name
		{
			get
			{
				return m_attributeName;
			}
			set
			{
                System.Diagnostics.Debug.Assert(value != null);
                System.Diagnostics.Debug.Assert(CHtmlUtil.ExistWhiteSpaceChar(value) == false);

                m_attributeName = value.Trim().ToLower(); 
			}
		}

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// ��
		/// </summary>
		public string Value
		{
			get
			{
				return m_attributeValue;
			}
			set
			{
                System.Diagnostics.Debug.Assert(value != null);
                m_attributeValue = value; 
			}
		}

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public IHtmlNodeHasAttribute OwnerNode
        {
            get
            {
                return m_ownerNode;
            }
            internal set
            {
                m_ownerNode = value;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ��������� �±׷� ��ȯ.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.HTML;
        }

    #endregion	

    /////////////////////////////////////////////////////////////////////////////////
    #region �±׼Ӽ�

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// html�� ��ȯ
		/// </summary>		
        public string HTML
		{
            get
            {
                StringBuilder writer = new StringBuilder();
                TransformHTML(writer);

                return writer.ToString();
            }
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �ڽĳ�带 ������ ��ü html ��ȯ
        /// </summary>
        internal void TransformHTML(StringBuilder writer)
        {
            System.Diagnostics.Debug.Assert(writer != null);

            writer.Append(m_attributeName);
            writer.Append("=\"");
            writer.Append(CHtmlUtil.TranStrToHtmlText(m_attributeValue));
            writer.Append("\"");
        }
	
    #endregion	

	/////////////////////////////////////////////////////////////////////////////////
	#region �������

		/// <summary>
		/// �Ӽ���
		/// </summary>
		private string m_attributeName = "";
		/// <summary>
		/// �Ӽ���
		/// </summary>
		private string m_attributeValue = "";
        /// <summary>
        /// 
        /// </summary>
        private IHtmlNodeHasAttribute m_ownerNode = null;

    #endregion

	}

}
