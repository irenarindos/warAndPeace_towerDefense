using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerState 
{
	public LevelBase level;
	public float researchCredits;
	private IDictionary<string, float> research;

	public PlayerState()
	{
		LevelBase.init();
		research = new Dictionary<string, float>();
		level = LevelBase.getLevel("infinite1");
	}

	public bool upgradeTech(string name)
	{
		if (researchCredits >= getResearchCost(name))
		{
			researchCredits -= getResearchCost(name);
			research[name] = research[name] + 1;
			return true;
		}
		return false;
	}

	public void initResearch()
	{
		research["BasicLevel"] = 1;
		research["GearLevel"] = 0;
		research["GearMax"] = 1;
		research["ScopeLevel"] = 0;
		research["ScopeMax"] = 0;
		research["DynamiteLevel"] = 0;
		research["DynamiteMax"] = 0;
		research["SteamLevel"] = 0;
		research["SteamMax"] = 0;
		research["BombLevel"] = 0;
		research["BombMax"] = 1;
		research["EnergyDrainLevel"] = 0;
		research["EnergyDrainMax"] = 0;
	}

	public float getResearch(string name)
	{
		if (research.ContainsKey(name)) {
			return research[name];
		}
		return -1;
	}

	public float getResearchCost(string name)
	{
		float result = Mathf.Pow(2, research[name]);
		if (name.Contains("Level")) result = result*result;
		return result*35;
	}

	public void setResearch(string name, float value)
	{
		research[name] = value;
	}
}


public class MainMenu : MonoBehaviour {
	public static PlayerState instance;


	// Use this for initialization
	void Start () {
		init ();
	}

	public void init()
	{
		instance = new PlayerState();
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartInfiniteMode()
	{
		instance.level = LevelBase.getLevel("infinite1");
		Application.LoadLevel(1);
	}

	public void StartCampaignMode()
	{
		instance.level = LevelBase.getLevel("level1");
		instance.initResearch();
		Application.LoadLevel(1);
	}
}
