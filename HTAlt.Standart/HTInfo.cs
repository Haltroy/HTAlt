/*

Copyright © 2018 - 2021 haltroy

Use of this source code is governed by an MIT License that can be found in github.com/Haltroy/HTAlt/blob/master/LICENSE

*/

using System;
using System.ComponentModel;

namespace HTAlt
{
    /// <summary>
    /// This class holds information about this project.
    /// </summary>
    public static class HTInfo
    {
        /// <summary>
        /// The Project's Name.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("The Project's Name.")]
        public static string ProjectName { get; } = "HTAlt";

        /// <summary>
        /// The Project's Code Name.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("The Project's Code Name.")]
        public static string ProjectCodeName { get; } = "Coffee II";

        /// <summary>
        /// The Project's version.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("The Project's version.")]
        public static string ProjectVersion { get; } = "0.1.7.1";

        /// <summary>
        /// The Project's developer.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("The Project's developer.")]
        public static string ProjectDeveloper { get; } = "Haltroy";

        /// <summary>
        /// The Project's website.
        /// </summary>
        [Bindable(false)]
        [Category("HTAlt")]
        [Description("The Project's website.")]
        public static Uri ProjectWebsite => new Uri("https://github.com/haltroy/HTAlt");
    }
}