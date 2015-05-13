using UnityEngine;
using System.Collections.Generic;

public class BoltCleanup : MonoBehaviour {
	public float expiration = 0f;
	public float refresh = 10f;
	public float lastRefresh = 0f;
	public Vector2 source;
	public Creep target;
	public Material boltmat;
	public float damage;
	public IList<ImpactEffect> effects;
	public TowerBehavior tower;


	// Use this for initialization
	void Start () {
		refreshBolt();
	}

	void refreshBolt() 
	{
		if (target == null) return;
		LineRenderer lr = gameObject.GetComponent<LineRenderer>();
		lr.material = boltmat;
		IList<Vector2> verts = new List<Vector2>();
		Vector2 at = source;
		Vector2 targetpos = (Vector2)target.gameObject.transform.position;
		float dist = (at - targetpos).magnitude;
		verts.Add(at);
		Utils.midpointDisplacement(at, targetpos, verts, dist/2, dist/64);
		lr.SetWidth(0.02f, 0.02f);
		lr.SetVertexCount(verts.Count);
		int i = 0;
		foreach (Vector2 v in verts)
		{
			lr.SetPosition(i++, new Vector3(v.x, v.y, -1f));
		}
		lastRefresh = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > expiration)
		{
			foreach (ImpactEffect eff in effects)
			{
				eff.apply(target, tower);
			}
			target.realizeDamage(damage);
			Destroy(gameObject);
			return;
		}
		if (Time.time > lastRefresh + refresh)
		{
			refreshBolt();
		}
	}
}
