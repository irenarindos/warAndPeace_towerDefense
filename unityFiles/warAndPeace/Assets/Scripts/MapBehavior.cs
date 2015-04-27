using UnityEngine;
using System.Collections.Generic;

public class MapBehavior : MonoBehaviour {

	private IList<Vector2> waypoints;
	private IList<Creep> creeps;
	int wave;
	bool spawned;
	public Sprite creepsprite;
	public float SPAWNDELAY;
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
	
	// Update is called once per frame
	void Update () {
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
		creeps.Add(newcreep);
		go.transform.position = (Vector3)waypoints[0];
	}
}
