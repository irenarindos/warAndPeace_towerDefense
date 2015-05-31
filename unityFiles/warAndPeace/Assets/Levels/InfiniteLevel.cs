using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class InfiniteLevel : LevelBase {

	
	public override IEnumerator spawnWave(int wave, MapBehavior map)
	{
		int level = wave;
		int count = 10;
		int pergroup = 1;
		if (wave % 8 == 0) count = 15;
		if (wave % 10 == 0) count = 1;
		if (wave % 7 == 0) { count = 5; pergroup = 4;  level /= 2; }
		if (wave % 18 == 0) { count = 4; pergroup = 5; level /= 3; }
		Creep.CreepTrait traits = Creep.CreepTrait.NONE;
		if (wave % 4 == 0) traits |= Creep.CreepTrait.FAST;
		//if (wave % 11 == 0) traits.Add(Creep.CreepTrait.SHIELDED);
		if (wave % 9 == 0) 
		{
			traits |= Creep.CreepTrait.PLATED50;
			level -= 5;
		}
		if (wave % 43 == 0)
		{
			traits |= Creep.CreepTrait.PLATED1;
			level /= 12;
		}
		if (wave % 23 == 0)
		{
			traits |= Creep.CreepTrait.PLATED10;
			level /= 7;
		}
		if (wave % 17 == 0) traits |= Creep.CreepTrait.ENRAGED;
		if (wave % 10 == 0) count += wave/40;
		else count += wave/12;
		
		for (int i = 0; i < count; ++i)
		{
			for (int j = 0; j < pergroup; ++ j)
			{
				int clevel = level;
				Creep.CreepType type = Creep.CreepType.NORMAL;
				
				if (wave % 6 == 0) { type = Creep.CreepType.LARGE; clevel -= 1; }
				if (wave % 5 == 0 && i %4 == 3) 
				{ 
					type = Creep.CreepType.LARGE;
					if (wave > 10)
						clevel -= 3;
				}
				if (j%3 == 2) traits |= Creep.CreepTrait.ENRAGED;
				if (wave % 10 == 0) type = Creep.CreepType.BOSS;
				map.spawnCreep(type, traits, clevel);
				yield return new WaitForSeconds(0.2f);
			}
			yield return new WaitForSeconds(1.0f);
		}
	}
	
	public override int getWaveCount()
	{
		return -1;
	}
	
	public override int getWaveReward(int wave)
	{
		return wave*5;
	}
}
