using System.Diagnostics;

namespace EOverlayLauncherPlugin;

public class Utils
{
    public static void RunCommandInMainOs(string command)
    {
        var startInfo = new ProcessStartInfo("wineconsole");

        
        //TODO: check if app exists, check if installed, maybe even install?
        // should I include it w/ the plugin somehow?
        
        // Would maybe like to launch the app "headless" and only show the overlays
        // Could add some commmand line option to hide the add overlays, other options
        
        // TODO:\ Need to get the config file loaded properly
        startInfo.Arguments = $"start /unix /usr/bin/ffxiv-overlay-tools";
        // startInfo.Arguments = $"gnome-terminal -- bash -c \"{command}; exec bash\"";

        var process = new Process();
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();
    }
}
