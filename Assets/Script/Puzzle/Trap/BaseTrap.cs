using UnityEngine;

public class BaseTrap : MonoBehaviour
{
    [SerializeField] private Type trapType;

    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("InTriggerStay");
            PlayerInteractor player = other.GetComponent<PlayerInteractor>();
            PlayerRespawn respawnSystem = other.GetComponent<PlayerRespawn>();
            if (player != null)
            {
                if (trapType == Type.None || player.GetPlayerType() != trapType)
                {
                    //take damage or reset pos player
                    Debug.Log("Destroy");
                    respawnSystem.Respawn();
                    //player.gameObject.SetActive(false);
                }
            }
            
        }
    }
}
