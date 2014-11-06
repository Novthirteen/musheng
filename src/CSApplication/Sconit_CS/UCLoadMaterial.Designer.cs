namespace Sconit_CS
{
    partial class UCLoadMaterial
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbOut = new System.Windows.Forms.GroupBox();
            this.dgOutList = new System.Windows.Forms.DataGridView();
            this.gbPickUp = new System.Windows.Forms.GroupBox();
            this.btnLoadMaterial = new System.Windows.Forms.Button();
            this.tbBarCode = new System.Windows.Forms.TextBox();
            this.lblBarCode = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.gbIn = new System.Windows.Forms.GroupBox();
            this.dgInList = new System.Windows.Forms.DataGridView();
            this.inSeq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inItemDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inSortLevel1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inColorLevel1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inSortLevel2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inColorLevel2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outSeq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outHuId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outItemCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outItemDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outSortLevel1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outColorLevel1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outSortLevel2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outColorLevel2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbOut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOutList)).BeginInit();
            this.gbPickUp.SuspendLayout();
            this.gbIn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgInList)).BeginInit();
            this.SuspendLayout();
            // 
            // gbOut
            // 
            this.gbOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbOut.Controls.Add(this.dgOutList);
            this.gbOut.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbOut.Location = new System.Drawing.Point(9, 201);
            this.gbOut.Name = "gbOut";
            this.gbOut.Size = new System.Drawing.Size(1008, 299);
            this.gbOut.TabIndex = 4;
            this.gbOut.TabStop = false;
            this.gbOut.Text = "已上料";
            // 
            // dgOutList
            // 
            this.dgOutList.AllowUserToAddRows = false;
            this.dgOutList.AllowUserToDeleteRows = false;
            this.dgOutList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgOutList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgOutList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgOutList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.outSeq,
            this.outHuId,
            this.outItemCode,
            this.outItemDescription,
            this.outSortLevel1,
            this.outColorLevel1,
            this.outSortLevel2,
            this.outColorLevel2,
            this.outQty});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Format = "0.########";
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgOutList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgOutList.Location = new System.Drawing.Point(6, 28);
            this.dgOutList.Name = "dgOutList";
            this.dgOutList.ReadOnly = true;
            this.dgOutList.RowHeadersVisible = false;
            this.dgOutList.RowTemplate.Height = 23;
            this.dgOutList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgOutList.Size = new System.Drawing.Size(996, 265);
            this.dgOutList.TabIndex = 0;
            // 
            // gbPickUp
            // 
            this.gbPickUp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbPickUp.Controls.Add(this.btnLoadMaterial);
            this.gbPickUp.Controls.Add(this.tbBarCode);
            this.gbPickUp.Controls.Add(this.lblBarCode);
            this.gbPickUp.Controls.Add(this.lblMessage);
            this.gbPickUp.Location = new System.Drawing.Point(145, 3);
            this.gbPickUp.Name = "gbPickUp";
            this.gbPickUp.Size = new System.Drawing.Size(872, 79);
            this.gbPickUp.TabIndex = 5;
            this.gbPickUp.TabStop = false;
            // 
            // btnLoadMaterial
            // 
            this.btnLoadMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadMaterial.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadMaterial.Location = new System.Drawing.Point(770, 23);
            this.btnLoadMaterial.Name = "btnLoadMaterial";
            this.btnLoadMaterial.Size = new System.Drawing.Size(96, 41);
            this.btnLoadMaterial.TabIndex = 2;
            this.btnLoadMaterial.Text = "上  料";
            this.btnLoadMaterial.UseVisualStyleBackColor = true;
            this.btnLoadMaterial.Click += new System.EventHandler(this.btnLoadMaterial_Click);
            // 
            // tbBarCode
            // 
            this.tbBarCode.Font = new System.Drawing.Font("Arial", 20F);
            this.tbBarCode.Location = new System.Drawing.Point(82, 23);
            this.tbBarCode.MaxLength = 50;
            this.tbBarCode.Name = "tbBarCode";
            this.tbBarCode.Size = new System.Drawing.Size(382, 38);
            this.tbBarCode.TabIndex = 1;
            // 
            // lblBarCode
            // 
            this.lblBarCode.AutoSize = true;
            this.lblBarCode.Font = new System.Drawing.Font("Microsoft YaHei", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblBarCode.Location = new System.Drawing.Point(17, 29);
            this.lblBarCode.Name = "lblBarCode";
            this.lblBarCode.Size = new System.Drawing.Size(59, 28);
            this.lblBarCode.TabIndex = 0;
            this.lblBarCode.Text = "条码:";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMessage.Location = new System.Drawing.Point(470, 28);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(96, 27);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Message";
            // 
            // gbIn
            // 
            this.gbIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbIn.Controls.Add(this.dgInList);
            this.gbIn.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbIn.Location = new System.Drawing.Point(9, 87);
            this.gbIn.Name = "gbIn";
            this.gbIn.Size = new System.Drawing.Size(1008, 99);
            this.gbIn.TabIndex = 3;
            this.gbIn.TabStop = false;
            this.gbIn.Text = "待上料";
            // 
            // dgInList
            // 
            this.dgInList.AllowUserToAddRows = false;
            this.dgInList.AllowUserToDeleteRows = false;
            this.dgInList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgInList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgInList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgInList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.inSeq,
            this.inItemCode,
            this.inItemDescription,
            this.inSortLevel1,
            this.inColorLevel1,
            this.inSortLevel2,
            this.inColorLevel2});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.Format = "0.########";
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgInList.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgInList.Location = new System.Drawing.Point(6, 28);
            this.dgInList.Name = "dgInList";
            this.dgInList.ReadOnly = true;
            this.dgInList.RowHeadersVisible = false;
            this.dgInList.RowTemplate.Height = 23;
            this.dgInList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgInList.Size = new System.Drawing.Size(996, 65);
            this.dgInList.TabIndex = 0;
            // 
            // inSeq
            // 
            this.inSeq.DataPropertyName = "Position";
            this.inSeq.FillWeight = 20F;
            this.inSeq.HeaderText = "位号";
            this.inSeq.Name = "inSeq";
            this.inSeq.ReadOnly = true;
            // 
            // inItemCode
            // 
            this.inItemCode.DataPropertyName = "ItemCode";
            this.inItemCode.FillWeight = 50F;
            this.inItemCode.HeaderText = "物料号";
            this.inItemCode.Name = "inItemCode";
            this.inItemCode.ReadOnly = true;
            // 
            // inItemDescription
            // 
            this.inItemDescription.DataPropertyName = "ItemDescription";
            this.inItemDescription.FillWeight = 90F;
            this.inItemDescription.HeaderText = "描述";
            this.inItemDescription.Name = "inItemDescription";
            this.inItemDescription.ReadOnly = true;
            // 
            // inSortLevel1
            // 
            this.inSortLevel1.DataPropertyName = "SortLevel1";
            this.inSortLevel1.FillWeight = 30F;
            this.inSortLevel1.HeaderText = "分光1";
            this.inSortLevel1.Name = "inSortLevel1";
            this.inSortLevel1.ReadOnly = true;
            // 
            // inColorLevel1
            // 
            this.inColorLevel1.DataPropertyName = "ColorLevel1";
            this.inColorLevel1.FillWeight = 30F;
            this.inColorLevel1.HeaderText = "分色1";
            this.inColorLevel1.Name = "inColorLevel1";
            this.inColorLevel1.ReadOnly = true;
            // 
            // inSortLevel2
            // 
            this.inSortLevel2.DataPropertyName = "SortLevel2";
            this.inSortLevel2.FillWeight = 30F;
            this.inSortLevel2.HeaderText = "分光2";
            this.inSortLevel2.Name = "inSortLevel2";
            this.inSortLevel2.ReadOnly = true;
            // 
            // inColorLevel2
            // 
            this.inColorLevel2.DataPropertyName = "ColorLevel2";
            this.inColorLevel2.FillWeight = 30F;
            this.inColorLevel2.HeaderText = "分色2";
            this.inColorLevel2.Name = "inColorLevel2";
            this.inColorLevel2.ReadOnly = true;
            // 
            // outSeq
            // 
            this.outSeq.DataPropertyName = "Position";
            this.outSeq.FillWeight = 30F;
            this.outSeq.HeaderText = "位号";
            this.outSeq.Name = "outSeq";
            this.outSeq.ReadOnly = true;
            // 
            // outHuId
            // 
            this.outHuId.DataPropertyName = "HuId";
            this.outHuId.FillWeight = 70F;
            this.outHuId.HeaderText = "条码";
            this.outHuId.Name = "outHuId";
            this.outHuId.ReadOnly = true;
            // 
            // outItemCode
            // 
            this.outItemCode.DataPropertyName = "ItemCode";
            this.outItemCode.FillWeight = 50F;
            this.outItemCode.HeaderText = "物料号";
            this.outItemCode.Name = "outItemCode";
            this.outItemCode.ReadOnly = true;
            // 
            // outItemDescription
            // 
            this.outItemDescription.DataPropertyName = "ItemDescription";
            this.outItemDescription.FillWeight = 90F;
            this.outItemDescription.HeaderText = "描述";
            this.outItemDescription.Name = "outItemDescription";
            this.outItemDescription.ReadOnly = true;
            // 
            // outSortLevel1
            // 
            this.outSortLevel1.DataPropertyName = "SortLevel1";
            this.outSortLevel1.FillWeight = 30F;
            this.outSortLevel1.HeaderText = "分光1";
            this.outSortLevel1.Name = "outSortLevel1";
            this.outSortLevel1.ReadOnly = true;
            // 
            // outColorLevel1
            // 
            this.outColorLevel1.DataPropertyName = "ColorLevel1";
            this.outColorLevel1.FillWeight = 30F;
            this.outColorLevel1.HeaderText = "分色1";
            this.outColorLevel1.Name = "outColorLevel1";
            this.outColorLevel1.ReadOnly = true;
            // 
            // outSortLevel2
            // 
            this.outSortLevel2.DataPropertyName = "SortLevel2";
            this.outSortLevel2.FillWeight = 30F;
            this.outSortLevel2.HeaderText = "分光2";
            this.outSortLevel2.Name = "outSortLevel2";
            this.outSortLevel2.ReadOnly = true;
            // 
            // outColorLevel2
            // 
            this.outColorLevel2.DataPropertyName = "ColorLevel2";
            this.outColorLevel2.FillWeight = 30F;
            this.outColorLevel2.HeaderText = "分色2";
            this.outColorLevel2.Name = "outColorLevel2";
            this.outColorLevel2.ReadOnly = true;
            // 
            // outQty
            // 
            this.outQty.DataPropertyName = "Qty";
            dataGridViewCellStyle1.Format = "0.####";
            this.outQty.DefaultCellStyle = dataGridViewCellStyle1;
            this.outQty.FillWeight = 30F;
            this.outQty.HeaderText = "数量";
            this.outQty.Name = "outQty";
            this.outQty.ReadOnly = true;
            // 
            // UCLoadMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOut);
            this.Controls.Add(this.gbIn);
            this.Controls.Add(this.gbPickUp);
            this.Name = "UCLoadMaterial";
            this.Size = new System.Drawing.Size(1031, 503);
            this.gbOut.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgOutList)).EndInit();
            this.gbPickUp.ResumeLayout(false);
            this.gbPickUp.PerformLayout();
            this.gbIn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgInList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOut;
        private System.Windows.Forms.DataGridView dgOutList;
        private System.Windows.Forms.GroupBox gbPickUp;
        private System.Windows.Forms.Button btnLoadMaterial;
        private System.Windows.Forms.TextBox tbBarCode;
        private System.Windows.Forms.Label lblBarCode;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.GroupBox gbIn;
        private System.Windows.Forms.DataGridView dgInList;
        private System.Windows.Forms.DataGridViewTextBoxColumn outSeq;
        private System.Windows.Forms.DataGridViewTextBoxColumn outHuId;
        private System.Windows.Forms.DataGridViewTextBoxColumn outItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn outItemDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn outSortLevel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn outColorLevel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn outSortLevel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn outColorLevel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn outQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn inSeq;
        private System.Windows.Forms.DataGridViewTextBoxColumn inItemCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn inItemDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn inSortLevel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn inColorLevel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn inSortLevel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn inColorLevel2;
    }
}
