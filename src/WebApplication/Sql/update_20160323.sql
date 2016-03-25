
insert into acc_menu values('Menu.MRP.DemandSchedule',1,'新需求日程','~/Main.aspx?mid=MRP.Schedule.DemandSchedule',1,'~/Images/Nav/DmdSchedule.png',getdate(),'su',getdate(),'su',null)
insert into acc_permission(PM_Code,PM_Desc,PM_CateCode) values('Menu.MRP.DemandSchedule','新需求日程','MRP');
insert into acc_menuCommon(Menu,ParentMenu,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) values('Menu.MRP.DemandSchedule','Menu.MRP.Trans',3,68,1,getdate(),'su',getdate(),'su');