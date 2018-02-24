using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {

    private Enums.RoleColor color;
    private Enums.RoleKind rolekind;
    private City currentCity = null;
    
    public Pawn(Enums.RoleKind aRolekind)
    {
        rolekind = aRolekind;
        color = Maps.getInstance().getRoleColor(aRolekind);
    }

    public void setCity(City c)
    {
        currentCity = c;
    }

    public City getCity()
    {
        return currentCity;
    }

    public Enums.RoleKind getRoleKind()
    {
        return rolekind;
    }

    public Enums.RoleColor getColor()
    {
        return color;
    }

}
