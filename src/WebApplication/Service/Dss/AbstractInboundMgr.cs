﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Utility;
using System.IO;
using com.Sconit.Entity;
using com.Sconit.Entity.Dss;
using com.Sconit.Entity.Exception;

using com.Sconit.Service.Ext.Dss;
using Castle.Services.Transaction;


namespace com.Sconit.Service.Dss
{
    public abstract class AbstractInboundMgr : SessionBase, IInboundMgr
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger("Log.DssInbound");
        public virtual IDssImportHistoryMgrE dssImportHistoryMgrE { get; set; }

        public AbstractInboundMgr(IDssImportHistoryMgrE dssImportHistoryMgrE)
        {
            this.dssImportHistoryMgrE = dssImportHistoryMgrE;
        }

        public virtual void ProcessInboundFile(DssInboundControl dssInboundControl, string[] files)
        {
            log.Info("Start process inbound ");

            //重新提交数据
            IList<DssImportHistory> activeDssImportHistoryList = dssImportHistoryMgrE.GetActiveDssImportHistory(dssInboundControl.Id);

            #region DataReader
            IList<DssImportHistory> dssImportHistoryList = new List<DssImportHistory>();
            foreach (string fileName in files)
            {
                try
                {
                    FlatFileReader reader = null;
                    try
                    {
                        DssImportHistory dssImportHistory = new DssImportHistory();
                        dssImportHistory.DssInboundCtrl = dssInboundControl;
                        dssImportHistory.IsActive = true;
                        dssImportHistory.KeyCode = Path.GetFileNameWithoutExtension(fileName);
                        dssImportHistory.CreateDate = DateTime.Now;

                        reader = this.DataReader(fileName, Encoding.GetEncoding(dssInboundControl.FileEncoding), "|");
                        for (string[] lineData = reader.ReadLine(); lineData != null; lineData = reader.ReadLine())
                        {
                            this.FillDssImportHistory(lineData, dssImportHistory);

                            if (dssImportHistory[0] == "0")
                            {
                                dssImportHistory.EventCode = BusinessConstants.DSS_EVENT_CODE_DELETE;
                                DssHelper.FormatDeleteData(lineData, BusinessConstants.DSS_SYSTEM_CODE_QAD);//QAD删除去引号
                            }
                            else
                            {
                                dssImportHistory.EventCode = BusinessConstants.DSS_EVENT_CODE_CREATE;
                            }
                        }

                        dssImportHistoryList.Add(dssImportHistory);
                    }
                    catch (Exception ex)
                    {
                        reader.Dispose();
                        log.Error("Process inbound file: " + fileName + " Error.", ex);
                        throw ex;
                    }
                    finally
                    {
                        reader.Dispose();
                        log.Info("Process inbound file: " + fileName + " successful.");
                    }
                }
                catch (Exception ex)
                {

                    log.Error("Create DssImportHistory error:", ex);
                }
            }
            #endregion

            #region CreateDssImportHistory
            CreateDssImportHistory(dssInboundControl, dssImportHistoryList, files);
            #endregion

            #region Archive download file
            try
            {
                ArchiveFile(files, dssInboundControl.ArchiveFloder);
            }
            catch (Exception ex)
            {
                log.Error("Archive download file error:", ex);
            }
            #endregion

            IListHelper.AddRange<DssImportHistory>(activeDssImportHistoryList, dssImportHistoryList);

            IList<object> objCreate = this.ProcessCreateData(activeDssImportHistoryList);
            IList<object> objDelete = this.ProcessDeleteData(activeDssImportHistoryList);

            try
            {
                this.CreateOrUpdateObject(objCreate);
                this.DeleteObject(objDelete);

                //山寨，先把除工单的全部更新active为false,以免每次都随工单执行
                if(dssInboundControl.Id != 9)
                {
                    foreach(DssImportHistory dssImpHis in activeDssImportHistoryList)
                    {
                        dssImpHis.IsActive = false;
                        dssImportHistoryMgrE.UpdateDssImportHistory(dssImpHis);
                    }
                
                }
            }
            catch (Exception ex)
            {
                log.Error("Write to database error:", ex);
            }
        }

        public virtual void CreateDssImportHistory(DssInboundControl dssInboundControl, IList<DssImportHistory> dssImportHistoryList, string[] files)
        {
            #region Create DssImportHistory
            try
            {
                dssImportHistoryMgrE.CreateDssImportHistory(dssImportHistoryList);
            }
            catch (Exception ex)
            {
                log.Error("Write to database error:", ex);
                throw new BusinessErrorException("Write to database error.", ex);
            }
            #endregion
        }

        public virtual FlatFileReader DataReader(string fileName, Encoding enc, string delimiter)
        {
            return new FlatFileReader(fileName, enc, delimiter); ;
        }

        public virtual void ArchiveFile(IList<string> fileFullPaths, string archiveFloder)
        {
            if (fileFullPaths != null && fileFullPaths.Count > 0)
            {
                foreach (var fileFullPath in fileFullPaths)
                {
                    this.ArchiveFile(fileFullPath, archiveFloder);
                }
            }
        }
        public virtual void ArchiveFile(string fileFullPath, string archiveFloder)
        {
            string fomatedFileFullPath = fileFullPath.Replace("\\", "/");
            string fileName = fomatedFileFullPath.Substring(fomatedFileFullPath.LastIndexOf("/") + 1);

            log.Info("Archive file : " + archiveFloder + fileName);
            archiveFloder = archiveFloder.Replace("\\", "/");
            if (!archiveFloder.EndsWith("/"))
            {
                archiveFloder += "/";
            }

            if (!Directory.Exists(archiveFloder))
            {
                Directory.CreateDirectory(archiveFloder);
            }

            if (File.Exists(archiveFloder + fileName))
            {
                File.Delete(archiveFloder + fileName);
            }

            File.Move(fileFullPath, archiveFloder + fileName);
        }

        public virtual IList<object> ProcessCreateData(IList<DssImportHistory> dssImportHistoryList)
        {
            var query = dssImportHistoryList
                .Where(d => StringHelper.Eq(d.EventCode, BusinessConstants.DSS_EVENT_CODE_CREATE))
                .ToList<DssImportHistory>();

            return this.DeserializeForCreate(query);
        }

        public virtual IList<object> ProcessDeleteData(IList<DssImportHistory> dssImportHistoryList)
        {
            var query = dssImportHistoryList
                .Where(d => StringHelper.Eq(d.EventCode, BusinessConstants.DSS_EVENT_CODE_DELETE))
                .ToList<DssImportHistory>();

            return this.DeserializeForDelete(query);
        }

        protected virtual IList<object> DeserializeForCreate(IList<DssImportHistory> dssImportHistoryList)
        {
            IList<object> objList = new List<object>();
            if (dssImportHistoryList != null && dssImportHistoryList.Count > 0)
            {
                foreach (var item in dssImportHistoryList)
                {
                    try
                    {
                        object obj = this.DeserializeForCreate(item);
                        objList.Add(obj);
                    }
                    catch (BusinessErrorException ex)
                    {
                        log.Error("DeserializeForCreate error:" + ex);
                    }
                }
            }
            return objList;
        }

        protected virtual IList<object> DeserializeForDelete(IList<DssImportHistory> dssImportHistoryList)
        {
            IList<object> objList = new List<object>();
            if (dssImportHistoryList != null && dssImportHistoryList.Count > 0)
            {
                foreach (var item in dssImportHistoryList)
                {
                    try
                    {
                        object obj = this.DeserializeForDelete(item);
                        objList.Add(obj);
                    }
                    catch (BusinessErrorException ex)
                    {
                        log.Error("DeserializeForDelete error:" + ex);
                    }
                }
            }
            return objList;
        }

        protected virtual void CreateOrUpdateObject(IList<object> objList)
        {
            if (objList != null && objList.Count > 0)
            {
                foreach (var obj in objList)
                {
                    this.CreateOrUpdateObject(obj);
                }
            }
        }

        protected virtual void DeleteObject(IList<object> objList)
        {
            if (objList != null && objList.Count > 0)
            {
                foreach (var obj in objList)
                {
                    this.DeleteObject(obj);
                }
            }
        }

        protected virtual void FillDssImportHistory(string[] lineData, DssImportHistory dssImportHistory)
        {
            if (lineData != null && lineData.Length > 0 && dssImportHistory != null)
            {
                for (int i = 0; i < lineData.Length; i++)
                {
                    if (lineData[i] == "?")
                        lineData[i] = null;
                    else
                        dssImportHistory[i] = lineData[i];
                    //log.Debug("Read Data[" + i + "]: " + lineData[i]);
                }
            }
        }

        protected abstract object DeserializeForCreate(DssImportHistory dssImportHistory);
        protected abstract object DeserializeForDelete(DssImportHistory dssImportHistory);

        protected abstract void CreateOrUpdateObject(object obj);
        protected abstract void DeleteObject(object obj);
    }
}
