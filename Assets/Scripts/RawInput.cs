// NativeMethods.cs
using System;
using System.Runtime.InteropServices;

public static class RawInput
{
	// 가상 키 코드 정의 (우리가 관심 있는 키만)
	public const int VK_KEY_1 = 0x31;
	public const int VK_KEY_2 = 0x32;
	public const int VK_KEY_3 = 0x33;
	public const int VK_KEY_4 = 0x34;

	// Win32 API 함수 임포트
	[DllImport("user32.dll")]
	public static extern IntPtr CreateWindowEx(uint dwExStyle, string lpClassName, string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

	[DllImport("user32.dll")]
	public static extern bool DestroyWindow(IntPtr hWnd);

	[DllImport("user32.dll")]
	public static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

	[DllImport("user32.dll")]
	public static extern bool TranslateMessage([In] ref MSG lpMsg);

	[DllImport("user32.dll")]
	public static extern IntPtr DispatchMessage([In] ref MSG lpMsg);

	[DllImport("user32.dll")]
	public static extern bool RegisterRawInputDevices(RAWINPUTDEVICE[] pRawInputDevices, uint uiNumDevices, uint cbSize);

	[DllImport("user32.dll")]
	public static extern uint GetRawInputData(IntPtr hRawInput, uint uiCommand, IntPtr pData, ref uint pcbSize, uint cbSizeHeader);

	// 필요한 상수 정의
	public const uint RIDEV_INPUTSINK = 0x00000100;
	public const uint RID_INPUT = 0x10000003;
	public const uint WM_INPUT = 0x00FF;
	public static readonly IntPtr HWND_MESSAGE = new IntPtr(-3);

	// 필요한 구조체 정의
	[StructLayout(LayoutKind.Sequential)]
	public struct MSG
	{
		public IntPtr hwnd;
		public uint message;
		public IntPtr wParam;
		public IntPtr lParam;
		public uint time;
		public POINT pt;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct POINT { public int X; public int Y; }

	[StructLayout(LayoutKind.Sequential)]
	public struct RAWINPUTDEVICE
	{
		public ushort usUsagePage;
		public ushort usUsage;
		public uint dwFlags;
		public IntPtr hwndTarget;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RAWINPUTHEADER
	{
		public uint dwType;
		public uint dwSize;
		public IntPtr hDevice;
		public IntPtr wParam;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RAWKEYBOARD
	{
		public ushort MakeCode;
		public ushort Flags;
		public ushort Reserved;
		public ushort VKey;
		public uint Message;
		public uint ExtraInformation;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct RAWINPUT
	{
		[FieldOffset(0)]
		public RAWINPUTHEADER header;
		[FieldOffset(24)]
		public RAWKEYBOARD keyboard;
	}
}