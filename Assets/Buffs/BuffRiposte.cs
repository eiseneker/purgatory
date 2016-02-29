using UnityEngine;
using System.Collections;
using System;

public class BuffRiposte : Buff {

	public BuffRiposte(){
		bufftype = BuffType.Reaction;
	}

	// Use this for initialization
	public override int Perform (PartyMember owner, GameObject attacker, int damage) {
		if (UnityEngine.Random.value < 25f) {
			EventQueue.AddMessage (owner.beingName + " ripostes!");
			EventQueue.AddLambda (() => {
				EventQueue.AddEvent (owner.gameObject, attacker, 10, DamageTypes.Physical);
			});
		}
		return(0);
	}

	public override bool NextTurn (){
		return(false);
	}

}
