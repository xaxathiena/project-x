using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    // Start is called before the first frame update
    [SerializeField] private Transform theMortherTree;
    public Vector3 MotherTreePosition => theMortherTree.position;
}
