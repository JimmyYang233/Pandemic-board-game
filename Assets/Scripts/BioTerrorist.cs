using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
