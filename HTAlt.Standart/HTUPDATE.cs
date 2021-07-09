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

        /// <summary>
        /// Loads URL without sync.
        /// </summary>
        public async void LoadUrlAsync()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                Log("Loading info from URL (async)...", LogLevel.Info);
                LoadUrlSync();
            });
        }

        /// <summary>
        /// Loads URL with sync.
        /// </summary>
        public void LoadUrlSync()
        {
            Log("Loading info from URL...", LogLevel.Info);
            System.Net.WebClient webC = new System.Net.WebClient();
            if (string.IsNullOrWhiteSpace(URL)) { Log("URL was either null, empty or just whitespaces.", LogLevel.Error); return; }
            string htu = string.Empty;
            try
            {
                htu = webC.DownloadString(URL);
            }
            catch (Exception ex)
            {
                Log("Cannot get information, exception caught: " + ex.ToString(), LogLevel.Error); return;
            }
            if (string.IsNullOrWhiteSpace(htu)) { Log("The information received was either null, empty or just white spaces. ", LogLevel.Error); return; }
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
                                Log("Mirror link was null.", LogLevel.Error);
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
                                Log("Version InnerXML is empty.", LogLevel.Error);
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
                Log("XML configuration has error(s): " + xe.ToString(), LogLevel.Error);
            }
            catch (Exception ex)
            {
                Log("Cannot work on information, exception caught: " + ex.ToString(), LogLevel.Error); return;
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

        /// <summary>
        /// Updates the packages with sync.
        /// </summary>
        /// <param name="force">Forces to do update even when it is already in the latest version.</param>
        public void DoSyncUpdate(bool force = false)
        {
            Stopwatch w = new Stopwatch();
            w.Start();
            Log("Syncing...", LogLevel.Info);
            LoadFromUrl();
            Log("Sync complete. Starting update...", LogLevel.Info);
            if (OnBeforeUpdate != null)
            {
                Log("OnBeforeUpdate() started...", LogLevel.Info);
                OnBeforeUpdate(this, new EventArgs());
                Log("OnBeforeUpdate() ended...", LogLevel.Info);
            }
            int uType = GetUpdateType;
            if (force || uType > 0)
            {
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
                            Exception error = null;
                            if (download.Hashes.Count > 0)
                            {
                                for (int i = 0; i < download.Hashes.Count; i++)
                                {
                                    try
                                    {
                                        download.Hashes[i].Verify(tempFile);
                                    }
                                    catch (Exception ex)
                                    {
                                        error = ex;
                                        break;
                                    }
                                }
                            }
                            if (error != null)
                            {
                                Log("Error on verifying package \"" + System.IO.Path.GetFileName(tempFile) + "\" exception caught: " + error.ToString(), LogLevel.Error);
                            }
                            else
                            {
                                try
                                {
                                    Log("Getting a backup...");
                                    System.IO.Compression.ZipFile.CreateFromDirectory(WorkFolder, backupFile, System.IO.Compression.CompressionLevel.NoCompression, false, System.Text.Encoding.UTF8);
                                    Log("Backup created. Installing...");
                                    try
                                    {
                                        System.IO.Compression.ZipFile.ExtractToDirectory(WorkFolder, tempFile, System.Text.Encoding.UTF8);
                                        Log("Installation finished with no error(s).", LogLevel.Info);
                                        CurrentVer = LatestVer;
                                    }
                                    catch (Exception _ex)
                                    {
                                        Log("Error while installing, error caught: " + _ex.ToString(), LogLevel.Info);
                                        System.IO.Compression.ZipFile.ExtractToDirectory(WorkFolder, backupFile, System.Text.Encoding.UTF8);
                                        Log("Reverted back to previous state.", LogLevel.Info);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log("Error while installing, error caught: " + ex.ToString(), LogLevel.Info);
                                }

                                Log("Finishing update...", LogLevel.Info);
                                if (OnAfterUpdate != null)
                                {
                                    Log("OnAfterUpdate() started...", LogLevel.Info);
                                    OnAfterUpdate(this, new EventArgs());
                                    Log("OnAfterUpdate() ended...", LogLevel.Info);
                                }
                                Log("Update finished in " + w.ElapsedMilliseconds + " ms.", LogLevel.Info);
                            }
                        }
                    });
                    webc.DownloadFileAsync(new Uri(download.Url), tempFile);
                }
                else
                {
                    Log("Cannot find suitable download for this version. Architectures are missing.", LogLevel.Info);
                }
            }
            else
            {
                Log("Package already up-to-date.", LogLevel.Info);
            }
        }

        /// <summary>
        /// 0 = No updates 1 = Normal Update 2 = LTS Update
        /// </summary>
        private int GetUpdateType => CurrentVersion.LTS
                    ? CurrentVer < LatestLTSVer ? isLTSRevoked(LatestLTSVersion) ? 1 : 2 : isLTSRevoked(CurrentVersion) ? 1 : 0
                    : CurrentVer < LatestVer ? 1 : 0;

        /// <summary>
        /// Updates the packages without sync.
        /// </summary>
        /// <param name="force">Forces to do update even when it is already in the latest version.</param>
        public async void DoAsyncUpdate(bool force = false)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                DoSyncUpdate();
            });
        }

        /// <summary>
        /// Event used for logging.
        /// </summary>
        /// <param name="x">Message</param>
        /// <param name="level"><see cref="LogLevel"/></param>
        public void Log(string x, LogLevel level = LogLevel.None)
        {
            if (OnLogEntry != null) { OnLogEntry(this, new OnLogEntryEventArgs(x, level)); }
        }

        /// <summary>
        /// Latest Version number.
        /// </summary>
        public int LatestVer { get; set; } = 1;

        /// <summary>
        /// Latest LTS version number.
        /// </summary>
        public int LatestLTSVer => LatestLTSVersion.ID;

        /// <summary>
        /// Current version number.
        /// </summary>
        public int CurrentVer { get; set; } = 1;

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
        /// Returns <c>true</c> if it's currently checking for updates, otherwise <c>false</c>.
        /// </summary>
        public bool isCheckingForUpdates { get; set; }

        /// <summary>
        /// Checks if the packages are up to date.
        /// </summary>
        public bool isUpToDate => LatestLTSVer == CurrentVer || LatestVer == CurrentVer;

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

        /// <summary>
        /// Gets version by its number.
        /// </summary>
        /// <param name="v">Version number.</param>
        /// <returns><see cref="HTUPDATE_Version"/></returns>
        public HTUPDATE_Version GetVersion(int v)
        {
            HTUPDATE_Version ver = null;
            for (int i = 0; i < Versions.Count; i++)
            {
                if (Versions[i].ID == v)
                {
                    ver = Versions[i];
                    break;
                }
            }
            return ver;
        }
    }

    public class OnLogEntryEventArgs : EventArgs
    {
        public OnLogEntryEventArgs(string newLog, LogLevel level = LogLevel.None)
        {
            Level = level;
            LogEntry = newLog;
        }

        public LogLevel Level { get; internal set; }
        public string LogEntry { get; internal set; }
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

                                case "based":
                                    BasedVersion = HTUPDATE.GetVersion(int.Parse(node.InnerXml.XmlToString()));
                                    break;

                                case "archs":
                                    for (int _i = 0; i < node.ChildNodes.Count; _i++)
                                    {
                                        XmlNode subnode = node.ChildNodes[_i];
                                        if (subnode.Name.ToLowerEnglish() == "arch")
                                        {
                                            HTUPDATE_Arch arch = new HTUPDATE_Arch(this);
                                            for (int ai = 0; ai < subnode.ChildNodes.Count; ai++)
                                            {
                                                XmlNode subsubnode = subnode.ChildNodes[ai];
                                                switch (subsubnode.Name.ToLowerEnglish())
                                                {
                                                    case "hash":
                                                        if (subsubnode.Attributes["Algorithm"] != null && !string.IsNullOrWhiteSpace(subsubnode.InnerXml))
                                                        {
                                                            HTUPDATE_Hash hash = new HTUPDATE_Hash
                                                            {
                                                                Hash = subsubnode.InnerXml.XmlToString()
                                                            };
                                                            switch (subsubnode.Attributes["Algorithm"].Value.XmlToString().ToLowerEnglish())
                                                            {
                                                                case "md5": hash.Algorithm = System.Security.Cryptography.MD5.Create(); break;
                                                                case "sha256": hash.Algorithm = System.Security.Cryptography.SHA256.Create(); break;
                                                                case "sha1": hash.Algorithm = System.Security.Cryptography.SHA1.Create(); break;
                                                                case "sha384": hash.Algorithm = System.Security.Cryptography.SHA384.Create(); break;
                                                                case "SHA512": hash.Algorithm = System.Security.Cryptography.SHA512.Create(); break;
                                                                case "hmacmd5": hash.Algorithm = System.Security.Cryptography.HMACMD5.Create(); break;
                                                                case "hmacsha1": hash.Algorithm = System.Security.Cryptography.HMACSHA1.Create(); break;
                                                                case "hmacsha256": hash.Algorithm = System.Security.Cryptography.HMACSHA256.Create(); break;
                                                                case "hmacsha384": hash.Algorithm = System.Security.Cryptography.HMACSHA384.Create(); break;
                                                                case "hmacsha512": hash.Algorithm = System.Security.Cryptography.HMACSHA512.Create(); break;
                                                            }
                                                            arch.Hashes.Add(hash);
                                                        }
                                                        break;

                                                    case "arch":

                                                        arch.Arch = subsubnode.Value.XmlToString();
                                                        break;

                                                    case "delta":

                                                        arch.isDelta = subsubnode.Value.XmlToString() == "true";
                                                        break;

                                                    case "url":

                                                        arch.Url = subsubnode.InnerXml.XmlToString();
                                                        break;
                                                }
                                            }
                                            Archs.Add(arch);
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
        /// The version which this version is based on.
        /// </summary>
        public HTUPDATE_Version BasedVersion { get; set; }

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
        /// <param name="version"><see cref="HTUPDATE_Version"/></param>
        public HTUPDATE_Arch(HTUPDATE_Version version) : this()
        {
            if (version != null) { Version = version; } else { throw new ArgumentNullException(nameof(version)); }
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
        public string Url { get; set; }

        /// <summary>
        /// Determines if this package hs binary changes or not.
        /// </summary>
        public bool isDelta { get; set; }

        /// <summary>
        /// A list of file hashes.
        /// </summary>
        public List<HTUPDATE_Hash> Hashes { get; set; } = new List<HTUPDATE_Hash>();

        /// <summary>
        /// Checks if a version is compatible or not.
        /// </summary>
        /// <returns><see cref="bool"/></returns>
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
    /// File Hash.
    /// </summary>
    public class HTUPDATE_Hash
    {
        /// <summary>
        /// Hash of the file.
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Algorithm of the hash.
        /// </summary>
        public System.Security.Cryptography.HashAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Verifys the file.
        /// </summary>
        /// <param name="file"><see cref="string"/></param>
        /// <returns><see cref="bool"/></returns>
        public bool Verify(string file)
        {
            return Tools.VerifyFile(Algorithm, file, Hash);
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

    public static class HTUPDATE_Default_OS
    {
        public class Any : HTUPDATE_OS
        {
            public override string Name => "Any OS";
            public override string Version => "";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64", "arm", "arm64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return true;
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return true;
            }
        }

        #region Unix

        public class Unix : Any
        {
            public override string Name => "Unix";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64", "arm", "arm64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is Unix;
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return true;
            }
        }

        public class BSD : Unix
        {
            public override string Name => "BSD";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64", "arm", "arm64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is BSD;
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return true;
            }
        }

        public class FreeBSD : BSD
        {
            public override string Name => "FreeBSD";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64", "arm", "arm64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is FreeBSD;
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return true;
            }
        }

        public class macOS : FreeBSD
        {
            public override string Name => "macOS";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64", "arm", "arm64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is macOS && isCompatibleWithVersion(otherOS.Version);
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) > int.Parse(version.Substring(0, version.IndexOf('.') - 1))) || (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) < int.Parse(version.Substring(0, version.IndexOf('.') - 1))) ? false : int.Parse((base.Version.Substring(base.Version.IndexOf('.') - 1)).Substring(0, base.Version.Substring(base.Version.IndexOf('.') - 1).IndexOf('.') - 1)) >= int.Parse((version.Substring(version.IndexOf('.') - 1)).Substring(0, version.Substring(version.IndexOf('.') - 1).IndexOf('.') - 1));
            }
        }

        #region GNU/Linux

        public class Linux : Unix
        {
            public override string Name => "Linux";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64", "arm", "arm64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is Linux && isCompatibleWithVersion(otherOS.Version);
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) > int.Parse(version.Substring(0, version.IndexOf('.') - 1))) || (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) < int.Parse(version.Substring(0, version.IndexOf('.') - 1))) ? false : int.Parse((base.Version.Substring(base.Version.IndexOf('.') - 1)).Substring(0, base.Version.Substring(base.Version.IndexOf('.') - 1).IndexOf('.') - 1)) >= int.Parse((version.Substring(version.IndexOf('.') - 1)).Substring(0, version.Substring(version.IndexOf('.') - 1).IndexOf('.') - 1));
            }
        }

        #region Arch Family

        public class Arch : Linux
        {
            public override string Name => "Arch Linux";
            public override string[] SupportedArchs => new string[] { "noarch", "x64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is Arch;
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return true;
            }
        }

        public class Manjaro : Arch
        {
            public override string Name => "Manjaro";
            public override string[] SupportedArchs => new string[] { "noarch", "x64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is Arch;
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) > int.Parse(version.Substring(0, version.IndexOf('.') - 1))) || (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) < int.Parse(version.Substring(0, version.IndexOf('.') - 1))) ? false : int.Parse((base.Version.Substring(base.Version.IndexOf('.') - 1)).Substring(0, base.Version.Substring(base.Version.IndexOf('.') - 1).IndexOf('.') - 1)) >= int.Parse((version.Substring(version.IndexOf('.') - 1)).Substring(0, version.Substring(version.IndexOf('.') - 1).IndexOf('.') - 1));
            }
        }

        public class Garuda : Manjaro
        {
            public override string Name => "Garuda Linux";
            public override string[] SupportedArchs => new string[] { "noarch", "x64" };
        }

        #endregion Arch Family

        #region Red Hat Family

        public class Fedora : Linux
        {
            public override string Name => "Fedora";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64", "arm", "arm64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is Fedora && isCompatibleWithVersion(otherOS.Version);
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) <= int.Parse(version.Substring(0, version.IndexOf('.') - 1)) && int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) >= int.Parse(version.Substring(0, version.IndexOf('.') - 1));
            }
        }

        public class CentOS : Fedora
        {
            public override string Name => "CentOS";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64", "arm", "arm64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is Fedora && isCompatibleWithVersion(otherOS.Version);
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) > int.Parse(version.Substring(0, version.IndexOf('.') - 1))) || (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) < int.Parse(version.Substring(0, version.IndexOf('.') - 1))) ? false : int.Parse((base.Version.Substring(base.Version.IndexOf('.') - 1)).Substring(0, base.Version.Substring(base.Version.IndexOf('.') - 1).IndexOf('.') - 1)) >= int.Parse((version.Substring(version.IndexOf('.') - 1)).Substring(0, version.Substring(version.IndexOf('.') - 1).IndexOf('.') - 1));
            }
        }

        public class RHEL : Fedora
        {
            public override string Name => "Red hat Enterprise Linux";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64", "arm", "arm64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is Fedora && isCompatibleWithVersion(otherOS.Version);
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) > int.Parse(version.Substring(0, version.IndexOf('.') - 1))) || (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) < int.Parse(version.Substring(0, version.IndexOf('.') - 1))) ? false : int.Parse((base.Version.Substring(base.Version.IndexOf('.') - 1)).Substring(0, base.Version.Substring(base.Version.IndexOf('.') - 1).IndexOf('.') - 1)) >= int.Parse((version.Substring(version.IndexOf('.') - 1)).Substring(0, version.Substring(version.IndexOf('.') - 1).IndexOf('.') - 1));
            }
        }

        #endregion Red Hat Family

        #region Debian Family

        public class Debian : Linux // DO NOT INCLUDE CODENAMES IN VERSION SUCH AS buster, sid etc.
        {
            public override string Name => "Debian";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64", "arm", "arm64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is Fedora && isCompatibleWithVersion(otherOS.Version);
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) > int.Parse(version.Substring(0, version.IndexOf('.') - 1))) || (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) < int.Parse(version.Substring(0, version.IndexOf('.') - 1))) ? false : int.Parse((base.Version.Substring(base.Version.IndexOf('.') - 1)).Substring(0, base.Version.Substring(base.Version.IndexOf('.') - 1).IndexOf('.') - 1)) >= int.Parse((version.Substring(version.IndexOf('.') - 1)).Substring(0, version.Substring(version.IndexOf('.') - 1).IndexOf('.') - 1));
            }
        }

        public class Ubuntu : Debian
        {
            public override string Name => "Ubuntu";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64", "arm", "arm64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is Fedora && isCompatibleWithVersion(otherOS.Version);
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) > int.Parse(version.Substring(0, version.IndexOf('.') - 1))) || (int.Parse(base.Version.Substring(0, base.Version.IndexOf('.') - 1)) < int.Parse(version.Substring(0, version.IndexOf('.') - 1))) ? false : int.Parse((base.Version.Substring(base.Version.IndexOf('.') - 1)).Substring(0, base.Version.Substring(base.Version.IndexOf('.') - 1).IndexOf('.') - 1)) >= int.Parse((version.Substring(version.IndexOf('.') - 1)).Substring(0, version.Substring(version.IndexOf('.') - 1).IndexOf('.') - 1));
            }
        }

        public class


        #endregion Debian Family

        #endregion GNU/Linux

        #endregion Unix

        #region Windows

        public class Windows : Any
        {
            public override string Name => "Microsoft Windows";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is Windows;
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return true;
            }
        }

        public class Windows7 : Windows
        {
            public override string Name => "Microsoft Windows 7";
            public override string Version => "6.1";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is Windows;
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return true;
            }
        }

        public class Windows7SP1 : Windows
        {
            public override string Name => "Microsoft Windows 7 SP1";
            public override string Version => "6.1sp1";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                return otherOS is Windows;
            }

            public override bool isCompatibleWithVersion(string version)
            {
                return true;
            }
        }

        public class Windows8 : Windows
        {
            public override string Name => "Microsoft Windows 8";
            public override string Version => "6.2";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64", "arm", "arm64" };

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                if (otherOS is Windows)
                {
                    return isCompatibleWithVersion(otherOS.Version);
                }
                else
                {
                    return false;
                }
            }

            public override bool isCompatibleWithVersion(string version)
            {
                if (version.Length > 2)
                {
                    string ver_mn = version.Substring(2);
                    if (version.StartsWith("6"))
                    {
                        return !ver_mn.StartsWith("0") && !ver_mn.StartsWith("1");
                    }
                    else if (version.StartsWith("7"))
                    {
                        return false;
                    }
                    else if (version.StartsWith("8") || version.StartsWith("10") || version.StartsWith("11"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return version.StartsWith("10") || version.StartsWith("6") || version.StartsWith("7") || version.StartsWith("8") || version.StartsWith("8.1") || version.StartsWith("11");
                }
            }
        }

        public class Windows81 : Windows8
        {
            public override string Name => "Microsoft Windows 8.1";
            public override string Version => "6.2.9200.00";
        }

        public class Windows10 : Windows
        {
            public override string Name => "Microsoft Windows 10";
            public override string[] SupportedArchs => new string[] { "noarch", "x86", "x64", "arm", "arm64" };

            public string UpdateVersion { get; set; }
            public override string Version { get => "10." + UpdateVersion; set => UpdateVersion = value.StartsWith("10.") ? value.Substring(3) : value; }

            public override bool isCompatible(HTUPDATE_OS otherOS)
            {
                if (otherOS is Windows)
                {
                    return isCompatibleWithVersion(otherOS.Version);
                }
                else
                {
                    return false;
                }
            }

            public override bool isCompatibleWithVersion(string version)
            {
                if (version.Length > 2)
                {
                    string ver_mn = version.Substring(3);
                    if (version.StartsWith("10"))
                    {
                        return int.Parse(ver_mn.Replace("H", "0")) >= int.Parse(UpdateVersion.Replace("H", "0"));
                    }
                    else if (version.StartsWith("7") || version.StartsWith("6"))
                    {
                        return false;
                    }
                    else if (version.StartsWith("11"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return version.StartsWith("10") || version.StartsWith("6") || version.StartsWith("7") || version.StartsWith("8") || version.StartsWith("8.1") || version.StartsWith("11");
                }
            }
        }

        #endregion Windows
    }

    public class HTUPDATE_OS
    {
        public virtual string Name { get; set; }
        public virtual string Version { get; set; }
        public virtual string[] SupportedArchs { get; set; } = new string[] { "noarch" };

        public virtual bool isCompatible(HTUPDATE_OS otherOS)
        {
            return otherOS.Name == Name;
        }

        public virtual bool isCompatibleWithVersion(string version)
        {
            return version == Version;
        }
    }
}