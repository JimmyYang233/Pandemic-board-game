using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EpidemicCard : PlayerCard {
    private static EpidemicCard INSTANCE = new EpidemicCard();
	private string name = "Epidemic";

    private List<int> intList = new List<int>(){5,6,7,3,0,1,2};

    private EpidemicCard() : base(CardType.EpidemicCard)
    {
    }
    
    public static EpidemicCard getEpidemicCard()
    {
        return INSTANCE;
    }

	public override string getName(){
		return name;
	}

    public VirulentStrainEpidemicEffects getVirulentStrainEpidemicEffects(){
        if(intList.Count>0){
            // int index = UnityEngine.Random.Range(0, intList.Count);
            int index = 0;
            VirulentStrainEpidemicEffects vse =  (VirulentStrainEpidemicEffects)(intList[index]);
            intList.RemoveAt(index);
            return vse;
        }
        else{
            return (VirulentStrainEpidemicEffects)0;
        }
    }

    public List<int> getIntList(){
        return intList;
    }

    public void setIntList(List<int> list){
        intList = list;
    }
}
