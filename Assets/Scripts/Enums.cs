using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour {

    public enum Difficulty { Introductory, Standard, Heroic};
    public enum DiseaseColor { Red,Blue,Yellow,Black,Purple};
    public enum RoleColor {Red, Blue, Gray };
    public enum GamePhase { ReadyToJoin,SetupGame,PlayerTakeTurn,InfectCities,PlayerDrawCard,Completed};
    public enum RoleKind { ContingencyPlanner, Dispatcher, Medic, OperationsExpert, QuarantineSpecialist, Researcher, Scientist,Archivist,ContenmentSpecialist,Epidemiologist,FieldOperative,Generalist,Troubleshooter};
    public enum EventKind{ResilientPopulation, Airlift, OneQuietNight, Forecast, GovernmentGrant };
    public enum CityName {
        Algiers,
Atlanta,
Baghdad,
Bangkok,
Beijing,
Bogota,
BuenosAries,
Cairo,
Chennai,
Chicago,
Delhi,
Essen,
HoChiMinhCity,
HongKong,
Istanbul,
Jakarta,
Johannesburg,
Karachi,
Khartoum,
Kinshasa,
Kolkata,
Lagos,
Lima,
London,
LosAngeles,
Madrid,
Manila,
MexicoCity,
Miami,
Milan,
Montreal,
Moscow,
Mumbai,
NewYork,
Osaka,
Paris,
Riyadh,
SanFrancisco,
Santiago,
SaoPaulo,
Seoul,
Shanghai,
StPetersburg,
Sydney,
Taipei,
Tehran,
Tokyo,
Washington
    };
    public enum Challenge { Nochallenge, Mutation, BioTerroist,VirulentStrain, MutationAddVirulentStrain,VirulentStrainAddBioTerroist};
    public enum CardType {CityCard, EpidemicCard, EventCard, InfectionCard };
}
