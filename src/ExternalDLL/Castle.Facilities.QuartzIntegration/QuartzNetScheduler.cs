﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Castle.Core;
using Quartz;
using Quartz.Collection;
using Quartz.Impl;
using Quartz.Spi;

namespace Castle.Facilities.QuartzIntegration {
	public class QuartzNetScheduler : IScheduler, IStartable, IDisposable {
		private readonly IScheduler scheduler;
		private readonly NameValueCollection properties = new NameValueCollection();
		public bool WaitForJobsToCompleteAtShutdown { get; set; }

		public IJobListener[] SetGlobalJobListeners {
			set {
				foreach (var jobListener in value)
					scheduler.AddGlobalJobListener(jobListener);
			}
		}

		public ITriggerListener[] SetGlobalTriggerListeners {
			set {
				foreach (var triggerListener in value) {
					scheduler.AddGlobalTriggerListener(triggerListener);
				}
			}
		}

		public ISchedulerListener[] SetSchedulerListeners {
			set {
				foreach (var schedulerListener in value) {
					scheduler.AddSchedulerListener(schedulerListener);
				}
			}
		}

		private readonly IDictionary<string, IList<IJobListener>> jobListeners = new Dictionary<string, IList<IJobListener>>();

		public IDictionary JobListeners {
			set {
				foreach (DictionaryEntry jl in value) {
				    var listeners = new List<IJobListener>();
					foreach (IJobListener jobListener in jl.Value as IList) {
						scheduler.AddJobListener(jobListener);
                        listeners.Add(jobListener);
					}
                    jobListeners[jl.Key as string] = listeners;
				}
			}
		}

		private readonly IDictionary<string, IList<ITriggerListener>> triggerListeners = new Dictionary<string, IList<ITriggerListener>>();

		public IDictionary TriggerListeners {
			set {
				foreach (DictionaryEntry tl in value) {
                    var listeners = new List<ITriggerListener>();
					foreach (ITriggerListener triggerListener in tl.Value as IList) {
						scheduler.AddTriggerListener(triggerListener);
                        listeners.Add(triggerListener);
					}
				    triggerListeners[tl.Key as string] = listeners;
				}
			}
		}

		public IList GlobalJobListeners {
			get { return scheduler.GlobalJobListeners; }
		}

		public QuartzNetScheduler(IDictionary<string, string> props, IJobFactory jobFactory) {
			foreach (var prop in props.Keys) {
				properties[prop] = props[prop];
			}
			var sf = new StdSchedulerFactory(properties);
			scheduler = sf.GetScheduler();
			scheduler.JobFactory = jobFactory;
			WaitForJobsToCompleteAtShutdown = true; // default
		}

		public bool IsJobGroupPaused(string groupName) {
			return scheduler.IsJobGroupPaused(groupName);
		}

		public bool IsTriggerGroupPaused(string groupName) {
			return scheduler.IsTriggerGroupPaused(groupName);
		}

		public SchedulerMetaData GetMetaData() {
			return scheduler.GetMetaData();
		}

		public IList GetCurrentlyExecutingJobs() {
			return scheduler.GetCurrentlyExecutingJobs();
		}

		public ISet GetPausedTriggerGroups() {
			return scheduler.GetPausedTriggerGroups();
		}

		public IJobListener GetGlobalJobListener(string name) {
			return scheduler.GetGlobalJobListener(name);
		}

		public ITriggerListener GetGlobalTriggerListener(string name) {
			return scheduler.GetGlobalTriggerListener(name);
		}

		public void Start() {
			scheduler.Start();
			scheduler.PauseAll();
			foreach (var jobName in jobListeners) {
				var jobDetail = GetJobDetail(jobName.Key, null);
				foreach (var jobListener in jobName.Value) {
					jobDetail.AddJobListener(jobListener.Name);
				}
				foreach (var t in scheduler.GetTriggersOfJob(jobDetail.Name, jobDetail.Group)) {
					scheduler.DeleteJob(jobDetail.Name, jobDetail.Group);
					scheduler.ScheduleJob(jobDetail, t);
				}
			}
			foreach (var t in triggerListeners) {
				var trigger = GetTrigger(t.Key, null);
				foreach (var triggerListener in t.Value) {
					trigger.AddTriggerListener(triggerListener.Name);
				}
				var job = scheduler.GetJobDetail(trigger.JobName, trigger.JobGroup);
				scheduler.DeleteJob(job.Name, job.Group);
				scheduler.ScheduleJob(job, trigger);
			}
			scheduler.ResumeAll();
		}

		public void StartDelayed(TimeSpan delay) {
			scheduler.StartDelayed(delay);
		}

		public void Stop() {
			scheduler.Shutdown(WaitForJobsToCompleteAtShutdown);
		}

		public void Standby() {
			scheduler.Standby();
		}

		public void Shutdown() {
			Stop();
		}

		public void Shutdown(bool waitForJobsToComplete) {
			scheduler.Shutdown(waitForJobsToComplete);
		}

		public DateTime ScheduleJob(JobDetail jobDetail, Trigger trigger) {
			return scheduler.ScheduleJob(jobDetail, trigger);
		}

		public DateTime ScheduleJob(Trigger trigger) {
			return scheduler.ScheduleJob(trigger);
		}

		public bool UnscheduleJob(string triggerName, string groupName) {
			return scheduler.UnscheduleJob(triggerName, groupName);
		}

		public DateTime? RescheduleJob(string triggerName, string groupName, Trigger newTrigger) {
			return scheduler.RescheduleJob(triggerName, groupName, newTrigger);
		}

		public void AddJob(JobDetail jobDetail, bool replace) {
			scheduler.AddJob(jobDetail, replace);
		}

		public bool DeleteJob(string jobName, string groupName) {
			return scheduler.DeleteJob(jobName, groupName);
		}

		public void TriggerJob(string jobName, string groupName) {
			scheduler.TriggerJob(jobName, groupName);
		}

		public void TriggerJobWithVolatileTrigger(string jobName, string groupName) {
			scheduler.TriggerJobWithVolatileTrigger(jobName, groupName);
		}

		public void TriggerJob(string jobName, string groupName, JobDataMap data) {
			scheduler.TriggerJob(jobName, groupName, data);
		}

		public void TriggerJobWithVolatileTrigger(string jobName, string groupName, JobDataMap data) {
			scheduler.TriggerJobWithVolatileTrigger(jobName, groupName, data);
		}

		public void PauseJob(string jobName, string groupName) {
			scheduler.PauseJob(jobName, groupName);
		}

		public void PauseJobGroup(string groupName) {
			scheduler.PauseJobGroup(groupName);
		}

		public void PauseTrigger(string triggerName, string groupName) {
			scheduler.PauseTrigger(triggerName, groupName);
		}

		public void PauseTriggerGroup(string groupName) {
			scheduler.PauseTriggerGroup(groupName);
		}

		public void ResumeJob(string jobName, string groupName) {
			scheduler.ResumeJob(jobName, groupName);
		}

		public void ResumeJobGroup(string groupName) {
			scheduler.ResumeJobGroup(groupName);
		}

		public void ResumeTrigger(string triggerName, string groupName) {
			scheduler.ResumeTrigger(triggerName, groupName);
		}

		public void ResumeTriggerGroup(string groupName) {
			scheduler.ResumeTriggerGroup(groupName);
		}

		public void PauseAll() {
			scheduler.PauseAll();
		}

		public void ResumeAll() {
			scheduler.ResumeAll();
		}

		public string[] GetJobNames(string groupName) {
			return scheduler.GetJobNames(groupName);
		}

		public Trigger[] GetTriggersOfJob(string jobName, string groupName) {
			return scheduler.GetTriggersOfJob(jobName, groupName);
		}

		public string[] GetTriggerNames(string groupName) {
			return scheduler.GetTriggerNames(groupName);
		}

		public JobDetail GetJobDetail(string jobName, string jobGroup) {
			return scheduler.GetJobDetail(jobName, jobGroup);
		}

		public Trigger GetTrigger(string triggerName, string triggerGroup) {
			return scheduler.GetTrigger(triggerName, triggerGroup);
		}

		public TriggerState GetTriggerState(string triggerName, string triggerGroup) {
			return scheduler.GetTriggerState(triggerName, triggerGroup);
		}

		public void AddCalendar(string calName, ICalendar calendar, bool replace, bool updateTriggers) {
			scheduler.AddCalendar(calName, calendar, replace, updateTriggers);
		}

		public bool DeleteCalendar(string calName) {
			return scheduler.DeleteCalendar(calName);
		}

		public ICalendar GetCalendar(string calName) {
			return scheduler.GetCalendar(calName);
		}

		public string[] GetCalendarNames() {
			return scheduler.GetCalendarNames();
		}

		public bool Interrupt(string jobName, string groupName) {
			return scheduler.Interrupt(jobName, groupName);
		}

		public void AddGlobalJobListener(IJobListener jobListener) {
			scheduler.AddGlobalJobListener(jobListener);
		}

		public void AddJobListener(IJobListener jobListener) {
			scheduler.AddJobListener(jobListener);
		}

		public bool RemoveGlobalJobListener(IJobListener jobListener) {
			return scheduler.RemoveGlobalJobListener(jobListener);
		}

		public bool RemoveGlobalJobListener(string name) {
			return scheduler.RemoveGlobalJobListener(name);
		}

		public bool RemoveJobListener(string name) {
			return scheduler.RemoveJobListener(name);
		}

		public IJobListener GetJobListener(string name) {
			return scheduler.GetJobListener(name);
		}

		public void AddGlobalTriggerListener(ITriggerListener triggerListener) {
			scheduler.AddGlobalTriggerListener(triggerListener);
		}

		public void AddTriggerListener(ITriggerListener triggerListener) {
			scheduler.AddTriggerListener(triggerListener);
		}

		public bool RemoveGlobalTriggerListener(ITriggerListener triggerListener) {
			return scheduler.RemoveGlobalTriggerListener(triggerListener);
		}

		public bool RemoveGlobalTriggerListener(string name) {
			return scheduler.RemoveGlobalTriggerListener(name);
		}

		public bool RemoveTriggerListener(string name) {
			return scheduler.RemoveTriggerListener(name);
		}

		public ITriggerListener GetTriggerListener(string name) {
			return scheduler.GetTriggerListener(name);
		}

		public void AddSchedulerListener(ISchedulerListener schedulerListener) {
			scheduler.AddSchedulerListener(schedulerListener);
		}

		public bool RemoveSchedulerListener(ISchedulerListener schedulerListener) {
			return scheduler.RemoveSchedulerListener(schedulerListener);
		}

		public string SchedulerName {
			get { return scheduler.SchedulerName; }
		}

		public string SchedulerInstanceId {
			get { return scheduler.SchedulerInstanceId; }
		}

		public SchedulerContext Context {
			get { return scheduler.Context; }
		}

		public bool InStandbyMode {
			get { return scheduler.InStandbyMode; }
		}

		public bool IsShutdown {
			get { return scheduler.IsShutdown; }
		}

		public IJobFactory JobFactory {
			set { scheduler.JobFactory = value; }
		}

		public string[] JobGroupNames {
			get { return scheduler.JobGroupNames; }
		}

		public string[] TriggerGroupNames {
			get { return scheduler.TriggerGroupNames; }
		}

		public string[] CalendarNames {
			get { return scheduler.CalendarNames; }
		}

		public ISet JobListenerNames {
			get { return scheduler.JobListenerNames; }
		}

		public IList GlobalTriggerListeners {
			get { return scheduler.GlobalTriggerListeners; }
		}

		public ISet TriggerListenerNames {
			get { return scheduler.TriggerListenerNames; }
		}

		public IList SchedulerListeners {
			get { return scheduler.SchedulerListeners; }
		}

		public bool IsStarted {
			get { return scheduler.IsStarted; }
		}

		public void Dispose() {
			Stop();
		}
	}
}