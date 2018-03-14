using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role {

    private string description;
    private RoleKind rolekind;
    private int handlimit;
    private Pawn pawn;

    public Role(RoleKind r)
    {
        rolekind = r;
        handlimit = 7;
        if (r == RoleKind.Archivist)
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
		//test
        return pawn;
    }

    public void setPawn(Pawn p)
    {
        //test
        this.pawn=p;
		p.setRole (this.rolekind);
    }

    public RoleKind getRoleKind()
    {
        return rolekind;
    }

    public string getDescription()
    {
        return description;
    }

    public bool equal(Role role)
    {
        if (this.getRoleKind() == role.getRoleKind())
        {
            return true;
        }
        return false;
    }
}
