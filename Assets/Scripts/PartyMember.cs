using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class PartyMember : Being, IAttackable {

	public static List<PartyMember> members = new List<PartyMember>();
	public delegate void ActionDelegate(PartyMember originator, GameObject target);
	public Job job;
	public static int currency;
	public int magicPoints = 0;
	public int hitPoints = 0;
	public int agility = 0;
	public List<List<bool>> levelUps = new List<List<bool>>();
	public List<Item> heldItems = new List<Item>();
	public Item weapon;
	public Item armor;
	public Item accessory;
	private GameObject overlay;
	public IDictionary<ItemTypes, Item> equipment = new Dictionary<ItemTypes, Item>();
	private Image image;
	public Turnable turnable;
	public int maxHitPoints = 0;
	public int maxMagicPoints = 0;
	public List<Ability> abilities = new List<Ability> ();
	public float threat;
	public float defaultThreat = 100;

	public void TurnActive(){
		int i = 0;
		foreach (BuffRiposte buff in buffs) {
			if (!buff.NextTurn ()) {
				buffs[i] = null;
			}
			i++;
		}
		buffs.RemoveAll (buff => buff == null);
	}

	public override int Strength(){
		return(Mathf.Clamp(job.Strength () + strength + strengthOffset, 0, 9999));
	}

	public int Agility(){
		return(job.Agility () + this.agility);
	}

	public int MaxMagicPoints(){
		return(job.MagicPoints () + this.maxMagicPoints);
	}

	public int MaxHitPoints(){
		return(job.HitPoints() + this.maxHitPoints);
	}

	public void ShowOverlay(){
		overlay.SetActive (true);
	}

	public void HideOverlay(){
		overlay.SetActive (false);
	}

	public Item AddItem(GameObject itemToAdd){
		Item item = itemToAdd.GetComponent<Item> ();
		return(AddItem(item));
	}

	public Item AddItem(Item itemToAdd){
		Item item = null;
		if(heldItems.Count < 2){
			item = itemToAdd;
			item.owner = this;
			heldItems.Add (item);
		}
		return(item);
	}

	public void RemoveItem(Item itemToRemove){
		if (itemToRemove.owner == this) {
			itemToRemove.owner = null;
		}
		heldItems.Remove (itemToRemove);
	}

	public void Equip(Item itemToEquip){
		ItemTypes itemType = itemToEquip.ItemType ();
		Item equippedItem = equipment [itemType];
		if (equippedItem != null) {
			heldItems.Add (equipment [itemType]);
		}
		heldItems.Remove (itemToEquip);
		equipment [itemType] = itemToEquip;
	}

	// Use this for initialization
	void Start () {
		threat = defaultThreat;
		abilities.Add (new AbilityAttack());
		turnable = GetComponent<Turnable> ();
		magicPoints = maxMagicPoints;
		members.Add (this);
		this.beingName = "Rock " + members.Count.ToString ();
		overlay = transform.Find ("Overlay").gameObject;
		HideOverlay ();
		SetJob ();
		SetLevelUps ();
		hitPoints = MaxHitPoints();
		currency = 500;
		AddItem(Instantiate (Resources.Load ("Items/Potion"), transform.position, Quaternion.identity) as GameObject);
		AddItem(Instantiate (Resources.Load ("Items/Sword"), transform.position, Quaternion.identity) as GameObject);
		Unselect ();
		equipment [ItemTypes.Armor] = null;
		equipment [ItemTypes.Weapon] = null;
		equipment [ItemTypes.Accessory] = null;
		image = transform.Find ("Image").GetComponent<Image> ();
		image.sprite = Resources.Load<Sprite> ("Sprites/job_" + job.SpriteName());
		turnable.sprite = transform.Find ("Image").GetComponent<Image> ().sprite;
	}

	void SetLevelUps(){
		foreach (Job selectedJob in Job.jobs) {
			int count = selectedJob.LevelUps ().Count;
			List<bool> list = new List<bool>();
			foreach (LevelUpStruct levelUpStruct in selectedJob.LevelUps()) {
				list.Add (false);
			}
			levelUps.Add (list);
		}
	}

	void SetJob(){
		int randomValue = UnityEngine.Random.Range(0, Job.jobs.Count);
		job = Job.jobs[randomValue];
		job.partyMember = this;
//		job.Bootstrap ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string Name(){
		return("playur");
	}

	public static void UnselectAll(){
		foreach (PartyMember partyMember in PartyMember.members) {
			partyMember.Unselect ();
		}
	}


	public void DoClickAction() {
		UnselectAll ();
		if (GameController.inEncounter) {
			CombatMenu.SelectTarget (gameObject);
		} else if (GameController.inShopMenu) {
			ShopHUD.SelectPartyMember (gameObject);
		}else {
			Select ();
			LevelUpHUD.SelectPartyMember (this);
		}
	}

	public void ReceiveHit(GameObject attacker, int damage, DamageTypes damageType){
		foreach (BuffRiposte buff in buffs) {
			buff.Perform (this, attacker, damage);
		}
		EventQueue.AddMessage (beingName + " sustains " + damage + " damage", 1);
		hitPoints -= damage;
	}

	public void DestroyMe(){

	}

	public void UpdateLevelUpSlot(int i){
		int jobIndex = Job.jobs.IndexOf (job);
		levelUps [jobIndex] [i] = true;
	}

	public bool HasLevelUpSlot(int i){
		int jobIndex = Job.jobs.IndexOf (job);
		return(levelUps [jobIndex] [i]);
	}

	public void Select(){
		transform.Find ("Selector").GetComponent<Image> ().color = new Color (1, 1, 1, 1);
	}

	public void Unselect(){
		transform.Find ("Selector").GetComponent<Image> ().color = new Color (1, 1, 1, 0);
	}

	public int HitPoints(){
		return(hitPoints);
	}


}
