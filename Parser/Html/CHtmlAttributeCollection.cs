/*****************************************************************************\
>
>
>	 CHtmlAttributeCollection Class
>
>
>
>
\*****************************************************************************/

// CHtmlAttributeCollection.cs: implementation of the CHtmlAttributeCollection class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Cloud9.Parser.Html
{    
	
	/// <summary>
	///  ������Ʈ�� ���� �Ӽ��� ����
	/// </summary>
    public sealed class CHtmlAttributeCollection : Cloud9.Parser.Html.Base.IDiagnosisable, IEnumerable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region �⺻	

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 
		/// </summary>
		public CHtmlAttributeCollection()
		{
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public CHtmlAttributeCollection(IHtmlNodeHasAttribute ownerNode)
        {
            m_ownerNode = ownerNode;
        }

        /////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 
		/// </summary>
		public void AssertValid()
		{
            for(int index = 0, count = m_attributeList.Count; index < count; ++index)
                m_attributeList[index].AssertValid();
		}

        /////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 
		/// </summary>
		public void Dump(StringBuilder buffer, string prefix)
		{
			AssertValid();
        }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////
	#region ���հ�ü �⺻ �޼ҵ�	

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return m_attributeList.GetEnumerator();
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public int Capacity
        {
            get
            {
                return m_attributeList.Capacity;
            }
            set
            {
                m_attributeList.Capacity = value;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return m_attributeList.Count;
            }
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
        /// �Ӽ� �߰� �޼ҵ�
		/// </summary>
		/// <param name="attribute">�Ӽ�.</param>
		/// <returns>�߰��� �ε���.</returns>
		public void Add(CHtmlAttribute attribute)
		{
			System.Diagnostics.Debug.Assert(attribute != null);

            if(m_ownerNode != null)
            {
                if(attribute.OwnerNode != null)
                    attribute.OwnerNode.Attributes.RemoveAt(attribute.OwnerNode.Attributes.IndexOf(attribute));

                attribute.OwnerNode = m_ownerNode;
            }

            // Update m_attributeHash
            if(m_attributeHash.ContainsKey(attribute.Name) == false)
                m_attributeHash[attribute.Name] = attribute;
            
            m_attributeList.Add(attribute);
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ������ ������ġ�� �Ӽ� �߰�
        /// </summary>
        /// <param name="index">�߰��� �ε��� ��ġ</param>
        /// <param name="node">�߰��� �Ӽ�</param>
        public void Insert(int index, CHtmlAttribute attribute)
        {
            System.Diagnostics.Debug.Assert(index >= 0 && index <= m_attributeList.Count);
            System.Diagnostics.Debug.Assert(attribute != null);

            // �Ӽ� ������
            if(m_ownerNode != null)
            {
                if(attribute.OwnerNode != null)
                    attribute.OwnerNode.Attributes.RemoveAt(attribute.OwnerNode.Attributes.IndexOf(attribute));

                attribute.OwnerNode = m_ownerNode;
            }

            // Update m_attributeHash
            if(m_attributeHash.ContainsKey(attribute.Name) == false)
                m_attributeHash[attribute.Name] = attribute;
            else
            {
                CHtmlAttribute temp = (CHtmlAttribute)m_attributeHash[attribute.Name];
                int tempIndex = m_attributeList.IndexOf(temp);
                if(index < tempIndex)
                    m_attributeHash[attribute.Name] = attribute;
            }

            m_attributeList.Insert(index, attribute);
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            // �Ӽ� ������
            if(m_ownerNode != null)
            {
                for(int index = 0, count = m_attributeList.Count; index < count; ++index)
                {
                    CHtmlAttribute attribute = m_attributeList[index];
                    System.Diagnostics.Debug.Assert(m_ownerNode == attribute.OwnerNode);
                    attribute.OwnerNode = null;
                }                
            }

            m_attributeList.Clear();
            m_attributeHash.Clear();
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            System.Diagnostics.Debug.Assert(index >= 0 && index < m_attributeList.Count);

            CHtmlAttribute attribute = m_attributeList[index];

            // �Ӽ� ������
            if(m_ownerNode != null)
            {
                System.Diagnostics.Debug.Assert(m_ownerNode == attribute.OwnerNode);
                attribute.OwnerNode = null;
            }  

            m_attributeList.RemoveAt(index);

            // Update m_attributeHash
            if(m_attributeHash[attribute.Name] == attribute)
            {
                m_attributeHash.Remove(attribute.Name);
                for(int count = m_attributeList.Count; index < count; ++index)
                {
                    CHtmlAttribute temp = m_attributeList[index];
                    if(attribute.Name.Equals(temp.Name))
                    {
                        m_attributeHash[attribute.Name] = temp;
                        break;
                    }
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public void RemoveRange(int index, int count)
        {
            System.Diagnostics.Debug.Assert(index >= 0 && index < m_attributeList.Count);
            System.Diagnostics.Debug.Assert(count > 0);
            System.Diagnostics.Debug.Assert(index + count <= m_attributeList.Count);

            for(int scanIndex = index; scanIndex < index + count; ++scanIndex)
            {
                CHtmlAttribute attribute = m_attributeList[scanIndex];

                // �Ӽ� ������
                if(m_ownerNode != null)
                {
                    System.Diagnostics.Debug.Assert(m_ownerNode == attribute.OwnerNode);
                    attribute.OwnerNode = null;
                }

                // Update m_attributeHash
                if(m_attributeHash[attribute.Name] == attribute)
                {
                    m_attributeHash.Remove(attribute.Name);
                    for(int listIndex = index + count, listCount = m_attributeList.Count; listIndex < listCount; ++listIndex)
                    {
                        CHtmlAttribute temp = m_attributeList[listIndex];
                        if(attribute.Name.Equals(temp.Name))
                        {
                            m_attributeHash[attribute.Name] = temp;
                            break;
                        }
                    }
                }
            }

            m_attributeList.RemoveRange(index, count);
        }


    #endregion

    /////////////////////////////////////////////////////////////////////////////////
    #region ���� �⺻ �޼ҵ�/�Ӽ�

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasAttribute(string name)
        {
            System.Diagnostics.Debug.Assert(name != null);
            return m_attributeHash.ContainsKey(name);
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �ε�����ȣ�� ����.
        /// </summary>
        public CHtmlAttribute this[int index]
        {
            get
            {
                System.Diagnostics.Debug.Assert(index >= 0 && index < m_attributeList.Count);
                return m_attributeList[index];
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �Ӽ������� ����
        /// </summary>
        public CHtmlAttribute this[string name]
        {
            get
            {
                System.Diagnostics.Debug.Assert(name != null);
                return (CHtmlAttribute)m_attributeHash[name];
            }
        }
        
        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �Ӽ��� �ε��� ��ġ
        /// </summary>
        /// <param name="name">�Ӽ���.</param>
        /// <returns>�Ӽ� �ε��� ��ġ</returns>
        public int IndexOf(string name)
        {
            System.Diagnostics.Debug.Assert(name != null);

            name = name.ToLower();

            int result = -1;
            for(int index = 0, count = m_attributeList.Count; index < count; ++index)
            {
                CHtmlAttribute attribute = m_attributeList[index];
                if(attribute.Name.Equals(name))
                {
                    result = index;
                    break;
                }
            }

            return result;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �Ӽ��� �ε��� ��ġ
        /// </summary>
        /// <param name="node">�Ӽ����</param>
        /// <returns>�ε��� ��ġ</returns>
        public int IndexOf(CHtmlAttribute attribute)
        {
            System.Diagnostics.Debug.Assert(attribute != null);
            return m_attributeList.IndexOf(attribute);
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// �Ӽ��� �ش��ϴ� �Ӽ���� ��ȯ
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CHtmlAttributeCollection FindByName(string name)
        {
            System.Diagnostics.Debug.Assert(name != null);

            name = name.ToLower();

            CHtmlAttributeCollection result = new CHtmlAttributeCollection();
            FindByName(name, result);
            return result;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="result"></param>
        public void FindByName(string name, CHtmlAttributeCollection result)
        {
            System.Diagnostics.Debug.Assert(name != null);
            System.Diagnostics.Debug.Assert(result != null);

            name = name.ToLower();

            for(int index = 0, count = m_attributeList.Count; index < count; ++index)
            {
                CHtmlAttribute attribute = m_attributeList[index];
                if(attribute.Name.Equals(name))
                    result.Add(attribute);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        public void FindByNameValue(string name, string value, CHtmlAttributeCollection result)
        {
            System.Diagnostics.Debug.Assert(name != null);
            System.Diagnostics.Debug.Assert(result != null);

            name = name.ToLower();
            for(int index = 0, count = m_attributeList.Count; index < count; ++index)
            {
                CHtmlAttribute attribute = m_attributeList[index];
                if(attribute.Name.Equals(name) && attribute.Value.Equals(value))
                    result.Add(attribute);
            }
        }

    #endregion

    /////////////////////////////////////////////////////////////////////////////////
	#region �������

        /// <summary>
        /// 
        /// </summary>
        private IHtmlNodeHasAttribute m_ownerNode = null;
		/// <summary>
		/// 
		/// </summary>
        private Hashtable m_attributeHash = new Hashtable();
        /// <summary>
        /// 
        /// </summary>
        private List<CHtmlAttribute> m_attributeList = new List<CHtmlAttribute>();	

    #endregion 

	}
}
