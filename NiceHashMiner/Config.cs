﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace NiceHashMiner
{
    public class SubConfigClass
    { }

    public class Algo : SubConfigClass
    {
#pragma warning disable 649
        public string Name; // only used for easier manual identification in config file
        public string ExtraLaunchParameters;
        public string UsePassword;
        public double BenchmarkSpeed;
        public bool Skip;
        public bool[] DisabledDevices;
#pragma warning restore 649
    }

    public class Group : SubConfigClass
    {
#pragma warning disable 649
        public string Name; // only used for easier manual identification in config file
        public int APIBindPort;
        public string ExtraLaunchParameters;
        public string UsePassword;
        public double MinimumProfit;
        public int DaggerHashimotoGenerateDevice;
        public int[] DisabledDevices;
        public Algo[] Algorithms;
#pragma warning restore 649

        public Group()
        {
            DisabledDevices = new int[0];
            Algorithms = new Algo[0];
        }
    }

    public class Config
    {
#pragma warning disable 649
        public Version ConfigFileVersion;
        public int Language;
        public bool DebugConsole;
        public string BitcoinAddress;
        public string WorkerName;
        public int ServiceLocation;
        public bool AutoStartMining;
        public bool HideMiningWindows;
        public bool MinimizeToTray;
        public int LessThreads;
        public int ForceCPUExtension; // 0 - automatic, 1 - SSE2, 2 - AVX, 3 - AVX2
        public int SwitchMinSecondsFixed;
        public int SwitchMinSecondsDynamic;
        public int SwitchMinSecondsAMD;
        public int MinerAPIQueryInterval;
        public int MinerRestartDelayMS;
        public int MinerAPIGraceSeconds;
        public int[] BenchmarkTimeLimitsCPU;
        public int[] BenchmarkTimeLimitsNVIDIA;
        public int[] BenchmarkTimeLimitsAMD;
        public bool DisableDetectionNVidia5X;
        public bool DisableDetectionNVidia3X;
        public bool DisableDetectionNVidia2X;
        public bool DisableDetectionAMD;
        public bool DisableAMDTempControl;
        public bool AutoScaleBTCValues;
        public bool StartMiningWhenIdle;
        public int MinIdleSeconds;
        public bool LogToFile;
        public long LogMaxFileSize;  // in bytes
        public bool ShowDriverVersionWarning;
        public bool DisableWindowsErrorReporting;
        public bool UseNewSettingsPage;
        public bool NVIDIAP0State;
        public int ethminerAPIPortNvidia;
        public int ethminerAPIPortAMD;
        public int ethminerDefaultBlockHeight;
        public Group[] Groups;
#pragma warning restore 649

        public static Config ConfigData;

        public static void InitializeConfig()
        {
            // Set defaults
            ConfigData = new Config();
            SetDefaults();

            try
            {
                if (new FileInfo("config.json").Length > 17000)
                    ConfigData = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
                else
                {
                    File.Delete("config.json");
                }
            }
            catch { }

            if (ConfigData.ConfigFileVersion == null || 
                ConfigData.ConfigFileVersion.CompareTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version) != 0)
            {
                Helpers.ConsolePrint("CONFIG", "Config file is from an older version of NiceHashMiner..");
                Helpers.ConsolePrint("CONFIG", "Backing up config.json to config_old.json..");
                try
                {
                    if (File.Exists("config_old.json"))
                        File.Delete("config_old.json");
                    File.Move("config.json", "config_old.json");
                } catch { }

                SetDefaults();
            }

            if (ConfigData.SwitchMinSecondsFixed <= 0)
                ConfigData.SwitchMinSecondsFixed = 90;
            if (ConfigData.SwitchMinSecondsDynamic <= 0)
                ConfigData.SwitchMinSecondsDynamic = 30;
            if (ConfigData.SwitchMinSecondsAMD <= 0)
                ConfigData.SwitchMinSecondsAMD = 60;
            if (ConfigData.MinerAPIQueryInterval <= 0)
                ConfigData.MinerAPIQueryInterval = 5;
            if (ConfigData.MinerRestartDelayMS <= 0)
                ConfigData.MinerRestartDelayMS = 500;
            if (ConfigData.MinerAPIGraceSeconds < 0)
                ConfigData.MinerAPIGraceSeconds = 0;
            if (ConfigData.BenchmarkTimeLimitsCPU == null || ConfigData.BenchmarkTimeLimitsCPU.Length < 3)
                ConfigData.BenchmarkTimeLimitsCPU = new int[] { 10, 20, 60 };
            if (ConfigData.BenchmarkTimeLimitsNVIDIA == null || ConfigData.BenchmarkTimeLimitsNVIDIA.Length < 3)
                ConfigData.BenchmarkTimeLimitsNVIDIA = new int[] { 10, 20, 60 };
            if (ConfigData.BenchmarkTimeLimitsAMD == null || ConfigData.BenchmarkTimeLimitsAMD.Length < 3)
                ConfigData.BenchmarkTimeLimitsAMD = new int[] { 120, 180, 240 };
            if (ConfigData.MinIdleSeconds <= 0)
                ConfigData.MinIdleSeconds = 60;
            if (ConfigData.LogMaxFileSize <= 0)
                ConfigData.LogMaxFileSize = 1048576;
        }

        public static void SetDefaults()
        {
            ConfigData.ConfigFileVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            ConfigData.Language = 0;
            ConfigData.BitcoinAddress = "";
            ConfigData.WorkerName = "worker1";
            ConfigData.ServiceLocation = 0;
            ConfigData.LessThreads = 0;
            ConfigData.Groups = new Group[0];
            ConfigData.DebugConsole = false;
            ConfigData.HideMiningWindows = false;
            ConfigData.MinimizeToTray = false;
            ConfigData.AutoStartMining = false;
            ConfigData.DisableDetectionNVidia5X = false;
            ConfigData.DisableDetectionNVidia3X = false;
            ConfigData.DisableDetectionNVidia2X = false;
            ConfigData.DisableDetectionAMD = false;
            ConfigData.DisableAMDTempControl = false;
            ConfigData.AutoScaleBTCValues = true;
            ConfigData.StartMiningWhenIdle = false;
            ConfigData.LogToFile = true;
            ConfigData.LogMaxFileSize = 1048576;
            ConfigData.ShowDriverVersionWarning = true;
            ConfigData.DisableWindowsErrorReporting = true;
            ConfigData.UseNewSettingsPage = true;
            ConfigData.NVIDIAP0State = false;
            ConfigData.MinerRestartDelayMS = 500;
            ConfigData.ethminerAPIPortNvidia = 34561;
            ConfigData.ethminerAPIPortAMD = 34562;
            ConfigData.ethminerDefaultBlockHeight = 1700000;
            ConfigData.MinerAPIGraceSeconds = 30;
            ConfigData.SwitchMinSecondsFixed = 90;
            ConfigData.SwitchMinSecondsDynamic = 30;
            ConfigData.SwitchMinSecondsAMD = 90;
            ConfigData.MinIdleSeconds = 60;
        }

        public static void Commit()
        {
            try { File.WriteAllText("config.json", JsonConvert.SerializeObject(ConfigData, Formatting.Indented)); }
            catch { }
        }

        public static void RebuildGroups()
        {
            // rebuild config groups
            Group[] CG = new Group[Form1.Miners.Length];
            for (int i = 0; i < Form1.Miners.Length; i++)
            {
                CG[i] = new Group();
                CG[i].Name = Form1.Miners[i].MinerDeviceName;
                CG[i].APIBindPort = Form1.Miners[i].APIPort;
                CG[i].ExtraLaunchParameters = Form1.Miners[i].ExtraLaunchParameters;
                CG[i].UsePassword = Form1.Miners[i].UsePassword;
                CG[i].MinimumProfit = Form1.Miners[i].MinimumProfit;
                CG[i].DaggerHashimotoGenerateDevice = Form1.Miners[i].DaggerHashimotoGenerateDevice;
                CG[i].Algorithms = new Algo[Form1.Miners[i].SupportedAlgorithms.Length];
                for (int k = 0; k < Form1.Miners[i].SupportedAlgorithms.Length; k++)
                {
                    CG[i].Algorithms[k] = new Algo();
                    CG[i].Algorithms[k].Name = Form1.Miners[i].SupportedAlgorithms[k].NiceHashName;
                    CG[i].Algorithms[k].BenchmarkSpeed = Form1.Miners[i].SupportedAlgorithms[k].BenchmarkSpeed;
                    CG[i].Algorithms[k].ExtraLaunchParameters = Form1.Miners[i].SupportedAlgorithms[k].ExtraLaunchParameters;
                    CG[i].Algorithms[k].UsePassword = Form1.Miners[i].SupportedAlgorithms[k].UsePassword;
                    CG[i].Algorithms[k].Skip = Form1.Miners[i].SupportedAlgorithms[k].Skip;

                    CG[i].Algorithms[k].DisabledDevices = new bool[Form1.Miners[i].CDevs.Count];
                    for (int j = 0; j < Form1.Miners[i].CDevs.Count; j++)
                    {
                        CG[i].Algorithms[k].DisabledDevices[j] = Form1.Miners[i].SupportedAlgorithms[k].DisabledDevice[j];
                    }
                }
                List<int> DD = new List<int>();
                for (int k = 0; k < Form1.Miners[i].CDevs.Count; k++)
                {
                    if (!Form1.Miners[i].CDevs[k].Enabled)
                        DD.Add(k);
                }
                CG[i].DisabledDevices = DD.ToArray();
            }
            ConfigData.Groups = CG;
            Config.Commit();
        }

        public static bool ConfigFileExist()
        {
            if (File.Exists("config.json"))
                return true;

            return false;
        }
    }
}
