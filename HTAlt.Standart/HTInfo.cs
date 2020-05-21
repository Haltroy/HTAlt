using System;
using System.ComponentModel;

namespace HTAlt.Standart
{
    /// <summary>
    /// This class holds information about this project.
    /// </summary>
    public class HTInfo
    {
        private static readonly string version = "0.1.4.0";
        private static readonly string codeName = "Midnight Walk";
        private static readonly string name = "HTAlt";
        private static readonly string link = "https://github.com/haltroy/HTAlt";
        private static readonly string developer = "Haltroy";
        /// <summary>
        /// The Project's Name.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("The Project's Name.")]
        public string ProjectName => name;
        /// <summary>
        /// The Project's Code Name.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("The Project's Code Name.")]
        public string ProjectCodeName => codeName;
        /// <summary>
        /// The Project's version.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("The Project's version.")]
        public Version ProjectVersion => new Version(version);
        /// <summary>
        /// The Project's developer.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("The Project's developer.")]
        public string ProjectDeveloper => developer;
        /// <summary>
        /// The Project's website.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("The Project's website.")]
        public Uri ProjectWebsite => new Uri(link);
    }
}
