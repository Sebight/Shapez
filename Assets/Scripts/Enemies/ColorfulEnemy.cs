using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ColorfulEnemy : Enemy
{
    private bool tweenGoing = false;

    private void ResetTweenTimeout() => tweenGoing = false;
    
    public override void Navigate()
    {
        base.Navigate();
        // transform.RotateAround(transform.position, Vector3.up, 10);

        if (!tweenGoing)
        {
            transform.GetComponent<MeshRenderer>().material.DOColor(Random.ColorHSV(), 0.25f);
            tweenGoing = true;
            Invoke("ResetTweenTimeout", 0.25f);
        }
    }
}
