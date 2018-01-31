using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject gameMaster;

	// Use this for initialization
	void Awake () {
		if(GameMaster.Instance == null)
        {
            Instantiate(gameMaster);
        }
	}
}
