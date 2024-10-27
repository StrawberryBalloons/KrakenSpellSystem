using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField]
    private Button serverBtn;

    [SerializeField]
    private Button hostBtn;

    [SerializeField]
    private Button clientBtn;

    [SerializeField]
    private Button singleBtn;
    [SerializeField]
    private Button quitBtn;

    public GameObject networkCanvas;
    public GameObject playerPrefab;


    private void Awake()
    {
        // serverBtn.onClick.AddListener(() =>
        // {
        //     NetworkManager.Singleton.StartServer();
        //     disableNetworkCanvas();
        // });

        // hostBtn.onClick.AddListener(() =>
        // {
        //     NetworkManager.Singleton.StartHost();
        //     disableNetworkCanvas();
        // });

        // clientBtn.onClick.AddListener(() =>
        // {
        //     NetworkManager.Singleton.StartClient();
        //     disableNetworkCanvas();
        // });

        singleBtn.onClick.AddListener(() =>
        {
            // GameObject player = Instantiate(playerPrefab);
            // disableNetworkCanvas();
            SceneManager.LoadScene("SinglePlayerDemo");
        });


        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });

    }

    void disableNetworkCanvas()
    {
        networkCanvas.SetActive(!networkCanvas.activeSelf);
    }

}
