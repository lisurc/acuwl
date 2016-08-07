using IniParser;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace ACUWL {
    class Program {
        const string ACUDisplayName = "Assassin's Creed Unity";
        const string ACUIniDirectoryName = "Assassin's Creed Unity";
        const string MyIniName = "ACUWL.ini";
        const string ACUIniName = "ACU.ini";
        const string ACUExeName = "ACU.exe";

        static void Main(string[] args) {
            Console.WriteLine("Assassin's Creed Unity custom launcher");
            if (args.Length == 1) {
                if (args[0] == "/?" || args[0] == "/help") {
                    Console.WriteLine("Usage: ACULauncher.exe [IniPath] [ExecPath]");
                    Console.WriteLine("You can also create a {0} ini file with a [Settings] section key and the key/value pairs \"IniPath\" and \"ExecPath\"", MyIniName);
                    PressAnyKeyToExit(0);
                }
            }
            var parser = new FileIniDataParser();
            parser.Parser.Configuration.AssigmentSpacer = null;
            parser.Parser.Configuration.NewLineStr = Environment.NewLine;
            parser.Parser.Configuration.AllowDuplicateKeys = true;
            parser.Parser.Configuration.AllowDuplicateSections = true;
            parser.Parser.Configuration.AllowKeysWithoutSection = true;
            var myIni = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), MyIniName);

            string acuIni = null;
            string acuExecutable = null;

            if (File.Exists(myIni)) {
                var myData = parser.ReadFile(myIni);
                var acuIniPath = myData["Settings"]?["IniPath"];
                if (!string.IsNullOrEmpty(acuIniPath)) {
                    acuIni = Path.Combine(acuIniPath, MyIniName);
                }
                acuExecutable = myData["Settings"]?["ExecPath"];
            }

            if (string.IsNullOrEmpty(acuIni)) {
                acuIni = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ACUIniDirectoryName);
                acuIni = Path.Combine(acuIni, ACUIniName);
            }

            if (!File.Exists(acuIni)) {
                Console.Error.WriteLine("Can not locate ACU ini path: {0}", acuIni);
                PressAnyKeyToExit(1);
            }

            var acuData = parser.ReadFile(acuIni);
            var windowMode = acuData["Graphics"]?["WindowMode"];
            int windowModeInt;
            if (string.IsNullOrWhiteSpace(windowMode) || !int.TryParse(windowMode, out windowModeInt) || windowModeInt != 0) {
                acuData["Graphics"]["WindowMode"] = "0";
                parser.WriteFile(acuIni, acuData, Encoding.Default);
            }

            // same directory?
            if (string.IsNullOrWhiteSpace(acuExecutable)) {
                var localExe = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ACUExeName);
                if (File.Exists(localExe)) {
                    acuExecutable = localExe;
                }
            }

            // installed program?
            if (string.IsNullOrWhiteSpace(acuExecutable)) {
                var installDir = FindLocationByDisplayName();
                if (!string.IsNullOrWhiteSpace(installDir)) {
                    acuExecutable = Path.Combine(installDir, ACUExeName);
                }
            }

            if (!File.Exists(acuExecutable)) {
                Console.Error.WriteLine("Can not locate ACU executable path: {0}", acuExecutable);
                PressAnyKeyToExit(1);
            }
            
            Process.Start(acuExecutable);
        }

        /// <summary>
        /// Displays a message to invite the user to press any key to exit the program.
        /// </summary>
        /// <param name="code"></param>
        private static void PressAnyKeyToExit(int code) {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            Environment.Exit(code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string FindLocationByDisplayName() {
            var views = new RegistryView[] { RegistryView.Registry64, RegistryView.Registry32 };

            foreach (var view in views) {
                try {
                    using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view)) {
                        using (var parentKey = baseKey.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall")) {
                            string[] nameList = parentKey.GetSubKeyNames();
                            for (int i = 0; i < nameList.Length; i++) {
                                using (var regKey = parentKey.OpenSubKey(nameList[i])) {
                                    try {
                                        if (regKey.GetValue("DisplayName")?.ToString() == ACUDisplayName) {
                                            return regKey.GetValue("InstallLocation")?.ToString();
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
                catch { }
            }
            return null;
        }
    }
}
