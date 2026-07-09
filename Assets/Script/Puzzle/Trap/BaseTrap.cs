using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseTrap : MonoBehaviour
{
    [SerializeField] private Type trapType;

    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("InTriggerStay");
            PlayerSystem player = other.GetComponent<PlayerSystem>();
            PlayerRespawn respawnSystem = other.GetComponent<PlayerRespawn>();
            if (player != null)
            {
                if (trapType == Type.None || player.GetPlayerType() != trapType)
                {
                    //take damage or reset pos player
                    //Debug.Log("Destroy");
                    player.TakeDamage(1);
                    respawnSystem.Respawn();
                    //player.gameObject.SetActive(false);
                }
            }
            
        }
    }
}
