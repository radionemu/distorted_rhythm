using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using ThreadPriority = System.Threading.ThreadPriority;

public class Inputmanager : MonoBehaviour
{
	public bool[] isDown = new bool[5];
	public bool[] isStroke = { false, false };
	public bool[] prevStroke = { false, false };

	public Camera PCcam;
	public Camera MBcam;
	Camera maincam => _ = settingManager.BuildSetting == settingManager.PortMode.Desktop ? PCcam : MBcam;

	public Judge mJudge;
	// Start is called before the first frame update
	Ray ray;
	RaycastHit hit;

	public float threshold = 600f;
	public float deltaThreshold = 60f;
	private int touchindex = 0;
	private Vector2 touchBeganPos;
	private Vector2 touchEndPos;

	float width;
	float height;
	float prevDelta = 0.0f;
	int prevdeltasign = 0;

	float prevTime = 0.0f;

	// --- Multi Thread ---
	[DllImport("user32.dll")]
	public static extern short GetAsyncKeyState(int vKey);
	private ConcurrentQueue<Vector2> mouseInputQueue = new ConcurrentQueue<Vector2>();
	private Thread inputThread;
	private bool isRunning = false;

	private IntPtr windowHandle = IntPtr.Zero;

	public enum VirtualKeycode : Int32
	{
		VK_ALPHA1 = 0x31,
		VK_ALPHA2 = 0x32,
		VK_ALPHA3 = 0x33,
		VK_ALPHA4 = 0x34,
		VK_ALPHA7 = 0x37,
		VK_ALPHA8 = 0x38,
		VK_NUMPAD7 = 0x67,
		VK_NUMPAD8 = 0x68,
	}

	private readonly VirtualKeycode[] keysToTrack = new VirtualKeycode[]
	{
		VirtualKeycode.VK_ALPHA1,
		VirtualKeycode.VK_ALPHA2,
		VirtualKeycode.VK_ALPHA3,
		VirtualKeycode.VK_ALPHA4
	};

	void Awake()
	{
	}

	void Start()
	{
		isRunning = true;
		inputThread = new Thread(InputThreadFunction);

		// 2. 스레드 우선순위 설정
		inputThread.Priority = ThreadPriority.Highest;
		inputThread.IsBackground = true;
		inputThread.Start();
	}


	bool[] isprev = new bool[4];

	bool isprevStrk1 = false;
	bool isprevStrk2 = false;
	bool isprevStrk3 = false;
	bool isprevStrk4 = false;
	private void InputThreadFunction()
	{
		while (isRunning)
		{
			isStroke[0] = false; isStroke[1] = false;
			for (int i = 0; i < 4; i++)
			{
				isDown[i] = (GetAsyncKeyState((int)keysToTrack[i]) & 0x8000) != 0;
			}
			isStroke[0] = (GetAsyncKeyState((int)VirtualKeycode.VK_NUMPAD7) & 0x8000) != 0 || (GetAsyncKeyState((int)VirtualKeycode.VK_ALPHA7) & 0x8000) != 0;
			isStroke[1] = (GetAsyncKeyState((int)VirtualKeycode.VK_NUMPAD8) & 0x8000) != 0 || (GetAsyncKeyState((int)VirtualKeycode.VK_ALPHA8) & 0x8000) != 0;

			bool hasDownStroke1 = GetAsyncKeyState((int)VirtualKeycode.VK_NUMPAD7) == unchecked((short)0x8000);
			bool hasDownStroke2 = GetAsyncKeyState((int)VirtualKeycode.VK_NUMPAD8) == unchecked((short)0x8000);
			bool hasDownStroke3 = GetAsyncKeyState((int)VirtualKeycode.VK_ALPHA7) == unchecked((short)0x8000);
			bool hasDownStroke4 = GetAsyncKeyState((int)VirtualKeycode.VK_ALPHA8) == unchecked((short)0x8000);

			bool final1 = hasDownStroke1 && !isprevStrk1;
			bool final2 = hasDownStroke2 && !isprevStrk2;
			bool final3 = hasDownStroke3 && !isprevStrk3;
			bool final4 = hasDownStroke4 && !isprevStrk4;


			if (final1 || final2 || final3 || final4)
			{
				mJudge.ReqJudgeThread(isDown);
			}

			if ((isprev[0] != isDown[0]) || (isprev[1] != isDown[1]) || (isprev[2] != isDown[2]) || (isprev[3] != isDown[3]))
			{
				mJudge.ReqCharge();
			}

			isprevStrk1 = hasDownStroke1;
			isprevStrk2 = hasDownStroke2;
			isprevStrk3 = hasDownStroke3;
			isprevStrk4 = hasDownStroke4;
			for (int i = 0; i < 4; i++)
			{
				isprev[i] = isDown[i];
			}

			Thread.Sleep(1);
		}
	}

	void OnDestroy()
	{
		isRunning = false;

		if (inputThread != null && inputThread.IsAlive)
		{
			inputThread.Join();
		}
	}

	// Update is called once per frame
	void Update()
	{
		return;
		Array.Fill(isDown, false);
		Array.Fill(isStroke, false);

		if (Input.GetKey(KeyCode.Alpha1))
			isDown[0] = true;
		else
			isDown[0] = false;

		if (Input.GetKey(KeyCode.Alpha2))
			isDown[1] = true;
		else
			isDown[1] = false;

		if (Input.GetKey(KeyCode.Alpha3))
			isDown[2] = true;
		else
			isDown[2] = false;

		if (Input.GetKey(KeyCode.Alpha4))
			isDown[3] = true;
		else
			isDown[3] = false;

		if (Input.GetKey(KeyCode.Keypad7))
			isDown[4] = isStroke[0] = true;
		else
			isDown[4] = isStroke[0] = false;

		if (Input.GetKey(KeyCode.Keypad8))
			isDown[4] = isStroke[1] = true;
		else
			isDown[4] = isStroke[1] = false;

		if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Keypad8))
			mJudge.ReqJudgeThread(isDown);

		if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Alpha3) || Input.GetKeyUp(KeyCode.Alpha4))
			mJudge.ReqCharge();

		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4))
			mJudge.ReqCharge();
	}


}

