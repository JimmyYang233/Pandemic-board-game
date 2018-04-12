using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BioTerrorist : Role {
    private bool bioTerroristExtraDriveUsed = false;
    private bool isCaptured = false;
    private bool infectLocallyUsed = false;
    private bool infectRemotelyUsed = false;
    private bool isSpotted = false;

    public BioTerrorist() : base(RoleKind.BioTerrorist)
    {
    }

    public bool getinfectLocallyUsed()
    {
        return infectLocallyUsed;
    }

    public bool getInfectRemotelyUsed()
    {
        return infectRemotelyUsed;
    }

    public void useInfectLocally()
    {
        infectLocallyUsed = true;
    }

    public void useInfectRemotely()
    {
        infectRemotelyUsed = true;
    }
    
    public void refillDriveAction()
    {
        bioTerroristExtraDriveUsed = false;
        infectLocallyUsed = false;
        infectRemotelyUsed = false;
    }

    public bool getBioTerroristExtraDriveUsed()
    {
        return bioTerroristExtraDriveUsed;
    }

    public void setbioTerroristExtraDriveUsed()
    {
        bioTerroristExtraDriveUsed = true;
    }

	public bool getIsCaptured(){
		return isCaptured;
	}

    public void setCaptured()
    {
        isCaptured = true;
    }

    public void spot()
    {
        isSpotted = true;
    }

    public void unSpot()
    {
        isSpotted = false;
    }

    public bool getIsSpotted()
    {
        return isSpotted;
    }
}
