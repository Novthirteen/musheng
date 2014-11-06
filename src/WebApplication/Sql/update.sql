
update acc_menu set pageurl ='~/Main.aspx?mid=Cost.Report.InvIOBnew__mp--ModuleType-Cost' where pageUrl ='~/Main.aspx?mid=Cost.Report.InvIOBnew'
update acc_menu set pageurl ='~/Main.aspx?mid=Cost.Report.InvIOBnew__mp--ModuleType-Qty' where pageUrl ='~/Main.aspx?mid=Reports.InvIOBnew'

insert  into entityOPt values('EnableMailToSupplier','TRUE','�Ƿ��͹�Ӧ���ʼ�֪ͨ','1')


--begin 20120426 tiansu �����嵥����
INSERT acc_menu VALUES ('Menu.MasterData.BomDetail',1,'�����嵥','~/Main.aspx?mid=MasterData.Bom.BomDetail__mp--IsView-true',1,'~/Images/Nav/BomDetail.png',getdate(),null,getdate(),null,null)
INSERT ACC_MenuCommon VALUES ('Menu.MasterData.BomDetail','Menu.Production.Info',3,240,1,getdate(),null,getdate(),null)
INSERT ACC_Permission VALUES ('Menu.MasterData.BomDetail','�����嵥','Production')
--end 20120426 tiansu


--20120419 tiansu ���䱨���ӿ��
ALTER VIEW [dbo].[InvAgingView]
AS
SELECT     MAX(dbo.LocationLotDet.Id) AS Id, dbo.LocationLotDet.Location, dbo.LocationLotDet.Item, SUM(dbo.LocationLotDet.Qty) AS Qty, dbo.LocationLotDet.CreateDate, 
                      dbo.Location.Region, dbo.LocationLotDet.Bin
FROM         dbo.LocationLotDet INNER JOIN
                      dbo.Location ON dbo.LocationLotDet.Location = dbo.Location.Code
WHERE     (dbo.LocationLotDet.Qty <> 0)
GROUP BY dbo.LocationLotDet.Location, dbo.LocationLotDet.Item, dbo.LocationLotDet.CreateDate, dbo.Location.Region, dbo.LocationLotDet.Bin
go

--

----20111115 wangxiang
alter table ProdLineIp2 add ItemDesc varchar(255);
update ProdLineIp2 set itemdesc=desc1+isnull(desc2,'') from item where item.code=ProdLineIp2.item
alter table party add Aging int;
update party set aging=0 where aging is null;
----end 20111115 wangxiang

INSERT ACC_Permission VALUES ('Page_CustomReport','自定义报�?,'MasterDataOperation')

INSERT ACC_Permission VALUES ('Menu.Application.Console','报表查询','Application')
INSERT acc_menu VALUES ('Menu.Application.Console',1,'报表查询','~/Main.aspx?mid=ManageSconit.Console',1,'~/Images/Nav/Console.png',getdate(),null,getdate(),null,null)
INSERT ACC_MenuCommon VALUES ('Menu.Application.Console','Menu.Application',2,240,1,getdate(),null,getdate(),null)


/****** 对象:  Table [dbo].[SqlReport]    脚本日期: 07/22/2011 23:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SqlReport](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Sql] [varchar](max) NOT NULL,
	[Desc1] [varchar](255) NULL,
	[Seq] [int] NOT NULL,
 CONSTRAINT [PK_SqlReport] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF

insert into sqlreport values('
--采购单价
select item, min(case when pricelistdet.isincludetax = 1 then unitprice / 1.17 else unitprice end) as unitprice into #pricelist from pricelistdet
inner join pricelistmstr on pricelistdet.pricelist = pricelistmstr.code
where pricelistmstr.type = ''Purchase''
group by item

--Bom
select bom, item, RateQty into #temp_det 
from bomdet
where (enddate is null or enddate > getdate()) and startdate < getdate() and id <> 46426

select distinct code into #temp_bom from bommstr where isactive = 1

------------------------------------------------
--select b.code from #temp_det as d inner join #temp_bom as b on d.item = b.code

select * into #temp_semibom from #temp_det as a where a.bom in (select b.code from #temp_det as d inner join #temp_bom as b on d.item = b.code)

select d.bom, s.item, d.RateQty * s.RateQty as RateQty into #temp_semidet1 from #temp_det as d inner join #temp_semibom as s
on d.item = s.bom
------------------------------------------------
--select b.code from  #temp_semidet1 as d inner join #temp_bom as b on d.item = b.code

select * into #temp_semibom2 from #temp_det as a where a.bom in (select b.code from #temp_semidet1 as d inner join #temp_bom as b on d.item = b.code)

select d.bom, s.item, d.RateQty * s.RateQty as RateQty into #temp_semidet2 from #temp_det as d inner join #temp_semibom2 as s
on d.item = s.bom
------------------------------------------------

delete from #temp_det where #temp_det.item in (select b.code from #temp_det as d inner join #temp_bom as b on d.item = b.code)
delete from #temp_semidet1 where #temp_semidet1.item in (select b.code from #temp_semidet1 as d inner join #temp_bom as b on d.item = b.code)
delete from #temp_semidet2 where #temp_semidet2.item in (select b.code from #temp_semidet2 as d inner join #temp_bom as b on d.item = b.code)
insert into  #temp_det select * from #temp_semidet1
insert into  #temp_det select * from #temp_semidet2

--成本价格
select d.bom as ''零件�?', item.desc1 as ''描述1'', item.desc2 as ''描述2'', sum(d.rateqty * p.unitprice) as ''单价'' from #temp_det as d inner join #pricelist as p on d.item = p.item
inner join item on item.code = d.bom
group by d.bom, item.desc1, item.desc2
union
select item.code as ''零件�?', item.desc1 as ''描述1'', item.desc2 as ''描述2'', unitprice as ''单价'' from 
#pricelist as p inner join item on item.code = p.item
',
'查询成本价格,无参�?,10)




--begin wangxiang 20110831
alter table mrp_shipplan add sourceItem varchar(50);
alter table mrp_shipplan add sourceItemDesc varchar(50);
alter table mrp_recplan add sourceItem varchar(50);
alter table mrp_recplan add sourceItemDesc varchar(50);
--end wangxiang 20110831

--begin tiansu 20110813 开票的时间�?
alter table billmstr add StartDate DateTime;
alter table billmstr add EndDate DateTime;
--end tiansu 20110813


----begin wangxiang 20110804 增加拣货结果
INSERT ACC_Permission VALUES ('Menu.DistributionMenu.PickListResult','拣货结果','Distribution')
INSERT acc_menu VALUES ('Menu.DistributionMenu.PickListResult',1,'拣货结果','~/Main.aspx?mid=Reports.PickListResult',1,'',getdate(),null,getdate(),null,null)
INSERT ACC_MenuCommon VALUES ('Menu.DistributionMenu.PickListResult','Menu.Distribution.Info',3,239,1,getdate(),null,getdate(),null)
----end wangxiang 20110804 增加拣货结果

alter table orderdet add Routing varchar(50);
alter table orderdet add ReturnRouting varchar(50);


----begin wangxiang 20110624 asn更新flow
alter table ipmstr add flow varchar(50);
alter table receiptmstr add flow varchar(50);
update ipmstr set flow =
(select distinct(om.flow) from ipdet d inner join orderloctrans l on d.orderloctransid=l.id 
inner join orderdet od on l.orderdetid=od.id
inner join ordermstr om on od.orderno=om.orderno
 where ipmstr.ipno=d.ipno)

update receiptmstr set flow =
(select distinct(om.flow) from receiptdet d inner join orderloctrans l on d.orderloctransid=l.id 
inner join orderdet od on l.orderdetid=od.id
inner join ordermstr om on od.orderno=om.orderno
 where receiptmstr.recno=d.recno)

update acc_menu set pageurl='~/Main.aspx?mid=Reports.RecParty__mp--IsSupplier-true'
 where pageurl='~/Main.aspx?mid=Order.ReceiptNotes__mp--ModuleType-Distribution_IsSupplier-true'

update acc_menu set pageurl='~/Main.aspx?mid=Reports.AsnParty__mp--IsSupplier-true'
where pageurl='~/Main.aspx?mid=Warehouse.InProcessLocation__mp--ModuleType-Procurement_Action-View_IsSupplier-true'
----begin wangxiang 20110614 修改视图
USE [sconit_musheng]
GO
/****** 对象:  View [dbo].[ActBillView]    脚本日期: 06/14/2011 16:04:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[ActBillView]
AS
SELECT     MAX(Id) AS Id, OrderNo, RecNo, ExtRecNo, TransType, BillAddr, Item, Uom, UC, EffDate, SUM(BillQty - ISNULL(BilledQty, 0)) AS Qty, UnitPrice, 
                      SUM(BillAmount - ISNULL(BilledAmount, 0)) AS Amount
FROM         dbo.ActBill
WHERE     (BillQty > 0) AND (BillQty > BilledQty) OR
                      (BillQty < 0) AND (BillQty < BilledQty)
GROUP BY OrderNo, RecNo, ExtRecNo, TransType, BillAddr, Item, Uom, UC, EffDate, UnitPrice
----end wangxiang 20110614 修改视图


INSERT ACC_Permission VALUES ('ForceReceiveOrder','强制收货','OrderOperation');
insert into acc_permission values('ImportOrderLoctrans','投入产出导入','OrderOperation');



--begin tiansu 20110603 拣货单加目的库位

alter table picklistmstr add location varchar(50) null;


--零件导入
INSERT ACC_Permission VALUES ('ItemImport','零件导入','MasterDataOperation');
GO


GO

--end tiansu 20110603




INSERT ACC_Permission VALUES ('Menu.SupplierMenu.InventoryIOB','供应商收发存','SupplierMenu')
INSERT acc_menu VALUES ('Menu.SupplierMenu.InventoryIOB',1,'供应商收发存','~/Main.aspx?mid=Reports.InvIOBParty',1,'~/Images/Nav/SupplierInventoryIOB.png',getdate(),null,getdate(),null,null)
INSERT ACC_MenuCommon VALUES ('Menu.SupplierMenu.InventoryIOB','Menu.SupplierMenu',2,239,1,getdate(),null,getdate(),null)




INSERT ACC_Permission VALUES ('Menu.SupplierMenu.Inventory','供应商库�?SupplierMenu')
INSERT acc_menu VALUES ('Menu.SupplierMenu.Inventory',1,'供应商库�?~/Main.aspx?mid=Reports.InvParty',1,'~/Images/Nav/SupplierInventory.png',getdate(),null,getdate(),null,null)
INSERT ACC_MenuCommon VALUES ('Menu.SupplierMenu.Inventory','Menu.SupplierMenu',2,239,1,getdate(),null,getdate(),null)


INSERT INTO CodeMstr (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('FlowStrategy','WO',70,0,'WO')

--begin 20110531 增加删除替代物料
insert into ACC_PermissionCategory values ('MasterDataOperation','基础数据操作','Page');
insert into acc_permission values('DeleteItemDisContinue','删除替代物料','MasterDataOperation');
--end 20110531

INSERT ACC_Permission VALUES ('Menu.SupplierMenu.ASN','发货通知','SupplierMenu');

alter table ProdLineFact add routing varchar(50);

--begin 20110526合并拣货剿alter table ordermstr add IsPLCreate bit;
update ordermstr set IsPLCreate = 0 where IsAutoCreatePL = 0;
update ordermstr set IsPLCreate = 1 where IsAutoCreatePL = 1;
alter table ordermstr alter column IsPLCreate bit not null;
insert into batchtriggerparam values (23,'FlowCode','RM-FXCBA,RM-FXCBM,RM-FXCBS|RM-YTCBM|RM-FXABA,RM-FXABM,RM-FXABS|RM-CMCBS');
--end 20110526合并拣货�?
insert into acc_permission values ('Menu.Inventory.CycleCount','盘点','Inventory');
update orderloctrans set rawitem=item where rawitem is null;
INSERT ACC_Permission VALUES ('Menu.Production.LedColorLevel','分色等级','Production')
INSERT acc_menu VALUES ('Menu.Production.LedColorLevel',1,'分色等级','~/Main.aspx?mid=MasterData.LedColorLevel',1,'',getdate(),null,getdate(),null,null)
INSERT ACC_MenuCommon VALUES ('Menu.Production.LedColorLevel','Menu.Production.SetUp',3,456,1,getdate(),null,getdate(),null)

-- begin 20110524 默认数量�?
INSERT INTO "EntityOpt" (PreCode,PreValue,CodeDesc,Seq) VALUES ('IsDefaultQtyZero','True','默认展开订单数量是否为零',60);
-- end  20110524

--begin wangxiang 20110519
insert into acc_permission values('OrderLocTrans','投入产出','OrderOperation');
insert into acc_permission values('OrderBinding','订单绑定','OrderOperation');

alter table orderloctrans add ItemDiscontinue varchar(50);
alter table orderloctrans add rawItem varchar(50);
--end wangxiang 20110519


--sunwanpeng 2011-5-23 being
INSERT ACC_Permission VALUES ('Menu.MasterData.ItemType','物料类型','MasterData')
INSERT acc_menu VALUES ('Menu.MasterData.ItemType',1,'物料类型','~/Main.aspx?mid=MasterData.ItemType',1,'~/Images/Nav/ItemType.png',getdate(),null,getdate(),null,null)
INSERT ACC_MenuCommon VALUES ('Menu.MasterData.ItemType','Menu.MasterData',2,434,1,getdate(),null,getdate(),null)
GO
--sunwanpeng 2011-5-23 end

--begin tiansu 20110520 上料打印按钮权限
insert into ACC_Permission(pm_code,pm_desc,pm_catecode) values('Module_LoadMaterialPrint','上料打印','Terminal');
--end tiansu 20110520

--begin tiansu 20110520 要货退货放到供货管理中，发货退货放到发货管理中

update acc_menucommon set parentmenu='Menu.Procurement.Trans',level_=3 where menu='Menu.Procurement.Return';
update acc_menucommon set parentmenu='Menu.Distribution.Trans',level_=3 where menu='Menu.Distribution.ShipReturn';

--end tiansu 20110520


--begin tiansu 20110519 上料结果查询，可以按批号查。生产追涿INSERT ACC_Permission VALUES ('Menu.Production.ProdLineIp2','生产追溯','Production');
INSERT acc_menu VALUES ('Menu.Production.ProdLineIp2',1,'生产追溯','~/Main.aspx?mid=Production.ProdLineIp2__mp--ModuleType-Production',1,'~/Images/Nav/ProdLineIp2.png',getdate(),null,getdate(),null,null);
INSERT ACC_MenuCommon VALUES ('Menu.Production.ProdLineIp2','Menu.Production.Info',3,187,1,getdate(),null,getdate(),null);
GO

--end tiansu 20110519



--begin sunwanpeng 20110518 修正历史数据
update item set Category1=null where Category1=''
update item set Category2=null where Category2=''
update item set Category3=null where Category3=''
update item set Category4=null where Category4=''
update item set Category5=null where Category5=''
--end sunwanpeng 20110518


--begin tiansu 20110512 加生产日昿alter table PickListDet add ManufactureDate DateTime;
--end tiansu 20110512

--begin tiansu 20110511 去掉工艺流程类型
alter table RoutingMstr drop column type;
--end tiansu 20110511



-- begin 20110510 税率比例(%) tiansu
INSERT INTO "EntityOpt" (PreCode,PreValue,CodeDesc,Seq) VALUES ('TaxRate','k17','税率比例(%)',60);
-- end  20110510 tiansu

insert into acc_permission values('Menu.MRP.DmdSchedule','需求日�?MRP')
insert into acc_permission values('Menu.MRP.CustomerScheduleReport','客户日程报表','MRP')
insert into acc_permission values('Menu.MRP.CustomerSchedule','客户日程','MRP')
--begin tiansu 20110505 零件加外包装
alter table item add HuLotSize int;
--end tiansu 20110505


--begin sunwanpeng 20110504 是否已修改密眿alter table ACC_User add ModifyPwd bit NOT NULL DEFAULT ((0));
GO
update ACC_User set ModifyPwd=1;
GO
--end sunwanpeng 20110504

insert  codemstr values('MrpOpt','OrderBeforePlan','10','1','定单优先')
insert  codemstr values('MrpOpt','OrderOnly','30','0','只看定单')
insert  codemstr values('MrpOpt','PlanOnly','20','0','只看计划')


--begin tiansu 20110417 付款金额
alter table billmstr add PaymentAmount decimal(18, 8);
GO
--end tiansu 20110417



--begin tiansu 20110413 上料表PLFeedSeq
INSERT ACC_Permission VALUES ('Menu.Production.PLFeedSeq','上料�?Production');
INSERT acc_menu VALUES ('Menu.Production.PLFeedSeq',1,'上料�?~/Main.aspx?mid=Production.PLFeedSeq__mp--ModuleType-Production_ModuleSubType-Nml',1,'~/Images/Nav/PLFeedSeq.png',getdate(),null,getdate(),null,null);
INSERT ACC_MenuCommon VALUES ('Menu.Production.PLFeedSeq','Menu.Production.Setup',3,187,1,getdate(),null,getdate(),null);
GO
insert language values('Menu.Production.PLFeedSeq','上料�?PLFeedSeq');
GO
--end tiansu 20110413

--begin tiansu 20110411 会计期间置顶，工艺流程搬到生产管理中
update acc_menucommon set parentmenu='Menu.Production.Setup' where menu='Menu.MasterData.Routing';
update acc_menucommon set seq=35 where menu='Menu.Cost.FinanceCalendar';
GO
--end tiansu 20110411


--begin dingxin 20110303
insert codemstr values('BackFlushMethod', 'BatchFeedGR', 30, 0, '投料收货回冲');
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ItemDisCon](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Item] [varchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[DisconItem] [varchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[UnitQty] [decimal](18, 8) NOT NULL,
	[Priority] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [varchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[LastModifyDate] [datetime] NOT NULL,
	[LastModifyUser] [varchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
 CONSTRAINT [PK_ItemDisCon] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[ItemDisCon]  WITH CHECK ADD  CONSTRAINT [FK_ItemDisCon_Create_User] FOREIGN KEY([CreateUser])
REFERENCES [dbo].[ACC_User] ([USR_Code])
GO
ALTER TABLE [dbo].[ItemDisCon] CHECK CONSTRAINT [FK_ItemDisCon_Create_User]
GO
ALTER TABLE [dbo].[ItemDisCon]  WITH CHECK ADD  CONSTRAINT [FK_ItemDisCon_Item] FOREIGN KEY([Item])
REFERENCES [dbo].[Item] ([Code])
GO
ALTER TABLE [dbo].[ItemDisCon] CHECK CONSTRAINT [FK_ItemDisCon_Item]
GO
ALTER TABLE [dbo].[ItemDisCon]  WITH CHECK ADD  CONSTRAINT [FK_ItemDisCon_Item1] FOREIGN KEY([DisconItem])
REFERENCES [dbo].[Item] ([Code])
GO
ALTER TABLE [dbo].[ItemDisCon] CHECK CONSTRAINT [FK_ItemDisCon_Item1]
GO
ALTER TABLE [dbo].[ItemDisCon]  WITH CHECK ADD  CONSTRAINT [FK_ItemDisCon_LastModify_User] FOREIGN KEY([LastModifyUser])
REFERENCES [dbo].[ACC_User] ([USR_Code])
GO
ALTER TABLE [dbo].[ItemDisCon] CHECK CONSTRAINT [FK_ItemDisCon_LastModify_User]
--end dingxin 20110303

--begin dingxin 20110302
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StandardCost_CostCenter]') AND parent_object_id = OBJECT_ID(N'[dbo].[StandardCost]'))
ALTER TABLE [dbo].[StandardCost] DROP CONSTRAINT [FK_StandardCost_CostCenter]

alter table standardcost drop column costcenter;

alter table standardcost add CostGroup varchar(50) not null;

ALTER TABLE [dbo].[StandardCost]  WITH CHECK ADD  CONSTRAINT [FK_StandardCost_CostGroup] FOREIGN KEY([CostGroup])
REFERENCES [dbo].[CostGroup] ([Code])
GO
ALTER TABLE [dbo].[StandardCost] CHECK CONSTRAINT [FK_StandardCost_CostGroup]
GO
--end dingxin 20110302

--begin liqiuyun 20110301
update codemstr set codevalue='Daily' where code ='TimePeriodType' and codevalue='Day'
update codemstr set codevalue='Hourly' where code ='TimePeriodType' and codevalue='Hour'
update codemstr set codevalue='Monthly' where code ='TimePeriodType' and codevalue='Month'
update codemstr set codevalue='Quarterly' where code ='TimePeriodType' and codevalue='Quarter'
update codemstr set codevalue='Weekly' where code ='TimePeriodType' and codevalue='Week'
update codemstr set codevalue='Yearly' where code ='TimePeriodType' and codevalue='Year'
--end liqiuyun 20110301

--begin dingxin 20110228
alter table inventoryBalance add ItemCategory varchar(50);
GO
ALTER TABLE [dbo].[InventoryBalance]  WITH CHECK ADD  CONSTRAINT [FK_InventoryBalance_ItemCategory] FOREIGN KEY([ItemCategory])
REFERENCES [dbo].[ItemCategory] ([Code])
GO
ALTER TABLE [dbo].[InventoryBalance] CHECK CONSTRAINT [FK_InventoryBalance_ItemCategory]
GO

alter table item add ScrapPct decimal(18,8);
alter table item add TextField5 varchar(255);
alter table item add TextField6 varchar(255);
alter table item add TextField7 varchar(255);
alter table item add TextField8 varchar(255);

alter table BomDet add TextField1 varchar(255);
alter table BomDet add TextField2 varchar(255);
alter table BomDet add TextField3 varchar(255);
alter table BomDet add TextField4 varchar(255);
alter table BomDet add TextField5 varchar(255);
alter table BomDet add TextField6 varchar(255);
alter table BomDet add TextField7 varchar(255);
alter table BomDet add TextField8 varchar(255);
--end dingxin 20110228

--begin dingxin 20110221
alter table InventoryBalance add location varchar(50) not null;

ALTER TABLE [dbo].[InventoryBalance]  WITH CHECK ADD  CONSTRAINT [FK_InventoryBalance_Location] FOREIGN KEY([Location])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[InventoryBalance] CHECK CONSTRAINT [FK_InventoryBalance_Location]
GO

alter table costtrans add CostAllocateTrans int;

alter table CostBalance drop column lastmodifydate;
alter table CostBalance drop column lastmodifyuser;
alter table CostBalance add FinanceYear int not null;
alter table CostBalance add FinanceMonth int not null;
alter table CostBalance add ItemCategory varchar(50) not null;
ALTER TABLE [dbo].[CostBalance]  WITH CHECK ADD  CONSTRAINT [FK_CostBalance_ItemCategory] FOREIGN KEY([ItemCategory])
REFERENCES [dbo].[ItemCategory] ([Code]);
GO
ALTER TABLE [dbo].[CostBalance] CHECK CONSTRAINT [FK_CostBalance_ItemCategory];
GO

alter table RoutingDet add TactTime numeric(18, 8);
alter table OrderOp drop column Activity;
alter table IpMstr drop column CurrAct;
alter table IpMstr drop column CurrOp;
--end dingxin 20110221 


update acc_menu set pageUrl ='~/Main.aspx?mid=Planning.MPS' where code ='Menu.Planning.MPS'
update acc_menucommon set isactive =1 where id =23

--begin tiansu 20110127 
ALTER  table   dbo.ItemCategory
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL,
	[BitField1] [bit] NULL,--用于慕盛LED类别
	[BitField2] [bit] NULL;
GO
update ItemCategory set [BitField1]=0;
GO
--end tiansu 20110127






--begin dx 20110126 质量管理
alter table RoutingDet drop column Activity;
alter table Location alter column Type varchar(50) not null;

delete from codemstr where code = 'LocationType';

insert into entityopt values('DefaultInspectLocation', '', '默认检验库�?1);
insert into entityopt values('DefaultRejectLocation', '', '默认不合格品库位', 1);

insert into codemstr values('LocationType', 'Nml', 1, 1, '普通库�?
insert into codemstr values('LocationType', 'INP', 2, 0, '待验库位');
insert into codemstr values('LocationType', 'REJ', 3, 0, '不合格品库位');

alter table InspectMstr add InspLoc varchar(50);
alter table InspectMstr add RejLoc varchar(50);

ALTER TABLE [dbo].[InspectMstr]  WITH CHECK ADD  CONSTRAINT [FK_InspectMstr_InspectLocation] FOREIGN KEY([InspLoc])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[InspectMstr]  WITH CHECK ADD  CONSTRAINT [FK_InspectMstr_RejectLocation] FOREIGN KEY([RejLoc])
REFERENCES [dbo].[Location] ([Code])
GO

ALTER TABLE [dbo].[InspectMstr] CHECK CONSTRAINT [FK_InspectMstr_InspectLocation]
GO
ALTER TABLE [dbo].[InspectMstr] CHECK CONSTRAINT [FK_InspectMstr_RejectLocation]
GO
--end dx 20110126

--begin wx 20110125
alter table orderdet add UnitPriceFrom decimal(18,8);
alter table orderdet add UnitPriceTo decimal(18,8);
alter table orderdet add IsProvEst bit;
alter table orderdet add TaxCode varchar(50);
alter table orderdet add IsIncludeTax bit;
--end wx 20110125

--begin dx 20110125 质量管理
alter table flowmstr add InspLocFrom varchar(50);
alter table flowmstr add InspLocTo varchar(50);
alter table flowmstr add RejLocFrom varchar(50);
alter table flowmstr add RejLocTo varchar(50);
alter table flowmstr add NeedRejInspect bit;
alter table flowdet add InspLocFrom varchar(50);
alter table flowdet add InspLocTo varchar(50);
alter table flowdet add RejLocFrom varchar(50);
alter table flowdet add RejLocTo varchar(50);
alter table flowdet add NeedRejInspect bit;
alter table OrderMstr add InspLocFrom varchar(50);
alter table OrderMstr add InspLocTo varchar(50);
alter table OrderMstr add RejLocFrom varchar(50);
alter table OrderMstr add RejLocTo varchar(50);
alter table OrderMstr add NeedRejInspect bit;
alter table OrderDet add InspLocFrom varchar(50);
alter table OrderDet add InspLocTo varchar(50);
alter table OrderDet add RejLocFrom varchar(50);
alter table OrderDet add RejLocTo varchar(50);
alter table OrderDet add NeedRejInspect bit;
alter table OrderLocTrans add InspLoc varchar(50);
alter table OrderOp add Loc varchar(50);
GO

ALTER TABLE [dbo].[FlowMstr]  WITH CHECK ADD  CONSTRAINT [FK_FlowMstr_InspectLocationFrom] FOREIGN KEY([InspLocFrom])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[FlowMstr]  WITH CHECK ADD  CONSTRAINT [FK_FlowMstr_InspectLocationTo] FOREIGN KEY([InspLocTo])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[FlowMstr]  WITH CHECK ADD  CONSTRAINT [FK_FlowMstr_RejectLocationFrom] FOREIGN KEY([RejLocFrom])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[FlowMstr]  WITH CHECK ADD  CONSTRAINT [FK_FlowMstr_RejectLocationTo] FOREIGN KEY([RejLocTo])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[FlowDet]  WITH CHECK ADD  CONSTRAINT [FK_FlowDet_InspectLocationFrom] FOREIGN KEY([InspLocFrom])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[FlowDet]  WITH CHECK ADD  CONSTRAINT [FK_FlowDet_InspectLocationTo] FOREIGN KEY([InspLocTo])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[FlowDet]  WITH CHECK ADD  CONSTRAINT [FK_FlowDet_RejectLocationFrom] FOREIGN KEY([RejLocFrom])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[FlowDet]  WITH CHECK ADD  CONSTRAINT [FK_FlowDet_RejectLocationTo] FOREIGN KEY([RejLocTo])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[OrderMstr]  WITH CHECK ADD  CONSTRAINT [FK_OrderMstr_InspectLocationFrom] FOREIGN KEY([InspLocFrom])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[OrderMstr]  WITH CHECK ADD  CONSTRAINT [FK_OrderMstr_InspectLocationTo] FOREIGN KEY([InspLocTo])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[OrderMstr]  WITH CHECK ADD  CONSTRAINT [FK_OrderMstr_RejectLocationFrom] FOREIGN KEY([RejLocFrom])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[OrderMstr]  WITH CHECK ADD  CONSTRAINT [FK_OrderMstr_RejectLocationTo] FOREIGN KEY([RejLocTo])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[OrderDet]  WITH CHECK ADD  CONSTRAINT [FK_OrderDet_InspectLocationFrom] FOREIGN KEY([InspLocFrom])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[OrderDet]  WITH CHECK ADD  CONSTRAINT [FK_OrderDet_InspectLocationTo] FOREIGN KEY([InspLocTo])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[OrderDet]  WITH CHECK ADD  CONSTRAINT [FK_OrderDet_RejectLocationFrom] FOREIGN KEY([RejLocFrom])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[OrderDet]  WITH CHECK ADD  CONSTRAINT [FK_OrderDet_RejectLocationTo] FOREIGN KEY([RejLocTo])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[OrderLocTrans]  WITH CHECK ADD  CONSTRAINT [FK_OrderLocTrans_InspectLocation] FOREIGN KEY([InspLoc])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[OrderOp]  WITH CHECK ADD  CONSTRAINT [FK_OrderOp_Location] FOREIGN KEY([Loc])
REFERENCES [dbo].[Location] ([Code])
GO

ALTER TABLE [dbo].[FlowMstr] CHECK CONSTRAINT [FK_FlowMstr_InspectLocationFrom]
GO
ALTER TABLE [dbo].[FlowMstr] CHECK CONSTRAINT [FK_FlowMstr_InspectLocationTo]
GO
ALTER TABLE [dbo].[FlowMstr] CHECK CONSTRAINT [FK_FlowMstr_RejectLocationFrom]
GO
ALTER TABLE [dbo].[FlowMstr] CHECK CONSTRAINT [FK_FlowMstr_RejectLocationTo]
GO
ALTER TABLE [dbo].[FlowDet] CHECK CONSTRAINT [FK_FlowDet_InspectLocationFrom]
GO
ALTER TABLE [dbo].[FlowDet] CHECK CONSTRAINT [FK_FlowDet_InspectLocationTo]
GO
ALTER TABLE [dbo].[FlowDet] CHECK CONSTRAINT [FK_FlowDet_RejectLocationFrom]
GO
ALTER TABLE [dbo].[FlowDet] CHECK CONSTRAINT [FK_FlowDet_RejectLocationTo]
GO
ALTER TABLE [dbo].[OrderMstr] CHECK CONSTRAINT [FK_OrderMstr_InspectLocationFrom]
GO
ALTER TABLE [dbo].[OrderMstr] CHECK CONSTRAINT [FK_OrderMstr_InspectLocationTo]
GO
ALTER TABLE [dbo].[OrderMstr] CHECK CONSTRAINT [FK_OrderMstr_RejectLocationFrom]
GO
ALTER TABLE [dbo].[OrderMstr] CHECK CONSTRAINT [FK_OrderMstr_RejectLocationTo]
GO
ALTER TABLE [dbo].[OrderDet] CHECK CONSTRAINT [FK_OrderDet_InspectLocationFrom]
GO
ALTER TABLE [dbo].[OrderDet] CHECK CONSTRAINT [FK_OrderDet_InspectLocationTo]
GO
ALTER TABLE [dbo].[OrderDet] CHECK CONSTRAINT [FK_OrderDet_RejectLocationFrom]
GO
ALTER TABLE [dbo].[OrderDet] CHECK CONSTRAINT [FK_OrderDet_RejectLocationTo]
GO
ALTER TABLE [dbo].[OrderLocTrans] CHECK CONSTRAINT [FK_OrderLocTrans_InspectLocation]
GO
ALTER TABLE [dbo].[OrderOp] CHECK CONSTRAINT [FK_OrderOp_Location]
GO

insert into codemstr values('OrderSubType', 'Rej', 5, 0, '不合格品退�?
insert into codemstr values('OrderSubType', 'Rus', 6, 0, '让不使用');
--end dx 20110125


--begin tiansu 20110117 CS 库存管理->生成条码 按钮权限
insert into ACC_Permission(PM_Code,PM_Desc,PM_CateCode) values('Module_LoadMaterial','上料','Terminal');
GO
insert into ACC_Permission(PM_Code,PM_Desc,PM_CateCode) values('Module_ReloadMaterial','换料','Terminal');
GO

CREATE TABLE [dbo].[PartyGrade](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PartyCode] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[Value] [varchar](50) NULL,
	[Seq] [int] NULL
) ON [PRIMARY]

GO

Alter table Item Add ManufactureParty varchar(50) null;
GO
Alter table HuDet Add CATPartyGrade int null;
GO
Alter table HuDet Add HUEPartyGrade int null;
GO
--end tiansu 20110117



--20110105 新增成本蝿SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CostGroup](
	[Code] [varchar](50) NOT NULL,
	[Desc1] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Cost_Group] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinanceCalendar](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinYear] [int] NOT NULL,
	[FinMonth] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[IsClose] [bit] NOT NULL,
 CONSTRAINT [PK_FinanceCalendar] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

Drop table CurrencyExchange
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CurrencyExchange](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BaseCurrency] [varchar](50) NOT NULL,
	[ExchangeCurrency] [varchar](50) NOT NULL,
	[BaseQty] [decimal](18, 8) NOT NULL,
	[ExchangeQty] [decimal](18, 8) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_ExchangeRate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CurrencyExchange]  WITH CHECK ADD  CONSTRAINT [FK_CurrencyExchange_BaseCurrency] FOREIGN KEY([BaseCurrency])
REFERENCES [dbo].[Currency] ([Code])
GO
ALTER TABLE [dbo].[CurrencyExchange] CHECK CONSTRAINT [FK_CurrencyExchange_BaseCurrency]
GO
ALTER TABLE [dbo].[CurrencyExchange]  WITH CHECK ADD  CONSTRAINT [FK_CurrencyExchange_ExchangeCurrency] FOREIGN KEY([ExchangeCurrency])
REFERENCES [dbo].[Currency] ([Code])
GO
ALTER TABLE [dbo].[CurrencyExchange] CHECK CONSTRAINT [FK_CurrencyExchange_ExchangeCurrency]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ItemCategory](
	[Code] [varchar](50) NOT NULL,
	[Desc1] [varchar](255) NOT NULL,
	[Parent] [varchar](50) NULL,
 CONSTRAINT [PK_ItemCategory] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[ItemCategory]  WITH CHECK ADD  CONSTRAINT [FK_ItemCategory_ItemCategory] FOREIGN KEY([Parent])
REFERENCES [dbo].[ItemCategory] ([Code])
GO
ALTER TABLE [dbo].[ItemCategory] CHECK CONSTRAINT [FK_ItemCategory_ItemCategory]
GO

GO
SET ANSI_PADDING OFF

Alter table Item Add Category varchar(50) null;
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_ItemCategory] FOREIGN KEY([Category])
REFERENCES [dbo].[ItemCategory] ([Code])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_ItemCategory]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CostCenter](
	[Code] [varchar](50) NOT NULL,
	[Desc1] [varchar](255) NOT NULL,
	[CostGroup] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CostCenter] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CostCenter]  WITH CHECK ADD  CONSTRAINT [FK_CostCenter_CostGroup] FOREIGN KEY([CostGroup])
REFERENCES [dbo].[CostGroup] ([Code])
GO
ALTER TABLE [dbo].[CostCenter] CHECK CONSTRAINT [FK_CostCenter_CostGroup]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CostElement](
	[Code] [varchar](50) NOT NULL,
	[Desc1] [varchar](255) NOT NULL,
	[Category] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CostElement] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

alter table workcenter add CostCenter varchar(50);
GO
ALTER TABLE [dbo].[WorkCenter]  WITH CHECK ADD  CONSTRAINT [FK_WorkCenter_CostCenter] FOREIGN KEY([CostCenter])
REFERENCES [dbo].[CostCenter] ([Code])
GO
ALTER TABLE [dbo].[WorkCenter] CHECK CONSTRAINT [FK_WorkCenter_CostCenter]
GO

insert into codemstr values('CostCategory', 'Material', 10, false, '材料');
insert into codemstr values('CostCategory', 'Labor', 20, false, '人工');
insert into codemstr values('CostCategory', 'Expense', 30, false, '费用');
insert into codemstr values('CostCategory', 'Subcontract', 40, false, '委外加工');

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StandardCost](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Item] [varchar](50) NOT NULL,
	[CostElement] [varchar](50) NOT NULL,
	[CostCenter] [varchar](50) NOT NULL,
	[Cost] [numeric](18, 8) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[LastModifyDate] [datetime] NOT NULL,
	[LastModifyUser] [varchar](50) NOT NULL,
 CONSTRAINT [PK_StandardCost] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[StandardCost]  WITH CHECK ADD  CONSTRAINT [FK_StandardCost_CostCenter] FOREIGN KEY([CostCenter])
REFERENCES [dbo].[CostCenter] ([Code])
GO
ALTER TABLE [dbo].[StandardCost] CHECK CONSTRAINT [FK_StandardCost_CostCenter]
GO
ALTER TABLE [dbo].[StandardCost]  WITH CHECK ADD  CONSTRAINT [FK_StandardCost_CostElement] FOREIGN KEY([CostElement])
REFERENCES [dbo].[CostElement] ([Code])
GO
ALTER TABLE [dbo].[StandardCost] CHECK CONSTRAINT [FK_StandardCost_CostElement]
GO
ALTER TABLE [dbo].[StandardCost]  WITH CHECK ADD  CONSTRAINT [FK_StandardCost_Create_User] FOREIGN KEY([CreateUser])
REFERENCES [dbo].[ACC_User] ([USR_Code])
GO
ALTER TABLE [dbo].[StandardCost] CHECK CONSTRAINT [FK_StandardCost_Create_User]
GO
ALTER TABLE [dbo].[StandardCost]  WITH CHECK ADD  CONSTRAINT [FK_StandardCost_Item] FOREIGN KEY([Item])
REFERENCES [dbo].[Item] ([Code])
GO
ALTER TABLE [dbo].[StandardCost] CHECK CONSTRAINT [FK_StandardCost_Item]
GO
ALTER TABLE [dbo].[StandardCost]  WITH CHECK ADD  CONSTRAINT [FK_StandardCost_LastModify_User] FOREIGN KEY([LastModifyUser])
REFERENCES [dbo].[ACC_User] ([USR_Code])
GO
ALTER TABLE [dbo].[StandardCost] CHECK CONSTRAINT [FK_StandardCost_LastModify_User]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ExpenseElement](
	[Code] [varchar](50) NOT NULL,
	[Desc1] [varchar](255) NULL,
 CONSTRAINT [PK_ExpenseElement] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CostTrans](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Item] [varchar](50) NOT NULL,
	[ItemCategory] [varchar](50) NOT NULL,
	[OrderNo] [varchar](50) NULL,
	[RecNo] [varchar](50) NULL,
	[CostGroup] [varchar](50) NOT NULL,
	[CostCenter] [varchar](50) NOT NULL,
	[CostElement] [varchar](50) NOT NULL,
	[Currency] [varchar](50) NOT NULL,
	[BaseCurrency] [varchar](50) NOT NULL,
	[ExchangeRate] [numeric](18, 8) NOT NULL,
	[Qty] [numeric](18, 8) NOT NULL,
	[Amount] [numeric](18, 8) NOT NULL,
	[DiffAmount] [numeric](18, 8) NOT NULL,
	[RefItem] [varchar](50) NULL,
	[RefQty] [numeric](18, 8) NULL,
	[EffDate] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CostTrans] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CostTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostTrans_ACC_User] FOREIGN KEY([CreateUser])
REFERENCES [dbo].[ACC_User] ([USR_Code])
GO
ALTER TABLE [dbo].[CostTrans] CHECK CONSTRAINT [FK_CostTrans_ACC_User]
GO
ALTER TABLE [dbo].[CostTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostTrans_BaseCurrency] FOREIGN KEY([BaseCurrency])
REFERENCES [dbo].[Currency] ([Code])
GO
ALTER TABLE [dbo].[CostTrans] CHECK CONSTRAINT [FK_CostTrans_BaseCurrency]
GO
ALTER TABLE [dbo].[CostTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostTrans_CostCenter] FOREIGN KEY([CostCenter])
REFERENCES [dbo].[CostCenter] ([Code])
GO
ALTER TABLE [dbo].[CostTrans] CHECK CONSTRAINT [FK_CostTrans_CostCenter]
GO
ALTER TABLE [dbo].[CostTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostTrans_CostElement] FOREIGN KEY([CostElement])
REFERENCES [dbo].[CostElement] ([Code])
GO
ALTER TABLE [dbo].[CostTrans] CHECK CONSTRAINT [FK_CostTrans_CostElement]
GO
ALTER TABLE [dbo].[CostTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostTrans_CostGroup] FOREIGN KEY([CostGroup])
REFERENCES [dbo].[CostGroup] ([Code])
GO
ALTER TABLE [dbo].[CostTrans] CHECK CONSTRAINT [FK_CostTrans_CostGroup]
GO
ALTER TABLE [dbo].[CostTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostTrans_Currency] FOREIGN KEY([Currency])
REFERENCES [dbo].[Currency] ([Code])
GO
ALTER TABLE [dbo].[CostTrans] CHECK CONSTRAINT [FK_CostTrans_Currency]
GO
ALTER TABLE [dbo].[CostTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostTrans_Item] FOREIGN KEY([Item])
REFERENCES [dbo].[Item] ([Code])
GO
ALTER TABLE [dbo].[CostTrans] CHECK CONSTRAINT [FK_CostTrans_Item]
GO
ALTER TABLE [dbo].[CostTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostTrans_ItemCategory] FOREIGN KEY([ItemCategory])
REFERENCES [dbo].[ItemCategory] ([Code])
GO
ALTER TABLE [dbo].[CostTrans] CHECK CONSTRAINT [FK_CostTrans_ItemCategory]
GO
ALTER TABLE [dbo].[CostTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostTrans_OrderMstr] FOREIGN KEY([OrderNo])
REFERENCES [dbo].[OrderMstr] ([OrderNo])
GO
ALTER TABLE [dbo].[CostTrans] CHECK CONSTRAINT [FK_CostTrans_OrderMstr]
GO
ALTER TABLE [dbo].[CostTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostTrans_ReceiptMstr] FOREIGN KEY([RecNo])
REFERENCES [dbo].[ReceiptMstr] ([RecNo])
GO
ALTER TABLE [dbo].[CostTrans] CHECK CONSTRAINT [FK_CostTrans_ReceiptMstr]
GO
ALTER TABLE [dbo].[CostTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostTrans_RefItem] FOREIGN KEY([RefItem])
REFERENCES [dbo].[Item] ([Code])
GO
ALTER TABLE [dbo].[CostTrans] CHECK CONSTRAINT [FK_CostTrans_RefItem]
GO

Alter table Region add CostCenter varchar(50);
Alter table Region add InspectLoc varchar(50);
Alter table Region add RejectLoc varchar(50);
GO
ALTER TABLE [dbo].[Region]  WITH CHECK ADD  CONSTRAINT [FK_Region_CostCenter] FOREIGN KEY([CostCenter])
REFERENCES [dbo].[CostCenter] ([Code])
GO
ALTER TABLE [dbo].[Region] CHECK CONSTRAINT [FK_Region_CostCenter]
GO
ALTER TABLE [dbo].[Region]  WITH CHECK ADD  CONSTRAINT [FK_Region_InspectLocation] FOREIGN KEY([InspectLoc])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[Region] CHECK CONSTRAINT [FK_Region_InspectLocation]
GO
ALTER TABLE [dbo].[Region]  WITH CHECK ADD  CONSTRAINT [FK_Region_RejectLocation] FOREIGN KEY([RejectLoc])
REFERENCES [dbo].[Location] ([Code])
GO
ALTER TABLE [dbo].[Region] CHECK CONSTRAINT [FK_Region_RejectLocation]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CostDet](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Item] [varchar](50) NOT NULL,
	[ItemCategory] [varchar](50) NOT NULL,
	[CostGroup] [varchar](50) NOT NULL,
	[CostElement] [varchar](50) NOT NULL,
	[Cost] [numeric](18, 8) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[FinanceYear] [int] NOT NULL,
	[FinanceMonth] [int] NOT NULL,
 CONSTRAINT [PK_CostDet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CostDet]  WITH CHECK ADD  CONSTRAINT [FK_CostDet_CostElement] FOREIGN KEY([CostElement])
REFERENCES [dbo].[CostElement] ([Code])
GO
ALTER TABLE [dbo].[CostDet] CHECK CONSTRAINT [FK_CostDet_CostElement]
GO
ALTER TABLE [dbo].[CostDet]  WITH CHECK ADD  CONSTRAINT [FK_CostDet_CostGroup] FOREIGN KEY([CostGroup])
REFERENCES [dbo].[CostGroup] ([Code])
GO
ALTER TABLE [dbo].[CostDet] CHECK CONSTRAINT [FK_CostDet_CostGroup]
GO
ALTER TABLE [dbo].[CostDet]  WITH CHECK ADD  CONSTRAINT [FK_CostDet_Create_User] FOREIGN KEY([CreateUser])
REFERENCES [dbo].[ACC_User] ([USR_Code])
GO
ALTER TABLE [dbo].[CostDet] CHECK CONSTRAINT [FK_CostDet_Create_User]
GO
ALTER TABLE [dbo].[CostDet]  WITH CHECK ADD  CONSTRAINT [FK_CostDet_Item] FOREIGN KEY([Item])
REFERENCES [dbo].[Item] ([Code])
GO
ALTER TABLE [dbo].[CostDet] CHECK CONSTRAINT [FK_CostDet_Item]
GO
ALTER TABLE [dbo].[CostDet]  WITH CHECK ADD  CONSTRAINT [FK_CostDet_ItemCategory] FOREIGN KEY([ItemCategory])
REFERENCES [dbo].[ItemCategory] ([Code])
GO
ALTER TABLE [dbo].[CostDet] CHECK CONSTRAINT [FK_CostDet_ItemCategory]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CostBalance](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Item] [varchar](50) NOT NULL,
	[CostGroup] [varchar](50) NOT NULL,
	[CostElement] [varchar](50) NOT NULL,
	[Balance] [numeric](18, 8) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
	[LastModifyDate] [datetime] NOT NULL,
	[LastModifyUser] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CostBalance_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CostBalance]  WITH CHECK ADD  CONSTRAINT [FK_CostBalance_CostElement] FOREIGN KEY([CostElement])
REFERENCES [dbo].[CostElement] ([Code])
GO
ALTER TABLE [dbo].[CostBalance] CHECK CONSTRAINT [FK_CostBalance_CostElement]
GO
ALTER TABLE [dbo].[CostBalance]  WITH CHECK ADD  CONSTRAINT [FK_CostBalance_CostGroup] FOREIGN KEY([CostGroup])
REFERENCES [dbo].[CostGroup] ([Code])
GO
ALTER TABLE [dbo].[CostBalance] CHECK CONSTRAINT [FK_CostBalance_CostGroup]
GO
ALTER TABLE [dbo].[CostBalance]  WITH CHECK ADD  CONSTRAINT [FK_CostBalance_Create_User] FOREIGN KEY([CreateUser])
REFERENCES [dbo].[ACC_User] ([USR_Code])
GO
ALTER TABLE [dbo].[CostBalance] CHECK CONSTRAINT [FK_CostBalance_Create_User]
GO
ALTER TABLE [dbo].[CostBalance]  WITH CHECK ADD  CONSTRAINT [FK_CostBalance_Item] FOREIGN KEY([Item])
REFERENCES [dbo].[Item] ([Code])
GO
ALTER TABLE [dbo].[CostBalance] CHECK CONSTRAINT [FK_CostBalance_Item]
GO
ALTER TABLE [dbo].[CostBalance]  WITH CHECK ADD  CONSTRAINT [FK_CostBalance_LastModify_User] FOREIGN KEY([LastModifyUser])
REFERENCES [dbo].[ACC_User] ([USR_Code])
GO
ALTER TABLE [dbo].[CostBalance] CHECK CONSTRAINT [FK_CostBalance_LastModify_User]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InventoryBalance](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Item] [varchar](50) NOT NULL,
	[CostGroup] [varchar](50) NOT NULL,
	[Qty] [decimal](18, 8) NOT NULL,
	[FinanceYear] [int] NOT NULL,
	[FinanceMonth] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
 CONSTRAINT [PK_InventoryBalance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[InventoryBalance]  WITH CHECK ADD  CONSTRAINT [FK_InventoryBalance_ACC_User] FOREIGN KEY([CreateUser])
REFERENCES [dbo].[ACC_User] ([USR_Code])
GO
ALTER TABLE [dbo].[InventoryBalance] CHECK CONSTRAINT [FK_InventoryBalance_ACC_User]
GO
ALTER TABLE [dbo].[InventoryBalance]  WITH CHECK ADD  CONSTRAINT [FK_InventoryBalance_CostGroup] FOREIGN KEY([CostGroup])
REFERENCES [dbo].[CostGroup] ([Code])
GO
ALTER TABLE [dbo].[InventoryBalance] CHECK CONSTRAINT [FK_InventoryBalance_CostGroup]
GO
ALTER TABLE [dbo].[InventoryBalance]  WITH CHECK ADD  CONSTRAINT [FK_InventoryBalance_Item] FOREIGN KEY([Item])
REFERENCES [dbo].[Item] ([Code])
GO
ALTER TABLE [dbo].[InventoryBalance] CHECK CONSTRAINT [FK_InventoryBalance_Item]
GO
insert into EntityOpt values ('CostElementMaterial', '1000', '成本要素直接材料代码', 0);
insert into EntityOpt values ('CostElementLabor', '2000', '成本要素直接人工代码', 0);

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaxRate](
	[Code] [varchar](50) NOT NULL,
	[Desc1] [varchar](50) NOT NULL,
	[TaxRate] [decimal](18, 8) NOT NULL,
 CONSTRAINT [PK_Tax] PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

alter table RoutingDet drop column TactTime;
alter table RoutingDet add MachQty numeric(18, 8);
alter table RoutingDet add Yield numeric(18, 8);
alter table OrderOp drop column UnitTime;
alter table OrderOp drop column WorkTime;
alter table OrderOp add SetupTime numeric(18, 8);
alter table OrderOp add RunTime numeric(18, 8);
alter table OrderOp add MoveTime numeric(18, 8);
alter table OrderOp add MachQty numeric(18, 8);
alter table OrderOp add Yield numeric(18, 8);
GO

ALTER TABLE [dbo].[WorkCenter] DROP CONSTRAINT [FK_WorkCenter_Party];
alter table WorkCenter drop column Party;
alter table WorkCenter add column Region varchar(50);
GO
ALTER TABLE [dbo].[WorkCenter]  WITH CHECK ADD  CONSTRAINT [FK_WorkCenter_Region] FOREIGN KEY([Region])
REFERENCES [dbo].[Region] ([Code])
GO
ALTER TABLE [dbo].[WorkCenter] CHECK CONSTRAINT [FK_WorkCenter_Region]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CostAllocateMethod](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExpenseElement] [varchar](50) NOT NULL,
	[CostCenter] [varchar](50) NOT NULL,
	[CostElement] [varchar](50) NOT NULL,
	[DependCostElement] [varchar](50) NOT NULL,
	[AllocateBy] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CostAllocateMethod] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CostAllocateMethod]  WITH CHECK ADD  CONSTRAINT [FK_CostAllocateMethod_CostCenter] FOREIGN KEY([CostCenter])
REFERENCES [dbo].[CostCenter] ([Code])
GO
ALTER TABLE [dbo].[CostAllocateMethod] CHECK CONSTRAINT [FK_CostAllocateMethod_CostCenter]
GO
ALTER TABLE [dbo].[CostAllocateMethod]  WITH CHECK ADD  CONSTRAINT [FK_CostAllocateMethod_CostElement] FOREIGN KEY([CostElement])
REFERENCES [dbo].[CostElement] ([Code])
GO
ALTER TABLE [dbo].[CostAllocateMethod] CHECK CONSTRAINT [FK_CostAllocateMethod_CostElement]
GO
ALTER TABLE [dbo].[CostAllocateMethod]  WITH CHECK ADD  CONSTRAINT [FK_CostAllocateMethod_CostElement1] FOREIGN KEY([DependCostElement])
REFERENCES [dbo].[CostElement] ([Code])
GO
ALTER TABLE [dbo].[CostAllocateMethod] CHECK CONSTRAINT [FK_CostAllocateMethod_CostElement1]
GO
ALTER TABLE [dbo].[CostAllocateMethod]  WITH CHECK ADD  CONSTRAINT [FK_CostAllocateMethod_ExpenseElement] FOREIGN KEY([ExpenseElement])
REFERENCES [dbo].[ExpenseElement] ([Code])
GO
ALTER TABLE [dbo].[CostAllocateMethod] CHECK CONSTRAINT [FK_CostAllocateMethod_ExpenseElement]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CostAllocateTrans](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExpenseElement] [varchar](50) NULL,
	[CostCenter] [varchar](50) NOT NULL,
	[CostElement] [varchar](50) NOT NULL,
	[DependCostElement] [varchar](50) NOT NULL,
	[AllocateBy] [varchar](50) NOT NULL,
	[Amount] [decimal](18, 8) NOT NULL,
	[EffDate] [datetime] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateUser] [varchar](50) NOT NULL,
 CONSTRAINT [PK_CostAllocateTrans] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[CostAllocateTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostAllocateTrans_ACC_User] FOREIGN KEY([CreateUser])
REFERENCES [dbo].[ACC_User] ([USR_Code])
GO
ALTER TABLE [dbo].[CostAllocateTrans] CHECK CONSTRAINT [FK_CostAllocateTrans_ACC_User]
GO
ALTER TABLE [dbo].[CostAllocateTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostAllocateTrans_CostCenter] FOREIGN KEY([CostCenter])
REFERENCES [dbo].[CostCenter] ([Code])
GO
ALTER TABLE [dbo].[CostAllocateTrans] CHECK CONSTRAINT [FK_CostAllocateTrans_CostCenter]
GO
ALTER TABLE [dbo].[CostAllocateTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostAllocateTrans_CostElement] FOREIGN KEY([CostElement])
REFERENCES [dbo].[CostElement] ([Code])
GO
ALTER TABLE [dbo].[CostAllocateTrans] CHECK CONSTRAINT [FK_CostAllocateTrans_CostElement]
GO
ALTER TABLE [dbo].[CostAllocateTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostAllocateTrans_CostElement1] FOREIGN KEY([DependCostElement])
REFERENCES [dbo].[CostElement] ([Code])
GO
ALTER TABLE [dbo].[CostAllocateTrans] CHECK CONSTRAINT [FK_CostAllocateTrans_CostElement1]
GO
ALTER TABLE [dbo].[CostAllocateTrans]  WITH CHECK ADD  CONSTRAINT [FK_CostAllocateTrans_ExpenseElement] FOREIGN KEY([ExpenseElement])
REFERENCES [dbo].[ExpenseElement] ([Code])
GO
ALTER TABLE [dbo].[CostAllocateTrans] CHECK CONSTRAINT [FK_CostAllocateTrans_ExpenseElement]
GO

insert into codemstr values('AllocateBy', 'Amount', 10, 1, '按金�?
insert into codemstr values('AllocateBy', 'Qty', 20, 1, '按数�?
GO

Alter table PlanBill add CostCenter varchar(50);
Alter table PlanBill add CostGroup varchar(50);

Alter table ActBill add CostCenter varchar(50);
Alter table ActBill add CostGroup varchar(50);

Alter table BillTrans add CostCenter varchar(50);
Alter table BillTrans add CostGroup varchar(50);

Alter table LocTrans add CostCenterFrom varchar(50);
Alter table LocTrans add CostGroupFrom varchar(50);
Alter table LocTrans add CostCenterTo varchar(50);
Alter table LocTrans add CostGroupTo varchar(50);
--20110105

--20101207 盘点并上架企业选项
Insert into entityopt values('PutWhenCycleCount',  'True', '盘点并上�?100);
--20101207

--dingxin 盘点 2010-11-29
alter table cyclecountmstr add IsDynamic bit;
update cyclecountmstr set IsDynamic = 0;
alter table cyclecountmstr alter column IsDynamic bit not null;
alter table cyclecountdet add StartTime datetime;
alter table cyclecountdet add EndTime datetime;
alter table cyclecountdet add Memo varchar(255);
alter table cyclecountresult add Memo varchar(255);


/****** 对象:  View [dbo].[MenuView]    脚本日期: 11/24/2010 15:21:07 ******/
SELECT     Menu1.Code, Menu1.Version, Menu1.Desc_, Menu1.PageUrl, Menu1.IsActive, Menu1.ImageUrl, Menu1.Remark, 
                      dbo.ACC_MenuCommon.Id AS MenuRelationId, 'ACC_MenuCommon' AS Type, '' AS IndustryOrCompanyCode, ParentMenu1.Code AS ParentCode, 
                      ParentMenu1.Version AS ParenVersion, dbo.ACC_MenuCommon.Level_, dbo.ACC_MenuCommon.Seq, 
                      dbo.ACC_MenuCommon.IsActive AS MenuRelationIsActive, dbo.ACC_MenuCommon.CreateDate, dbo.ACC_MenuCommon.CreateUser, 
                      dbo.ACC_MenuCommon.LastModifyDate, dbo.ACC_MenuCommon.LastModifyUser
FROM         dbo.ACC_MenuCommon INNER JOIN
                      dbo.ACC_Menu AS Menu1 ON Menu1.Code = dbo.ACC_MenuCommon.Menu LEFT OUTER JOIN
                      dbo.ACC_Menu AS ParentMenu1 ON dbo.ACC_MenuCommon.ParentMenu = ParentMenu1.Code
WHERE     (NOT EXISTS
                          (SELECT     1 AS Expr1
                            FROM          dbo.ACC_MenuIndustry INNER JOIN
                                                   dbo.ACC_Industry ON dbo.ACC_MenuIndustry.Industry = dbo.ACC_Industry.Code INNER JOIN
                                                   dbo.ACC_Company ON dbo.ACC_Industry.Code = dbo.ACC_Company.Industry INNER JOIN
                                                   dbo.EntityOpt ON dbo.ACC_Company.Code = dbo.EntityOpt.PreValue INNER JOIN
                                                   dbo.ACC_Menu ON dbo.ACC_MenuIndustry.Menu = dbo.ACC_Menu.Code
                            WHERE      (dbo.EntityOpt.PreCode = 'CompanyCode') AND (dbo.ACC_Menu.Code = Menu1.Code))) AND (NOT EXISTS
                          (SELECT     1 AS Expr1
                            FROM          dbo.ACC_MenuCompany INNER JOIN
                                                   dbo.ACC_Company AS ACC_Company_3 ON dbo.ACC_MenuCompany.Company = ACC_Company_3.Code INNER JOIN
                                                   dbo.EntityOpt AS EntityOpt_3 ON ACC_Company_3.Code = EntityOpt_3.PreValue INNER JOIN
                                                   dbo.ACC_Menu AS ACC_Menu_2 ON dbo.ACC_MenuCompany.Menu = ACC_Menu_2.Code
                            WHERE      (EntityOpt_3.PreCode = 'CompanyCode') AND (Menu1.Code = ACC_Menu_2.Code)))
UNION
SELECT     Menu2.Code, Menu2.Version, Menu2.Desc_, Menu2.PageUrl, Menu2.IsActive, Menu2.ImageUrl, Menu2.Remark, 
                      ACC_MenuIndustry_1.Id AS MenuRelationId, 'ACC_MenuIndustry' AS Type, ACC_MenuIndustry_1.Industry AS IndustryOrCompanyCode, 
                      ParentMenu2.Code AS ParentCode, ParentMenu2.Version AS ParentVersion, ACC_MenuIndustry_1.Level_, ACC_MenuIndustry_1.Seq, 
                      ACC_MenuIndustry_1.IsActive AS MenuRelationIsActive, ACC_MenuIndustry_1.CreateDate, ACC_MenuIndustry_1.CreateUser, 
                      ACC_MenuIndustry_1.LastModifyDate, ACC_MenuIndustry_1.LastModifyUser
FROM         dbo.ACC_MenuIndustry AS ACC_MenuIndustry_1 INNER JOIN
                      dbo.ACC_Industry AS ACC_Industry_1 ON ACC_MenuIndustry_1.Industry = ACC_Industry_1.Code INNER JOIN
                      dbo.ACC_Company AS ACC_Company_4 ON ACC_Company_4.Industry = ACC_Industry_1.Code INNER JOIN
                      dbo.EntityOpt AS EntityOpt_4 ON ACC_Company_4.Code = EntityOpt_4.PreValue INNER JOIN
                      dbo.ACC_Menu AS Menu2 ON Menu2.Code = ACC_MenuIndustry_1.Menu LEFT OUTER JOIN
                      dbo.ACC_Menu AS ParentMenu2 ON ACC_MenuIndustry_1.ParentMenu = ParentMenu2.Code
WHERE     (EntityOpt_4.PreCode = 'CompanyCode') AND (NOT EXISTS
                          (SELECT     1 AS Expr1
                            FROM          dbo.ACC_MenuCompany AS ACC_MenuCompany_2 INNER JOIN
                                                   dbo.ACC_Company AS ACC_Company_2 ON ACC_MenuCompany_2.Company = ACC_Company_2.Code INNER JOIN
                                                   dbo.EntityOpt AS EntityOpt_2 ON ACC_Company_2.Code = EntityOpt_2.PreValue INNER JOIN
                                                   dbo.ACC_Menu AS ACC_Menu_1 ON ACC_MenuCompany_2.Menu = ACC_Menu_1.Code
                            WHERE      (EntityOpt_2.PreCode = 'CompanyCode') AND (ACC_Menu_1.Code = Menu2.Code)))
UNION
SELECT     Menu3.Code, Menu3.Version, Menu3.Desc_, Menu3.PageUrl, Menu3.IsActive, Menu3.ImageUrl, Menu3.Remark, 
                      ACC_MenuCompany_1.Id AS MenuRelationId, 'ACC_MenuCompany' AS Type, ACC_MenuCompany_1.Company AS IndustryOrCompanyCode, 
                      ParentMenu3.Code AS ParentCode, ParentMenu3.Version AS ParentVersion, ACC_MenuCompany_1.Level_, ACC_MenuCompany_1.Seq, 
                      ACC_MenuCompany_1.IsActive AS MenuRelationIsActive, ACC_MenuCompany_1.CreateDate, ACC_MenuCompany_1.CreateUser, 
                      ACC_MenuCompany_1.LastModifyDate, ACC_MenuCompany_1.LastModifyUser
FROM         dbo.ACC_MenuCompany AS ACC_MenuCompany_1 INNER JOIN
                      dbo.ACC_Menu AS Menu3 ON ACC_MenuCompany_1.Menu = Menu3.Code INNER JOIN
                      dbo.ACC_Company AS ACC_Company_1 ON ACC_MenuCompany_1.Company = ACC_Company_1.Code INNER JOIN
                      dbo.EntityOpt AS EntityOpt_1 ON ACC_Company_1.Code = EntityOpt_1.PreValue LEFT OUTER JOIN
                      dbo.ACC_Menu AS ParentMenu3 ON ACC_MenuCompany_1.ParentMenu = ParentMenu3.Code
WHERE     (EntityOpt_1.PreCode = 'CompanyCode')



--begin liqiuyun 20101123 增加 产品篿菜�?
INSERT ACC_Permission VALUES ('Menu.ItemCategory','产品�?MasterData')
INSERT acc_menu VALUES ('ItemCategory.166','Menu.ItemCategory.166',1,'Menu.ItemCategory','Menu.ItemCategory.Description','产品�?~/MasterData/ItemCategory/Default.aspx',1,'~/Images/Nav/ItemCategory.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
INSERT ACC_MenuCommon VALUES ('ItemCategory.166',11,2,27,1,'2010-09-03 00:00:00',null,'2010-09-03 00:00:00',null)

--end liqiuyun 20101123

alter table OrderDet add Remark varchar(50);


--修复供应商菜单bug
update acc_permission set pm_code='~/Main.aspx?mid=Warehouse.InProcessLocation__mp--ModuleType-Procurement_Action-View_IsSupplier-true'
where pm_code='~/Main.aspx?mid=Warehouse.InProcessLocation__mp--ModuleType-Distribution_Action-View_IsSupplier-true'

update acc_menu set pageurl='~/Main.aspx?mid=Warehouse.InProcessLocation__mp--ModuleType-Procurement_Action-View_IsSupplier-true'
where pageurl='~/Main.aspx?mid=Warehouse.InProcessLocation__mp--ModuleType-Distribution_Action-View_IsSupplier-true'
--


----增加发货单模晿INSERT INTO [Sconit_Xng_Test].[dbo].[CodeMstr]([Code],[CodeValue],[Seq],[IsDefault],[Desc1])
     VALUES('OrderTemplate','DeliveryOrder.xls',40,0,'发货单模�?

--beging 盘点
alter table CycleCountMstr add Bins varchar(max);
alter table CycleCountMstr add Items varchar(max);
alter table CycleCountMstr add IsScanHu bit;
alter table CycleCountMstr add StartUser varchar(50);
alter table CycleCountMstr add StartDate datetime;
alter table CycleCountMstr add CompleteUser varchar(50);
alter table CycleCountMstr add CompleteDate datetime;
alter table CycleCountResult add IsProcess bit;

ALTER TABLE [dbo].[CycleCountMstr]  WITH CHECK ADD  CONSTRAINT [FK_CycleCountMstr_ACC_User5] FOREIGN KEY([StartUser])
REFERENCES [dbo].[ACC_User] ([USR_Code]);

ALTER TABLE [dbo].[CycleCountMstr]  WITH CHECK ADD  CONSTRAINT [FK_CycleCountMstr_ACC_User6] FOREIGN KEY([CompleteUser])
REFERENCES [dbo].[ACC_User] ([USR_Code]);

delete from codemstr where code = 'PhysicalCountType' and codevalue = 'SpotCheck';
GO
update ACC_Menu set PageUrl='~/Main.aspx?mid=Inventory.Stocktaking' where Id='94';
update dbo.ACC_Permission set pm_code='~/Main.aspx?mid=Inventory.Stocktaking' where pm_id=541;
GO
--end 




--增加默认班次
INSERT INTO "EntityOpt" (PreCode,PreValue,CodeDesc,Seq) VALUES ('DefaultShift','A','默认班次',0)

--nng增加工单自动完工程序
INSERT INTO [dbo].[BatchJobDet]([Name],[Desc1],[ServiceName]) VALUES ('WOCompleteJob','Job of Automatic Complete WorkOrders','OrderCompleteJob');
INSERT INTO [dbo].[BatchTrigger]([Name],[Desc1],[JobId],[NextFireTime],[PrevFireTime],[RepeatCount],[Interval],[IntervalType],[TimesTriggered],[Status]) VALUES ('WOCompleteTrigger','Trigger of Wo Complete',5,'2010-11-1',null,0,1,'Days',0,'Pause')


--begin tiansu 20101104  销售单折扣精度bug

alter table orderdet alter column DiscountTo decimal(18, 8);
alter table ordermstr alter column DiscountTo decimal(18, 8);
GO

--end tiansu 


--begin wangxiang 10/27/2010 修改订单明细视图，显示米摿SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[OrderDetView]
AS
SELECT     MAX(dbo.OrderDet.Id) AS Id, dbo.OrderMstr.Flow, dbo.FlowMstr.Desc1, dbo.OrderMstr.Type, dbo.OrderMstr.PartyFrom, dbo.OrderMstr.PartyTo, 
                      CONVERT(datetime, CONVERT(varchar(8), dbo.OrderMstr.StartTime, 112)) AS EffDate, dbo.OrderMstr.Shift, dbo.OrderDet.Item, dbo.OrderDet.Uom, 
                      SUM(dbo.OrderDet.ReqQty) AS ReqQty, SUM(dbo.OrderDet.OrderQty) AS OrderQty, ISNULL(SUM(dbo.OrderDet.ShipQty), 0) AS ShipQty, 
                      ISNULL(SUM(dbo.OrderDet.RecQty), 0) AS RecQty, ISNULL(SUM(dbo.OrderDet.RejQty), 0) AS RejQty, ISNULL(SUM(dbo.OrderDet.ScrapQty), 0) 
                      AS ScrapQty, dbo.OrderMstr.Status, ISNULL(SUM(dbo.OrderDet.NumField1), 0) AS NumField1
FROM         dbo.OrderDet INNER JOIN
                      dbo.OrderMstr ON dbo.OrderDet.OrderNo = dbo.OrderMstr.OrderNo INNER JOIN
                      dbo.FlowMstr ON dbo.OrderMstr.Flow = dbo.FlowMstr.Code
GROUP BY dbo.OrderMstr.Flow, dbo.FlowMstr.Desc1, dbo.OrderMstr.Type, dbo.OrderMstr.PartyFrom, dbo.OrderMstr.PartyTo, CONVERT(varchar(8), 
                      dbo.OrderMstr.StartTime, 112), dbo.OrderMstr.Shift, dbo.OrderDet.Item, dbo.OrderDet.Uom, dbo.OrderMstr.Status, dbo.OrderDet.NumField1
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
--end wangxiang 10/27/2010

--begin liqiuyun  10/25/2010  修改账单
alter table Actbill alter column LocFrom varchar(50)
alter table Actbill alter column IpNo  varchar(50)
alter table Actbill add Flow varchar(50);

alter table BillDet alter column LocFrom varchar(50)
alter table BillDet alter column IpNo  varchar(50)
alter table BillDet alter column RefItemCode  varchar(50)
alter table BillDet add Flow varchar(50);

alter table PlanBill alter column LocFrom varchar(50)
alter table PlanBill alter column IpNo  varchar(50)
alter table PlanBill add Flow varchar(50);
--end liqiuyun  10/25/2010  修改账单

--begin liqiuyun  10/19/2010  修改外部订单勿alter table Ordermstr alter column extOrderNo varchar(255)
alter table Receiptmstr alter column extrecno  varchar(255)
--end liqiuyun

/****** 对象:  View [dbo].[LocBinItemDet]    脚本日期: 10/19/2010 08:04:20  liqiuyun******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[LocBinItemDet]
AS
SELECT     MAX(dbo.LocationLotDet.Id) AS Id, dbo.LocationLotDet.Location, dbo.LocationLotDet.Bin, dbo.LocationLotDet.Item, 
                      dbo.Item.Desc1 + ISNULL(' [' + dbo.Item.Desc2 + ']', '') AS ItemDesc, dbo.Item.UC, dbo.Item.Uom, SUM(dbo.LocationLotDet.Qty) AS Qty
FROM         dbo.LocationLotDet INNER JOIN
                      dbo.Item ON dbo.LocationLotDet.Item = dbo.Item.Code
WHERE     (dbo.LocationLotDet.Qty <> 0)
GROUP BY dbo.LocationLotDet.Location, dbo.LocationLotDet.Bin, dbo.LocationLotDet.Item, dbo.Item.Desc1 + ISNULL(' [' + dbo.Item.Desc2 + ']', ''), dbo.Item.UC, 
                      dbo.Item.Uom

--begin wangxiang 20100925 修改菜单
INSERT ACC_Permission VALUES ('~/Main.aspx?mid=Visualization.ProductLineInprocessLocation','生产投料明细','Visualization');
INSERT acc_menu VALUES ('165','Menu.ProductLineInprocessLocation.165',1,'Menu.ProductLineInprocessLocation','Menu.ProductLineInprocessLocation.Description','生产投料明细','~/Main.aspx?mid=Visualization.ProductLineInprocessLocation',1,'~/Images/Nav/ViewWOIP.png',getdate(),null,getdate(),null,null)
INSERT ACC_MenuCommon VALUES ('165','134',3,150,1,getdate(),null,getdate(),null)
--end wangxiang

delete from codemstr where code='FlowType' and codevalue='Inspection'
delete from codemstr where code='OrderType' and codevalue='Inspection'

alter table location add IsSetCS bit;
update location set IsSetCS = 0;

--begin tiansu 20100913 修改菜单
update acc_menu set id='Menu.POPlanBill.165',Code='Menu.POPlanBill.165' where id='164';
GO
update ACC_MenuCommon set MenuId='Menu.POPlanBill.165'  where MenuId='164';
GO
--end tiansu 20100913


--begin liqiuyun 20100913 增加供应哿供应商寄咿菜单
INSERT ACC_Permission VALUES ('~/Main.aspx?mid=Finance.PlanBill.PO__mp--ModuleType-PO_IsSupplier-true','供应商寄�?SupplierMenu')
INSERT acc_menu VALUES ('164','Menu.POPlanBill.164',1,'Menu.POPlanBill','Menu.POPlanBill.Description','供应商寄�?~/Main.aspx?mid=Finance.PlanBill.PO__mp--ModuleType-PO_IsSupplier-true',1,'~/Images/Nav/POPlanBill.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null)
INSERT ACC_MenuCommon VALUES ('164','142',2,289,1,'2010-09-03 00:00:00',null,'2010-09-03 00:00:00',null)
--end liqiuyun 20100913




alter table HuDet add HuTemplate varchar(50);




--begin liqiuyun 20100903 权限修改
update acc_permissioncategory set pmc_desc='目视管理' where pmc_code='Visualization';
update ACC_PERMISSION set  pm_desc='委外路线' where  pm_desc='委外加工';
insert into acc_permissioncategory(PMC_Code,PMC_Desc,PMC_Type) values ('Quality','质量管理','Menu');
update acc_permission set pm_catecode='Quality' where pm_code ='~/Main.aspx?mid=Order.OrderHead.Procurement__mp--ModuleType-Procurement_ModuleSubType-Rtn_StatusGroupId-1_IsQuick-true__act--NewAction';
update acc_permission set pm_catecode='Quality' where pm_code ='~/Main.aspx?mid=Order.OrderHead.Procurement__mp--ModuleType-Procurement_ModuleSubType-Rtn_StatusGroupId-1_IsQuick-true_IsReject-true__act--NewAction';
update acc_permission set pm_catecode='Quality' where pm_code ='~/Main.aspx?mid=Order.OrderHead.Production__mp--ModuleType-Production_ModuleSubType-Rwo_StatusGroupId-1__act--NewAction';
update acc_permission set pm_catecode='Quality' where pm_code ='~/Main.aspx?mid=Production.WorkshopScrap';
update acc_permission set pm_catecode='Quality' where pm_code ='~/Main.aspx?mid=Order.OrderHead.Production__mp--ModuleType-Production_ModuleSubType-Adj_StatusGroupId-1_IsScrap-true__act--NewAction';
update acc_permission set pm_catecode='Quality' where pm_code ='~/Main.aspx?mid=Order.OrderHead.Production__mp--ModuleType-Production_ModuleSubType-Rwo_StatusGroupId-1_IsScrap-true_IsReuse-true__act--NewAction';
update acc_permission set pm_catecode='Quality' where pm_code ='~/Main.aspx?mid=Order.OrderHead.Distribution__mp--ModuleType-Distribution_ModuleSubType-Rtn_StatusGroupId-1_IsQuick-true__act--NewAction';
update acc_permission set pm_catecode='Quality' where pm_code ='~/Main.aspx?mid=Inventory.InspectOrder';
update acc_permission set pm_catecode='Quality' where pm_code ='~/Main.aspx?mid=Order.OrderHead.Transfer__mp--ModuleType-Transfer_ModuleSubType-Rtn_StatusGroupId-1_IsQuick-true_IsReject-true__act--NewAction';
update acc_permission set pm_desc='让步使用' where pm_code ='~/Main.aspx?mid=Order.OrderHead.Transfer__mp--ModuleType-Transfer_ModuleSubType-Rtn_StatusGroupId-1_IsQuick-true_IsReject-true__act--NewAction';
insert acc_permission values('~/Main.aspx?mid=Warehouse.InProcessLocation__mp--ModuleType-Distribution_Action-View_AsnType-Gap','收货差异处理','Quality');
GO
--end liqiuyun 20100903




--begin tiansu 20100903 添加面套模板
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('HuTemplate','BarCodeShellFabric.xls',30,0,'条码模板(面套)');
GO
update acc_menu set pageurl='' where id=32;
GO
update acc_menucommon set isactive=0 where menuid=32;
GO

INSERT INTO "dbo"."ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('163','Menu.GRAdjustment.163',1,'Menu.GRAdjustment','Menu.GRAdjustment.Description','收货调整','~/Main.aspx?mid=Order.ReceiptNotes__mp--ModuleType-Procurement_ModuleSubType-Adj',1,'~/Images/Nav/GRAdjustment.png','2010-09-03 00:00:00',null,'2010-09-03 00:00:00',null,null)
GO

set IDENTITY_INSERT ACC_MenuCommon on;
INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (160,'163','27',3,97,1,'2010-09-03 00:00:00',null,'2010-09-03 00:00:00',null)
set IDENTITY_INSERT ACC_MenuCommon off;
--end tiansu 20100903


--begin tiansu 20100902 就改菜单视图

/****** 对象:  View [dbo].[MenuView]    脚本日期: 09/02/2010 09:52:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[MenuView]
AS
SELECT     Menu1.Id, Menu1.Code, Menu1.Version, Menu1.Title, Menu1.Desc_, Menu1.Description, Menu1.PageUrl, Menu1.IsActive, Menu1.ImageUrl, 
                      Menu1.Remark, dbo.ACC_MenuCommon.Id AS MenuRelationId, 'ACC_MenuCommon' AS Type, '' AS IndustryOrCompanyCode, 
                      dbo.ACC_MenuCommon.ParentMenuId AS ParentId, ParentMenu1.Code AS ParentCode, ParentMenu1.Version AS ParenVersion, 
                      dbo.ACC_MenuCommon.Level_, dbo.ACC_MenuCommon.Seq, dbo.ACC_MenuCommon.IsActive AS MenuRelationIsActive, 
                      dbo.ACC_MenuCommon.CreateDate, dbo.ACC_MenuCommon.CreateUser, dbo.ACC_MenuCommon.LastModifyDate, 
                      dbo.ACC_MenuCommon.LastModifyUser
FROM         dbo.ACC_MenuCommon INNER JOIN
                      dbo.ACC_Menu AS Menu1 ON Menu1.Id = dbo.ACC_MenuCommon.MenuId LEFT OUTER JOIN
                      dbo.ACC_Menu AS ParentMenu1 ON dbo.ACC_MenuCommon.ParentMenuId = ParentMenu1.Id
WHERE     (NOT EXISTS
                          (SELECT     1 AS Expr1
                            FROM          dbo.ACC_MenuIndustry INNER JOIN
                                                   dbo.ACC_Industry ON dbo.ACC_MenuIndustry.IndustryCode = dbo.ACC_Industry.Code INNER JOIN
                                                   dbo.ACC_Company ON dbo.ACC_Industry.Code = dbo.ACC_Company.IndustryCode INNER JOIN
                                                   dbo.EntityOpt ON dbo.ACC_Company.Code = dbo.EntityOpt.PreValue INNER JOIN
                                                   dbo.ACC_Menu ON dbo.ACC_MenuIndustry.MenuId = dbo.ACC_Menu.Id
                            WHERE      (dbo.EntityOpt.PreCode = 'CompanyCode') AND (dbo.ACC_Menu.Code = Menu1.Code))) AND (NOT EXISTS
                          (SELECT     1 AS Expr1
                            FROM          dbo.ACC_MenuCompany INNER JOIN
                                                   dbo.ACC_Company AS ACC_Company_3 ON dbo.ACC_MenuCompany.CompanyCode = ACC_Company_3.Code INNER JOIN
                                                   dbo.EntityOpt AS EntityOpt_3 ON ACC_Company_3.Code = EntityOpt_3.PreValue INNER JOIN
                                                   dbo.ACC_Menu AS ACC_Menu_2 ON dbo.ACC_MenuCompany.MenuId = ACC_Menu_2.Id
                            WHERE      (EntityOpt_3.PreCode = 'CompanyCode') AND (Menu1.Code = ACC_Menu_2.Code)))
UNION
SELECT     Menu2.Id, Menu2.Code, Menu2.Version, Menu2.Title, Menu2.Desc_, Menu2.Description, Menu2.PageUrl, Menu2.IsActive, Menu2.ImageUrl, 
                      Menu2.Remark, ACC_MenuIndustry_1.Id AS MenuRelationId, 'ACC_MenuIndustry' AS Type, 
                      ACC_MenuIndustry_1.IndustryCode AS IndustryOrCompanyCode, ACC_MenuIndustry_1.ParentMenuId AS ParentId, ParentMenu2.Code AS ParentCode, 
                      ParentMenu2.Version AS ParentVersion, ACC_MenuIndustry_1.Level_, ACC_MenuIndustry_1.Seq, 
                      ACC_MenuIndustry_1.IsActive AS MenuRelationIsActive, ACC_MenuIndustry_1.CreateDate, ACC_MenuIndustry_1.CreateUser, 
                      ACC_MenuIndustry_1.LastModifyDate, ACC_MenuIndustry_1.LastModifyUser
FROM         dbo.ACC_MenuIndustry AS ACC_MenuIndustry_1 INNER JOIN
                      dbo.ACC_Industry AS ACC_Industry_1 ON ACC_MenuIndustry_1.IndustryCode = ACC_Industry_1.Code INNER JOIN
                      dbo.ACC_Company AS ACC_Company_4 ON ACC_Company_4.IndustryCode = ACC_Industry_1.Code INNER JOIN
                      dbo.EntityOpt AS EntityOpt_4 ON ACC_Company_4.Code = EntityOpt_4.PreValue INNER JOIN
                      dbo.ACC_Menu AS Menu2 ON Menu2.Id = ACC_MenuIndustry_1.MenuId LEFT OUTER JOIN
                      dbo.ACC_Menu AS ParentMenu2 ON ACC_MenuIndustry_1.ParentMenuId = ParentMenu2.Id
WHERE     (EntityOpt_4.PreCode = 'CompanyCode') AND (NOT EXISTS
                          (SELECT     1 AS Expr1
                            FROM          dbo.ACC_MenuCompany AS ACC_MenuCompany_2 INNER JOIN
                                                   dbo.ACC_Company AS ACC_Company_2 ON ACC_MenuCompany_2.CompanyCode = ACC_Company_2.Code INNER JOIN
                                                   dbo.EntityOpt AS EntityOpt_2 ON ACC_Company_2.Code = EntityOpt_2.PreValue INNER JOIN
                                                   dbo.ACC_Menu AS ACC_Menu_1 ON ACC_MenuCompany_2.MenuId = ACC_Menu_1.Id
                            WHERE      (EntityOpt_2.PreCode = 'CompanyCode') AND (ACC_Menu_1.Code = Menu2.Code)))
UNION
SELECT     Menu3.Id, Menu3.Code, Menu3.Version, Menu3.Title, Menu3.Desc_, Menu3.Description, Menu3.PageUrl, Menu3.IsActive, Menu3.ImageUrl, 
                      Menu3.Remark, ACC_MenuCompany_1.Id AS MenuRelationId, 'ACC_MenuCompany' AS Type, 
                      ACC_MenuCompany_1.CompanyCode AS IndustryOrCompanyCode, ACC_MenuCompany_1.ParentMenuId AS ParentId, 
                      ParentMenu3.Code AS ParentCode, ParentMenu3.Version AS ParentVersion, ACC_MenuCompany_1.Level_, ACC_MenuCompany_1.Seq, 
                      ACC_MenuCompany_1.IsActive AS MenuRelationIsActive, ACC_MenuCompany_1.CreateDate, ACC_MenuCompany_1.CreateUser, 
                      ACC_MenuCompany_1.LastModifyDate, ACC_MenuCompany_1.LastModifyUser
FROM         dbo.ACC_MenuCompany AS ACC_MenuCompany_1 INNER JOIN
                      dbo.ACC_Menu AS Menu3 ON ACC_MenuCompany_1.MenuId = Menu3.Id INNER JOIN
                      dbo.ACC_Company AS ACC_Company_1 ON ACC_MenuCompany_1.CompanyCode = ACC_Company_1.Code INNER JOIN
                      dbo.EntityOpt AS EntityOpt_1 ON ACC_Company_1.Code = EntityOpt_1.PreValue LEFT OUTER JOIN
                      dbo.ACC_Menu AS ParentMenu3 ON ACC_MenuCompany_1.ParentMenuId = ParentMenu3.Id
WHERE     (EntityOpt_1.PreCode = 'CompanyCode')

--end tiansu 20100902




ALTER  table   dbo.HuDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetNextSequence]
	@CodePrefix varchar(50),
	@NextSequence int OUTPUT
AS
Begin Tran
	Declare @invValue int;
	select  @invValue = IntValue FROM NumCtrl WITH (UPDLOCK, ROWLOCK) where Code = @CodePrefix;
	if @invValue is null
	begin
		if @NextSequence is not null
		begin 
			insert into NumCtrl(Code, IntValue) values (@CodePrefix, @NextSequence + 1);
		end	
		else
		begin
			set @NextSequence = 1;
			insert into NumCtrl(Code, IntValue) values (@CodePrefix, 2);
		end
	end 
	else
	begin
		if @NextSequence is not null
		begin 
			if @invValue <= @NextSequence
			begin
				update NumCtrl set IntValue = @NextSequence + 1 where Code = @CodePrefix;
			end
		end
		else
		begin
			set @NextSequence = @invValue;
			update NumCtrl set IntValue = @NextSequence + 1 where Code = @CodePrefix;
		end
	end	
Commit tran

alter table FlowDet add ExtraDmdSource varchar(255);

alter table locationlotdet add RefLoc varchar(50);

ALTER table dbo.BillTrans
add	[BillDet] int NULL;

ALTER  table   dbo.OrderMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[TextField5] [varchar](255) NULL,
	[TextField6] [varchar](255) NULL,
	[TextField7] [varchar](255) NULL,
	[TextField8] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[NumField5] [decimal](18, 8) NULL,
	[NumField6] [decimal](18, 8) NULL,
	[NumField7] [decimal](18, 8) NULL,
	[NumField8] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL,
	[DateField3] [datetime] NULL,
	[DateField4] [datetime] NULL;
	
	
ALTER  table   dbo.OrderDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[TextField5] [varchar](255) NULL,
	[TextField6] [varchar](255) NULL,
	[TextField7] [varchar](255) NULL,
	[TextField8] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[NumField5] [decimal](18, 8) NULL,
	[NumField6] [decimal](18, 8) NULL,
	[NumField7] [decimal](18, 8) NULL,
	[NumField8] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL,
	[DateField3] [datetime] NULL,
	[DateField4] [datetime] NULL;
	
	
ALTER  table   dbo.FlowMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[TextField5] [varchar](255) NULL,
	[TextField6] [varchar](255) NULL,
	[TextField7] [varchar](255) NULL,
	[TextField8] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[NumField5] [decimal](18, 8) NULL,
	[NumField6] [decimal](18, 8) NULL,
	[NumField7] [decimal](18, 8) NULL,
	[NumField8] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL,
	[DateField3] [datetime] NULL,
	[DateField4] [datetime] NULL;
	
ALTER  table   dbo.FlowDet
add	[ExtraDmdSource] [varchar](255) Null,
    [TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[TextField5] [varchar](255) NULL,
	[TextField6] [varchar](255) NULL,
	[TextField7] [varchar](255) NULL,
	[TextField8] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[NumField5] [decimal](18, 8) NULL,
	[NumField6] [decimal](18, 8) NULL,
	[NumField7] [decimal](18, 8) NULL,
	[NumField8] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL,
	[DateField3] [datetime] NULL,
	[DateField4] [datetime] NULL;
	
	

ALTER  table   dbo.BillMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
	
ALTER  table   dbo.BillDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
	
ALTER  table   dbo.InspectMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
	
ALTER  table   dbo.InspectDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
	
ALTER  table   dbo.IpMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.IpDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.PickListMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.PickListDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.PriceListMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.PriceListDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.ReceiptMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.ReceiptDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
	
ALTER  table   dbo.Item
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
	
ALTER  table   dbo.Location
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.Party
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.PartyAddr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
go


--
-- Script To Create dbo.OrderTracer Table
-- Generated 星期䴿八�?25, 2010, at 02:58 PM
--
-- Author: Deng Xuyao
--
BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Creating dbo.OrderTracer Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

CREATE TABLE [dbo].[OrderTracer] (
   [Id] [int] IDENTITY (1, 1) NOT NULL,
   [OrderDetId] [int] NOT NULL,
   [TracerType] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL,
   [Code] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL,
   [Item] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL,
   [ReqTime] [datetime] NOT NULL,
   [OrderQty] [decimal] (18, 8) NOT NULL CONSTRAINT [DF_OrderTracer_OrderQty] DEFAULT ((0)),
   [AccumQty] [decimal] (18, 8) NOT NULL CONSTRAINT [DF_OrderTracer_AccumQty] DEFAULT ((0)),
   [Qty] [decimal] (18, 8) NOT NULL CONSTRAINT [DF_OrderTracer_Qty] DEFAULT ((0)),
   [RefOrderLocTransId] [int] NOT NULL CONSTRAINT [DF_OrderTracer_RefOrderLocTransId] DEFAULT ((0)),
   [Memo] [varchar] (255) COLLATE Chinese_PRC_CI_AS NULL
)
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[OrderTracer] ADD CONSTRAINT [PK_OrderTracer] PRIMARY KEY CLUSTERED ([Id])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_OrderTracer] ON [dbo].[OrderTracer] ([OrderDetId])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.OrderTracer Table Added Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Add dbo.OrderTracer Table'
END
GO

BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Creating dbo.OrderTracer Table'
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF NOT EXISTS (SELECT name FROM sysobjects WHERE name = N'FK_OrderTracer_OrderDet')
      ALTER TABLE [dbo].[OrderTracer] ADD CONSTRAINT [FK_OrderTracer_OrderDet] FOREIGN KEY ([OrderDetId]) REFERENCES [dbo].[OrderDet] ([Id])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.OrderTracer Table Added Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Add dbo.OrderTracer Table'
END
GO


--
-- Script To Update dbo.LeanEngineView View 
-- Generated 星期嗿八�?26, 2010, at 08:40 AM
--
-- Author: Deng Xuyao
--
BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.LeanEngineView View'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   exec sp_dropextendedproperty N'MS_DiagramPane1', 'Schema', N'dbo', 'View', N'LeanEngineView'
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   exec sp_dropextendedproperty N'MS_DiagramPaneCount', 'Schema', N'dbo', 'View', N'LeanEngineView'
GO

exec('ALTER VIEW dbo.LeanEngineView
AS
SELECT     dbo.FlowView.FlowDetId, dbo.FlowView.Flow, dbo.FlowView.IsAutoCreate, dbo.FlowView.LocFrom, dbo.FlowView.LocTo, dbo.FlowDet.Item, 
                      dbo.FlowDet.Uom, dbo.FlowDet.UC, dbo.FlowDet.HuLotSize, dbo.FlowDet.Bom, dbo.FlowDet.SafeStock, dbo.FlowDet.MaxStock, 
                      dbo.FlowDet.MinLotSize, dbo.FlowDet.OrderLotSize, dbo.FlowDet.BatchSize, dbo.FlowDet.RoundUpOpt, dbo.FlowMstr.Type, dbo.FlowMstr.PartyFrom, 
                      dbo.FlowMstr.PartyTo, dbo.FlowMstr.FlowStrategy, dbo.FlowMstr.LeadTime, dbo.FlowMstr.EmTime, dbo.FlowMstr.MaxCirTime, 
                      dbo.FlowMstr.WinTime1, dbo.FlowMstr.WinTime2, dbo.FlowMstr.WinTime3, dbo.FlowMstr.WinTime4, dbo.FlowMstr.WinTime5, 
                      dbo.FlowMstr.WinTime6, dbo.FlowMstr.WinTime7, dbo.FlowMstr.NextOrderTime, dbo.FlowMstr.NextWinTime, dbo.FlowMstr.WeekInterval, 
                      dbo.FlowDet.ExtraDmdSource
FROM         dbo.FlowView INNER JOIN
                      dbo.FlowDet ON dbo.FlowView.FlowDetId = dbo.FlowDet.Id INNER JOIN
                      dbo.FlowMstr ON dbo.FlowDet.Flow = dbo.FlowMstr.Code')
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   exec sp_addextendedproperty  N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "FlowView"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 166
               Right = 184
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FlowDet"
            Begin Extent = 
               Top = 6
               Left = 222
               Bottom = 191
               Right = 396
            End
            DisplayFlags = 280
            TopColumn = 36
         End
         Begin Table = "FlowMstr"
            Begin Extent = 
               Top = 6
               Left = 434
               Bottom = 211
               Right = 618
            End
            DisplayFlags = 280
            TopColumn = 69
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', 'Schema', N'dbo', 'View', N'LeanEngineView'
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   exec sp_addextendedproperty  N'MS_DiagramPaneCount', N'1', 'Schema', N'dbo', 'View', N'LeanEngineView'
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.LeanEngineView View Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.LeanEngineView View'
END
GO






















































select item, min(case when pricelistdet.isincludetax = 1 then unitprice / 1.17 else unitprice end) 
as unitprice into #pricelist from pricelistdet
inner join pricelistmstr on pricelistdet.pricelist = pricelistmstr.code
where pricelistmstr.type = 'Purchase'
group by item

--------RM---------
insert c_balance 
select loctrans.item,loctrans.uom,item.category,sum(qty) as qty
,isnull(a.unitprice*sum(qty),0) as Amount
,isnull(a.unitprice,0) as cost
,'2011-5' as fc,0 as isprovest,
getdate() as createtime,'su' as createuser
 from loctrans
left join item on item.code = loctrans.item
left join #pricelist a on loctrans.item = a.item
where effdate <'2011-6-1' and item.category like '%RM%'  
group by loctrans.item,loctrans.uom,item.category,a.unitprice