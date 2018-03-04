using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpidemicCard : PlayerCard {
    private static EpidemicCard INSTANCE = new EpidemicCard();

    private EpidemicCard() : base(CardType.EpidemicCard)
    {
    }
    
    public EpidemicCard getEpidemicCard()
    {
        return INSTANCE;
    }
}
