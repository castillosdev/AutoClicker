using System;
using System.Runtime.InteropServices;
using System.Timers;
using System.Threading;

namespace AutoClicker
{
    class Program
    {
        private static System.Timers.Timer clickTimer;
        private static int targetX;
        private static int targetY;
        private static bool isRunning = false;

        // Import required Windows API functions
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int nIndex);

        // Constants
        const int SM_CXSCREEN = 0;
        const int SM_CYSCREEN = 1;
        const int INPUT_MOUSE = 0;
        const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;

        // Structures
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public int type;
            [FieldOffset(8)]
            public MOUSEINPUT mi;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AutoClicker Console App");
            Console.WriteLine("F5  - Set target position");
            Console.WriteLine("F6  - Start/Stop clicking");
            Console.WriteLine("F12 - Exit");
            Console.WriteLine("--------------------------");

            clickTimer = new System.Timers.Timer();
            clickTimer.AutoReset = true;
            clickTimer.Elapsed += ClickTimer_Elapsed;

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.F5:
                            SetTargetPosition();
                            break;
                        case ConsoleKey.F6:
                            ToggleAutoClicker();
                            break;
                        case ConsoleKey.F12:
                            Environment.Exit(0);
                            break;
                    }
                }
                Thread.Sleep(50);
            }
        }

        static void SetTargetPosition()
        {
            if (GetCursorPos(out POINT cursorPos))
            {
                targetX = cursorPos.X;
                targetY = cursorPos.Y;
                Console.WriteLine($"Target position set to: ({targetX}, {targetY})");
            }
            else
            {
                Console.WriteLine("Failed to get cursor position!");
            }
        }

        static void ToggleAutoClicker()
        {
            if (!isRunning)
            {
                if (targetX == 0 && targetY == 0)
                {
                    Console.WriteLine("Set target position first (F5)!");
                    return;
                }

                Console.Write("Enter click interval (ms): ");
                if (int.TryParse(Console.ReadLine(), out int interval))
                {
                    clickTimer.Interval = interval;
                    isRunning = true;
                    clickTimer.Start();
                    Console.WriteLine("AutoClicker started!");
                }
                else
                {
                    Console.WriteLine("Invalid interval!");
                }
            }
            else
            {
                isRunning = false;
                clickTimer.Stop();
                Console.WriteLine("AutoClicker stopped!");
            }
        }

        static void ClickTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            PerformClick();
        }

        static void PerformClick()
        {
            int screenWidth = GetSystemMetrics(SM_CXSCREEN);
            int screenHeight = GetSystemMetrics(SM_CYSCREEN);

            // Convert to absolute coordinates (0-65535)
            int absX = (targetX * 65535) / (screenWidth - 1);
            int absY = (targetY * 65535) / (screenHeight - 1);

            INPUT[] inputs = new INPUT[2];

            // Mouse down
            inputs[0] = new INPUT
            {
                type = INPUT_MOUSE,
                mi = new MOUSEINPUT
                {
                    dx = absX,
                    dy = absY,
                    dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN
                }
            };

            // Mouse up
            inputs[1] = new INPUT
            {
                type = INPUT_MOUSE,
                mi = new MOUSEINPUT
                {
                    dx = absX,
                    dy = absY,
                    dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP
                }
            };

            SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
        }
    }
}