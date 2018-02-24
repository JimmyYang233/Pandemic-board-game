using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disease : MonoBehaviour {

    private bool cured = false;
    private bool eradicated = false;
    private Enums.DiseaseColor color;
    private int numberOfDiseaseCubesLeft = 24;

    public Disease(Enums.DiseaseColor aColor)
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

    public Enums.DiseaseColor getColor()
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
