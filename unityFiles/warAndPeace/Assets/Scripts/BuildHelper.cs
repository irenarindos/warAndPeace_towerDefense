using UnityEngine;
using System.Collections.Generic;


public class BuildHelper : MonoBehaviour {

	private IList<GameObject> dots;
	public int i, j;

	public Sprite dotsprite;

	// Use this for initialization
	void Start () {
		dots = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void populate(int[,] colors)
	{
		for (int i = 0; i < colors.GetLength(0); ++i)
		{
			for (int j = 0; j < colors.GetLength(1); ++j)
			{
				GameObject go = new GameObject();
				SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
				rend.sprite = dotsprite; 
				if (colors[i,j] == 1)
				{
					rend.color = new Color(0,1,0,0.3f);
				}
				else
				{
					rend.color = new Color(1,0,0,0.3f);
				}
				rend.sortingOrder = 1;
				go.transform.parent = gameObject.transform;
				// this is super-ugly/map-specific ... needs to be more general at some point
				go.transform.localPosition = new Vector3((i+0.5f)*0.2048f - 5.12f, (j+0.5f)*0.0768f*4f - 7.68f/2f, 0);
				go.AddComponent<DotProperty>();
				go.GetComponent<DotProperty>().buildable = colors[i,j];
				go.GetComponent<DotProperty>().i = i;
				go.GetComponent<DotProperty>().j = j;
				dots.Add (go);
			}
		}
	}

	public DotProperty getBuildProperty(Vector2 where)
	{
		Vector3 closest = new Vector3(-100f,-100f,-100f);
		GameObject closestObj = null;
		foreach (GameObject obj in dots)
		{
			if ((obj.transform.position - (Vector3)where).magnitude < (closest - (Vector3)where).magnitude)
			{
				closest = obj.transform.position;
				closestObj = obj;
			}
		}
		if (!closestObj) return new DotProperty();
		return closestObj.GetComponent<DotProperty>();
	}

	public void unpopulate()
	{
		foreach (GameObject obj in dots)
		{
			Destroy (obj);
		}
		dots = new List<GameObject>();
	}

	public Vector3 getClosestDot(Vector2 where)
	{
		Vector3 closest = new Vector3(-100f,-100f,-100f);
		foreach (GameObject obj in dots)
		{
			if ((obj.transform.position - (Vector3)where).magnitude < (closest - (Vector3)where).magnitude)
			{
				closest = obj.transform.position;
			}
		}
		return closest;
	}
}

public class DotProperty : MonoBehaviour
{
	public int buildable;
	public int i, j;
}
