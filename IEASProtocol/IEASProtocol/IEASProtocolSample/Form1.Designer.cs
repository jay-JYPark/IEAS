namespace IEASProtocolSample
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.disconnectBtn = new System.Windows.Forms.Button();
            this.connectBtn = new System.Windows.Forms.Button();
            this.authentiCodeTextBox = new System.Windows.Forms.TextBox();
            this.gatewayPortTextBox = new System.Windows.Forms.TextBox();
            this.gatewayIPTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.alertCAPTestBtn = new System.Windows.Forms.Button();
            this.eventListView = new System.Windows.Forms.ListView();
            this.time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contents = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listClearBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "통합게이트웨이 IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "통합게이트웨이 Port";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.disconnectBtn);
            this.groupBox1.Controls.Add(this.connectBtn);
            this.groupBox1.Controls.Add(this.authentiCodeTextBox);
            this.groupBox1.Controls.Add(this.gatewayPortTextBox);
            this.groupBox1.Controls.Add(this.gatewayIPTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(732, 149);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "통합게이트웨이 연결 설정";
            // 
            // disconnectBtn
            // 
            this.disconnectBtn.Enabled = false;
            this.disconnectBtn.Location = new System.Drawing.Point(614, 92);
            this.disconnectBtn.Name = "disconnectBtn";
            this.disconnectBtn.Size = new System.Drawing.Size(92, 36);
            this.disconnectBtn.TabIndex = 7;
            this.disconnectBtn.Text = "연결종료";
            this.disconnectBtn.UseVisualStyleBackColor = true;
            this.disconnectBtn.Click += new System.EventHandler(this.disconnectBtn_Click);
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(614, 32);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(92, 36);
            this.connectBtn.TabIndex = 6;
            this.connectBtn.Text = "연결";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // authentiCodeTextBox
            // 
            this.authentiCodeTextBox.Location = new System.Drawing.Point(196, 107);
            this.authentiCodeTextBox.MaxLength = 32;
            this.authentiCodeTextBox.Name = "authentiCodeTextBox";
            this.authentiCodeTextBox.Size = new System.Drawing.Size(293, 21);
            this.authentiCodeTextBox.TabIndex = 5;
            this.authentiCodeTextBox.Text = "1619F74053AB4555FDF06A17D226F55E";
            // 
            // gatewayPortTextBox
            // 
            this.gatewayPortTextBox.Location = new System.Drawing.Point(196, 70);
            this.gatewayPortTextBox.MaxLength = 7;
            this.gatewayPortTextBox.Name = "gatewayPortTextBox";
            this.gatewayPortTextBox.Size = new System.Drawing.Size(293, 21);
            this.gatewayPortTextBox.TabIndex = 4;
            this.gatewayPortTextBox.Text = "26750";
            this.gatewayPortTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gatewayPortTextBox_KeyPress);
            // 
            // gatewayIPTextBox
            // 
            this.gatewayIPTextBox.Location = new System.Drawing.Point(196, 32);
            this.gatewayIPTextBox.MaxLength = 15;
            this.gatewayIPTextBox.Name = "gatewayIPTextBox";
            this.gatewayIPTextBox.Size = new System.Drawing.Size(293, 21);
            this.gatewayIPTextBox.TabIndex = 3;
            this.gatewayIPTextBox.Text = "58.181.17.154";
            this.gatewayIPTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gatewayIPTextBox_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "인증 코드";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listClearBtn);
            this.groupBox2.Controls.Add(this.alertCAPTestBtn);
            this.groupBox2.Controls.Add(this.eventListView);
            this.groupBox2.Location = new System.Drawing.Point(12, 167);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(732, 361);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // alertCAPTestBtn
            // 
            this.alertCAPTestBtn.Location = new System.Drawing.Point(8, 305);
            this.alertCAPTestBtn.Name = "alertCAPTestBtn";
            this.alertCAPTestBtn.Size = new System.Drawing.Size(125, 36);
            this.alertCAPTestBtn.TabIndex = 7;
            this.alertCAPTestBtn.Text = "발령 CAP 시험";
            this.alertCAPTestBtn.UseVisualStyleBackColor = true;
            this.alertCAPTestBtn.Click += new System.EventHandler(this.alertCAPTestBtn_Click);
            // 
            // eventListView
            // 
            this.eventListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.time,
            this.contents});
            this.eventListView.GridLines = true;
            this.eventListView.Location = new System.Drawing.Point(6, 14);
            this.eventListView.Name = "eventListView";
            this.eventListView.Size = new System.Drawing.Size(720, 270);
            this.eventListView.TabIndex = 0;
            this.eventListView.UseCompatibleStateImageBehavior = false;
            this.eventListView.View = System.Windows.Forms.View.Details;
            // 
            // time
            // 
            this.time.Text = "Event Time";
            this.time.Width = 250;
            // 
            // contents
            // 
            this.contents.Text = "내용";
            this.contents.Width = 500;
            // 
            // listClearBtn
            // 
            this.listClearBtn.Location = new System.Drawing.Point(601, 305);
            this.listClearBtn.Name = "listClearBtn";
            this.listClearBtn.Size = new System.Drawing.Size(125, 36);
            this.listClearBtn.TabIndex = 8;
            this.listClearBtn.Text = "리스트 Clear";
            this.listClearBtn.UseVisualStyleBackColor = true;
            this.listClearBtn.Click += new System.EventHandler(this.listClearBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 540);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "IEASProtocolSample";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button disconnectBtn;
        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.TextBox authentiCodeTextBox;
        private System.Windows.Forms.TextBox gatewayPortTextBox;
        private System.Windows.Forms.TextBox gatewayIPTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView eventListView;
        private System.Windows.Forms.ColumnHeader time;
        private System.Windows.Forms.ColumnHeader contents;
        private System.Windows.Forms.Button alertCAPTestBtn;
        private System.Windows.Forms.Button listClearBtn;
    }
}

