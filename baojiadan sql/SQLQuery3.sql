insert ACC_Menu values('Menu.Quote.Info.Customer',1,'客户信息','~/Main.aspx?mid=Quote.Customer',1,'',getdate(),null,getdate(),null,null)
insert ACC_MenuCommon values('Menu.Quote.Info.Customer','Menu.Quote.Info',3,448,1,getdate(),null,getdate(),null)
insert ACC_Permission values('Menu.Quote.Info.Customer','客户信息','Quote')

insert ACC_Menu values('Menu.Quote.Setup.GPID',1,'生成项目ID','~/Main.aspx?mid=Quote.GPID',1,'',getdate(),null,getdate(),null,null)
insert ACC_MenuCommon values('Menu.Quote.Setup.GPID','Menu.Quote.Setup',3,450,1,getdate(),null,getdate(),null)
insert ACC_Permission values('Menu.Quote.Setup.GPID','生成项目ID','Quote')

create table Quote_GPID
(
	ID varchar(50) primary key,
	CustomerCode varchar(50),
	Descr varchar(255),
	StartDate datetime,
	Status bit,
)
alter table Quote_GPID add Product varchar(50)
alter table Quote_GPID add EndCustomer varchar(50)
alter table Quote_GPID add Addr varchar(50)
alter table Quote_GPID add LifeCycle varchar(50)
alter table Quote_GPID add OTS datetime
alter table Quote_GPID add PPAP datetime
alter table Quote_GPID add SOP datetime
alter table Quote_GPID add ProjectManager varchar(255)
alter table Quote_GPID add Buyer varchar(255)
alter table Quote_GPID add Technology varchar(255)
alter table Quote_GPID add Quality varchar(255)
alter table Quote_GPID add Desc1 varchar(255)
alter table Quote_GPID add Desc2 varchar(255)

insert into NumCtrl values('AF','1',null)

create table ItemPack
(
	Id int identity,
	Spec varchar(50),
	Descr varchar(255),
	PinNum decimal(18,8),
	PinConversion decimal(18,8)
)

insert ACC_Menu values('Menu.MasterData.ItemPack',1,'物料封装','~/Main.aspx?mid=MasterData.ItemPack',1,'',getdate(),null,getdate(),null,null)
insert ACC_MenuCommon values('Menu.MasterData.ItemPack','Menu.MasterData',2,451,1,getdate(),null,getdate(),null)
insert ACC_Permission values('Menu.MasterData.ItemPack','物料封装','MasterData')

alter table Item add ItemPack varchar(50)

alter table Quote_Project add PVision varchar(50) 

alter table Quote_ToolingInfo add CustomerBillDate datetime
alter table Quote_ToolingInfo add CustomerBillNo varchar(50)

alter table Quote_Item add ProductId int 
alter table Quote_Item add BitNum varchar(50)
alter table Quote_Item add Side varchar(50)

