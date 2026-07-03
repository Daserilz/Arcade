using System.Collections;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private Type myPlayerType;

    [Header("Transform Settings")]
    [SerializeField] private GameObject originalModel;
    [SerializeField] private GameObject mechanismPrefab;
    [SerializeField] private GameObject creativePrefab;

    [Header("Respawn Settings")]
    [SerializeField] private ObjectGone objectGoneManager; // drag your ObjectGone script here in Inspector

    private ObjInteract currentTarget;
    private Type originalPlayerType;
    private GameObject tempCharacterInstance;
    private Coroutine transformCoroutine;

    private void Start()
    {
        originalPlayerType = myPlayerType;
    }

    private void Update()
    {
        // 🔹 Check if Player2 presses U
        if (Input.GetKeyDown(KeyCode.U) && objectGoneManager != null)
        {
            Debug.Log("<color=green>Player2 pressed U to respawn hidden object!</color>");
            objectGoneManager.TryRespawnHiddenObject();
        }
    }

    public Type GetPlayerType()
    {
        return myPlayerType;
    }

    public void OnInteractEvent(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log($"<color=cyan> input keyboard {context} success</color>");
            if (currentTarget != null)
            {
                if (currentTarget.isOneUse && currentTarget.hasInteracted) return;
                currentTarget.Interact(myPlayerType);
            }
        }
    }

    public void RegisterInteractable(ObjInteract target)
    {
        currentTarget = target;
    }

    public void UnregisterInteractable(ObjInteract target)
    {
        if (currentTarget == target)
        {
            currentTarget = null;
        }
    }

    // ---------------- Swap Character -------------------
    public void SwitchForm(float duration)
    {
        Type targetType = Type.None;

        if (myPlayerType == Type.Creative)
            targetType = Type.Mechanism;
        else if (myPlayerType == Type.Mechanism)
            targetType = Type.Creative;

        if (targetType == Type.None) return;

        if (transformCoroutine != null)
            StopCoroutine(transformCoroutine);

        transformCoroutine = StartCoroutine(TransformationRoutine(targetType, duration));
    }

    private IEnumerator TransformationRoutine(Type targetType, float duration)
    {
        if (tempCharacterInstance != null)
            Destroy(tempCharacterInstance);

        myPlayerType = targetType;
        if (originalModel != null) originalModel.SetActive(false);

        GameObject prefabToSpawn = null;
        if (targetType == Type.Mechanism) prefabToSpawn = mechanismPrefab;
        else if (targetType == Type.Creative) prefabToSpawn = creativePrefab;

        if (prefabToSpawn != null)
        {
            tempCharacterInstance = Instantiate(prefabToSpawn, transform.position, transform.rotation);
            tempCharacterInstance.transform.SetParent(this.transform);
        }

        Debug.Log($"<color=yellow>Switched to Type: {myPlayerType} temporarily!</color>");

        yield return new WaitForSeconds(duration);

        RevertToNormal();
    }

    private void RevertToNormal()
    {
        if (tempCharacterInstance != null)
            Destroy(tempCharacterInstance);

        if (originalModel != null) originalModel.SetActive(true);

        myPlayerType = originalPlayerType;
        transformCoroutine = null;
        Debug.Log($"<color=orange>Reverted to normal! Type: {myPlayerType}</color>");
    }
}
