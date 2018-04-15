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
		if (aColor == Color.magenta) {
			numberOfDiseaseCubesLeft = 12;
		}
    }

    public bool isCured()
    {
        return cured;
    }

	public void setCured(bool isCured){
		cured = isCured;
	}

    public bool isEradicated()
    {
        return eradicated;
    }

	public void setEradicated(bool isEradicated){
		eradicated = isEradicated;
	}

    public Color getColor()
    {
        return color;
    }

    public int getNumOfDiseaseCubeLeft()
    {
        return numberOfDiseaseCubesLeft;
    }

    public void decrementNumOfDiseaseCubeLeft()
    {
        numberOfDiseaseCubesLeft--;
    }

    public void incrementNumOfDiseaseCubeLeft()
    {
        numberOfDiseaseCubesLeft++;
    }

    public void setNumOfDiseaseCubeLeft(int cubeLeft){
		this.numberOfDiseaseCubesLeft = cubeLeft;
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
