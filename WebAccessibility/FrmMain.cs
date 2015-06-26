using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DockWindow;
using System.Net;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Threading;
using Cloud9.Parser.Html;
using Cloud9.Threading;
using Cloud9.WebAccessibility;
using Cloud9.Checker;

namespace WebAccessibility
{
    public partial class FrmMain : Form
    {
        private Hashtable m_linkList = new Hashtable(); // 링크정보
        private string m_startUrl = "";                                     // 시작주소
        private bool m_noProxy = false;                                     // 프록시
        private int m_linkCount = 0;

        #region Of Thread
        private int[] m_stats;
        private TimeSpan m_refreshInterval;
        private DateTime m_nextRefreshTime;
        private WorkQueue m_work;
        #endregion

        #region Of Window
        private FrmLeftDock frmLeftDock = new FrmLeftDock();
        private FrmCenterMain frmCenterMain = new FrmCenterMain();
        private FrmCenterReport frmCenterReport = new FrmCenterReport();
        #endregion

        #region Of Init
        public FrmMain()
        {
            InitializeComponent();

            ReadSettings();

            InitApplication();

            InitWindow();            
        }

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            lock (m_work)
            {
                m_work.Pause();
                m_work.Clear();
            }

            base.Dispose(disposing);
        }

        private void InitApplication()
        {
            #region Threading
            m_nextRefreshTime = DateTime.Now;
            m_refreshInterval = TimeSpan.FromSeconds(0.20);
            m_stats = new int[6];

            m_work = new WorkQueue();
            ((WorkThreadPool)m_work.WorkerPool).MaxThreads = 10;
            m_work.ConcurrentLimit = 10;
            m_work.AllWorkCompleted += new EventHandler(m_work_AllWorkCompleted);
            m_work.WorkerException += new ResourceExceptionEventHandler(m_work_WorkerException);
            m_work.ChangedWorkItemState += new ChangedWorkItemStateEventHandler(m_work_ChangedWorkItemState);

            Console.WriteLine("MinThread: " + ((WorkThreadPool)m_work.WorkerPool).MinThreads.ToString());
            Console.WriteLine("MaxThread: " + ((WorkThreadPool)m_work.WorkerPool).MaxThreads.ToString());
            Console.WriteLine("ConCurrent Limit: " + m_work.ConcurrentLimit.ToString());
            #endregion
        }

        private void ReadSettings()
        {

        }

        private void InitWindow()
        {
            // Dock 스타일 지정
            DockManager.FastMoveDraw = false;
            DockManager.Style = DockVisualStyle.VS2005;

            // 왼쪽윈도우 생성
            frmLeftDock.AllowClose = false;
            dockManager.DockWindow(frmLeftDock, DockStyle.Left);

            // 가운데 윈도우 생성
            frmCenterMain.AllowUnDock= frmCenterReport.AllowUnDock = false;
            frmCenterMain.AllowClose = frmCenterReport.AllowClose = false;

            dockManager.DockWindow(frmCenterMain, DockStyle.Fill);
            DockContainer cont = frmCenterMain.HostContainer;
            cont.DockWindow(frmCenterReport, DockStyle.Fill);
            ////frmCenterMain.Focus();

            ListView.CheckForIllegalCrossThreadCalls = false;
        }
        #endregion


        #region Threading
        private void m_work_AllWorkCompleted(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler(m_work_AllWorkCompleted), new object[] { sender, e });
            }
            else
            {
                btnScan.Image = Image.FromFile(Application.StartupPath + @"\res\Run.ico");
            }
        }

        private void m_work_ChangedWorkItemState(object sender, ChangedWorkItemStateEventArgs e)
        {
            lock (this)
            {
                m_stats[(int)e.PreviousState] -= 1;
                m_stats[(int)e.WorkItem.State] += 1;
            }

            if (DateTime.Now > m_nextRefreshTime)
            {
                //RefreshCounts();
                m_nextRefreshTime = DateTime.Now + m_refreshInterval;
            }

        }


        private void m_work_WorkerException(object sender, ResourceExceptionEventArgs e)
        {
            Application.OnThreadException(e.Exception);
        }
        #endregion


        private void btnScan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbAddress.Text))
            {
                //timerApp.Enabled = false;
                btnScan.Tag = false;
                btnScan.Enabled = true;
                MessageBox.Show("주소를 입력해주세요.");
                cmbAddress.Focus();
                return;
            }
            // 가져올URL 체크
            if (cmbAddress.Text.Trim().Length < 7)
            {
                //timerApp.Enabled = false;
                btnScan.Tag = false;
                btnScan.Enabled = true;
                MessageBox.Show("가져올 URL을 정확히 입력하세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbAddress.Focus();
                return;
            }

            frmLeftDock.trvLinks.Nodes.Clear();
            

            m_linkList.Clear();
            m_noProxy = btnProxyYN.Checked;
            m_startUrl = cmbAddress.Text.Trim();

            m_work.Pause();
            m_work.Clear();

            new Thread(new ThreadStart(StartScan)).Start();

            SetStatusText("Crawler 시작!!");
        }

        /// <summary>
        /// 상태바 레이블 출력
        /// </summary>
        /// <param name="msg"></param>
        private void SetStatusText(string msg)
        {
            statusLabel.Text = msg;
        }


        #region working

        private void StartScan()
        {
            string pageData = "";
            HttpStatusCode status = AppUtil.GetPageData(m_startUrl, false, out pageData);
            if (status == HttpStatusCode.OK)
            {
                frmCenterMain.lstList.Items.Add(m_startUrl);

                CHtmlDocument doc = new CHtmlDocument(pageData);
                doc.HRef = m_startUrl;
                RoleChecker.getInstance().Start(doc);
            }

            PrintReport();
        }

        private void PrintReport()
        {
            string[] files = Directory.GetFiles(@"c:\temp\log");
            frmCenterReport.lstReport.SuspendLayout();
            foreach(string file in files)
            {
                FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(stream);
                while(sr.Peek() > 0)
                {
                    string []items = sr.ReadLine().Split('\t');
                    if(items.Length == 3)
                    {
                        ListViewItem item = frmCenterReport.lstReport.Items.Add(items[0]);
                        item.SubItems.Add(items[1]);
                        item.SubItems.Add(items[2]);
                    }
                }
            }
            frmCenterReport.lstReport.ResumeLayout();
        }

        /// <summary>
        /// 주소 리스트를 바탕으로 트리 새로 갱신, 사용할 일이 없음
        /// </summary>
        private void RefreshTree()
        {
            if(m_linkList.Count > 0)
            {
                //frmLeftDock.trvLinks.Nodes.Clear();

                frmLeftDock.trvLinks.SuspendLayout();
                foreach(DictionaryEntry de in m_linkList)
                {
                    //LinkInfo linkInfo = de.Value as LinkInfo;
                    string sPath = de.Key.ToString();
                    //if (linkInfo. > m_config.General.MaxDepth) return;      // 지정뎁스까지 검색
                    if (sPath.IndexOf(m_startUrl) < 0) return;          // 타 사이트는 검색에서 제외
                    if (sPath.IndexOf("#") >= 0) sPath = sPath.Substring(0, sPath.IndexOf("#"));

                    Uri uri = new Uri(sPath);
                    AddTreeItem(null, uri);                    
                }

                frmLeftDock.trvLinks.ResumeLayout();
            }
        }

        /*
        private void StartCheck()
        {
            if (m_linkList.Count > 0)
            {
                btnScan.Image = Image.FromFile(Application.StartupPath + @"\res\Pause.ico");

                int depth = 1;
                string sHtml = "";

                foreach (DictionaryEntry de in m_linkList)
                {
                    LinkInfo linkInfo = de.Value as LinkInfo;
                    string sPath = linkInfo.LocalPath;

                    if (linkInfo.StatusCode == HttpStatusCode.OK)
                    {
                        sHtml = AppUtil.FileRead(linkInfo.LocalPath);
                        if (string.IsNullOrEmpty(sHtml)) continue;

                        // 분석은 스레드 최대 10개까지 생성하여 맡긴다.
                        lock (m_work)
                        {
                            RoleChecker checker = new RoleChecker(sPath, new CHtmlDocument(sHtml));
                            checker.Depth = depth;      ///  ?
                            m_work.Add(checker);
                            
                        }
                    }
                }

                //m_work.Resume();
            }
        }
    */
        #endregion


        #region Of TreeView
        /// <summary>
        /// 동일 깊이에서 동일 텍스트의 노드를 검색
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private TreeNode FindNodeByText(TreeNodeCollection nodes, string text)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Text.Trim().ToLower() == text.Trim().ToLower())
                {
                    return node;
                }
            }
            return nodes.Add(text); ;
        }
        
        /// <summary>
        /// Uri 세그먼트를 바탕으로 트리컨트롤에 등록
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="uri"></param>
        /// <param name="depth"></param>
        private void AddTreeItem(TreeNode parentNode, Uri uri)
        {
            if (parentNode == null)
            {
                string domain = uri.Scheme + "://" + uri.Host + (uri.IsDefaultPort ? "" : ":" + uri.Port.ToString()) + "/";
                parentNode = FindNodeByText(frmLeftDock.trvLinks.Nodes, domain);
            }
            //if (depth >= uri.Segments.Length) return;
            int level = parentNode.Level + 1;
            if (level >= uri.Segments.Length) return;

            string searchText = "";
            if (level == uri.Segments.Length - 1)
                searchText = uri.Segments[uri.Segments.Length - 1] + (!string.IsNullOrEmpty(uri.Query) ? uri.Query : "");
            else
                searchText = uri.Segments[level];

            TreeNode findNode = FindNodeByText(parentNode.Nodes, searchText);

            // 재귀호출
            AddTreeItem(findNode, uri);
        }
        #endregion

        private void timerApp_Tick(object sender, EventArgs e)
        {

        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            //StartCheck();
        }

    }
}