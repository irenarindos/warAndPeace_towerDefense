using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LabBehavior : MonoBehaviour {

	public Text tooltip;
	public Text resources;

	// Use this for initialization
	void Start () {
		updateLevels();
	}
	
	// Update is called once per frame
	void Update () {
		resources.text = "Research credits: " + MainMenu.instance.researchCredits;
	}

	public void updateLevels()
	{
		string[] names = {"BasicLevel", "BombLevel", "SteamLevel", "ScopeLevel", "GearLevel", "DynamiteLevel",
			"BombMax", "SteamMax", "ScopeMax", "GearMax", "DynamiteMax"};
		foreach (string name in names)
		{
			GameObject obj = GameObject.Find("Canvas/" + name);
			obj.GetComponent<Text>().text = MainMenu.instance.getResearch(name).ToString();

		}
	}

	public void playNextLevel()
	{
		if (MainMenu.instance.level.getSuccessor() != "")
		{
		    MainMenu.instance.level = LevelBase.getLevel(MainMenu.instance.level.getSuccessor());
			Application.LoadLevel(1);
		}
	}

	public void upgradeTech(string name)
	{
		MainMenu.instance.upgradeTech(name);
		updateLevels();
	}

	public void showTooltip(string name)
	{
		tooltip.text = "Upgrade cost: " + Mathf.Round(MainMenu.instance.getResearchCost(name)) + " research credits";
	}

	public void hideTooltip()
	{
		tooltip.text = "";
	}

	public void showTooltipLevel()
	{
		tooltip.text = "Play next level";
		if (MainMenu.instance.level.getSuccessor() == "")
		{
			tooltip.text = "No more levels! You won!";
		}
	}
}
