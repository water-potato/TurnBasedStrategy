using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;
    // Update is called once per frame
    void Update()
    {
        if (isActive == false)
            return;

        float spinAngle = 360f * Time.deltaTime;
        
        transform.eulerAngles += new Vector3(0, spinAngle, 0);
        totalSpinAmount += spinAngle;

        if(totalSpinAmount> 360f)
        {
            isActive = false;
            onActionComplete();
        }
    }

    public void Spin(Action onActionComplete )
    {
        totalSpinAmount = 0f;
        isActive = true;

        this.onActionComplete = onActionComplete; 
    }
}