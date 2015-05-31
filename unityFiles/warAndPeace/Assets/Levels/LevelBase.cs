using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LevelBase  {
	public static IDictionary<string,LevelBase> levels;

	public static void init()
	{
		levels = new Dictionary<string, LevelBase>();
		levels.Add("level1", new Level1());
		levels.Add("level2", new Level2());
		levels.Add("infinite1", new InfiniteLevel());
		levels.Add("", new LevelBase()); // blank level is dummy that doesn't do anything
	}

	public static LevelBase getLevel(string name)
	{
		return levels[name];
	}

	public virtual string getSuccessor()
	{
		return "";
	}

	public virtual void getMap(IList<Vector2> waypoints, int[,] groundProperties)
	{
		waypoints.Add(new Vector2(0f,5f));
		waypoints.Add(new Vector2(-1f,1.2f));
		waypoints.Add(new Vector2(-3.6f,0.6f));
		waypoints.Add(new Vector2(-5.1f,-0.5f));
		
		waypoints.Add(new Vector2(-3.5f,-2f));
		waypoints.Add(new Vector2(1f,-2f));
		waypoints.Add(new Vector2(4f,-2.5f));
		waypoints.Add(new Vector2(6f,-1.2f));
		waypoints.Add(new Vector2(11f,-1.2f));
		
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
	
	public virtual IEnumerator spawnWave(int wave, MapBehavior map)
	{
		yield break;
	}

	public virtual int getWaveCount()
	{
		return 0;
	}

	public virtual int getWaveReward(int wave)
	{
		return 0;
	}

	protected virtual IEnumerator spawnGroup(int count, float delay, int level, Creep.CreepType type, Creep.CreepTrait traits, MapBehavior map)
	{
		for (int j = 0; j < count; ++ j)
		{
			map.spawnCreep(type, traits, level);
			yield return new WaitForSeconds(delay);
		}
	}

	public virtual float getResearchCredits()
	{
		return 0;
	}
}
