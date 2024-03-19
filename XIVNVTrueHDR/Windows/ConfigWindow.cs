using System;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace XIVNVTrueHDR.Windows;

public class ConfigWindow : Window
{
    private readonly IniProvider _iniProvider;

    public ConfigWindow(IniProvider iniProvider) : base(
        "XIV NvTrueHDR Configuration",
        ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeCondition = ImGuiCond.Always;

        _iniProvider = iniProvider;
    }

    public override void Draw()
    {
        float indent = ImGui.GetFrameHeight();
        if (!_iniProvider.Available)
        {
            ImGui.Text("Configuration file not found");
            return;
        }

        if (!_iniProvider.Loaded)
        {
            if (ImGui.Button("Load INI"))
            {
                _iniProvider.LoadData();
            }

            return;
        }

        var quality = _iniProvider.Quality;
        if (ImGui.Combo("HDR Quality", ref quality, ["Low", "Medium", "High"], 3))
        {
            _iniProvider.Quality = (byte)quality;
        }
        if (ImGui.IsItemHovered())
            ImGui.SetTooltip("High HDR quality has a non-insignificant FPS impact.");
        ImGui.Text("Note: HDR Quality changes require a restart.");

        var hdrDisplayMode = _iniProvider.HDRDisplayMode;
        if (ImGui.Combo("HDR Display Mode", ref hdrDisplayMode, ["SDR", "SDR to HDR", "SDR/HDR Split", "HDR/SDR Split"], 4))
        {
            _iniProvider.HDRDisplayMode = hdrDisplayMode;
        }
        if (ImGui.IsItemHovered())
            ImGui.SetTooltip("SDR mode uses SDR Mode Brightness Slider");

        var setPeakBrightness = _iniProvider.SetPeakBrightness;
        if (ImGui.Checkbox("Set Peak Brightness", ref setPeakBrightness))
        {
            _iniProvider.SetPeakBrightness = setPeakBrightness;
        }
        using (ImRaii.PushIndent(indent))
        using (ImRaii.Disabled(!_iniProvider.SetPeakBrightness))
        {
            int val = _iniProvider.PeakBrightness;
            if (ImGui.SliderInt("Peak Brightness", ref val, 400, 1500))
            {
                _iniProvider.PeakBrightness = val;
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Peak brightness in nits: suggested 750-1000 or whatever your display can offer. Don't go lower than 400." + Environment.NewLine +
                "Default: 1000");
        }

        var setPaperWhite = _iniProvider.SetPaperWhite;
        if (ImGui.Checkbox("Set Paper White", ref setPaperWhite))
        {
            _iniProvider.SetPaperWhite = setPaperWhite;
        }
        using (ImRaii.PushIndent(indent))
        using (ImRaii.Disabled(!_iniProvider.SetPaperWhite))
        {
            int val = _iniProvider.PaperWhite;
            if (ImGui.SliderInt("Paper White", ref val, 1, 100))
            {
                _iniProvider.PaperWhite = val;
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Paper white multiplier, unclear how exactly it is calculated. 50 is approximately 200 nits. Suggested is 50 but try around with different values."
                + Environment.NewLine + "Default: 50");
        }

        var setContrast = _iniProvider.SetContrast;
        if (ImGui.Checkbox("Set Contrast", ref setContrast))
        {
            _iniProvider.SetContrast = setContrast;
        }
        using (ImRaii.PushIndent(indent))
        using (ImRaii.Disabled(!_iniProvider.SetContrast))
        {
            float val = _iniProvider.Contrast;
            if (ImGui.SliderFloat("Contrast", ref val, 0.5f, 1.5f))
            {
                _iniProvider.Contrast = val;
            }
            if (ImGui.IsItemHovered()) ImGui.SetTooltip("Higher = more contrast, leave at 1 for original/neutral contrast." + Environment.NewLine + "Default: 0.85");
        }

        var setSaturation = _iniProvider.SetSaturation;
        if (ImGui.Checkbox("Set Saturation", ref setSaturation))
        {
            _iniProvider.SetSaturation = setSaturation;
        }
        using (ImRaii.PushIndent(indent))
        using (ImRaii.Disabled(!_iniProvider.SetSaturation))
        {
            float val = _iniProvider.Saturation;
            if (ImGui.SliderFloat("Saturation", ref val, 0.5f, 1.5f))
            {
                _iniProvider.Saturation = val;
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Higher = more saturation, leave at 1 for original/neutral saturation." + Environment.NewLine + "Default: 1.1");
        }

        var setStrength = _iniProvider.SetStrength;
        if (ImGui.Checkbox("Set Strength", ref setStrength))
        {
            _iniProvider.SetStrength = setStrength;
        }
        using (ImRaii.PushIndent(indent))
        using (ImRaii.Disabled(!_iniProvider.SetStrength))
        {
            float val = _iniProvider.Strength;
            if (ImGui.SliderFloat("Strength", ref val, 0.5f, 1.5f))
            {
                _iniProvider.Strength = val;
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Strength of the HDR highlights curve (probably), leave at 1 or more." + Environment.NewLine + "Default: 1.3");
        }

        var setGamma = _iniProvider.SetGamma;
        if (ImGui.Checkbox("Set Gamma", ref setGamma))
        {
            _iniProvider.SetGamma = setGamma;
        }
        using (ImRaii.PushIndent(indent))
        using (ImRaii.Disabled(!_iniProvider.SetGamma))
        {
            float val = _iniProvider.Gamma;
            if (ImGui.SliderFloat("Gamma", ref val, 1.8f, 2.5f))
            {
                _iniProvider.Gamma = val;
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Something akin to gamma, leave it as is." + Environment.NewLine + "Default: 2.2");
        }

        if (_iniProvider.HDRDisplayMode == 0)
        {
            var setSDRBrightness = _iniProvider.SetSDRBrightness;
            if (ImGui.Checkbox("Set SDR Brightness", ref setSDRBrightness))
            {
                _iniProvider.SetSDRBrightness = setSDRBrightness;
            }
            using (ImRaii.PushIndent(indent))
            using (ImRaii.Disabled(!_iniProvider.SetSDRBrightness))
            {
                int val = _iniProvider.SDRBrightness;
                if (ImGui.SliderInt("SDR Brightness", ref val, 10, 400))
                {
                    _iniProvider.SDRBrightness = val;
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Only affects the image when HDR Display Mode is set to SDR");
            }
        }

        var node = ImRaii.TreeNode("Debug");
        if (node)
        {
            var setIndicatorHud = _iniProvider.SetIndicatorHUD;
            if (ImGui.Checkbox("Set HUD Indicator", ref setIndicatorHud))
            {
                _iniProvider.SetIndicatorHUD = setIndicatorHud;
            }
            using (ImRaii.PushIndent(indent))
            using (ImRaii.Disabled(!_iniProvider.SetIndicatorHUD))
            {
                bool val = _iniProvider.IndicatorHUD ?? false;
                if (ImGui.Checkbox("Show HUD Indicator", ref val))
                {
                    _iniProvider.IndicatorHUD = val;
                }
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip("Shows an indicator in the top left if NvTrueHDR is working.");
            }

            int hdrVis = _iniProvider.HDRVisualization;
            if (ImGui.Combo("HDR Visualization", ref hdrVis, ["Disabled", "Overbright Pixels"], 2))
            {
                _iniProvider.HDRVisualization = hdrVis;
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Some HDR debugging visualization.");
        }
        node.Dispose();

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        bool immediateWrite = _iniProvider.ImmediateWrite;
        using (ImRaii.Disabled(!_iniProvider.Dirty || immediateWrite))
        {
            if (ImGui.Button("Save INI"))
            {
                _ = _iniProvider.WriteDataAsync();
            }
        }

        ImGui.SameLine();
        if (ImGui.Checkbox("Immediate write to INI", ref immediateWrite))
        {
            _iniProvider.ImmediateWrite = immediateWrite;
        }
        if (ImGui.IsItemHovered())
            ImGui.SetTooltip("When enabled will automatically write changes to the ini file");
    }
}
