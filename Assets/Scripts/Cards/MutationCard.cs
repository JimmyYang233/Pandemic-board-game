using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationCard : InfectionCard {

    private static MutationCard INSTANCE = new MutationCard();
    private Color color = Color.magenta;
	private string name = "MuatationCard";


    private MutationCard(): base(CardType.MutationCard)
    {}

    public static MutationCard getMutationCard()
    {
        return INSTANCE;
    }

    public Color getColor()
    {
        return color;
    }

	public override string getName(){
		return name;
	}
    
}