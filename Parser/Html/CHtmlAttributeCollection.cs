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
	///  엘리먼트에 속한 속성들 집합
	/// </summary>
    public sealed class CHtmlAttributeCollection : Cloud9.Parser.Html.Base.IDiagnosisable, IEnumerable
	{

	/////////////////////////////////////////////////////////////////////////////////
	#region 기본	

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
	#region 집합객체 기본 메소드	

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
        /// 속성 추가 메소드
		/// </summary>
		/// <param name="attribute">속성.</param>
		/// <returns>추가된 인덱스.</returns>
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
        /// 집합의 지정위치에 속성 추가
        /// </summary>
        /// <param name="index">추가할 인덱스 위치</param>
        /// <param name="node">추가한 속성</param>
        public void Insert(int index, CHtmlAttribute attribute)
        {
            System.Diagnostics.Debug.Assert(index >= 0 && index <= m_attributeList.Count);
            System.Diagnostics.Debug.Assert(attribute != null);

            // 속성 소유자
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
            // 속성 소유지
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

            // 속성 소유자
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

                // 속성 소유자
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
    #region 집합 기본 메소드/속성

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
        /// 인덱스번호로 접근.
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
        /// 속성명으로 접근
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
        /// 속성의 인덱스 위치
        /// </summary>
        /// <param name="name">속성명.</param>
        /// <returns>속성 인덱스 위치</returns>
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
        /// 속성의 인뎃스 위치
        /// </summary>
        /// <param name="node">속성노드</param>
        /// <returns>인덱스 위치</returns>
        public int IndexOf(CHtmlAttribute attribute)
        {
            System.Diagnostics.Debug.Assert(attribute != null);
            return m_attributeList.IndexOf(attribute);
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 속성명에 해당하는 속성노드 반환
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
	#region 멤버변수

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
