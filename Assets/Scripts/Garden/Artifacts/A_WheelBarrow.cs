using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_WheelBarrow : Artifact
{
    bool isFirstTime = true;
    public override void OnKilled()
    {
        base.OnKilled();
        if (playerDataManager.actionCountModifier >0)
        playerDataManager.actionCountModifier--;
    }
    public override void ActivateAction()
    {
        if (isFirstTime == false)
            return;
        isFirstTime = false;
        playerDataManager.actionCountModifier++;
        base.ActivateAction();


    }

}
