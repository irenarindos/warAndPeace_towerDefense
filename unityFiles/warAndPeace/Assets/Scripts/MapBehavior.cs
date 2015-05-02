using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapBehavior : MonoBehaviour {

	private IList<Vector2> waypoints;
	public IList<Creep> creeps;
	int wave;
	bool spawned;
	public Sprite creepsprite;
	public float SPAWNDELAY;
	public int resources = 100;
	public Text restext;
	public Text towertext;
	public Text lifewavetext;
	public int lives;
	private bool running = true;
	private bool buildingTower = false;
	private GameObject newTower;
	public GameObject towerTemplate;
	// Use this for initialization
	void Start () {
		wave = 0;
		creeps = new List<Creep>();
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
	}

	public TowerBehavior selectedTower;

	public void upgrade(int what)
	{
		if (selectedTower != null)
		{
			selectedTower.upgrade(what);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!running) return;
		if (creeps.Count == 0)
		{
			spawned = false;
			wave++;
		}
		if (!spawned)
		{
			for (int i = 0; i < 10; ++i)
			{
				Invoke("spawnCreep", i*SPAWNDELAY);
			}
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
			newTower.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
			//transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
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

	void spawnCreep()
	{
		Creep newcreep;
		GameObject go = new GameObject();
		SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
		rend.sprite = creepsprite; 
		newcreep = go.AddComponent<Creep>();
		newcreep.map = this;
		newcreep.health = 100 + 25*wave;
		newcreep.maxHealth = 100 + 25*wave;
		newcreep.value = wave;
		creeps.Add(newcreep);
		go.transform.position = (Vector3)waypoints[0];
	}

	public void startTowerBuild()
	{
		newTower = Instantiate (towerTemplate);
		newTower.GetComponent<TowerBehavior>().map = this;
		buildingTower = true;
	}

	public void onMouseDown()
	{
		if (buildingTower)
		{
			buildingTower = false;
			newTower.GetComponent<TowerBehavior>().map = this;
			newTower.GetComponent<TowerBehavior>().isBuilt = true;
		}
	}
}
