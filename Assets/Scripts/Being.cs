using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Being : MonoBehaviour {

	public string beingName;
	public int strength = 0;
	public int strengthOffset = 0;
	public List<Buff> buffs = new List<Buff> ();


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual int Strength(){
		return(0);
	}


}
