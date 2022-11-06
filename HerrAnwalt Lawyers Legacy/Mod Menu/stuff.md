## v.0.2

<code>Scenes:
<code>LangugeSelectionStart</code>
<code>MenuApple</code>
<code>Office</code>
<code>IntroBeta</code>
<code>CutSceneInDerSchule</code>
<code>Sp_00_Tutorial</code>
<code>Sp_01</code>
<code>Sp_02</code>
<code>Sp_03</code>
<code>Sp_04</code>
<code>Sp_05</code>
<code>Sp_06_Lava</code>
<code>Sp_07_Lava</code>
<code>Sp_08_DauerLava</code>
<code>Outro</code>
<code>EndlosFigthing</code></code>


script save skins:
<code>
if (GUI.Button(new Rect(10f, 120f, 380f, 20f), ModMenu.unlockAllSkinsString, guistyle))
			{
				ModMenu.unlockAllSkinsString = ModMenu.unlockAllSkinsString;
				Debug.Log("UNLOCK ALL SKINS.");
				PlayerPrefs.SetInt("CapturedStudent" + ModMenu.skinselec.difficutly.ToString(), 25);
				PlayerPrefs.SetInt("StudentsSaved" + ModMenu.skinselec.difficutly.ToString(), 25);
				PlayerPrefs.SetInt("SuperItemCollectionCount", ModMenu.skinselec.superItemRequirement);
				PlayerPrefs.SetInt("BestScoreFightingMode", ModMenu.skinselec.EndlessFigthingRequirement);
				PlayerPrefs.SetInt("StudentsSaved" + ModMenu.skinselec.difficutly.ToString(), ModMenu.skinselec.SavedStudentsRequirement);
				ModMenu.skinselec.studentsSaved = ModMenu.skinselec.SavedStudentsRequirement;
			}
</code>
