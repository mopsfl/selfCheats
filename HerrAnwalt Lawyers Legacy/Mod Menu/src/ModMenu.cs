using System;
using UnityEngine;

namespace ModMenu
{
	// Token: 0x020000F4 RID: 244
	public class ModMenu : MonoBehaviour
	{
		// Token: 0x060004F9 RID: 1273
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
			ModMenu.modmenuVersion = "0.21";
			ModMenu.standardMaxHealth = ModMenu.ui.maxHealth;
			ModMenu.standardMovementSpeed = ModMenu.plrControl.movementSpeed;
			ModMenu.standardJumpTimer = ModMenu.plrControl.jumpTimerSet;
			ModMenu.Title = "HerrAnwalt: Lawyers Legacy Mod Menu v." + ModMenu.modmenuVersion;
		}

		// Token: 0x060004FA RID: 1274
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

		// Token: 0x060004FB RID: 1275
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
			if (!ModMenu.plrAttacks)
			{
				ModMenu.plrAttacks = UnityEngine.Object.FindObjectOfType<PlayerAttacks>();
				return;
			}
		}

		// Token: 0x060004FC RID: 1276
		public static void LoadMenu()
		{
			Debug.Log("LOAD MENU");
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

		// Token: 0x060004FD RID: 1277
		public static void InitMenu(int windowID)
		{
			Debug.Log("INIT MENU");
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
			Debug.Log(ModMenu.windowRect.position);
		}

		// Token: 0x040004DB RID: 1243
		public static string Title;

		// Token: 0x040004DC RID: 1244
		public static bool MenuVisible;

		// Token: 0x040004DD RID: 1245
		public static PlayerControl plrControl;

		// Token: 0x040004DE RID: 1246
		public static float standardMovementSpeed;

		// Token: 0x040004DF RID: 1247
		public static SprungBrett jumpPad;

		// Token: 0x040004E0 RID: 1248
		public static bool speedhack;

		// Token: 0x040004E1 RID: 1249
		public static string speedhackString;

		// Token: 0x040004E2 RID: 1250
		public static GameObject Player;

		// Token: 0x040004E3 RID: 1251
		public static string healString;

		// Token: 0x040004E4 RID: 1252
		public static string godModeString;

		// Token: 0x040004E5 RID: 1253
		public static bool godMode;

		// Token: 0x040004E6 RID: 1254
		public static int standardMaxHealth;

		// Token: 0x040004E7 RID: 1255
		public static AnwaltUIManager ui;

		// Token: 0x040004E8 RID: 1256
		public static bool infiniteJump;

		// Token: 0x040004E9 RID: 1257
		public static string infiniteJumpString;

		// Token: 0x040004EA RID: 1258
		public static float standardJumpTimer;

		// Token: 0x040004EB RID: 1259
		private static string modmenuVersion;

		// Token: 0x040004EC RID: 1260
		public static Rect windowRect = new Rect(20f, 250f, 400f, 400f);

		// Token: 0x040004ED RID: 1261
		public static bool instantKill;

		// Token: 0x040004EE RID: 1262
		public static string instantKillString;

		// Token: 0x040004EF RID: 1263
		public static string quickAttackString;

		// Token: 0x040004F0 RID: 1264
		public static bool quickAttack;

		// Token: 0x040004F1 RID: 1265
		public static PlayerAttacks plrAttacks;

		// Token: 0x040004F2 RID: 1266
		public static BodenIstLavaTest blava;

		// Token: 0x040004F3 RID: 1267
		public static string lavaString;

		// Token: 0x040004F4 RID: 1268
		public static bool lava;

		// Token: 0x040004F5 RID: 1269
		public static bool hideUI;

		// Token: 0x04000566 RID: 1382
		public static string scriptVersion;
	}
}
