
/****** Object:  StoredProcedure [dbo].[GenPurchase]    Script Date: 2015/4/23 15:29:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
if exists (select * from sys.objects where type='P' and Name='Proc_UpdateBillPeriod')
DROP PROCEDURE Proc_UpdateBillPeriod
go
CREATE PROCEDURE [dbo].[Proc_UpdateBillPeriod]  -- exec Proc_UpdateBillPeriod '2014-07-18','2014-08-20','su'
( 
@PurchaseDate datetime, 
@FinanceDate datetime,
@ModifyUser	varchar(50)
) 
AS 
begin
	set @PurchaseDate=dateadd(day,1,@PurchaseDate)
	set @FinanceDate=dateadd(day,1,@FinanceDate)
	declare @trancount int
	declare @Msg nvarchar(MAX)
	declare @CurrentTime datetime
	set @trancount = @@trancount
	set @Msg = ''
	set @CurrentTime=getdate()
	begin try
		create table #tempOrderNoAndHuId
		(
			RowId int identity(1, 1) Primary Key,
			HuId varchar(50),
			OrderNo varchar(50)
		)
		--有条码的 采购入库 改库存事物  改actbill
		truncate table #tempOrderNoAndHuId
		insert into #tempOrderNoAndHuId (HuId,OrderNo)
		select HuId, OrderNo from LocTrans where HuId in(select HuId from LocTrans where HuId in(select HuId from LocTrans where TransType='RCT-PO' and HuId is not null and EffDate between @PurchaseDate and @FinanceDate) group by HuId having COUNT(*)=1)

		if @trancount = 0
		begin
            begin tran
        end
		--插入修改事物日志
		insert into [dbo].[UpdateBillPeriodLog](UpType, ActBillId, OrderNo, HuId, LocTransId, IpNo, RecNo, LotNo, BatchNo, Item, ItemDesc, Uom, Qty, partyFrom, PartyFromName, PartyTo, PartyToName, Loc, LocName, OrderDetId, OldEffDate, NewEffDate, ModifyUser, ModifyDate)
		select 'LocTrans' as UpType,null, OrderNo, HuId, Id, IpNo, RecNo, LotNo, BatchNo, Item, ItemDesc, Uom, Qty, partyFrom, PartyFromName, PartyTo, PartyToName, Loc, LocName, OrderDetId, EffDate, @FinanceDate, @ModifyUser, @CurrentTime from LocTrans where HuId in( select HuId from #tempOrderNoAndHuId)
		--修改入库事物生效日期
		update LocTrans set EffDate=@FinanceDate where HuId in( select HuId from #tempOrderNoAndHuId)

		--插入修ActBill物日志
		insert into [dbo].[UpdateBillPeriodLog](UpType, ActBillId, OrderNo, HuId, LocTransId, IpNo, RecNo, LotNo, BatchNo, Item, ItemDesc, Uom, Qty, partyFrom, PartyFromName, PartyTo, PartyToName, Loc, LocName, OrderDetId, OldEffDate, NewEffDate, ModifyUser, ModifyDate)
		select 'ActBill' as UpType, Id, OrderNo, null, null, IpNo, RecNo, null, null, Item, null, Uom, BillQty, null, null, null, null, null, null, null, EffDate, @FinanceDate, @ModifyUser, @CurrentTime from ActBill where OrderNo in(select OrderNo from #tempOrderNoAndHuId) 

		--修改ActBill 生效日期
		update ActBill set EffDate=@FinanceDate where OrderNo in(select OrderNo from #tempOrderNoAndHuId) and EffDate between @PurchaseDate and @FinanceDate

		if @trancount = 0 
		begin  
            commit
        end
	end try
	begin catch
		if @trancount = 0
        begin
            rollback
        end
		set @Msg = N'修改账单失败：' + Error_Message()
		RAISERROR(@Msg, 16, 1) 
	end catch
end




