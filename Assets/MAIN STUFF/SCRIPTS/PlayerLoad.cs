using UnityEngine;

public class PlayerLoad : MonoBehaviour
{
    void Start()
    {
        MainMenu.LoadPlayerPosition(transform);
    }
}