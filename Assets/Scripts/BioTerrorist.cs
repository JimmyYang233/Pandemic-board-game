using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BioTerrorist : Role {
    private bool bioTerroristExtraDriveUsed;
    private bool isCaptured;


    public BioTerrorist() : base(RoleKind.BioTerrorist)
    {
        bioTerroristExtraDriveUsed = false;
        isCaptured = false;
    }


    public void refillDriveAction()
    {
        bioTerroristExtraDriveUsed = false;
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
}
