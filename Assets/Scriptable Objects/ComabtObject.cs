using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Comabat_Object", order = 1)]
public class ComabtObject : ScriptableObject
{
    public List<GameObject> frontLineEnemies;
    public List<GameObject> backLineLineEnemies;


    public bool IsEliteRewards;
}
