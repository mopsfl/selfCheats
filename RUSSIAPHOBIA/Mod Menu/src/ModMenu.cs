using System;
using UnityEngine;

namespace ModMenu
{
	// Token: 0x020005CB RID: 1483
	public class ModMenu : MonoBehaviour
	{
		// Token: 0x06001E55 RID: 7765 RVA: 0x000BF7F0 File Offset: 0x000BD9F0
		static ModMenu()
		{
			ModMenu.speedHackString = "Speed Hack - OFF";
			ModMenu.modmenuVersion = "1.0";
			ModMenu.Title = "Game Name Mod Menu v." + ModMenu.modmenuVersion;
			ModMenu.hideUI = false;
			ModMenu.collectAllItemsString = "Collect All Items";
			ModMenu.godMode = false;
			ModMenu.godModeString = "God Mode - OFF";
			ModMenu.standardWalkSpeed = Singleton<PlayerController>.Instance.walkSpeed;
			ModMenu.strongFlashString = "Strong Flashlight - OFF";
			ModMenu.unlockedach = false;
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x000BF888 File Offset: 0x000BDA88
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

		// Token: 0x06001E57 RID: 7767 RVA: 0x000BF8C8 File Offset: 0x000BDAC8
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
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x000BFAF0 File Offset: 0x000BDCF0
		private static void InitMenu(int windowID)
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
			guistyle2.hover.textColor = Color.white;
			if (GUI.Button(new Rect(10f, 30f, 380f, 20f), ModMenu.speedHackString, guistyle))
			{
				ModMenu.speedHack = !ModMenu.speedHack;
				if (ModMenu.speedHack)
				{
					ModMenu.speedHackString = "Speed Hack - ON";
					Singleton<PlayerController>.Instance.walkSpeed = ModMenu.standardWalkSpeed * 5f;
					return;
				}
				ModMenu.speedHackString = "Speed Hack - OFF";
				Singleton<PlayerController>.Instance.walkSpeed = ModMenu.standardWalkSpeed;
			}
			if (GUI.Button(new Rect(10f, 60f, 380f, 20f), ModMenu.strongFlashString, guistyle))
			{
				ModMenu.strongFlash = !ModMenu.strongFlash;
				if (ModMenu.strongFlash)
				{
					ModMenu.strongFlashString = "Strong Flashlight - ON";
					Singleton<FlashlightItem>.Instance.LightObject.intensity = 3.5f;
					Singleton<FlashlightItem>.Instance.LightObject.range = 1000f;
					return;
				}
				ModMenu.strongFlashString = "Strong Flashlight - OFF";
				Singleton<FlashlightItem>.Instance.LightObject.intensity = ModMenu.standardFlashIntensity;
				Singleton<FlashlightItem>.Instance.LightObject.range = ModMenu.standardFlashRange;
			}
			if (GUI.Button(new Rect(10f, 90f, 380f, 20f), ModMenu.collectAllItemsString, guistyle))
			{
				int maxpresentsNumber = Singleton<ItemCollecter>.Instance.MAXpresentsNumber;
				int maxpoopNumber = Singleton<ItemCollecter>.Instance.MAXpoopNumber;
				int maxcardsNumber = Singleton<ItemCollecter>.Instance.MAXcardsNumber;
				int maxbagsNumber = Singleton<ItemCollecter>.Instance.MAXbagsNumber;
				for (int i = 0; i < maxpresentsNumber; i++)
				{
					Singleton<ItemCollecter>.Instance.addPresent();
				}
				for (int j = 0; j < maxpoopNumber; j++)
				{
					Singleton<ItemCollecter>.Instance.addPoop();
				}
				for (int k = 0; k < maxcardsNumber; k++)
				{
					Singleton<ItemCollecter>.Instance.addCards();
				}
				for (int l = 0; l < maxbagsNumber; l++)
				{
					Singleton<ItemCollecter>.Instance.addBags();
				}
			}
			if (GUI.Button(new Rect(10f, 120f, 380f, 20f), ModMenu.godModeString, guistyle))
			{
				ModMenu.godMode = !ModMenu.godMode;
				if (ModMenu.godMode)
				{
					ModMenu.godModeString = "Godmode - ON";
					Singleton<HealthManager>.Instance.maximumHealth = 100000000f;
					Singleton<HealthManager>.Instance.Health = Singleton<HealthManager>.Instance.maximumHealth;
				}
				else
				{
					ModMenu.godModeString = "Godmode - OFF";
					Singleton<HealthManager>.Instance.maximumHealth = 200f;
					Singleton<HealthManager>.Instance.Health = 100f;
				}
			}
			if (GUI.Button(new Rect(10f, 150f, 380f, 20f), "Unlock all Steam achievements", guistyle))
			{
				for (int m = 1; m <= 100; m++)
				{
					Singleton<ItemCollecter>.Instance.achivs.UnlockAchievement(string.Format("achievement_{0}", m));
				}
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

		// Token: 0x040029F0 RID: 10736
		public static string Title;

		// Token: 0x040029F1 RID: 10737
		public static bool MenuVisible;

		// Token: 0x040029F2 RID: 10738
		private static string modmenuVersion;

		// Token: 0x040029F3 RID: 10739
		public static Rect windowRect = new Rect(20f, 250f, 400f, 400f);

		// Token: 0x040029F4 RID: 10740
		public static bool hideUI;

		// Token: 0x040029F5 RID: 10741
		public static float standardWalkSpeed;

		// Token: 0x040029F6 RID: 10742
		public static string speedHackString;

		// Token: 0x040029F7 RID: 10743
		public static bool speedHack = false;

		// Token: 0x040029F8 RID: 10744
		public static bool strongFlash;

		// Token: 0x040029F9 RID: 10745
		public static string strongFlashString;

		// Token: 0x040029FA RID: 10746
		public static float standardFlashRange;

		// Token: 0x040029FB RID: 10747
		public static float standardFlashIntensity;

		// Token: 0x040029FC RID: 10748
		public static string collectAllItemsString;

		// Token: 0x040029FD RID: 10749
		public static bool godMode;

		// Token: 0x040029FE RID: 10750
		public static string godModeString;

		// Token: 0x040029FF RID: 10751
		public static bool unlockedach;
	}
}
