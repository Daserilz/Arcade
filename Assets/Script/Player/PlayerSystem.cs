using System.Collections;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Type myPlayerType;
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    [Header("Transform Settings")]
    [SerializeField] private GameObject originalModel;
    [SerializeField] private GameObject mechanismPrefab;
    [SerializeField] private GameObject creativePrefab;

    [Header("Respawn Settings")]
    private PlayerRespawn playerRespawn;
    [SerializeField] private ObjectGone objectGoneManager;



    private ObjInteract currentTarget;
    private Type originalPlayerType;
    private GameObject tempCharacterInstance;
    private Coroutine transformCoroutine;

    private void Start()
    {
        originalPlayerType = myPlayerType;
        playerRespawn = GetComponent<PlayerRespawn>();
        playerRespawn.playerRenderer = originalModel.GetComponent<Renderer>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // 🔹 Player2 presses U
        if (Input.GetKeyDown(KeyCode.U) && objectGoneManager != null)
        {
            objectGoneManager.TryRespawnHiddenObject();
   

        }

        // 🔹 Player1 presses Q
        if (Input.GetKeyDown(KeyCode.Q) && objectGoneManager != null)
        {
            objectGoneManager.TryHideNearestObject(transform.position);
        }
    }

    public Type GetPlayerType() => myPlayerType;

    public void OnInteractEvent(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed && currentTarget != null)
        {
            if (currentTarget.isOneUse && currentTarget.hasInteracted) return;
            currentTarget.Interact(myPlayerType);
        }
    }

    public void RegisterInteractable(ObjInteract target) => currentTarget = target;
    public void UnregisterInteractable(ObjInteract target)
    {
        if (currentTarget == target) currentTarget = null;
    }

    //Damage and HP 
    public void TakeDamage(int damage)
    {
        if (playerRespawn.isInvincible)
        {
            return;
        }
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healPoint)
    {
        currentHealth += healPoint;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void InstantDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        LevelManager levelManager = FindAnyObjectByType<LevelManager>();
        levelManager.RemovePlayer();
    }


    // ---------------- Swap Character -------------------
    public void SwitchForm(float duration)
    {
        Type targetType = Type.None;
        if (myPlayerType == Type.Creative) targetType = Type.Mechanism;
        else if (myPlayerType == Type.Mechanism) targetType = Type.Creative;
        if (targetType == Type.None) return;

        if (transformCoroutine != null) StopCoroutine(transformCoroutine);
        transformCoroutine = StartCoroutine(TransformationRoutine(targetType, duration));
    }

    private IEnumerator TransformationRoutine(Type targetType, float duration)
    {
        if (tempCharacterInstance != null) Destroy(tempCharacterInstance);

        myPlayerType = targetType;
        if (originalModel != null) originalModel.SetActive(false);

        GameObject prefabToSpawn = targetType == Type.Mechanism ? mechanismPrefab : creativePrefab;
        if (prefabToSpawn != null)
        {
            tempCharacterInstance = Instantiate(prefabToSpawn, transform.position, transform.rotation);
            tempCharacterInstance.transform.SetParent(this.transform);
            playerRespawn.playerRenderer = tempCharacterInstance.GetComponent<Renderer>();
        }

        Debug.Log($"<color=yellow>Switched to Type: {myPlayerType} temporarily!</color>");

        yield return new WaitForSeconds(duration);
        RevertToNormal();
    }

    private void RevertToNormal()
    {
        if (tempCharacterInstance != null) Destroy(tempCharacterInstance);
        if (originalModel != null) originalModel.SetActive(true);
        playerRespawn.playerRenderer = originalModel.GetComponent<Renderer>();

        myPlayerType = originalPlayerType;
        transformCoroutine = null;
     
        Debug.Log($"<color=orange>Reverted to normal! Type: {myPlayerType}</color>");
    }

}
