using UnityEngine;
using System.Collections;

public class BaddieCorgi : Baddie {

	public override int Level(){
		return(1);
	}

	public override void DoAction(){
		Attack attack = new Attack ();
		attack.defender = DeriveTargetFromThreat ().gameObject;
		attack.damage = Random.Range (1, Strength() + 1);
		attack.attacker = gameObject;
		EventQueue.AddMessage (beingName + " bites " + attack.defender.GetComponent<Being>().beingName + "!");
		EventQueue.AddEvent (gameObject, attack.defender.gameObject, attack.damage, DamageTypes.Physical);
		turnable.ResetTurn ();
	}
}
