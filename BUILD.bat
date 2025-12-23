@echo off
setlocal
cd /d %~dp0

echo [DPIA] Derleme Islemi Baslatiliyor...
echo [DPIA] Building Process Starting...

:: CSC derleyicisini bul
set "CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"

if not exist "%CSC%" (
    echo [ERROR] .NET Framework 4.0 veya uzeri bulunamadi. / .NET Framework 4.0 or higher not found.
    pause
    exit /b
)

:: Kaynak dosyaları paketleme icin hazırla (Eger ana klasordelerse)
echo [DPIA] Kaynaklar paketleniyor... / Packing resources...

:: Derle
"%CSC%" /out:DPIA_v1.1.exe ^
       /win32manifest:app.manifest ^
       /r:System.ServiceProcess.dll ^
       /r:System.IO.Compression.FileSystem.dll ^
       /resource:engine_x64.exe,DPIA.engine_x64.exe ^
       /resource:engine_x86.exe,DPIA.engine_x86.exe ^
       /resource:WinDivert.dll,DPIA.WinDivert.dll ^
       /resource:WinDivert64.sys,DPIA.WinDivert64.sys ^
       /resource:WinDivert32.sys,DPIA.WinDivert32.sys ^
       Program.cs

if %errorLevel% == 0 (
    echo.
    echo [SUCCESS] DPIA_v1.1.exe basariyla olusturuldu!
    echo [SUCCESS] DPIA_v1.1.exe created successfully!
) else (
    echo.
    echo [FAILED] Derleme sirasinda hata olustu. / Build error.
)

pause
