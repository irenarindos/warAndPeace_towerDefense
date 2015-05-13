using UnityEngine;
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
	public int resources = 150;
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
	// Use this for initialization
	void Start () {
		wave = 0;
		creeps = new List<Creep>();
		towers = new List<TowerBehavior>();
		waypoints = new List<Vector2>();
		waypoints.Add(new Vector2(0f,5f));
		waypoints.Add(new Vector2(-1f,1.2f));
		waypoints.Add(new Vector2(-4.8f,-0.5f));
		waypoints.Add(new Vector2(-3.5f,-2f));
		waypoints.Add(new Vector2(1f,-2f));
		waypoints.Add(new Vector2(4f,-2.5f));
		waypoints.Add(new Vector2(6f,-1.2f));
		waypoints.Add(new Vector2(11f,-1.2f));
		spawned = false;
		groundProperties = new int[50,25];
		groundProperties[13,16] = 1;
		groundProperties[14,16] = 1;
		groundProperties[17,9] = 1;
		groundProperties[18,9] = 1;
		groundProperties[19,9] = 1;
		groundProperties[17,17] = 1;
		groundProperties[18,17] = 1;
		groundProperties[18,11] = 1;
		groundProperties[18,12] = 1;
		groundProperties[19,11] = 1;
		groundProperties[19,12] = 1;
		groundProperties[26,5] = 1;
		groundProperties[27,5] = 1;
		groundProperties[31,9] = 1;
		groundProperties[32,9] = 1;
		groundProperties[38,6] = 1;
		groundProperties[39,6] = 1;
		groundProperties[40,6] = 1;
	}

	public TowerBehavior selectedTower;

	public void upgrade(int what)
	{
		if (selectedTower != null)
		{
			selectedTower.upgrade(what);
			showTooltip(what);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!running) return;
		if (spawnedCreeps == 0 && spawnedAll)
		{
			spawned = false;
			wave++;
		}
		if (!spawned)
		{
			spawnedAll = false;
			StartCoroutine("spawnCreeps");
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
		int count = 10;
		for (int c = 0; c < 5; ++c)
		{
			wavecountdowntext.text = "Next wave in " + (5-c);
			yield return new WaitForSeconds(1);
		}
		wavecountdowntext.text = "";
		if (wave % 8 == 0) count = 15;
		if (wave % 10 == 0) count = 1;
		if (wave % 7 == 0) count = 8;
		
		for (int i = 0; i < count; ++i)
		{
			Creep.CreepType type = Creep.CreepType.NORMAL;
			Creep.CreepTrait[] traits = new Creep.CreepTrait[] {};
			if (wave % 6 == 0) type = Creep.CreepType.LARGE;
			if (wave % 5 == 0 && i %4 == 3) type = Creep.CreepType.LARGE;
			if (wave % 4 == 0) traits = new Creep.CreepTrait[] { Creep.CreepTrait.FAST };
			if (wave % 11 == 0) traits = new Creep.CreepTrait[] { Creep.CreepTrait.SHIELDED };
			if (wave % 10 == 0) type = Creep.CreepType.BOSS;

			spawnCreep(type, traits);
			yield return new WaitForSeconds(1.0f);
		}
		spawnedAll = true;
	}
	
	void spawnCreep(Creep.CreepType type, Creep.CreepTrait[] traits)
	{
		Creep newcreep;
		GameObject go = new GameObject();
		SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
		rend.sprite = creepsprite; 
		newcreep = go.AddComponent<Creep>();
		newcreep.map = this;

		newcreep.setType(type, traits, wave);
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
		if (selectedTower) selectedTower.unselect();
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


		if (buildingTower)
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
		else
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
			Vector3 pos = new Vector3(mousePosition.x, mousePosition.y, 0);
			foreach (TowerBehavior t in towers)
			{
				if ((t.gameObject.transform.position - pos).magnitude < 0.15)
				{
					t.select();
				}
			}
			
		}
	}
}
