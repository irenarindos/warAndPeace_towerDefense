﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


public class MapBehavior : MonoBehaviour {

	private IList<Vector2> waypoints;
	private IList<TowerBehavior> towers;
	public IList<Creep> creeps;
	int wave;
	public int spawnedCreeps = 0;
	bool spawned;
	public Sprite creepsprite;
	public float SPAWNDELAY;
	public int resources = 100;
	public Text restext;
	public Text towertext;
	public Text lifewavetext;
	public Text wavecountdowntext;
	public Text tooltiptext;
	public int lives;
	private bool running = true;
	private bool buildingTower = false;
	private GameObject newTower;
	public GameObject towerTemplate;
	private int[,] groundProperties;
	private bool spawnedAll = true;
	public RuntimeAnimatorController creepanim;
	// Use this for initialization
	void Start () {
		wave = 0;

		if (MainMenu.instance == null)
		{
			MainMenu.instance = new PlayerState();
		}
		resources = MainMenu.instance.level.getStartingResources();
		creeps = new List<Creep>();
		towers = new List<TowerBehavior>();
		waypoints = new List<Vector2>();
		groundProperties = new int[50,25];

		MainMenu.instance.level.getMap(waypoints, groundProperties);
		spawned = false;
	}

	public TowerBehavior selectedTower;
	public Creep selectedCreep;

	public void upgrade(int what)
	{
		if (selectedTower != null)
		{
			selectedTower.upgrade(what);
			showTooltip(what);
		}
	}

	public void unselect()
	{
		if (selectedTower != null) selectedTower.unselect();
		if (selectedCreep != null) selectedCreep.unselect();
	}
	
	// Update is called once per frame
	void Update () {
		if (!running) return;
		if (spawnedCreeps == 0 && spawnedAll)
		{
			spawned = false;
			resources += MainMenu.instance.level.getWaveReward(wave); //  wave*5
			wave++;
		}
		if (!spawned)
		{
			spawnedAll = false;
			StartCoroutine(spawnCreeps());
			spawned = true;
		}
		restext.text = "Resources: " + resources;
		lifewavetext.text = "Lives: " + lives + "; wave: " + wave;
		if (lives <= 0)
		{
			restext.text = "You lost";
			restext.fontSize = 72;
			running = false;
		}
		if (buildingTower)
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
			newTower.transform.position = gameObject.GetComponent<BuildHelper>().getClosestDot((Vector2)mousePosition);
			if (resources < 50)
			{
				newTower.GetComponent<SpriteRenderer>().color = Color.red;
			}
			else
			{
				newTower.GetComponent<SpriteRenderer>().color = Color.blue;
			}
		}
		if (selectedTower)
		{
			selectedTower.showStats();
		} 
		else if (selectedCreep)
		{
			selectedCreep.showStats();
		}
		else towertext.text = "";
		if (Input.GetMouseButtonDown(0) && buildingTower)
		{
			DotProperty prop = gameObject.GetComponent<BuildHelper>().getBuildProperty((Vector2)newTower.transform.position);
			if (prop.buildable == 1 && resources >= 50)
			{
				buildingTower = false;
				newTower.GetComponent<TowerBehavior>().map = this;
				newTower.GetComponent<TowerBehavior>().isBuilt = true;
				towers.Add(newTower.GetComponent<TowerBehavior>());
				newTower.GetComponent<TowerBehavior>().select();
				groundProperties[prop.i,prop.j] = 0;
				gameObject.GetComponent<BuildHelper>().unpopulate();
				resources -= 50;
			}
		}
		else if (Input.GetMouseButtonDown(1) && buildingTower)
		{
			buildingTower = false;
			Destroy (newTower);
			gameObject.GetComponent<BuildHelper>().unpopulate();
		}
	}

	public void removeCreep(Creep creep)
	{
		creeps.Remove(creep);
	}

	public IList<Vector2> getPathToWaypoint(int index, Vector2 origin)
	{
		IList<Vector2> result = new List<Vector2>();
		if (index >= waypoints.Count) return result;
		result.Add(waypoints[index]+Random.insideUnitCircle*0.25f);
		return result;
	}

	IEnumerator spawnCreeps()
	{
		if (wave > MainMenu.instance.level.getWaveCount() && MainMenu.instance.level.getWaveCount() >= 0)
		{
			for (int c = 0; c < 5; ++c)
			{
				wavecountdowntext.text = "All waves cleared!\n Return to the lab in " + (5-c);
				wavecountdowntext.color = new Color(255, 0, 0);
				yield return new WaitForSeconds(1);
			}
			MainMenu.instance.researchCredits += MainMenu.instance.level.getResearchCredits();
			Application.LoadLevel(2);
			yield break;
		}
		for (int c = 0; c < 5; ++c)
		{
			wavecountdowntext.text = "Next wave in " + (5-c);
			yield return new WaitForSeconds(1);
		}
		wavecountdowntext.text = "";
		yield return StartCoroutine(MainMenu.instance.level.spawnWave(wave, this));
		spawnedAll = true;

	}
	
	public void spawnCreep(Creep.CreepType type, Creep.CreepTrait traits, int level)
	{
		Creep newcreep;
		GameObject go = new GameObject();
		SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
		rend.sprite = creepsprite; 
		newcreep = go.AddComponent<Creep>();
		newcreep.map = this;
		Animator anim  = go.AddComponent<Animator>();
		anim.runtimeAnimatorController = creepanim;
		go.AddComponent<BoxCollider2D>();
		newcreep.setType(type, traits, level);
		creeps.Add(newcreep);
		go.transform.position = (Vector3)waypoints[0];
		spawnedCreeps++;
	}

	public void startTowerBuild()
	{

		if (buildingTower)
		{
			buildingTower = false;
			Destroy (newTower);
			gameObject.GetComponent<BuildHelper>().unpopulate();
			return;
		}
		unselect ();
		newTower = Instantiate (towerTemplate);
		newTower.GetComponent<TowerBehavior>().map = this;
		buildingTower = true;

		gameObject.GetComponent<BuildHelper>().populate(groundProperties);
	}

	public void showTooltip(int nr)
	{
		if (selectedTower != null)
		{
		    tooltiptext.text = selectedTower.getTooltip(nr);
		}
	}

	public void hideTooltip()
	{
		tooltiptext.text = "";
	}

	public void OnMouseDown()
	{


	}
}
