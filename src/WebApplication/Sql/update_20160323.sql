
insert into acc_menu values('Menu.MRP.DemandSchedule',1,'�������ճ�','~/Main.aspx?mid=MRP.Schedule.DemandSchedule',1,'~/Images/Nav/DmdSchedule.png',getdate(),'su',getdate(),'su',null)
insert into acc_permission(PM_Code,PM_Desc,PM_CateCode) values('Menu.MRP.DemandSchedule','�������ճ�','MRP');
insert into acc_menuCommon(Menu,ParentMenu,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) values('Menu.MRP.DemandSchedule','Menu.MRP.Trans',3,68,1,getdate(),'su',getdate(),'su');



--add 20160328
insert into acc_menu values('Menu.SupplierMenu.DemandSchedule',1,'�¹�Ӧ���ճ�','~/Main.aspx?mid=MRP.Schedule.DemandSchedule__mp--IsSupplier-true',1,'~/Images/Nav/DmdSchedule.png',getdate(),'su',getdate(),'su',null)
insert into acc_permission(PM_Code,PM_Desc,PM_CateCode) values('Menu.SupplierMenu.DemandSchedule','�¹�Ӧ���ճ�','SupplierMenu');
insert into acc_menuCommon(Menu,ParentMenu,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) values('Menu.SupplierMenu.DemandSchedule','Menu.SupplierMenu',2,365,1,getdate(),'su',getdate(),'su');
