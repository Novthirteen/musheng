

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_ToolingInfo') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_ToolingInfo
(
TL_No varchar(50) primary key,
TL_Name varchar(255),
TL_Spec varchar(255),
Customer varchar(255),
ProductName varchar(255),
PCBNo varchar(50),
MSNo varchar(50),
Price decimal(18,2),
Number int,
ApplyDate datetime,
ApplyUser varchar(255),
ArriveDate datetime,
TL_SalesType varchar(255),
Suppliers varchar(50),
SuppliersInNo varchar(50),
CustomerBStatus bit,
ProjectNo varchar(50)
)

insert ACC_Menu values('Menu.Quote',1,'报价管理','',1,'',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Trans',1,'事务','',1,'~/Images/Nav/Transaction.png',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Info',1,'信息','',1,'~/Images/Nav/Information.png',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Setup',1,'设置','',1,'~/Images/Nav/Setup.png',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Info.Tooling',1,'工装信息','~/Main.aspx?mid=Quote.Tooling',1,'',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Info.ProductInfo',1,'产品信息','~/Main.aspx?mid=Quote.ProductInfo',1,'',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Info.Item',1,'物料','~/Main.aspx?mid=Quote.Item',1,'',getdate(),null,getdate(),null,null)
--insert ACC_Menu values('Menu.Quote.Info.Para',1,'参数','~/Main.aspx?mid=Quote.Para',1,'',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Setup.Template',1,'模板','~/Main.aspx?mid=Quote.Template',1,'',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Setup.CusTemplate',1,'客户模板','~/Main.aspx?mid=Quote.CusTemplate',1,'',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Trans.Quotes',1,'报价单','~/Main.aspx?mid=Quote.Quotes',1,'',getdate(),null,getdate(),null,null)

insert ACC_MenuCommon values('Menu.Quote','Menu.Home',1,437,1,getdate(),null,getdate(),null)
insert ACC_MenuCommon values('Menu.Quote.Trans','Menu.Quote',2,444,1,getdate(),null,getdate(),null)
insert ACC_MenuCommon values('Menu.Quote.Info','Menu.Quote',2,438,1,getdate(),null,getdate(),null)
insert ACC_MenuCommon values('Menu.Quote.Setup','Menu.Quote',2,441,1,getdate(),null,getdate(),null)
insert ACC_MenuCommon values('Menu.Quote.Info.Tooling','Menu.Quote.Info',3,439,1,getdate(),null,getdate(),null)
insert ACC_MenuCommon values('Menu.Quote.Info.ProductInfo','Menu.Quote.Info',3,440,1,getdate(),null,getdate(),null)
insert ACC_MenuCommon values('Menu.Quote.Info.Item','Menu.Quote.Info',3,446,1,getdate(),null,getdate(),null)
--insert ACC_MenuCommon values('Menu.Quote.Info.Para','Menu.Quote.Info',3,447,1,getdate(),null,getdate(),null)
insert ACC_MenuCommon values('Menu.Quote.Setup.Template','Menu.Quote.Setup',3,442,1,getdate(),null,getdate(),null)
insert ACC_MenuCommon values('Menu.Quote.Setup.CusTemplate','Menu.Quote.Setup',3,443,1,getdate(),null,getdate(),null)
insert ACC_MenuCommon values('Menu.Quote.Trans.Quotes','Menu.Quote.Trans',3,445,1,getdate(),null,getdate(),null)


insert ACC_PermissionCategory values('QuoteOperation','工装维护操作','Page')
insert ACC_PermissionCategory values('QuotePOperation','报价维护操作','Page')

insert ACC_Permission values('ProjectID','项目ID','QuoteOperation')
insert ACC_Permission values('TL_Name','工装名称','QuoteOperation')
insert ACC_Permission values('TL_Spec','工装规格','QuoteOperation')
insert ACC_Permission values('CustomerName','客户名称','QuoteOperation')
insert ACC_Permission values('ProductName','产品型号/名称','QuoteOperation')
insert ACC_Permission values('MSNo','慕盛图号','QuoteOperation')
insert ACC_Permission values('Price','价格(含税)','QuoteOperation')
insert ACC_Permission values('Number','数量','QuoteOperation')
insert ACC_Permission values('ApplyDate','申请日期','QuoteOperation')
insert ACC_Permission values('ApplyUser','申请方','QuoteOperation')
insert ACC_Permission values('ArriveDate','到货日期','QuoteOperation')
insert ACC_Permission values('TL_SalesType','工装销售形式','QuoteOperation')
insert ACC_Permission values('Supplier','供应商','QuoteOperation')
insert ACC_Permission values('SupplierInNo','供应商入库发票','QuoteOperation')
insert ACC_Permission values('PCBNo','PCB版本编号','QuoteOperation')
insert ACC_Permission values('CustomerBStatus','客户方结算状态','QuoteOperation')

insert ACC_PermissionCategory values('Quote','报价管理','Menu')
insert ACC_Permission values('Menu.Quote.Info.Tooling','工装信息','Quote')
insert ACC_Permission values('Menu.Quote.Info.ProductInfo','产品信息','Quote')
insert ACC_Permission values('Menu.Quote.Info.Item','物料','Quote')
--insert ACC_Permission values('Menu.Quote.Info.Para','参数','Quote')
insert ACC_Permission values('Menu.Quote.Setup.Template','模板','Quote')
insert ACC_Permission values('Menu.Quote.Setup.CusTemplate','客户模板','Quote')
insert ACC_Permission values('Menu.Quote.Trans.Quotes','报价单','Quote')


IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_CustomerInfo') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_CustomerInfo
(
Id int identity(1,1),
Code varchar(50),
Name varchar(255),			--客户名称
SType varchar(50),			--结算方式
BillPeriod int,				--账期(天)
P_LossRate varchar(50),		--费损率
P_ManageFee varchar(50),	--管理费用(加工费用)
P_FinanceFee varchar(50),	--财务费用(加工费用)
P_Profit varchar(50),		--利润(加工费用)
M_ManageFee varchar(50),	--管理费用(材料费用)
M_FinanceFee varchar(50),	--财务费用(材料费用)
DeliveryAdd varchar(255),	--交付地点
DeliveryCity varchar(50),	--交付所在地
Status bit,					--状态
StartDate datetime,			--启用时间
EndDate datetime			--结束时间
)


IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_SType') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_SType  --结算方式
(
Id int identity(1,1),
Name varchar(255)
)

insert Quote_SType values('收货结算')
insert Quote_SType values('上线结算')
insert Quote_SType values('下线结算')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_CooperationMode') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_CooperationMode  --合作方式
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_CooperationMode values('来料加工')
insert Quote_CooperationMode values('集成供货')
insert Quote_CooperationMode values('混合模式')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_QFor') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_QFor  --报价针对
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_QFor values('项目询价')
insert Quote_QFor values('一次性加工')
insert Quote_QFor values('批量')
insert Quote_QFor values('手工样件')
insert Quote_QFor values('小批量机贴')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_PT') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_PT  --加工工艺
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_PT values('有铅')
insert Quote_PT values('无铅')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_TSType') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_TSType  --工装结算方式
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_TSType values('一次性结算')
insert Quote_TSType values('分摊结算')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_BoardMode') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_BoardMode  --分板方式
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_BoardMode values('手工分板')
insert Quote_BoardMode values('直线分板')
insert Quote_BoardMode values('曲线分板')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_PackMode') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_PackMode  --包装方式
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_PackMode values('周转箱 + 气泡袋')
insert Quote_PackMode values('纸箱 + 气泡袋')
insert Quote_PackMode values('周转箱 + 吸塑')
insert Quote_PackMode values('纸箱 + 吸塑')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_LogisticsMode') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_LogisticsMode  --物流方式
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_LogisticsMode values('物流车')
insert Quote_LogisticsMode values('委外物流')
insert Quote_LogisticsMode values('快递')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_LogisticsFee') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_LogisticsFee  --物流费用
(
Id int identity(1,1),
CityName varchar(255),
Unit varchar(50),
Price decimal(18,2)
)
insert Quote_LogisticsFee values('上海','箱','12.00')
insert Quote_LogisticsFee values('南京','箱','18.00')
insert Quote_LogisticsFee values('十堰','箱','18.00')
insert Quote_LogisticsFee values('苏州','箱','15.00')
insert Quote_LogisticsFee values('芜湖','箱','18.00')
insert Quote_LogisticsFee values('乐清','箱','25.00')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_ToolingList') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_ToolingList  --工装清单
(
Id int identity(1,1),
Name varchar(255),
Unit varchar(50),
Price decimal(18,2)
)
insert Quote_ToolingList values('钢网板','块','650.00')
insert Quote_ToolingList values('点灯治具','套','2800.00')
insert Quote_ToolingList values('ICT治具','套','8500.00')
insert Quote_ToolingList values('波峰焊治具（石英石）','套','750.00')
insert Quote_ToolingList values('手工焊接治具','套','2000.00')
insert Quote_ToolingList values('曲线分板治具','套','2700.00')
insert Quote_ToolingList values('功能测试治具','套','0.00')
insert Quote_ToolingList values('表面涂覆治具','套','0.00')
insert Quote_ToolingList values('线路板制版','套','1500.00')
insert Quote_ToolingList values('吸塑模具_B','套','4300.00')
insert Quote_ToolingList values('吸塑模具_F','套','3800.00')
insert Quote_ToolingList values('吸塑模具_周转箱','套','4300.00')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_PFee') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_PFee  --加工费用
(
Id int identity(1,1),
Para1 varchar(255),
Para2 varchar(50),
Price1 decimal(18,4),
Price2 decimal(18,4)
)
insert Quote_PFee values('项目询价','有铅','0.0250','0.0300')
insert Quote_PFee values('项目询价','无铅','0.0280','0.0350')
insert Quote_PFee values('一次性加工','有铅','0.0250','0.0300')
insert Quote_PFee values('一次性加工','无铅','0.0280','0.0350')
insert Quote_PFee values('批量','有铅','0.0250','0.0300')
insert Quote_PFee values('批量','无铅','0.0280','0.0350')
insert Quote_PFee values('手工样件','有铅','0.0800','0.0000')
insert Quote_PFee values('手工样件','无铅','0.1000','0.0000')
insert Quote_PFee values('小批量机贴','有铅','0.0250','0.0300')
insert Quote_PFee values('小批量机贴','无铅','0.02800','0.0350')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_BootFee') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_BootFee  --开机费用
(
Id int identity(1,1),
Name1 varchar(255),
Name2 varchar(255),
Price decimal(18,2)
)
insert Quote_BootFee values('小批量机贴','AOI编程','200.00')
insert Quote_BootFee values('小批量机贴','贴片机编程','500.00')
insert Quote_BootFee values('ICT小批量机贴','贴片机调试','400.00')
insert Quote_BootFee values('小批量机贴','波峰焊调试','200.00')
insert Quote_BootFee values('小批量机贴','生产防错','200.00')
insert Quote_BootFee values('小批量机贴','辅助人工','150.00')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_CalculatePara') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_CalculatePara  --计算参数
(
Id int identity(1,1),
Name varchar(255),
Price decimal(18,2)
)
insert Quote_CalculatePara values('设备单价','0.00')
insert Quote_CalculatePara values('手工单价','0.00')
insert Quote_CalculatePara values('管理费用','0.00')
insert Quote_CalculatePara values('财务费用','0.00')
insert Quote_CalculatePara values('利润','0.00')
insert Quote_CalculatePara values('管理费用','0.00')
insert Quote_CalculatePara values('财务费用','0.00')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_OutBox') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_OutBox  --外箱
(
Id int identity(1,1),
Name varchar(255),
Price decimal(18,4)
)
insert Quote_OutBox values('B箱','2.9500')
insert Quote_OutBox values('C箱','5.3400')
insert Quote_OutBox values('D箱','5.9000')
insert Quote_OutBox values('F箱','4.5000')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_Plate') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_Plate  --层板
(
Id int identity(1,1),
Name varchar(255),
Price decimal(18,4)
)
insert Quote_Plate values('B箱','0.2900')
insert Quote_Plate values('C箱','0.6000')
insert Quote_Plate values('D箱','0.7400')
insert Quote_Plate values('F箱','0.4000')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_Partition') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_Partition  --隔断
(
Id int identity(1,1),
Name varchar(255),
Price decimal(18,4)
)
insert Quote_Partition values('C-1','3.6900')
insert Quote_Partition values('C-2','3.1300')
insert Quote_Partition values('C-3','3.1900')
insert Quote_Partition values('C-4','3.5600')
insert Quote_Partition values('D-1','6.3000')
insert Quote_Partition values('D-2','8.0200')
insert Quote_Partition values('D-3','3.0500')
insert Quote_Partition values('F-1','4.1400')
insert Quote_Partition values('F-2','3.1000')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_BubbleBag') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_BubbleBag  --气泡袋
(
Id int identity(1,1),
Name varchar(255),
Price decimal(18,4)
)
insert Quote_BubbleBag values('大号','0.3100')
insert Quote_BubbleBag values('中号','0.2000')
insert Quote_BubbleBag values('小号','0.1150')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_Blister') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_Blister  --吸塑
(
Id int identity(1,1),
Name varchar(255),
Price decimal(18,4)
)
insert Quote_Blister values('B箱','1.5000')
insert Quote_Blister values('F箱','2.9000')
insert Quote_Blister values('其他','3.2000')




create table Quote_CostCategory
(
Id int identity(1,1),
Name varchar(255)
)

create table Quote_CostList
(
Id int identity(1,1),
Name varchar(255),
CCId int
)
alter table Quote_CostCategory add constraint fk_Id primary key (Id)
alter table Quote_CostList add constraint fk_IdCostList primary key (Id)
alter table Quote_CostList
add constraint CostCategory_fk  foreign key (CCId) references Quote_CostCategory (Id)

alter table Quote_CostList add Number decimal(18,4)
alter table Quote_CostList add Unit varchar(50)
alter table Quote_CostList add Price decimal(18,4)

alter table Quote_CostList alter column Number varchar(255)
alter table Quote_CostList alter column Price varchar(255)

create table Quote_CusTemplate
(
Id int identity(1,1) primary key,
CustomerCode varchar(50),
CustomerName varchar(255),
CostCategory int,
CostList int
)
alter table Quote_CusTemplate
add constraint CusTemplate_fk1  foreign key (CostCategory) references Quote_CostCategory (Id)
alter table Quote_CusTemplate
add constraint CusTemplate_fk2  foreign key (CostList) references Quote_CostList (Id)
alter table Quote_CusTemplate add SortId int