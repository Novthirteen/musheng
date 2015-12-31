IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_ProductInfo') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_ProductInfo  --��Ʒ��Ϣ��
(
Id int identity(1,1) primary key,
CustomerCode varchar(50),
CustomerName varchar(255),
ProductName varchar(255),
ProductNo varchar(50),
VersionNo varchar(50),
PT varchar(255),                --�ӹ�����
PCBNum varchar(50),             --ƴ����
AdvisePCBNum varchar(50),		--����ƴ����
DoubleSideMount bit,			--˫����װ
ChipBurning bit,				--оƬ��д
BurningNum varchar(50),			--��д����
LightNum varchar(50),			--�������
BoardMode varchar(255),			--�ְ巽ʽ
ConnPoint varchar(50),			--���ӵ�
DeviceShaping bit,				--��������
ShapingType varchar(50),		--��������
ShapingSecCount varchar(50),	--�ϼ�����
DeviceCoding bit,				--�������
CodingType varchar(50),			--�������
CodingSecCount varchar(50),		--�ϼ�����
FCTTest bit,					--FCT���� 
TestSec varchar(50),			--��������
ProductAssembly bit,			--��Ʒװ��
AssemblySec varchar(50),		--װ������
FinalAssemblyTest bit,			--��װ����
FinalTestSec varchar(50),		--�ܲ�����
SpecialReq varchar(255),		--����Ҫ��
SurfaceCoating bit,				--����Ϳ��
MaterialNo varchar(50),			--�����ͺ�
CoatingAcreage varchar(50),		--Ϳ�����
CoatingSec varchar(50),			--Ϳ������
ProductFilling bit,				--��Ʒ�ེ
FillingPrice decimal(18,4),		--�ེ�۸�
PackMode varchar(255),			--��װ��ʽ
OutBox varchar(255),			--����
OutBoxPrice decimal(18,4),		--���䵥��
Plate varchar(255),				--���
PlateNum varchar(50),			--�������
PlatePrice decimal(18,4),		--��嵥��
Partition varchar(255),			--����
PartitionNum varchar(50),		--��������
PartitionPrice decimal(18,4),	--���ϵ���
BubbleBag varchar(255),			--���ݴ�
BubbleBagPrice decimal(18,4),	--���ݴ�����
Blister varchar(255),			--����
BlisterNum varchar(50),			--��������
BlisterPrice decimal(18,4),		--���ܵ���
FCLNum varchar(50),				--��������

DeliveryAdd varchar(255),		--�����ص�
LogisticsFee decimal(18,2),		--��������
LogisticsCost decimal(18,2),	--�����ɱ�
OutBoxResult decimal(18,4),		--����
PlateResult decimal(18,4),		--���
PartitionResult decimal(18,4),	--����
BubbleBagResult decimal(18,4),	--���ݴ�
BlisterResult decimal(18,4),	--����
PackPrice decimal(18,4),		--��װ����
PackCost decimal(18,4)			--��װ�ɱ�
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