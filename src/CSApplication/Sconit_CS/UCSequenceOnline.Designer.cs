namespace Sconit_CS
{
    partial class UCSequenceOnline
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbWODetail = new System.Windows.Forms.GroupBox();
            this.gvInProcessWO = new System.Windows.Forms.DataGridView();
            this.WONo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReqQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OnlineDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblInProcess = new System.Windows.Forms.Label();
            this.gvSubmitWO = new System.Windows.Forms.DataGridView();
            this.WONo1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDesc1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReqQty1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSubmit = new System.Windows.Forms.Label();
            this.tbFlow = new System.Windows.Forms.TextBox();
            this.gb = new System.Windows.Forms.GroupBox();
            this.lblWO = new System.Windows.Forms.Label();
            this.lblmessage = new System.Windows.Forms.Label();
            this.gbWODetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvInProcessWO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSubmitWO)).BeginInit();
            this.gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbWODetail
            // 
            this.gbWODetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbWODetail.Controls.Add(this.lblSubmit);
            this.gbWODetail.Controls.Add(this.gvInProcessWO);
            this.gbWODetail.Controls.Add(this.lblInProcess);
            this.gbWODetail.Controls.Add(this.gvSubmitWO);
            this.gbWODetail.Location = new System.Drawing.Point(17, 83);
            this.gbWODetail.Name = "gbWODetail";
            this.gbWODetail.Size = new System.Drawing.Size(994, 501);
            this.gbWODetail.TabIndex = 0;
            this.gbWODetail.TabStop = false;
            // 
            // gvInProcessWO
            // 
            this.gvInProcessWO.AllowUserToAddRows = false;
            this.gvInProcessWO.AllowUserToDeleteRows = false;
            this.gvInProcessWO.AllowUserToResizeRows = false;
            this.gvInProcessWO.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvInProcessWO.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvInProcessWO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvInProcessWO.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.WONo,
            this.ItemDesc,
            this.ReqQty,
            this.OnlineDate});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvInProcessWO.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvInProcessWO.Location = new System.Drawing.Point(14, 297);
            this.gvInProcessWO.Name = "gvInProcessWO";
            this.gvInProcessWO.ReadOnly = true;
            this.gvInProcessWO.RowHeadersVisible = false;
            this.gvInProcessWO.RowTemplate.Height = 40;
            this.gvInProcessWO.Size = new System.Drawing.Size(965, 198);
            this.gvInProcessWO.TabIndex = 3;
            // 
            // WONo
            // 
            this.WONo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.WONo.DataPropertyName = "OrderNo";
            this.WONo.HeaderText = "工单号";
            this.WONo.MinimumWidth = 40;
            this.WONo.Name = "WONo";
            this.WONo.ReadOnly = true;
            this.WONo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ItemDesc
            // 
            this.ItemDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemDesc.DataPropertyName = "ItemDescription";
            this.ItemDesc.HeaderText = "产品描述";
            this.ItemDesc.MinimumWidth = 240;
            this.ItemDesc.Name = "ItemDesc";
            this.ItemDesc.ReadOnly = true;
            this.ItemDesc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ReqQty
            // 
            this.ReqQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ReqQty.DataPropertyName = "OrderedQty";
            this.ReqQty.HeaderText = "需求数";
            this.ReqQty.MinimumWidth = 40;
            this.ReqQty.Name = "ReqQty";
            this.ReqQty.ReadOnly = true;
            this.ReqQty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // OnlineDate
            // 
            this.OnlineDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OnlineDate.DataPropertyName = "ReleaseDate";
            this.OnlineDate.HeaderText = "上线时间";
            this.OnlineDate.MinimumWidth = 120;
            this.OnlineDate.Name = "OnlineDate";
            this.OnlineDate.ReadOnly = true;
            this.OnlineDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // lblInProcess
            // 
            this.lblInProcess.AutoSize = true;
            this.lblInProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInProcess.Location = new System.Drawing.Point(9, 261);
            this.lblInProcess.Name = "lblInProcess";
            this.lblInProcess.Size = new System.Drawing.Size(117, 25);
            this.lblInProcess.TabIndex = 2;
            this.lblInProcess.Text = "已上线队列";
            // 
            // gvSubmitWO
            // 
            this.gvSubmitWO.AllowUserToAddRows = false;
            this.gvSubmitWO.AllowUserToDeleteRows = false;
            this.gvSubmitWO.AllowUserToResizeRows = false;
            this.gvSubmitWO.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvSubmitWO.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvSubmitWO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvSubmitWO.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.WONo1,
            this.ItemDesc1,
            this.ReqQty1});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvSubmitWO.DefaultCellStyle = dataGridViewCellStyle4;
            this.gvSubmitWO.Location = new System.Drawing.Point(14, 51);
            this.gvSubmitWO.Name = "gvSubmitWO";
            this.gvSubmitWO.ReadOnly = true;
            this.gvSubmitWO.RowHeadersVisible = false;
            this.gvSubmitWO.RowTemplate.Height = 40;
            this.gvSubmitWO.Size = new System.Drawing.Size(965, 198);
            this.gvSubmitWO.TabIndex = 0;
            // 
            // WONo1
            // 
            this.WONo1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.WONo1.DataPropertyName = "OrderNo";
            this.WONo1.HeaderText = "工单号";
            this.WONo1.MinimumWidth = 40;
            this.WONo1.Name = "WONo1";
            this.WONo1.ReadOnly = true;
            this.WONo1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ItemDesc1
            // 
            this.ItemDesc1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ItemDesc1.DataPropertyName = "ItemDescription";
            this.ItemDesc1.HeaderText = "产品描述";
            this.ItemDesc1.MinimumWidth = 360;
            this.ItemDesc1.Name = "ItemDesc1";
            this.ItemDesc1.ReadOnly = true;
            this.ItemDesc1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ReqQty1
            // 
            this.ReqQty1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ReqQty1.DataPropertyName = "OrderedQty";
            this.ReqQty1.HeaderText = "需求数";
            this.ReqQty1.MinimumWidth = 40;
            this.ReqQty1.Name = "ReqQty1";
            this.ReqQty1.ReadOnly = true;
            this.ReqQty1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // lblSubmit
            // 
            this.lblSubmit.AutoSize = true;
            this.lblSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubmit.Location = new System.Drawing.Point(9, 17);
            this.lblSubmit.Name = "lblSubmit";
            this.lblSubmit.Size = new System.Drawing.Size(117, 25);
            this.lblSubmit.TabIndex = 1;
            this.lblSubmit.Text = "待上线队列";
            // 
            // tbFlow
            // 
            this.tbFlow.Font = new System.Drawing.Font("Arial", 20F);
            this.tbFlow.Location = new System.Drawing.Point(123, 19);
            this.tbFlow.Name = "tbFlow";
            this.tbFlow.Size = new System.Drawing.Size(380, 38);
            this.tbFlow.TabIndex = 1;
            // 
            // gb
            // 
            this.gb.Controls.Add(this.lblmessage);
            this.gb.Controls.Add(this.lblWO);
            this.gb.Controls.Add(this.tbFlow);
            this.gb.Location = new System.Drawing.Point(121, 7);
            this.gb.Name = "gb";
            this.gb.Size = new System.Drawing.Size(890, 70);
            this.gb.TabIndex = 0;
            this.gb.TabStop = false;
            // 
            // lblWO
            // 
            this.lblWO.AutoSize = true;
            this.lblWO.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWO.Location = new System.Drawing.Point(7, 21);
            this.lblWO.Name = "lblWO";
            this.lblWO.Size = new System.Drawing.Size(110, 33);
            this.lblWO.TabIndex = 2;
            this.lblWO.Text = "生产线:";
            // 
            // lblmessage
            // 
            this.lblmessage.AutoSize = true;
            this.lblmessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblmessage.ForeColor = System.Drawing.Color.Black;
            this.lblmessage.Location = new System.Drawing.Point(509, 28);
            this.lblmessage.Name = "lblmessage";
            this.lblmessage.Size = new System.Drawing.Size(92, 25);
            this.lblmessage.TabIndex = 3;
            this.lblmessage.Text = "message";
            this.lblmessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UCSequenceOnline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gb);
            this.Controls.Add(this.gbWODetail);
            this.Name = "UCSequenceOnline";
            this.Size = new System.Drawing.Size(1024, 600);
            this.Load += new System.EventHandler(this.UCSequenceOnline_Load);
            this.gbWODetail.ResumeLayout(false);
            this.gbWODetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvInProcessWO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSubmitWO)).EndInit();
            this.gb.ResumeLayout(false);
            this.gb.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbWODetail;
        private System.Windows.Forms.DataGridView gvSubmitWO;
        private System.Windows.Forms.Label lblSubmit;
        private System.Windows.Forms.DataGridView gvInProcessWO;
        private System.Windows.Forms.DataGridViewTextBoxColumn WONo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReqQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn OnlineDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn WONo1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDesc1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReqQty1;
        private System.Windows.Forms.Label lblInProcess;
        private System.Windows.Forms.TextBox tbFlow;
        private System.Windows.Forms.GroupBox gb;
        private System.Windows.Forms.Label lblWO;
        private System.Windows.Forms.Label lblmessage;
    }
}
