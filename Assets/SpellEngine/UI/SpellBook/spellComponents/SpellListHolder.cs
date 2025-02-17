using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellListHolder : MonoBehaviour
{
    public List<SpellListManager> allSpellLists = new List<SpellListManager>();
    public float COOLDOWN_DURATION = 1f;
    private bool isCooldownActive = false;

    public GameObject spellListExecutorPrefab;  // Prefab for the SpellListExecutor
    private GameObject caster;
    private PlayerStats casterStats;
    public GameObject hud;

    void Awake()
    {
        caster = transform.parent.gameObject;
        if (spellListExecutorPrefab == null)
        {
            Debug.LogError("SpellListExecutor prefab is not assigned.");
        }

        casterStats = caster.GetComponent<PlayerStats>();
    }

    void Update()
    {
        //added a check for the HUD so that spells can't be cast when it's not active
        if (!isCooldownActive && !Cursor.visible)
        {
            // Variable to track if any spell has been triggered
            bool spellTriggered = false;

            // Check for spell triggers
            foreach (var spellListManager in allSpellLists)
            {
                if (Input.GetKeyDown(spellListManager.key) && !spellTriggered)
                {
                    // Spawn a new SpellListExecutor and send the selected spell list to it for execution
                    ExecuteSpellList(spellListManager);


                    // Set flag to true since a spell has been triggered
                    spellTriggered = true;
                    COOLDOWN_DURATION = casterStats.GetCurrentStat(StatType.CastDelay);
                }
            }

            // If any spell has been triggered, start cooldown
            if (spellTriggered)
            {
                Debug.Log("Cooldown started");
                StartCoroutine(CooldownRoutine());
            }
        }
    }

    IEnumerator CooldownRoutine()
    {
        // Set cooldown flag to prevent further TriggerSpell calls
        isCooldownActive = true;

        // Wait for the cooldown duration
        yield return new WaitForSeconds(COOLDOWN_DURATION);

        // Reset cooldown flag after the cooldown period
        isCooldownActive = false;
    }


    void ExecuteSpellList(SpellListManager spellListManager)
    {
        if (spellListExecutorPrefab != null)
        {
            // Instantiate a new SpellListExecutor from the prefab
            GameObject spellExecutorInstance = Instantiate(spellListExecutorPrefab, transform.position, transform.rotation);
            SpellListExecutor spellListExecutor = spellExecutorInstance.GetComponent<SpellListExecutor>();

            spellListExecutor.castStepDelay = casterStats.GetCurrentStat(StatType.CastStepDelay);
            spellListExecutor.spellDuration = casterStats.GetCurrentStat(StatType.SpellDuration);

            if (spellListExecutor != null)
            {
                spellListExecutor.caster = caster;  // Assign the caster

                // Optionally, set initial inputs for the spells
                spellListExecutor.inputs = new List<GameObject>(); // Populate with relevant GameObjects
                spellListExecutor.inputs.Add(caster); //default to caster from the start

                spellListExecutor.ExecuteSpellList(spellListManager);
            }
            else
            {
                Debug.LogError("SpellListExecutor component is missing on the instantiated prefab.");
            }
        }
        else
        {
            Debug.LogError("SpellListExecutor prefab is not assigned.");
        }
    }
}
