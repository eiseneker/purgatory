using UnityEngine;
using System.Collections;

public class BaddieFrog : Baddie {

	public override int Level(){
		return(1);
	}

	public override void DoAction(){
		PartyMember target = DeriveTargetFromThreat ();
		int damage = Random.Range (1, Strength() + 1);
		EventQueue.AddMessage (beingName + " licks " + target.beingName + "!");
		EventQueue.AddEvent (gameObject, target.gameObject, damage, DamageTypes.Physical);
		turnable.ResetTurn ();
	}

}
