using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AI_Enemies_IBaseState 
{
    public void UpdateState(Enemy owner);
    public void OnExit(Enemy owner);
    public void OnEnter(Enemy owner);

    public void OnTrigEnter(Enemy owner,Collider other);
}
