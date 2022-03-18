using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AI_Chompies_BaseState 
{
    public abstract void OnEnter(AI_Chompies_MGR AI);
    public abstract void UpdateState(AI_Chompies_MGR AI);
    public abstract void OnExit(AI_Chompies_MGR AI);

    public abstract void OnTriggerEnter(AI_Chompies_MGR AI,Collider collider);

    public void CanSeePlayer()
    {

    }     

}
