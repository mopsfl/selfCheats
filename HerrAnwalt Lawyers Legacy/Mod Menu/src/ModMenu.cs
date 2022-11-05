using System;
using UnityEngine;

namespace ModMenu
{
	// Token: 0x020000F0 RID: 240
	public class ModMenu : MonoBehaviour
	{
		// Token: 0x060004E1 RID: 1249
		public static void DrawMenu()
		{
			GUIStyle guistyle = new GUIStyle(GUI.skin.label);
			Texture2D background = new Texture2D(100, 30);
			guistyle.normal.background = background;
			guistyle.alignment = TextAnchor.MiddleCenter;
			guistyle.normal.textColor = Color.black;
			if (Application.version != ModMenu.modmenuVersion)
			{
				Debug.Log("MOD MENU NOT COMPATIBLE FOR THIS GAME VERSION!");
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
				GUIStyle style = new GUIStyle(GUI.skin.box);
				GUI.Box(new Rect(0f, 250f, 400f, 400f), ModMenu.Title, style);
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
				ModMenu.ui.healCompletly();
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

		// Token: 0x060004E3 RID: 1251
		static ModMenu()
		{
			ModMenu.standardMaxHealth = ModMenu.ui.maxHealth;
			ModMenu.Player = GameObject.FindGameObjectWithTag("Player");
			ModMenu.plrControl = UnityEngine.Object.FindObjectOfType<PlayerControl>();
			ModMenu.standardMovementSpeed = ModMenu.plrControl.movementSpeed;
			ModMenu.standardJumpTimer = ModMenu.plrControl.jumpTimerSet;
		}

		// Token: 0x040004C7 RID: 1223
		public static string Title = "HerrAnwalt: Lawyers Legacy Mod Menu - by mopsfl";

		// Token: 0x040004C8 RID: 1224
		public static bool MenuVisible = false;

		// Token: 0x040004C9 RID: 1225
		public static PlayerControl plrControl;

		// Token: 0x040004CA RID: 1226
		public static float standardMovementSpeed;

		// Token: 0x040004CB RID: 1227
		public static SprungBrett jumpPad;

		// Token: 0x040004CC RID: 1228
		public static bool speedhack = false;

		// Token: 0x040004CD RID: 1229
		public static string speedhackString = "Speed Hack - OFF";

		// Token: 0x040004CE RID: 1230
		public static GameObject Player;

		// Token: 0x040004CF RID: 1231
		public static string healString = "Max Health";

		// Token: 0x040004D0 RID: 1232
		public static string godModeString = "Infinite Health - OFF";

		// Token: 0x040004D1 RID: 1233
		public static bool godMode = false;

		// Token: 0x040004D2 RID: 1234
		public static int standardMaxHealth;

		// Token: 0x040004D3 RID: 1235
		public static AnwaltUIManager ui = UnityEngine.Object.FindObjectOfType<AnwaltUIManager>();

		// Token: 0x040004D4 RID: 1236
		public static bool infiniteJump;

		// Token: 0x040004D5 RID: 1237
		public static string infiniteJumpString = "Infinite Jump - OFF";

		// Token: 0x040004D6 RID: 1238
		public static float standardJumpTimer;

		// Token: 0x040004D7 RID: 1239
		private static string modmenuVersion = "0.2";
	}
}
