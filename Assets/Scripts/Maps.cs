using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maps : MonoBehaviour {

    private static Maps Instance = new Maps();
    private Dictionary<Enums.RoleKind, string> playerDescriptions = new Dictionary<Enums.RoleKind, string>();
    private Dictionary<Enums.RoleKind, Enums.RoleColor> roleColors = new Dictionary<Enums.RoleKind, Enums.RoleColor>();
    private Dictionary<Enums.CityName, CityInformation> cityInformations = new Dictionary<Enums.CityName, CityInformation>();

    private Maps(){
        playerDescriptions.Add(Enums.RoleKind.ContingencyPlanner, "The Contingency Planner may, as an action, take an Event card from anywhere in the Player Discard Pile and place it on his Role card. Only 1 Event card can be on his role card at a time. It does not count against his hand limit. When the Contingency Planner plays the Event card on his role card, remove this Event card from the game (instead of discarding it).");
        playerDescriptions.Add(Enums.RoleKind.Dispatcher, "The Dispatcher may, as an action, either: 1.move any pawn, if its owner agrees, to any city containing another pawn, or 2. move another player’s pawn, if its owner agrees, as if it were his own.");
        playerDescriptions.Add(Enums.RoleKind.Medic, "The Medic removes all cubes, not 1, of the same color when doing the Treat Disease action. If a disease has been cured, he automatically removes all cubes of that color from a city, simply by entering it or being there. This does not take an action.");
        playerDescriptions.Add(Enums.RoleKind.OperationsExpert, "The Operations Expert may, as an action, either: 1. build a research station in his current city without discarding (or using) a City card, or 2. once per turn, move from a research station to any city by discarding any City card.");
        playerDescriptions.Add(Enums.RoleKind.QuarantineSpecialist, "The Quarantine Specialist prevents both outbreaks and the placement of disease cubes in the city she is in and all cities connected to that city. She does not affect cubes placed during setup.");
        playerDescriptions.Add(Enums.RoleKind.Researcher, "When doing the Share Knowledge action, the Researcher may give any City card from her hand to another player in the same city as her, without this card having to match her city. The transfer must be from her hand to the other player’s hand, but it can occur on either player’s turn.");
        playerDescriptions.Add(Enums.RoleKind.Scientist, "The Scientist needs only 4 (not 5) City cards of the same disease color to Discover a Cure for that disease.");
        playerDescriptions.Add(Enums.RoleKind.Archivist, "The Archivist’s hand limit is 8 cards. He may, once per turn, as an action, draw the City card that matches the city he is in from the Player Discard Pile into his hand.");
        playerDescriptions.Add(Enums.RoleKind.ContainmentSpecialist, "When the Containment Specialist enters a city, if 2 or more disease cubes of the same color are present, he removes 1 of them.");
        playerDescriptions.Add(Enums.RoleKind.Epidemiologist, "The Epidemiologist, once per turn and on her turn (only), may take any City card from a player in the same city. The other player must agree. Doing this is not an action.");
        playerDescriptions.Add(Enums.RoleKind.FieldOperative, "The Field Operative may, once per turn as an action, move 1 disease cube from the city he is in and place it as a sample on his role card. When he Discovers a Cure, he may replace exactly 2 of the needed City cards by returning 3 cubes of the cure color from his Role card to the supply.");
        playerDescriptions.Add(Enums.RoleKind.Generalist, "The Generalist may do up to 5 actions each turn.");
        playerDescriptions.Add(Enums.RoleKind.Troubleshooter, "The Troubleshooter, at the start of her turn, looks at as many Infection cards as the current infection rate (by taking them from the top of the Infection Deck, looking at them, and putting them back in the same order).");


        //TO-DO
        roleColors.Add(Enums.RoleKind.Scientist, Enums.RoleColor.Gray);
        //TO-DO
        Enums.CityName[] neighbors = { Enums.CityName.Chicago, Enums.CityName.Washington, Enums.CityName.Miami };
        cityInformations.Add(Enums.CityName.Atlanta, new CityInformation(Enums.CityName.Atlanta, Enums.DiseaseColor.Blue, new List<Enums.CityName>(neighbors)));
        //TO-DO
    }

    public static Maps getInstance()
    {
        return Instance;
    }

    public string getDescription(Enums.RoleKind roleKind)
    {
        return playerDescriptions[roleKind];
    }

    public Enums.RoleColor getRoleColor(Enums.RoleKind roleKind)
    {
        return roleColors[roleKind];
    }

    public List<Enums.CityName> getNeighbors(Enums.CityName cityName)
    {
        return cityInformations[cityName].getNeighbors();
    }

    public Enums.DiseaseColor getCityColor(Enums.CityName cityName)
    {
        return cityInformations[cityName].getColor();
    }

    public List<Enums.CityName> getCityNames()
    {
        List<Enums.CityName> tmp = new List<Enums.CityName>(cityInformations.Keys);
        return tmp;
    }
}
