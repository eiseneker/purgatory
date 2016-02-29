using UnityEngine;
using System.Collections;
using System;

public class Buff {

	public enum BuffType { None, Reaction, Attack };
	public BuffType bufftype;

	// Use this for initialization
	public virtual int Perform (PartyMember owner, GameObject actor, int damage) {
		return(0);
	}

	public virtual bool NextTurn (){
		return(false);
	}

}
