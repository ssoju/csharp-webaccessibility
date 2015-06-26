using System;
using System.Net;

namespace Cloud9.WebCrawler
{
	/// <summary>
	/// Class describing a link
	/// </summary>
	public class LinkInfo
	{
		private string linkPathValue;
        private int dirDepth;
        private string localPath;

        public string LocalPath
        {
            get { return localPath; }
            set { localPath = value; }
        }

        public int DirDepth
        {
            get { return dirDepth; }
            set { dirDepth = value; }
        }
        private string html;

        public string Html
        {
            get { return html; }
            set { html = value; }
        }
        private HttpStatusCode statusCodeValue;

        public HttpStatusCode StatusCodeValue
        {
            get { return statusCodeValue; }
            set { statusCodeValue = value; }
        }
        private string contentType;

        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }
        private string charset;

        public string Charset
        {
            get { return charset; }
            set { charset = value; }
        }


		public string LinkPath
		{
			get
			{
				return this.linkPathValue;
			}
		}

		public HttpStatusCode StatusCode
		{
			get
			{
				return this.statusCodeValue;
			}

			set
			{
				this.statusCodeValue = value;
			}
		}

		public LinkInfo(string path, HttpStatusCode status)
		{
			this.linkPathValue = path;
			this.statusCodeValue = status;
		}

        public LinkInfo(string path, HttpStatusCode status, int dirDepth)
            : this(path, status)
        {
            this.dirDepth = dirDepth;
        }
	}
}
