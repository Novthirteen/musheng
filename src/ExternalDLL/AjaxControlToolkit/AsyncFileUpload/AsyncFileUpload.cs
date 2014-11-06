﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Script;
using System.ComponentModel;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using AjaxControlToolkit;
using System.Text;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;

#region [ Resources ]

[assembly: WebResource("AjaxControlToolkit.AsyncFileUpload.AsyncFileUpload.js", "application/x-javascript")]
[assembly: WebResource("AjaxControlToolkit.AsyncFileUpload.AsyncFileUpload.debug.js", "application/x-javascript")]

[assembly: WebResource("AjaxControlToolkit.AsyncFileUpload.images.fileupload.png", "image/png")]

#endregion

namespace AjaxControlToolkit
{
    [Designer("AjaxControlToolkit.AsyncFileUploadDesigner, AjaxControlToolkit")]
    [RequiredScript(typeof(CommonToolkitScripts))]
    [ClientScriptResource("AjaxControlToolkit.AsyncFileUpload", "AjaxControlToolkit.AsyncFileUpload.AsyncFileUpload.js")]
    public class AsyncFileUpload : ScriptControlBase
    {
        #region [ Phrases Static Class ]
        public static class Constants
        {
            public readonly static string FileUploadIDKey = "AsyncFileUploadID";
            public readonly static string InternalErrorInvalidIFrame = "The ExtendedFileUpload control has encountered an error with the uploader in this page. Please refresh the page and try again.";
            public readonly static string fileUploadGUID = "b3b89160-3224-476e-9076-70b500c816cf";
            public static class Errors
            {
                public readonly static string NoFiles = "No files are attached to the upload.";
                public readonly static string FileNull = "The file attached is invalid.";
                public readonly static string NoFileName = "The file attached has an invalid filename.";
                public readonly static string InputStreamNull = "The file attached could not be read.";
                public readonly static string EmptyContentLength = "The file attached is empty.";
            }
            public static class StatusMessages
            {
                public readonly static string UploadSuccessful = "The file uploaded successfully.";
            }
        }
        #endregion

        #region [ Public Enums ]
        public enum UploaderStyleEnum
        {
            Traditional = 0,
            Modern = 1
        }

        public enum PersistedStoreTypeEnum
        {
            Session = 0
        }
        #endregion

        #region [ Fields ]

        private PersistedStoreTypeEnum persistStorageType = PersistedStoreTypeEnum.Session;
        private string lastError = String.Empty;
        private bool failedValidation = false;
        private UploaderStyleEnum controlStyle = UploaderStyleEnum.Traditional;
        private string hiddenFieldID = String.Empty;
        private string innerTBID = String.Empty;
        private HtmlInputFile inputFile = null;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new AsyncFileUpload
        /// </summary>
        public AsyncFileUpload()
            : base(true, HtmlTextWriterTag.Span)
        {
        }

        #endregion

        #region [ Public Server Events ]

        [Bindable(true)]
        [Category("Server Events")]
        public event EventHandler<AsyncFileUploadEventArgs> UploadedComplete;

        [Bindable(true)]
        [Category("Server Events")]
        public event EventHandler<AsyncFileUploadEventArgs> UploadedFileError;
 
        #endregion

        #region [ Public Client Events ]

        [DefaultValue("")]
        [Category("Behavior")]
        [ExtenderControlEvent]
        [ClientPropertyName("uploadStarted")]
        public string OnClientUploadStarted
        {
            get { return (string)(ViewState["OnClientUploadStarted"] ?? string.Empty); }
            set { ViewState["OnClientUploadStarted"] = value; }
        }

        [DefaultValue("")]
        [Category("Behavior")]
        [ExtenderControlEvent]
        [ClientPropertyName("uploadComplete")]
        public string OnClientUploadComplete
        {
            get { return (string)(ViewState["OnClientUploadComplete"] ?? string.Empty); }
            set { ViewState["OnClientUploadComplete"] = value; }
        }

        [DefaultValue("")]
        [Category("Behavior")]
        [ExtenderControlEvent]
        [ClientPropertyName("uploadError")]
        public string OnClientUploadError
        {
            get { return (string)(ViewState["OnClientUploadError"] ?? string.Empty); }
            set { ViewState["OnClientUploadError"] = value; }
        }

        #endregion

        #region [ Private Properties ]
        private bool IsDesignMode
        {
            get { return (HttpContext.Current == null); }
        }
        #endregion

        #region [ Public Properties ]
        [BrowsableAttribute(false)]
        public byte[] FileBytes
        {
            get
            {
                PopulatObjectPriorToRender(this.ClientID);
                HttpPostedFile file = AfuPersistedStoreManager.Instance.GetFileFromSession(this.ClientID);
                if (file != null)
                {
                    try
                    {
                        return GetBytesFromStream(file.InputStream);
                    }
                    catch
                    {
                    }
                }
                return null;
            }
        }

        [Category("Behavior")]
        [Description("ID of Throbber")]
        [DefaultValue("")]
        public string ThrobberID
        {
            get { return (string)(ViewState["ThrobberID"] ?? string.Empty); }
            set { ViewState["ThrobberID"] = value; }
        }

        [Category("Appearance")]
        [TypeConverter(typeof(WebColorConverter))]
        [Description("Control's background color on upload complete.")]
        [DefaultValue(typeof(Color), "Lime")]
        public Color CompleteBackColor
        {
            get { return (Color)(ViewState["CompleteBackColor"] ?? Color.Lime); }
            set { ViewState["CompleteBackColor"] = value; }
        }

        [Category("Appearance")]
        [TypeConverter(typeof(WebColorConverter))]
        [Description("Control's background color when uploading is in progress.")]
        [DefaultValue(typeof(Color), "White")]
        public Color UploadingBackColor
        {
            get { return (Color)(ViewState["UploadingBackColor"] ?? Color.White); }
            set { ViewState["UploadingBackColor"] = value; }
        }

        [Category("Appearance")]
        [TypeConverter(typeof(WebColorConverter))]
        [Description("Control's background color on upload error.")]
        [DefaultValue(typeof(Color), "Red")]
        public Color ErrorBackColor
        {
            get { return (Color)(ViewState["ErrorBackColor"] ?? Color.Red); }
            set { ViewState["ErrorBackColor"] = value; }
        }

        [DefaultValue(typeof(Unit), "")]
        [Category("Layout")]
        public override Unit Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }

        [BrowsableAttribute(false)]
        public bool FailedValidation
        {
            get { return failedValidation; }
            set { failedValidation = value; }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(AsyncFileUpload.PersistedStoreTypeEnum.Session)]
        public PersistedStoreTypeEnum PersistedStoreType
        {
            get { return persistStorageType; }
            set { persistStorageType = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [BrowsableAttribute(true)]
        [DefaultValue(UploaderStyleEnum.Traditional)]
        public UploaderStyleEnum UploaderStyle
        {
            get { return controlStyle; }
            set { controlStyle = value; }
        }

        [BrowsableAttribute(false)]
        public HttpPostedFile PostedFile
        {
            get
            {
                PopulatObjectPriorToRender(this.ClientID);
                return AfuPersistedStoreManager.Instance.GetFileFromSession(this.ClientID);
            }
        }

        [BrowsableAttribute(false)]
        public bool HasFile
        {
            get
            {
                PopulatObjectPriorToRender(this.ClientID);
                return AfuPersistedStoreManager.Instance.FileExists(this.ClientID);
            }
        }

        [BrowsableAttribute(false)]
        public string FileName
        {
            get
            {
                PopulatObjectPriorToRender(this.ClientID);
                return Path.GetFileName(AfuPersistedStoreManager.Instance.GetFileName(this.ClientID));
            }
        }

        [BrowsableAttribute(false)]
        public string ContentType
        {
            get
            {
                PopulatObjectPriorToRender(this.ClientID);
                return AfuPersistedStoreManager.Instance.GetContentType(this.ClientID);
            }
        }

        [BrowsableAttribute(false)]
        public Stream FileContent
        {
            get
            {
                PopulatObjectPriorToRender(this.ClientID);
                HttpPostedFile postedFile = AfuPersistedStoreManager.Instance.GetFileFromSession(this.ClientID);
                if (postedFile.InputStream != null)
                {
                    return postedFile.InputStream;
                }
                else
                {
                    return null;
                }
            }
        }

        [BrowsableAttribute(false)]
        public bool IsUploading
        {
            get
            {
                return (this.Page.Request.QueryString[Constants.FileUploadIDKey] != null);
            }
        }
        #endregion

        #region [ Methods ]

        public void SaveAs(string filename)
        {
            PopulatObjectPriorToRender(this.ClientID);
            HttpPostedFile postedFile = AfuPersistedStoreManager.Instance.GetFileFromSession(this.ClientID);
            postedFile.SaveAs(filename);
        }
        
        private void PopulatObjectPriorToRender(string controlId)
        {
            if ((!AfuPersistedStoreManager.Instance.FileExists(controlId)) && (this.Page != null && this.Page.Request.Files.Count != 0))
            {
                RecievedFile(controlId);
            }
        }

        protected virtual void OnUploadedFileError(AsyncFileUploadEventArgs e)
        {
            if (UploadedFileError != null)
            {
                UploadedFileError(this, e);
            }
        }

        protected virtual void OnUploadedComplete(AsyncFileUploadEventArgs e)
        {
            if (UploadedComplete != null)
            {
                UploadedComplete(this, e);
            }
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OnUploadedComplete", "top.$find(\"" + this.ClientID + "\")._stopLoad('111------www');", true);
        }

        private void RecievedFile(string sendingControlID)
        {
            AsyncFileUploadEventArgs eventArgs = null;
            lastError = String.Empty;

            if (this.Page.Request.Files.Count > 0)
            {
                HttpPostedFile file = null;
                if (sendingControlID == null || sendingControlID == String.Empty)
                {
                    file = this.Page.Request.Files[0];
                }
                else
                {
                    foreach (string postedFile in this.Page.Request.Files)
                    {
                        if (postedFile.Replace("$","_").StartsWith(sendingControlID))
                        {
                            file = this.Page.Request.Files[postedFile];
                            break;
                        }
                    }
                }
                if (file == null)
                {
                    lastError = Constants.Errors.FileNull;
                    eventArgs = new AsyncFileUploadEventArgs(AsyncFileUploadState.Failed, Constants.Errors.FileNull, String.Empty, String.Empty);
                    OnUploadedFileError(eventArgs);
                }
                else if (file.FileName == String.Empty)
                {
                    lastError = Constants.Errors.NoFileName;
                    eventArgs = new AsyncFileUploadEventArgs(AsyncFileUploadState.Unknown, Constants.Errors.NoFileName, file.FileName, file.ContentLength.ToString());
                    OnUploadedFileError(eventArgs);
                }
                else if (file.InputStream == null)
                {
                    lastError = Constants.Errors.NoFileName;
                    eventArgs = new AsyncFileUploadEventArgs(AsyncFileUploadState.Failed, Constants.Errors.NoFileName, file.FileName, file.ContentLength.ToString());
                    OnUploadedFileError(eventArgs);
                }
                else if (file.ContentLength < 1)
                {
                    lastError = Constants.Errors.EmptyContentLength;
                    eventArgs = new AsyncFileUploadEventArgs(AsyncFileUploadState.Unknown, Constants.Errors.EmptyContentLength, file.FileName, file.ContentLength.ToString());
                    OnUploadedFileError(eventArgs);
                }
                else
                {
                    eventArgs = new AsyncFileUploadEventArgs(AsyncFileUploadState.Success, String.Empty, file.FileName, file.ContentLength.ToString());
                    GC.SuppressFinalize(file);
                    AfuPersistedStoreManager.Instance.AddFileToSession(this.ClientID, file.FileName, file);
                    OnUploadedComplete(eventArgs);
                }
            }
        }

        public byte[] GetBytesFromStream(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                stream.Seek(0, SeekOrigin.Begin);
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                    {
                        return ms.ToArray();
                    }
                    ms.Write(buffer, 0, read);
                }
            }
        }

        #endregion

        #region [ Members ]


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            AfuPersistedStoreManager.Instance.PersistedStorageType = (AfuPersistedStoreManager.PersistedStoreTypeEnum)this.PersistedStoreType;

            string sendingControlID = this.Page.Request.QueryString[Constants.FileUploadIDKey];

            if ((sendingControlID != null && sendingControlID.StartsWith(this.ClientID)) || sendingControlID == null)
            {
                RecievedFile(this.ClientID);
                if (sendingControlID != null && sendingControlID.StartsWith(this.ClientID))
                {
                    if (lastError == String.Empty)
                    {
                        byte[] bytes = this.FileBytes;
                        if (bytes != null)
                        {
                            Controls.Add(new LiteralControl(bytes.Length.ToString() + "------" + ContentType));
                        }
                    }
                    else
                    {
                        Controls.Add(new LiteralControl("error------" + lastError));
                    }
                }
            }
        }

        internal void CreateChilds()
        {
            this.Controls.Clear();
            this.CreateChildControls();
        }

        protected override void CreateChildControls()
        {
            AfuPersistedStoreManager.Instance.ExtendedFileUploadGUID = Constants.fileUploadGUID;
            string sendingControlID = null;
            if (!IsDesignMode)
            {
                sendingControlID = this.Page.Request.QueryString[Constants.FileUploadIDKey];
            }
            if ((IsDesignMode || sendingControlID == null || sendingControlID == String.Empty))
            {
                this.hiddenFieldID = GenerateHtmlInputHiddenControl();
                string lastFileName = String.Empty;

                if (AfuPersistedStoreManager.Instance.FileExists(this.ClientID))
                {
                    lastFileName = AfuPersistedStoreManager.Instance.GetFileName(this.ClientID);
                }

                GenerateHtmlInputFileControl(lastFileName);
            }
        }

        //protected override void RenderContents(HtmlTextWriter output)
        //{
        //    base.RenderContents(output);
        //    AfuPersistedStoreManager.Instance.ExtendedFileUploadGUID = Constants.fileUploadGUID;
        //    string sendingControlID = null;
        //    if (!IsDesignMode)
        //    {
        //        sendingControlID = this.Page.Request.QueryString[Constants.FileUploadIDKey];
        //    }
        //    if ((IsDesignMode || sendingControlID == null || sendingControlID == String.Empty))
        //    {
        //    }
        //    else
        //    {
        //        byte[] bytes = this.FileBytes;
        //        if (bytes != null)
        //        {
        //            output.Write(bytes.Length.ToString());
        //        }
        //    }

        //}

        protected string GenerateHtmlInputHiddenControl ()
        {
            HiddenField field = new HiddenField();
            Controls.Add(field);
            return field.ClientID;
        }

        protected string GenerateHtmlInputFileControl(string lastFileName)
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            Controls.Add(div);
            div.Attributes.Add("name", div.ClientID);

            if (this.UploaderStyle == UploaderStyleEnum.Modern)
            {
                string bgImage = String.Empty;
                bgImage = Page.ClientScript.GetWebResourceUrl(typeof(AsyncFileUpload), "AjaxControlToolkit.AsyncFileUpload.images.fileupload.png");
                string style = "background:url(" + bgImage + ") no-repeat 100% 1px; height:24px; margin:0px;";
                if (!Width.IsEmpty)
                {
                    style += "width:" + Width.ToString() + ";";
                }
                else
                {
                    style += "width:355px;";
                }
                div.Attributes.Add("style", style);
            }

            if (!(this.UploaderStyle == UploaderStyleEnum.Modern && IsDesignMode))
            {
                inputFile = new HtmlInputFile();
                div.Controls.Add(inputFile);
                inputFile.Attributes.Add("id", inputFile.ClientID);
                inputFile.Attributes.Add("onkeydown", "return false;");
                inputFile.Attributes.Add("onkeypress", "return false;");
                inputFile.Attributes.Add("onmousedown", "return false;");
                if (this.UploaderStyle != UploaderStyleEnum.Modern)
                {
                    if (BackColor != Color.Empty)
                    {
                        inputFile.Style[HtmlTextWriterStyle.BackgroundColor] = ColorTranslator.ToHtml(BackColor);
                    }
                    if (!Width.IsEmpty)
                    {
                        inputFile.Style[HtmlTextWriterStyle.Width] = Width.ToString();
                    }
                    else
                    {
                        inputFile.Style[HtmlTextWriterStyle.Width] = "355px";
                    }
                }
            }

            if (this.UploaderStyle == UploaderStyleEnum.Modern)
            {
                string style = "opacity:0.0; -moz-opacity: 0.0; filter: alpha(opacity=00); font-size:14px;";
                if (!Width.IsEmpty)
                {
                    style += "width:"+Width.ToString()+";";
                }
                if (inputFile != null) inputFile.Attributes.Add("style", style);
                TextBox textBox = new TextBox();

                if (!IsDesignMode)
                {
                    HtmlGenericControl div1 = new HtmlGenericControl("div");
                    div.Controls.Add(div1);
                    div1.Attributes.Add("name", div.ClientID);
                    style = "margin-top:-23px;";
                    div1.Attributes.Add("style", style);
                    div1.Attributes.Add("type", "text");
                    div1.Controls.Add(textBox);
                    style = "height:17px; font-size:12px; font-family:Tahoma;";
                }
                else
                {
                    div.Controls.Add(textBox);
                    style = "height:23px; font-size:12px; font-family:Tahoma;";
                }
                if (!Width.IsEmpty && Width.ToString().IndexOf("px") > 0)
                {
                    style += "width:" + (int.Parse(Width.ToString().Substring(0, Width.ToString().IndexOf("px"))) - 107).ToString() +"px;";
                }
                else
                {
                    style += "width:248px;";
                }
                if (lastFileName != String.Empty || this.failedValidation)
                {
                    if ((this.FileBytes != null && this.FileBytes.Length > 0) && (!this.failedValidation))
                    {
                        style += "background-color:#00FF00;";
                    }
                    else
                    {
                        this.failedValidation = false;
                        style += "background-color:#FF0000;";
                    }
                    textBox.Text = lastFileName;
                }
                else if (BackColor != Color.Empty)
                {
                    style += "background-color:"+ColorTranslator.ToHtml(BackColor)+";";
                }
                textBox.ReadOnly = true;
                textBox.Attributes.Add("style", style);
                this.innerTBID = textBox.ClientID;
            }
            else if (IsDesignMode)
            {
                Controls.Clear();
                Controls.Add(inputFile);
            }

            return div.ClientID;
        }

        protected override void DescribeComponent(ScriptComponentDescriptor descriptor)
        {
            base.DescribeComponent(descriptor);
            if (!IsDesignMode)
            {
                if (this.hiddenFieldID != String.Empty) descriptor.AddElementProperty("hiddenField", this.hiddenFieldID);
                if (this.innerTBID != String.Empty) descriptor.AddElementProperty("innerTB", this.innerTBID);
                if (this.inputFile != null) descriptor.AddElementProperty("inputFile", this.inputFile.ClientID);
                descriptor.AddProperty("postBackUrl", Path.GetFileName(this.Page.Request.FilePath));
                descriptor.AddProperty("formName", Path.GetFileName(this.Page.Form.Name));
                if (CompleteBackColor != Color.Empty)
                {
                    descriptor.AddProperty("completeBackColor", ColorTranslator.ToHtml(CompleteBackColor));
                }
                if (ErrorBackColor != Color.Empty)
                {
                    descriptor.AddProperty("errorBackColor", ColorTranslator.ToHtml(ErrorBackColor));
                }
                if (UploadingBackColor != Color.Empty)
                {
                    descriptor.AddProperty("uploadingBackColor", ColorTranslator.ToHtml(UploadingBackColor));
                }
                if (ThrobberID != string.Empty)
                {
                    Control control = this.FindControl(ThrobberID);
                    if (control != null)
                    {
                        descriptor.AddElementProperty("throbber", control.ClientID);
                    }
                }
            }
        }

        protected override Style CreateControlStyle()
        {
            AsyncFileUploadStyle style = new AsyncFileUploadStyle(ViewState);
            return style;
        }

        #endregion

        #region [ AsyncFileUploadStyle ]

        private sealed class AsyncFileUploadStyle : Style
        {
            public AsyncFileUploadStyle(StateBag state)
                : base(state)
            {
            }

            protected override void FillStyleAttributes(CssStyleCollection attributes, IUrlResolutionService urlResolver)
            {
                base.FillStyleAttributes(attributes, urlResolver);

                attributes.Remove(HtmlTextWriterStyle.BackgroundColor);
                attributes.Remove(HtmlTextWriterStyle.Width);
            }
        }

        #endregion
   }
}