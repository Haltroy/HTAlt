using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;

namespace HTAlt
{
    /// <summary>
    /// Haltroy UPDATE class.
    /// </summary>
    public class HTUPDATE
    {

        #region HT Info

        private readonly HTInfo info = new HTInfo();
        private readonly Uri wikiLink = new Uri("https://htalt.haltroy.com/api/HTAlt.Standart/HTUPDATE");
        private readonly Version firstHTAltVersion = new Version("0.1.7.0");
        private readonly string description = "Haltroy UPDATE class.";

        /// <summary>
        /// This control's wiki link.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's wiki link.")]
        public Uri WikiLink => wikiLink;

        /// <summary>
        /// This control's first appearance version for HTAlt.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's first appearance version for HTAlt.")]
        public Version FirstHTAltVersion => firstHTAltVersion;

        /// <summary>
        /// This control's description.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("This control's description.")]
        public string Description => description;

        /// <summary>
        /// Information about this control's project.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("Information about this control's project.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public HTInfo ProjectInfo => info;

        #endregion HT Info

        /// <summary>
        /// Creates a new HTUPDATE from <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> as <seealso cref="string"/></param>
        /// <param name="workFolder"><see cref="string"/> as path to working directory.</param>
        public HTUPDATE(string uri,string workFolder)
        {
            URL = uri;
            WorkFolder = workFolder;
        }
        /// <summary>
        /// Loads HTUPDATE information from <see cref="URL"/>.
        /// </summary>
        public void LoadFromUrl()
        {
            
        }
        public void Update()
        {
            Stopwatch w = new Stopwatch();
            w.Start();
            Log("Starting update...");
            if (OnBeforeUpdate != null)
            {
                Log("OnBeforeUpdate() started...");
                OnBeforeUpdate(this, new EventArgs());
                Log("OnBeforeUpdate() ended...");
            }
            // TODO: Update
            Log("Finishing update...");
            if (OnAfterUpdate != null)
            {
                Log("OnAfterUpdate() started...");
                OnAfterUpdate(this, new EventArgs());
                Log("OnAfterUpdate() ended...");
            }
            Log("Update finished in " + w.ElapsedMilliseconds  + " ms.");
        }
        public void CheckForUpdate()
        {
            Stopwatch w = new Stopwatch();
            w.Start();
            Log("Checking for updates...");
           
            Log("Check finished in " + w.ElapsedMilliseconds + " ms.");
        }

        private void Log(string x, LogEventType type = LogEventType.Nothing)
        {
            if (OnLogEntry != null) { OnLogEntry(this,new OnLogEntryEventArgs(x,type)); }
        }
        /// <summary>
        /// Event raised before updating.
        /// </summary>
        public event EventHandler OnBeforeUpdate;
        /// <summary>
        /// EVent raised after updating.
        /// </summary>
        public event EventHandler OnAfterUpdate;
        /// <summary>
        /// Delegante for <see cref="OnLogEntry"/> event.
        /// </summary>
        /// <param name="sender"><see cref="HTUPDATE"/></param>
        /// <param name="e"></param>
        public delegate void OnLogEntryDelegate(object sender, OnLogEntryEventArgs e);
        public event OnLogEntryDelegate OnLogEntry;
        /// <summary>
        /// <c>true</c> to make all tasks asynchronous, otherwise <c>false</c>.
        /// </summary>
        public bool DoTaskAsAsync { get; set; }
        /// <summary>
        /// The current startus of the HTUPDATE.
        /// </summary>
        public UpdateStatus UpdateStatus { get; set; } = UpdateStatus.Unknown;
        /// <summary>
        /// Returns <c>true</c> if it's currently checking for updates, otherwise <c>false</c>.
        /// </summary>
        public bool isCheckingForUpdates { get; set; }
        /// <summary>
        /// Details of update error.
        /// </summary>
        public Exception UpdateError { get; set; }
        /// <summary>
        /// Codename of the HTUPDATE component.
        /// </summary>
        public string CodeName { get; set; }
        /// <summary>
        /// Folder used for working on updates by HTUPDATE.
        /// </summary>
        public string WorkFolder { get; set; }
        /// <summary>
        /// Location of the HTUPDATE file on Web.
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// Versions of this product that are detected by HTUPDATE.
        /// </summary>
        public HTUPDATE_Version[] Versions { get; set; }
    }
    public class OnLogEntryEventArgs : EventArgs
    {
        public OnLogEntryEventArgs(string newLog, LogEventType type = LogEventType.Nothing)
        {
            Type = type;
            LogEntry = newLog;
        }
        public LogEventType Type { get; internal set; }
        public string LogEntry { get; internal set; }
    }
    public enum LogEventType
    {
        Nothing,
        Info,
        Warning,
        Error
    }
    /// <summary>
    /// Version class for <see cref="HTUPDATE"/>.
    /// </summary>
    public class HTUPDATE_Version
    {
        /// <summary>
        /// Creates a new version with XML node.
        /// </summary>
        /// <param name="node">The node that stores information about this version.</param>
        public HTUPDATE_Version(XmlNode node)
        {
            if (node != null && node.Attributes.Count > 0)
            {
                try
                {
                    if (node.Attributes["Name"] != null && node.Attributes["ID"] != null && node.Attributes["Url"] != null && node.Attributes["Arch"] != null)
                    {
                        Name = node.Attributes["Name"].Value;
                        ID = int.Parse(node.Attributes["ID"].Value);
                        Url = node.Attributes["Url"].Value;
                        var archs = node.Attributes["Arch"].Value.Split(';');
                        for (int i = 0; i < archs.Length; i++)
                        {
                            Archs.Add(archs[i]);
                        }
                        if (node.Attributes["Flags"] != null)
                        {
                            Flags = node.Attributes["Flags"].Value;
                        }
                        if (node.Attributes["LTS"] != null && node.Attributes["LTSRevoke"] != null)
                        {
                            LTS = node.Attributes["LTS"].Value == "true";
                            LTSRevokeDate = node.Attributes["LTSRevoke"].Value;
                        }
                    }
                }catch { }
            }
        }
        /// <summary>
        /// Creates a new epty version.
        /// </summary>
        public HTUPDATE_Version(){}
        /// <summary>
        /// HTUPDATE associated with this version.
        /// </summary>
        public HTUPDATE HTUPDATE { get; set; }
        /// <summary>
        /// Number of the version. This property is important.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Display name of the version. This property is important.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Location of the version. This property is important.
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Flags associated with this version.
        /// </summary>
        public string Flags { get; set; }
        /// <summary>
        /// Determines if this version is supported until revoke time.
        /// </summary>
        public bool LTS { get; set; }
        /// <summary>
        /// Date of this Long Term Supported version's end-of-support. Must be in dd/MM/yyyy format. This propery is only important when LongTerm is true.
        /// </summary>
        public string LTSRevokeDate { get; set; }
        /// <summary>
        /// Processor architectures supported in this version. Can also include environemnt information such as Win-x86. This property is important.
        /// </summary>
        public List<string> Archs { get; set; } = new List<string>();
    }
    /// <summary>
    /// Update status
    /// </summary>
    public enum UpdateStatus
    {
        /// <summary>
        /// HTUPDATE requires an update.
        /// </summary>
        NeedsUpdate,
        /// <summary>
        /// HTUPDATE is up-to-date.
        /// </summary>
        UpToDate,
        /// <summary>
        /// State is unknown, might be checking for updates.
        /// </summary>
        Unknown,
        /// <summary>
        /// Error occurred while checking for updates.
        /// </summary>
        Error
    }
    /// <summary>
    /// Statuses used while updating HTUPDATE.
    /// </summary>
    public enum HTUPDATE_Status
    {
        /// <summary>
        /// 
        /// </summary>
        Stopped,
        GettingInfo,
        DownloadingFile,
        Unzipping,
        Done,
        Error
    }
}
