﻿using UnityEngine;
using System.Collections;

public class AbilityFire : Ability {

	// Use this for initialization
	public override bool Perform (PartyMember originator, GameObject target) {
		if (originator.magic > 0) {
			originator.magic -= 1;
			int damage = Random.Range (10, 20);
			EventQueue.AddMessage (originator.memberName + "casts fire!");
			EventQueue.AddEvent (target, damage, DamageTypes.Fire);
			return(true);
		} else {
			EventQueue.AddMessage ("need more mp");
			return(false);
		}
	}

	public override string Name () {
		return("Fire");
	}

	public override string SpriteName(){
		return("button_fire");
	}

	public override string Description(){
		return("Attack a target with fire");
	}
}
