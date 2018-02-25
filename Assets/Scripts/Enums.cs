using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour {

    public enum Difficulty { Introductory, Standard, Heroic, Legendary};	//done
    public enum DiseaseColor { Red,Blue,Yellow,Black,Purple}; 	//done
    public enum RoleColor {Red, Blue, Gray };   //to do
    public enum GamePhase { ReadyToJoin,SetupGame,PlayerTakeTurn,InfectCities,PlayerDrawCard,Completed}; 	//done?
    public enum RoleKind { ContingencyPlanner, Dispatcher, Medic, OperationsExpert, QuarantineSpecialist, Researcher, Scientist, Archivist, BioTerrorist, 
		ContainmentSpecialist, Epidemiologist, FieldOperative, Generalist, Troubleshooter};    //done
    public enum EventKind{ResilientPopulation, Airlift, OneQuietNight, Forecast, GovernmentGrant, BorrowedTime, CommercialTravelBan, MobileHospital, NewAssignment, RapidVaccineDeployment,
	ReExaminedResearch, RemoteTreatment, SpecialOrders}; //13 in total, done
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
    };	//done
    public enum Challenge { Nochallenge, Mutation, BioTerroist,VirulentStrain};	//done
    public enum CardType {CityCard, EpidemicCard, EventCard, InfectionCard };	//done
	public enum PlayerStatus {Offline, Available, Ready, InGame};				//done
}
