using Dalamud.Game.Command;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using XIVNVTrueHDR.Windows;

namespace XIVNVTrueHDR;

public sealed class Plugin : IDalamudPlugin
{
    public string Name => "XIV NvTrueHDR Configurator";
    private const string _commandName = "/hdr";
    private DalamudPluginInterface PluginInterface { get; init; }
    private ICommandManager CommandManager { get; init; }
    public WindowSystem WindowSystem = new("XIVNVTrueHDR");
    private ConfigWindow ConfigWindow { get; init; }
    private IniProvider IniProvider { get; init; }

    public Plugin(
        DalamudPluginInterface pluginInterface,
        ICommandManager commandManager,
        IDataManager dataManager)
    {
        PluginInterface = pluginInterface;
        CommandManager = commandManager;
        IniProvider = new(dataManager);

        IniProvider.LoadData();

        ConfigWindow = new ConfigWindow(IniProvider);

        WindowSystem.AddWindow(ConfigWindow);

        CommandManager.AddHandler(_commandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open Configuration Menu"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        CommandManager.RemoveHandler(_commandName);
    }

    private void OnCommand(string command, string args)
    {
        if (string.IsNullOrEmpty(args))
            ConfigWindow.IsOpen = true;
        else
        {
            if (args == "toggle")
            {
                if (!IniProvider.Loaded) return;
                var currentMode = IniProvider.HDRDisplayMode;
                if (currentMode == 0) IniProvider.HDRDisplayMode = 1;
                else IniProvider.HDRDisplayMode = 0;
                if (!IniProvider.ImmediateWrite) _ = IniProvider.WriteDataAsync();
            }
        }
    }

    private void DrawUI()
    {
        WindowSystem.Draw();
    }

    public void DrawConfigUI()
    {
        ConfigWindow.IsOpen = true;
    }
}
