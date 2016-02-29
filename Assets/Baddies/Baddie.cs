using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Baddie : Being, IAttackable {

	public int hitPoints;
	private Button button;
	public Turnable turnable;

	protected PartyMember DeriveTargetFromThreat(){
		float randomValue = Random.Range (0, PartyMember.members[0].threat + PartyMember.members[1].threat + PartyMember.members[2].threat + PartyMember.members[3].threat);
		PartyMember target;
		if (randomValue <= PartyMember.members [0].threat) {
			target = PartyMember.members [0];
		} else if (randomValue <= PartyMember.members [0].threat + PartyMember.members [1].threat) {
			target = PartyMember.members [1];
		} else if (randomValue <= PartyMember.members [0].threat + PartyMember.members [1].threat + PartyMember.members [2].threat) {
			target = PartyMember.members [2];
		} else { 
			target = PartyMember.members [3];
		}
		return(target);
	}

	public override int Strength(){
		return(Mathf.Clamp(strength + strengthOffset, 0, 9999));
	}

	public virtual int Level(){
		return(0);
	}

	void Awake(){
		turnable = gameObject.AddComponent<Turnable> ();
	}

	// Use this for initialization
	void Start () {
		hitPoints = 10;
		button = GetComponent<Button> ();
		turnable.sprite = transform.Find ("Body").GetComponent<Image> ().sprite;
	}

	public void DestroyMe(){
		EventQueue.AddMessage (beingName + " is destroyed", 1);
		RoomController.instance.enemies.Remove (gameObject);
		BattleController.instance.turnables.Remove (turnable);
		Destroy (gameObject);
	}

	public void ReceiveHit(GameObject attacker, int damage, DamageTypes damageType){
		if (gameObject != null) {
			int index = EventQueue.AddMessage (beingName + " surstains " + damage + " damage");
			hitPoints -= damage;

			if (hitPoints < 1) {
				EventQueue.AddDestroy (gameObject, index + 1);
			}
		}
	}

	public int HitPoints(){
		return(hitPoints);
	}

	public void DoClickAction() {
		CombatMenu.SelectTarget (gameObject);

	}

	public virtual void DoAction(){
	}

	public void DisableClick(){
		button.enabled = false;
	}

	public void EnableClick(){
		button.enabled = true;
	}
}
