
基础数据表

1.初始化数据库,按照次顺序执行数据库脚本文件:

	sconittrunk.sql

	data_user_role_userpre.sql

	data_permissioncategory.sql

	data_permission.sql

	data_menu.sql

	data_codemstr.sql

	data_entityopt.sql

	data_batch.sql

2.开发时候添加的字段请加入到上述脚本文件中,并且把增量放入 update.sql 中,并且包含初始值,例如:
	ALTER  TABLE   OrderMstr
			ADD	[TextField1] [varchar](255) NULL;
	UPDATE OrderMstr SET TextField1= '初始值';




update acc_permission set pm_desc='客供品价格单' where pm_id=3264
update acc_permission set pm_desc='成品报表' where pm_id=3265
update acc_permission set pm_desc='原材料报表' where pm_id=3266
update acc_permission set pm_desc='废品报表' where pm_id=3267
update acc_permission set pm_desc='金额收发存' where pm_id=3269
update acc_permission set pm_desc='原材料收发存' where pm_id=3270


INSERT ACC_Permission VALUES ('Cost.Report.InvIOBnew','金额收发存(新)','Cost')
INSERT acc_menu VALUES ('Cost.Report.InvIOBnew',1,'金额收发存(新)','~/Main.aspx?mid=Cost.Report.InvIOBnew',1,'~/Images/Nav/InvIOBnew.png',getdate(),null,getdate(),null,null)
INSERT ACC_MenuCommon VALUES ('Cost.Report.InvIOBnew','Menu.Cost.Info',3,40,1,getdate(),null,getdate(),null)


INSERT ACC_Permission VALUES ('Menu.Inventory.InventoryIOBnew','收发存报表(新)','Inventory')
INSERT acc_menu VALUES ('Menu.Inventory.InventoryIOBnew',1,'收发存报表(新)','~/Main.aspx?mid=Reports.InvIOBnew',1,'~/Images/Nav/InvIOBnew.png',getdate(),null,getdate(),null,null)
INSERT ACC_MenuCommon VALUES ('Menu.Inventory.InventoryIOBnew','Menu.Inventory.Info',3,307,1,getdate(),null,getdate(),null)


INSERT ACC_Permission VALUES ('Cost.Report.IOBMutiLoc','金额收发存(多库位)','Cost')
INSERT acc_menu VALUES ('Cost.Report.IOBMutiLoc',1,'金额收发存(多库位)','~/Main.aspx?mid=Cost.Report.IOBMutiLoc',1,'~/Images/Nav/IOBMutiLoc.png',getdate(),null,getdate(),null,null)
INSERT ACC_MenuCommon VALUES ('Cost.Report.IOBMutiLoc','Menu.Cost.Info',3,50,1,getdate(),null,getdate(),null)