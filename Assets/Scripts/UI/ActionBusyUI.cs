using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
        gameObject.SetActive(false);
    }
    public void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        gameObject.SetActive(isBusy);
    }
}
 