using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    public static Player instance;
    public Vector3 outsideHousePos, insideHousePos;
    void Awake(){
        instance = this;
        animator = GetComponent<Animator>();
    }
    Vector3 target;
    public IEnumerator WalkToLocation(Vector3 target){
        this.target = target;
        animator.SetBool("Walking", true);
        while(Vector3.Distance(transform.position, target) > 0.2f){
            yield return null;
        }
        transform.position = target;
        animator.SetBool("Walking", true);
    }
    public void StepForward(){
        Vector3 newPos = Vector3.MoveTowards(transform.position, target, 0.2f);
        //newPos = new Vector3()
    }

    public void WalkIntoHouse(){
        transform.position = outsideHousePos;
        StartCoroutine(WalkToLocation(insideHousePos));
    }
}
