IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_ProductInfo') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_ProductInfo  --产品信息表
(
Id int identity(1,1) primary key,
CustomerCode varchar(50),
CustomerName varchar(255),
ProductName varchar(255),
ProductNo varchar(50),
VersionNo varchar(50),
PT varchar(255),                --加工工艺
PCBNum varchar(50),             --拼版数
AdvisePCBNum varchar(50),		--建议拼版数
DoubleSideMount bit,			--双面贴装
ChipBurning bit,				--芯片烧写
BurningNum varchar(50),			--烧写秒数
LightNum varchar(50),			--点灯数量
BoardMode varchar(255),			--分板方式
ConnPoint varchar(50),			--连接点
DeviceShaping bit,				--器件整形
ShapingType varchar(50),		--整形种类
ShapingSecCount varchar(50),	--合计秒数
DeviceCoding bit,				--器件编带
CodingType varchar(50),			--编带种类
CodingSecCount varchar(50),		--合计秒数
FCTTest bit,					--FCT测试 
TestSec varchar(50),			--测试秒数
ProductAssembly bit,			--产品装配
AssemblySec varchar(50),		--装配秒数
FinalAssemblyTest bit,			--总装测试
FinalTestSec varchar(50),		--总测秒数
SpecialReq varchar(255),		--特殊要求
SurfaceCoating bit,				--表面涂覆
MaterialNo varchar(50),			--材料型号
CoatingAcreage varchar(50),		--涂覆面积
CoatingSec varchar(50),			--涂覆秒数
ProductFilling bit,				--产品灌胶
FillingPrice decimal(18,4),		--灌胶价格
PackMode varchar(255),			--包装方式
OutBox varchar(255),			--外箱
OutBoxPrice decimal(18,4),		--外箱单价
Plate varchar(255),				--层板
PlateNum varchar(50),			--层板数量
PlatePrice decimal(18,4),		--层板单价
Partition varchar(255),			--隔断
PartitionNum varchar(50),		--隔断数量
PartitionPrice decimal(18,4),	--隔断单价
BubbleBag varchar(255),			--气泡袋
BubbleBagPrice decimal(18,4),	--气泡袋单价
Blister varchar(255),			--吸塑
BlisterNum varchar(50),			--吸塑数量
BlisterPrice decimal(18,4),		--吸塑单价
FCLNum varchar(50),				--整箱数量

DeliveryAdd varchar(255),		--交付地点
LogisticsFee decimal(18,2),		--物流单价
LogisticsCost decimal(18,2),	--物流成本
OutBoxResult decimal(18,4),		--外箱
PlateResult decimal(18,4),		--层板
PartitionResult decimal(18,4),	--隔断
BubbleBagResult decimal(18,4),	--气泡袋
BlisterResult decimal(18,4),	--吸塑
PackPrice decimal(18,4),		--包装单价
PackCost decimal(18,4)			--包装成本
)

alter table Quote_ProductInfo add ProjectId varchar(50)
alter table Quote_ProductInfo add CreateDate datetime
alter table Quote_ProductInfo add Status varchar(50)

alter table Quote_ProductInfo add Source_ varchar(50)
alter table Quote_ProductInfo add Remark1 varchar(255)
alter table Quote_ProductInfo add Remark2 varchar(255)

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_Item') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_Item  --BOM
(
Id int Primary key identity(1,1),
ProjectId varchar(50),
ItemCode varchar(50),
Supplier varchar(255),
Category varchar(255),
Brand varchar(255),
Model varchar(50),
SingleNum varchar(50),
PurchasePrice decimal(18,2),
Price decimal(18,2),
PinNum varchar(50),
PinConversion varchar(50),
Point varchar(50)
)

create table Quote_Project
(
Id int primary key identity(1,1),
ProjectId varchar(50),
Status varchar(50),
CustomerCode varchar(50),
CustomerName varchar(255),
InputUserName varchar(50),
InputDate datetime,
ProductName varchar(255),
ProductNo varchar(50),
VersionNo varchar(50),
DeliveryAdd varchar(255),
CooperationMode varchar(255),
SType varchar(255),
BillPeriod varchar(50),
MonthlyDemand varchar(50),
QFor varchar(255),
TSType varchar(255),
PlanAllocationNum varchar(50),
PT varchar(255),
PCBNum varchar(50),
DoubleSideMount varchar(50),
ChipBurning varchar(50),
LightNum varchar(50),
BoardMode varchar(255),
ConnPoint varchar(50),
DeviceShaping varchar(50),
DeviceCoding varchar(50),
CodingType varchar(50),
SurfaceCoating varchar(50),
CoatingAcreage varchar(50),
PackMode varchar(255),
EachBox varchar(50),
LogisticsMode varchar(50),
IsBack varchar(50),
SalesUP varchar(50),
SalesUPI varchar(50),
LumpSumFee varchar(50),
LumpSumFeeI varchar(50),
Data text,
ToCustomerName varchar(50),
ToCustomerDate datetime
)

insert into NumCtrl values ('P','1',null)