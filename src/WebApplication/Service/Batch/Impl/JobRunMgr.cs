using com.Sconit.Service.Ext.Batch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Sconit.Entity.Batch;
using com.Sconit.Entity;
using System.Reflection;
using Castle.Windsor;
using com.Sconit.Utility;



namespace com.Sconit.Service.Batch.Impl
{
    public class JobRunMgr : IJobRunMgr
    {
        #region IJobRunMgrE Members

        private static log4net.ILog log = log4net.LogManager.GetLogger("Log.BatchJob");

        public IBatchTriggerMgrE batchTriggerMgrE { get; set; }
        public IBatchJobParameterMgrE batchJobParameterMgrE { get; set; }
        public IBatchTriggerParameterMgrE batchTriggerParameterMgrE { get; set; }
        public IBatchRunLogMgrE batchRunLogMgrE { get; set; }


        public void RunBatchJobs(IWindsorContainer container)
        {
            log.Info("----------------------------------Invincible's dividing line---------------------------------------");
            log.Info("BatchJobs run start.");

            IList<BatchTrigger> tobeFiredTriggerList = this.batchTriggerMgrE.GetTobeFiredTrigger();

            if (tobeFiredTriggerList != null && tobeFiredTriggerList.Count > 0)
            {
                foreach (BatchTrigger tobeFiredTrigger in tobeFiredTriggerList)
                {
                    BatchJobDetail jobDetail = tobeFiredTrigger.BatchJobDetail;
                    BatchRunLog runLog = new BatchRunLog();
                    try
                    {
                        #region Job运行前处理
                        log.Info("Start run job. JobId:" + jobDetail.Id + ", JobName:" + jobDetail.Name);
                        runLog.BatchJobDetail = jobDetail;
                        runLog.BatchTrigger = tobeFiredTrigger;
                        runLog.StartTime = DateTime.Now;
                        runLog.Status = "InProcess";
                        this.batchRunLogMgrE.CreateBatchRunLog(runLog);
                        #endregion

                        #region 运行Job

                        JobDataMap dataMap = new JobDataMap();
                        #region Job参数获取
                        IList<BatchJobParameter> batchJobParameterList = this.batchJobParameterMgrE.GetBatchJobParameter(jobDetail.Id);
                        if (batchJobParameterList != null && batchJobParameterList.Count > 0)
                        {
                            foreach (BatchJobParameter batchJobParameter in batchJobParameterList)
                            {
                                log.Debug("Set Job Parameter Name:" + batchJobParameter.ParameterName + ", Value:" + batchJobParameter.ParameterValue);
                                dataMap.PutData(batchJobParameter.ParameterName, batchJobParameter.ParameterValue);
                            }
                        }
                        #endregion

                        #region Trigger参数获取
                        IList<BatchTriggerParameter> batchTriggerParameterList = this.batchTriggerParameterMgrE.GetBatchTriggerParameter(tobeFiredTrigger.Id);
                        if (batchTriggerParameterList != null && batchTriggerParameterList.Count > 0)
                        {
                            foreach (BatchTriggerParameter batchTriggerParameter in batchTriggerParameterList)
                            {
                                log.Debug("Set Trigger Parameter Name:" + batchTriggerParameter.ParameterName + ", Value:" + batchTriggerParameter.ParameterValue);
                                dataMap.PutData(batchTriggerParameter.ParameterName, batchTriggerParameter.ParameterValue);
                            }
                        }
                        #endregion

                        #region 初始化JobRunContext
                        JobRunContext jobRunContext = new JobRunContext(tobeFiredTrigger, jobDetail, dataMap, container);
                        #endregion

                        #region 调用Job
                        IJob job = container.Resolve<IJob>(jobDetail.ServiceName);
                        log.Debug("Start run job: " + jobDetail.ServiceName);
                        job.Execute(jobRunContext);
                        #endregion

                        #endregion

                        #region Job运行后处理
                        log.Info("Job run successful. JobId:" + jobDetail.Id + ", JobName:" + jobDetail.Name);
                        runLog.EndTime = DateTime.Now;
                        runLog.Status = "Successful";
                        this.batchRunLogMgrE.UpdateBatchRunLog(runLog);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            log.Error("Job run failure. JobId:" + jobDetail.Id + ", JobName:" + jobDetail.Name, ex);
                            runLog.EndTime = DateTime.Now;
                            runLog.Status = "Failure";
                            if (ex.Message != null && ex.Message.Length > 255)
                            {
                                runLog.Message = ex.Message.Substring(0, 255);
                            }
                            else
                            {
                                runLog.Message = ex.Message;
                            }
                            this.batchRunLogMgrE.UpdateBatchRunLog(runLog);
                        }
                        catch (Exception ex1)
                        {
                            log.Error("", ex1);
                        }
                    }
                    finally
                    {
                        #region 更新BatchTrigger
                        try
                        {
                            BatchTrigger oldTobeFiredTrigger = this.batchTriggerMgrE.LoadBatchTrigger(tobeFiredTrigger.Id);
                            oldTobeFiredTrigger.TimesTriggered++;
                            oldTobeFiredTrigger.PreviousFireTime = oldTobeFiredTrigger.NextFireTime;
                            if (oldTobeFiredTrigger.RepeatCount != 0 && oldTobeFiredTrigger.TimesTriggered >= oldTobeFiredTrigger.RepeatCount)
                            {
                                //关闭Trigger
                                log.Debug("Close Trigger:" + oldTobeFiredTrigger.Name);
                                oldTobeFiredTrigger.Status = BusinessConstants.CODE_MASTER_STATUS_VALUE_CLOSE;
                                oldTobeFiredTrigger.NextFireTime = null;
                            }
                            else
                            {
                                //设置下次运行时间
                                log.Debug("Set Trigger Next Start Time, Add:" + oldTobeFiredTrigger.Interval.ToString() + " " + oldTobeFiredTrigger.IntervalType);
                                //if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_YEAR)
                                //{
                                //    oldTobeFiredTrigger.NextFireTime = dateTimeNow.AddYears(oldTobeFiredTrigger.Interval);
                                //}
                                //else if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_MONTH)
                                //{
                                //    oldTobeFiredTrigger.NextFireTime = dateTimeNow.AddMonths(oldTobeFiredTrigger.Interval);
                                //}
                                //else if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_DAY)
                                //{
                                //    oldTobeFiredTrigger.NextFireTime = dateTimeNow.AddDays(oldTobeFiredTrigger.Interval);
                                //}
                                //else if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_HOUR)
                                //{
                                //    oldTobeFiredTrigger.NextFireTime = dateTimeNow.AddHours(oldTobeFiredTrigger.Interval);
                                //}
                                //else if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_MINUTE)
                                //{
                                //    oldTobeFiredTrigger.NextFireTime = dateTimeNow.AddMinutes(oldTobeFiredTrigger.Interval);
                                //}
                                //else if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_SECOND)
                                //{
                                //    oldTobeFiredTrigger.NextFireTime = dateTimeNow.AddSeconds(oldTobeFiredTrigger.Interval);
                                //}
                                //else if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_MILLISECOND)
                                //{
                                //    oldTobeFiredTrigger.NextFireTime = dateTimeNow.AddMilliseconds(oldTobeFiredTrigger.Interval);
                                //}      
                                DateTime dateTimeNow = DateTime.Now;
                                if (!oldTobeFiredTrigger.NextFireTime.HasValue)
                                {
                                    oldTobeFiredTrigger.NextFireTime = dateTimeNow;
                                }

                                while (oldTobeFiredTrigger.NextFireTime.Value <= dateTimeNow)
                                {
                                    if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_YEAR)
                                    {
                                        oldTobeFiredTrigger.NextFireTime = oldTobeFiredTrigger.NextFireTime.Value.AddYears(oldTobeFiredTrigger.Interval);
                                    }
                                    else if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_MONTH)
                                    {
                                        oldTobeFiredTrigger.NextFireTime = oldTobeFiredTrigger.NextFireTime.Value.AddMonths(oldTobeFiredTrigger.Interval);
                                    }
                                    else if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_DAY)
                                    {
                                        oldTobeFiredTrigger.NextFireTime = oldTobeFiredTrigger.NextFireTime.Value.AddDays(oldTobeFiredTrigger.Interval);
                                    }
                                    else if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_HOUR)
                                    {
                                        oldTobeFiredTrigger.NextFireTime = oldTobeFiredTrigger.NextFireTime.Value.AddHours(oldTobeFiredTrigger.Interval);
                                    }
                                    else if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_MINUTE)
                                    {
                                        oldTobeFiredTrigger.NextFireTime = oldTobeFiredTrigger.NextFireTime.Value.AddMinutes(oldTobeFiredTrigger.Interval);
                                    }
                                    else if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_SECOND)
                                    {
                                        oldTobeFiredTrigger.NextFireTime = oldTobeFiredTrigger.NextFireTime.Value.AddSeconds(oldTobeFiredTrigger.Interval);
                                    }
                                    else if (oldTobeFiredTrigger.IntervalType == BusinessConstants.DATETIME_TYPE_MILLISECOND)
                                    {
                                        oldTobeFiredTrigger.NextFireTime = oldTobeFiredTrigger.NextFireTime.Value.AddMilliseconds(oldTobeFiredTrigger.Interval);
                                    }
                                    else
                                    {
                                        throw new ArgumentException("invalid Interval Type:" + oldTobeFiredTrigger.IntervalType);
                                    }
                                }
                                log.Debug("Trigger Next Start Time is set as:" + oldTobeFiredTrigger.NextFireTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                            }

                            this.batchTriggerMgrE.UpdateBatchTrigger(oldTobeFiredTrigger);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occur when update batch trigger.", ex);
                        }
                        #endregion
                    }
                }
            }
            else
            {
                log.Info("No job found may run in this batch.");
            }

            log.Info("BatchJobs run end.");
        }

        #endregion
    }
}



#region Extend Class


namespace com.Sconit.Service.Ext.Batch.Impl
{
    public partial class JobRunMgrE : com.Sconit.Service.Batch.Impl.JobRunMgr, IJobRunMgrE
    {

    }
}

#endregion
