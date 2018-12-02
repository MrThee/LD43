using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticHarness {

    public List<FiringArray> firingArrays = new List<FiringArray>();

    public void LaunchArray(int arrayIndex){
        firingArrays[arrayIndex].Launch();
    }
}