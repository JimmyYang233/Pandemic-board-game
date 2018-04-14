using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class challengeSelection : MonoBehaviour {

    public Button StartGame;
    public Button HideRoom;
    public Button LeaveRoom;
    public Toggle VirulentStrainChallenge;
    public Toggle MutationChallenge;
    public Toggle BioTerroristChallenge;

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
        }
    }

    public void MutationChallengeToggled()
    {
        if (MutationChallenge.isOn)
        {
            BioTerroristChallenge.isOn = false;
        }
    }

    public void BioTerroristChallengeToggled()
    {
        if (BioTerroristChallenge.isOn)
        {
            MutationChallenge.isOn = false;
        }
       
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
