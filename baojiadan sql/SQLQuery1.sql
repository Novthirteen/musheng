

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

insert ACC_Menu values('Menu.Quote',1,'���۹���','',1,'',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Trans',1,'����','',1,'~/Images/Nav/Transaction.png',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Info',1,'��Ϣ','',1,'~/Images/Nav/Information.png',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Setup',1,'����','',1,'~/Images/Nav/Setup.png',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Info.Tooling',1,'��װ��Ϣ','~/Main.aspx?mid=Quote.Tooling',1,'',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Info.ProductInfo',1,'��Ʒ��Ϣ','~/Main.aspx?mid=Quote.ProductInfo',1,'',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Info.Item',1,'����','~/Main.aspx?mid=Quote.Item',1,'',getdate(),null,getdate(),null,null)
--insert ACC_Menu values('Menu.Quote.Info.Para',1,'����','~/Main.aspx?mid=Quote.Para',1,'',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Setup.Template',1,'ģ��','~/Main.aspx?mid=Quote.Template',1,'',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Setup.CusTemplate',1,'�ͻ�ģ��','~/Main.aspx?mid=Quote.CusTemplate',1,'',getdate(),null,getdate(),null,null)
insert ACC_Menu values('Menu.Quote.Trans.Quotes',1,'���۵�','~/Main.aspx?mid=Quote.Quotes',1,'',getdate(),null,getdate(),null,null)

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


insert ACC_PermissionCategory values('QuoteOperation','��װά������','Page')
insert ACC_PermissionCategory values('QuotePOperation','����ά������','Page')

insert ACC_Permission values('ProjectID','��ĿID','QuoteOperation')
insert ACC_Permission values('TL_Name','��װ����','QuoteOperation')
insert ACC_Permission values('TL_Spec','��װ���','QuoteOperation')
insert ACC_Permission values('CustomerName','�ͻ�����','QuoteOperation')
insert ACC_Permission values('ProductName','��Ʒ�ͺ�/����','QuoteOperation')
insert ACC_Permission values('MSNo','Ľʢͼ��','QuoteOperation')
insert ACC_Permission values('Price','�۸�(��˰)','QuoteOperation')
insert ACC_Permission values('Number','����','QuoteOperation')
insert ACC_Permission values('ApplyDate','��������','QuoteOperation')
insert ACC_Permission values('ApplyUser','���뷽','QuoteOperation')
insert ACC_Permission values('ArriveDate','��������','QuoteOperation')
insert ACC_Permission values('TL_SalesType','��װ������ʽ','QuoteOperation')
insert ACC_Permission values('Supplier','��Ӧ��','QuoteOperation')
insert ACC_Permission values('SupplierInNo','��Ӧ����ⷢƱ','QuoteOperation')
insert ACC_Permission values('PCBNo','PCB�汾���','QuoteOperation')
insert ACC_Permission values('CustomerBStatus','�ͻ�������״̬','QuoteOperation')

insert ACC_PermissionCategory values('Quote','���۹���','Menu')
insert ACC_Permission values('Menu.Quote.Info.Tooling','��װ��Ϣ','Quote')
insert ACC_Permission values('Menu.Quote.Info.ProductInfo','��Ʒ��Ϣ','Quote')
insert ACC_Permission values('Menu.Quote.Info.Item','����','Quote')
--insert ACC_Permission values('Menu.Quote.Info.Para','����','Quote')
insert ACC_Permission values('Menu.Quote.Setup.Template','ģ��','Quote')
insert ACC_Permission values('Menu.Quote.Setup.CusTemplate','�ͻ�ģ��','Quote')
insert ACC_Permission values('Menu.Quote.Trans.Quotes','���۵�','Quote')


IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_CustomerInfo') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_CustomerInfo
(
Id int identity(1,1),
Code varchar(50),
Name varchar(255),			--�ͻ�����
SType varchar(50),			--���㷽ʽ
BillPeriod int,				--����(��)
P_LossRate varchar(50),		--������
P_ManageFee varchar(50),	--�������(�ӹ�����)
P_FinanceFee varchar(50),	--�������(�ӹ�����)
P_Profit varchar(50),		--����(�ӹ�����)
M_ManageFee varchar(50),	--�������(���Ϸ���)
M_FinanceFee varchar(50),	--�������(���Ϸ���)
DeliveryAdd varchar(255),	--�����ص�
DeliveryCity varchar(50),	--�������ڵ�
Status bit,					--״̬
StartDate datetime,			--����ʱ��
EndDate datetime			--����ʱ��
)


IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_SType') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_SType  --���㷽ʽ
(
Id int identity(1,1),
Name varchar(255)
)

insert Quote_SType values('�ջ�����')
insert Quote_SType values('���߽���')
insert Quote_SType values('���߽���')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_CooperationMode') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_CooperationMode  --������ʽ
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_CooperationMode values('���ϼӹ�')
insert Quote_CooperationMode values('���ɹ���')
insert Quote_CooperationMode values('���ģʽ')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_QFor') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_QFor  --�������
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_QFor values('��Ŀѯ��')
insert Quote_QFor values('һ���Լӹ�')
insert Quote_QFor values('����')
insert Quote_QFor values('�ֹ�����')
insert Quote_QFor values('С��������')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_PT') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_PT  --�ӹ�����
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_PT values('��Ǧ')
insert Quote_PT values('��Ǧ')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_TSType') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_TSType  --��װ���㷽ʽ
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_TSType values('һ���Խ���')
insert Quote_TSType values('��̯����')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_BoardMode') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_BoardMode  --�ְ巽ʽ
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_BoardMode values('�ֹ��ְ�')
insert Quote_BoardMode values('ֱ�߷ְ�')
insert Quote_BoardMode values('���߷ְ�')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_PackMode') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_PackMode  --��װ��ʽ
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_PackMode values('��ת�� + ���ݴ�')
insert Quote_PackMode values('ֽ�� + ���ݴ�')
insert Quote_PackMode values('��ת�� + ����')
insert Quote_PackMode values('ֽ�� + ����')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_LogisticsMode') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_LogisticsMode  --������ʽ
(
Id int identity(1,1),
Name varchar(255)
)
insert Quote_LogisticsMode values('������')
insert Quote_LogisticsMode values('ί������')
insert Quote_LogisticsMode values('���')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_LogisticsFee') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_LogisticsFee  --��������
(
Id int identity(1,1),
CityName varchar(255),
Unit varchar(50),
Price decimal(18,2)
)
insert Quote_LogisticsFee values('�Ϻ�','��','12.00')
insert Quote_LogisticsFee values('�Ͼ�','��','18.00')
insert Quote_LogisticsFee values('ʮ��','��','18.00')
insert Quote_LogisticsFee values('����','��','15.00')
insert Quote_LogisticsFee values('�ߺ�','��','18.00')
insert Quote_LogisticsFee values('����','��','25.00')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_ToolingList') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_ToolingList  --��װ�嵥
(
Id int identity(1,1),
Name varchar(255),
Unit varchar(50),
Price decimal(18,2)
)
insert Quote_ToolingList values('������','��','650.00')
insert Quote_ToolingList values('����ξ�','��','2800.00')
insert Quote_ToolingList values('ICT�ξ�','��','8500.00')
insert Quote_ToolingList values('���庸�ξߣ�ʯӢʯ��','��','750.00')
insert Quote_ToolingList values('�ֹ������ξ�','��','2000.00')
insert Quote_ToolingList values('���߷ְ��ξ�','��','2700.00')
insert Quote_ToolingList values('���ܲ����ξ�','��','0.00')
insert Quote_ToolingList values('����Ϳ���ξ�','��','0.00')
insert Quote_ToolingList values('��·���ư�','��','1500.00')
insert Quote_ToolingList values('����ģ��_B','��','4300.00')
insert Quote_ToolingList values('����ģ��_F','��','3800.00')
insert Quote_ToolingList values('����ģ��_��ת��','��','4300.00')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_PFee') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_PFee  --�ӹ�����
(
Id int identity(1,1),
Para1 varchar(255),
Para2 varchar(50),
Price1 decimal(18,4),
Price2 decimal(18,4)
)
insert Quote_PFee values('��Ŀѯ��','��Ǧ','0.0250','0.0300')
insert Quote_PFee values('��Ŀѯ��','��Ǧ','0.0280','0.0350')
insert Quote_PFee values('һ���Լӹ�','��Ǧ','0.0250','0.0300')
insert Quote_PFee values('һ���Լӹ�','��Ǧ','0.0280','0.0350')
insert Quote_PFee values('����','��Ǧ','0.0250','0.0300')
insert Quote_PFee values('����','��Ǧ','0.0280','0.0350')
insert Quote_PFee values('�ֹ�����','��Ǧ','0.0800','0.0000')
insert Quote_PFee values('�ֹ�����','��Ǧ','0.1000','0.0000')
insert Quote_PFee values('С��������','��Ǧ','0.0250','0.0300')
insert Quote_PFee values('С��������','��Ǧ','0.02800','0.0350')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_BootFee') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_BootFee  --��������
(
Id int identity(1,1),
Name1 varchar(255),
Name2 varchar(255),
Price decimal(18,2)
)
insert Quote_BootFee values('С��������','AOI���','200.00')
insert Quote_BootFee values('С��������','��Ƭ�����','500.00')
insert Quote_BootFee values('ICTС��������','��Ƭ������','400.00')
insert Quote_BootFee values('С��������','���庸����','200.00')
insert Quote_BootFee values('С��������','��������','200.00')
insert Quote_BootFee values('С��������','�����˹�','150.00')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_CalculatePara') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_CalculatePara  --�������
(
Id int identity(1,1),
Name varchar(255),
Price decimal(18,2)
)
insert Quote_CalculatePara values('�豸����','0.00')
insert Quote_CalculatePara values('�ֹ�����','0.00')
insert Quote_CalculatePara values('�������','0.00')
insert Quote_CalculatePara values('�������','0.00')
insert Quote_CalculatePara values('����','0.00')
insert Quote_CalculatePara values('�������','0.00')
insert Quote_CalculatePara values('�������','0.00')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_OutBox') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_OutBox  --����
(
Id int identity(1,1),
Name varchar(255),
Price decimal(18,4)
)
insert Quote_OutBox values('B��','2.9500')
insert Quote_OutBox values('C��','5.3400')
insert Quote_OutBox values('D��','5.9000')
insert Quote_OutBox values('F��','4.5000')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_Plate') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_Plate  --���
(
Id int identity(1,1),
Name varchar(255),
Price decimal(18,4)
)
insert Quote_Plate values('B��','0.2900')
insert Quote_Plate values('C��','0.6000')
insert Quote_Plate values('D��','0.7400')
insert Quote_Plate values('F��','0.4000')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_Partition') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_Partition  --����
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
create table Quote_BubbleBag  --���ݴ�
(
Id int identity(1,1),
Name varchar(255),
Price decimal(18,4)
)
insert Quote_BubbleBag values('���','0.3100')
insert Quote_BubbleBag values('�к�','0.2000')
insert Quote_BubbleBag values('С��','0.1150')

IF NOT EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'Quote_Blister') AND OBJECTPROPERTY(ID, 'IsTable') = 1)
create table Quote_Blister  --����
(
Id int identity(1,1),
Name varchar(255),
Price decimal(18,4)
)
insert Quote_Blister values('B��','1.5000')
insert Quote_Blister values('F��','2.9000')
insert Quote_Blister values('����','3.2000')




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