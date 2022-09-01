using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020000FD RID: 253
public class ItemCollecter : MonoBehaviour
{
	// Token: 0x06000763 RID: 1891
	private void Start()
	{
    //INJECTED CODE
		for (int i = 1; i <= 100; i++)
		{
			this.achivs.UnlockAchievement(string.Format("achievement_{0}", i));
		}
    //END INJECTED CODE
		base.StartCoroutine(this.zalupa1());
	}

	// Token: 0x06000764 RID: 1892
	private IEnumerator zalupa1()
	{
		yield return new WaitForSeconds(0.5f);
		bool flag = this.car;
		if (flag)
		{
			this.car.SetActive(false);
		}
		bool flag2 = this.poopText;
		if (flag2)
		{
			this.poopText.text = string.Format("{0}: {1}/{2}", this.translater.Strings["Feces"].ToString(), this.poopNumber, this.MAXpoopNumber);
		}
		bool flag3 = this.cardsText;
		if (flag3)
		{
			this.cardsText.text = string.Format("{0}: {1}/{2}", this.translater.Strings["flash_drives"].ToString(), this.cardsNumber, this.MAXcardsNumber);
		}
		this.presentsText.text = string.Format("{0}: {1}/{2}", this.translater.Strings["Gifts"].ToString(), this.presentsNumber, this.MAXpresentsNumber);
		this.bagsText.text = string.Format("{0}: {1}/{2}", this.translater.Strings["Money_Bags"].ToString(), this.bagsNumber, this.MAXbagsNumber);
		this.ready = true;
		yield break;
	}

	// Token: 0x06000765 RID: 1893
	private void Update()
	{
		bool flag = this.ready;
		if (flag)
		{
			bool flag2 = this.poopText;
			if (flag2)
			{
				this.poopText.text = string.Format("{0}: {1}/{2}", this.translater.Strings["Feces"].ToString(), this.poopNumber, this.MAXpoopNumber);
			}
			bool flag3 = this.cardsText;
			if (flag3)
			{
				this.cardsText.text = string.Format("{0}: {1}/{2}", this.translater.Strings["flash_drives"].ToString(), this.cardsNumber, this.MAXcardsNumber);
			}
			this.presentsText.text = string.Format("{0}: {1}/{2}", this.translater.Strings["Gifts"].ToString(), this.presentsNumber, this.MAXpresentsNumber);
			this.bagsText.text = string.Format("{0}: {1}/{2}", this.translater.Strings["Money_Bags"].ToString(), this.bagsNumber, this.MAXbagsNumber);
		}
	}

	// Token: 0x06000766 RID: 1894
	public void addPresent()
	{
		this.presentsNumber++;
		this.presentsText.text = string.Format("{0}: {1}/{2}", this.translater.Strings["Gifts"].ToString(), this.presentsNumber, this.MAXpresentsNumber);
		this.checkForEnd();
		this.achivs.UnlockAchievement(string.Format("achievement_{0}", Mathf.Clamp(10 + Mathf.Clamp(this.presentsNumber, 1, 20) + (this.currentScene - 1) * 20, 10, 100)));
	}

	// Token: 0x06000767 RID: 1895
	public void addBags()
	{
		Debug.Log("!!!!!!!!!!!!!!!!!!!!!!" + this.translater.Strings["Money_Bags"]);
		this.bagsNumber++;
		this.bagsText.text = string.Format("{0}: {1}/{2}", this.translater.Strings["Money_Bags"].ToString(), this.bagsNumber, this.MAXbagsNumber);
		this.checkForEnd();
	}

	// Token: 0x06000768 RID: 1896
	public void addPoop()
	{
		this.poopNumber++;
		this.poopText.text = string.Format("{0}: {1}/{2}", this.translater.Strings["Feces"].ToString(), this.poopNumber, this.MAXpoopNumber);
		this.checkForEnd();
	}

	// Token: 0x06000769 RID: 1897
	public void addCards()
	{
		this.cardsNumber++;
		this.cardsText.text = string.Format("{0}: {1}/{2}", this.translater.Strings["flash_drives"].ToString(), this.cardsNumber, this.MAXcardsNumber);
		this.checkForEnd();
	}

	// Token: 0x0600076A RID: 1898
	public bool checkForEnd()
	{
		bool flag = this.cardsNumber >= this.MAXcardsNumber && this.poopNumber >= this.MAXpoopNumber && this.bagsNumber >= this.MAXbagsNumber && this.presentsNumber >= this.MAXpresentsNumber;
		bool result;
		if (flag)
		{
			bool flag2 = this.car;
			if (flag2)
			{
				this.car.SetActive(true);
			}
			bool flag3 = this.returnLabel;
			if (flag3)
			{
				this.returnLabel.SetActive(true);
			}
			this.m_Scene = SceneManager.GetActiveScene();
			this.sceneName = this.m_Scene.name;
			bool flag4 = this.sceneName == "0_home" && this.returnLabel;
			if (flag4)
			{
				this.returnLabel.GetComponent<Text>().text = "Go down the stairs to the exit";
			}
			result = true;
		}
		else
		{
			result = false;
		}
		return result;
	}

	// Token: 0x04000C0F RID: 3087
	public int presentsNumber;

	// Token: 0x04000C10 RID: 3088
	public int MAXpresentsNumber;

	// Token: 0x04000C11 RID: 3089
	public int bagsNumber;

	// Token: 0x04000C12 RID: 3090
	public int MAXbagsNumber;

	// Token: 0x04000C13 RID: 3091
	public int poopNumber;

	// Token: 0x04000C14 RID: 3092
	public int MAXpoopNumber;

	// Token: 0x04000C15 RID: 3093
	public int cardsNumber;

	// Token: 0x04000C16 RID: 3094
	public int MAXcardsNumber;

	// Token: 0x04000C17 RID: 3095
	public Text presentsText;

	// Token: 0x04000C18 RID: 3096
	public Text bagsText;

	// Token: 0x04000C19 RID: 3097
	public Text poopText;

	// Token: 0x04000C1A RID: 3098
	public Text cardsText;

	// Token: 0x04000C1B RID: 3099
	public GameObject returnLabel;

	// Token: 0x04000C1C RID: 3100
	public GameObject car;

	// Token: 0x04000C1D RID: 3101
	public Translater translater;

	// Token: 0x04000C1E RID: 3102
	public int currentScene;

	// Token: 0x04000C1F RID: 3103
	public SteamAchievements achivs;

	// Token: 0x04000C20 RID: 3104
	private Scene m_Scene;

	// Token: 0x04000C21 RID: 3105
	private string sceneName;

	// Token: 0x04000C22 RID: 3106
	private bool ready;
}
