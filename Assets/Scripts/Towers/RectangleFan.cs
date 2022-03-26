using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleFan : Tower
{
    public override void Attack()
    {
        base.Attack();
        transform.Rotate(0, 360 * Time.deltaTime, 0);
    }
}
