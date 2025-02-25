using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ActionWheelController : MonoBehaviour
{
    public GameObject actionWheel; // The UI Panel containing the wheel
    public RectTransform wheelTransform; // The RectTransform of the wheel UI
    public Button[] segmentButtons; // 8 buttons in the wheel
    public float selectionThreshold = 30f; // Minimum distance from the center to highlight a segment
    private int selectedSegment = -1;

    // UI Raycaster (must be attached to Canvas)
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    // Placeholder for player's stamina (Replace with actual player stamina reference)
    public float playerStamina = 100f;
    public float staminaRequired = 10f; // Example stamina cost

    public PlayerCombat playerCombat; // Reference to the player's combat system
    public PlayerStats playerStats;

    void Start()
    {
        // Get the Canvas' GraphicRaycaster
        raycaster = GetComponentInParent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right Click Press
        {
            actionWheel.SetActive(true);
            RevealCursor();
        }

        if (Input.GetMouseButton(1)) // While Holding Right Click
        {
            UpdateSelection();
        }

        if (Input.GetMouseButtonUp(1)) // Right Click Release
        {
            ExecuteSelectedAction();
            actionWheel.SetActive(false);
            HideCursor();
            selectedSegment = -1;
        }
    }

    void UpdateSelection()
    {
        int newSegment = GetSegmentUnderMouse();

        if (newSegment != selectedSegment)
        {
            selectedSegment = newSegment;
            HighlightSegment(newSegment);
        }
    }

    int GetSegmentUnderMouse()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        foreach (var result in results)
        {
            for (int i = 0; i < segmentButtons.Length; i++)
            {
                if (result.gameObject == segmentButtons[i].gameObject)
                {
                    return i;
                }
            }
        }

        return -1; // No segment detected
    }

    void HighlightSegment(int index)
    {
        for (int i = 0; i < segmentButtons.Length; i++)
        {
            segmentButtons[i].GetComponent<Image>().color = (i == index) ? Color.yellow : Color.white;
        }
    }

    void ExecuteSelectedAction()
    {
        if (selectedSegment >= 0 && selectedSegment < segmentButtons.Length)
        {
            if (playerStats.GetCurrentStat(StatType.Stamina) >= staminaRequired)
            {
                playerStats.UseStamina(staminaRequired);
                Debug.Log("Performing attack");
                playerCombat.PerformAttack(selectedSegment);
            }
            else
            {
                Debug.Log("Not enough stamina to perform this action!");
            }
        }

    }

    void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void RevealCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
