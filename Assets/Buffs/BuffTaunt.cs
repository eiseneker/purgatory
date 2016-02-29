using UnityEngine;
using System.Collections;
using System;

public class BuffTaunt : Buff {

	public BuffTaunt(){
		bufftype = BuffType.Attack;
	}

	// Use this for initialization
	public override int Perform (PartyMember owner, GameObject attacker, int damage) {
		return(0);
	}

	public override bool NextTurn (){
		return(false);
	}

}
