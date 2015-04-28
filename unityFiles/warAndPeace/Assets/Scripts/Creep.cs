using UnityEngine;
using System.Collections.Generic;

public class Creep : MonoBehaviour {

	private const float TARGETDIST = 0.05f;
	public MapBehavior map;
	private IList<Vector2> path;
	private bool valid;
	public bool dead;
	private int waypoint;
	private int pathpoint;
	public float health;
	// Use this for initialization
	void Start () {
		valid = false;
		dead = false;
		waypoint = 0;
		health = 100;
		getWaypoint();
	}

	void getWaypoint()
	{
		path = map.getPathToWaypoint(waypoint, gameObject.transform.position);
		pathpoint = 0;
		valid = true;
		if (path.Count == 0)
		{
			dead = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (path == null || dead) return;
		float v = 1.0f; 
		v *= Time.deltaTime;
		if ((((Vector2)gameObject.transform.position - path[pathpoint]).magnitude < TARGETDIST) || !valid)
		{
			if (pathpoint + 1 == path.Count)
			{
				waypoint++;
				getWaypoint ();
			}
			else
			{
				pathpoint++;
			}
		}
		if (pathpoint < path.Count)
		{
			Vector2 dir = path[pathpoint] - (Vector2)gameObject.transform.position;
			gameObject.transform.position += (Vector3)dir.normalized*v;
		}
	}

	public void damage(float dmg)
	{
		health -= dmg;
		//gameObject.GetComponent<SpriteRenderer>().color.r = (int)(255*health/100.0);
		gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.white, health/100.0f);
		if (health <= 0)
		{
			dead = true;

		}
	}
}
