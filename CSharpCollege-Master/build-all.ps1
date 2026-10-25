# Собирает все проекты курса и раскладывает готовые программы по папкам _exe\<проект>,
# чтобы их можно было запускать двойным щелчком по .exe.
#
# Запуск: двойной щелчок по build-all.cmd
#     или: powershell -ExecutionPolicy Bypass -File build-all.ps1
#
# Нужно: Build Tools for Visual Studio 2022 (.NET desktop build tools, Web development build tools,
# пакет нацеливания .NET Framework 4.7.2) и .NET SDK 10 (для проекта 8).
param(
    [string]$Configuration = 'Release',

    # Путь к MSBuild.exe, если vswhere его не находит
    [string]$MsBuildPath = ''
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$out = Join-Path $root '_exe'
$logs = Join-Path $out 'logs'

# Порт, на котором IIS Express поднимает сервер проекта 10 (DevelopmentServerPort из LightsOutGame.WebApi.csproj)
$serverPort = 51546
# Порт веб-версии игры (проект 8), как в launchSettings.json
$webPort = 5105

function Find-MsBuild {
    if ($MsBuildPath) {
        return $MsBuildPath
    }

    $vswhere = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
    if (-not (Test-Path $vswhere)) {
        return $null
    }

    return & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' |
        Select-Object -First 1
}

# .cmd-файлы пишутся без BOM и с переводами строк CRLF, иначе cmd.exe их не поймёт
function Write-CmdFile([string]$path, [string]$text) {
    [IO.File]::WriteAllText($path, ($text -replace "`r?`n", "`r`n") + "`r`n", (New-Object Text.UTF8Encoding $false))
}

function Build-Solution([string]$solution, [string]$logName) {
    $log = Join-Path $logs "$logName.log"
    & $script:msbuild $solution -restore -p:RestorePackagesConfig=true "-p:Configuration=$Configuration" `
        -nologo -v:minimal "-flp:logfile=$log;verbosity=normal" | Out-Host
    return ($LASTEXITCODE -eq 0)
}

function Copy-App([string]$from, [string]$name) {
    $target = Join-Path $out $name
    if (Test-Path $target) {
        Remove-Item $target -Recurse -Force
    }

    New-Item $target -ItemType Directory | Out-Null
    Copy-Item (Join-Path $from '*') $target -Recurse -Force
    return $target
}

$msbuild = Find-MsBuild
if (-not $msbuild -or -not (Test-Path $msbuild)) {
    Write-Host 'Не найден MSBuild. Установите Build Tools for Visual Studio 2022 (см. начало файла)' -ForegroundColor Red
    Write-Host 'или укажите путь: build-all.ps1 -MsBuildPath "...\MSBuild.exe"' -ForegroundColor Red
    exit 1
}

New-Item $logs -ItemType Directory -Force | Out-Null
Write-Host "MSBuild: $msbuild"

# Name - папка в _exe; Solution - .sln от корня; App - папка проекта с .exe (рядом с .sln); Exe - имя программы
$apps = @(
    @{ Name = '01 AlarmClock';        Solution = 'Projects.24.3.24.4/1. AlarmClock/AlarmClock/AlarmClock.sln';                        App = 'AlarmClock';        Exe = 'AlarmClock.exe' },
    @{ Name = '02 Drawing';           Solution = 'Projects.24.3.24.4/2. Drawing/Drawing/Drawing.sln';                                 App = 'Drawing';           Exe = 'Drawing.exe' },
    @{ Name = '03 Calculator';        Solution = 'Projects.24.3.24.4/3. Calculator/Calculator/Calculator.sln';                        App = 'Calculator';        Exe = 'Calculator.exe' },
    @{ Name = '04 Calculator WPF';    Solution = 'Projects.24.3.24.4/4. Calculator WPF/Calculator/Calculator.sln';                    App = 'Calculator.Wpf';    Exe = 'Calculator.Wpf.exe' },
    @{ Name = '05 CardFile';          Solution = 'Projects.24.1.24.2.25.1/5. CardFile/CardFile/CardFile.sln';                         App = 'CardFile.Wpf';      Exe = 'CardFile.Wpf.exe' },
    @{ Name = '06 GraphEditor';       Solution = 'Projects.24.3.24.4/6. GraphEditor/GraphEditor/GraphEditor.sln';                     App = 'GraphEditor';       Exe = 'GraphEditor.exe' },
    @{ Name = '07 LightsOut WPF';     Solution = 'Projects.24.3.24.4/7. Game WPF/LightsOutGame/LightsOutGame.sln';                    App = 'LightsOutGame.Wpf'; Exe = 'LightsOutGame.Wpf.exe' },
    @{ Name = '09 LightsOut ADO.NET'; Solution = 'Projects.24.3.24.4/9. Game DB/ADO.NET/LightsOutGame/LightsOutGame.sln';             App = 'LightsOutGame.Wpf'; Exe = 'LightsOutGame.Wpf.exe' },
    @{ Name = '09 LightsOut EF';      Solution = 'Projects.24.3.24.4/9. Game DB/EF/LightsOutGame/LightsOutGame.sln';                  App = 'LightsOutGame.Wpf'; Exe = 'LightsOutGame.Wpf.exe' },
    @{ Name = '10 LightsOut Client';  Solution = 'Projects.24.3.24.4/10. Game Client-Server/LightsOutGame/LightsOutGame.Client.sln'; App = 'LightsOutGame.Wpf'; Exe = 'LightsOutGame.Wpf.exe' }
)

$results = New-Object System.Collections.ArrayList

foreach ($app in $apps) {
    Write-Host ''
    Write-Host "=== $($app.Name) ===" -ForegroundColor Cyan
    $solution = Join-Path $root $app.Solution

    try {
        if (-not (Build-Solution $solution $app.Name)) {
            [void]$results.Add([pscustomobject]@{ Проект = $app.Name; Результат = 'ОШИБКА СБОРКИ'; Запуск = "см. _exe\logs\$($app.Name).log" })
            continue
        }

        $binDir = Join-Path (Join-Path (Split-Path $solution) $app.App) "bin/$Configuration"
        $target = Copy-App $binDir $app.Name

        if ($app.Name -eq '10 LightsOut Client') {
            # клиент ходит на сервер, запущенный через start-server.cmd (http, порт $serverPort)
            $config = Join-Path $target "$($app.Exe).config"
            $text = [IO.File]::ReadAllText($config)
            $text = $text -replace 'key="ServerConnection" value="[^"]*"', "key=`"ServerConnection`" value=`"http://localhost:$serverPort/`""
            [IO.File]::WriteAllText($config, $text, (New-Object Text.UTF8Encoding $false))
        }

        [void]$results.Add([pscustomobject]@{ Проект = $app.Name; Результат = 'OK'; Запуск = "_exe\$($app.Name)\$($app.Exe)" })
    }
    catch {
        # например, программа из прошлой сборки ещё открыта и её папку нельзя перезаписать
        [void]$results.Add([pscustomobject]@{ Проект = $app.Name; Результат = 'ОШИБКА'; Запуск = $_.Exception.Message })
    }
}

# --- Проект 8: веб-версия игры (ASP.NET Core) ---
Write-Host ''
Write-Host '=== 08 LightsOut Web ===' -ForegroundColor Cyan
$webName = '08 LightsOut Web'
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    [void]$results.Add([pscustomobject]@{ Проект = $webName; Результат = 'ПРОПУЩЕН'; Запуск = 'нужен .NET SDK 10 (dotnet)' })
}
else {
    $webProject = Join-Path $root 'Projects.24.3.24.4/8. Game Web/LightsOutGame/LightsOutGame.Web/LightsOutGame.Web.csproj'
    $webTarget = Join-Path $out $webName
    if (Test-Path $webTarget) {
        Remove-Item $webTarget -Recurse -Force
    }

    & dotnet publish $webProject -c $Configuration -o $webTarget -nologo | Out-Host
    if ($LASTEXITCODE -eq 0) {
        Write-CmdFile (Join-Path $webTarget 'start-web.cmd') @"
@echo off
rem Starts the Lights Out web app and opens it in the browser
cd /d "%~dp0"
start "LightsOut Web" LightsOutGame.Web.exe --urls http://localhost:$webPort
timeout /t 3 /nobreak >nul
start "" http://localhost:$webPort/Game
"@
        [void]$results.Add([pscustomobject]@{ Проект = $webName; Результат = 'OK'; Запуск = "_exe\$webName\start-web.cmd" })
    }
    else {
        [void]$results.Add([pscustomobject]@{ Проект = $webName; Результат = 'ОШИБКА СБОРКИ'; Запуск = 'см. вывод dotnet publish выше' })
    }
}

# --- Проект 10: сервер (ASP.NET Web API, запускается в IIS Express) ---
Write-Host ''
Write-Host '=== 10 LightsOut Server ===' -ForegroundColor Cyan
$serverName = '10 LightsOut Server'
$serverSolution = Join-Path $root 'Projects.24.3.24.4/10. Game Client-Server/LightsOutGame/LightsOutGame.Server.sln'
if (Build-Solution $serverSolution $serverName) {
    $webApiDir = (Resolve-Path (Join-Path (Split-Path $serverSolution) 'LightsOutGame.WebApi')).Path
    $serverTarget = Join-Path $out $serverName
    New-Item $serverTarget -ItemType Directory -Force | Out-Null
    Write-CmdFile (Join-Path $serverTarget 'start-server.cmd') @"
@echo off
rem Starts LightsOutGame.WebApi in IIS Express: http://localhost:$serverPort/  (Swagger: /swagger)
set IISEXPRESS=%ProgramFiles%\IIS Express\iisexpress.exe
if not exist "%IISEXPRESS%" (
    echo IIS Express not found: %IISEXPRESS%
    echo Install "IIS 10.0 Express" from Microsoft and run this file again.
    pause
    exit /b 1
)
"%IISEXPRESS%" /path:"$webApiDir" /port:$serverPort
pause
"@
    [void]$results.Add([pscustomobject]@{ Проект = $serverName; Результат = 'OK'; Запуск = "_exe\$serverName\start-server.cmd" })
}
else {
    [void]$results.Add([pscustomobject]@{ Проект = $serverName; Результат = 'ОШИБКА СБОРКИ'; Запуск = "см. _exe\logs\$serverName.log (нужны Web development build tools)" })
}

Write-Host ''
Write-Host '=== Итог ===' -ForegroundColor Cyan
$results | Format-Table -AutoSize | Out-String -Width 300 | Write-Host
Write-Host 'Проекты 9 и 10 работают с PostgreSQL: строки подключения - в файлах *.exe.config в папках _exe'
Write-Host '(для сервера 10 - в Web.config проекта LightsOutGame.WebApi). Таблицы: schema.sql (ADO.NET) или Update-Database (EF).'
