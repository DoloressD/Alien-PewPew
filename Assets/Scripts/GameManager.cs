using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager i;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform spawnPos;
    public UIManager uiManager;

    private void Awake()
    {
        if (i != null && i != this)
            Destroy(this);
        else
            i = this;

        TextPopup.CreateTitlePopup("Shoot The Enemies!");
    }

    public void RevivePlayer()
    {
        Instantiate(playerPrefab, spawnPos.transform.position, Quaternion.identity);
    }
}
