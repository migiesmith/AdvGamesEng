using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonArgs : SceneArguments {

	protected DungeonParams dgnParams;



	public DungeonParams getDgnParams()
	{
		return dgnParams;
	}

	public void setDgnParams(DungeonParams dgnParams)
	{
		this.dgnParams = dgnParams;
	}

}
