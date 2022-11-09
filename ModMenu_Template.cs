using System;
using UnityEngine;

namespace ModMenu
{
	// Token: 0x020005CA RID: 1482
	public class ModMenu : MonoBehaviour
	{
		// Token: 0x06001E4E RID: 7758
		static ModMenu()
		{
			ModMenu.test = false;
			ModMenu.testString = "Test - OFF";
			ModMenu.modmenuVersion = "1.0";
			ModMenu.Title = "Game Name Mod Menu v." + ModMenu.modmenuVersion;
			ModMenu.hideUI = false;
		}

		// Token: 0x06001E4F RID: 7759
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

		// Token: 0x06001E50 RID: 7760
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
			Debug.Log(Application.version);
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

		// Token: 0x06001E51 RID: 7761
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
			if (GUI.Button(new Rect(10f, 30f, 380f, 20f), ModMenu.testString, guistyle))
			{
				ModMenu.test = !ModMenu.test;
				if (ModMenu.test)
				{
					ModMenu.testString = "Test - ON";
					Debug.Log("Test cheat enabled");
					return;
				}
				ModMenu.testString = "Test - OFF";
				Debug.Log("Test cheat disabled");
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

		// Token: 0x06001E52 RID: 7762
		public ModMenu()
		{
		}

		// Token: 0x040029ED RID: 10733
		public static string Title;

		// Token: 0x040029EE RID: 10734
		public static bool MenuVisible;

		// Token: 0x040029EF RID: 10735
		public static string testString;

		// Token: 0x040029F0 RID: 10736
		private static string modmenuVersion;

		// Token: 0x040029F1 RID: 10737
		public static bool test;

		// Token: 0x040029F2 RID: 10738
		public static Rect windowRect = new Rect(20f, 250f, 400f, 400f);

		// Token: 0x040029F3 RID: 10739
		public static bool hideUI;
	}
}
