using UnityEngine;
using System.Collections.Generic;

public class Creep : MonoBehaviour {

	private const float TARGETDIST = 0.05f;
	public MapBehavior map;
	private IList<Vector2> path;
	private bool valid;
	private bool dead;
	private int waypoint;
	private int pathpoint;
	// Use this for initialization
	void Start () {
		valid = false;
		dead = false;
		waypoint = 0;
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
}
