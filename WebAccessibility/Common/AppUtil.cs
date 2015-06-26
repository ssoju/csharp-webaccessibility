using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;
using System.Globalization;

namespace Cloud9.WebAccessibility
{
    public abstract class AppUtil
    {




        /// <summary>
        /// 주소로 부터 태그를 읽어온다.
        /// </summary>
        /// <param name="sUrl"></param>
        /// <param name="pageData"></param>
        /// <returns></returns>
        public static HttpStatusCode GetPageData(string sUrl, bool noProxy, out string pageData)
        {

            HttpStatusCode status = (HttpStatusCode)0;
            HttpWebResponse resp = null;
            pageData = "";

            try
            {
                // url이 유효한지 체크
                if (string.IsNullOrEmpty(sUrl)) return status;
                if (sUrl.StartsWith("javascript") || sUrl.StartsWith("#") || sUrl.StartsWith("vbscript")) return status;
                if (!Uri.IsWellFormedUriString(sUrl, UriKind.Absolute)) return status;

                Uri pageUri = new Uri(sUrl);

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(pageUri);

                if (noProxy)
                {
                    req.Proxy = new WebProxy();
                }
                resp = (HttpWebResponse)req.GetResponse();
                if (!resp.Headers["Content-type"].StartsWith("text/html")) return ((HttpStatusCode)0);

                pageUri = resp.ResponseUri;
                using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                {
                    pageData = reader.ReadToEnd();
                    reader.Close();
                }
                status = resp.StatusCode;
            }
            catch (WebException e)
            {
                string str = string.Format(CultureInfo.CurrentCulture,
                                        "[경고] WebException: {0}",
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

        /// <summary>
        /// Uri 객체에서 페이지의 base href를 추출
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string MakeDocBaseHref(Uri uri)
        {
            string result = uri.Scheme + "://" + uri.Host + "/";
            for (int i = 1, len = uri.Segments.Length - 1; i < len; ++i)
                result += uri.Segments[i];

            return result;
        }

        /// <summary>
        /// 파일 읽기.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FileRead(string path)
        {
            StringBuilder sb = new StringBuilder();
            try {
                using(StreamReader sr = new StreamReader(path))
                {
                    while (sr.Peek() > 0)
                    {
                        sb.AppendLine(sr.ReadLine());
                    }
                }
            }
            catch(Exception e)
            {

            }
            return sb.ToString();
        }

        /// <summary>
        /// 파일 쓰기
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        public static bool FileWrite(string path, string text)
        {
            bool result = false;

            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(text);
                    sw.Close();
                }

                result = true;
            }
            catch(Exception)
            {

            }
            return result;
        }
    }
}
