using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.Sconit.Web;
using com.Sconit.Entity.Quote;
using com.Sconit.Entity.MasterData;
using System.Text.RegularExpressions;

public partial class Quote_ProductInfo_Edit : EditModuleBase
{
    public event EventHandler BackEvent;
    protected string Id
    {
        get
        {
            return (string)ViewState["Id"];
        }
        set
        {
            ViewState["Id"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadPermission();
        if (!IsPostBack)
        {
            LoadPT();
            LoadBoardMode();
            LoadPackMode();
            LoadOutBox();
            LoadPlate();
            LoadPartition();
            LoadBubbleBag();
            LoadBlister();
        }
    }

    #region Load

    protected void ddlCustomerName_Click(object sender, EventArgs e)
    {
        LoadDeliveryAdd();
    }

    void LoadDeliveryAdd()
    {
        IList<QuoteCustomerInfo> quoteCustomerInfo = TheToolingMgr.GetQuoteCustomerInfoByCode(txtCustomer.Text);
        if (quoteCustomerInfo.Count > 0)
        {
            txtDeliveryAdd.Text = quoteCustomerInfo[0].DeliveryCity;
        }
        else
        {
            txtDeliveryAdd.Text = string.Empty;
        }

        if (txtDeliveryAdd.Text != string.Empty)
        {
            IList<LogisticsFee> LogisticsFeeList = TheToolingMgr.GetLogisticsFeeByCityName(txtDeliveryAdd.Text);
            if (LogisticsFeeList.Count > 0)
            {
                txtLogisticsFee.Text = LogisticsFeeList[0].Price.ToString();
            }
            if (txtLogisticsFee.Text != string.Empty)
            {
                if (IsNumber(txtFCLNum.Text))
                {
                    txtLogisticsCost.Text = (decimal.Parse(txtLogisticsFee.Text) / Int32.Parse(txtFCLNum.Text)).ToString();
                }
            }
            else
            {
                txtLogisticsCost.Text = string.Empty;
            }
        }
        else
        {
            txtLogisticsFee.Text = string.Empty;
        }
    }

    bool IsNumber(string text)
    {
        Regex reg = new Regex("^[0-9]+$");
        Match ma = reg.Match(text);
        if (ma.Success)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void LoadPT()
    {
        PT pt = new PT();
        IList<PT> ptList = TheToolingMgr.GetPT();
        ptList.Add(pt);
        ddlPT.DataSource = ptList;
        ddlPT.DataTextField = "Name";
        ddlPT.DataValueField = "Name";
        ddlPT.DataBind();
    }

    void LoadBoardMode()
    {
        IList<BoardMode> bmList = TheToolingMgr.GetBoardMode();
        BoardMode bm = new BoardMode();
        bmList.Add(bm);
        ddlBoardMode.DataSource = bmList;
        ddlBoardMode.DataTextField = "Name";
        ddlBoardMode.DataValueField = "Name";
        ddlBoardMode.DataBind();
    }

    void LoadPackMode()
    {
        IList<PackMode> pmList = TheToolingMgr.GetPackMode();
        PackMode pm = new PackMode();
        pmList.Add(pm);
        ddlPackMode.DataSource = pmList;
        ddlPackMode.DataTextField = "Name";
        ddlPackMode.DataValueField = "Name";
        ddlPackMode.DataBind();
    }

    void LoadOutBox()
    {
        IList<OutBox> obList = TheToolingMgr.GetOutBox();
        OutBox ob = new OutBox();
        obList.Add(ob);
        ddlOutBox.DataSource = obList;
        ddlOutBox.DataTextField = "Name";
        ddlOutBox.DataValueField = "Price";
        ddlOutBox.DataBind();
        //txtOutBoxPrice.Text = ddlOutBox.SelectedValue;
        //txtOutBoxResult.Text = ddlOutBox.SelectedValue;
    }
    protected void ddlOutBox_Click(object sender, EventArgs e)
    {
        txtOutBoxPrice.Text = ddlOutBox.SelectedValue;
        txtOutBoxResult.Text = ddlOutBox.SelectedValue;
    }

    void LoadPlate()
    {
        IList<Plate> pList = TheToolingMgr.GetPlate();
        Plate p = new Plate();
        pList.Add(p);
        ddlPlate.DataSource = pList;
        ddlPlate.DataTextField = "Name";
        ddlPlate.DataValueField = "Price";
        ddlPlate.DataBind();
        //txtPlatePrice.Text = ddlPlate.SelectedValue;
        //if (txtPlateNum.Text != string.Empty)
        //{
        //    if (IsNumber(txtPlateNum.Text))
        //    {
        //        txtPlateResult.Text = (Int32.Parse(txtPlateNum.Text) * decimal.Parse(txtPlatePrice.Text)).ToString();
        //    }
        //    else
        //    {
        //        txtPlateResult.Text = string.Empty;
        //    }
        //}
        //else
        //{
        //    txtPlateResult.Text = string.Empty;
        //}
    }

    protected void txtPlateNum_OnChangeClick(object sender, EventArgs e)
    {
        if (txtPlatePrice.Text != string.Empty)
        {
            if (IsNumber(txtPlateNum.Text))
            {
                txtPlateResult.Text = (Int32.Parse(txtPlateNum.Text) * decimal.Parse(txtPlatePrice.Text)).ToString();
            }
            else
            {
                txtPlateResult.Text = string.Empty;
            }
        }
        else
        {
            txtPlateResult.Text = string.Empty;
        }
    }
    protected void ddlPlate_Click(object sender, EventArgs e)
    {
        txtPlatePrice.Text = ddlPlate.SelectedValue;
        if (txtPlateNum.Text != string.Empty)
        {
            if (IsNumber(txtPlateNum.Text))
            {
                txtPlateResult.Text = (Int32.Parse(txtPlateNum.Text) * decimal.Parse(txtPlatePrice.Text)).ToString();
            }
            else
            {
                txtPlateResult.Text = string.Empty;
            }
        }
        else
        {
            txtPlateResult.Text = string.Empty;
        }
    }
    void LoadPartition()
    {
        IList<Partition> ptList = TheToolingMgr.GetPartition();
        Partition pt = new Partition();
        ptList.Add(pt);
        ddlPartition.DataSource = ptList;
        ddlPartition.DataTextField = "Name";
        ddlPartition.DataValueField = "Price";
        ddlPartition.DataBind();
        //txtPartitionPrice.Text = ddlPartition.SelectedValue;
        //if (IsNumber(txtPartitionNum.Text))
        //{
        //    txtPartitionResult.Text = (Int32.Parse(txtPartitionNum.Text) * decimal.Parse(txtPartitionPrice.Text)).ToString();
        //}
        //else
        //{
        //    txtPartitionResult.Text = string.Empty;
        //}
    }
    protected void ddlPartition_Click(object sender, EventArgs e)
    {
        txtPartitionPrice.Text = ddlPartition.SelectedValue;
        if (IsNumber(txtPartitionNum.Text))
        {
            txtPartitionResult.Text = (Int32.Parse(txtPartitionNum.Text) * decimal.Parse(txtPartitionPrice.Text)).ToString();
        }
        else
        {
            txtPartitionResult.Text = string.Empty;
        }
    }

    protected void txtPartitionNum_ChangeClick(object sender, EventArgs e)
    {
        if (txtPartitionPrice.Text != string.Empty)
        {
            if (IsNumber(txtPartitionNum.Text))
            {
                txtPartitionResult.Text = (Int32.Parse(txtPartitionNum.Text) * decimal.Parse(txtPartitionPrice.Text)).ToString();
            }
            else
            {
                txtPartitionResult.Text = string.Empty;
            }
        }
        else
        {
            txtPartitionResult.Text = string.Empty;
        }
    }

    void LoadBubbleBag()
    {
        IList<BubbleBag> bbList = TheToolingMgr.GetBubbleBag();
        BubbleBag bb = new BubbleBag();
        bbList.Add(bb);
        ddlBubbleBag.DataSource = bbList;
        ddlBubbleBag.DataTextField = "Name";
        ddlBubbleBag.DataValueField = "Price";
        ddlBubbleBag.DataBind();
        //txtBubbleBagPrice.Text = ddlBubbleBag.SelectedValue;
        //txtBubbleBagResult.Text = ddlBubbleBag.SelectedValue;
    }

    protected void ddlBubbleBag_Click(object sender, EventArgs e)
    {
        txtBubbleBagPrice.Text = ddlBubbleBag.SelectedValue;
        txtBubbleBagResult.Text = ddlBubbleBag.SelectedValue;
    }

    void LoadBlister()
    {
        IList<Blister> bList = TheToolingMgr.GetBlister();
        Blister b = new Blister();
        bList.Add(b);
        ddlBlister.DataSource = bList;
        ddlBlister.DataTextField = "Name";
        ddlBlister.DataValueField = "Price";
        ddlBlister.DataBind();
        //txtBlisterPrice.Text = ddlBlister.SelectedValue;
        //if (IsNumber(txtBlisterNum.Text))
        //{
        //    txtBlisterResult.Text = (Int32.Parse(txtBlisterNum.Text) * decimal.Parse(txtBlisterPrice.Text)).ToString();
        //}
        //else
        //{
        //    txtBlisterResult.Text = string.Empty;
        //}
    }

    protected void txtBlisterNum_ChangeClick(object sender, EventArgs e)
    {
        if (txtBlisterPrice.Text != string.Empty)
        {
            if (IsNumber(txtBlisterNum.Text))
            {
                txtBlisterResult.Text = (Int32.Parse(txtBlisterNum.Text) * decimal.Parse(txtBlisterPrice.Text)).ToString();
            }
            else
            {
                txtBlisterResult.Text = string.Empty;
            }
        }
        else
        {
            txtBlisterResult.Text = string.Empty;
        }
    }

    protected void ddlBlister_Click(object sender, EventArgs e)
    {
        txtBlisterPrice.Text = ddlBlister.SelectedValue;
        if (IsNumber(txtBlisterNum.Text))
        {
            txtBlisterResult.Text = (Int32.Parse(txtBlisterNum.Text) * decimal.Parse(txtBlisterPrice.Text)).ToString();
        }
        else
        {
            txtBlisterResult.Text = string.Empty;
        }
    }
    protected void txtFCLNum_ChangeClick(object sender, EventArgs e)
    {
        if (txtLogisticsFee.Text != string.Empty)
        {
            if (IsNumber(txtFCLNum.Text))
            {
                txtLogisticsCost.Text = (decimal.Parse(txtLogisticsFee.Text) / Int32.Parse(txtFCLNum.Text)).ToString();
            }
            else
            {
                txtLogisticsCost.Text = string.Empty;
            }
        }
        else
        {
            txtLogisticsCost.Text = string.Empty;
        }
    }
    #endregion
    public void InitPageParameter(string id)
    {
        Id = id;
        IList<ProductInfo> productInfoList = TheToolingMgr.GetProductInfoById(id);
        if (productInfoList.Count > 0)
        {
            ProductInfo productInfo = productInfoList[0];
            txtCustomer.Text = productInfo.CustomerCode;
            LoadDeliveryAdd();
            txtProductName.Text = productInfo.ProductName;
            txtProductNo.Text = productInfo.ProductNo;
            txtVersionNo.Text = productInfo.VersionNo;
            ddlPT.SelectedValue = productInfo.PT;
            txtPCBNum.Text = productInfo.PCBNum;
            txtAdvisePCBNum.Text = productInfo.AdvisePCBNum;
            if (productInfo.DoubleSideMount)
            {
                ddlDoubleSideMount.SelectedValue = "1";
            }
            else
            {
                ddlDoubleSideMount.SelectedValue = "0";
            }
            if (productInfo.ChipBurning)
            {
                ddlChipBurning.SelectedValue = "1";
            }
            else
            {
                ddlChipBurning.SelectedValue = "0";
            }
            txtBurningNum.Text = productInfo.BurningNum;
            txtLightNum.Text = productInfo.LightNum;
            ddlBoardMode.SelectedValue = productInfo.BoardMode;
            txtConnPoint.Text = productInfo.ConnPoint;
            if (productInfo.DeviceShaping)
            {
                ddlDeviceShaping.SelectedValue = "1";
            }
            else
            {
                ddlDeviceShaping.SelectedValue = "0";
            }
            txtShapingType.Text = productInfo.ShapingType;
            txtShapingSecCount.Text = productInfo.ShapingSecCount;
            if (productInfo.DeviceCoding)
            {
                ddlDeviceCoding.SelectedValue = "1";
            }
            else
            {
                ddlDeviceCoding.SelectedValue = "0";
            }
            txtCodingType.Text = productInfo.CodingType;
            txtCodingSecCount.Text = productInfo.CodingSecCount;
            if (productInfo.FCTTest)
            {
                ddlFCTTest.SelectedValue = "1";
            }
            else
            {
                ddlFCTTest.SelectedValue = "0";
            }
            txtTestSec.Text = productInfo.TestSec;
            if (productInfo.ProductAssembly)
            {
                ddlProductAssembly.SelectedValue = "1";
            }
            else
            {
                ddlProductAssembly.SelectedValue = "0";
            }
            txtAssemblySec.Text = productInfo.AssemblySec;
            if (productInfo.FinalAssemblyTest)
            {
                ddlFinalAssemblyTest.SelectedValue = "1";
            }
            else
            {
                ddlFinalAssemblyTest.SelectedValue = "0";
            }
            txtFinalTestSec.Text = productInfo.FinalTestSec;
            txtSpecialReq.Text = productInfo.SpecialReq;
            if (productInfo.SurfaceCoating)
            {
                ddlSurfaceCoating.SelectedValue = "1";
            }
            else
            {
                ddlSurfaceCoating.SelectedValue = "0";
            }
            txtMaterialNo.Text = productInfo.MaterialNo;
            txtCoatingAcreage.Text = productInfo.CoatingAcreage;
            txtCoatingSec.Text = productInfo.CoatingSec;
            if (productInfo.ProductFilling)
            {
                ddlProductFilling.SelectedValue = "1";
            }
            else
            {
                ddlProductFilling.SelectedValue = "0";
            }
            if (productInfo.FillingPrice != null)
            {
                txtFillingPrice.Text = productInfo.FillingPrice.ToString();
            }
            else
            {
                txtFillingPrice.Text = string.Empty;
            }
            ddlPackMode.SelectedValue = productInfo.PackMode;
            if (productInfo.OutBoxPrice != null)
            {
                ddlOutBox.SelectedValue = productInfo.OutBoxPrice.ToString();
                txtOutBoxPrice.Text = productInfo.OutBoxPrice.ToString();
                txtOutBoxResult.Text = productInfo.OutBoxPrice.ToString();
            }
            if (productInfo.PlatePrice != null)
            {
                ddlPlate.SelectedValue = productInfo.PlatePrice.ToString();
                txtPlatePrice.Text = productInfo.PlatePrice.ToString();
            }
            txtPlateNum.Text = productInfo.PlateNum;
            if (productInfo.PlateResult != null)
            {
                txtPlateResult.Text = productInfo.PlateResult.ToString();
            }
            else
            {
                txtPlateResult.Text = string.Empty;
            }
            if (productInfo.PartitionPrice != null)
            {
                ddlPartition.SelectedValue = productInfo.PartitionPrice.ToString();
                txtPartitionPrice.Text = productInfo.PartitionPrice.ToString();
            }
            txtPartitionNum.Text = productInfo.PartitionNum;
            if (productInfo.PartitionResult != null)
            {
                txtPartitionResult.Text = productInfo.PartitionResult.ToString();
            }
            else
            {
                txtPartitionResult.Text = string.Empty;
            }
            if (productInfo.BubbleBagPrice != null)
            {
                ddlBubbleBag.SelectedValue = productInfo.BubbleBagPrice.ToString();
                txtBubbleBagPrice.Text = productInfo.BubbleBagPrice.ToString();
            }
            if (productInfo.BubbleBagResult != null)
            {
                txtBubbleBagResult.Text = productInfo.BubbleBagResult.ToString();
            }
            else
            {
                txtBubbleBagResult.Text = string.Empty;
            }
            if (productInfo.BlisterPrice != null)
            {
                ddlBlister.SelectedValue = productInfo.BlisterPrice.ToString();
                txtBlisterPrice.Text = productInfo.BlisterPrice.ToString();
            }
            txtBlisterNum.Text = productInfo.BlisterNum;
            if (productInfo.BlisterResult != null)
            {
                txtBlisterResult.Text = productInfo.BlisterResult.ToString();
            }
            else
            {
                txtBlisterResult.Text = string.Empty;
            }
            txtFCLNum.Text = productInfo.FCLNum;
            txtDeliveryAdd.Text = productInfo.DeliveryAdd;
            if (productInfo.LogisticsFee != null)
            {
                txtLogisticsFee.Text = productInfo.LogisticsFee.ToString();
            }
            else
            {
                txtLogisticsFee.Text = string.Empty;
            }
            if (productInfo.LogisticsCost != null)
            {
                txtLogisticsCost.Text = productInfo.LogisticsCost.ToString();
            }
            else
            {
                txtLogisticsCost.Text = string.Empty;
            }
            txtProjectId.Text = productInfo.ProjectId;

            txtSource.Text = productInfo.Source_;
            txtRemark1.Text = productInfo.Remark1;
            txtRemark2.Text = productInfo.Remark2;
        }
    }

    public void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ProductInfo productInfo = new ProductInfo();
            productInfo.Id = Int32.Parse(Id);
            #region
            if (txtCustomer.Text.Trim() != string.Empty)
            {
                productInfo.CustomerCode = txtCustomer.Text.Trim();
            }
            if (txtProductName.Text.Trim() != string.Empty)
            {
                productInfo.ProductName = txtProductName.Text.Trim();
            }
            if (txtProductNo.Text.Trim() != string.Empty)
            {
                productInfo.ProductNo = txtProductNo.Text.Trim();
            }
            if (txtVersionNo.Text.Trim() != string.Empty)
            {
                productInfo.VersionNo = txtVersionNo.Text.Trim();
            }
            if (ddlPT.SelectedValue != string.Empty)
            {
                productInfo.PT = ddlPT.SelectedValue;
            }
            if (txtPCBNum.Text.Trim() != string.Empty)
            {
                productInfo.PCBNum = txtPCBNum.Text.Trim();
            }
            if (txtAdvisePCBNum.Text.Trim() != string.Empty)
            {
                productInfo.AdvisePCBNum = txtAdvisePCBNum.Text.Trim();
            }
            if (ddlDoubleSideMount.SelectedValue != string.Empty)
            {
                if (ddlDoubleSideMount.SelectedValue == "0")
                {
                    productInfo.DoubleSideMount = false;
                }
                if (ddlDoubleSideMount.SelectedValue == "1")
                {
                    productInfo.DoubleSideMount = true;
                }
            }
            if (ddlChipBurning.SelectedValue != string.Empty)
            {
                if (ddlChipBurning.SelectedValue == "0")
                {
                    productInfo.ChipBurning = false;
                }
                if (ddlChipBurning.SelectedValue == "1")
                {
                    productInfo.ChipBurning = true;
                }
            }
            if (txtBurningNum.Text.Trim() != string.Empty)
            {
                productInfo.BurningNum = txtBurningNum.Text.Trim();
            }
            if (txtLightNum.Text.Trim() != string.Empty)
            {
                productInfo.LightNum = txtLightNum.Text.Trim();
            }
            if (ddlBoardMode.SelectedValue != string.Empty)
            {
                productInfo.BoardMode = ddlBoardMode.SelectedValue;
            }
            if (txtConnPoint.Text.Trim() != string.Empty)
            {
                productInfo.ConnPoint = txtConnPoint.Text.Trim();
            }
            if (ddlDeviceShaping.SelectedValue != string.Empty)
            {
                if (ddlDeviceShaping.SelectedValue == "0")
                {
                    productInfo.DeviceShaping = false;
                }
                if (ddlDeviceShaping.SelectedValue == "1")
                {
                    productInfo.DeviceShaping = true;
                }
            }
            if (txtShapingType.Text.Trim() != string.Empty)
            {
                productInfo.ShapingType = txtShapingType.Text.Trim();
            }
            if (txtShapingSecCount.Text.Trim() != string.Empty)
            {
                productInfo.ShapingSecCount = txtShapingSecCount.Text.Trim();
            }
            if (ddlDeviceCoding.SelectedValue != string.Empty)
            {
                if (ddlDeviceCoding.SelectedValue == "0")
                {
                    productInfo.DeviceCoding = false;
                }
                if (ddlDeviceCoding.SelectedValue == "1")
                {
                    productInfo.DeviceCoding = true;
                }
            }
            if (txtCodingType.Text.Trim() != string.Empty)
            {
                productInfo.CodingType = txtCodingType.Text.Trim();
            }
            if (txtCodingSecCount.Text.Trim() != string.Empty)
            {
                productInfo.CodingSecCount = txtCodingSecCount.Text.Trim();
            }
            if (ddlFCTTest.SelectedValue != string.Empty)
            {
                if (ddlFCTTest.SelectedValue == "0")
                {
                    productInfo.FCTTest = false;
                }
                if (ddlFCTTest.SelectedValue == "1")
                {
                    productInfo.FCTTest = true;
                }
            }
            if (txtTestSec.Text.Trim() != string.Empty)
            {
                productInfo.TestSec = txtTestSec.Text.Trim();
            }
            if (ddlProductAssembly.SelectedValue != string.Empty)
            {
                if (ddlProductAssembly.SelectedValue == "0")
                {
                    productInfo.ProductAssembly = false;
                }
                if (ddlProductAssembly.SelectedValue == "1")
                {
                    productInfo.ProductAssembly = true;
                }
            }
            if (txtAssemblySec.Text.Trim() != string.Empty)
            {
                productInfo.AssemblySec = txtAssemblySec.Text.Trim();
            }
            if (ddlFinalAssemblyTest.SelectedValue != string.Empty)
            {
                if (ddlFinalAssemblyTest.SelectedValue == "0")
                {
                    productInfo.FinalAssemblyTest = false;
                }
                if (ddlFinalAssemblyTest.SelectedValue == "1")
                {
                    productInfo.FinalAssemblyTest = true;
                }
            }
            if (txtFinalTestSec.Text.Trim() != string.Empty)
            {
                productInfo.FinalTestSec = txtFinalTestSec.Text.Trim();
            }
            if (txtSpecialReq.Text.Trim() != string.Empty)
            {
                productInfo.SpecialReq = txtSpecialReq.Text.Trim();
            }
            if (ddlSurfaceCoating.SelectedValue != string.Empty)
            {
                if (ddlSurfaceCoating.SelectedValue == "0")
                {
                    productInfo.SurfaceCoating = false;
                }
                if (ddlSurfaceCoating.SelectedValue == "1")
                {
                    productInfo.SurfaceCoating = true;
                }
            }
            if (txtMaterialNo.Text.Trim() != string.Empty)
            {
                productInfo.MaterialNo = txtMaterialNo.Text.Trim();
            }
            if (txtCoatingAcreage.Text.Trim() != string.Empty)
            {
                productInfo.CoatingAcreage = txtCoatingAcreage.Text.Trim();
            }
            if (txtCoatingSec.Text.Trim() != string.Empty)
            {
                productInfo.CoatingSec = txtCoatingSec.Text.Trim();
            }
            if (ddlProductFilling.SelectedValue != string.Empty)
            {
                if (ddlProductFilling.SelectedValue == "0")
                {
                    productInfo.ProductFilling = false;
                }
                if (ddlProductFilling.SelectedValue == "1")
                {
                    productInfo.ProductFilling = true;
                }
            }
            if (txtFillingPrice.Text.Trim() != string.Empty)
            {
                productInfo.FillingPrice = decimal.Parse(txtFillingPrice.Text.Trim());
            }
            if (ddlPackMode.SelectedValue != string.Empty)
            {
                productInfo.PackMode = ddlPackMode.SelectedValue;
            }
            if (ddlOutBox.SelectedItem.Text != string.Empty)
            {
                productInfo.OutBox = ddlOutBox.SelectedItem.Text;
            }
            if (txtOutBoxPrice.Text.Trim() != string.Empty)
            {
                productInfo.OutBoxPrice = decimal.Parse(txtOutBoxPrice.Text.Trim());
            }
            if (ddlPlate.SelectedItem.Text != string.Empty)
            {
                productInfo.Plate = ddlPlate.SelectedItem.Text;
            }
            if (txtPlateNum.Text.Trim() != string.Empty)
            {
                productInfo.PlateNum = txtPlateNum.Text.Trim();
            }
            if (txtPlatePrice.Text.Trim() != string.Empty)
            {
                productInfo.PlatePrice = decimal.Parse(txtPlatePrice.Text.Trim());
            }
            if (ddlPartition.SelectedItem.Text != string.Empty)
            {
                productInfo.Partition = ddlPartition.SelectedItem.Text;
            }
            if (txtPartitionNum.Text.Trim() != string.Empty)
            {
                productInfo.PartitionNum = txtPartitionNum.Text.Trim();
            }
            if (txtPartitionPrice.Text.Trim() != string.Empty)
            {
                productInfo.PartitionPrice = decimal.Parse(txtPartitionPrice.Text.Trim());
            }
            if (ddlBubbleBag.SelectedItem.Text != string.Empty)
            {
                productInfo.BubbleBag = ddlBubbleBag.SelectedItem.Text;
            }
            if (txtBubbleBagPrice.Text.Trim() != string.Empty)
            {
                productInfo.BubbleBagPrice = decimal.Parse(txtBubbleBagPrice.Text.Trim());
            }
            if (ddlBlister.SelectedItem.Text != string.Empty)
            {
                productInfo.Blister = ddlBlister.SelectedItem.Text;
            }
            if (txtBlisterNum.Text.Trim() != string.Empty)
            {
                productInfo.BlisterNum = txtBlisterNum.Text.Trim();
            }
            if (txtBlisterPrice.Text.Trim() != string.Empty)
            {
                productInfo.BlisterPrice = decimal.Parse(txtBlisterPrice.Text.Trim());
            }
            if (txtFCLNum.Text.Trim() != string.Empty)
            {
                productInfo.FCLNum = txtFCLNum.Text.Trim();
            }
            if (txtDeliveryAdd.Text.Trim() != string.Empty)
            {
                productInfo.DeliveryAdd = txtDeliveryAdd.Text.Trim();
            }
            if (txtLogisticsFee.Text.Trim() != string.Empty)
            {
                productInfo.LogisticsFee = decimal.Parse(txtLogisticsFee.Text.Trim());
            }
            if (txtLogisticsCost.Text.Trim() != string.Empty)
            {
                productInfo.LogisticsCost = decimal.Parse(txtLogisticsCost.Text.Trim());
            }
            if (txtOutBoxResult.Text.Trim() != string.Empty)
            {
                productInfo.OutBoxResult = decimal.Parse(txtOutBoxResult.Text.Trim());
            }
            if (txtPlateResult.Text.Trim() != string.Empty)
            {
                productInfo.PlateResult = decimal.Parse(txtPlateResult.Text.Trim());
            }
            if (txtPartitionResult.Text.Trim() != string.Empty)
            {
                productInfo.PartitionResult = decimal.Parse(txtPartitionResult.Text.Trim());
            }
            if (txtBubbleBagResult.Text.Trim() != string.Empty)
            {
                productInfo.BubbleBagResult = decimal.Parse(txtBubbleBagResult.Text.Trim());
            }
            if (txtBlisterResult.Text.Trim() != string.Empty)
            {
                productInfo.BlisterResult = decimal.Parse(txtBlisterResult.Text.Trim());
            }
            productInfo.PackPrice = productInfo.OutBoxResult + productInfo.PlateResult + productInfo.PartitionResult + productInfo.BlisterResult;
            if (productInfo.FCLNum != string.Empty && productInfo.FCLNum != null)
            {
                if (IsNumber(productInfo.FCLNum))
                {
                    productInfo.PackCost = productInfo.BubbleBagResult + productInfo.PackPrice / Int32.Parse(productInfo.FCLNum);
                }
                else
                {
                    productInfo.PackCost = productInfo.BubbleBagResult;
                }
            }
            else
            {
                productInfo.PackCost = productInfo.BubbleBagResult;
            }
            if (txtProjectId.Text.Trim() != string.Empty)
            {
                productInfo.ProjectId = txtProjectId.Text.Trim();
            }
            else
            {
                productInfo.ProjectId = TheNumberControlMgr.GenerateNumber("P");
            }
            productInfo.CreateDate = DateTime.Now;
            productInfo.Status = "Create";

            productInfo.Source_ = txtSource.Text.Trim();
            productInfo.Remark1 = txtRemark1.Text.Trim();
            productInfo.Remark2 = txtRemark2.Text.Trim();
            #endregion
            TheToolingMgr.UpdateProductInfo(productInfo);
            ShowSuccessMessage("Common.Business.Result.Save.Successfully");
        }
        catch
        {
            ShowSuccessMessage("Common.Business.Result.Save.Failed");
        }
    }

    public void btnBack_Click(object sender, EventArgs e)
    {
        BackEvent(sender, e);
    }

    void LoadPermission()
    {
        this.txtProductName.Enabled = false;
        this.txtProductNo.Enabled = false;
        this.txtVersionNo.Enabled = false;
        this.ddlPT.Enabled = false;
        this.txtPCBNum.Enabled = false;
        this.txtAdvisePCBNum.Enabled = false;
        this.ddlDoubleSideMount.Enabled = false;
        this.ddlChipBurning.Enabled = false;
        this.txtBurningNum.Enabled = false;
        this.txtLightNum.Enabled = false;
        this.txtConnPoint.Enabled = false;
        this.ddlBoardMode.Enabled = false;
        this.ddlDeviceShaping.Enabled = false;
        this.txtShapingType.Enabled = false;
        this.txtShapingSecCount.Enabled = false;
        this.ddlDeviceCoding.Enabled = false;
        this.txtCodingType.Enabled = false;
        this.txtCodingSecCount.Enabled = false;
        this.ddlFCTTest.Enabled = false;
        this.txtTestSec.Enabled = false;
        this.ddlProductAssembly.Enabled = false;
        this.txtAssemblySec.Enabled = false;
        this.ddlFinalAssemblyTest.Enabled = false;
        this.txtFinalTestSec.Enabled = false;
        this.txtSpecialReq.Enabled = false;
        this.ddlSurfaceCoating.Enabled = false;
        this.txtMaterialNo.Enabled = false;
        this.txtCoatingAcreage.Enabled = false;
        this.txtCoatingSec.Enabled = false;
        this.ddlProductFilling.Enabled = false;
        this.txtFillingPrice.Enabled = false;
        this.ddlPackMode.Enabled = false;
        this.ddlOutBox.Enabled = false;
        this.ddlPlate.Enabled = false;
        this.txtPlateNum.Enabled = false;
        this.ddlPartition.Enabled = false;
        this.txtPartitionNum.Enabled = false;
        this.ddlBubbleBag.Enabled = false;
        this.ddlBlister.Enabled = false;
        this.txtBlisterNum.Enabled = false;
        this.txtFCLNum.Enabled = false;
        //this.txtProjectNo.Enabled = false;
        if (this.CurrentUser.PagePermission != null && this.CurrentUser.PagePermission.Count > 0)
        {
            foreach (Permission permission in this.CurrentUser.PagePermission)
            {
                if (permission.Code == "P_CustomerName")
                {
                    //this.ddlCustomerName.Enabled = true;
                }
                if (permission.Code == "P_ProductName")
                {
                    this.txtProductName.Enabled = true;
                }
                if (permission.Code == "P_ProductNo")
                {
                    this.txtProductNo.Enabled = true;
                }
                if (permission.Code == "P_VersionNo")
                {
                    this.txtVersionNo.Enabled = true;
                }
                if (permission.Code == "P_PT")
                {
                    this.ddlPT.Enabled = true;
                }
                if (permission.Code == "P_PCBNum")
                {
                    this.txtPCBNum.Enabled = true;
                }
                if (permission.Code == "P_AdvisePCBNum")
                {
                    this.txtAdvisePCBNum.Enabled = true;
                }
                if (permission.Code == "P_DoubleSideMount")
                {
                    this.ddlDoubleSideMount.Enabled = true;
                }
                if (permission.Code == "P_ChipBurning")
                {
                    this.ddlChipBurning.Enabled = true;
                }
                if (permission.Code == "P_BurningNum")
                {
                    this.txtBurningNum.Enabled = true;
                }
                if (permission.Code == "P_LightNum")
                {
                    this.txtLightNum.Enabled = true;
                }
                if (permission.Code == "P_BoardMode")
                {
                    this.ddlBoardMode.Enabled = true;
                }
                if (permission.Code == "P_ConnPoint")
                {
                    this.txtConnPoint.Enabled = true;
                }
                if (permission.Code == "P_DeviceShaping")
                {
                    this.ddlDeviceShaping.Enabled = true;
                }
                if (permission.Code == "P_ShapingType")
                {
                    this.txtShapingType.Enabled = true;
                }
                if (permission.Code == "P_ShapingSecCount")
                {
                    this.txtShapingSecCount.Enabled = true;
                }
                if (permission.Code == "P_DeviceCoding")
                {
                    this.ddlDeviceCoding.Enabled = true;
                }
                if (permission.Code == "P_CodingType")
                {
                    this.txtCodingType.Enabled = true;
                }
                if (permission.Code == "P_CodingSecCount")
                {
                    this.txtCodingSecCount.Enabled = true;
                }
                if (permission.Code == "P_FCTTest")
                {
                    this.ddlFCTTest.Enabled = true;
                }
                if (permission.Code == "P_TestSec")
                {
                    this.txtTestSec.Enabled = true;
                }
                if (permission.Code == "P_ProductAssembly")
                {
                    this.ddlProductAssembly.Enabled = true;
                }
                if (permission.Code == "P_AssemblySec")
                {
                    this.txtAssemblySec.Enabled = true;
                }
                if (permission.Code == "P_FinalAssemblyTest")
                {
                    this.ddlFinalAssemblyTest.Enabled = true;
                }
                if (permission.Code == "P_FinalTestSec")
                {
                    this.txtFinalTestSec.Enabled = true;
                }
                if (permission.Code == "P_SpecialReq")
                {
                    this.txtSpecialReq.Enabled = true;
                }
                if (permission.Code == "P_SurfaceCoating")
                {
                    this.ddlSurfaceCoating.Enabled = true;
                }
                if (permission.Code == "P_MaterialNo")
                {
                    this.txtMaterialNo.Enabled = true;
                }
                if (permission.Code == "P_CoatingAcreage")
                {
                    this.txtCoatingAcreage.Enabled = true;
                }
                if (permission.Code == "P_CoatingSec")
                {
                    this.txtCoatingSec.Enabled = true;
                }
                if (permission.Code == "P_ProductFilling")
                {
                    this.ddlProductFilling.Enabled = true;
                }
                if (permission.Code == "P_FillingPrice")
                {
                    this.txtFillingPrice.Enabled = true;
                }
                if (permission.Code == "P_PackMode")
                {
                    this.ddlPackMode.Enabled = true;
                }
                if (permission.Code == "P_OutBox")
                {
                    this.ddlOutBox.Enabled = true;
                }
                if (permission.Code == "P_Plate")
                {
                    this.ddlPlate.Enabled = true;
                }
                if (permission.Code == "P_PlateNum")
                {
                    this.txtPlateNum.Enabled = true;
                }
                if (permission.Code == "P_Partition")
                {
                    this.ddlPartition.Enabled = true;
                }
                if (permission.Code == "P_PartitionNum")
                {
                    this.txtPartitionNum.Enabled = true;
                }
                if (permission.Code == "P_BubbleBag")
                {
                    this.ddlBubbleBag.Enabled = true;
                }
                if (permission.Code == "P_Blister")
                {
                    this.ddlBlister.Enabled = true;
                }
                if (permission.Code == "P_BlisterNum")
                {
                    this.txtBlisterNum.Enabled = true;
                }
                if (permission.Code == "P_FCLNum")
                {
                    this.txtFCLNum.Enabled = true;
                }
                if (permission.Code == "P_ProjectId")
                {
                    //this.txtProjectNo.Enabled = true;
                }
            }
        }
    }
}