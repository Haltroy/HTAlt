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
        /// <param name="name">Name of your project.</param>
        /// <param name="uri"><see cref="Uri"/> as <seealso cref="string"/></param>
        /// <param name="workFolder"><see cref="string"/> as path to working directory.</param>
        /// <param name="arch">Current Processor Architecture.</param>
        /// <param name="tempFolder">Temporary folder for temporary files such as downloaded packages.</param>
        /// <param name="version">Current version of your project.</param>
        public HTUPDATE(string name, string uri, string workFolder, string tempFolder, int version, string arch)
        {
            Name = name;
            URL = uri;
            WorkFolder = workFolder;
            CurrentVer = version;
            TempFolder = tempFolder;
            Arch = arch;
        }

        /// <summary>
        /// Loads HTUPDATE information from <see cref="URL"/>.
        /// </summary>
        public void LoadFromUrl()
        {
            if (DoTaskAsAsync)
            {
                LoadUrlAsync();
            }
            else
            {
                LoadUrlSync();
            }
        }

        private async void LoadUrlAsync()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                Log("Loading info from URL (async)...", LogEventType.Info);
                System.Net.WebClient webC = new System.Net.WebClient();
                if (string.IsNullOrWhiteSpace(URL)) { Log("URL was either null, empty or just whitespaces.", LogEventType.Error); return; }
                string htu = string.Empty;
                try
                {
                    htu = webC.DownloadString(URL);
                }
                catch (Exception ex)
                {
                    Log("Cannot get information, exception caught: " + ex.ToString(), LogEventType.Error); return;
                }
                if (string.IsNullOrWhiteSpace(htu)) { Log("The information received was either null, empty or just white spaces. ", LogEventType.Error); return; }
                webC.Dispose();
                webC = null;
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(htu);
                    XmlNode rootNode = doc.FindRoot();
                    List<string> applied = new List<string>();
                    for (int i = 0; i < rootNode.ChildNodes.Count; i++)
                    {
                        XmlNode node = rootNode.ChildNodes[i];
                        switch (node.Name.ToLowerEnglish())
                        {
                            case "mirror":
                                if (applied.Contains(node.Name.ToLowerEnglish()))
                                {
                                    break;
                                }
                                applied.Add(node.Name.ToLowerEnglish());
                                Log("Mirror found.");
                                if (node.Attributes["URL"] == null)
                                {
                                    Log("Mirror link was null.", LogEventType.Error);
                                }
                                else
                                {
                                    URL = node.Attributes["URL"].ToString();
                                    Log("Mirrored to \"" + URL + "\".");
                                    LoadFromUrl();
                                }
                                return;

                            case "version":
                                if (applied.Contains(node.Name.ToLowerEnglish()))
                                {
                                    break;
                                }
                                applied.Add(node.Name.ToLowerEnglish());
                                if (string.IsNullOrWhiteSpace(node.InnerXml))
                                {
                                    Log("Version InnerXML is empty.", LogEventType.Error);
                                    return;
                                }
                                LatestVer = int.Parse(node.InnerXml.XmlToString());
                                break;

                            case "versions":
                                if (applied.Contains(node.Name.ToLowerEnglish()))
                                {
                                    break;
                                }
                                applied.Add(node.Name.ToLowerEnglish());
                                for (int _i = 0; _i < node.ChildNodes.Count; _i++)
                                {
                                    if (node.ChildNodes[_i].Name.ToLowerEnglish() != "version")
                                    {
                                        ThrownNodes.Add(node.ChildNodes[_i]);
                                    }
                                    else
                                    {
                                        HTUPDATE_Version ver = new HTUPDATE_Version(node.ChildNodes[_i]);
                                        if (!Versions.Contains(ver))
                                        {
                                            Versions.Add(ver);
                                        }
                                    }
                                }
                                break;

                            default:
                                if (!node.IsComment())
                                {
                                    ThrownNodes.Add(node);
                                }
                                break;
                        }
                    }
                }
                catch (XmlException xe)
                {
                    Log("XML configuration has error(s): " + xe.ToString(), LogEventType.Error);
                }
                catch (Exception ex)
                {
                    Log("Cannot work on information, exception caught: " + ex.ToString(), LogEventType.Error); return;
                }
            });
        }

        private void LoadUrlSync()
        {
            Log("Loading info from URL (async)...", LogEventType.Info);
            System.Net.WebClient webC = new System.Net.WebClient();
            if (string.IsNullOrWhiteSpace(URL)) { Log("URL was either null, empty or just whitespaces.", LogEventType.Error); return; }
            string htu = string.Empty;
            try
            {
                htu = webC.DownloadString(URL);
            }
            catch (Exception ex)
            {
                Log("Cannot get information, exception caught: " + ex.ToString(), LogEventType.Error); return;
            }
            if (string.IsNullOrWhiteSpace(htu)) { Log("The information received was either null, empty or just white spaces. ", LogEventType.Error); return; }
            webC.Dispose();
            webC = null;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(htu);
                XmlNode rootNode = doc.FindRoot();
                List<string> applied = new List<string>();
                for (int i = 0; i < rootNode.ChildNodes.Count; i++)
                {
                    XmlNode node = rootNode.ChildNodes[i];
                    switch (node.Name.ToLowerEnglish())
                    {
                        case "mirror":
                            if (applied.Contains(node.Name.ToLowerEnglish()))
                            {
                                break;
                            }
                            applied.Add(node.Name.ToLowerEnglish());
                            Log("Mirror found.");
                            if (node.Attributes["URL"] == null)
                            {
                                Log("Mirror link was null.", LogEventType.Error);
                            }
                            else
                            {
                                URL = node.Attributes["URL"].ToString();
                                Log("Mirrored to \"" + URL + "\".");
                                LoadFromUrl();
                            }
                            return;

                        case "version":
                            if (applied.Contains(node.Name.ToLowerEnglish()))
                            {
                                break;
                            }
                            applied.Add(node.Name.ToLowerEnglish());
                            if (string.IsNullOrWhiteSpace(node.InnerXml))
                            {
                                Log("Version InnerXML is empty.", LogEventType.Error);
                                return;
                            }
                            LatestVer = int.Parse(node.InnerXml.XmlToString());
                            break;

                        case "versions":
                            if (applied.Contains(node.Name.ToLowerEnglish()))
                            {
                                break;
                            }
                            applied.Add(node.Name.ToLowerEnglish());
                            for (int _i = 0; _i < node.ChildNodes.Count; _i++)
                            {
                                HTUPDATE_Version ver = new HTUPDATE_Version(node.ChildNodes[_i]);
                                if (!Versions.Contains(ver))
                                {
                                    Versions.Add(ver);
                                }
                            }
                            Versions.Sort((x, y) => x.ID.CompareTo(y.ID));
                            break;

                        default:
                            if (!node.IsComment())
                            {
                                ThrownNodes.Add(node);
                            }
                            break;
                    }
                }
            }
            catch (XmlException xe)
            {
                Log("XML configuration has error(s): " + xe.ToString(), LogEventType.Error);
            }
            catch (Exception ex)
            {
                Log("Cannot work on information, exception caught: " + ex.ToString(), LogEventType.Error); return;
            }
        }

        /// <summary>
        /// Updates the packages
        /// </summary>
        /// <param name="force">Forces to do update even when it is already in the latest version.</param>
        public void Update(bool force = false)
        {
            if (DoTaskAsAsync)
            {
                DoAsyncUpdate();
            }
            else
            {
                DoSyncUpdate();
            }
        }

        private void DoSyncUpdate(bool force = false)
        {
            Stopwatch w = new Stopwatch();
            w.Start();
            Log("Syncing...", LogEventType.Info);
            LoadFromUrl();
            Log("Sync complete. Starting update...", LogEventType.Info);
            if (OnBeforeUpdate != null)
            {
                Log("OnBeforeUpdate() started...", LogEventType.Info);
                OnBeforeUpdate(this, new EventArgs());
                Log("OnBeforeUpdate() ended...", LogEventType.Info);
            }
            if (force || CurrentVer != LatestVer)
            {
                HTUPDATE_Arch download = null;
                List<HTUPDATE_Arch> arch = LatestVersion.FindArch(Arch);
                HTUPDATE_Arch noarch = LatestVersion.FindNoArch();
                if (arch.Count > 0)
                {
                    download = arch[0];
                }
                else
                {
                    if (noarch != null)
                    {
                        download = noarch;
                    }
                    else
                    {
                        download = null;
                    }
                }
                if (download != null)
                {
                    string tempFile = System.IO.Path.Combine(TempFolder, Name + "-" + download.Version.Name + "-" + download.Arch + ".hup");
                    string backupFile = System.IO.Path.Combine(TempFolder, Name + "-backup.hup");
                    System.Net.WebClient webc = new System.Net.WebClient();
                    webc.DownloadProgressChanged += OnPackageDownloadProgress;
                    webc.DownloadFileCompleted += new AsyncCompletedEventHandler((sender, e) =>
                    {
                        if (e.Error != null)
                        {
                            Log("Cannot download package, error caught: " + e.Error.ToString());
                        }
                        else if (e.Cancelled)
                        {
                            Log("Cannot download package, cancelled.");
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(download.MD5Hash) || !string.IsNullOrWhiteSpace(download.SHA256Hash))
                            {
                                string stat = "";
                                Log("Verifying...");
                                if (!string.IsNullOrWhiteSpace(download.MD5Hash))
                                {
                                    stat += tempFile.VerifyFile(download.MD5Hash) ? "m" : "M";
                                }
                                if (!string.IsNullOrWhiteSpace(download.SHA256Hash))
                                {
                                    stat += tempFile.VerifyFile(download.SHA256Hash, true) ? "s" : "S";
                                }
                                if (stat == "m" || stat == "s" || stat == "ms" || stat == "sm")
                                {
                                    Log("Verified.");
                                }
                                else
                                {
                                    Log("Cannot verify package, different hash.");
                                    return;
                                }
                            }
                            try
                            {
                                Log("Getting a backup...");
                                System.IO.Compression.ZipFile.CreateFromDirectory(WorkFolder, backupFile, System.IO.Compression.CompressionLevel.NoCompression, false, System.Text.Encoding.UTF8);
                                Log("Backup created. Installing...");
                                try
                                {
                                    System.IO.Compression.ZipFile.ExtractToDirectory(WorkFolder, tempFile, System.Text.Encoding.UTF8);
                                    Log("Installation finished with no error(s).", LogEventType.Info);
                                    CurrentVer = LatestVer;
                                }
                                catch (Exception _ex)
                                {
                                    Log("Error while installing, error caught: " + _ex.ToString(), LogEventType.Info);
                                    System.IO.Compression.ZipFile.ExtractToDirectory(WorkFolder, backupFile, System.Text.Encoding.UTF8);
                                    Log("Reverted back to previous state.", LogEventType.Info);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log("Error while installing, error caught: " + ex.ToString(), LogEventType.Info);
                            }

                            Log("Finishing update...", LogEventType.Info);
                            if (OnAfterUpdate != null)
                            {
                                Log("OnAfterUpdate() started...", LogEventType.Info);
                                OnAfterUpdate(this, new EventArgs());
                                Log("OnAfterUpdate() ended...", LogEventType.Info);
                            }
                            Log("Update finished in " + w.ElapsedMilliseconds + " ms.", LogEventType.Info);
                        }
                    });
                    webc.DownloadFileAsync(new Uri(download.DownlaodUrl), tempFile);
                }
                else
                {
                    Log("Cannot find suitable download for this version. Architectures are missing.", LogEventType.Info);
                }
            }
            else
            {
                Log("Package already up-to-date.", LogEventType.Info);
            }
        }

        /// <summary>
        /// 0 = No updates 1 = Normal Update 2 = LTS Update
        /// </summary>
        private int GetUpdateType => CurrentVersion.LTS
                    ? CurrentVer < LatestLTSVer ? isLTSRevoked(LatestLTSVersion) ? 1 : 2 : isLTSRevoked(CurrentVersion) ? 1 : 0
                    : CurrentVer < LatestVer ? 1 : 0;

        private async void DoAsyncUpdate(bool force = false)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                Stopwatch w = new Stopwatch();
                w.Start();
                Log("Syncing...", LogEventType.Info);
                LoadFromUrl();
                Log("Sync complete. Starting update...", LogEventType.Info);
                if (OnBeforeUpdate != null)
                {
                    Log("OnBeforeUpdate() started...", LogEventType.Info);
                    OnBeforeUpdate(this, new EventArgs());
                    Log("OnBeforeUpdate() ended...", LogEventType.Info);
                }
                int uType = GetUpdateType;
                if (force || uType > 0)
                {
                    HTUPDATE_Arch download = null;
                    List<HTUPDATE_Arch> arch = uType > 1 ? LatestLTSVersion.FindArch(Arch) : LatestVersion.FindArch(Arch);
                    HTUPDATE_Arch noarch = uType > 1 ? LatestLTSVersion.FindNoArch() : LatestVersion.FindNoArch();
                    if (arch.Count > 0)
                    {
                        download = arch[0];
                    }
                    else
                    {
                        if (noarch != null)
                        {
                            download = noarch;
                        }
                        else
                        {
                            download = null;
                        }
                    }
                    if (download != null)
                    {
                        string tempFile = System.IO.Path.Combine(TempFolder, Name + "-" + download.Version.Name + "-" + download.Arch + ".hup");
                        string backupFile = System.IO.Path.Combine(TempFolder, Name + "-backup.hup");
                        System.Net.WebClient webc = new System.Net.WebClient();
                        webc.DownloadProgressChanged += OnPackageDownloadProgress;
                        webc.DownloadFileCompleted += new AsyncCompletedEventHandler((sender, e) =>
                        {
                            if (e.Error != null)
                            {
                                Log("Cannot download package, error caught: " + e.Error.ToString());
                            }
                            else if (e.Cancelled)
                            {
                                Log("Cannot download package, cancelled.");
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(download.MD5Hash) || !string.IsNullOrWhiteSpace(download.SHA256Hash))
                                {
                                    string stat = "";
                                    Log("Verifying...");
                                    if (!string.IsNullOrWhiteSpace(download.MD5Hash))
                                    {
                                        stat += tempFile.VerifyFile(download.MD5Hash) ? "m" : "M";
                                    }
                                    if (!string.IsNullOrWhiteSpace(download.SHA256Hash))
                                    {
                                        stat += tempFile.VerifyFile(download.SHA256Hash, true) ? "s" : "S";
                                    }
                                    if (stat == "m" || stat == "s" || stat == "ms" || stat == "sm")
                                    {
                                        Log("Verified.");
                                    }
                                    else
                                    {
                                        Log("Cannot verify package, different hash.");
                                        return;
                                    }
                                }
                                try
                                {
                                    Log("Getting a backup...");
                                    System.IO.Compression.ZipFile.CreateFromDirectory(WorkFolder, backupFile, System.IO.Compression.CompressionLevel.NoCompression, false, System.Text.Encoding.UTF8);
                                    Log("Backup created. Installing...");
                                    try
                                    {
                                        System.IO.Compression.ZipFile.ExtractToDirectory(WorkFolder, tempFile, System.Text.Encoding.UTF8);
                                        Log("Installation finished with no error(s).", LogEventType.Info);
                                        CurrentVer = LatestVer;
                                    }
                                    catch (Exception _ex)
                                    {
                                        Log("Error while installing, error caught: " + _ex.ToString(), LogEventType.Info);
                                        System.IO.Compression.ZipFile.ExtractToDirectory(WorkFolder, backupFile, System.Text.Encoding.UTF8);
                                        Log("Reverted back to previous state.", LogEventType.Info);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log("Error while installing, error caught: " + ex.ToString(), LogEventType.Info);
                                }

                                Log("Finishing update...", LogEventType.Info);
                                if (OnAfterUpdate != null)
                                {
                                    Log("OnAfterUpdate() started...", LogEventType.Info);
                                    OnAfterUpdate(this, new EventArgs());
                                    Log("OnAfterUpdate() ended...", LogEventType.Info);
                                }
                                Log("Update finished in " + w.ElapsedMilliseconds + " ms.", LogEventType.Info);
                            }
                        });
                        webc.DownloadFileAsync(new Uri(download.DownlaodUrl), tempFile);
                    }
                    else
                    {
                        Log("Cannot find suitable download for this version. Architectures are missing.", LogEventType.Info);
                    }
                }
                else
                {
                    Log("Package already up-to-date.", LogEventType.Info);
                }
            });
        }

        public void CheckForUpdate()
        {
            Stopwatch w = new Stopwatch();
            w.Start();
            Log("Checking for updates...", LogEventType.Info);
            // TODO
            Log("Check finished in " + w.ElapsedMilliseconds + " ms.", LogEventType.Info);
        }

        private void Log(string x, LogEventType type = LogEventType.Nothing)
        {
            if (OnLogEntry != null) { OnLogEntry(this, new OnLogEntryEventArgs(x, type)); }
        }

        private int LatestVer { get; set; } = 1;
        private int LatestLTSVer => LatestLTSVersion.ID;
        private int CurrentVer { get; set; } = 1;

        /// <summary>
        /// Folder where HTUPDATE store temporary files such as downloaded packages.
        /// </summary>
        public string TempFolder { get; private set; }

        /// <summary>
        /// Current processor architecture. This architecture is going to be picked over "noarch". If this architecture doesn't exists, "noarch" packages will be installed.
        /// </summary>
        public string Arch { get; private set; }

        /// <summary>
        /// A list of nodes that are thrown away while gathering information.
        /// </summary>
        public List<XmlNode> ThrownNodes { get; set; } = new List<XmlNode>();

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

        public event System.Net.DownloadProgressChangedEventHandler OnPackageDownloadProgress;

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
        public List<HTUPDATE_Version> Versions { get; set; } = new List<HTUPDATE_Version>();

        /// <summary>
        /// Gets the current version of this product.
        /// </summary>
        public HTUPDATE_Version CurrentVersion => Versions.FindAll(it => it.ID == CurrentVer).Count > 0 ? Versions.FindAll(it => it.ID == CurrentVer)[0] : null;

        /// <summary>
        /// Gets the current version of this product.
        /// </summary>
        public HTUPDATE_Version LatestVersion => Versions.FindAll(it => it.ID == LatestVer).Count > 0 ? Versions.FindAll(it => it.ID == LatestVer)[0] : null;

        /// <summary>
        /// Gets a list of LTS versions.
        /// </summary>
        public List<HTUPDATE_Version> LTSVersions => Versions.FindAll(it => it.LTS);

        /// <summary>
        /// Gets the latest LTS version.
        /// </summary>

        public HTUPDATE_Version LatestLTSVersion => LTSVersions.Count > 0 ? LTSVersions[LTSVersions.Count - 1] : null;
        /// <summary>
        /// Checks if the <paramref name="ver"/> is revoked.
        /// </summary>
        /// <param name="ver"><see cref="HTUPDATE_Version"/></param>
        /// <returns><see cref="bool"/></returns>

        public bool isLTSRevoked(HTUPDATE_Version ver)
        {
            return DateTime.Now.CompareTo(DateTime.ParseExact(ver.LTSRevokeDate, "yyyy-MM-dd", null)) > 0;
        }

        /// <summary>
        /// Name of your product.
        /// </summary>
        public string Name { get; set; }
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
        public HTUPDATE_Version(XmlNode vernode)
        {
            if (vernode != null && vernode.ChildNodes.Count > 0)
            {
                try
                {
                    List<string> applied = new List<string>();
                    for (int i = 0; i < vernode.ChildNodes.Count; i++)
                    {
                        XmlNode node = vernode.ChildNodes[i];
                        if (!applied.Contains(node.Name.ToLowerEnglish()))
                        {
                            applied.Add(node.Name.ToLowerEnglish());
                            switch (node.Name.ToLowerEnglish())
                            {
                                case "name":
                                    Name = node.InnerXml.XmlToString();
                                    break;

                                case "id":
                                    ID = int.Parse(node.InnerXml.XmlToString());
                                    break;

                                case "flags":
                                    Flags = node.InnerXml.XmlToString().Split(';');
                                    break;

                                case "archs":
                                    for (int _i = 0; i < node.ChildNodes.Count; _i++)
                                    {
                                        XmlNode subnode = node.ChildNodes[_i];
                                        if (subnode.Name.ToLowerEnglish() == "arch")
                                        {
                                            if (subnode.Attributes["Arch"] != null && subnode.Attributes["Url"] != null)
                                            {
                                                HTUPDATE_Arch arch = new HTUPDATE_Arch(subnode.Attributes["Arch"].Value.XmlToString().ToLowerEnglish(), subnode.Attributes["Url"].Value.XmlToString(), this);
                                                if (subnode.Attributes["MD5"] != null)
                                                {
                                                    arch.MD5Hash = subnode.Attributes["MD5"].Value.XmlToString();
                                                }
                                                if (subnode.Attributes["SHA256"] != null)
                                                {
                                                    arch.SHA256Hash = subnode.Attributes["SHA256"].Value.XmlToString();
                                                }
                                                Archs.Add(arch);
                                            }
                                        }
                                    }
                                    break;

                                case "lts":
                                    LTS = node.InnerXml.XmlToString() == "true";
                                    break;

                                case "ltsrevokedate":
                                    LTSRevokeDate = node.InnerXml.XmlToString();
                                    break;
                            }
                        }
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Creates a new epty version.
        /// </summary>
        public HTUPDATE_Version() { }

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
        public string[] Flags { get; set; }

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
        public List<HTUPDATE_Arch> Archs { get; set; } = new List<HTUPDATE_Arch>();

        /// <summary>
        /// Finds an architecture.
        /// </summary>
        /// <param name="arch">Processor Architecture.</param>
        /// <returns>A list of <see cref="HTUPDATE_Arch"/></returns>
        public List<HTUPDATE_Arch> FindArch(string arch)
        {
            return Archs.FindAll(it => it.Arch.ToLowerEnglish() == arch.ToLowerEnglish());
        }

        /// <summary>
        /// Finds a package for no-architecture.
        /// </summary>
        /// <returns><see cref="HTUPDATE_Arch"/></returns>
        public HTUPDATE_Arch FindNoArch()
        {
            return Archs.FindAll(it => it.Arch.ToLowerEnglish() == "noarch").Count > 0 ? Archs.FindAll(it => it.Arch.ToLowerEnglish() == "noarch")[0] : null;
        }
    }

    /// <summary>
    /// HTUPDATE VErsion Architecture.
    /// </summary>
    public class HTUPDATE_Arch
    {
        /// <summary>
        /// Creates a new <see cref="HTUPDATE_Arch"/>.
        /// </summary>
        /// <param name="arch">Processor architecture.</param>
        /// <param name="downlaodUrl">Location of the package.</param>
        /// <param name="ver"><see cref="HTUPDATE_Version"/></param>
        /// <param name="mD5Hash"><see cref="System.Security.Cryptography.MD5"/></param>
        /// <param name="sHA256Hash"><see cref="System.Security.Cryptography.SHA256"/></param>
        public HTUPDATE_Arch(string arch, string downlaodUrl, HTUPDATE_Version ver, string mD5Hash = "", string sHA256Hash = "")
        {
            Arch = arch;
            Version = ver;
            DownlaodUrl = downlaodUrl;
            MD5Hash = mD5Hash;
            SHA256Hash = sHA256Hash;
        }

        /// <summary>
        /// Creates a new <see cref="HTUPDATE_Arch"/>.
        /// </summary>
        public HTUPDATE_Arch()
        {
        }

        /// <summary>
        /// Version of this architecture.
        /// </summary>
        public HTUPDATE_Version Version { get; set; }

        /// <summary>
        /// Name of the architecture.
        /// </summary>
        public string Arch { get; set; }

        /// <summary>
        /// Download location for the files
        /// </summary>
        public string DownlaodUrl { get; set; }

        /// <summary>
        /// <see cref="System.Security.Cryptography.MD5"/>
        /// </summary>
        public string MD5Hash { get; set; }

        /// <summary>
        /// <see cref="System.Security.Cryptography.SHA256"/>
        /// </summary>
        public string SHA256Hash { get; set; }

        public bool isCompatible()
        {
            Arch = Arch.ToLowerEnglish();
            if (Arch == "noarch")
            {
                return true;
            }
            else
            {
                if (Arch == "arm" && (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture == System.Runtime.InteropServices.Architecture.Arm || System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture == System.Runtime.InteropServices.Architecture.Arm64))
                {
                    return true;
                }
                if (Arch == "arm64" && System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture == System.Runtime.InteropServices.Architecture.Arm64)
                {
                    return true;
                }
                if (Arch == "x86" && (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture == System.Runtime.InteropServices.Architecture.X86 || System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture == System.Runtime.InteropServices.Architecture.X64))
                {
                    return true;
                }
                if (Arch == "x86" && System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture == System.Runtime.InteropServices.Architecture.X64)
                {
                    return true;
                }
                return false;
            }
        }
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
}