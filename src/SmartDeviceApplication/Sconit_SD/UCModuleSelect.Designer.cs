namespace Sconit_SD
{
    partial class UCModuleSelect
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
            this.tabModuleSelect = new System.Windows.Forms.TabControl();
            this.tabProcurement = new System.Windows.Forms.TabPage();
            this.btnOrderQuick = new System.Windows.Forms.Button();
            this.btnReceiveReturn = new System.Windows.Forms.Button();
            this.btnReceive = new System.Windows.Forms.Button();
            this.tabProduction = new System.Windows.Forms.TabPage();
            this.btnReuse = new System.Windows.Forms.Button();
            this.btnReturnMaterial = new System.Windows.Forms.Button();
            this.btnForceReloadMaterial = new System.Windows.Forms.Button();
            this.btnReloadMaterial = new System.Windows.Forms.Button();
            this.btnLoadMaterial = new System.Windows.Forms.Button();
            this.btnOnline = new System.Windows.Forms.Button();
            this.tabDistribution = new System.Windows.Forms.TabPage();
            this.btnShipConfirm = new System.Windows.Forms.Button();
            this.btnShipReturn = new System.Windows.Forms.Button();
            this.btnPickListOnline = new System.Windows.Forms.Button();
            this.btnShipOrder = new System.Windows.Forms.Button();
            this.btnShip = new System.Windows.Forms.Button();
            this.btnPickList = new System.Windows.Forms.Button();
            this.tabInventory = new System.Windows.Forms.TabPage();
            this.btnHuStatus = new System.Windows.Forms.Button();
            this.btnStockTaking = new System.Windows.Forms.Button();
            this.btnDevanning = new System.Windows.Forms.Button();
            this.btnPutAway = new System.Windows.Forms.Button();
            this.btnPickUp = new System.Windows.Forms.Button();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.tabInspection = new System.Windows.Forms.TabPage();
            this.btnInspection = new System.Windows.Forms.Button();
            this.btnInspect = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.tabModuleSelect.SuspendLayout();
            this.tabProcurement.SuspendLayout();
            this.tabProduction.SuspendLayout();
            this.tabDistribution.SuspendLayout();
            this.tabInventory.SuspendLayout();
            this.tabInspection.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabModuleSelect
            // 
            this.tabModuleSelect.Controls.Add(this.tabProcurement);
            this.tabModuleSelect.Controls.Add(this.tabProduction);
            this.tabModuleSelect.Controls.Add(this.tabDistribution);
            this.tabModuleSelect.Controls.Add(this.tabInventory);
            this.tabModuleSelect.Controls.Add(this.tabInspection);
            this.tabModuleSelect.Location = new System.Drawing.Point(3, 13);
            this.tabModuleSelect.Name = "tabModuleSelect";
            this.tabModuleSelect.SelectedIndex = 0;
            this.tabModuleSelect.Size = new System.Drawing.Size(244, 256);
            this.tabModuleSelect.TabIndex = 4;
            this.tabModuleSelect.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // tabProcurement
            // 
            this.tabProcurement.Controls.Add(this.btnOrderQuick);
            this.tabProcurement.Controls.Add(this.btnReceiveReturn);
            this.tabProcurement.Controls.Add(this.btnReceive);
            this.tabProcurement.Location = new System.Drawing.Point(4, 25);
            this.tabProcurement.Name = "tabProcurement";
            this.tabProcurement.Size = new System.Drawing.Size(236, 227);
            this.tabProcurement.Text = "供货";
            // 
            // btnOrderQuick
            // 
            this.btnOrderQuick.Location = new System.Drawing.Point(19, 67);
            this.btnOrderQuick.Name = "btnOrderQuick";
            this.btnOrderQuick.Size = new System.Drawing.Size(84, 20);
            this.btnOrderQuick.TabIndex = 3;
            this.btnOrderQuick.Text = "3.快速收货";
            this.btnOrderQuick.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnOrderQuick.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnReceiveReturn
            // 
            this.btnReceiveReturn.Location = new System.Drawing.Point(125, 25);
            this.btnReceiveReturn.Name = "btnReceiveReturn";
            this.btnReceiveReturn.Size = new System.Drawing.Size(84, 20);
            this.btnReceiveReturn.TabIndex = 2;
            this.btnReceiveReturn.Text = "2.要货退货";
            this.btnReceiveReturn.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnReceiveReturn.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnReceive
            // 
            this.btnReceive.Location = new System.Drawing.Point(19, 25);
            this.btnReceive.Name = "btnReceive";
            this.btnReceive.Size = new System.Drawing.Size(84, 20);
            this.btnReceive.TabIndex = 1;
            this.btnReceive.Text = "1.收货";
            this.btnReceive.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnReceive.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // tabProduction
            // 
            this.tabProduction.Controls.Add(this.btnReuse);
            this.tabProduction.Controls.Add(this.btnReturnMaterial);
            this.tabProduction.Controls.Add(this.btnForceReloadMaterial);
            this.tabProduction.Controls.Add(this.btnReloadMaterial);
            this.tabProduction.Controls.Add(this.btnLoadMaterial);
            this.tabProduction.Controls.Add(this.btnOnline);
            this.tabProduction.Location = new System.Drawing.Point(4, 25);
            this.tabProduction.Name = "tabProduction";
            this.tabProduction.Size = new System.Drawing.Size(236, 227);
            this.tabProduction.Text = "生产";
            // 
            // btnReuse
            // 
            this.btnReuse.Location = new System.Drawing.Point(133, 105);
            this.btnReuse.Name = "btnReuse";
            this.btnReuse.Size = new System.Drawing.Size(84, 20);
            this.btnReuse.TabIndex = 6;
            this.btnReuse.Text = "6.材料回用";
            // 
            // btnReturnMaterial
            // 
            this.btnReturnMaterial.Location = new System.Drawing.Point(19, 105);
            this.btnReturnMaterial.Name = "btnReturnMaterial";
            this.btnReturnMaterial.Size = new System.Drawing.Size(84, 20);
            this.btnReturnMaterial.TabIndex = 5;
            this.btnReturnMaterial.Text = "5.退料";
            this.btnReturnMaterial.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnReturnMaterial.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnForceReloadMaterial
            // 
            this.btnForceReloadMaterial.Location = new System.Drawing.Point(19, 149);
            this.btnForceReloadMaterial.Name = "btnForceReloadMaterial";
            this.btnForceReloadMaterial.Size = new System.Drawing.Size(84, 20);
            this.btnForceReloadMaterial.TabIndex = 7;
            this.btnForceReloadMaterial.Text = "7.强制换料";
            this.btnForceReloadMaterial.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnForceReloadMaterial.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnReloadMaterial
            // 
            this.btnReloadMaterial.Location = new System.Drawing.Point(133, 65);
            this.btnReloadMaterial.Name = "btnReloadMaterial";
            this.btnReloadMaterial.Size = new System.Drawing.Size(84, 20);
            this.btnReloadMaterial.TabIndex = 4;
            this.btnReloadMaterial.Text = "4.换料";
            this.btnReloadMaterial.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnReloadMaterial.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnLoadMaterial
            // 
            this.btnLoadMaterial.Location = new System.Drawing.Point(19, 65);
            this.btnLoadMaterial.Name = "btnLoadMaterial";
            this.btnLoadMaterial.Size = new System.Drawing.Size(84, 20);
            this.btnLoadMaterial.TabIndex = 3;
            this.btnLoadMaterial.Text = "3.上料";
            this.btnLoadMaterial.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnLoadMaterial.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnOnline
            // 
            this.btnOnline.Location = new System.Drawing.Point(19, 25);
            this.btnOnline.Name = "btnOnline";
            this.btnOnline.Size = new System.Drawing.Size(84, 20);
            this.btnOnline.TabIndex = 1;
            this.btnOnline.Text = "1.上线";
            this.btnOnline.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnOnline.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // tabDistribution
            // 
            this.tabDistribution.Controls.Add(this.btnShipConfirm);
            this.tabDistribution.Controls.Add(this.btnShipReturn);
            this.tabDistribution.Controls.Add(this.btnPickListOnline);
            this.tabDistribution.Controls.Add(this.btnShipOrder);
            this.tabDistribution.Controls.Add(this.btnShip);
            this.tabDistribution.Controls.Add(this.btnPickList);
            this.tabDistribution.Location = new System.Drawing.Point(4, 25);
            this.tabDistribution.Name = "tabDistribution";
            this.tabDistribution.Size = new System.Drawing.Size(236, 227);
            this.tabDistribution.Text = "发货";
            // 
            // btnShipConfirm
            // 
            this.btnShipConfirm.Location = new System.Drawing.Point(125, 105);
            this.btnShipConfirm.Name = "btnShipConfirm";
            this.btnShipConfirm.Size = new System.Drawing.Size(84, 20);
            this.btnShipConfirm.TabIndex = 6;
            this.btnShipConfirm.Text = "6.发货确认";
            this.btnShipConfirm.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnShipConfirm.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnShipReturn
            // 
            this.btnShipReturn.Location = new System.Drawing.Point(19, 105);
            this.btnShipReturn.Name = "btnShipReturn";
            this.btnShipReturn.Size = new System.Drawing.Size(84, 20);
            this.btnShipReturn.TabIndex = 5;
            this.btnShipReturn.Text = "5.发货退货";
            this.btnShipReturn.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnShipReturn.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnPickListOnline
            // 
            this.btnPickListOnline.Location = new System.Drawing.Point(19, 25);
            this.btnPickListOnline.Name = "btnPickListOnline";
            this.btnPickListOnline.Size = new System.Drawing.Size(84, 20);
            this.btnPickListOnline.TabIndex = 1;
            this.btnPickListOnline.Text = "1.拣货上线";
            this.btnPickListOnline.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnPickListOnline.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnShipOrder
            // 
            this.btnShipOrder.Location = new System.Drawing.Point(125, 65);
            this.btnShipOrder.Name = "btnShipOrder";
            this.btnShipOrder.Size = new System.Drawing.Size(84, 20);
            this.btnShipOrder.TabIndex = 4;
            this.btnShipOrder.Text = "4.订单发货";
            this.btnShipOrder.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnShipOrder.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnShip
            // 
            this.btnShip.Location = new System.Drawing.Point(19, 65);
            this.btnShip.Name = "btnShip";
            this.btnShip.Size = new System.Drawing.Size(84, 20);
            this.btnShip.TabIndex = 3;
            this.btnShip.Text = "3.拣货发货";
            this.btnShip.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnShip.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnPickList
            // 
            this.btnPickList.Location = new System.Drawing.Point(125, 25);
            this.btnPickList.Name = "btnPickList";
            this.btnPickList.Size = new System.Drawing.Size(84, 20);
            this.btnPickList.TabIndex = 2;
            this.btnPickList.Text = "2.拣货";
            this.btnPickList.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnPickList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // tabInventory
            // 
            this.tabInventory.Controls.Add(this.btnHuStatus);
            this.tabInventory.Controls.Add(this.btnStockTaking);
            this.tabInventory.Controls.Add(this.btnDevanning);
            this.tabInventory.Controls.Add(this.btnPutAway);
            this.tabInventory.Controls.Add(this.btnPickUp);
            this.tabInventory.Controls.Add(this.btnTransfer);
            this.tabInventory.Location = new System.Drawing.Point(4, 25);
            this.tabInventory.Name = "tabInventory";
            this.tabInventory.Size = new System.Drawing.Size(236, 227);
            this.tabInventory.Text = "仓库";
            // 
            // btnHuStatus
            // 
            this.btnHuStatus.Location = new System.Drawing.Point(133, 106);
            this.btnHuStatus.Name = "btnHuStatus";
            this.btnHuStatus.Size = new System.Drawing.Size(84, 20);
            this.btnHuStatus.TabIndex = 6;
            this.btnHuStatus.Text = "6.条码状态";
            this.btnHuStatus.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnHuStatus.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnStockTaking
            // 
            this.btnStockTaking.Location = new System.Drawing.Point(19, 106);
            this.btnStockTaking.Name = "btnStockTaking";
            this.btnStockTaking.Size = new System.Drawing.Size(84, 20);
            this.btnStockTaking.TabIndex = 5;
            this.btnStockTaking.Text = "5.盘点";
            this.btnStockTaking.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnStockTaking.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnDevanning
            // 
            this.btnDevanning.Location = new System.Drawing.Point(133, 65);
            this.btnDevanning.Name = "btnDevanning";
            this.btnDevanning.Size = new System.Drawing.Size(84, 20);
            this.btnDevanning.TabIndex = 4;
            this.btnDevanning.Text = "4.拆箱";
            this.btnDevanning.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnDevanning.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnPutAway
            // 
            this.btnPutAway.Location = new System.Drawing.Point(133, 25);
            this.btnPutAway.Name = "btnPutAway";
            this.btnPutAway.Size = new System.Drawing.Size(84, 20);
            this.btnPutAway.TabIndex = 2;
            this.btnPutAway.Text = "2.上架";
            this.btnPutAway.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnPutAway.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnPickUp
            // 
            this.btnPickUp.Location = new System.Drawing.Point(19, 65);
            this.btnPickUp.Name = "btnPickUp";
            this.btnPickUp.Size = new System.Drawing.Size(84, 20);
            this.btnPickUp.TabIndex = 3;
            this.btnPickUp.Text = "3.下架";
            this.btnPickUp.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnPickUp.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnTransfer
            // 
            this.btnTransfer.Location = new System.Drawing.Point(19, 25);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(84, 20);
            this.btnTransfer.TabIndex = 1;
            this.btnTransfer.Text = "1.移库";
            this.btnTransfer.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnTransfer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // tabInspection
            // 
            this.tabInspection.Controls.Add(this.btnInspection);
            this.tabInspection.Controls.Add(this.btnInspect);
            this.tabInspection.Location = new System.Drawing.Point(4, 25);
            this.tabInspection.Name = "tabInspection";
            this.tabInspection.Size = new System.Drawing.Size(236, 227);
            this.tabInspection.Text = "质量";
            // 
            // btnInspection
            // 
            this.btnInspection.Location = new System.Drawing.Point(19, 25);
            this.btnInspection.Name = "btnInspection";
            this.btnInspection.Size = new System.Drawing.Size(72, 20);
            this.btnInspection.TabIndex = 1;
            this.btnInspection.Text = "1.报验";
            this.btnInspection.Click += new System.EventHandler(this.UCModuleSelect_Click);
            this.btnInspection.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            // 
            // btnInspect
            // 
            this.btnInspect.Location = new System.Drawing.Point(125, 25);
            this.btnInspect.Name = "btnInspect";
            this.btnInspect.Size = new System.Drawing.Size(72, 20);
            this.btnInspect.TabIndex = 2;
            this.btnInspect.Text = "2.检验";
            this.btnInspect.Visible = false;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(152, 275);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 20);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "退出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // UCModuleSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.tabModuleSelect);
            this.Name = "UCModuleSelect";
            this.Size = new System.Drawing.Size(247, 358);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UCModuleSelect_KeyUp);
            this.tabModuleSelect.ResumeLayout(false);
            this.tabProcurement.ResumeLayout(false);
            this.tabProduction.ResumeLayout(false);
            this.tabDistribution.ResumeLayout(false);
            this.tabInventory.ResumeLayout(false);
            this.tabInspection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabModuleSelect;
        private System.Windows.Forms.TabPage tabProcurement;
        private System.Windows.Forms.TabPage tabInventory;
        private System.Windows.Forms.Button btnReceiveReturn;
        private System.Windows.Forms.Button btnReceive;
        private System.Windows.Forms.TabPage tabDistribution;
        private System.Windows.Forms.Button btnShipConfirm;
        private System.Windows.Forms.Button btnShipReturn;
        private System.Windows.Forms.Button btnShip;
        private System.Windows.Forms.Button btnPickList;
        private System.Windows.Forms.Button btnStockTaking;
        private System.Windows.Forms.Button btnDevanning;
        private System.Windows.Forms.Button btnPutAway;
        private System.Windows.Forms.Button btnPickUp;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPickListOnline;
        private System.Windows.Forms.TabPage tabInspection;
        private System.Windows.Forms.Button btnInspect;
        private System.Windows.Forms.Button btnHuStatus;
        private System.Windows.Forms.Button btnInspection;
        private System.Windows.Forms.Button btnShipOrder;
        private System.Windows.Forms.TabPage tabProduction;
        private System.Windows.Forms.Button btnReturnMaterial;
        private System.Windows.Forms.Button btnReloadMaterial;
        private System.Windows.Forms.Button btnLoadMaterial;
        private System.Windows.Forms.Button btnOnline;
        private System.Windows.Forms.Button btnReuse;
        private System.Windows.Forms.Button btnForceReloadMaterial;
        private System.Windows.Forms.Button btnOrderQuick;
    }
}
