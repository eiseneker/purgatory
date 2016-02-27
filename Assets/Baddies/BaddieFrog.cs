﻿using UnityEngine;
using System.Collections;

public class BaddieFrog : Baddie {

	public override int Level(){
		return(1);
	}

	public override void DoAction(){
		int randomValue = Random.Range (0, PartyMember.members.Count);
		PartyMember target = PartyMember.members [randomValue];
		int damage = Random.Range (1, 10);
		EventQueue.AddMessage (beingName + " licks " + target.beingName + "!");
		EventQueue.AddEvent (target.gameObject, damage, DamageTypes.Physical);
		turnable.ResetTurn ();
	}
}
