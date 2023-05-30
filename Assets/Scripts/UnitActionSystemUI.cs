using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;


    private void Awake()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
    }

    private void CreateUnitActionButtons()
    {

        foreach(Transform buttonTransfrom in actionButtonContainerTransform) // foreach 안에 Transform을 넣으면 자식 transform들을 가져온다.
        {
            Destroy(buttonTransfrom.gameObject);
        }


        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray()) 
        {
           Transform buttonTransform = Instantiate(actionButtonPrefab , actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = buttonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }
}
