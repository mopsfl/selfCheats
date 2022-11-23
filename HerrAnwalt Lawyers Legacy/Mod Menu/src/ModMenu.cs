using System;
using UnityEngine;

namespace ModMenu
{
	// Token: 0x0200011B RID: 283
	public class ModMenu : MonoBehaviour
	{
		// Token: 0x06000846 RID: 2118
		static ModMenu()
		{
			ModMenu.MenuVisible = false;
			ModMenu.speedhack = false;
			ModMenu.instantKill = false;
			ModMenu.quickAttack = false;
			ModMenu.godMode = false;
			ModMenu.lava = false;
			ModMenu.hideUI = false;
			ModMenu.speedhackString = "Speed Hack - OFF";
			ModMenu.healString = "Max Health - OFF";
			ModMenu.godModeString = "Infinite Health - OFF";
			ModMenu.infiniteJumpString = "Infinite Jump - OFF";
			ModMenu.quickAttackString = "No Attack Cooldown - OFF";
			ModMenu.lavaString = "Ignore Lava - OFF";
			ModMenu.plrAttacks = UnityEngine.Object.FindObjectOfType<PlayerAttacks>();
			ModMenu.ui = UnityEngine.Object.FindObjectOfType<AnwaltUIManager>();
			ModMenu.Player = GameObject.FindGameObjectWithTag("Player");
			ModMenu.plrControl = UnityEngine.Object.FindObjectOfType<PlayerControl>();
			ModMenu.blava = UnityEngine.Object.FindObjectOfType<BodenIstLavaTest>();
			ModMenu.modmenuVersion = "0.223";
			ModMenu.standardMaxHealth = ModMenu.ui.maxHealth;
			ModMenu.standardMovementSpeed = ModMenu.plrControl.movementSpeed;
			ModMenu.standardJumpTimer = ModMenu.plrControl.jumpTimerSet;
			ModMenu.Title = "HerrAnwalt: Lawyers Legacy Mod Menu v." + ModMenu.modmenuVersion;
		}

		// Token: 0x06000847 RID: 2119
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

		// Token: 0x06000848 RID: 2120
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
			if (!ModMenu.plrAttacks)
			{
				ModMenu.plrAttacks = UnityEngine.Object.FindObjectOfType<PlayerAttacks>();
				return;
			}
		}

		// Token: 0x06000849 RID: 2121
		public static void LoadMenu()
		{
			GUIStyle guistyle = new GUIStyle(GUI.skin.label);
			guistyle.normal.background = ModMenu.MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 1f));
			guistyle.normal.textColor = Color.white;
			guistyle.alignment = TextAnchor.UpperCenter;
			guistyle.border = new RectOffset(2, 2, 2, 2);
			GUIStyle guistyle2 = new GUIStyle(GUI.skin.label);
			guistyle2.normal.background = ModMenu.MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 1f));
			guistyle2.normal.textColor = Color.white;
			guistyle2.alignment = TextAnchor.MiddleCenter;
			guistyle2.border = new RectOffset(2, 2, 2, 2);
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
			if (ModMenu.hideUI && GUI.Button(new Rect((float)(Screen.width - 80), (float)(Screen.height - 30), 65f, 20f), "Mod Menu", guistyle2))
			{
				ModMenu.hideUI = !ModMenu.hideUI;
				if (!ModMenu.hideUI)
				{
					ModMenu.windowRect.height = 400f;
					ModMenu.windowRect.width = 400f;
				}
			}
			ModMenu.windowRect = GUI.Window(0, ModMenu.windowRect, new GUI.WindowFunction(ModMenu.InitMenu), ModMenu.Title, guistyle);
			ModMenu.windowRect.x = Mathf.Clamp(ModMenu.windowRect.x, 0f, (float)Screen.width - ModMenu.windowRect.width);
			ModMenu.windowRect.y = Mathf.Clamp(ModMenu.windowRect.y, 0f, (float)Screen.height - ModMenu.windowRect.height);
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
			if (!ModMenu.plrAttacks)
			{
				ModMenu.plrAttacks = UnityEngine.Object.FindObjectOfType<PlayerAttacks>();
			}
			if (!ModMenu.blava)
			{
				ModMenu.blava = UnityEngine.Object.FindObjectOfType<BodenIstLavaTest>();
			}
			if (ModMenu.quickAttack)
			{
				ModMenu.plrAttacks.attackCooldown = new float[5];
				ModMenu.plrAttacks.attackEnabled = new bool[]
				{
					true,
					true,
					true,
					true,
					true
				};
			}
		}

		// Token: 0x0600084A RID: 2122
		public static void InitMenu(int windowID)
		{
			GUIStyle guistyle = new GUIStyle(GUI.skin.label);
			guistyle.normal.background = ModMenu.MakeTex(2, 2, new Color(50f, 0f, 0f, 1f));
			guistyle.normal.textColor = Color.white;
			guistyle.alignment = TextAnchor.UpperCenter;
			guistyle.border = new RectOffset(2, 2, 2, 2);
			guistyle.border.Add(new Rect(2f, 2f, 2f, 2f));
			guistyle.hover.background = ModMenu.MakeTex(2, 2, new Color(0f, 0f, 25f, 1f));
			GUIStyle guistyle2 = new GUIStyle(GUI.skin.label);
			guistyle2.normal.background = ModMenu.MakeTex(2, 2, new Color(0.2f, 0.2f, 0.2f, 1f));
			guistyle2.normal.textColor = Color.white;
			guistyle2.alignment = TextAnchor.UpperCenter;
			guistyle2.border = new RectOffset(2, 2, 2, 2);
			guistyle2.border.Add(new Rect(2f, 2f, 2f, 2f));
			guistyle2.hover.background = ModMenu.MakeTex(2, 2, new Color(0.3f, 0.3f, 0.3f, 1f));
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
			if (GUI.Button(new Rect(10f, 120f, 380f, 20f), ModMenu.quickAttackString, guistyle))
			{
				ModMenu.quickAttack = !ModMenu.quickAttack;
				if (ModMenu.quickAttack)
				{
					ModMenu.quickAttackString = "No Attack Cooldown - ON";
					ModMenu.plrAttacks.attackCooldown = new float[5];
					ModMenu.plrAttacks.attackEnabled = new bool[]
					{
						true,
						true,
						true,
						true,
						true
					};
					return;
				}
				ModMenu.quickAttackString = "No Attack Cooldown - OFF";
				ModMenu.plrAttacks.attackCooldown = new float[]
				{
					0.6f,
					0.6f,
					0.6f,
					0.6f,
					0.6f
				};
			}
			if (GUI.Button(new Rect(10f, 150f, 380f, 20f), ModMenu.lavaString, guistyle))
			{
				ModMenu.lava = !ModMenu.lava;
				if (ModMenu.lava)
				{
					ModMenu.lavaString = "Ignore Lava - ON";
					ModMenu.blava.lavaRenderer.enabled = false;
					return;
				}
				ModMenu.lavaString = "Ignore Lava - OFF";
			}
			GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
			if (GUI.Button(new Rect(10f, 370f, 380f, 20f), "Hide", guistyle2))
			{
				ModMenu.hideUI = !ModMenu.hideUI;
				if (ModMenu.hideUI)
				{
					ModMenu.windowRect.height = 0f;
					ModMenu.windowRect.width = 0f;
					ModMenu.windowRect.position = new Vector2((float)Screen.width, (float)Screen.height);
					return;
				}
				ModMenu.windowRect.height = 400f;
				ModMenu.windowRect.height = 400f;
			}
		}

		// Token: 0x040005A8 RID: 1448
		public static string Title;

		// Token: 0x040005A9 RID: 1449
		public static bool MenuVisible;

		// Token: 0x040005AA RID: 1450
		public static PlayerControl plrControl;

		// Token: 0x040005AB RID: 1451
		public static float standardMovementSpeed;

		// Token: 0x040005AC RID: 1452
		public static SprungBrett jumpPad;

		// Token: 0x040005AD RID: 1453
		public static bool speedhack;

		// Token: 0x040005AE RID: 1454
		public static string speedhackString;

		// Token: 0x040005AF RID: 1455
		public static GameObject Player;

		// Token: 0x040005B0 RID: 1456
		public static string healString;

		// Token: 0x040005B1 RID: 1457
		public static string godModeString;

		// Token: 0x040005B2 RID: 1458
		public static bool godMode;

		// Token: 0x040005B3 RID: 1459
		public static int standardMaxHealth;

		// Token: 0x040005B4 RID: 1460
		public static AnwaltUIManager ui;

		// Token: 0x040005B5 RID: 1461
		public static bool infiniteJump;

		// Token: 0x040005B6 RID: 1462
		public static string infiniteJumpString;

		// Token: 0x040005B7 RID: 1463
		public static float standardJumpTimer;

		// Token: 0x040005B8 RID: 1464
		private static string modmenuVersion;

		// Token: 0x040005B9 RID: 1465
		public static Rect windowRect = new Rect(20f, 250f, 400f, 400f);

		// Token: 0x040005BA RID: 1466
		public static bool instantKill;

		// Token: 0x040005BB RID: 1467
		public static string instantKillString;

		// Token: 0x040005BC RID: 1468
		public static string quickAttackString;

		// Token: 0x040005BD RID: 1469
		public static bool quickAttack;

		// Token: 0x040005BE RID: 1470
		public static PlayerAttacks plrAttacks;

		// Token: 0x040005BF RID: 1471
		public static BodenIstLavaTest blava;

		// Token: 0x040005C0 RID: 1472
		public static string lavaString;

		// Token: 0x040005C1 RID: 1473
		public static bool lava;

		// Token: 0x040005C2 RID: 1474
		public static bool hideUI;

		// Token: 0x040005C3 RID: 1475
		public static string scriptVersion;
	}
}
