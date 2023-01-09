using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitOn : MonoBehaviour
{
    [SerializeField]
    private GameObject character;
    Animator animator;
    private bool isWalkingTorwards = false;
    private bool sittingOn = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = character.GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        character.GetComponent<PlayerController>().SitDown(this.transform);
        /*
        if (!sittingOn)
        {
            animator.SetTrigger("isWalking");
            character.GetComponent<Animator>().GetComponent<CharacterController>().enabled = false;
            //animator.SetFloat("speedPercent", .0f);
            isWalkingTorwards = true;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
      /*  if (isWalkingTorwards)
        {
            Vector3 targetDir;

            targetDir = new Vector3(transform.position.x - character.transform.position.x, 0f, transform.position.z - character.transform.position.z);
            Quaternion rot = Quaternion.LookRotation(targetDir);
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, rot, 0.05f);
            character.transform.Translate(Vector3.forward * .0025f);
            if (Vector3.Distance(character.transform.position, transform.position) < 0.6f)
            {
                animator.SetTrigger("isSitting");
                character.transform.rotation = transform.rotation; // Or Slerp aiguain
                isWalkingTorwards = false;
                sittingOn = true;
            }
        }*/
    }
}
