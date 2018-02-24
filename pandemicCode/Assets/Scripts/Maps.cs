using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maps : MonoBehaviour {

    private static Maps Instance = new Maps();
    private Dictionary<Enums.RoleKind, string> playerDescriptions = new Dictionary<Enums.RoleKind, string>();
    private Dictionary<Enums.RoleKind, Enums.RoleColor> roleColors = new Dictionary<Enums.RoleKind, Enums.RoleColor>();
    private Dictionary<Enums.CityName, CityInformation> cityInformations = new Dictionary<Enums.CityName, CityInformation>();

    private Maps(){
        playerDescriptions.Add(Enums.RoleKind.Scientist, "ABCDEFG(TO-DO)");
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
}
