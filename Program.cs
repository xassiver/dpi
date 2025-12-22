using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceProcess;

/*
 * DPIA - Dynamic DPI Assistant
 * ----------------------------
 * Bu proje, internet sansürünü ve DPI (Deep Packet Inspection) engellerini aşmak için 
 * GoodbyeDPI çekirdeğini kullanan, kullanıcı dostu ve özelleştirilebilir bir sistemdir.
 * 
 * Neden Arka Planda Çalışır?
 * DPIA, bir Windows Servisi olarak kurulur. Bu sayede:
 * 1. Bilgisayar açıldığında otomatik olarak devreye girer.
 * 2. Uygulama penceresini açık tutmanıza gerek kalmaz.
 * 3. Sistem kaynaklarını minimum düzeyde tüketir (sadece paket başlıklarını manipüle eder).
 * 
 * Performans ve Güvenlik:
 * - Sadece paketlerin 'başlık' kısımlarını değiştirir, veriye dokunmaz.
 * - İşlemciyi yormaz, FPS veya Ping değerlerinizi etkilemez.
 * - Açık kaynaklıdır ve tamamen şeffaf bir yapıya sahiptir.
 */

namespace DPIA
{
    class Program
    {
        static string dpiaTag = "DPIA";
        static string tempDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DPIA");
        static string engineName = "dpia_engine.exe";
        static bool isTurkish = CultureInfo.CurrentCulture.Name.StartsWith("tr");

        static Dictionary<string, string> T = new Dictionary<string, string>();

        static void InitLang()
        {
            if (isTurkish) {
                T["StatusTitle"] = "DPIA DURUMU: ";
                T["Installed"] = "KURULU VE CALISIYOR";
                T["NotInstalled"] = "KURULU DEGIL VEYA DURMUS";
                T["MenuTitle"] = "\n--- DPIA ANA MENU ---";
                T["StartInstall"] = "1. DPIA Kurulumunu Baslat (Akıllı Otomatik Mod)";
                T["ExpertMode"] = "2. Uzman Ayarları (Parametreleri Kendin Belirle)";
                T["Uninstall"] = "3. DPIA Servisini Kaldır ve Temizle";
                T["ShowInfo"] = "4. Arka Plan Servisi ve Performans Hakkında Bilgi";
                T["Exit"] = "0. Cikis";
                T["WorkCheck"] = "\n[?] Engelli sitelere erisim var mı? (E/H): ";
                T["TryingProfile"] = ">>> Profil {0} deneniyor... Lutfen bekleyin.";
                T["Success"] = "TEBRIKLER! DPIA basariyla ayarlandi.";
                T["AllFailed"] = "Maalesef tum alternatifler denendi ama sonuc alınamadı.";
                T["Cleanup"] = "Eski servisler ve kilitli dosyalar temizleniyor...";
                T["Finished"] = "\nIslem tamamlandı. Menüye dönmek için ENTER.";
                T["InfoText"] = "\n--- DPIA CALISMA MANTIGI ---\n" +
                                "1. DPIA, bir Windows Servisi olarak kurulur. Bu, PC her acildiginda \n" +
                                "   otomatik calisacagi anlamina gelir.\n" +
                                "2. Sadece internet paketlerinin baslıklarındaki sansur mekanizmalarını \n" +
                                "   sasirtir. Veri trafiğinizi izlemez veya yavaslatmaz.\n" +
                                "3. Oyunlarda MS (Ping) artisina veya FPS dususune neden olmaz.\n" +
                                "4. Arka planda calismasinin sebebi, size kesintisiz bir deneyim sunmaktir.";
            } else {
                T["StatusTitle"] = "DPIA STATUS: ";
                T["Installed"] = "INSTALLED AND RUNNING";
                T["NotInstalled"] = "NOT INSTALLED OR STOPPED";
                T["MenuTitle"] = "\n--- DPIA MAIN MENU ---";
                T["StartInstall"] = "1. Start DPIA Installation (Smart Auto Mode)";
                T["ExpertMode"] = "2. Expert Settings (Manual Parameters)";
                T["Uninstall"] = "3. Uninstall and Cleanup DPIA";
                T["ShowInfo"] = "4. Information About Background Service & Performance";
                T["Exit"] = "0. Exit";
                T["WorkCheck"] = "\n[?] Can you access the sites now? (Y/N): ";
                T["TryingProfile"] = ">>> Trying Profile {0}... Please wait.";
                T["Success"] = "CONGRATULATIONS! DPIA configured successfully.";
                T["AllFailed"] = "All alternatives failed.";
                T["Cleanup"] = "Cleaning up old services and locked files...";
                T["Finished"] = "\nProcess complete. Press ENTER for menu.";
                T["InfoText"] = "\n--- HOW DPIA WORKS ---\n" +
                                "1. DPIA installs as a Windows Service, meaning it starts \n" +
                                "   automatically with your PC.\n" +
                                "2. It only manipulates packet headers to bypass censorship. \n" +
                                "   It does not monitor or throttle your data.\n" +
                                "3. It won't cause Ping spikes or FPS drops in games.\n" +
                                "4. It runs in the background to provide a seamless experience.";
            }
        }

        static void Main(string[] args)
        {
            InitLang();
            Console.Title = "DPIA - Open Source DPI Assistant";

            while (true)
            {
                Console.Clear();
                bool installed = IsDPIAInstalled();
                Console.ForegroundColor = installed ? ConsoleColor.Cyan : ConsoleColor.Yellow;
                Console.WriteLine("========================================");
                Console.Write("  " + T["StatusTitle"]);
                Console.ForegroundColor = installed ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine(installed ? T["Installed"] : T["NotInstalled"]);
                Console.ForegroundColor = installed ? ConsoleColor.Cyan : ConsoleColor.Yellow;
                Console.WriteLine("========================================");
                Console.ResetColor();

                Console.WriteLine(T["MenuTitle"]);
                Console.WriteLine(T["StartInstall"]);
                Console.WriteLine(T["ExpertMode"]);
                Console.WriteLine(T["Uninstall"]);
                Console.WriteLine(T["ShowInfo"]);
                Console.WriteLine(T["Exit"]);
                Console.Write("\n> ");

                string choice = Console.ReadLine();
                if (choice == "0") break;
                if (choice == "4") { Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(T["InfoText"]); Console.ReadLine(); continue; }

                try {
                    Cleanup();
                    ExtractResources();

                    if (choice == "1") RunIterativeInstall();
                    else if (choice == "2") RunExpertInstall();
                    else if (choice == "3") { Console.WriteLine(T["Finished"]); Console.ReadLine(); }
                } catch (Exception ex) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[!] Error: " + ex.Message);
                    Console.ResetColor();
                    Console.ReadLine();
                }
            }
        }

        static bool IsDPIAInstalled()
        {
            try {
                ServiceController sc = new ServiceController(dpiaTag);
                return sc.Status == ServiceControllerStatus.Running;
            } catch {
                return false;
            }
        }

        static void ExtractResources()
        {
            for (int i = 0; i < 5; i++) {
                try {
                    if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
                    break;
                } catch { 
                    Cleanup();
                    Thread.Sleep(1000); 
                }
            }
            
            if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
            
            string arch = (IntPtr.Size == 8) ? "x64" : "x86";
            SaveResource("DPIA.engine_" + arch + ".exe", Path.Combine(tempDir, engineName));
            SaveResource("DPIA.WinDivert.dll", Path.Combine(tempDir, "WinDivert.dll"));
            string sysName = (IntPtr.Size == 8) ? "WinDivert64.sys" : "WinDivert32.sys";
            SaveResource("DPIA." + sysName, Path.Combine(tempDir, sysName));
        }

        static void SaveResource(string resourceName, string path)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream s = assembly.GetManifestResourceStream(resourceName))
            {
                if (s == null) return;
                using (FileStream fs = new FileStream(path, FileMode.Create)) { s.CopyTo(fs); }
            }
        }

        static void RunIterativeInstall()
        {
            string[] profiles = {
                "-5 --set-ttl 5 --dns-addr 77.88.8.8 --dns-port 1253",
                "--set-ttl 3",
                "--set-ttl 4",
                "-5 --set-ttl 3",
                "-9 --set-ttl 5"
            };

            for (int i = 0; i < profiles.Length; i++)
            {
                Cleanup();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n" + string.Format(T["TryingProfile"], i + 1));
                
                InstallService(profiles[i]);
                Thread.Sleep(2000);
                
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(T["WorkCheck"]);
                string res = Console.ReadLine().ToLower();

                if (res == "e" || res == "y")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n" + T["Success"]);
                    Console.ReadLine();
                    return;
                }
            }
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(T["AllFailed"]);
            Cleanup();
            Console.ReadLine();
        }

        static void RunExpertInstall()
        {
            Console.Write("\nSet-TTL (Standart: 5): ");
            string ttl = Console.ReadLine();
            if (string.IsNullOrEmpty(ttl)) ttl = "5";

            Console.Write("DNS Yonlendirme acilsin mi? (E/H - Onerilen: E): ");
            string dnsChoice = Console.ReadLine().ToLower();
            bool useDns = (dnsChoice == "e" || dnsChoice == "y" || string.IsNullOrEmpty(dnsChoice));

            Cleanup();
            string args = "-5 --set-ttl " + ttl;
            if (useDns) args += " --dns-addr 77.88.8.8 --dns-port 1253";

            InstallService(args);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n" + T["Success"]);
            Console.ReadLine();
        }

        static void InstallService(string args)
        {
            string exePath = Path.Combine(tempDir, engineName);
            string ScCmd = string.Format("create \"{0}\" binPath= \"\\\"{1}\\\" {2}\" start= \"auto\"", dpiaTag, exePath, args);
            RunCommand("sc " + ScCmd);
            RunCommand("sc description \"" + dpiaTag + "\" \"DPIA: Dynamic DPI Assistant Service\"");
            RunCommand("sc start \"" + dpiaTag + "\"");
        }

        static void Cleanup()
        {
            RunCommand("sc stop " + dpiaTag);
            RunCommand("sc delete " + dpiaTag);
            RunCommand("sc stop WinDivert");
            RunCommand("sc delete WinDivert");
            RunCommand("taskkill /F /IM " + engineName);
            RunCommand("taskkill /F /IM goodbyedpi.exe");
            Thread.Sleep(500); 
        }

        static void RunCommand(string command)
        {
            try {
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + command);
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.CreateNoWindow = true;
                Process p = Process.Start(psi);
                if (p != null) p.WaitForExit();
            } catch { }
        }
    }
}
