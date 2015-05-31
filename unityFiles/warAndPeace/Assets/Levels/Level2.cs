using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


public class Level2 : LevelBase {
	public override IEnumerator spawnWave(int wave, MapBehavior map)
	{
		if (wave == 1) yield return map.StartCoroutine(spawnGroup(10, 0.9f, 1, Creep.CreepType.NORMAL, Creep.CreepTrait.NONE, map));
		if (wave == 2) yield return map.StartCoroutine(spawnGroup(10, 0.8f, 2, Creep.CreepType.NORMAL, Creep.CreepTrait.NONE, map));
		if (wave == 3) 
		{
			for (int i = 0; i < 3; ++i)
			{
				yield return map.StartCoroutine(spawnGroup(3, 0.8f, 3, Creep.CreepType.NORMAL, Creep.CreepTrait.NONE, map));
				yield return new WaitForSeconds(0.5f);
				yield return map.StartCoroutine(spawnGroup(1, 0.8f, 3, Creep.CreepType.LARGE, Creep.CreepTrait.NONE, map));
				yield return new WaitForSeconds(0.5f);
			}
		}
		if (wave == 4) yield return map.StartCoroutine(spawnGroup(10, 1.2f, 4, Creep.CreepType.NORMAL, Creep.CreepTrait.NONE, map));
		if (wave == 5) yield return map.StartCoroutine(spawnGroup(10, 1.2f, 5, Creep.CreepType.NORMAL, Creep.CreepTrait.FAST, map));
		if (wave == 6) yield return map.StartCoroutine(spawnGroup(12, 1.0f, 6, Creep.CreepType.NORMAL, Creep.CreepTrait.NONE, map));
		if (wave == 7) yield return map.StartCoroutine(spawnGroup(12, 1.0f, 7, Creep.CreepType.NORMAL, Creep.CreepTrait.NONE, map));
		if (wave == 8) 
		{
			for (int i = 0; i < 3; ++i)
			{
				yield return map.StartCoroutine(spawnGroup(3, 1f, 9, Creep.CreepType.NORMAL, Creep.CreepTrait.NONE, map));
				yield return new WaitForSeconds(0.5f);
				yield return map.StartCoroutine(spawnGroup(1, 1f, 8, Creep.CreepType.LARGE, Creep.CreepTrait.NONE, map));
				yield return new WaitForSeconds(0.5f);
			}
		}
		if (wave == 9) yield return map.StartCoroutine(spawnGroup(12, 1.0f, 10, Creep.CreepType.NORMAL, Creep.CreepTrait.FAST, map));
		if (wave == 10) yield return map.StartCoroutine(spawnGroup(1, 1.0f, 12, Creep.CreepType.BOSS, Creep.CreepTrait.NONE, map));
	}
	
	public override int getWaveCount()
	{
		return 10;
	}
	
	public override int getWaveReward(int wave)
	{
		return 20;
	}
	
	public override float getResearchCredits()
	{
		return 50;
	}
	
	public override string getSuccessor()
	{
		return "level2";
	}
}
