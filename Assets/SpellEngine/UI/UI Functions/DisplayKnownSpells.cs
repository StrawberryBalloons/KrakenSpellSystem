using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DisplayKnownSpells : MonoBehaviour
{
    public List<SpellListManager> allSpellLists;
    public SpellListHolder spellHolder;
    public GameObject spellEntryPrefab;
    public Transform spellListParent;

    private TMP_InputField selectedInputField; // Track the currently selected input field
    private bool waitForInput = false;

    void OnEnable()
    {
        RemoveChildObjectsByName(spellEntryPrefab.name + "(Clone)");
        PopulateSpellList();
    }
    void RemoveChildObjectsByName(string name)
    {
        // Iterate through all child GameObjects
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.name == name)
            {
                Destroy(child.gameObject);
            }
        }
    }
    void PopulateSpellList()
    {
        allSpellLists = spellHolder.allSpellLists;
        foreach (var spell in allSpellLists)
        {
            GameObject entry = Instantiate(spellEntryPrefab, spellListParent);

            // Get TMP text component for spell name
            TextMeshProUGUI spellNameText = entry.transform.Find("SpellNameText").GetComponent<TextMeshProUGUI>();
            spellNameText.text = spell.name;

            // Get TMP input field for key
            TMP_InputField keyInput = entry.transform.Find("KeyInputField").GetComponent<TMP_InputField>();
            keyInput.text = spell.key.ToString();

            // Add listener for key input change
            keyInput.onValueChanged.AddListener((value) => UpdateSpellKey(spell, keyInput.text));

            // Add event for input field selection
            keyInput.onSelect.AddListener((_) => SetSelectedInputField(keyInput));

            // Add event for deselecting input field
            keyInput.onDeselect.AddListener((_) => selectedInputField = null);

            // Get Button for remove action
            Button removeButton = entry.transform.Find("RemoveButton").GetComponent<Button>();
            removeButton.onClick.AddListener(() => RemoveSpell(spell, entry));
        }
    }

    void Update()
    {
        // Check for keyboard input if an input field is selected
        if (selectedInputField != null && Input.anyKeyDown)
        {
            if (waitForInput)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        selectedInputField.text = keyCode.ToString();
                        waitForInput = false;
                        break;
                    }
                }
            }
            else
            {
                waitForInput = true;
            }
        }
    }

    void SetSelectedInputField(TMP_InputField inputField)
    {
        selectedInputField = inputField;
    }

    void RemoveSpell(SpellListManager spell, GameObject entry)
    {
        allSpellLists.Remove(spell);
        Destroy(entry);
    }

    void UpdateSpellKey(SpellListManager spell, string newKey)
    {
        spell.key = (KeyCode)System.Enum.Parse(typeof(KeyCode), newKey, true);

    }
}
