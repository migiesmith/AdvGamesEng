using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipTutorialManager : MonoBehaviour {

    public enum ShipTutorial
    {
        FABRICATOR_SELL,
        FABRICATOR_BROWSE,
        FABRICATOR_END,
        ARMORY_DEPOSIT,
        ARMORY_WITHDRAW,
        MAP_ENTER,
        MAP_PICKUP
    }

    private ShipTutorial stage;

    public Text FabricatorText;
    public Text ArmoryText;
    public Text MapText;

	// Use this for initialization
	void Start () {
        stage = ShipTutorial.FABRICATOR_SELL;
	}
	
    public void itemsSold()
    {
        if (stage == ShipTutorial.FABRICATOR_SELL)
        {
            FabricatorText.text = "The Resource cost of the selected item is displayed beneath the \"Create\" button.\n\nUse the arrow buttons to the left of the screen to browse the available items.";
            stage = ShipTutorial.FABRICATOR_BROWSE;
        }
    }

    public void itemsBrowsed()
    {
        if (stage == ShipTutorial.FABRICATOR_BROWSE)
        {
            FabricatorText.text = "Unwanted equipment can be recycled by dropping it on the pad to the left of the screen, returning 80% of its Resource cost.\n\nNow proceed to the Armoury.";
            stage = ShipTutorial.ARMORY_DEPOSIT;
        }
    }

    public void weaponDeposited()
    {
        if (stage == ShipTutorial.ARMORY_DEPOSIT)
            stage = ShipTutorial.ARMORY_WITHDRAW;
    }

    public void weaponWithdrawn()
    {
        if (stage == ShipTutorial.ARMORY_WITHDRAW)
            stage = ShipTutorial.MAP_ENTER;
    }

    public void mapEntered()
    {
        if (stage == ShipTutorial.MAP_ENTER)
            stage = ShipTutorial.MAP_PICKUP;
    }
}
