using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class challengeSelection : MonoBehaviour {

    public Button StartGame;
    public Button HideRoom;
    public Button LeaveRoom;
    public Toggle VirulentStrainChallenge;
    public Toggle MutationChallenge;
    public Toggle BioTerroristChallenge;


	private void Start(){
		

	}

    private void Update()
    {	
        if (PhotonNetwork.isMasterClient)
        {
            StartGame.interactable = true;
            HideRoom.interactable = true;
            VirulentStrainChallenge.interactable = true;
            MutationChallenge.interactable = true;
            BioTerroristChallenge.interactable = true;
        }
        else
        {
            StartGame.interactable = false;
            HideRoom.interactable = false;
            VirulentStrainChallenge.interactable = false;
            MutationChallenge.interactable = false;
            BioTerroristChallenge.interactable = false;
			if (PhotonNetwork.room != null) {
				Challenge challenge = (Challenge)Enum.Parse (typeof(Challenge), (string)PhotonNetwork.room.CustomProperties["Challenge"]);
				if (challenge.Equals (Challenge.Nochallenge)) {
					VirulentStrainChallenge.isOn = false;
					MutationChallenge.isOn = false;
					BioTerroristChallenge.isOn = false;
				}
				else if (challenge.Equals (Challenge.BioTerroist)) {
					VirulentStrainChallenge.isOn = false;
					MutationChallenge.isOn = false;
					BioTerroristChallenge.isOn = true;
				}
				else if (challenge.Equals (Challenge.BioTerroistAndVirulentStrain)) {
					VirulentStrainChallenge.isOn = true;
					MutationChallenge.isOn = false;
					BioTerroristChallenge.isOn = true;
				}
				else if (challenge.Equals (Challenge.Mutation)) {
					VirulentStrainChallenge.isOn = false;
					MutationChallenge.isOn = true;
					BioTerroristChallenge.isOn = false;
				}
				else if (challenge.Equals (Challenge.MutationAndVirulentStrain)) {
					VirulentStrainChallenge.isOn = true;
					MutationChallenge.isOn = true;
					BioTerroristChallenge.isOn = false;
				}
				else if (challenge.Equals (Challenge.VirulentStrain)) {
					VirulentStrainChallenge.isOn = true;
					MutationChallenge.isOn = false;
					BioTerroristChallenge.isOn = false;
				}
			
			}
			//if(PhotonNetwork.room.CustomProperties["Challenge"]
        }
		if (!PlayerNetwork.Instance.isNewGame){
			StartGame.interactable = false;
			HideRoom.interactable = false;
			VirulentStrainChallenge.interactable = false;
			MutationChallenge.interactable = false;
			BioTerroristChallenge.interactable = false;
			if (PhotonNetwork.room != null) {
				Challenge challenge = (Challenge)Enum.Parse (typeof(Challenge), (string)PhotonNetwork.room.CustomProperties["Challenge"]);
				if (challenge == Challenge.BioTerroist || challenge == Challenge.BioTerroistAndVirulentStrain) {
					BioTerroristChallenge.isOn = true;
				}
				if (challenge == Challenge.Mutation || challenge == Challenge.MutationAndVirulentStrain) {
					MutationChallenge.isOn = true;
				}
				if (challenge == Challenge.VirulentStrain || challenge == Challenge.MutationAndVirulentStrain) {
					VirulentStrainChallenge.isOn = true;
				}
			}

		}
    }

    public void MutationChallengeToggled()
    {
        if (MutationChallenge.isOn)
        {
            BioTerroristChallenge.isOn = false;
        }
		ExitGames.Client.Photon.Hashtable roomProperty = new ExitGames.Client.Photon.Hashtable ();
		roomProperty.Add ("Challenge", getChallenge().ToString() );
		PhotonNetwork.room.SetCustomProperties (roomProperty);
		Debug.Log (PhotonNetwork.room.CustomProperties);

    }

    public void BioTerroristChallengeToggled()
    {
        if (BioTerroristChallenge.isOn)
        {
            MutationChallenge.isOn = false;
        }
		ExitGames.Client.Photon.Hashtable roomProperty = new ExitGames.Client.Photon.Hashtable ();
		roomProperty.Add ("Challenge", getChallenge().ToString() );
		PhotonNetwork.room.SetCustomProperties (roomProperty);
		Debug.Log (PhotonNetwork.room.CustomProperties);
       
    }

	public void VirulentStrainChallengeToggled(){
		ExitGames.Client.Photon.Hashtable roomProperty = new ExitGames.Client.Photon.Hashtable ();
		roomProperty.Add ("Challenge", getChallenge().ToString() );
		PhotonNetwork.room.SetCustomProperties (roomProperty);
		//Debug.Log (PhotonNetwork.room.CustomProperties);
	}

    public Challenge getChallenge()
    {
        if (VirulentStrainChallenge.isOn)
        {
            if (MutationChallenge.isOn)
            {
                return Challenge.MutationAndVirulentStrain;
            }
            else if (BioTerroristChallenge.isOn)
            {
                return Challenge.BioTerroistAndVirulentStrain;
            }
            else
            {
                return Challenge.VirulentStrain;
            }
        }
        else
        {
            if (MutationChallenge.isOn)
            {
                return Challenge.Mutation;
            }
            else if (BioTerroristChallenge.isOn)
            {
                return Challenge.BioTerroist;
            }
            else
            {
                return Challenge.Nochallenge;
            }
        }

    }

}
