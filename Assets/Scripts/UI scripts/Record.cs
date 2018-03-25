using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Record : MonoBehaviour {
    public GameObject textDisplay;
    public GameObject scrollBar;

    private void displayRecord(string text)
    {
        // Debug.Log(text);
        string previousText = textDisplay.GetComponent<Text>().text;
        textDisplay.GetComponent<Text>().text = previousText  + text + " \n";
        scrollBar.GetComponent<Scrollbar>().value = 0;
    }

    public void drive(Player player, City city){
        displayRecord(player.getRoleKind().ToString() + " drove to " + city.getCityName().ToString());
    }

    public void directFlight(Player player, City city){
        displayRecord(player.getRoleKind().ToString() + " took the direct flight to " + city.getCityName().ToString());
    }

    public void charterFlight(Player player, City city){
        displayRecord(player.getRoleKind().ToString() + " took the charter flight to " + city.getCityName().ToString());
    }

    public void shuttleFlight(Player player, City city){
        displayRecord(player.getRoleKind().ToString() + " took the shuttle flight to " + city.getCityName().ToString());
    }

    public void treat(Player player, int num, Disease d, City city){
        if (d.getColor() == Color.black){
            displayRecord(player.getRoleKind().ToString() + " treated "+ num.ToString()+" black cube in " + city.getCityName().ToString());
        }else if(d.getColor() == Color.blue){
            displayRecord(player.getRoleKind().ToString() + " treated "+ num.ToString()+" blue cube in " + city.getCityName().ToString());
        }else if(d.getColor() == Color.yellow){
            displayRecord(player.getRoleKind().ToString() + " treated "+ num.ToString()+" yellow cube in " + city.getCityName().ToString());
        }else if(d.getColor() == Color.red){
            displayRecord(player.getRoleKind().ToString() + " treated "+ num.ToString()+" red cube in " + city.getCityName().ToString());
        }else{
            displayRecord(player.getRoleKind().ToString() + " treated "+ num.ToString()+" cube in " + city.getCityName().ToString());
        }
    }

    public void takeCard(Player p1, Player p2, CityCard card){
        displayRecord(p1.getRoleKind().ToString() + " took the City Card: "+ card.getCity().getCityName().ToString()+ " from " + p2.getRoleKind().ToString());
    }

    public void giveCard(Player p1, Player p2, CityCard card){
        displayRecord(p1.getRoleKind().ToString() + " gave the City Card: "+ card.getCity().getCityName().ToString()+ " to " + p2.getRoleKind().ToString());
    }

    public void cure(){}

    public void build(City city){
        displayRecord("A research station is built in "+ city.getCityName().ToString());
    }

    public void pass(Player p1){
        displayRecord(p1.getRoleKind().ToString() + "'s turn ended.");
    }

    public void draw(Player player, PlayerCard card){
        switch (card.getType())
        {
            case CardType.CityCard:
                displayRecord(player.getRoleKind().ToString() + " drew a City Card: "+ ((CityCard)card).getCity().getCityName().ToString());
                break;
            case CardType.EventCard:
                displayRecord(player.getRoleKind().ToString() + " drew an Event Card: "+ ((EventCard)card).getEventKind().ToString());
                break;
            case CardType.EpidemicCard:
                displayRecord(player.getRoleKind().ToString() + " drew an Epidemic Card");
                break;
            default:
                displayRecord(player.getRoleKind().ToString() + " drew a card");
                break;
        }
       
    }

    public void discard(Player player, PlayerCard card){
        switch (card.getType())
        {
            case CardType.CityCard:
                displayRecord(player.getRoleKind().ToString() + " discarded a Player Card: "+ ((CityCard)card).getCity().getCityName().ToString());
                break;
            case CardType.EventCard:
                displayRecord(player.getRoleKind().ToString() + " discarded an Event Card: "+ ((EventCard)card).getEventKind().ToString());
                break;
            case CardType.EpidemicCard:
                displayRecord(player.getRoleKind().ToString() + " discarded an Epidemic Card");
                break;
            default:
                displayRecord(player.getRoleKind().ToString() + " discarded a card");
                break;
        }
       
    }


    public void infect(City city, int number, bool hasMedic,bool hasQS,bool isCured,bool isEradicated){
        displayRecord("Infection in "+ city.getCityName().ToString()+": "+number.ToString()+" cube will be added!");
        if(hasMedic&&isCured){
            displayRecord("Infection is prevented by Medic");
        }else if(hasQS){
            displayRecord("Infection is prevented by Quarantine Specialist");
        }if(isEradicated){
            displayRecord("Desease has been eradicated~");
        }
    }

    
}
