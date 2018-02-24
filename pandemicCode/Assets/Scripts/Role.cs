using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour {

    private string description;
    private Enums.RoleKind rolekind;
    private int handlimit;
    private Pawn pawn;

    public Role(Enums.RoleKind r)
    {
        rolekind = r;
        handlimit = 7;
        if (r == Enums.RoleKind.Archivist)
        {
            handlimit = 8;
        }
        description = Maps.getInstance().getDescription(r);
        pawn = new Pawn(r);
    }

    public int getHandLimit()
    {
        return handlimit;
    }

    public Pawn getPawn()
    {
        return pawn;
    }

    public Enums.RoleKind getRoleKind()
    {
        return rolekind;
    }

    public string getDescription()
    {
        return description;
    }
}
