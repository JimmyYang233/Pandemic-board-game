using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour {

    public enum Difficulty { Introductory, Standard, Heroic};
    public enum DiseaseColor { Red,Blue,Yellow,Black,Purple};
    public enum RoleColor {Red, Blue, Gray };
    public enum GamePhase { ReadyToJoin,SetupGame,PlayerTakeTurn,InfectCities,PlayerDrawCard,Completed};
    public enum RoleKind { ContingencyPlanner, Dispatcher, Medic, OperationsExpert, QuarantineSpecialist, Researcher, Scientist,Archivist};
    public enum EventKind{ResilientPopulation, Airlift, OneQuietNight, Forecast, GovernmentGrant };
    public enum CityName { Algiers, Atlanta, Baghdad, Bangkok, Beijing, Bogota, Cairo, BuenosAires, Chennai, Chicago, Washington, Miami};
    public enum Challenge { Nochallenge, Mutation, BioTerroist,VirulentStrain, MutationAddVirulentStrain,VirulentStrainAddBioTerroist};
    public enum CardType {CityCard, EpidemicCard, EventCard, InfectionCard };
}
