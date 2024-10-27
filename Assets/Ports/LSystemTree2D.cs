using System.Collections.Generic;
using UnityEngine;

public class LSystemTree2D : MonoBehaviour
{
    public int iterations = 4;
    public float initialBranchLength = 5.0f;
    public float branchScaleFactor = 0.2f; // Smaller scale factor
    public float leafScaleFactor = 0.2f; // Smaller scale factor
    public float chonkiness = 0.05f;
    public float angle = 30.0f;

    public float randomAngleVariance = 10f; // Adjust as needed
    public GameObject branchPrefab; // Assign the branch prefab in the Inspector
    public GameObject leafPrefab;   // Assign the leaf prefab in the Inspector

    public string axiom = "F";
    private string currentString;
    private Stack<TransformInfo> transformStack = new Stack<TransformInfo>();
    public List<Rule> ruleset = new List<Rule>();
    public GameObject parent;

    private Dictionary<char, string> rules = new Dictionary<char, string>
    {
    };

    private void Start()
    {
        foreach (Rule r in ruleset)
        {
            rules.Add(r.key, r.value);
        }
        Generate();
    }
    //axiom: "F@"  rules: { 'F', "FF+[+F-F-F@]-[-F+F+F@]" }
    //Output: FF+[+F-F-F]-[-F+F+F]FF+[+F-F-F]-[-F+F+F]+[+FF+[+F-F-F]-[-F+F+F]-FF+[+F-F-F]-[-F+F+F]-FF+[+F-F-F]-[-F+F+F]]-[-FF+[+F-F-F]-[-F+F+F]+FF+[+F-F-F]-[-F+F+F]+FF+[+F-F-F]-[-F+F+F]]@
    private void Generate()
    {
        currentString = axiom;

        for (int i = 0; i < iterations; i++)
        {
            initialBranchLength *= branchScaleFactor; // Apply the scale factor
            string nextString = "";

            foreach (char c in currentString)
            {
                if (rules.ContainsKey(c))
                {
                    nextString += rules[c];
                }
                else
                {
                    nextString += c.ToString();
                }
            }

            currentString = nextString;
        }
        Debug.Log(currentString);

        DrawLSystem();
    }

    private void DrawLSystem()
    {
        foreach (char c in currentString)
        {
            if (c == 'F')
            {
                Vector3 initialPosition = transform.position;
                transform.Translate(Vector3.forward * initialBranchLength);

                if (branchPrefab != null)
                {
                    // Instantiate the branch prefab between the initial and current positions
                    GameObject branchObject = Instantiate(branchPrefab, (initialPosition + transform.position) / 2f, Quaternion.identity, parent.transform);
                    branchObject.transform.LookAt(transform.position);
                    branchObject.transform.localScale = new Vector3(chonkiness, initialBranchLength / 2f, chonkiness);
                    branchObject.transform.Rotate(90, 0, 0); // Add a 90-degree rotation to the x-axis

                }
            }


            // I should add a random rotate, not sure if a forward/baclkwards is needed

            else if (c == '+')//up
            {
                transform.Rotate(Vector3.up * (angle + Random.Range(-randomAngleVariance, randomAngleVariance)));
            }
            else if (c == '-') //down
            {
                transform.Rotate(Vector3.up * (-angle + Random.Range(-randomAngleVariance, randomAngleVariance)));
            }
            else if (c == '&') //left
            {
                transform.Rotate(Vector3.left * (angle + Random.Range(-randomAngleVariance, randomAngleVariance)));
            }
            else if (c == '^')//right
            {
                transform.Rotate(Vector3.left * (-angle + Random.Range(-randomAngleVariance, randomAngleVariance)));
            }
            else if (c == '<') //multiply branch length
            {
                initialBranchLength *= 1 + branchScaleFactor;
            }
            else if (c == '>') //divide branch length
            {
                initialBranchLength /= 1 + branchScaleFactor;
            }
            else if (c == '|')
            {
                transform.Rotate(Vector3.up * 180);
            }
            else if (c == '[')
            {
                transformStack.Push(new TransformInfo(transform.position, transform.rotation));
            }
            else if (c == ']')
            {
                TransformInfo ti = transformStack.Pop();
                transform.position = ti.position;
                transform.rotation = ti.rotation;
            }
            if (c == '@')
            {
                Vector3 initialPosition = transform.position;
                transform.Translate(Vector3.forward * initialBranchLength);
                // Instantiate the branch prefab between the initial and current positions
                GameObject leafObject = Instantiate(leafPrefab, (initialPosition + transform.position) / 2f, Quaternion.identity, parent.transform);
                leafObject.transform.localScale = new Vector3(1f * leafScaleFactor, 1f * leafScaleFactor, 1f * leafScaleFactor);

            }
        }
    }

    private bool HasRuleForCharacter(string str, char character)
    {
        // Check if the given character exists in the rule string
        return str.IndexOf(character) != -1;
    }
}

public struct TransformInfo
{
    public Vector3 position;
    public Quaternion rotation;

    public TransformInfo(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}
