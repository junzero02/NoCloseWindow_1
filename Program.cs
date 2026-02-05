using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace NoCloseWindow_1
{
    static class Program
    {
        private static Mutex? mutex;

        [STAThread]
        static void Main()
        {
            bool createdNew;
            mutex = new Mutex(true, "NoCloseWindow_1_Mutex", out createdNew);

            if (!createdNew)
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string exePath = Application.ExecutablePath;
            string exeDir = Path.GetDirectoryName(exePath) ?? "";
            string batPath = Path.Combine(exeDir, "restart.bat");
            string unlockPath = Path.Combine(exeDir, "unlock.txt");

            // restart.bat 생성 (없으면)
            if (!File.Exists(batPath))
            {
                string batContent = $@"
@echo off
:loop
tasklist /FI ""IMAGENAME eq {Path.GetFileName(exePath)}"" 2>NUL | find /I ""{Path.GetFileName(exePath)}"" >NUL
if errorlevel 1 (
    echo {Path.GetFileName(exePath)} is not running. Restarting...
    start """" ""{Path.GetFileName(exePath)}""
)
timeout /t 5 >NUL
goto loop
";

                File.WriteAllText(batPath, batContent);
            }

            // 숨김 모드로 restart.bat 직접 실행
            Process.Start(new ProcessStartInfo
            {
                FileName = batPath,
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            });

            // 백그라운드 스레드에서 bat 파일 감시 (2초마다)
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(2000); // 2초마다 확인

                    try
                    {
                        if (!File.Exists(batPath))
                        {
                            // bat 파일이 없으면 다시 생성
                            string batContent = $@"
@echo off
:loop
tasklist /FI ""IMAGENAME eq {Path.GetFileName(exePath)}"" 2>NUL | find /I ""{Path.GetFileName(exePath)}"" >NUL
if errorlevel 1 (
    echo {Path.GetFileName(exePath)} is not running. Restarting...
    start """" ""{Path.GetFileName(exePath)}""
)
timeout /t 5 >NUL
goto loop
";

                            File.WriteAllText(batPath, batContent);

                            // 다시 실행
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = batPath,
                                UseShellExecute = true,
                                CreateNoWindow = true,
                                WindowStyle = ProcessWindowStyle.Hidden
                            });
                        }

                        if (!IsProcessRunning("cmd.exe", batPath))
                        {
                            // bat가 실행 중이 아니면 다시 실행
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = batPath,
                                UseShellExecute = true,
                                CreateNoWindow = true,
                                WindowStyle = ProcessWindowStyle.Hidden
                            });
                        }
                    }
                    catch
                    {
                        // 예외 무시하고 계속 감시
                    }
                }
            }) { IsBackground = true }.Start();

            // 외부 트리거 감시 (unlock.txt, 3초마다)
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(3000); // 3초마다 확인

                    try
                    {
                        if (File.Exists(unlockPath))
                        {
                            // UI 스레드에서 종료
                            if (Application.OpenForms[0] is Form1 form)
                            {
                                form.Invoke(new Action(() =>
                                {
                                    Application.Exit();
                                }));
                            }
                        }
                    }
                    catch
                    {
                        // 예외 무시하고 계속 감시
                    }
                }
            }) { IsBackground = true }.Start();

            // 메인 UI 실행
            Application.Run(new Form1());
        }

        private static bool IsProcessRunning(string processName, string argument)
        {
            foreach (Process process in Process.GetProcessesByName(processName))
            {
                try
                {
                    if (process.MainModule?.FileName == argument)
                        return true;
                }
                catch
                {
                    // 예외 무시
                }
            }
            return false;
        }
    }
}
