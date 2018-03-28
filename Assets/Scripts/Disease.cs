using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Disease : MonoBehaviour {

    private bool cured = false;
    private bool eradicated = false;
    private Color color;
    private int numberOfDiseaseCubesLeft = 24;

    public Disease(Color aColor)
    {
        color = aColor;
    }

    public bool isCured()
    {
        return cured;
    }

    public bool isEradicated()
    {
        return eradicated;
    }

    public Color getColor()
    {
        return color;
    }

    public int getNumOfDiseaseCubeLeft()
    {
        return numberOfDiseaseCubesLeft;
    }

    public void addCubes(int num)
    {
        numberOfDiseaseCubesLeft=numberOfDiseaseCubesLeft+num;
    }

    public void removeCubes(int num)
    {
        numberOfDiseaseCubesLeft = numberOfDiseaseCubesLeft-num;
    }

    public void cure()
    {
        cured = true;
    }

    public void eradicate()
    {
        eradicated = true;
    }
}
