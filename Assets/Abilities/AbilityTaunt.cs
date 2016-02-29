using UnityEngine;
using System.Collections;

public class AbilityTaunt : Ability {

	// Use this for initialization
	public override bool Perform (PartyMember originator, GameObject target) {
		EventQueue.AddMessage (originator.beingName + " done used taunt!");
		EventQueue.AddLambda (() => {
			target.GetComponent<Being>().buffs.Add (new BuffTaunt());
		});
		return(true);
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
