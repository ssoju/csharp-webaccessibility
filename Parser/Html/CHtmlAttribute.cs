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
    /// CHTmlElement에 포함된 속성을 관리하는 클래스
	/// </summary>
    public sealed class CHtmlAttribute : Cloud9.Parser.Html.Base.IDiagnosisable, ICloneable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region 기본

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 생성자
		/// </summary>
		public CHtmlAttribute()
		{
		}

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 생성자
        /// </summary>
        public CHtmlAttribute(string name)
        {
            System.Diagnostics.Debug.Assert(name != null && CHtmlUtil.ExistWhiteSpaceChar(name) == false);

            m_attributeName = name.Trim().ToLower();
        }

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 생성자
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
        /// 생성자
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
		/// 디버깅 내용 출력
		/// </summary>
		public void AssertValid()
		{
            System.Diagnostics.Debug.Assert(m_attributeName != null && CHtmlUtil.ExistWhiteSpaceChar(m_attributeName) == false, "Member variable, m_attributeName is invalid");
            System.Diagnostics.Debug.Assert(m_attributeValue != null, "Member variable, m_attributeValue is invalid");       	
		}

		/////////////////////////////////////////////////////////////////////////////       
		/// <summary>
		/// 덤프문자열 생성
		/// </summary>
		public void Dump(StringBuilder buffer, string prefix)
		{
            AssertValid();
            string old = prefix;
            buffer.Append(old + "쥂Object " + GetType().Name + " Dump : \n");

            prefix += " ";

            if(m_attributeName.Length == 0)
                buffer.Append(prefix + "Attribute is empty\n");
            else
                buffer.Append(prefix + "Attribute: \"" + this.HTML + "\"\n");
		}

    #endregion	

    /////////////////////////////////////////////////////////////////////////////////
    #region 속성

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 콜㈙쫁붙
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
		/// 값
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
        /// 노드정보를 태그로 변환.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.HTML;
        }

    #endregion	

    /////////////////////////////////////////////////////////////////////////////////
    #region 태그속성

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// html로 변환
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
        /// 자식노드를 포함한 전체 html 반환
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
	#region 멤버변수

		/// <summary>
		/// 속성명
		/// </summary>
		private string m_attributeName = "";
		/// <summary>
		/// 속성값
		/// </summary>
		private string m_attributeValue = "";
        /// <summary>
        /// 
        /// </summary>
        private IHtmlNodeHasAttribute m_ownerNode = null;

    #endregion

	}

}
