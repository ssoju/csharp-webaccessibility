namespace WebAccessibility
{
    partial class FrmCenterReport
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstReport = new System.Windows.Forms.ListView();
            this.URL = new System.Windows.Forms.ColumnHeader();
            this.Tag = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // lstReport
            // 
            this.lstReport.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.URL,
            this.Tag,
            this.columnHeader1});
            this.lstReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstReport.FullRowSelect = true;
            this.lstReport.Location = new System.Drawing.Point(0, 0);
            this.lstReport.Name = "lstReport";
            this.lstReport.Size = new System.Drawing.Size(256, 228);
            this.lstReport.TabIndex = 0;
            this.lstReport.UseCompatibleStateImageBehavior = false;
            this.lstReport.View = System.Windows.Forms.View.Details;
            this.lstReport.SelectedIndexChanged += new System.EventHandler(this.lstReport_SelectedIndexChanged);
            // 
            // URL
            // 
            this.URL.Text = "Url";
            this.URL.Width = 285;
            // 
            // Tag
            // 
            this.Tag.Text = "Tag";
            this.Tag.Width = 150;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "에러 내용";
            // 
            // FrmCenterReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 228);
            this.Controls.Add(this.lstReport);
            this.DockType = DockWindow.DockContainerType.Document;
            this.IsVisible = true;
            this.Name = "FrmCenterReport";
            this.Text = "보고서";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ListView lstReport;
        private System.Windows.Forms.ColumnHeader URL;
        private System.Windows.Forms.ColumnHeader Tag;
        private System.Windows.Forms.ColumnHeader columnHeader1;

    }
}