using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    public static Player instance;
    public Vector3 outsideHousePos, insideHousePos;
    public float walkSpeed;
    public AudioSource audioSource;
    public AudioClip stepClip;
    void Awake(){
        instance = this;
        animator = GetComponent<Animator>();
    }

    void Update(){
        if (!GameManager.instance.transitioning){
            HandleInput();
        }
    }
    Vector3 target;
    bool walking;
    public IEnumerator WalkToLocation(Vector3 target){
        this.target = target;
        animator.SetBool("Walking", true);
        while(Vector3.Distance(transform.position, target) >= 0.02f){
            yield return null;
        }
        transform.position = target;
        animator.SetBool("Walking", false);
    }
    public void StepForward(){
        Vector3 newPos = Vector3.MoveTowards(transform.position, target, walkSpeed);
        newPos = new Vector3(
            Mathf.RoundToInt(newPos.x * 10) / 10f,
            Mathf.RoundToInt(newPos.y * 10) / 10f,
            0);
//            Debug.Log("Setting position to " + newPos);
        transform.position = newPos;

        AfterStep();
    }

    void HandleInput(){
        float val = Input.GetAxisRaw("Vertical");
        /*
        if (val > 0.5f && canMoveUp){
            target = transform.position + Vector3.up;
            animator.SetBool("Walking", true);
        }
        else */if (val < -0.5f){
            target = transform.position + Vector3.down;
            animator.SetBool("Walking", true);
        }
        else{
            animator.SetBool("Walking", false);
        }
    }

    void AfterStep(){
        if (transform.position.y <= HouseGen.instance.exitHousePos){
            animator.SetBool("Walking", false);
            GameManager.instance.ExitHouse();
        }
    }

    bool canMoveUp{
        get{
            return transform.position.y < HouseGen.instance.maxPlayerPos;
        }
    }

    public void StepSound(){
        audioSource.PlayOneShot(stepClip);
    }

    public IEnumerator WalkIntoHouse(){
        transform.position = outsideHousePos;
        yield return StartCoroutine(WalkToLocation(insideHousePos));
    }
}
