# Assassin's Creed Unity Windowed mode Launcher (acuwl)

[![GitHub version](https://badge.fury.io/gh/lisurc%2Facuwl.svg)](https://badge.fury.io/gh/lisurc%2Facuwl)
[![Build](https://github.com/lisurc/acuwl/actions/workflows/build.yml/badge.svg)](https://github.com/lisurc/acuwl/actions/workflows/build.yml)

## What
This small utility may be used to force the windowed mode of the game when starting Assassin's Creed Unity (ACU).

## Why
On some systems, ACU has proved to be unstable at launch with many different issues, mosty because of driver compatibility or Windows 10 upgrade. Black screen, Missing DLLs, etc. One of these issues can be resolved by launching the game in windowed mode so it can start correctly (don't ask why). Once the game has been started, you can switch back to full screen mode using the ALT+Enter keyboard shortcut.

## How
The utility works by updating the ACU.ini file of your User directory and start starting the game through the regular ACU.exe executable. Some help can be found by starting the utility with the "/?" or "/help" argument.

If your game has been installed through a regular installer (may it be Steam of UPlay), the utility should automatically find these files. 

If not, you may create a ACUWL.ini file alongsite the ACUWL.exe executable. The file content should look like the following:

```ini
[Settings]
IniPath=path_to_ini
ExecPath=path_to_exec
```

You can also create a shortcut to the ACUWL.exe executable with two arguments pointing at the necessary files like this:

```
ACUWL.exe [path_to_ini] [path_to_exec]
```

Note that you may need to start the utility with elevated privileges (Administrator). This is up to your actual system setup.

## Where
While you should never download an executable file from a stranger on the Internet, you can download the latest packaged executable in the [Releases](https://github.com/lisurc/acuwl/releases) page.

Alternatively you can clone this repository and build it with any recent Visual Studio Community edition.

## Credits
All products named in this project are trademarks of their respective owners or publishers, which include but is not limited to, Ubisoft, Assassin's Creed, Assassin's Creed Unity, UPlay, Steam.
