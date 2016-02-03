﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public struct LevelUpStruct
{
	public delegate void Perform(PartyMember partyMember);
	public string name;
	public string description;
	public int cost;
	public Perform performer;

	public LevelUpStruct(string inputName, string inputDescription, int inputCost, Perform inputPerformer)
	{
		name = inputName;
		description = inputDescription;
		cost = inputCost;
		performer = inputPerformer;
	}
}