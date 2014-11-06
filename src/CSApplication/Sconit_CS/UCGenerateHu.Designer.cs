namespace Sconit_CS
{
    partial class UCGenerateHu
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
            this.lblItem = new System.Windows.Forms.Label();
            this.tbItem = new System.Windows.Forms.TextBox();
            this.lblParty = new System.Windows.Forms.Label();
            this.lblCAT = new System.Windows.Forms.Label();
            this.lblHUE = new System.Windows.Forms.Label();
            this.lblLot = new System.Windows.Forms.Label();
            this.btnGenerateHu = new System.Windows.Forms.Button();
            this.tbSupplier = new System.Windows.Forms.TextBox();
            this.tbHUE = new System.Windows.Forms.TextBox();
            this.tbCAT = new System.Windows.Forms.TextBox();
            this.tbLot = new System.Windows.Forms.TextBox();
            this.gbGenerateHu = new System.Windows.Forms.GroupBox();
            this.lblQty = new System.Windows.Forms.Label();
            this.tbQty = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.gbGenerateHu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblItem
            // 
            this.lblItem.AutoSize = true;
            this.lblItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.lblItem.Location = new System.Drawing.Point(21, 29);
            this.lblItem.Name = "lblItem";
            this.lblItem.Size = new System.Drawing.Size(81, 25);
            this.lblItem.TabIndex = 1;
            this.lblItem.Text = "物料号:";
            // 
            // tbItem
            // 
            this.tbItem.Font = new System.Drawing.Font("Arial", 20F);
            this.tbItem.Location = new System.Drawing.Point(108, 20);
            this.tbItem.MaxLength = 20;
            this.tbItem.Name = "tbItem";
            this.tbItem.Size = new System.Drawing.Size(212, 38);
            this.tbItem.TabIndex = 2;
            this.tbItem.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbItem_KeyUp);
            // 
            // lblParty
            // 
            this.lblParty.AutoSize = true;
            this.lblParty.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.lblParty.Location = new System.Drawing.Point(21, 72);
            this.lblParty.Name = "lblParty";
            this.lblParty.Size = new System.Drawing.Size(81, 25);
            this.lblParty.TabIndex = 3;
            this.lblParty.Text = "供应商:";
            // 
            // lblCAT
            // 
            this.lblCAT.AutoSize = true;
            this.lblCAT.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.lblCAT.Location = new System.Drawing.Point(7, 117);
            this.lblCAT.Name = "lblCAT";
            this.lblCAT.Size = new System.Drawing.Size(102, 25);
            this.lblCAT.TabIndex = 7;
            this.lblCAT.Text = "光强等级:";
            // 
            // lblHUE
            // 
            this.lblHUE.AutoSize = true;
            this.lblHUE.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.lblHUE.Location = new System.Drawing.Point(322, 116);
            this.lblHUE.Name = "lblHUE";
            this.lblHUE.Size = new System.Drawing.Size(102, 25);
            this.lblHUE.TabIndex = 9;
            this.lblHUE.Text = "色度等级:";
            // 
            // lblLot
            // 
            this.lblLot.AutoSize = true;
            this.lblLot.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.lblLot.Location = new System.Drawing.Point(360, 72);
            this.lblLot.Name = "lblLot";
            this.lblLot.Size = new System.Drawing.Size(60, 25);
            this.lblLot.TabIndex = 5;
            this.lblLot.Text = "批号:";
            // 
            // btnGenerateHu
            // 
            this.btnGenerateHu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateHu.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.btnGenerateHu.Location = new System.Drawing.Point(649, 97);
            this.btnGenerateHu.Name = "btnGenerateHu";
            this.btnGenerateHu.Size = new System.Drawing.Size(118, 45);
            this.btnGenerateHu.TabIndex = 11;
            this.btnGenerateHu.Text = "生成条码";
            this.btnGenerateHu.UseVisualStyleBackColor = true;
            this.btnGenerateHu.Click += new System.EventHandler(this.btnGenerateHu_Click);
            // 
            // tbSupplier
            // 
            this.tbSupplier.Font = new System.Drawing.Font("Arial", 20F);
            this.tbSupplier.Location = new System.Drawing.Point(108, 64);
            this.tbSupplier.MaxLength = 20;
            this.tbSupplier.Name = "tbSupplier";
            this.tbSupplier.Size = new System.Drawing.Size(212, 38);
            this.tbSupplier.TabIndex = 4;
            // 
            // tbHUE
            // 
            this.tbHUE.Font = new System.Drawing.Font("Arial", 20F);
            this.tbHUE.Location = new System.Drawing.Point(418, 105);
            this.tbHUE.MaxLength = 20;
            this.tbHUE.Name = "tbHUE";
            this.tbHUE.Size = new System.Drawing.Size(212, 38);
            this.tbHUE.TabIndex = 10;
            // 
            // tbCAT
            // 
            this.tbCAT.Font = new System.Drawing.Font("Arial", 20F);
            this.tbCAT.Location = new System.Drawing.Point(108, 108);
            this.tbCAT.MaxLength = 20;
            this.tbCAT.Name = "tbCAT";
            this.tbCAT.Size = new System.Drawing.Size(212, 38);
            this.tbCAT.TabIndex = 8;
            // 
            // tbLot
            // 
            this.tbLot.Font = new System.Drawing.Font("Arial", 20F);
            this.tbLot.Location = new System.Drawing.Point(418, 61);
            this.tbLot.MaxLength = 20;
            this.tbLot.Name = "tbLot";
            this.tbLot.Size = new System.Drawing.Size(212, 38);
            this.tbLot.TabIndex = 6;
            // 
            // gbGenerateHu
            // 
            this.gbGenerateHu.Controls.Add(this.lblQty);
            this.gbGenerateHu.Controls.Add(this.tbQty);
            this.gbGenerateHu.Controls.Add(this.lblMessage);
            this.gbGenerateHu.Controls.Add(this.tbItem);
            this.gbGenerateHu.Controls.Add(this.tbLot);
            this.gbGenerateHu.Controls.Add(this.lblItem);
            this.gbGenerateHu.Controls.Add(this.tbCAT);
            this.gbGenerateHu.Controls.Add(this.lblParty);
            this.gbGenerateHu.Controls.Add(this.tbHUE);
            this.gbGenerateHu.Controls.Add(this.lblCAT);
            this.gbGenerateHu.Controls.Add(this.tbSupplier);
            this.gbGenerateHu.Controls.Add(this.lblHUE);
            this.gbGenerateHu.Controls.Add(this.btnGenerateHu);
            this.gbGenerateHu.Controls.Add(this.lblLot);
            this.gbGenerateHu.Location = new System.Drawing.Point(144, 14);
            this.gbGenerateHu.Name = "gbGenerateHu";
            this.gbGenerateHu.Size = new System.Drawing.Size(787, 186);
            this.gbGenerateHu.TabIndex = 12;
            this.gbGenerateHu.TabStop = false;
            // 
            // lblQty
            // 
            this.lblQty.AutoSize = true;
            this.lblQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.lblQty.Location = new System.Drawing.Point(358, 25);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(60, 25);
            this.lblQty.TabIndex = 14;
            this.lblQty.Text = "数量:";
            // 
            // tbQty
            // 
            this.tbQty.Font = new System.Drawing.Font("Arial", 20F);
            this.tbQty.Location = new System.Drawing.Point(418, 18);
            this.tbQty.MaxLength = 20;
            this.tbQty.Name = "tbQty";
            this.tbQty.Size = new System.Drawing.Size(212, 38);
            this.tbQty.TabIndex = 13;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(25, 155);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(77, 14);
            this.lblMessage.TabIndex = 12;
            this.lblMessage.Text = "lblMessage";
            this.lblMessage.Visible = false;
            // 
            // UCGenerateHu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbGenerateHu);
            this.Name = "UCGenerateHu";
            this.Size = new System.Drawing.Size(934, 500);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCGenerateHu_KeyUp);
            this.gbGenerateHu.ResumeLayout(false);
            this.gbGenerateHu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblItem;
        private System.Windows.Forms.TextBox tbItem;
        private System.Windows.Forms.Label lblParty;
        private System.Windows.Forms.Label lblCAT;
        private System.Windows.Forms.Label lblHUE;
        private System.Windows.Forms.Label lblLot;
        private System.Windows.Forms.Button btnGenerateHu;
        private System.Windows.Forms.TextBox tbSupplier;
        private System.Windows.Forms.TextBox tbHUE;
        private System.Windows.Forms.TextBox tbCAT;
        private System.Windows.Forms.TextBox tbLot;
        private System.Windows.Forms.GroupBox gbGenerateHu;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.TextBox tbQty;

    }
}
