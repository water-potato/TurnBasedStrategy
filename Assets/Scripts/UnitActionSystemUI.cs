    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();

    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void CreateUnitActionButtons()
    {

        foreach(Transform buttonTransfrom in actionButtonContainerTransform) // foreach 안에 Transform을 넣으면 자식 transform들을 가져온다.
        {
            Destroy(buttonTransfrom.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray()) 
        {
           Transform buttonTransform = Instantiate(actionButtonPrefab , actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = buttonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionUI in actionButtonUIList)
        {
            actionUI.UpdateSelectedVisual();
        } 

    }
}
