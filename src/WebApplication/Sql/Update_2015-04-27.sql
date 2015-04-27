
if exists (select 1 from sys.objects where type='U' and name='UpdateBillPeriodLog')
drop table UpdateBillPeriodLog
go
create table UpdateBillPeriodLog
(
	Id			int			identity(1,1) primary key,
	UpType		varchar(50)	not null,
	ActBillId	int			null,
	OrderNo		varchar(50)	null,
	HuId		varchar(50) null,
	LocTransId	int			null,
	IpNo		varchar(50)	null,
	RecNo		varchar(50)	null,
	LotNo		varchar(50) null,
	BatchNo		int			null,
	Item		varchar(50)	null,
	ItemDesc	varchar(50)	null,
	Uom			char(4)		null,
	Qty			decimal(18,8) null,
	partyFrom	varchar(50)	null,
	PartyFromName varchar(50)	null,
	PartyTo		varchar(50)	null,
	PartyToName	varchar(50)	null,
	Loc			varchar(50)	null,
	LocName		varchar(50)	null,
	OrderDetId	int			null,
	OldEffDate	datetime	not null,
	NewEffDate	datetime	not null,
	ModifyUser	varchar(50)	not null,
	ModifyDate	datetime	not null
)