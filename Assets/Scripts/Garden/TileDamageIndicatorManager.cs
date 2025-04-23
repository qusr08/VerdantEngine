using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackDirection
{
    Left,
    Right,
    Foward,
        Area
}
public class TileDamageIndicatorManager : MonoBehaviour
{
    public Animator [] incomingAttackIndicators;
   
    // Start is called before the first frame update
 
    public void StartIndicator(AttackDirection attackDirection)
    {
        switch (attackDirection)
        {
            case AttackDirection.Left:
                StartAnim(incomingAttackIndicators[0], "AttackedLeft");
                break;
            case AttackDirection.Right:
                StartAnim(incomingAttackIndicators[1], "AttackedRight");

                break;
            case AttackDirection.Foward:
                StartAnim(incomingAttackIndicators[2], "AttackedFront");

                break;
            case AttackDirection.Area:
                StartAnim(incomingAttackIndicators[3], "AttackedTop");

                break;
            default:
                break;
        }
    }
    void StartAnim(Animator indicaotr, string trigger)
    {

        if (indicaotr.gameObject.activeSelf)
        { 
            return;
        }
        indicaotr.gameObject.SetActive(true);
        indicaotr.SetTrigger(trigger);

    }

    public IEnumerator WaitForLoad (Animator indicaotr, string trigger)
    {
        yield return new WaitForSeconds(0.2f);
        StartAnim(indicaotr, trigger);
    }

    public void StopIndicator()
    {
        foreach (Animator item in incomingAttackIndicators)
        {
            item.SetTrigger("Rest");
            item.gameObject.SetActive(false);
            
        }
    }
}
