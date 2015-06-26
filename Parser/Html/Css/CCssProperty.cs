/*****************************************************************************\
>
>
>	 DCssProperty Class
>
>
>
>
\*****************************************************************************/

// DCssProperty.cs: implementation of the DCssProperty class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;

namespace Cloud9.Parser.Html.Css
{
	/// <summary>
    /// The DCssProperty object represents a named value
	/// </summary>
    public sealed class DCssProperty : Cloud9.Parser.Html.Base.IDiagnosisable, ICloneable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region �⺻	

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// ������
		/// </summary>
		public DCssProperty()
		{
		}

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ������
        /// </summary>
        public DCssProperty(string name, string value)
        {
            System.Diagnostics.Debug.Assert(name != null && CHtmlUtil.ExistWhiteSpaceChar(name) == false);
            System.Diagnostics.Debug.Assert(value != null);

            m_propertyName = name.ToLower();
            m_propertyValue = value.Trim(CHtmlUtil.WhiteSpaceCharsArray);
        }

        ///////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ������
        /// </summary>
        public DCssProperty(DCssProperty obj)
        {
            System.Diagnostics.Debug.Assert(obj != null);

            obj.AssertValid();
            m_propertyName = obj.m_propertyName;
            m_propertyValue = obj.m_propertyValue;
        }

        /////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new DCssProperty(this);
        }

		/////////////////////////////////////////////////////////////////////////////        
		/// <summary>
		/// 
		/// </summary>
		public void AssertValid()
		{
            System.Diagnostics.Debug.Assert(m_propertyName != null && CHtmlUtil.ExistWhiteSpaceChar(m_propertyName) == false, "Member variable, m_propertyName is invalid");
            System.Diagnostics.Debug.Assert(m_propertyValue != null, "Member variable, m_propertyValue is invalid");       	
		}

		/////////////////////////////////////////////////////////////////////////////       
		/// <summary>
		/// ������
		/// </summary>
		public void Dump(StringBuilder buffer, string prefix)
		{
            AssertValid();
            string old = prefix;
            buffer.Append(old + "�uObject " + GetType().Name + " Dump : \n");

            prefix += " ";
            buffer.Append(prefix + "Property: \"" + this.CSS + "\"\n");
		}

    #endregion	

    /////////////////////////////////////////////////////////////////////////////////
    #region  

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// �ݩʦW��
		/// </summary>
		public string Name
		{
			get
			{
                return m_propertyName;
			}
			set
			{
                System.Diagnostics.Debug.Assert(value != null);
                System.Diagnostics.Debug.Assert(CHtmlUtil.ExistWhiteSpaceChar(value) != true);

                m_propertyName = value.ToLower(); 
			}
		}

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// �ݩ�?
		/// </summary>
		public string Value
		{
			get
			{
                return m_propertyValue;
			}
			set
			{                
                System.Diagnostics.Debug.Assert(value != null);
                m_propertyValue = value.Trim(CHtmlUtil.WhiteSpaceCharsArray); 
			}
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This is the text associated with this node.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.CSS;
        }

    #endregion	

    /////////////////////////////////////////////////////////////////////////////////
    #region  

		///////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  �� CSS �r?
		/// </summary>		
        public string CSS
		{
            get
            {
                StringBuilder writer = new StringBuilder();
                TransformCSS(writer);

                return writer.ToString();
            }
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This will return the full CSS to represent this property
        /// </summary>
        internal void TransformCSS(StringBuilder writer)
        {
            System.Diagnostics.Debug.Assert(writer != null);

            writer.Append(m_propertyName);
            writer.Append(":");
            writer.Append(m_propertyValue);
            writer.Append(";");
        }
	
    #endregion	

	/////////////////////////////////////////////////////////////////////////////////
	#region �������

		/// <summary>
		/// �ݩʦW��
		/// </summary>
        private string m_propertyName = "";
		/// <summary>
		/// �ݩ�?
		/// </summary>
        private string m_propertyValue = "";

    #endregion

	}

}
