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
// blue
        cityInformations.Add(Enums.CityName.Atlanta, new CityInformation(Enums.CityName.Atlanta, Enums.DiseaseColor.Blue, 
        new List<Enums.CityName>({ Enums.CityName.Chicago, Enums.CityName.Washington, Enums.CityName.Miami })));

        cityInformations.Add(Enums.CityName.SanFrancisco, new CityInformation(Enums.CityName.SanFrancisco, Enums.DiseaseColor.Blue, 
        new List<Enums.CityName>({ Enums.CityName.Chicago, Enums.CityName.LosAngeles, Enums.CityName.Tokyo, Enums.CityName.Manila})));

        cityInformations.Add(Enums.CityName.Chicago, new CityInformation(Enums.CityName.Chicago, Enums.DiseaseColor.Blue, 
        new List<Enums.CityName>({ Enums.CityName.SanFrancisco, Enums.CityName.LosAngeles, Enums.CityName.MexicoCity, Enums.CityName.Atlanta, Enums.CityName.Montreal })));

        cityInformations.Add(Enums.CityName.Montreal, new CityInformation(Enums.CityName.Montreal, Enums.DiseaseColor.Blue, 
        new List<Enums.CityName>({ Enums.CityName.Chicago, Enums.CityName.Washington, Enums.CityName.NewYork })));

        cityInformations.Add(Enums.CityName.NewYork, new CityInformation(Enums.CityName.NewYork, Enums.DiseaseColor.Blue, 
        new List<Enums.CityName>({ Enums.CityName.Montreal, Enums.CityName.Washington, Enums.CityName.London, Enums.CityName.Madrid })));

        cityInformations.Add(Enums.CityName.Washington, new CityInformation(Enums.CityName.Washington, Enums.DiseaseColor.Blue, 
        new List<Enums.CityName>({ Enums.CityName.Montreal, Enums.CityName.Atlanta, Enums.CityName.Miami, Enums.CityName.NewYork })));

        cityInformations.Add(Enums.CityName.London, new CityInformation(Enums.CityName.London, Enums.DiseaseColor.Blue, 
        new List<Enums.CityName>({ Enums.CityName.NewYork, Enums.CityName.Madrid, Enums.CityName.Paris, Enums.CityName.Essen })));

        cityInformations.Add(Enums.CityName.Essen, new CityInformation(Enums.CityName.Essen, Enums.DiseaseColor.Blue, 
        new List<Enums.CityName>({ Enums.CityName.London, Enums.CityName.Paris, Enums.CityName.Milan, Enums.CityName.StPetersburg })));

        cityInformations.Add(Enums.CityName.Madrid, new CityInformation(Enums.CityName.Madrid, Enums.DiseaseColor.Blue, 
        new List<Enums.CityName>({ Enums.CityName.London, Enums.CityName.NewYork, Enums.CityName.SaoPaulo, Enums.CityName.Paris, Enums.CityName.Algiers })));

        cityInformations.Add(Enums.CityName.Paris, new CityInformation(Enums.CityName.Paris, Enums.DiseaseColor.Blue, 
        new List<Enums.CityName>({ Enums.CityName.London, Enums.CityName.Madrid, Enums.CityName.Algiers, Enums.CityName.Milan, Enums.CityName.Essen })));

        cityInformations.Add(Enums.CityName.Milan, new CityInformation(Enums.CityName.Milan, Enums.DiseaseColor.Blue, 
        new List<Enums.CityName>({ Enums.CityName.Essen, Enums.CityName.Paris, Enums.CityName.Istanbul })));

        cityInformations.Add(Enums.CityName.StPetersburg, new CityInformation(Enums.CityName.StPetersburg, Enums.DiseaseColor.Blue, 
        new List<Enums.CityName>({ Enums.CityName.Essen, Enums.CityName.Istanbul, Enums.CityName.Moscow })));
// black
        cityInformations.Add(Enums.CityName.Algiers, new CityInformation(Enums.CityName.Algiers, Enums.DiseaseColor.Black, 
        new List<Enums.CityName>({ Enums.CityName.Paris, Enums.CityName.Madrid, Enums.CityName.Cairo, Enums.CityName.Istanbul })));

        cityInformations.Add(Enums.CityName.Istanbul, new CityInformation(Enums.CityName.Istanbul, Enums.DiseaseColor.Black, 
        new List<Enums.CityName>({ Enums.CityName.Istanbul, Enums.CityName.Milan, Enums.CityName.Algiers, Enums.CityName.Cairo, Enums.CityName.Baghdad, Enums.CityName.Moscow })));

        cityInformations.Add(Enums.CityName.Moscow, new CityInformation(Enums.CityName.Moscow, Enums.DiseaseColor.Black, 
        new List<Enums.CityName>({ Enums.CityName.StPetersburg, Enums.CityName.Istanbul, Enums.CityName.Tehran })));

        cityInformations.Add(Enums.CityName.Cairo, new CityInformation(Enums.CityName.Cairo, Enums.DiseaseColor.Black, 
        new List<Enums.CityName>({ Enums.CityName.Istanbul, Enums.CityName.Algiers, Enums.CityName.Khartoum, Enums.CityName.Riyadh, Enums.CityName.Baghdad })));

        cityInformations.Add(Enums.CityName.Baghdad, new CityInformation(Enums.CityName.Baghdad, Enums.DiseaseColor.Black, 
        new List<Enums.CityName>({ Enums.CityName.Istanbul, Enums.CityName.Cairo, Enums.CityName.Riyadh, Enums.CityName.Karachi, Enums.CityName.Tehran })));

        cityInformations.Add(Enums.CityName.Tehran, new CityInformation(Enums.CityName.Tehran, Enums.DiseaseColor.Black, 
        new List<Enums.CityName>({ Enums.CityName.Moscow, Enums.CityName.Baghdad, Enums.CityName.Karachi, Enums.CityName.Delhi })));

        cityInformations.Add(Enums.CityName.Riyadh, new CityInformation(Enums.CityName.Riyadh, Enums.DiseaseColor.Black, 
        new List<Enums.CityName>({ Enums.CityName.Cairo, Enums.CityName.Baghdad, Enums.CityName.Karachi })));

        cityInformations.Add(Enums.CityName.Karachi, new CityInformation(Enums.CityName.Karachi, Enums.DiseaseColor.Black, 
        new List<Enums.CityName>({ Enums.CityName.Tehran, Enums.CityName.Baghdad, Enums.CityName.Riyadh, Enums.CityName.Mumbai, Enums.CityName.Delhi })));

        cityInformations.Add(Enums.CityName.Delhi, new CityInformation(Enums.CityName.Delhi, Enums.DiseaseColor.Black, 
        new List<Enums.CityName>({ Enums.CityName.Tehran, Enums.CityName.Karachi, Enums.CityName.Mumbai, Enums.CityName.Chennai, Enums.CityName.Kolkata })));

        cityInformations.Add(Enums.CityName.Mumbai, new CityInformation(Enums.CityName.Mumbai, Enums.DiseaseColor.Black, 
        new List<Enums.CityName>({ Enums.CityName.Karachi, Enums.CityName.Delhi, Enums.CityName.Chennai })));

        cityInformations.Add(Enums.CityName.Chennai, new CityInformation(Enums.CityName.Chennai, Enums.DiseaseColor.Black, 
        new List<Enums.CityName>({ Enums.CityName.Mumbai, Enums.CityName.Delhi, Enums.CityName.Kolkata, Enums.CityName.Bangkok, Enums.CityName.Jakarta })));

        cityInformations.Add(Enums.CityName.Kolkata, new CityInformation(Enums.CityName.Kolkata, Enums.DiseaseColor.Black, 
        new List<Enums.CityName>({ Enums.CityName.Delhi, Enums.CityName.Chennai, Enums.CityName.Bangkok, Enums.CityName.HongKong })));
// red
        cityInformations.Add(Enums.CityName.Beijing, new CityInformation(Enums.CityName.Beijing, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Shanghai, Enums.CityName.Seoul })));

        cityInformations.Add(Enums.CityName.Seoul, new CityInformation(Enums.CityName.Seoul, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Beijing, Enums.CityName.Shanghai, Enums.CityName.Tokyo })));

        cityInformations.Add(Enums.CityName.Shanghai, new CityInformation(Enums.CityName.Shanghai, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Beijing, Enums.CityName.HongKong, Enums.CityName.Taipei, Enums.CityName.Tokyo, Enums.CityName.Seoul })));

        cityInformations.Add(Enums.CityName.Tokyo, new CityInformation(Enums.CityName.Tokyo, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Seoul, Enums.CityName.Shanghai, Enums.CityName.Dsaka, Enums.CityName.SanFrancisco })));

        cityInformations.Add(Enums.CityName.Dsaka, new CityInformation(Enums.CityName.Dsaka, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Tokyo, Enums.CityName.Taipei })));

        cityInformations.Add(Enums.CityName.Taipei, new CityInformation(Enums.CityName.Taipei, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Shanghai, Enums.CityName.HongKong, Enums.CityName.Manila, Enums.CityName.Dsaka })));

        cityInformations.Add(Enums.CityName.HongKong, new CityInformation(Enums.CityName.HongKong, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Shanghai, Enums.CityName.Kolkata, Enums.CityName.Bangkok, Enums.CityName.HoChiMinhCity, Enums.CityName.Manila, Enums.CityName.Taipei })));

        cityInformations.Add(Enums.CityName.Bangkok, new CityInformation(Enums.CityName.Bangkok, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Kolkata, Enums.CityName.Chennai, Enums.CityName.Jakarta, Enums.CityName.HoChiMinhCity, Enums.CityName.HongKong })));

        cityInformations.Add(Enums.CityName.Jakarta, new CityInformation(Enums.CityName.Jakarta, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Chennai, Enums.CityName.Bangkok, Enums.CityName.HoChiMinhCity, Enums.CityName.Sydney })));

        cityInformations.Add(Enums.CityName.HoChiMinhCity, new CityInformation(Enums.CityName.HoChiMinhCity, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Bangkok, Enums.CityName.Jakarta, Enums.CityName.Manila, Enums.CityName.HongKong })));

        cityInformations.Add(Enums.CityName.Manila, new CityInformation(Enums.CityName.Manila, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Taipei, Enums.CityName.HongKong, Enums.CityName.HoChiMinhCity, Enums.CityName.Sydney, Enums.CityName.SanFrancisco })));

        cityInformations.Add(Enums.CityName.Sydney, new CityInformation(Enums.CityName.Sydney, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Jakarta, Enums.CityName.Manila, Enums.CityName.LosAngeles })));
    // yellow
        cityInformations.Add(Enums.CityName.LosAngeles, new CityInformation(Enums.CityName.LosAngeles, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.SanFrancisco, Enums.CityName.Sydney, Enums.CityName.MexicoCity, Enums.CityName.Chicago })));

        cityInformations.Add(Enums.CityName.MexicoCity, new CityInformation(Enums.CityName.MexicoCity, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Chicago, Enums.CityName.LosAngeles, Enums.CityName.Lima, Enums.CityName.Bogota })));

        cityInformations.Add(Enums.CityName.Miami, new CityInformation(Enums.CityName.Miami, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Atlanta, Enums.CityName.MexicoCity, Enums.CityName.Bogota, Enums.CityName.Washington })));

        cityInformations.Add(Enums.CityName.Bogota, new CityInformation(Enums.CityName.Bogota, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.MexicoCity, Enums.CityName.Miami, Enums.CityName.Lima, Enums.CityName.SaoPaulo, Enums.CityName.BuenosAries })));

        cityInformations.Add(Enums.CityName.Lima, new CityInformation(Enums.CityName.Lima, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.MexicoCity, Enums.CityName.Santiago, Enums.CityName.Bogota })));

        cityInformations.Add(Enums.CityName.Santiago, new CityInformation(Enums.CityName.Santiago, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Lima })));

        cityInformations.Add(Enums.CityName.SaoPaulo, new CityInformation(Enums.CityName.SaoPaulo, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Bogota, Enums.CityName.BuenosAries, Enums.CityName.Lagos, Enums.CityName.Madrid })));

        cityInformations.Add(Enums.CityName.BuenosAries, new CityInformation(Enums.CityName.BuenosAries, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Bogota, Enums.CityName.SaoPaulo })));

        cityInformations.Add(Enums.CityName.Lagos, new CityInformation(Enums.CityName.Lagos, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.SaoPaulo, Enums.CityName.Kinshasa, Enums.CityName.Khartoum })));

        cityInformations.Add(Enums.CityName.Khartoum, new CityInformation(Enums.CityName.Khartoum, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Lagos, Enums.CityName.Kinshasa, Enums.CityName.Johannesburg, Enums.CityName.Cairo })));

        cityInformations.Add(Enums.CityName.Kinshasa, new CityInformation(Enums.CityName.Kinshasa, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Lagos, Enums.CityName.Khartoum, Enums.CityName.Johannesburg })));

        cityInformations.Add(Enums.CityName.Johannesburg, new CityInformation(Enums.CityName.Johannesburg, Enums.DiseaseColor.Red, 
        new List<Enums.CityName>({ Enums.CityName.Kinshasa, Enums.CityName.Khartoum })));
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
        return cityInformations.Keys;
    }
}
