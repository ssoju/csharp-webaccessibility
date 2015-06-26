using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Cloud9.WebCrawler
{
    /// <summary>
    /// </summary>
    public class Crawler
    {
        /// <summary>
        /// </summary>
        private bool stopNow = true;

        /// <summary>
        /// </summary>
        private string startingPage = "";

        /// <summary>
        /// </summary>
        private bool noProxy = false;

        /// <summary>
        /// 외부사이트 검색 여부
        /// </summary>
        private bool isOutboundSearch = false;
        public bool IsOutboundSearch
        {
            get { return isOutboundSearch;  }
            set { this.isOutboundSearch = value;  }
        }

        /// <summary>
        /// 최대 깊이
        /// </summary>
        private int maxDepth = 1;
        public int MaxDepth
        {
            get { return maxDepth;  }
            set { this.maxDepth = value; }
        }

        /// <summary>
        /// Event(s)
        /// </summary>
        public delegate void CurrentPageEventHandler(object sender, CurrentPageEventArgs e);
        public event CurrentPageEventHandler CurrentPageEvent;
        public event EventHandler PageFoundEvent;
        public event EventHandler CrawlFinishedEvent;

        public Crawler(string startingPage, bool noProxy)
        {
            this.startingPage = startingPage;

            this.noProxy = noProxy;
        }

        public void Start()
        {
            lock (this)
            {
                this.stopNow = false;
            }
            new Thread(new ThreadStart(Crawl)).Start();
        }

        public void Stop()
        {
            lock (this)
            {
                this.stopNow = true;
            }
        }

        private bool PageIsHtml(string pageAddress, ref LinkInfo li)
        {
            HttpWebResponse resp = null;
            bool isHtml = false;
            const string TypeHTML = "text/html";

            li.StatusCode = (HttpStatusCode)(-2);  // not html

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(pageAddress);

                if (this.noProxy)
                {
                    req.Proxy = new WebProxy();
                }

                req.Method = "HEAD";

                resp = (HttpWebResponse)req.GetResponse();

                WebHeaderCollection headers = resp.Headers;

                string contentType = headers["Content-type"];
                if (contentType != null)
                {
                    contentType = contentType.ToLower(CultureInfo.InvariantCulture);
                    if (contentType.StartsWith(TypeHTML))
                    {
                        isHtml = true;
                    }

                    if(contentType.IndexOf(";")>=0)
                    {
                        MatchCollection tagMatches = Regex.Matches(contentType,
                                                                "charset=(.+)$",
                                                                RegexOptions.IgnoreCase);
                        foreach (Match m in tagMatches)
                        {
                            if (m.Groups.Count>0 && m.Groups[1].Success)
                            {
                                string charset = m.Groups[1].Captures[0].ToString();
                                li.Charset = charset;
                                break;
                            }
                        }
                    }

                    // 해당주소의 컨텐츠타입
                    li.ContentType = contentType;
                }

                li.StatusCode = resp.StatusCode;
            }
            catch (WebException e)
            {
                string str = string.Format(CultureInfo.CurrentCulture,
                                        "Caught WebException: {0}",
                                        e.Status.ToString()); ;

                resp = (HttpWebResponse)e.Response;
                if (null != resp)
                {
                    li.StatusCode = resp.StatusCode;
                    str = string.Format(CultureInfo.CurrentCulture,
                                        "{0} ({1})",
                                        str, li.StatusCode);
                }
                else
                {
                    li.StatusCode = (HttpStatusCode)(-1);
                }

                if (CurrentPageEvent != null)
                {
                    CurrentPageEvent(this, new CurrentPageEventArgs(str));
                }
            }
            catch (NotSupportedException)
            {
                li.StatusCode = (HttpStatusCode)(-1);
            }
            finally
            {
                if (null != resp)
                {
                    resp.Close();
                }
            }

            return isHtml;
        }

        private HttpStatusCode GetPageData(ref Uri pageUri,
                                        out string pageData,
                                        LinkInfo linkInfo)
        {
            HttpStatusCode status = (HttpStatusCode)0;
            HttpWebResponse resp = null;

            pageData = "";

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(pageUri);

                if (this.noProxy)
                {
                    req.Proxy = new WebProxy();
                }

                resp = (HttpWebResponse)req.GetResponse();
                pageUri = resp.ResponseUri;

                StreamReader sr = new StreamReader(resp.GetResponseStream());
                pageData = sr.ReadToEnd();
                sr.Close();

                status = resp.StatusCode;

                if (CurrentPageEvent != null)
                {
                    linkInfo.StatusCode = status;
                    linkInfo.Html = pageData;
                    CurrentPageEvent(this, new CurrentPageEventArgs(linkInfo));
                }
            }
            catch (WebException e)
            {
                string str = string.Format(CultureInfo.CurrentCulture,
                                        "Caught WebException: {0}",
                                        e.Status.ToString()); ;

                resp = (HttpWebResponse)e.Response;
                if (null != resp)
                {
                    status = resp.StatusCode;
                    str = string.Format(CultureInfo.CurrentCulture,
                                        "{0} ({1})",
                                        str, status);
                }
                else
                {
                    status = (HttpStatusCode)(-1);
                }

                if (CurrentPageEvent != null)
                {
                    CurrentPageEvent(this, new CurrentPageEventArgs(str));
                }
            }
            finally
            {
                if (null != resp)
                {
                    resp.Close();
                }
            }

            return status;
        }

        private void GetPageLinks(Uri pageUri,
                                string pageBody,
                                string tag,
                                string attribute,
                                Hashtable links)
        {
            string tagPattern = string.Format(CultureInfo.InvariantCulture,
                                            "< *{0} +[^<>]*>",
                                            tag);
            string attributePattern = string.Format(CultureInfo.InvariantCulture,
                                                    "{0} *= *([^ >])*",
                                                    attribute);

            pageBody = Regex.Replace(pageBody,
                                    "(\\n|\\r|\\t)",
                                    " ");

            MatchCollection tagMatches = Regex.Matches(pageBody,
                                                    tagPattern,
                                                    RegexOptions.IgnoreCase);
            foreach (Match m in tagMatches)
            {
                if (m.Groups[0].Success)
                {
                    string tagData = m.Groups[0].Captures[0].ToString();
                    Match attributeMatch = Regex.Match(tagData,
                                                    attributePattern,
                                                    RegexOptions.IgnoreCase);
                    if (attributeMatch.Groups[0].Success)
                    {
                        string link = attributeMatch.Groups[0].Captures[0].ToString();
                        link = Regex.Replace(link, "[ \"']", "");
                        link = link.Substring(attribute.Length + 1);

                        try
                        {
                            link = new Uri(pageUri, link).AbsoluteUri;
                            links.Add(link, new LinkInfo(link,
                                    (HttpStatusCode)0));

                            if (null != PageFoundEvent)
                            {
                                PageFoundEvent(this, EventArgs.Empty);
                            }
                        }
                        catch (IndexOutOfRangeException)
                        { }
                        catch (UriFormatException)
                        { }
                        catch (ArgumentException)
                        { }
                    }
                }
            }

        }

        private void Crawl()
        {
            Hashtable links = null;

            try
            {
                links = new Hashtable(StringComparer.InvariantCultureIgnoreCase);

                if (-1 == this.startingPage.IndexOf("://"))
                {
                    this.startingPage = string.Format(CultureInfo.InvariantCulture,
                                                    "http://{0}",
                                                    this.startingPage);
                }

                int currentDepth = 1;
                links.Add(this.startingPage, new LinkInfo(this.startingPage,
                                                        (HttpStatusCode)0));
                while (!this.stopNow)
                {
                    Hashtable found =
                        new Hashtable(StringComparer.InvariantCultureIgnoreCase);

                    #region 링크추출을 일련작업
                    foreach (string page in links.Keys)
                    {
                        if (!this.isOutboundSearch && page.IndexOf(startingPage) < 0) continue;

                        if (this.stopNow)
                        {
                            continue;
                        }

                        if (CurrentPageEvent != null)
                        {
                            CurrentPageEvent(this,
                                            new CurrentPageEventArgs(page));
                        }

                        #region 링크추출
                        LinkInfo li = (LinkInfo)links[page];
                        try
                        {
                            HttpStatusCode currentStatus = li.StatusCode;

                            if (((HttpStatusCode)0 == currentStatus) && PageIsHtml(page, ref li))
                            {
                                Uri pageUri = new Uri(page);
                                string pageData = "";
                                currentStatus = GetPageData(ref pageUri, out pageData, li);

                                if (HttpStatusCode.OK == currentStatus)
                                {
                                    #region 링크추출
                                    // <a href=
                                    GetPageLinks(pageUri,
                                                pageData,
                                                "a",
                                                "href",
                                                found);
                                    // <frame src=
                                    GetPageLinks(pageUri,
                                                pageData,
                                                "frame",
                                                "src",
                                                found);
                                    // <area href=
                                    GetPageLinks(pageUri,
                                                pageData,
                                                "area",
                                                "href",
                                                found);
                                    // <link href=
                                    GetPageLinks(pageUri,
                                                pageData,
                                                "link",
                                                "href",
                                                found);
                                    #endregion
                                }
                            }
                            else
                            { }
                            li.StatusCode = currentStatus;
                        }
                        catch (UriFormatException)
                        {
                            #region Error
                            li.StatusCode = (HttpStatusCode)(-1);

                            if (CurrentPageEvent != null)
                            {
                                String message = String.Format(
                                    CultureInfo.CurrentCulture,
                                    "Unable to crawl {0} (UriFormatException)",
                                    page);
                                CurrentPageEvent(this,
                                                new CurrentPageEventArgs(message));
                            }
                            #endregion
                        }
                        #endregion
                    }  // foreach 
                    #endregion

                    // 링크가 없다면
                    if (0 == found.Count)
                    {
                        // 멈춤
                        lock (this)
                        {
                            this.stopNow = true;
                        }
                        continue;
                    }

                    foreach (string page in found.Keys)
                    {
                        if (!links.ContainsKey(page))
                        {
                            LinkInfo lk = (LinkInfo)found[page];
                            lk.DirDepth = currentDepth;
                            links.Add(page, lk);
                        }
                    }

                    // 깊이 +1, 초과면 중지
                    if (this.maxDepth < currentDepth)
                    {
                        lock (this)
                        {
                            this.stopNow = true;
                        }
                        continue;
                    }
                    currentDepth += 1;
                }

            }
            catch (OutOfMemoryException)
            {
                links = null;

                lock (this)
                {
                    this.stopNow = true;
                }

                if (null != CurrentPageEvent)
                {
                    CurrentPageEvent(this,
                                    new CurrentPageEventArgs(
                                        "Crawl halted: out of memory"));
                }

            }
            catch (Exception e)
            {
                lock (this)
                {
                    this.stopNow = true;
                }

                if (null != CurrentPageEvent)
                {
                    string message = string.Format(CultureInfo.CurrentCulture,
                                                "Crawl halted: {0} - {1}",
                                                e.ToString(),
                                                e.Message);
                    CurrentPageEvent(this,
                                    new CurrentPageEventArgs(message));
                }
            }

            if (null != CrawlFinishedEvent)
            {
                CrawlFinishedEvent(this, EventArgs.Empty);
            }
        }
    }

    public class CurrentPageEventArgs : EventArgs
    {
        private int errorNo = 0;
        private string pageAddressValue;
        private LinkInfo linkInfo;

        public string PageAddress
        {
            get
            {
                return this.pageAddressValue;
            }
        }

        public LinkInfo LinkInfo
        {
            get { return linkInfo; }
            set { this.linkInfo = value; }
        }

        public int ErrorNo
        {
            get { return errorNo; }
            set { this.errorNo = value; }
        }

        public CurrentPageEventArgs(string page)
        {
            this.pageAddressValue = page;
        }


        public CurrentPageEventArgs(int errorNo, string page) : this(page)
        {
            this.errorNo = errorNo;
        }

        public CurrentPageEventArgs(LinkInfo linkInfo)
        {
            this.linkInfo = linkInfo;
        }
    }
}
