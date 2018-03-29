using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Role {

    private string description;
    private RoleKind rolekind;
    private int handlimit;
    private Pawn pawn;
    private Dictionary<Color, int> numberOfSamples = null;

    public Role(RoleKind r)
    {
        rolekind = r;
        handlimit = 7;
        if (r == RoleKind.Archivist)
        {
            handlimit = 8;
        }else if(r == RoleKind.FieldOperative){
            numberOfSamples = new Dictionary<Color, int>();
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

    public void addSample(Disease disease, int num){
        Color pColor = disease.getColor();
        int current = 0;

        if(numberOfSamples.ContainsKey(pColor)){
            current = numberOfSamples[pColor];
            numberOfSamples.Remove(pColor);
        }

        numberOfSamples.Add(pColor, num+current);
    }
}
