//BY MOPSFL

using System;
using UnityEngine;

namespace ModMenu
{
	// Token: 0x020005CE RID: 1486
	public class ModMenu : MonoBehaviour
	{
		// Token: 0x06001E68 RID: 7784
		public static void DrawMenu()
		{
			GUIStyle guistyle = new GUIStyle(GUI.skin.label);
			Texture2D background = new Texture2D(100, 30);
			guistyle.normal.background = background;
			guistyle.alignment = TextAnchor.MiddleCenter;
			guistyle.normal.textColor = Color.black;
			if (GUI.Button(new Rect((float)(Screen.width / 2 - 200), (float)(Screen.height - 30), 400f, 20f), "Mod Menu", guistyle))
			{
				ModMenu.MenuVisible = !ModMenu.MenuVisible;
				Debug.Log(ModMenu.MenuVisible);
			}
			if (ModMenu.MenuVisible)
			{
				GUIStyle style = new GUIStyle(GUI.skin.box);
				GUI.Box(new Rect(0f, 0f, 400f, 400f), ModMenu.Title, style);
				if (GUI.Button(new Rect(0f, 30f, 400f, 20f), ModMenu.superSpeedString, guistyle))
				{
					ModMenu.superSpeed = !ModMenu.superSpeed;
					if (ModMenu.superSpeed)
					{
						ModMenu.superSpeedString = "Super Speed  - ON";
						Singleton<PlayerController>.Instance.walkSpeed = 15f;
					}
					else
					{
						ModMenu.superSpeedString = "Super Speed  - OFF";
						Singleton<PlayerController>.Instance.walkSpeed = 4f;
					}
				}
				if (GUI.Button(new Rect(0f, 60f, 400f, 20f), ModMenu.strongFlashlightString, guistyle))
				{
					ModMenu.flashlightdistance = Singleton<FlashlightItem>.Instance.LightObject.range;
					ModMenu.strongFlashlight = !ModMenu.strongFlashlight;
					if (ModMenu.strongFlashlight)
					{
						ModMenu.strongFlashlightString = "Strong Flashlight - ON";
						Singleton<FlashlightItem>.Instance.LightObject.intensity = 5f;
						Singleton<FlashlightItem>.Instance.LightObject.range = 1000f;
						return;
					}
					ModMenu.strongFlashlightString = "Strong Flashlight - OFF";
					Singleton<FlashlightItem>.Instance.LightObject.intensity = 0.7f;
					Singleton<FlashlightItem>.Instance.LightObject.range = ModMenu.flashlightdistance;
				}
				if (GUI.Button(new Rect(0f, 90f, 400f, 20f), "Collect All Items", guistyle))
				{
					for (int i = 0; i < 100; i++)
					{
						Singleton<ItemCollecter>.Instance.addPresent();
						Singleton<ItemCollecter>.Instance.addPoop();
						Singleton<ItemCollecter>.Instance.addCards();
						Singleton<ItemCollecter>.Instance.addBags();
					}
				}
				if (GUI.Button(new Rect(0f, 120f, 400f, 20f), ModMenu.godModeString, guistyle))
				{
					ModMenu.godMode = !ModMenu.godMode;
					if (ModMenu.godMode)
					{
						ModMenu.godModeString = "Godmode - ON";
						Singleton<HealthManager>.Instance.maximumHealth = 1E+15f;
						Singleton<HealthManager>.Instance.Health = Singleton<HealthManager>.Instance.maximumHealth;
					}
					else
					{
						ModMenu.godModeString = "Godmode - OFF";
						Singleton<HealthManager>.Instance.maximumHealth = 200f;
						Singleton<HealthManager>.Instance.Health = 100f;
					}
				}
			}
			Debug.Log("[Mod Menu]: Initalized RUSSIAPHOBIA Mod Menu by mopsfl.");
		}

		// Token: 0x06001E6A RID: 7786
		static ModMenu()
		{
			ModMenu.superSpeedString = "Super Speed - OFF";
			ModMenu.strongFlashlightString = "Strong Flashlight - OFF";
		}

		// Token: 0x04002A04 RID: 10756
		public static string Title = "RUSSIAPHOBIA Mod Menu - by mopsfl";

		// Token: 0x04002A05 RID: 10757
		public static bool MenuVisible = false;

		// Token: 0x04002A06 RID: 10758
		public static bool superSpeed = false;

		// Token: 0x04002A07 RID: 10759
		public static string superSpeedString;

		// Token: 0x04002A08 RID: 10760
		public static bool strongFlashlight = false;

		// Token: 0x04002A09 RID: 10761
		public static string strongFlashlightString;

		// Token: 0x04002A0A RID: 10762
		public static float flashlightdistance;

		// Token: 0x04002A0B RID: 10763
		public static bool godMode = false;

		// Token: 0x04002A0C RID: 10764
		public static string godModeString = "Godmode - OFF";
	}
}
