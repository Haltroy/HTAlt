# HTUPDATE
HTUPDATE is an update tool for updating anything to executables, files, folders and more.
HTUPDATE can download up-to-date files from Internet and replace current files with them.

However, **HTUPDATE is not an Installer**. It only can used to download and extract files.
But you still can add pre-installation and post-installation code (written inside of your application)
to do your own tasks.

## Usage

HTUPDATE usage is simple. Install HTUPDATE using NuGet and use in your code like this:

        using HTUPDATE;
        var htu_info = new HTUPDATE_Info(
                          "https://mywebsite.com/myproject.htupdate", // URL of HTUPDATE file 
                          Application.StartupPath, // Working folder
                          "MyProject", // Code Name of your project
                          1); // Current Version Number
        var htu = new HTUPDATE(htu_info);

To check for updates:

        htu.CheckForUpdates();

To Install update:

        htu.Update();

You also can add post or pre update stuff like this:

        // Pre-update event
        htu.PreUpdate += myPreUpdate_Event;
        // Post-update event
        htu.PostUpdate += myPostUpdate_Event;

To create update files and packages, you can use the HTPacker.