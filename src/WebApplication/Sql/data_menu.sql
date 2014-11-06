INSERT INTO "ACC_Industry" (Code,Title,Desc_,LogoUrl,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES ('QP','QP','汽车零配件行业',null,1,'2010-07-15 10:25:08',null,'2010-07-15 10:25:08',null)
GO
INSERT INTO "ACC_Company" (Code,Title,Desc_,IndustryCode,LogoUrl,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES ('ChunShen','ChunShen','春申','QP',null,1,'2010-07-16 09:43:59',null,'2010-07-16 09:43:59',null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('1','Menu.TaxRate.165',1,'Menu.TaxRate','Menu.TaxRate.Description','税率','',1,null,'2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('1','Menu.ItemCategory.166',1,'Menu.ItemCategory','Menu.ItemCategory.Description','产品类','',1,null,'2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('1','Menu.Home.1',1,'Menu.Home','Menu.Home.Description','Home','',1,'~/Images/Nav/Home.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('10','Menu.Trigger.10',1,'Menu.Trigger','Menu.Trigger.Description','作业调度','~/Main.aspx?mid=Jobs.Trigger',1,'~/Images/Nav/Trigger.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('100','Menu.InvAging.100',1,'Menu.InvAging','Menu.InvAging.Description','库龄报表','~/Main.aspx?mid=Reports.LocAging',1,'~/Images/Nav/InvAging.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('101','Menu.InvTurnRate.101',1,'Menu.InvTurnRate','Menu.InvTurnRate.Description','库存周转率','~/Main.aspx?mid=Reports.InvTurn',1,'~/Images/Nav/InvTurnRate.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('102','Menu.InventoryIOB.102',1,'Menu.InventoryIOB','Menu.InventoryIOB.Description','库存收发存报表','~/Main.aspx?mid=Reports.InvIOB',1,'~/Images/Nav/InventoryIOB.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('103','Menu.ViewInventory.103',1,'Menu.ViewInventory','Menu.ViewInventory.Description','库存明细报表','~/Main.aspx?mid=Reports.Inventory',1,'~/Images/Nav/ViewInventory.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('104','Menu.CycleCountDiff.104',1,'Menu.CycleCountDiff','Menu.CycleCountDiff.Description','盘点差异报表','~/Main.aspx?mid=Reports.CycCntDiff',1,'~/Images/Nav/CycleCountDiff.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('105','Menu.Setup.105',1,'Menu.Setup','Menu.Setup.Description','设置','',1,'~/Images/Nav/Setup.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('106','Menu.TransferFlow.106',1,'Menu.TransferFlow','Menu.TransferFlow.Description','移库路线','~/Main.aspx?mid=Flow.Transfer',1,'~/Images/Nav/TransferFlow.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('107','Menu.Shipment.107',1,'Menu.Shipment','Menu.Shipment.Description','运输管理','',1,'~/Images/Nav/Shipment.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('108','Menu.Transaction.108',1,'Menu.Transaction','Menu.Transaction.Description','事务','',1,'~/Images/Nav/Transaction.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('109','Menu.NewItemProcurementOrder.109',1,'Menu.NewItemProcurementOrder','Menu.NewItemProcurementOrder.Description','新品采购','~/Main.aspx?mid=Order.OrderHead.Procurement__mp--ModuleType-Procurement_ModuleSubType-Nml_StatusGroupId-4_NewItem-true',1,'~/Images/Nav/NewItemProcurementOrder.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('11','Menu.MasterData.11',1,'Menu.MasterData','Menu.MasterData.Description','基础数据','',1,'~/Images/Nav/MasterData.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('110','Menu.ASN.110',1,'Menu.ASN','Menu.ASN.Description','发货通知','~/Main.aspx?mid=Warehouse.InProcessLocation__mp--ModuleType-Procurement_Action-View',1,'~/Images/Nav/ASN.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('111','Menu.ASN.111',1,'Menu.ASN','Menu.ASN.Description','发货通知','~/Main.aspx?mid=Warehouse.InProcessLocation__mp--ModuleType-Distribution_Action-View',1,'~/Images/Nav/ASN.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('112','Menu.Information.112',1,'Menu.Information','Menu.Information.Description','信息','',1,'~/Images/Nav/Information.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('113','Menu.ProcurementIntransitDetail.113',1,'Menu.ProcurementIntransitDetail','Menu.ProcurementIntransitDetail.Description','供货在途明细','~/Main.aspx?mid=Reports.IntransitDetail__mp--ModuleType-Procurement',1,'~/Images/Nav/ProcurementIntransitDetail.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('114','Menu.Setup.114',1,'Menu.Setup','Menu.Setup.Description','设置','',1,'~/Images/Nav/Setup.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('115','Menu.Shipper.115',1,'Menu.Shipper','Menu.Shipper.Description','承运商','',1,'~/Images/Nav/Shipper.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('116','Menu.Vehicle.116',1,'Menu.Vehicle','Menu.Vehicle.Description','车辆','',1,'~/Images/Nav/Vehicle.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('117','Menu.Driver.117',1,'Menu.Driver','Menu.Driver.Description','驾驶员','',1,'~/Images/Nav/Driver.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('118','Menu.Quality.118',1,'Menu.Quality','Menu.Quality.Description','质量管理','',1,'~/Images/Nav/Quality.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('119','Menu.Transaction.119',1,'Menu.Transaction','Menu.Transaction.Description','事务','',1,'~/Images/Nav/Transaction.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('12','Menu.Region.12',1,'Menu.Region','Menu.Region.Description','区域','~/Main.aspx?mid=MasterData.Region',1,'~/Images/Nav/Region.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('120','Menu.GRReturn.120',1,'Menu.GRReturn','Menu.GRReturn.Description','要货退货','~/Main.aspx?mid=Order.OrderHead.Procurement__mp--ModuleType-Procurement_ModuleSubType-Rtn_StatusGroupId-1_IsQuick-true__act--NewAction',1,'~/Images/Nav/GRReturn.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('121','Menu.GRRejectReturn.121',1,'Menu.GRRejectReturn','Menu.GRRejectReturn.Description','不合格品退货','~/Main.aspx?mid=Order.OrderHead.Procurement__mp--ModuleType-Procurement_ModuleSubType-Rtn_StatusGroupId-1_IsQuick-true_IsReject-true__act--NewAction',1,'~/Images/Nav/GRRejectReturn.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('122','Menu.WOReturn.122',1,'Menu.WOReturn','Menu.WOReturn.Description','返工单','~/Main.aspx?mid=Order.OrderHead.Production__mp--ModuleType-Production_ModuleSubType-Rwo_StatusGroupId-1__act--NewAction',1,'~/Images/Nav/WOReturn.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('123','Menu.WorkshopScrap.123',1,'Menu.WorkshopScrap','Menu.WorkshopScrap.Description','车间报验','~/Main.aspx?mid=Production.WorkshopScrap',1,'~/Images/Nav/WorkshopScrap.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('124','Menu.ScrapMaterial.124',1,'Menu.ScrapMaterial','Menu.ScrapMaterial.Description','原材料报废','~/Main.aspx?mid=Order.OrderHead.Production__mp--ModuleType-Production_ModuleSubType-Adj_StatusGroupId-1_IsScrap-true__act--NewAction',1,'~/Images/Nav/ScrapMaterial.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('125','Menu.MaterialReuse.125',1,'Menu.MaterialReuse','Menu.MaterialReuse.Description','原材料回用','~/Main.aspx?mid=Order.OrderHead.Production__mp--ModuleType-Production_ModuleSubType-Rwo_StatusGroupId-1_IsScrap-true_IsReuse-true__act--NewAction',1,'~/Images/Nav/MaterialReuse.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('126','Menu.ShipReturn.126',1,'Menu.ShipReturn','Menu.ShipReturn.Description','发货退货','~/Main.aspx?mid=Order.OrderHead.Distribution__mp--ModuleType-Distribution_ModuleSubType-Rtn_StatusGroupId-1_IsQuick-true__act--NewAction',1,'~/Images/Nav/ShipReturn.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('127','Menu.InspectionOrder.127',1,'Menu.InspectionOrder','Menu.InspectionOrder.Description','报验单','~/Main.aspx?mid=Inventory.InspectOrder',1,'~/Images/Nav/InspectionOrder.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('128','Menu.RejectReuse.128',1,'Menu.RejectReuse','Menu.RejectReuse.Description','不合格品偏差许可','~/Main.aspx?mid=Order.OrderHead.Transfer__mp--ModuleType-Transfer_ModuleSubType-Rtn_StatusGroupId-1_IsQuick-true_IsReject-true__act--NewAction',1,'~/Images/Nav/RejectReuse.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('129','Menu.ASNGap.129',1,'Menu.ASNGap','Menu.ASNGap.Description','收货差异处理','~/Main.aspx?mid=Warehouse.InProcessLocation__mp--ModuleType-Distribution_Action-View_AsnType-Gap',1,'~/Images/Nav/ASNGap.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('13','Menu.Item.13',1,'Menu.Item','Menu.Item.Description','物料','~/Main.aspx?mid=MasterData.Item',1,'~/Images/Nav/Item.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('130','Menu.Information.130',1,'Menu.Information','Menu.Information.Description','信息','',1,'~/Images/Nav/Information.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('131','Menu.InspectNotes.131',1,'Menu.InspectNotes','Menu.InspectNotes.Description','检验记录','',1,'~/Images/Nav/InspectNotes.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('132','Menu.InspectInvDetail.132',1,'Menu.InspectInvDetail','Menu.InspectInvDetail.Description','待验库存明细','',1,'~/Images/Nav/InspectInvDetail.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('133','Menu.NCInvDetail.133',1,'Menu.NCInvDetail','Menu.NCInvDetail.Description','不合格品明细','',1,'~/Images/Nav/NCInvDetail.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('134','Menu.Visualization.134',1,'Menu.Visualization','Menu.Visualization.Description','目视管理','',1,'~/Images/Nav/Visualization.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('135','Menu.OrderMonitoring.135',1,'Menu.OrderMonitoring','Menu.OrderMonitoring.Description','订单跟踪','~/Main.aspx?mid=MainPage',1,'~/Images/Nav/OrderMonitoring.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('136','Menu.SupplyChainRouting.136',1,'Menu.SupplyChainRouting','Menu.SupplyChainRouting.Description','供应链视图','~/Main.aspx?mid=Visualization.SupplyChainRouting',1,'~/Images/Nav/SupplyChainRouting.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('137','Menu.InvVisualBoard.137',1,'Menu.InvVisualBoard','Menu.InvVisualBoard.Description','库存目视板','~/Main.aspx?mid=Visualization.InvVisualBoard',1,'~/Images/Nav/InvVisualBoard.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('138','Menu.GoodsTraceability.138',1,'Menu.GoodsTraceability','Menu.GoodsTraceability.Description','物料追溯','~/Main.aspx?mid=Visualization.GoodsTraceability',1,'~/Images/Nav/GoodsTraceability.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('139','Menu.LocationBin.139',1,'Menu.LocationBin','Menu.LocationBin.Description','仓库可视化','~/Main.aspx?mid=Visualization.LocationBin',1,'~/Images/Nav/LocationBin.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('14','Menu.Location.14',1,'Menu.Location','Menu.Location.Description','库位','~/Main.aspx?mid=MasterData.Location',1,'~/Images/Nav/Location.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('140','Menu.NamedQueries.140',1,'Menu.NamedQueries','Menu.NamedQueries.Description','命名查询','~/Main.aspx?mid=Visualization.NamedQueries',1,'~/Images/Nav/NamedQueries.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('141','Menu.ViewItemFlow.141',1,'Menu.ViewItemFlow','Menu.ViewItemFlow.Description','查看物料路线','~/Main.aspx?mid=MasterData.Item.Flow',1,'~/Images/Nav/ViewItemFlow.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('142','Menu.Supplier.142',1,'Menu.Supplier','Menu.Supplier.Description','供应商','',1,'~/Images/Nav/Supplier.png','2010-07-15 10:15:13',null,'2010-07-15 10:15:13',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('143','Menu.SupplierHuPrint.143',1,'Menu.SupplierHuPrint','Menu.SupplierHuPrint.Description','供应商条码打印','~/Main.aspx?mid=Inventory.PrintHu__mp--ModuleType-Supplier',1,'~/Images/Nav/SupplierHuPrint.png','2010-07-15 10:15:13',null,'2010-07-15 10:15:13',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('144','Menu.ViewProcurementOrder.144',1,'Menu.ViewProcurementOrder','Menu.ViewProcurementOrder.Description','查看要货单','~/Main.aspx?mid=Order.OrderHead.Procurement__mp--ModuleType-Distribution_ModuleSubType-Nml_StatusGroupId-6_IsSupplier-true',1,'~/Images/Nav/ViewProcurementOrder.png','2010-07-15 10:15:13',null,'2010-07-15 10:15:13',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('145','Menu.OrderIssue.145',1,'Menu.OrderIssue','Menu.OrderIssue.Description','发货','~/Main.aspx?mid=Order.OrderIssue__mp--ModuleType-Distribution_IsSupplier-true',1,'~/Images/Nav/OrderIssue.png','2010-07-15 10:15:13',null,'2010-07-15 10:15:13',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('146','Menu.ASN.146',1,'Menu.ASN','Menu.ASN.Description','发货通知','~/Main.aspx?mid=Warehouse.InProcessLocation__mp--ModuleType-Distribution_Action-View_IsSupplier-true',1,'~/Images/Nav/ASN.png','2010-07-15 10:15:13',null,'2010-07-15 10:15:13',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('147','Menu.ReceiptNotes.147',1,'Menu.ReceiptNotes','Menu.ReceiptNotes.Description','供货收货单','~/Main.aspx?mid=Order.ReceiptNotes__mp--ModuleType-Distribution_IsSupplier-true',1,'~/Images/Nav/ReceiptNotes.png','2010-07-15 10:15:13',null,'2010-07-15 10:15:13',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('148','Menu.POBill.148',1,'Menu.POBill','Menu.POBill.Description','采购账单','~/Main.aspx?mid=Finance.Bill__mp--ModuleType-PO_IsSupplier-true',1,'~/Images/Nav/POBill.png','2010-07-15 10:15:13',null,'2010-07-15 10:15:13',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('149','Menu.SampleWO.149',1,'Menu.SampleWO','Menu.SampleWO.Description','样品生产单',null,1,'~/Images/Nav/SampleWO.png','2010-07-16 15:35:47',null,'2010-07-16 15:35:47',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('15','Menu.WorkCalendar.15',1,'Menu.WorkCalendar','Menu.WorkCalendar.Description','日历','~/Main.aspx?mid=MasterData.WorkCalendar',1,'~/Images/Nav/WorkCalendar.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('154','Menu.WIPDetail.154',1,'Menu.WIPDetail','Menu.WIPDetail.Description','在制品库存明细',null,1,null,'2010-07-16 00:00:00',null,'2010-07-16 00:00:00',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('155','Menu.ConfirmingRateReport.155',1,'Menu.ConfirmingRateReport','Menu.ConfirmingRateReport.Description','产品合格率报表',null,1,null,'2010-07-16 00:00:00',null,'2010-07-16 00:00:00',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('156','Menu.RushTransfer.156',1,'Menu.RushTransfer','Menu.RushTransfer.Description','快速移库',null,1,null,'2010-07-16 00:00:00',null,'2010-07-16 00:00:00',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('157','Menu.RepackNotes.157',1,'Menu.RepackNotes','Menu.RepackNotes.Description','翻箱记录',null,1,null,'2010-07-16 00:00:00',null,'2010-07-16 00:00:00',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('159','Menu.Shipment1.159',1,'Menu.Shipment1','Menu.Shipment1.Description','装箱单',null,1,null,'2010-07-16 00:00:00',null,'2010-07-16 00:00:00',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('16','Menu.Uom.16',1,'Menu.Uom','Menu.Uom.Description','单位','~/Main.aspx?mid=MasterData.Uom',1,'~/Images/Nav/Uom.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('161','Menu.PassCheck.161',1,'Menu.PassCheck','Menu.PassCheck.Description','通行检查',null,1,null,'2010-07-16 00:00:00',null,'2010-07-16 00:00:00',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('162','Menu.DeliveryConfirm.162',1,'Menu.DeliveryConfirm','Menu.DeliveryConfirm.Description','发货确认','~/Main.aspx?mid=Order.GoodsReceipt__mp--ModuleType-Distribution',1,'~/Images/Nav/GIConfirm.png','2010-07-15 00:00:00',null,'2010-07-15 00:00:00',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('163','Menu.GRAdjustment.163',1,'Menu.GRAdjustment','Menu.GRAdjustment.Description','收货调整','~/Main.aspx?mid=Order.ReceiptNotes__mp--ModuleType-Procurement_ModuleSubType-Adj',1,'~/Images/Nav/GRAdjustment.png','2010-09-03 00:00:00',null,'2010-09-03 00:00:00',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('17','Menu.Currency.17',1,'Menu.Currency','Menu.Currency.Description','货币','~/Main.aspx?mid=MasterData.Currency',1,'~/Images/Nav/Currency.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('18','Menu.Routing.18',1,'Menu.Routing','Menu.Routing.Description','工艺流程','~/Main.aspx?mid=MasterData.Routing',1,'~/Images/Nav/Routing.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('19','Menu.Employee.19',1,'Menu.Employee','Menu.Employee.Description','雇员','~/Main.aspx?mid=MasterData.Employee',1,'~/Images/Nav/Employee.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('2','Menu.Application.2',1,'Menu.Application','Menu.Application.Description','应用管理','',1,'~/Images/Nav/Application.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('20','Menu.Planning.20',1,'Menu.Planning','Menu.Planning.Description','计划管理','',1,'~/Images/Nav/Planning.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('21','Menu.Transaction.21',1,'Menu.Transaction','Menu.Transaction.Description','事务','',1,'~/Images/Nav/Transaction.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('22','Menu.DmdSchedule.22',1,'Menu.DmdSchedule','Menu.DmdSchedule.Description','需求日程','~/Main.aspx?mid=MRP__mp--ModuleType-DmdSchedule',1,'~/Images/Nav/DmdSchedule.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('23','Menu.MPS.23',1,'Menu.MPS','Menu.MPS.Description','主生产计划','~/Main.aspx?mid=MRP__mp--ModuleType-MPS',1,'~/Images/Nav/MPS.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('24','Menu.MRP.24',1,'Menu.MRP','Menu.MRP.Description','物料需求计划','~/Main.aspx?mid=MRP__mp--ModuleType-MRP',1,'~/Images/Nav/MRP.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('25','Menu.PS.25',1,'Menu.PS','Menu.PS.Description','班产计划','~/Main.aspx?mid=MRP.ShiftPlan',1,'~/Images/Nav/PS.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('26','Menu.Procurement.26',1,'Menu.Procurement','Menu.Procurement.Description','供货管理','',1,'~/Images/Nav/Procurement.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('27','Menu.Transaction.27',1,'Menu.Transaction','Menu.Transaction.Description','事务','',1,'~/Images/Nav/Transaction.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('28','Menu.ProcurementOrder.28',1,'Menu.ProcurementOrder','Menu.ProcurementOrder.Description','要货单','~/Main.aspx?mid=Order.OrderHead.Procurement__mp--ModuleType-Procurement_ModuleSubType-Nml_StatusGroupId-4',1,'~/Images/Nav/ProcurementOrder.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('29','Menu.OrderIssue.29',1,'Menu.OrderIssue','Menu.OrderIssue.Description','发货','~/Main.aspx?mid=Order.OrderIssue__mp--ModuleType-Procurement',1,'~/Images/Nav/OrderIssue.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('3','Menu.Preference.3',1,'Menu.Preference','Menu.Preference.Description','用户偏好','~/Main.aspx?mid=Security.UserPreference',1,'~/Images/Nav/Preference.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('30','Menu.GoodsReceipt.30',1,'Menu.GoodsReceipt','Menu.GoodsReceipt.Description','收货','~/Main.aspx?mid=Order.GoodsReceipt__mp--ModuleType-Procurement',1,'~/Images/Nav/GoodsReceipt.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('31','Menu.RushPO.31',1,'Menu.RushPO','Menu.RushPO.Description','快速要货','~/Main.aspx?mid=Order.OrderHead.Procurement__mp--ModuleType-Procurement_ModuleSubType-Nml_StatusGroupId-1_IsQuick-true__act--NewAction',1,'~/Images/Nav/RushPO.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('32','Menu.SamplePO.32',1,'Menu.SamplePO','Menu.SamplePO.Description','样品要货单','',1,'~/Images/Nav/GRAdjustment.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('33','Menu.POPlanBill.33',1,'Menu.POPlanBill','Menu.POPlanBill.Description','供应商寄售','~/Main.aspx?mid=Finance.PlanBill.PO__mp--ModuleType-PO',1,'~/Images/Nav/POPlanBill.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('34','Menu.POBill.34',1,'Menu.POBill','Menu.POBill.Description','采购账单','~/Main.aspx?mid=Finance.Bill__mp--ModuleType-PO',1,'~/Images/Nav/POBill.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('35','Menu.Information.35',1,'Menu.Information','Menu.Information.Description','信息','',1,'~/Images/Nav/Information.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('36','Menu.ReceiptNotes.36',1,'Menu.ReceiptNotes','Menu.ReceiptNotes.Description','供货收货单','~/Main.aspx?mid=Order.ReceiptNotes__mp--ModuleType-Procurement',1,'~/Images/Nav/ReceiptNotes.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('37','Menu.POPlannedBillDetail.37',1,'Menu.POPlannedBillDetail','Menu.POPlannedBillDetail.Description','供应商寄售明细','~/Main.aspx?mid=Reports.CSLoc__mp--ModuleType-Procurement',1,'~/Images/Nav/POPlannedBillDetail.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('38','Menu.ActingPOBillDetail.38',1,'Menu.ActingPOBillDetail','Menu.ActingPOBillDetail.Description','采购未开票明细','~/Main.aspx?mid=Reports.ActBill__mp--ModuleType-Procurement',1,'~/Images/Nav/ActingPOBillDetail.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('39','Menu.ActingPurchaseBillAging.39',1,'Menu.ActingPurchaseBillAging','Menu.ActingPurchaseBillAging.Description','采购未开票账龄','~/Main.aspx?mid=Reports.BillAging__mp--ModuleType-Procurement',1,'~/Images/Nav/ActingPurchaseBillAging.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('4','Menu.GeneralCode.4',1,'Menu.GeneralCode','Menu.GeneralCode.Description','选项','~/Main.aspx?mid=MasterData.GeneralCode',1,'~/Images/Nav/GeneralCode.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('40','Menu.Setup.40',1,'Menu.Setup','Menu.Setup.Description','设置','',1,'~/Images/Nav/Setup.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('41','Menu.Supplier.41',1,'Menu.Supplier','Menu.Supplier.Description','供应商','~/Main.aspx?mid=MasterData.Supplier',1,'~/Images/Nav/Supplier.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('42','Menu.ProcurementFlow.42',1,'Menu.ProcurementFlow','Menu.ProcurementFlow.Description','采购路线','~/Main.aspx?mid=Flow.Procurement',1,'~/Images/Nav/ProcurementFlow.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('43','Menu.SubconctractingFlow.43',1,'Menu.SubconctractingFlow','Menu.SubconctractingFlow.Description','委外路线','~/Main.aspx?mid=Flow.Subconctracting',1,'~/Images/Nav/SubconctractingFlow.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('44','Menu.CustomerGoodsFlow.44',1,'Menu.CustomerGoodsFlow','Menu.CustomerGoodsFlow.Description','客供品路线','~/Main.aspx?mid=Flow.CustomerGoods',1,'~/Images/Nav/CustomerGoodsFlow.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('45','Menu.ProcurementPriceList.45',1,'Menu.ProcurementPriceList','Menu.ProcurementPriceList.Description','采购价格单','~/Main.aspx?mid=Procurement.PriceList',1,'~/Images/Nav/ProcurementPriceList.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('46','Menu.Production.46',1,'Menu.Production','Menu.Production.Description','生产管理','',1,'~/Images/Nav/Production.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('47','Menu.Transaction.47',1,'Menu.Transaction','Menu.Transaction.Description','事务','',1,'~/Images/Nav/Transaction.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('48','Menu.WorkOrder.48',1,'Menu.WorkOrder','Menu.WorkOrder.Description','生产单','~/Main.aspx?mid=Order.OrderHead.Production__mp--ModuleType-Production_ModuleSubType-Nml_StatusGroupId-4',1,'~/Images/Nav/WorkOrder.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('49','Menu.NewItemWorkOrder.49',1,'Menu.NewItemWorkOrder','Menu.NewItemWorkOrder.Description','新品试制','~/Main.aspx?mid=Order.OrderHead.Production__mp--ModuleType-Production_ModuleSubType-Nml_StatusGroupId-4_NewItem-true',1,'~/Images/Nav/NewItemWorkOrder.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('5','Menu.Permission.5',1,'Menu.Permission','Menu.Permission.Description','访问控制','',1,'~/Images/Nav/Permission.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('50','Menu.WOCheckIn.50',1,'Menu.WOCheckIn','Menu.WOCheckIn.Description','生产单上线/取消','~/Main.aspx?mid=Order.BatchCheckIn__mp--ModuleType-Production_ModuleSubType-Nml',1,'~/Images/Nav/WOCheckIn.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('51','Menu.WOPrint.51',1,'Menu.WOPrint','Menu.WOPrint.Description','打印生产单','~/Main.aspx?mid=Order.BatchPrint__mp--ModuleType-Production_ModuleSubType-Nml',1,'~/Images/Nav/WOPrint.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('52','Menu.WOGR.52',1,'Menu.WOGR','Menu.WOGR.Description','生产收货','~/Main.aspx?mid=Order.GoodsReceipt__mp--ModuleType-Production',1,'~/Images/Nav/WOGR.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('53','Menu.RushWO.53',1,'Menu.RushWO','Menu.RushWO.Description','快速生产','~/Main.aspx?mid=Order.OrderHead.Production__mp--ModuleType-Production_ModuleSubType-Nml_StatusGroupId-1_IsQuick-true__act--NewAction',1,'~/Images/Nav/RushWO.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('54','Menu.Feed.54',1,'Menu.Feed','Menu.Feed.Description','生产投料','~/Main.aspx?mid=Production.Feed',1,'~/Images/Nav/Feed.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('55','Menu.Backflush.55',1,'Menu.Backflush','Menu.Backflush.Description','投料回冲','~/Main.aspx?mid=Production.Backflush',1,'~/Images/Nav/Backflush.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('56','Menu.Information.56',1,'Menu.Information','Menu.Information.Description','信息','',1,'~/Images/Nav/Information.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('57','Menu.ProductionReceiptNotes.57',1,'Menu.ProductionReceiptNotes','Menu.ProductionReceiptNotes.Description','生产收货单','~/Main.aspx?mid=Order.ReceiptNotes__mp--ModuleType-Production',1,'~/Images/Nav/ProductionReceiptNotes.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('58','Menu.ProductionIOReport.58',1,'Menu.ProductionIOReport','Menu.ProductionIOReport.Description','投入产出报表','~/Main.aspx?mid=Reports.ProdIO',1,'~/Images/Nav/ProductionIOReport.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('59','Menu.ShiftProductionReport.59',1,'Menu.ShiftProductionReport','Menu.ShiftProductionReport.Description','班产报表','~/Main.aspx?mid=Reports.ShiftProd',1,'~/Images/Nav/ShiftProductionReport.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('6','Menu.User.6',1,'Menu.User','Menu.User.Description','用户','~/Main.aspx?mid=Security.User',1,'~/Images/Nav/User.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('60','Menu.Setup.60',1,'Menu.Setup','Menu.Setup.Description','设置','',1,'~/Images/Nav/Setup.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('61','Menu.Bom.61',1,'Menu.Bom','Menu.Bom.Description','BOM','~/Main.aspx?mid=MasterData.Bom',1,'~/Images/Nav/Bom.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('62','Menu.ProductionFlow.62',1,'Menu.ProductionFlow','Menu.ProductionFlow.Description','生产线','~/Main.aspx?mid=Flow.Production',1,'~/Images/Nav/ProductionFlow.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('63','Menu.Distribution.63',1,'Menu.Distribution','Menu.Distribution.Description','发货管理','',1,'~/Images/Nav/Distribution.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('64','Menu.Transaction.64',1,'Menu.Transaction','Menu.Transaction.Description','事务','',1,'~/Images/Nav/Transaction.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('65','Menu.DistributionOrder.65',1,'Menu.DistributionOrder','Menu.DistributionOrder.Description','发货单','~/Main.aspx?mid=Order.OrderHead.Distribution__mp--ModuleType-Distribution_ModuleSubType-Nml_StatusGroupId-4',1,'~/Images/Nav/DistributionOrder.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('66','Menu.NewItemDistributionOrder.66',1,'Menu.NewItemDistributionOrder','Menu.NewItemDistributionOrder.Description','新品发运','~/Main.aspx?mid=Order.OrderHead.Distribution__mp--ModuleType-Distribution_ModuleSubType-Nml_StatusGroupId-4_NewItem-true',1,'~/Images/Nav/NewItemDistributionOrder.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('67','Menu.OrderIssue.67',1,'Menu.OrderIssue','Menu.OrderIssue.Description','发货','~/Main.aspx?mid=Order.OrderIssue__mp--ModuleType-Distribution',1,'~/Images/Nav/OrderIssue.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('68','Menu.PickList.68',1,'Menu.PickList','Menu.PickList.Description','拣货单','~/Main.aspx?mid=PickList',1,'~/Images/Nav/PickList.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('69','Menu.BatchPickList.69',1,'Menu.BatchPickList','Menu.BatchPickList.Description','拣货单关闭','~/Main.aspx?mid=PickList.Batch',1,'~/Images/Nav/BatchPickList.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('7','Menu.Role.7',1,'Menu.Role','Menu.Role.Description','角色','~/Main.aspx?mid=Security.Role',1,'~/Images/Nav/Role.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('70','Menu.BatchStartPickList.70',1,'Menu.BatchStartPickList','Menu.BatchStartPickList.Description','拣货单批次上线','~/Main.aspx?mid=PickList.BatchStart',1,'~/Images/Nav/BatchStartPickList.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('71','Menu.RushDO.71',1,'Menu.RushDO','Menu.RushDO.Description','快速发货','~/Main.aspx?mid=Order.OrderHead.Distribution__mp--ModuleType-Distribution_ModuleSubType-Nml_StatusGroupId-1_IsQuick-true__act--NewAction',1,'~/Images/Nav/RushDO.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('72','Menu.ShipAdjustment.72',1,'Menu.ShipAdjustment','Menu.ShipAdjustment.Description','发货调整','~/Main.aspx?mid=Order.OrderHead.Distribution__mp--ModuleType-Distribution_ModuleSubType-Adj_StatusGroupId-1_IsQuick-true__act--NewAction',1,'~/Images/Nav/ShipAdjustment.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('73','Menu.SOPlanBill.73',1,'Menu.SOPlanBill','Menu.SOPlanBill.Description','客户寄售','~/Main.aspx?mid=Finance.PlanBill.SO__mp--ModuleType-SO',1,'~/Images/Nav/SOPlanBill.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('74','Menu.SOBill.74',1,'Menu.SOBill','Menu.SOBill.Description','销售账单','~/Main.aspx?mid=Finance.Bill__mp--ModuleType-SO',1,'~/Images/Nav/SOBill.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('75','Menu.Information.75',1,'Menu.Information','Menu.Information.Description','信息','',1,'~/Images/Nav/Information.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('76','Menu.ShipmentMonitoring.76',1,'Menu.ShipmentMonitoring','Menu.ShipmentMonitoring.Description','发货在途明细','~/Main.aspx?mid=Reports.IntransitDetail__mp--ModuleType-Distribution',1,'~/Images/Nav/ShipmentMonitoring.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('77','Menu.DistributionReceiptNotes.77',1,'Menu.DistributionReceiptNotes','Menu.DistributionReceiptNotes.Description','发货收货单','~/Main.aspx?mid=Order.ReceiptNotes__mp--ModuleType-Distribution',1,'~/Images/Nav/DistributionReceiptNotes.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('78','Menu.SOPlannedBillDetail.78',1,'Menu.SOPlannedBillDetail','Menu.SOPlannedBillDetail.Description','客户寄售明细','~/Main.aspx?mid=Reports.CSLoc__mp--ModuleType-Distribution',1,'~/Images/Nav/SOPlannedBillDetail.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('79','Menu.ActingSalesBillDetail.79',1,'Menu.ActingSalesBillDetail','Menu.ActingSalesBillDetail.Description','销售未开票明细','~/Main.aspx?mid=Reports.ActBill__mp--ModuleType-Distribution',1,'~/Images/Nav/ActingSalesBillDetail.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('8','Menu.PrintSetup.8',1,'Menu.PrintSetup','Menu.PrintSetup.Description','打印监控','~/Main.aspx?mid=ManageSconit.PrintSetup',1,'~/Images/Nav/PrintSetup.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('80','Menu.ActingSalesBillAging.80',1,'Menu.ActingSalesBillAging','Menu.ActingSalesBillAging.Description','销售未开票账龄','~/Main.aspx?mid=Reports.BillAging__mp--ModuleType-Distribution',1,'~/Images/Nav/ActingSalesBillAging.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('81','Menu.Setup.81',1,'Menu.Setup','Menu.Setup.Description','设置','',1,'~/Images/Nav/Setup.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('82','Menu.Customer.82',1,'Menu.Customer','Menu.Customer.Description','客户','~/Main.aspx?mid=MasterData.Customer',1,'~/Images/Nav/Customer.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('83','Menu.DistributionFlow.83',1,'Menu.DistributionFlow','Menu.DistributionFlow.Description','销售路线','~/Main.aspx?mid=Flow.Distribution',1,'~/Images/Nav/DistributionFlow.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('84','Menu.DistributionPriceList.84',1,'Menu.DistributionPriceList','Menu.DistributionPriceList.Description','销售价格单','~/Main.aspx?mid=Distribution.PriceList',1,'~/Images/Nav/DistributionPriceList.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('85','Menu.Inventory.85',1,'Menu.Inventory','Menu.Inventory.Description','库存管理','',1,'~/Images/Nav/Inventory.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('86','Menu.Transaction.86',1,'Menu.Transaction','Menu.Transaction.Description','事务','',1,'~/Images/Nav/Transaction.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('87','Menu.GoodsUniting.87',1,'Menu.GoodsUniting','Menu.GoodsUniting.Description','货物单元化','~/Main.aspx?mid=Inventory.PrintHu__mp--ModuleType-Region',1,'~/Images/Nav/GoodsUniting.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('88','Menu.PutAway.88',1,'Menu.PutAway','Menu.PutAway.Description','上架','~/Main.aspx?mid=Warehouse.PutAway',1,'~/Images/Nav/PutAway.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('89','Menu.Pickup.89',1,'Menu.Pickup','Menu.Pickup.Description','下架','~/Main.aspx?mid=Warehouse.Pickup',1,'~/Images/Nav/Pickup.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('9','Menu.LeanEngine.9',1,'Menu.LeanEngine','Menu.LeanEngine.Description','精益引擎','~/Main.aspx?mid=ManageSconit.LeanEngine',1,'~/Images/Nav/LeanEngine.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('90','Menu.InvIn.90',1,'Menu.InvIn','Menu.InvIn.Description','计划外入库','~/Main.aspx?mid=Inventory.MiscOrder.__mp--ModuleType-Gr',1,'~/Images/Nav/InvIn.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('91','Menu.InvOut.91',1,'Menu.InvOut','Menu.InvOut.Description','计划外出库','~/Main.aspx?mid=Inventory.MiscOrder.__mp--ModuleType-Gi',1,'~/Images/Nav/InvOut.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('92','Menu.Repack.92',1,'Menu.Repack','Menu.Repack.Description','翻箱','~/Main.aspx?mid=Inventory.Repack__mp--Type-Repack',1,'~/Images/Nav/Repack.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('93','Menu.Devanning.93',1,'Menu.Devanning','Menu.Devanning.Description','拆箱','~/Main.aspx?mid=Inventory.Repack__mp--Type-Devanning',1,'~/Images/Nav/Devanning.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('94','Menu.CycleCount.94',1,'Menu.CycleCount','Menu.CycleCount.Description','盘点','~/Main.aspx?mid=Inventory.Stocktaking',1,'~/Images/Nav/CycleCount.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('95','Menu.InvAdjustment.95',1,'Menu.InvAdjustment','Menu.InvAdjustment.Description','库存调整','~/Main.aspx?mid=Inventory.InvAdjust',1,'~/Images/Nav/InvAdjustment.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('96','Menu.Information.96',1,'Menu.Information','Menu.Information.Description','信息','',1,'~/Images/Nav/Information.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('97','Menu.InventoryDetail.97',1,'Menu.InventoryDetail','Menu.InventoryDetail.Description','库存汇总报表','~/Main.aspx?mid=Reports.InvDetail__mp--ModuleType-InvDet',1,'~/Images/Nav/InventoryDetail.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('98','Menu.InventoryTrans.98',1,'Menu.InventoryTrans','Menu.InventoryTrans.Description','库存事务','~/Main.aspx?mid=Reports.LocTrans',1,'~/Images/Nav/InventoryTrans.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('99','Menu.HistoricalInventory.99',1,'Menu.HistoricalInventory','Menu.HistoricalInventory.Description','历史库存','~/Main.aspx?mid=Reports.InvDetail__mp--ModuleType-HisInv',1,'~/Images/Nav/HistoricalInventory.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('200','Menu.CostGroup.200',1,'Menu.CostGroup','Menu.CostGroup.Description','成本单元','~/Main.aspx?mid=Cost.CostGroup',1,'','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('202','Menu.CostElement.202',1,'Menu.CostElement','Menu.CostElement.Description','成本要素','~/Main.aspx?mid=Cost.CostElement',1,'','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('203','Menu.StandardCost.203',1,'Menu.StandardCost','Menu.StandardCost.Description','标准成本','~/Main.aspx?mid=Cost.StandardCost',1,'','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('204','Menu.FinanceCalendar.204',1,'Menu.FinanceCalendar','Menu.FinanceCalendar.Description','会计期间','~/Main.aspx?mid=Cost.FinanceCalendar',1,'','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('205','Menu.Cost.205',1,'Menu.Cost','Menu.Cost.Description','成本管理','',1,'','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
GO


set IDENTITY_INSERT ACC_MenuCommon on;



INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (1,'1','1',0,0,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (2,'2','1',1,2,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (3,'3','2',2,6,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (4,'4','2',2,8,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (5,'5','2',2,10,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (6,'6','5',3,18,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (7,'7','5',3,21,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (8,'8','2',2,16,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (9,'9','2',2,18,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (10,'10','2',2,20,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (11,'11','1',1,11,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (12,'12','11',2,24,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (13,'13','11',2,26,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (14,'14','11',2,28,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (15,'15','11',2,30,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (16,'16','11',2,32,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (17,'17','11',2,34,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (18,'18','11',2,36,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (19,'19','11',2,38,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (20,'20','1',1,20,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (21,'21','20',2,42,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (22,'22','21',3,66,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (23,'23','21',3,69,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (24,'24','21',3,72,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (25,'25','21',3,75,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (26,'26','1',1,26,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (27,'27','26',2,54,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (28,'28','27',3,84,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (29,'29','27',3,87,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (30,'30','27',3,90,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (31,'31','27',3,93,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (32,'32','27',3,96,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (33,'33','27',3,99,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (34,'34','27',3,102,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (35,'35','26',2,70,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (36,'36','35',3,108,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (37,'37','35',3,111,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (38,'38','35',3,114,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (39,'39','35',3,117,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (40,'40','26',2,80,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (41,'41','40',3,123,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (42,'42','40',3,126,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (43,'43','40',3,129,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (44,'44','40',3,132,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (45,'45','40',3,135,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (46,'46','1',1,46,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (47,'47','46',2,94,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (48,'48','47',3,144,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (49,'49','47',3,147,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (50,'50','47',3,150,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (51,'51','47',3,153,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (52,'52','47',3,156,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (53,'53','47',3,159,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (54,'54','47',3,162,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (55,'55','47',3,165,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (56,'56','46',2,112,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (57,'57','56',3,171,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (58,'58','56',3,174,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (59,'59','56',3,177,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (60,'60','46',2,120,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (61,'61','60',3,183,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (62,'62','60',3,186,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (63,'63','1',1,63,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (64,'64','63',2,128,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (65,'65','64',3,195,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (66,'66','64',3,198,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (67,'67','64',3,201,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (68,'68','64',3,204,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (69,'69','64',3,207,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (70,'70','64',3,210,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (71,'71','64',3,213,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (72,'72','64',3,216,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (73,'73','64',3,219,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (74,'74','64',3,222,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (75,'75','63',2,150,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (76,'76','75',3,228,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (77,'77','75',3,231,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (78,'78','75',3,234,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (79,'79','75',3,237,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (80,'80','75',3,240,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (81,'81','63',2,162,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (82,'82','81',3,246,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (83,'83','81',3,249,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (84,'84','81',3,252,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (85,'85','1',1,85,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (86,'86','85',2,172,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (87,'87','86',3,261,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (88,'88','86',3,264,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (89,'89','86',3,267,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (90,'90','86',3,270,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (91,'91','86',3,273,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (92,'92','86',3,276,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (93,'93','86',3,279,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (94,'94','86',3,282,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (95,'95','86',3,285,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (96,'96','85',2,192,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (97,'97','96',3,291,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (98,'98','96',3,294,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (99,'99','96',3,297,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (100,'100','96',3,300,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (101,'101','96',3,303,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (102,'102','96',3,306,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (103,'103','96',3,309,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (104,'104','96',3,312,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (105,'105','85',2,210,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (106,'106','105',3,318,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (107,'107','1',1,107,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (108,'108','107',2,216,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (109,'109','27',3,103,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (110,'110','27',3,94,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (111,'111','108',3,333,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (112,'112','107',2,224,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (113,'113','35',3,339,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (114,'114','107',2,228,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (115,'115','114',3,345,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (116,'116','114',3,348,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (117,'117','114',3,351,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (118,'118','1',1,118,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (119,'119','118',2,238,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (121,'121','119',3,363,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (122,'122','119',3,366,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (123,'123','119',3,369,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (124,'124','119',3,372,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (125,'125','119',3,375,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (127,'127','119',3,381,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (128,'128','119',3,384,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (129,'129','119',3,387,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (130,'130','118',2,260,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (131,'131','130',3,393,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (132,'132','130',3,396,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (133,'133','130',3,399,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (134,'134','1',1,134,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (135,'135','134',2,270,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (136,'136','134',2,272,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (137,'137','134',2,274,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (138,'138','134',2,276,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (139,'139','134',2,278,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (140,'140','134',2,280,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (141,'141','134',2,282,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (142,'142','40',1,142,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (143,'143','142',2,286,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (144,'144','142',2,288,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (145,'145','142',2,290,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (146,'146','142',2,292,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (147,'147','142',2,294,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (148,'148','142',2,296,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (150,'149','47',3,450,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (151,'154','56',3,453,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (152,'155','56',3,456,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (153,'156','86',3,459,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (154,'157','96',3,462,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (155,'159','108',3,465,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (156,'161','108',3,468,0,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (159,'162','64',3,209,1,'2010-09-02 00:00:00',null,'2010-09-02 00:00:00',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (160,'163','27',3,97,1,'2010-09-03 00:00:00',null,'2010-09-03 00:00:00',null)
GO
INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (168,'205','1',1,99,1,'2010-09-03 00:00:00',null,'2010-09-03 00:00:00',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (167,'200','205',2,100,1,'2010-09-03 00:00:00',null,'2010-09-03 00:00:00',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (163,'201','205',2,101,1,'2010-09-03 00:00:00',null,'2010-09-03 00:00:00',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (164,'202','205',2,102,1,'2010-09-03 00:00:00',null,'2010-09-03 00:00:00',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (165,'203','205',2,103,1,'2010-09-03 00:00:00',null,'2010-09-03 00:00:00',null)
GO

INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (166,'204','205',2,104,1,'2010-09-03 00:00:00',null,'2010-09-03 00:00:00',null)
GO


set IDENTITY_INSERT ACC_MenuCommon off;



set IDENTITY_INSERT "ACC_MenuIndustry" on;


INSERT INTO "dbo"."ACC_MenuIndustry" (Id,IndustryCode,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (9,'QP','126','119',3,376,1,'2010-09-02 00:00:00',null,'2010-09-02 00:00:00',null)
GO

INSERT INTO "dbo"."ACC_MenuIndustry" (Id,IndustryCode,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (10,'QP','111','64',3,208,1,'2010-09-02 00:00:00',null,'2010-09-02 00:00:00',null)
GO

INSERT INTO "dbo"."ACC_MenuIndustry" (Id,IndustryCode,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (11,'QP','120','119',3,360,1,'2010-09-02 00:00:00',null,'2010-09-02 00:00:00',null)
GO

set IDENTITY_INSERT "ACC_MenuIndustry" off;