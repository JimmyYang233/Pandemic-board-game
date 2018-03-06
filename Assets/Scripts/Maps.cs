using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maps : MonoBehaviour {

    private static Maps Instance = new Maps();
    private Dictionary<RoleKind, string> playerDescriptions = new Dictionary<RoleKind, string>();
    private Dictionary<RoleKind, Color> roleColors = new Dictionary<RoleKind, Color>();
    private Dictionary<CityName, CityInformation> cityInformations = new Dictionary<CityName, CityInformation>();
    //add event card description by shuran zheng
    private Dictionary<EventKind, string> eventCardDescription = new Dictionary<EventKind, string>();
    private List<Color> diseaseColor =new List<Color>();
    private Maps(){
        playerDescriptions.Add(RoleKind.ContingencyPlanner, "The Contingency Planner may, as an action, take an Event card from anywhere in the Player Discard Pile and place it on his Role card. Only 1 Event card can be on his role card at a time. It does not count against his hand limit. When the Contingency Planner plays the Event card on his role card, remove this Event card from the game (instead of discarding it).");
        playerDescriptions.Add(RoleKind.Dispatcher, "The Dispatcher may, as an action, either: 1.move any pawn, if its owner agrees, to any city containing another pawn, or 2. move another player’s pawn, if its owner agrees, as if it were his own.");
        playerDescriptions.Add(RoleKind.Medic, "The Medic removes all cubes, not 1, of the same color when doing the Treat Disease action. If a disease has been cured, he automatically removes all cubes of that color from a city, simply by entering it or being there. This does not take an action.");
        playerDescriptions.Add(RoleKind.OperationsExpert, "The Operations Expert may, as an action, either: 1. build a research station in his current city without discarding (or using) a City card, or 2. once per turn, move from a research station to any city by discarding any City card.");
        playerDescriptions.Add(RoleKind.QuarantineSpecialist, "The Quarantine Specialist prevents both outbreaks and the placement of disease cubes in the city she is in and all cities connected to that city. She does not affect cubes placed during setup.");
        playerDescriptions.Add(RoleKind.Researcher, "When doing the Share Knowledge action, the Researcher may give any City card from her hand to another player in the same city as her, without this card having to match her city. The transfer must be from her hand to the other player’s hand, but it can occur on either player’s turn.");
        playerDescriptions.Add(RoleKind.Scientist, "The Scientist needs only 4 (not 5) City cards of the same disease color to Discover a Cure for that disease.");
        playerDescriptions.Add(RoleKind.Archivist, "The Archivist’s hand limit is 8 cards. He may, once per turn, as an action, draw the City card that matches the city he is in from the Player Discard Pile into his hand.");
        playerDescriptions.Add(RoleKind.ContainmentSpecialist, "When the Containment Specialist enters a city, if 2 or more disease cubes of the same color are present, he removes 1 of them.");
        playerDescriptions.Add(RoleKind.Epidemiologist, "The Epidemiologist, once per turn and on her turn (only), may take any City card from a player in the same city. The other player must agree. Doing this is not an action.");
        playerDescriptions.Add(RoleKind.FieldOperative, "The Field Operative may, once per turn as an action, move 1 disease cube from the city he is in and place it as a sample on his role card. When he Discovers a Cure, he may replace exactly 2 of the needed City cards by returning 3 cubes of the cure color from his Role card to the supply.");
        playerDescriptions.Add(RoleKind.Generalist, "The Generalist may do up to 5 actions each turn.");
        playerDescriptions.Add(RoleKind.Troubleshooter, "The Troubleshooter, at the start of her turn, looks at as many Infection cards as the current infection rate (by taking them from the top of the Infection Deck, looking at them, and putting them back in the same order).");


        diseaseColor.Add(Color.black);
        diseaseColor.Add(Color.red);
        diseaseColor.Add(Color.blue);
        diseaseColor.Add(Color.yellow);

       // Colors are compared and chosen from http://www.99colors.net/dot-net-colors
        roleColors.Add(RoleKind.ContingencyPlanner, Color.mediumturquoise);
        roleColors.Add(RoleKind.Dispatcher, Color.orchid);
        roleColors.Add(RoleKind.Medic, Color.orange);
        roleColors.Add(RoleKind.OperationsExpert, Color.springgreen);
        roleColors.Add(RoleKind.QuarantineSpecialist, Color.limegreen);
        roleColors.Add(RoleKind.Researcher, Color.sienna);
        roleColors.Add(RoleKind.Scientist, Color.ghostwhite);
        roleColors.Add(RoleKind.Archivist, Color.blue);
        roleColors.Add(RoleKind.ContainmentSpecialist, Color.wheat);
        roleColors.Add(RoleKind.Epidemiologist, Color.pink);
        roleColors.Add(RoleKind.FieldOperative, Color.yellow);
        roleColors.Add(RoleKind.Generalist, Color.gray);
        roleColors.Add(RoleKind.Troubleshooter, Color.red);


// origin 5 eventcard
        eventCardDescription.Add(EventKind.Airlift, "Move a pawn to any city. If played on another player's pawn you must have permission to move it.");
        eventCardDescription.Add(EventKind.ResilientPopulation, "Remove from the game an infection card in the infection discard deck.");
        eventCardDescription.Add(EventKind.OneQuietNight, "Skip the next infect cities step (Do not flip over any infection cards).");
        eventCardDescription.Add(EventKind.Forecast, "Look at the top 6 cards of the infection deck and put them in any order you choose then put them back on top of the infection deck.");
        eventCardDescription.Add(EventKind.GovernmentGrant, "Put a research station on any city.");
//oth 8 eventcards
        eventCardDescription.Add(EventKind.CommercialTravelBan, "The infection rate is 1 until the current player’s next turn begins. Put this card in front of this player. Discard it when his next turn begins.");
        eventCardDescription.Add(EventKind.ReExaminedResearch, "Select a player. This player may draw any 1 city card from The player discard pile into his hand (discarding if ovei his hand limit).");
        eventCardDescription.Add(EventKind.RemoteTreatment, "Removec 2 Disease cubes from the board");
        eventCardDescription.Add(EventKind.BorrowedTime, "The player drawing this card may not immediately play it to continue taking actions (as that player is now in the Draw Cards step of his turn).");
        eventCardDescription.Add(EventKind.MobileHospital, "The Containment Specialist  rst removes 1 cube of each color with 2 or more cubes present and then removes 1 more cube (for each city entered).");
        eventCardDescription.Add(EventKind.NewAssignment, "If the current player was selected and he swapped either from or to the Generalist role, then he may do up to 5 actions this turn.");
        eventCardDescription.Add(EventKind.RapidVaccineDeployment, "This can be played only immediately after a Discover a Cure action. Each affected city must be connected to at least 1 other city affected by this event (unless this event affects just 1 city). At least 1 cube must be removed from each of these cities.");
        eventCardDescription.Add(EventKind.SpecialOrders, "As when moving other pawns with the Dispatcher, the current player must discard (or reveal) any cards used to move that pawn by Direct or Charter  ights. Powers that are part of movement (such as the Medic’s power to remove cured disease cubes or the Containment Specialist’s power) do occur.");

// blue
        CityName[] neighbors;
        neighbors = { CityName.Chicago, CityName.Washington, CityName.Miami };
        cityInformations.Add(CityName.Atlanta, new CityInformation(CityName.Atlanta, Color.blue, new List<CityName>(neighbors)));

        neighbors = { CityName.Chicago, CityName.LosAngeles, CityName.Tokyo, CityName.Manila};
        cityInformations.Add(CityName.SanFrancisco, new CityInformation(CityName.SanFrancisco, Color.blue, new List<CityName>(neighbors)));

        neighbors = { CityName.SanFrancisco, CityName.LosAngeles, CityName.MexicoCity, CityName.Atlanta, CityName.Montreal };
        cityInformations.Add(CityName.Chicago, new CityInformation(CityName.Chicago, Color.blue, new List<CityName>(neighbors)));

        neighbors = { CityName.Chicago, CityName.Washington, CityName.NewYork };
        cityInformations.Add(CityName.Montreal, new CityInformation(CityName.Montreal, Color.blue, new List<CityName>(neighbors)));

        neighbors = { CityName.Montreal, CityName.Washington, CityName.London, CityName.Madrid };
        cityInformations.Add(CityName.NewYork, new CityInformation(CityName.NewYork, Color.blue, new List<CityName>(neighbors)));

        neighbors = { CityName.Montreal, CityName.Atlanta, CityName.Miami, CityName.NewYork };
        cityInformations.Add(CityName.Washington, new CityInformation(CityName.Washington, Color.blue, new List<CityName>(neighbors)));


        neighbors = { CityName.NewYork, CityName.Madrid, CityName.Paris, CityName.Essen };
        cityInformations.Add(CityName.London, new CityInformation(CityName.London, Color.blue, new List<CityName>(neighbors)));

        neighbors = { CityName.London, CityName.Paris, CityName.Milan, CityName.StPetersburg }
        cityInformations.Add(CityName.Essen, new CityInformation(CityName.Essen, Color.blue, new List<CityName>(neighbors)));

        neighbors = { CityName.London, CityName.NewYork, CityName.SaoPaulo, CityName.Paris, CityName.Algiers }
        cityInformations.Add(CityName.Madrid, new CityInformation(CityName.Madrid, Color.blue, new List<CityName>(neighbors)));

        neighbors = { CityName.London, CityName.Madrid, CityName.Algiers, CityName.Milan, CityName.Essen }
        cityInformations.Add(CityName.Paris, new CityInformation(CityName.Paris, Color.blue, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Essen, CityName.Paris, CityName.Istanbul }
        cityInformations.Add(CityName.Milan, new CityInformation(CityName.Milan, Color.blue, new List<CityName>(neighbors)));

        neighbors = { CityName.Essen, CityName.Istanbul, CityName.Moscow }
        cityInformations.Add(CityName.StPetersburg, new CityInformation(CityName.StPetersburg, Color.blue, new List<CityName>(neighbors)));
// black
        neighbors = { CityName.Paris, CityName.Madrid, CityName.Cairo, CityName.Istanbul }
        cityInformations.Add(CityName.Algiers, new CityInformation(CityName.Algiers, Color.black, new List<CityName>(neighbors)));

        neighbors = { CityName.Istanbul, CityName.Milan, CityName.Algiers, CityName.Cairo, CityName.Baghdad, CityName.Moscow }
        cityInformations.Add(CityName.Istanbul, new CityInformation(CityName.Istanbul, Color.black, new List<CityName>(neighbors)));

        neighbors = { CityName.StPetersburg, CityName.Istanbul, CityName.Tehran }
        cityInformations.Add(CityName.Moscow, new CityInformation(CityName.Moscow, Color.black, new List<CityName>(neighbors)));

        neighbors = { CityName.Istanbul, CityName.Algiers, CityName.Khartoum, CityName.Riyadh, CityName.Baghdad }
        cityInformations.Add(CityName.Cairo, new CityInformation(CityName.Cairo, Color.black, new List<CityName>(neighbors)));

        neighbors = { CityName.Istanbul, CityName.Cairo, CityName.Riyadh, CityName.Karachi, CityName.Tehran }
        cityInformations.Add(CityName.Baghdad, new CityInformation(CityName.Baghdad, Color.black, new List<CityName>(neighbors)));

        neighbors = { CityName.Moscow, CityName.Baghdad, CityName.Karachi, CityName.Delhi }
        cityInformations.Add(CityName.Tehran, new CityInformation(CityName.Tehran, Color.black, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Cairo, CityName.Baghdad, CityName.Karachi }
        cityInformations.Add(CityName.Riyadh, new CityInformation(CityName.Riyadh, Color.black, new List<CityName>(neighbors)));

        neighbors = { CityName.Tehran, CityName.Baghdad, CityName.Riyadh, CityName.Mumbai, CityName.Delhi }
        cityInformations.Add(CityName.Karachi, new CityInformation(CityName.Karachi, Color.black, new List<CityName>(neighbors)));

        neighbors = { CityName.Tehran, CityName.Karachi, CityName.Mumbai, CityName.Chennai, CityName.Kolkata }
        cityInformations.Add(CityName.Delhi, new CityInformation(CityName.Delhi, Color.black, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Karachi, CityName.Delhi, CityName.Chennai }
        cityInformations.Add(CityName.Mumbai, new CityInformation(CityName.Mumbai, Color.black, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Mumbai, CityName.Delhi, CityName.Kolkata, CityName.Bangkok, CityName.Jakarta }
        cityInformations.Add(CityName.Chennai, new CityInformation(CityName.Chennai, Color.black, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Delhi, CityName.Chennai, CityName.Bangkok, CityName.HongKong }
        cityInformations.Add(CityName.Kolkata, new CityInformation(CityName.Kolkata, Color.black, new List<CityName>(neighbors)));
// red
        neighbors = { CityName.Shanghai, CityName.Seoul }
        cityInformations.Add(CityName.Beijing, new CityInformation(CityName.Beijing, Color.red, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Beijing, CityName.Shanghai, CityName.Tokyo }
        cityInformations.Add(CityName.Seoul, new CityInformation(CityName.Seoul, Color.red, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Beijing, CityName.HongKong, CityName.Taipei, CityName.Tokyo, CityName.Seoul }
        cityInformations.Add(CityName.Shanghai, new CityInformation(CityName.Shanghai, Color.red, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Seoul, CityName.Shanghai, CityName.Osaka, CityName.SanFrancisco }
        cityInformations.Add(CityName.Tokyo, new CityInformation(CityName.Tokyo, Color.red, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Tokyo, CityName.Taipei }
        cityInformations.Add(CityName.Osaka, new CityInformation(CityName.Osaka, Color.red, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Shanghai, CityName.HongKong, CityName.Manila, CityName.Osaka }
        cityInformations.Add(CityName.Taipei, new CityInformation(CityName.Taipei, Color.red, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Shanghai, CityName.Kolkata, CityName.Bangkok, CityName.HoChiMinhCity, CityName.Manila, CityName.Taipei }
        cityInformations.Add(CityName.HongKong, new CityInformation(CityName.HongKong, Color.red, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Kolkata, CityName.Chennai, CityName.Jakarta, CityName.HoChiMinhCity, CityName.HongKong }
        cityInformations.Add(CityName.Bangkok, new CityInformation(CityName.Bangkok, Color.red, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Chennai, CityName.Bangkok, CityName.HoChiMinhCity, CityName.Sydney }
        cityInformations.Add(CityName.Jakarta, new CityInformation(CityName.Jakarta, Color.red, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Bangkok, CityName.Jakarta, CityName.Manila, CityName.HongKong }
        cityInformations.Add(CityName.HoChiMinhCity, new CityInformation(CityName.HoChiMinhCity, Color.red, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Taipei, CityName.HongKong, CityName.HoChiMinhCity, CityName.Sydney, CityName.SanFrancisco }
        cityInformations.Add(CityName.Manila, new CityInformation(CityName.Manila, Color.red, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Jakarta, CityName.Manila, CityName.LosAngeles }
        cityInformations.Add(CityName.Sydney, new CityInformation(CityName.Sydney, Color.red, new List<CityName>(neighbors)));
    // yellow
        neighbors = { CityName.SanFrancisco, CityName.Sydney, CityName.MexicoCity, CityName.Chicago }
        cityInformations.Add(CityName.LosAngeles, new CityInformation(CityName.LosAngeles, Color.yellow, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Chicago, CityName.LosAngeles, CityName.Lima, CityName.Bogota }
        cityInformations.Add(CityName.MexicoCity, new CityInformation(CityName.MexicoCity, Color.yellow, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Atlanta, CityName.MexicoCity, CityName.Bogota, CityName.Washington }
        cityInformations.Add(CityName.Miami, new CityInformation(CityName.Miami, Color.yellow, new List<CityName>(neighbors)));
        
        neighbors = { CityName.MexicoCity, CityName.Miami, CityName.Lima, CityName.SaoPaulo, CityName.BuenosAries }
        cityInformations.Add(CityName.Bogota, new CityInformation(CityName.Bogota, Color.yellow, new List<CityName>(neighbors)));
        
        neighbors = { CityName.MexicoCity, CityName.Santiago, CityName.Bogota }
        cityInformations.Add(CityName.Lima, new CityInformation(CityName.Lima, Color.yellow, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Lima }
        cityInformations.Add(CityName.Santiago, new CityInformation(CityName.Santiago, Color.yellow, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Bogota, CityName.BuenosAries, CityName.Lagos, CityName.Madrid }
        cityInformations.Add(CityName.SaoPaulo, new CityInformation(CityName.SaoPaulo, Color.yellow, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Bogota, CityName.SaoPaulo }
        cityInformations.Add(CityName.BuenosAries, new CityInformation(CityName.BuenosAries, Color.yellow, new List<CityName>(neighbors)));
        
        neighbors = { CityName.SaoPaulo, CityName.Kinshasa, CityName.Khartoum }
        cityInformations.Add(CityName.Lagos, new CityInformation(CityName.Lagos, Color.yellow, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Lagos, CityName.Kinshasa, CityName.Johannesburg, CityName.Cairo }
        cityInformations.Add(CityName.Khartoum, new CityInformation(CityName.Khartoum, Color.yellow, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Lagos, CityName.Khartoum, CityName.Johannesburg }
        cityInformations.Add(CityName.Kinshasa, new CityInformation(CityName.Kinshasa, Color.yellow, new List<CityName>(neighbors)));
        
        neighbors = { CityName.Kinshasa, CityName.Khartoum }
        cityInformations.Add(CityName.Johannesburg, new CityInformation(CityName.Johannesburg, Color.yellow, new List<CityName>(neighbors)));
    }

    public static Maps getInstance()
    {
        return Instance;
    }

    public string getDescription(RoleKind roleKind)
    {
        return playerDescriptions[roleKind];
    }

    public Color getRoleColor(RoleKind roleKind)
    {
        return roleColors[roleKind];
    }

    public List<CityName> getNeighbors(CityName cityName)
    {
        return cityInformations[cityName].getNeighbors();
    }

    public Color getCityColor(CityName cityName)
    {
        return cityInformations[cityName].getColor();
    }

    public List<CityName> getCityNames()
    {
        List<CityName> tmp = new List<CityName>(cityInformations.Keys);
        return tmp;
    }

    public List<EventKind> getEventNames()
    {
        List<EventKind> tmp = new List<EventKind>(eventCardDescription.Keys);
        return tmp;
    }


    public List<Color> getDiseaseColor()
    {
        
        return diseaseColor;
    }
}
