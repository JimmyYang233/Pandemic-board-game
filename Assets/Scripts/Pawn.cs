using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {

    private Color color;
    private RoleKind rolekind;
    private City currentCity = null;
    
    public Pawn(RoleKind aRolekind)
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

    public RoleKind getRoleKind()
    {
        return rolekind;
    }

    public Color getColor()
    {
        return color;
    }

}
