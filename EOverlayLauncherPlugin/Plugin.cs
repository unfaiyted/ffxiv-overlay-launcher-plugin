using System.Diagnostics;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Runtime.InteropServices;
using Dalamud.Game.Gui;
using Dalamud.Interface.Windowing;
using EOverlayLauncherPlugin.Windows;

namespace EOverlayLauncherPlugin
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "ElectronOverlay";
        private const string CommandName = "/eoverlays";
        private const string AppPath = "ffxiv-overlay-tools";
        private readonly ChatGui chat;
        private readonly GameGui gui;

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("EOverlayLauncherPlugin");
        
        
        private ConfigWindow ConfigWindow { get; init; }
        private MainWindow MainWindow { get; init; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager,
            GameGui gui, ChatGui chat)
        {

            this.chat = chat;
            this.gui = gui;
            
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            ConfigWindow = new ConfigWindow(this);
            MainWindow = new MainWindow(this, goatImage, chat);
            
            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(MainWindow);

            this.chat.Print("Hello, world!");
           

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
           
           
            
            
            
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            
            ConfigWindow.Dispose();
            MainWindow.Dispose();
            
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slashcommand, just display our main ui
            MainWindow.IsOpen = true;
            
             if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // If we're running on Windows, we can launch the application directly
                Process.Start(AppPath);
            }
            else
            {
                
                // Otherwise, we need to use wine to launch the application
                /*ExecuteCommand($"wine \"{AppPath}\"");*/
                Utils.RunCommandInMainOs(AppPath);
            }

        }


        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }
    }
}
