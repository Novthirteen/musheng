﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Core.Resource;
using BatchJob.Properties;
using com.Sconit.Utility;
using com.Sconit.Service.Batch;
using System.Threading;


namespace BatchJob
{
    partial class BatchJob : ServiceBase
    {
        private static log4net.ILog log;
        private static IWindsorContainer container = null;

        public System.Timers.Timer timer;

        public BatchJob()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
            log = log4net.LogManager.GetLogger("Log.BatchJob");
            this.ServiceName = "BatchJob";
        }

        protected override void OnStart(string[] args)
        {
            Thread.Sleep(30000);
            try
            {
                log.Info("BatchJob Service Start");
                container = new WindsorContainer(new XmlInterpreter(new ConfigResource("castle")));

                timer = new System.Timers.Timer();
                timer.Interval = Convert.ToDouble(TimerHelper.GetInterval(Settings.Default.IntervalType, Convert.ToInt32(Settings.Default.IntervalValue)));
                timer.Enabled = true;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            }
            catch (Exception ex)
            {
                log.Error("BatchJob Service Start Failure", ex);
            }
        }

        protected override void OnStop()
        {
            log.Info("BatchJob Service Stop");
            if (container != null)
            {
                container.Dispose();
            }
        }

        private void RunJob()
        {
            try
            {
                IJobRunMgr jobRunMgr = container.Resolve<IJobRunMgr>("JobRunMgr.service");
                jobRunMgr.RunBatchJobs(container);
            }
            catch (Exception ex)
            {
                log.Error("Batch Job Run Failure", ex);
            }
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = !Settings.Default.InterruptTimer;
            RunJob();
            timer.Enabled = true;
        }
    }
}
