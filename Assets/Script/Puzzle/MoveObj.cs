using System.Collections;
using UnityEngine;

public class MoveObj : ObjInteract
{
    [Header("Move Settings")]
    public GameObject objectToMove; // obj move
    public Transform targetDestination; // goal 
    public float moveSpeed = 5f;

    //private bool isMoving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public override void Interact(Type playerType)
    {
        base.Interact(playerType);
        StartCoroutine(MovetoTarget());
    }

    IEnumerator MovetoTarget()
    {
        if (objectToMove == null || targetDestination == null) yield break;

        //isMoving = true;
        while (Vector3.Distance(objectToMove.transform.position, targetDestination.position) > 0.01f)
        {
            // ค่อยๆ เลื่อนตำแหน่ง
            objectToMove.transform.position = Vector3.MoveTowards(
                objectToMove.transform.position,
                targetDestination.position,
                moveSpeed * Time.deltaTime
            );

            // รอเฟรมถัดไปแล้วค่อยทำลูปต่อ (ป้องกันเกมค้าง)
            yield return null;
        }

        objectToMove.transform.position = targetDestination.position;
        //isMoving = false;
        Debug.Log("move Object to target done");
    }
}
