using Dalamud.Plugin.Services;
using IniParser;
using IniParser.Model;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace XIVNVTrueHDR;

public class IniProvider
{
    private readonly string _iniFilePath;
    private IniData? _iniContent;
    private readonly FileIniDataParser _iniParser;
    private bool _immediateWrite;
    private CancellationTokenSource _writeCts = new();

    public IniProvider(IDataManager dataManager)
    {
        _iniFilePath = Path.Join(dataManager.GameData.DataPath.Parent!.FullName, "truehdrtweaks.ini");
        Available = File.Exists(_iniFilePath);
        _iniParser = new(new IniParser.Parser.IniDataParser(
            new IniParser.Model.Configuration.IniParserConfiguration()
            {
                CommentString = "#"
            }));
    }

    public bool Available { get; init; }
    public bool Loaded => _iniContent != null;
    public bool Dirty { get; private set; } = false;
    public bool ImmediateWrite
    {
        get => _immediateWrite;
        set
        {
            if (value == _immediateWrite) return;
            _immediateWrite = value;
            if (_immediateWrite && Dirty) _ = WriteDataAsync();
        }
    }

    public void LoadData()
    {
        if (!Available) return;

        _iniContent = _iniParser.ReadFile(_iniFilePath);
    }

    public async Task WriteDataAsync()
    {
        if (!Dirty) return;
        _writeCts?.Cancel();
        _writeCts?.Dispose();
        _writeCts = new();
        var token = _writeCts.Token;

        await Task.Delay(TimeSpan.FromSeconds(0.5), token);

        _iniParser.WriteFile(_iniFilePath, _iniContent);
        Dirty = false;
    }

    public int Quality
    {
        get
        {
            if (!Loaded) return byte.MinValue;
            return int.Parse(_iniContent!["Values"]["Quality"]);
        }
        set
        {
            if (!Loaded || Quality == value) return;
            _iniContent!["Values"]["Quality"] = value.ToString();
            Dirty = true;
            if (ImmediateWrite) _ = WriteDataAsync();
        }
    }

    public bool SetIndicatorHUD
    {
        get => _iniContent!["Values"]["EnableIndicatorHUD"] != "-1";
        set
        {
            if (value)
                IndicatorHUD = false;
            else
                IndicatorHUD = null;
        }
    }

    public bool? IndicatorHUD
    {
        get
        {
            if (!Loaded) return false;
            return _iniContent!["Values"]["EnableIndicatorHUD"] == "1";
        }
        set
        {
            if (!Loaded || IndicatorHUD == value) return;
            _iniContent!["Values"]["EnableIndicatorHUD"] = value == null ? "-1" : (value.Value ? "1" : "0");
            Dirty = true;
            if (ImmediateWrite) _ = WriteDataAsync();
        }
    }

    public bool SetPeakBrightness
    {
        get => PeakBrightness != -1;
        set
        {
            if (value)
                PeakBrightness = 1000;
            else
                PeakBrightness = -1;
        }
    }

    public int PeakBrightness
    {
        get
        {
            if (!Loaded) return int.MinValue;
            return int.Parse(_iniContent!["Values"]["PeakBrightness"]);
        }
        set
        {
            if (!Loaded || PeakBrightness == value) return;
            _iniContent!["Values"]["PeakBrightness"] = value.ToString();
            Dirty = true;
            if (ImmediateWrite) _ = WriteDataAsync();
        }
    }

    public bool SetPaperWhite
    {
        get => PaperWhite != -1;
        set
        {
            if (value)
                PaperWhite = 50;
            else
                PaperWhite = -1;
        }
    }

    public int PaperWhite
    {
        get
        {
            if (!Loaded) return int.MinValue;
            return int.Parse(_iniContent!["Values"]["Paperwhite"]);
        }
        set
        {
            if (!Loaded || PaperWhite == value) return;
            _iniContent!["Values"]["Paperwhite"] = value.ToString();
            Dirty = true;
            if (ImmediateWrite) _ = WriteDataAsync();
        }
    }

    public bool SetContrast
    {
        get => Contrast != -1;
        set
        {
            if (value)
                Contrast = 0.85f;
            else
                Contrast = -1;
        }
    }

    public float Contrast
    {
        get
        {
            if (!Loaded) return float.MinValue;
            return float.Parse(_iniContent!["Values"]["Contrast"]);
        }
        set
        {
            if (!Loaded || Contrast == value) return;
            _iniContent!["Values"]["Contrast"] = value < 0 ? value.ToString("0") : value.ToString("0.00");
            Dirty = true;
            if (_immediateWrite) _ = WriteDataAsync();
        }
    }

    public bool SetSaturation
    {
        get => Saturation != -1;
        set
        {
            if (value)
                Saturation = 1.1f;
            else
                Saturation = -1;
        }
    }

    public float Saturation
    {
        get
        {
            if (!Loaded) return float.MinValue;
            return float.Parse(_iniContent!["Values"]["Saturation"]);
        }
        set
        {
            if (!Loaded || Saturation == value) return;
            _iniContent!["Values"]["Saturation"] = value < 0 ? value.ToString("0") : value.ToString("0.00");
            Dirty = true;
            if (_immediateWrite) _ = WriteDataAsync();
        }
    }

    public bool SetStrength
    {
        get => Strength != -1;
        set
        {
            if (value)
                Strength = 1.1f;
            else
                Strength = -1;
        }
    }

    public float Strength
    {
        get
        {
            if (!Loaded) return float.MinValue;
            return float.Parse(_iniContent!["Values"]["Strength"]);
        }
        set
        {
            if (!Loaded || Strength == value) return;
            _iniContent!["Values"]["Strength"] = value < 0 ? value.ToString("0") : value.ToString("0.00");
            Dirty = true;
            if (_immediateWrite) _ = WriteDataAsync();
        }
    }

    public bool SetGamma
    {
        get => Gamma != -1;
        set
        {
            if (value)
                Gamma = 1.1f;
            else
                Gamma = -1;
        }
    }

    public float Gamma
    {
        get
        {
            if (!Loaded) return float.MinValue;
            return float.Parse(_iniContent!["Values"]["Gamma"]);
        }
        set
        {
            if (!Loaded || Gamma == value) return;
            _iniContent!["Values"]["Gamma"] = value < 0 ? value.ToString("0") : value.ToString("0.0");
            Dirty = true;
            if (_immediateWrite) _ = WriteDataAsync();
        }
    }

    public int HDRDisplayMode
    {
        get
        {
            if (!Loaded) return int.MinValue;
            return int.Parse(_iniContent!["Values"]["HDRDisplayMode"]);
        }
        set
        {
            if (!Loaded || HDRDisplayMode == value) return;
            _iniContent!["Values"]["HDRDisplayMode"] = value.ToString();
            Dirty = true;
            if (ImmediateWrite) _ = WriteDataAsync();
        }
    }

    public int HDRVisualization
    {
        get
        {
            if (!Loaded) return byte.MinValue;
            return int.Parse(_iniContent!["Values"]["HDRVisualization"]);
        }
        set
        {
            if (!Loaded || HDRVisualization == value) return;
            _iniContent!["Values"]["HDRVisualization"] = value.ToString();
            Dirty = true;
            if (ImmediateWrite) _ = WriteDataAsync();
        }
    }

    public bool SetSDRBrightness
    {
        get => SDRBrightness != -1;
        set
        {
            if (value)
                SDRBrightness = 150;
            else
                SDRBrightness = -1;
        }
    }

    public int SDRBrightness
    {
        get
        {
            if (!Loaded) return int.MinValue;
            return int.Parse(_iniContent!["Values"]["SDRBrightness"]);
        }
        set
        {
            if (!Loaded || SDRBrightness == value) return;
            _iniContent!["Values"]["SDRBrightness"] = value.ToString();
            Dirty = true;
            if (_immediateWrite) _ = WriteDataAsync();
        }
    }
}
