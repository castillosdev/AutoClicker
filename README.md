# AutoClicker Console App

A simple Windows autoclicker that clicks at specified screen coordinates with adjustable intervals.

## Features
- 🔘 Set target position with **F5** (uses current mouse location)
- ▶️ **Start/Stop** clicking with **F6**
- ⏱️ Adjustable click interval (in milliseconds)
- 🖱️ Absolute coordinate system for multi-monitor support
- 🚫 No dependencies (native Windows API)

## Requirements
- Windows 7 or newer
- .NET Framework 4.0+
- Administrator privileges (recommended)

## Installation
1. Download latest release
2. Run `AutoClicker.exe`
3. **(Optional)** Compile from source with Visual Studio

## Usage
1. Position cursor and press **F5** to set target
2. Press **F6** to start
3. Enter desired interval (e.g., `100` = 10 clicks/sec)
4. Press **F6** again to stop
5. **F12** to exit

## Notes
- Works across multiple monitors
- Maintains click position even when moving mouse
- Run as Administrator if clicks aren't registering
- Coordinates persist until changed

⚠️ **Use responsibly!** Not for gaming/automation where prohibited.
