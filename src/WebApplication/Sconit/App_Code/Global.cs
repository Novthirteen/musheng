using System;
using System.Web;
using Castle.Core.Resource;
using Castle.Facilities.QuartzIntegration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.MicroKernel;
using Castle.MicroKernel.SubSystems.Conversion;
using com.Sconit.Service.Ext.MasterData;

namespace com.Sconit.Web
{     
    /// <summary>
    /// Summary description for Global
    /// </summary>
    public class Global : HttpApplication, IContainerAccessor
    {
        private static WindsorContainer container = null;
        private static log4net.ILog log = log4net.LogManager.GetLogger("Application");

        void Application_Start(object sender, EventArgs e)
        {
            // ��Ӧ�ó�������ʱ���еĴ���
            // init log4net
            log4net.Config.XmlConfigurator.Configure();

            container = new WindsorContainer(new XmlInterpreter(new ConfigResource()));

            //DB��ʼ��

            /*
            IKernel kernel = container.Kernel;
            IConversionManager conversionMng = (IConversionManager)
            kernel.GetSubSystem(SubSystemConstants.ConversionManagerKey);
            conversionMng.Add(new ReportTypeConverter());
             */
        }

        void Application_End(object sender, EventArgs e)
        {
            container.Dispose();

            //  ��Ӧ�ó���ر�ʱ���еĴ���
        }

        void Application_Error(object sender, EventArgs e)
        {
            //todo δ�����쳣����,ҳ��ͳһ��������
            Exception errInfo = Server.GetLastError();
            bool bSend = false;
            string msg = errInfo.Source + ":" + errInfo.Message;

            string msgBody = "URL:" + Request.RawUrl + "\n";
            msgBody += errInfo.Message + "\n" + errInfo.Message + "\n";
            msgBody += "Error StackTrack:" + errInfo.StackTrace + "\n";
            msgBody += "Time:" + DateTime.Now.ToString() + "\n";

            Response.Write(msgBody);
            //Server.ClearError();
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            //todo ��֤�û��Ƿ��¼��SessionʧЧ
            //���µ�½�󷵻�ԭҳ��
        }

        void Session_Start(object sender, EventArgs e)
        {
            // �ڻỰ��ʼʱ���еĴ��롣
        }

        void Session_End(object sender, EventArgs e)
        {
            // �ڻỰ����ʱ���еĴ��롣 
            // ע��: ֻ���� Web.config �ļ��е� sessionstate ģʽ����Ϊ
            // InProc ʱ���Ż����� Session_End �¼�������Ựģʽ����Ϊ StateServer 
            // �� SQLServer���򲻻��������¼���
            //string sessionID = Session.SessionID;
            //CasClient.Utils.CommonUtils.RemoveState(sessionID);
        }

        #region IContainerAccessor implementation

        public IWindsorContainer Container
        {
            get { return container; }
        }

        #endregion IContainerAccessor implementation


    }
}
