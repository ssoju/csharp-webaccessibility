/*****************************************************************************\
>
>
>	 CHtmlDocument Class
>
>
>
>
\*****************************************************************************/

// CHtmlDocument.cs: implementation of the CHtmlDocument class.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Text;
using Cloud9.Parser.Html;

namespace Cloud9.Parser.Html
{
	/// <summary>
    /// html document.
	/// </summary>
    public class CHtmlDocument : Cloud9.Parser.Html.Base.IDiagnosisable, ICloneable
	{

    /////////////////////////////////////////////////////////////////////////////////
    #region 기본

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public CHtmlDocument()
        {
            m_parser = new CHtmlParser();
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parser"></param>
        public CHtmlDocument(IHtmlParser parser)
        {
            System.Diagnostics.Debug.Assert(parser != null);
            m_parser = parser;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        public CHtmlDocument(string html)
        {
            System.Diagnostics.Debug.Assert(html != null);
            m_parser = new CHtmlParser();
            LoadHtml(html);
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
        /// 
		/// </summary>
		/// <param name="html"></param>
		/// <param name="parser"></param>
        public CHtmlDocument(string html, IHtmlParser parser)
		{
            System.Diagnostics.Debug.Assert(html != null);
            System.Diagnostics.Debug.Assert(parser != null);
            m_parser = parser;
            LoadHtml(html);
		}

        /////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            CHtmlDocument newDoc = new CHtmlDocument();
            newDoc.m_charset = m_charset;

            int count = m_nodeList.Count;
            newDoc.m_nodeList.Capacity = count;
            for(int index = 0; index < count; ++index)
                newDoc.m_nodeList.Add((CHtmlNode)m_nodeList[index].Clone());                

           return newDoc;
        }

        /////////////////////////////////////////////////////////////////////////////////        
		/// <summary>
		/// 
		/// </summary>
		public void AssertValid()
		{
            for(int index = 0, count = m_nodeList.Count; index < count; ++index)
                ((Cloud9.Parser.Html.Base.IDiagnosisable)m_nodeList[index]).AssertValid();
		}

        /////////////////////////////////////////////////////////////////////////////////       
		/// <summary>
		/// 생성자
		/// </summary>
		public void Dump(StringBuilder buffer, string prefix)
		{
			AssertValid();
			string old = prefix;
			buffer.Append(old + "쥂Object " + GetType().Name + " Dump : \n");							

			prefix += " ";
			buffer.Append(prefix + "CHtmlNode number: " + m_nodeList.Count + "\n");	

			if(m_nodeList.Count != 0)
			{
				buffer.Append(prefix + "Deep dump in the following:\n");

                for(int index = 0, count = m_nodeList.Count; index < count; ++index) 
                {
                    buffer.Append(prefix + " n");
                    m_nodeList[index].Dump(buffer, prefix);
                }      
			}
		}

    #endregion 

    /////////////////////////////////////////////////////////////////////////////////
    #region 멤버변수

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public virtual void Load(string filePath)
        {
            System.Diagnostics.Debug.Assert(filePath != null);

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            Load(fileStream);
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        public virtual void Load(Stream inStream)
        {
            System.Diagnostics.Debug.Assert(inStream != null);
            m_charset = null;

            StreamReader streamReader = null;

            m_charset = DetectCharset(inStream);
            if(m_charset != null)
                streamReader = new StreamReader(inStream, m_charset);
            else
            {
                streamReader = new StreamReader(inStream, true);
                m_charset = streamReader.CurrentEncoding;
            }

            m_nodeList.Clear();
            m_parser.Parse(streamReader.ReadToEnd(), m_nodeList);

            CHtmlNodeCollection bases = m_nodeList.FindByName("base", true);
            if (bases != null && bases.Count > 0)
                this.docBase = ((CHtmlElement)bases[bases.Count - 1]).Attributes["href"].Value;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public virtual void Load(TextReader reader)
        {            
            System.Diagnostics.Debug.Assert(reader != null);
            m_charset = null;

            LoadHtml(reader.ReadToEnd());            
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        public virtual void LoadHtml(string html)
        {
            System.Diagnostics.Debug.Assert(html != null);
            m_nodeList.Clear();
            m_parser.Parse(html, m_nodeList);

            if(m_charset == null)
            {
                m_charset = DetectCharset(m_nodeList);
                if(m_charset == null) m_charset = Encoding.Unicode;
            }

            CHtmlNodeCollection bases = m_nodeList.FindByName("base", true);
            if (bases != null && bases.Count > 0 && ((CHtmlElement)bases[bases.Count - 1]).Attributes["href"]!=null)
                this.docBase = ((CHtmlElement)bases[bases.Count - 1]).Attributes["href"].Value;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public virtual void Save(string filePath)
        {
            System.Diagnostics.Debug.Assert(filePath != null);

            FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            Save(fileStream);
            fileStream.Flush();
            fileStream.Close();
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outStream"></param>
        public virtual void Save(Stream outStream)
        {
            StreamWriter writer = new StreamWriter(outStream, m_charset);
            Save(writer);
            writer.Flush();
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public virtual void Save(TextWriter writer)
        {
             writer.Write(this.HTML);
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitor"></param>
        public void Visit(Cloud9.Parser.Html.Base.IBaseVisitor visitor)
        {
            int count = m_nodeList.Count;
            for(int index = 0; index < count; ++index)
                m_nodeList[index].Accept(visitor);
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public virtual Encoding Charset
        {
            get
            {
                return m_charset;
            }
            set
            {
                System.Diagnostics.Debug.Assert(value != null);

                if(!m_charset.Equals(value))
                {
                    m_charset = value;
                    
                    CHtmlNodeCollection metaNodes = new CHtmlNodeCollection();
                    CHtmlElement node = m_nodeList["html"] as CHtmlElement;
                    if(node != null) node = node.Nodes["head"] as CHtmlElement;
                    if(node != null) node.Nodes.FindByNameAttribute(metaNodes, "meta", "content", false);

                    for(int nodeIndex = 0, nodeCount = metaNodes.Count; nodeIndex < nodeCount; ++nodeIndex) 
                    {
                        CHtmlElement metaElement = metaNodes[nodeIndex] as CHtmlElement;
                        if(metaElement != null)
                        {
                            int index = -1;
                            CHtmlAttributeCollection attributes = metaElement.Attributes.FindByName("content");
                            for(int attributeIndex = 0, attributeCount = attributes.Count; attributeIndex < attributeCount; ++attributeIndex) 
                            {
                                CHtmlAttribute attribute = attributes[attributeIndex];
                                if((index = attribute.Value.IndexOf("charset")) != -1)
                                {
                                    string attributeValue = attribute.Value;
                                    int startIndex = index + 7;
                                    while(startIndex < attributeValue.Length && CHtmlUtil.EqualesOfAnyChar(attributeValue[startIndex], " =")) ++startIndex;
                                    int endIndex = startIndex + 1;
                                    while(endIndex < attributeValue.Length && !CHtmlUtil.EqualesOfAnyChar(attributeValue[endIndex], " ")) ++endIndex;

                                    if(startIndex < attributeValue.Length && endIndex - startIndex > 0)
                                    {
                                        attributeValue = attributeValue.Remove(startIndex, endIndex - startIndex);
                                        attributeValue = attributeValue.Insert(startIndex, m_charset.WebName);
                                        attribute.Value = attributeValue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

		/////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// 전체 html 반환.
		/// </summary>
        public virtual string HTML
		{
			get
			{
				StringBuilder writer = new StringBuilder();
                for(int index = 0, count = m_nodeList.Count; index < count; ++index)
                    m_nodeList[index].TransformHTML(writer, 0);

                return writer.ToString();
			}
		}

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        public CHtmlNodeCollection Nodes
        {
            get
            {
                return m_nodeList;
            }
        }

    #endregion

	/////////////////////////////////////////////////////////////////////////////////
	#region 

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlStream"></param>
        /// <returns></returns>
        private Encoding DetectCharset(CHtmlNodeCollection nodes)
        {
            Encoding result = null;

            string charset = "";

            CHtmlNodeCollection metaNodes = new CHtmlNodeCollection();
            CHtmlElement node = nodes["html"] as CHtmlElement;
            if(node != null) node = node.Nodes["head"] as CHtmlElement;
            if(node != null) node.Nodes.FindByNameAttribute(metaNodes, "meta", "content", false);

            for(int nodeIndex = 0, count = metaNodes.Count; nodeIndex < count; ++nodeIndex)
            {
                CHtmlElement metaElement = metaNodes[nodeIndex] as CHtmlElement;
                if(metaElement != null)
                {
                    int index = -1;
                    CHtmlAttributeCollection attributes = metaElement.Attributes.FindByName("content");
                    for(int attributeIndex = 0, attributeCount = attributes.Count; attributeIndex < attributeCount; ++attributeIndex) 
                    {
                        CHtmlAttribute attribute = attributes[attributeIndex];
                        if((index = attribute.Value.IndexOf("charset")) != -1)
                        {
                            string value = attribute.Value;
                            int startIndex = index + 7;
                            while(startIndex < value.Length && CHtmlUtil.EqualesOfAnyChar(value[startIndex], " =")) ++startIndex;
                            int endIndex = startIndex + 1;
                            while(endIndex < value.Length && !CHtmlUtil.EqualesOfAnyChar(value[endIndex], " ")) ++endIndex;

                            if(startIndex < value.Length && endIndex - startIndex > 0)
                            {
                                charset = value.Substring(startIndex, endIndex - startIndex);
                                try
                                {
                                    result = Encoding.GetEncoding(charset);
                                    break;
                                }
                                catch(Exception)
                                {
                                }
                            }
                        }
                    }
                }
            }
                
            return result;
        }

        /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlStream"></param>
        private Encoding DetectCharset(Stream inStream)
        {
            Encoding result = null;

            string charset = "";

            long position = inStream.Position;
            System.IO.StreamReader reader = new System.IO.StreamReader(inStream);
            while(reader.EndOfStream == false)
            {
                string buffer = reader.ReadLine();
                int index = buffer.IndexOf("charset");

                if(index != -1 && buffer.Length > "charset".Length)
                {
                    int startIndex = index + "charset".Length;
                    while(startIndex < buffer.Length && CHtmlUtil.EqualesOfAnyChar(buffer[startIndex], " \r\n\t=\'\"<>")) ++startIndex;
                    int endIndex = startIndex + 1;
                    while(endIndex < buffer.Length && !CHtmlUtil.EqualesOfAnyChar(buffer[endIndex], " \r\n\t=\'\"<>")) ++endIndex;

                    if(startIndex < buffer.Length && endIndex - startIndex > 0)
                    {
                        charset = buffer.Substring(startIndex, endIndex - startIndex);
                        try
                        {
                            result = Encoding.GetEncoding(charset);
                            break;
                        }
                        catch(Exception)
                        {
                        }
                    }
                }
            }

            inStream.Position = position;

            return result;
        }

        public string DocBase
        {
            get
            {
                return docBase;    
            }

            set
            {
                docBase = value;
            }
        }

        public string AbsolutePath
        {
            get
            {
                return absolutPath;
            }

            set
            {
                absolutPath = value;
            }
        }

        public string HRef
        {
            get
            {
                return href;
            }

            set
            {
                href = value;
            }
        }

        public CHtmlNodeCollection Links
        {
            get
            {
                CHtmlNodeCollection links = new CHtmlNodeCollection();
                if(this.m_nodeList.Count > 0) 
                {
                    FindLink(this.m_nodeList, ref links);
                }

                return links;
            }
        }

        private void FindLink(CHtmlNodeCollection parentNodes, ref CHtmlNodeCollection links)
        {
            if (parentNodes == null || parentNodes.Count == 0) return;

            CHtmlAttribute attr;
            foreach(CHtmlNode node in parentNodes)
            {
                if(node is CHtmlElement)
                {
                    if(node == null) continue;

                    attr = null;
                    CHtmlElement element = node as CHtmlElement;
                    switch(element.Name.Trim().ToLower())
                    {
                        case "a":
                        case "link":
                        case "frame":
                            attr = element.Attributes["href"];
                            break;
                        case "script":
                            attr = element.Attributes["src"];
                            break;
                    }

                    if(attr != null)
                    {
                        links.Add(node);
                    }

                    if(element.Nodes.Count > 0)
                    {
                        FindLink(element.Nodes, ref links);
                    }
                }
            }
        }


    #endregion

	/////////////////////////////////////////////////////////////////////////////////
	#region 멤버변수

        /// <summary>
        /// 
        /// </summary>
        private IHtmlParser m_parser = null;
        /// <summary>
        /// 
        /// </summary>
        private Encoding m_charset = Encoding.Default;
		/// <summary>
		/// 
		/// </summary>
		private CHtmlNodeCollection m_nodeList = new CHtmlNodeCollection(null);

        /// <summary>
        /// 
        /// </summary>
        private string docBase;

        /// <summary>
        /// 
        /// </summary>
        private string absolutPath;

        /// <summary>
        /// 
        /// </summary>
        private string href;

    #endregion 

	}
}
