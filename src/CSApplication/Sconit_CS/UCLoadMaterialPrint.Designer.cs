namespace Sconit_CS
{
    partial class UCLoadMaterialPrint
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
            this.gbWO = new System.Windows.Forms.GroupBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.tbWO = new System.Windows.Forms.TextBox();
            this.lblWO = new System.Windows.Forms.Label();
            this.gbWO.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbWO
            // 
            this.gbWO.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbWO.Controls.Add(this.btnPrint);
            this.gbWO.Controls.Add(this.lblMessage);
            this.gbWO.Controls.Add(this.tbWO);
            this.gbWO.Controls.Add(this.lblWO);
            this.gbWO.Location = new System.Drawing.Point(130, 18);
            this.gbWO.Name = "gbWO";
            this.gbWO.Size = new System.Drawing.Size(755, 64);
            this.gbWO.TabIndex = 1;
            this.gbWO.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPrint.Location = new System.Drawing.Point(652, 14);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(88, 45);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "打  印";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMessage.ForeColor = System.Drawing.Color.Black;
            this.lblMessage.Location = new System.Drawing.Point(473, 24);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(95, 27);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "message";
            // 
            // tbWO
            // 
            this.tbWO.Font = new System.Drawing.Font("Arial", 20F);
            this.tbWO.Location = new System.Drawing.Point(89, 17);
            this.tbWO.MaxLength = 50;
            this.tbWO.Name = "tbWO";
            this.tbWO.Size = new System.Drawing.Size(380, 38);
            this.tbWO.TabIndex = 1;
            this.tbWO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbWO_KeyPress);
            // 
            // lblWO
            // 
            this.lblWO.AutoSize = true;
            this.lblWO.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWO.Location = new System.Drawing.Point(8, 24);
            this.lblWO.Name = "lblWO";
            this.lblWO.Size = new System.Drawing.Size(77, 27);
            this.lblWO.TabIndex = 0;
            this.lblWO.Text = "订单号:";
            // 
            // UCLoadMaterialPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbWO);
            this.Name = "UCLoadMaterialPrint";
            this.Size = new System.Drawing.Size(900, 500);
            this.Load += new System.EventHandler(this.UCLoadMaterialPrint_Load);
            this.gbWO.ResumeLayout(false);
            this.gbWO.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbWO;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox tbWO;
        private System.Windows.Forms.Label lblWO;

    }
}
