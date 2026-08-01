using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerSystem : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Type myPlayerType;
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;

    [Header("Transform Settings")]
    [SerializeField] private GameObject originalModel;
    [SerializeField] private GameObject mechanismPrefab;
    [SerializeField] private GameObject creativePrefab;
    private LayerMask originalLayerMask;
   
    private Type originalPlayerType;
 

    [Header("Respawn Settings")]
    private PlayerRespawn playerRespawn;
    [SerializeField] private ObjectGone objectGoneManager;


    private PlayerBugAndObjFixer bugAndGoneFixer;
    private PlayerMovement playerMovement;

    private ObjInteract currentTarget;


    private GameObject tempCharacterInstance;
    private Coroutine transformCoroutine;

    private void Start()
    {
        originalPlayerType = myPlayerType;
        originalLayerMask = gameObject.layer;
        playerRespawn = GetComponent<PlayerRespawn>();
        playerMovement = GetComponent<PlayerMovement>();
        playerRespawn.UpdateRenderers(originalModel);
        currentHealth = maxHealth;
        bugAndGoneFixer = GetComponent<PlayerBugAndObjFixer>();
    }

    private void Update()
    {
        // 🔹 Player2 presses U
        //if (Input.GetKeyDown(KeyCode.U) && objectGoneManager != null)
        //{
        //    if (bugAndObjFixer != null)
        //    {
        //        bugAndObjFixer.PreformAction(myPlayerType);
        //    }
        //}

        //// 🔹 Player1 presses Q
        //if (Input.GetKeyDown(KeyCode.Q) && objectGoneManager != null)
        //{
        //    if (bugAndObjFixer != null)
        //    {
        //        bugAndObjFixer.PreformAction(myPlayerType);
        //    }
        //}
    }

    public Type GetPlayerType() => myPlayerType;

    public void OnInteractEvent(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed && currentTarget != null)
        {
            if (currentTarget.isOneUse && currentTarget.hasInteracted) return;
            currentTarget.Interact(myPlayerType);
        }
        if (context.performed && bugAndGoneFixer != null) bugAndGoneFixer.PreformAction(myPlayerType);
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
            currentHealth = 0;
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
        
        if (myPlayerType == Type.Creative && gameObject.layer == LayerMask.NameToLayer("PlayerCreative"))
        {
            targetType = Type.Mechanism;
            gameObject.layer = LayerMask.NameToLayer("PlayerMechanism");
        }
        else if (myPlayerType == Type.Mechanism && gameObject.layer == LayerMask.NameToLayer("PlayerMechanism")) 
        {
            targetType = Type.Creative;
            gameObject.layer = LayerMask.NameToLayer("PlayerCreative");
        }
     
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
            playerRespawn.UpdateRenderers(tempCharacterInstance);
            playerMovement.UpdateAnimator(tempCharacterInstance.GetComponentInChildren<Animator>());
        }

        Debug.Log($"<color=yellow>Switched to Type: {myPlayerType} temporarily!</color>");

        yield return new WaitForSeconds(duration);
        RevertToNormal();
    }

    private void RevertToNormal()
    {
        if (tempCharacterInstance != null) Destroy(tempCharacterInstance);
        if (originalModel != null) originalModel.SetActive(true);
        playerRespawn.UpdateRenderers(originalModel);
        gameObject.layer = originalLayerMask;
        playerMovement.UpdateAnimator(originalModel.GetComponentInChildren<Animator>());

        myPlayerType = originalPlayerType;
        transformCoroutine = null;
     
        Debug.Log($"<color=orange>Reverted to normal! Type: {myPlayerType}</color>");
    }

}
