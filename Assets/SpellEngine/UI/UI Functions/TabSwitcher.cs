using UnityEngine;
using UnityEngine.UI;

public class TabSwitcher : MonoBehaviour
{
    public GameObject spellCreatorTab;
    public GameObject spellViewerTab;
    public GameObject armourStatsTab;

    public Button spellCreatorButton;
    public Button spellViewerButton;
    public Button armourStatsButton;

    void Start()
    {
        // Add listeners to buttons
        spellCreatorButton.onClick.AddListener(() => ShowTab(spellCreatorTab));
        spellViewerButton.onClick.AddListener(() => ShowTab(spellViewerTab));
        armourStatsButton.onClick.AddListener(() => ShowTab(armourStatsTab));

        // Initialize by showing the first tab or any default tab you want
        ShowTab(spellCreatorTab);
    }

    void ShowTab(GameObject tabToShow)
    {
        // Deactivate all tabs
        spellCreatorTab.SetActive(false);
        spellViewerTab.SetActive(false);
        armourStatsTab.SetActive(false);

        // Activate the selected tab
        tabToShow.SetActive(true);
    }
}
