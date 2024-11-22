using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Title : MonoBehaviour
{


    [SerializeField]
    public List<TitleAsset.TitleData> titleStatsLists = new List<TitleAsset.TitleData>();



    public void AddTitleStatsList(string name, string description, List<TitleAsset.TitleStat> newStatsList)
    {
        TitleAsset.TitleData newTitleData = new TitleAsset.TitleData(name, description, newStatsList);
        titleStatsLists.Insert(0, newTitleData);
    }

    public void RemoveTitleStatsList(int index)
    {
        if (index >= 0 && index < titleStatsLists.Count)
        {
            titleStatsLists.RemoveAt(index);
        }
        else
        {
            Debug.LogWarning("Invalid index to remove title stats list.");
        }
    }
    /// <summary>
    /// Loads TitleAsset files from a specified folder and adds them to TitleStatsLists.
    /// </summary>
    /// <param name="folderPath">The folder containing TitleAsset files.</param>
    public void LoadTitlesFromFolder(string folderPath)
    {
        if (titleStatsLists.Count == 0)
        {
            Debug.Log("Loading titles from " + folderPath);
            if (!Directory.Exists(folderPath))
            {
                Debug.LogError($"Folder path does not exist: {folderPath}");
                return;
            }

            string[] files = Directory.GetFiles(folderPath, "*.asset");


            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                TitleAsset titleAsset = Resources.Load<TitleAsset>($"Titles/{fileName}");

                if (titleAsset != null)
                {
                    // titleStatsLists.Add(titleAsset.title);
                    AddTitleStatsList(titleAsset.title.name, titleAsset.title.description, titleAsset.title.titleStats);
                }
                else
                {
                    Debug.LogWarning($"Failed to load TitleAsset: {fileName}");
                }
            }

            Debug.Log($"Loaded {titleStatsLists.Count} titles from folder: {folderPath}");
        }

    }
}
