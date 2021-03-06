﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelUpHUD : MonoBehaviour {

	public static LevelUpHUD instance;
	public static PartyMember selectedPartyMember;
	public static LevelUpStruct.LevelUpTypes selectedLevelUpType;
	public static LevelUpStruct.LevelUpTypes selectedLevelUpType2;
	public static int selectedIndex;
	public static LevelUpStruct selectedLevelUpStruct;
	public Vector3 confirmButtonPosition;
	public static List<GameObject> buttonList;
	public Canvas canvas;
	private ActionButton selectedButton;


	// Use this for initialization
	void Start () {
		instance = this;
		selectedPartyMember = null;
		canvas = GetComponent<Canvas> ();
		confirmButtonPosition = instance.transform.Find ("ConfirmLevelUp").position;
		instance.transform.Find ("ConfirmLevelUp").position = new Vector3(9999, 9999, 0);
		buttonList = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SelectBoostLevelUps(){
		SelectLevelUpType (LevelUpStruct.LevelUpTypes.Boost);
	}

	public void SelectAbilityLevelUps(){
		SelectLevelUpType (LevelUpStruct.LevelUpTypes.Ability);
	}

	public void Show(){
		GameController.EnterLevelUpMenu ();
		instance.transform.Find ("CategoryButtons").GetComponent<Canvas> ().enabled = false;
		Prompt.SetText("Select a party member");
	}

	public void Close(){
		ObjectTooltip.Hide ();
		GameController.ExitLevelUpMenu ();
		PartyMember.UnselectAll ();
		instance.transform.Find ("ConfirmLevelUp").position = new Vector3(9999, 9999, 0);
		Prompt.Clear ();

		DestroyButtons ();
	}

	public static void DestroyButtons(){
		foreach(GameObject button in buttonList){
			Destroy (button);
		}
		buttonList = new List<GameObject> ();
	}

	public static void SelectPartyMember(PartyMember partyMember){
		instance.transform.Find ("CategoryButtons").GetComponent<Canvas> ().enabled = true;
		selectedPartyMember = partyMember;
		if(selectedLevelUpType == LevelUpStruct.LevelUpTypes.None) Prompt.SetText("Select a category");
		ShowLevelUps ();
	}

	public static void SelectLevelUpType(LevelUpStruct.LevelUpTypes levelUpType){
		selectedLevelUpType = levelUpType;
		Prompt.Clear ();
		ShowLevelUps ();
	}

	public static void ShowLevelUps(){
		print (selectedLevelUpType2);
		if (selectedLevelUpType != LevelUpStruct.LevelUpTypes.None && selectedPartyMember != null) {
			ObjectTooltip.Hide ();
			DestroyButtons ();
			int i = 0;
			int s = 0;
			int xIncrement = 0;
			foreach (LevelUpStruct levelUpStruct in selectedPartyMember.job.LevelUps()) {
				if (levelUpStruct.levelUpType == selectedLevelUpType) {
					if (xIncrement > 3)
						xIncrement = 0;
					float x = 70 + ((xIncrement) * 70);
					float y = 400 - (Mathf.Floor(s/4) * 70);

					GameObject buttonObject = Instantiate (Resources.Load ("ActionButton"), Vector3.zero, Quaternion.identity) as GameObject;
					ActionButton actionButton = buttonObject.GetComponent<ActionButton> ();
					buttonObject.transform.parent = instance.transform;
					buttonObject.transform.position = new Vector3 (x, y, buttonObject.transform.position.z);
					buttonObject.transform.localScale = new Vector3 (1f, 1f, 1);
					Button button = buttonObject.GetComponent<Button> ();
					actionButton.sprite = Resources.Load<Sprite> ("Sprites/" + levelUpStruct.spriteName);
					if (selectedPartyMember.HasLevelUpSlot (i)) {
						actionButton.startText = "LEARNED";
					} else {
						actionButton.startText = levelUpStruct.cost.ToString ();
					}

					button.transform.Find ("Text").GetComponent<Text> ().text = levelUpStruct.name;

					LevelUpStruct capturedLevelUpStruct = levelUpStruct;
					int capturedIndex = i;

					button.onClick.AddListener (delegate {
						ConfirmLevelUp (capturedLevelUpStruct, capturedIndex);
					});
					button.onClick.AddListener (delegate {
						HighlightButton (button.gameObject);
					});

					buttonList.Add (button.gameObject);
					xIncrement++;
					s++;
				}

				i++;
			}
		}
	}

	public static void HighlightButton(GameObject buttonObject){
		ClearButtonHighlights ();
		buttonObject.GetComponent<ActionButton> ().Highlight ();
		instance.selectedButton = buttonObject.GetComponent<ActionButton> ();
	}

	public static void ClearButtonHighlights(){
		foreach(GameObject button in buttonList){
			button.GetComponent<ActionButton> ().UnHighlight ();
		}
		instance.selectedButton = null;
	}

	public static void ConfirmLevelUp(LevelUpStruct levelUpStruct, int inputSelectedIndex){
		selectedIndex = inputSelectedIndex;
		selectedLevelUpStruct = levelUpStruct;
		ObjectTooltip.Show (levelUpStruct);
		if(levelUpStruct.cost <= PartyMember.currency && !selectedPartyMember.HasLevelUpSlot (selectedIndex)){
			instance.transform.Find ("ConfirmLevelUp").position = instance.confirmButtonPosition;
		}else{
			instance.transform.Find ("ConfirmLevelUp").position = new Vector3(9999, 9999, 0);
		}
		Prompt.Clear ();
	}

	public void ExecuteLevelUp(){
		ObjectTooltip.Hide ();
		selectedLevelUpStruct.performer (selectedPartyMember);
		selectedPartyMember.UpdateLevelUpSlot (selectedIndex);
		transform.Find ("ConfirmLevelUp").position = new Vector3(9999, 9999, 0);
		Prompt.Clear ();
		PartyMember.currency -= selectedLevelUpStruct.cost;
		selectedButton.SetText ("LEARNED");
		ClearButtonHighlights ();
	}
}
