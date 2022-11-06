using System;
using UnityEngine;

namespace ModMenu
{
	// Token: 0x020000F0 RID: 240
	public class ModMenu : MonoBehaviour
	{
		// Token: 0x060004E1 RID: 1249
		static ModMenu()
		{
			ModMenu.MenuVisible = false;
			ModMenu.speedhack = false;
			ModMenu.speedhackString = "Speed Hack - OFF";
			ModMenu.healString = "Max Health";
			ModMenu.godModeString = "Infinite Health - OFF";
			ModMenu.godMode = false;
			ModMenu.ui = UnityEngine.Object.FindObjectOfType<AnwaltUIManager>();
			ModMenu.infiniteJumpString = "Infinite Jump - OFF";
			ModMenu.modmenuVersion = "0.2";
			ModMenu.standardMaxHealth = ModMenu.ui.maxHealth;
			ModMenu.Player = GameObject.FindGameObjectWithTag("Player");
			ModMenu.plrControl = UnityEngine.Object.FindObjectOfType<PlayerControl>();
			ModMenu.standardMovementSpeed = ModMenu.plrControl.movementSpeed;
			ModMenu.standardJumpTimer = ModMenu.plrControl.jumpTimerSet;
			ModMenu.Title = "HerrAnwalt: Lawyers Legacy Mod Menu v." + ModMenu.modmenuVersion;
		}

		// Token: 0x060004E2 RID: 1250
		private static Texture2D MakeTex(int width, int height, Color col)
		{
			Color[] array = new Color[width * height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = col;
			}
			Texture2D texture2D = new Texture2D(width, height);
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x060004E3 RID: 1251
		public static void DoMyWindow(int windowID)
		{
			GUIStyle guistyle = new GUIStyle(GUI.skin.label);
			guistyle.normal.background = ModMenu.MakeTex(2, 2, new Color(50f, 0f, 0f, 1f));
			guistyle.normal.textColor = Color.white;
			guistyle.alignment = TextAnchor.UpperCenter;
			guistyle.border = new RectOffset(2, 2, 2, 2);
			if (GUI.Button(new Rect(10f, 30f, 380f, 20f), ModMenu.speedhackString, guistyle))
			{
				ModMenu.speedhack = !ModMenu.speedhack;
				if (ModMenu.speedhack)
				{
					ModMenu.speedhackString = "Speed Hack - ON";
					ModMenu.plrControl.movementSpeed = ModMenu.standardMovementSpeed * 3f;
					return;
				}
				ModMenu.speedhackString = "Speed Hack - OFF";
				ModMenu.plrControl.movementSpeed = ModMenu.standardMovementSpeed;
			}
			if (GUI.Button(new Rect(10f, 60f, 185f, 20f), ModMenu.godModeString, guistyle))
			{
				ModMenu.godMode = !ModMenu.godMode;
				if (ModMenu.godMode)
				{
					ModMenu.godModeString = "Infinite Health - ON";
					return;
				}
				ModMenu.godModeString = "Infinite Health - OFF";
			}
			if (!ModMenu.godMode && GUI.Button(new Rect(205f, 60f, 185f, 20f), ModMenu.healString, guistyle))
			{
				ModMenu.healString = "Max Health";
				ModMenu.ui.healCompletly();
			}
			if (GUI.Button(new Rect(10f, 90f, 380f, 20f), ModMenu.infiniteJumpString, guistyle))
			{
				ModMenu.infiniteJump = !ModMenu.infiniteJump;
				if (ModMenu.infiniteJump)
				{
					ModMenu.infiniteJumpString = "Infinite Jump - ON";
					ModMenu.plrControl.jumpTimerSet = 10f;
					ModMenu.plrControl.isGrounded = true;
					return;
				}
				ModMenu.infiniteJumpString = "Infinite Jump - OFF";
				ModMenu.plrControl.jumpTimerSet = ModMenu.standardJumpTimer;
				ModMenu.plrControl.isGrounded = true;
			}
			GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
		}

		// Token: 0x060004E4 RID: 1252
		public static void LoadOldMenu()
		{
			GUIStyle guistyle = new GUIStyle(GUI.skin.label);
			guistyle.normal.background = ModMenu.MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 1f));
			guistyle.normal.textColor = Color.white;
			guistyle.alignment = TextAnchor.UpperCenter;
			guistyle.border = new RectOffset(2, 2, 2, 2);
			if (Application.version != ModMenu.modmenuVersion)
			{
				GUI.Button(new Rect(0f, (float)(Screen.height - 30), (float)Screen.width, 20f), string.Concat(new string[]
				{
					"This Mod Menu version (",
					ModMenu.modmenuVersion,
					") is not compatible for this game version (",
					Application.version,
					")! Update to mod menu version ",
					Application.version
				}), guistyle);
				return;
			}
			if (GUI.Button(new Rect((float)(Screen.width / 2 - 200), (float)(Screen.height - 30), 400f, 20f), "Mod Menu", guistyle))
			{
				ModMenu.MenuVisible = !ModMenu.MenuVisible;
				Debug.Log(ModMenu.MenuVisible);
			}
			if (ModMenu.MenuVisible)
			{
				GUIStyle guistyle2 = new GUIStyle(GUI.skin.box);
				new Texture2D(100, 30);
				guistyle2.normal.background = ModMenu.MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 1f));
				GUI.Box(new Rect(0f, 250f, 400f, 400f), ModMenu.Title, guistyle2);
				if (GUI.Button(new Rect(0f, 280f, 400f, 20f), ModMenu.speedhackString, guistyle))
				{
					ModMenu.speedhack = !ModMenu.speedhack;
					if (ModMenu.speedhack)
					{
						ModMenu.speedhackString = "Speed Hack - ON";
						ModMenu.plrControl.movementSpeed = ModMenu.standardMovementSpeed * 3f;
						return;
					}
					ModMenu.speedhackString = "Speed Hack - OFF";
					ModMenu.plrControl.movementSpeed = ModMenu.standardMovementSpeed;
				}
				if (GUI.Button(new Rect(0f, 310f, 195f, 20f), ModMenu.godModeString, guistyle))
				{
					ModMenu.godMode = !ModMenu.godMode;
					if (ModMenu.godMode)
					{
						ModMenu.godModeString = "Infinite Health - ON";
						return;
					}
					ModMenu.godModeString = "Infinite Health - OFF";
				}
				if (!ModMenu.godMode && GUI.Button(new Rect(205f, 310f, 195f, 20f), ModMenu.healString, guistyle))
				{
					ModMenu.healString = "Max Health";
					ModMenu.ui.healCompletly();
				}
				if (GUI.Button(new Rect(0f, 340f, 400f, 20f), ModMenu.infiniteJumpString, guistyle))
				{
					ModMenu.infiniteJump = !ModMenu.infiniteJump;
					if (ModMenu.infiniteJump)
					{
						ModMenu.infiniteJumpString = "Infinite Jump - ON";
						ModMenu.plrControl.jumpTimerSet = 10f;
						ModMenu.plrControl.isGrounded = true;
						return;
					}
					ModMenu.infiniteJumpString = "Infinite Jump - OFF";
					ModMenu.plrControl.jumpTimerSet = ModMenu.standardJumpTimer;
					ModMenu.plrControl.isGrounded = true;
				}
			}
			if (ModMenu.godMode)
			{
				if (ModMenu.ui)
				{
					ModMenu.ui.healCompletly();
				}
				else
				{
					UnityEngine.Object.FindObjectOfType<AnwaltUIManager>();
				}
			}
			if (ModMenu.infiniteJump)
			{
				ModMenu.plrControl.isGrounded = true;
			}
			if (ModMenu.speedhack)
			{
				ModMenu.plrControl.movementSpeed = ModMenu.standardMovementSpeed * 3f;
			}
			if (!ModMenu.plrControl)
			{
				ModMenu.plrControl = UnityEngine.Object.FindObjectOfType<PlayerControl>();
			}
			if (!ModMenu.Player)
			{
				ModMenu.Player = GameObject.FindGameObjectWithTag("Player");
			}
			if (!ModMenu.ui)
			{
				ModMenu.ui = UnityEngine.Object.FindObjectOfType<AnwaltUIManager>();
			}
		}

		// Token: 0x060004E5 RID: 1253
		public static void LoadMenu()
		{
			GUIStyle guistyle = new GUIStyle(GUI.skin.label);
			guistyle.normal.background = ModMenu.MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 1f));
			guistyle.normal.textColor = Color.white;
			guistyle.alignment = TextAnchor.UpperCenter;
			guistyle.border = new RectOffset(2, 2, 2, 2);
			if (Application.version != ModMenu.modmenuVersion)
			{
				GUI.Button(new Rect(0f, (float)(Screen.height - 30), (float)Screen.width, 20f), string.Concat(new string[]
				{
					"This Mod Menu version (",
					ModMenu.modmenuVersion,
					") is not compatible for this game version (",
					Application.version,
					")! Update to mod menu version ",
					Application.version
				}), guistyle);
				return;
			}
			ModMenu.windowRect = GUI.Window(0, ModMenu.windowRect, new GUI.WindowFunction(ModMenu.DoMyWindow), ModMenu.Title, guistyle);
			if (ModMenu.godMode)
			{
				if (ModMenu.ui)
				{
					ModMenu.ui.healCompletly();
				}
				else
				{
					UnityEngine.Object.FindObjectOfType<AnwaltUIManager>();
				}
			}
			if (ModMenu.infiniteJump)
			{
				ModMenu.plrControl.isGrounded = true;
			}
			if (ModMenu.speedhack)
			{
				ModMenu.plrControl.movementSpeed = ModMenu.standardMovementSpeed * 3f;
			}
			if (!ModMenu.plrControl)
			{
				ModMenu.plrControl = UnityEngine.Object.FindObjectOfType<PlayerControl>();
			}
			if (!ModMenu.Player)
			{
				ModMenu.Player = GameObject.FindGameObjectWithTag("Player");
			}
			if (!ModMenu.ui)
			{
				ModMenu.ui = UnityEngine.Object.FindObjectOfType<AnwaltUIManager>();
			}
		}

		// Token: 0x040004C7 RID: 1223
		public static string Title;

		// Token: 0x040004C8 RID: 1224
		public static bool MenuVisible;

		// Token: 0x040004C9 RID: 1225
		public static PlayerControl plrControl;

		// Token: 0x040004CA RID: 1226
		public static float standardMovementSpeed;

		// Token: 0x040004CB RID: 1227
		public static SprungBrett jumpPad;

		// Token: 0x040004CC RID: 1228
		public static bool speedhack;

		// Token: 0x040004CD RID: 1229
		public static string speedhackString;

		// Token: 0x040004CE RID: 1230
		public static GameObject Player;

		// Token: 0x040004CF RID: 1231
		public static string healString;

		// Token: 0x040004D0 RID: 1232
		public static string godModeString;

		// Token: 0x040004D1 RID: 1233
		public static bool godMode;

		// Token: 0x040004D2 RID: 1234
		public static int standardMaxHealth;

		// Token: 0x040004D3 RID: 1235
		public static AnwaltUIManager ui;

		// Token: 0x040004D4 RID: 1236
		public static bool infiniteJump;

		// Token: 0x040004D5 RID: 1237
		public static string infiniteJumpString;

		// Token: 0x040004D6 RID: 1238
		public static float standardJumpTimer;

		// Token: 0x040004D7 RID: 1239
		private static string modmenuVersion;

		// Token: 0x040004D8 RID: 1240
		public static Rect windowRect = new Rect(20f, 250f, 400f, 400f);
	}
}
