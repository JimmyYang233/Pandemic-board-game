using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpidemicCard : PlayerCard {
    private EpidemicCard() : base(CardType.EpidemicCard)
    {
    }

    private static EpidemicCard INSTANCE = new EpidemicCard();

    public EpidemicCard getEpidemicCard()
    {
        return INSTANCE;
    }
}
