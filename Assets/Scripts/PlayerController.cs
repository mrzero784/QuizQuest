using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
      public Animator animator;

      void Start()
      {

      }

      // Update is called once per frame
      void Update()
      {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                  Attack();
            }
      }

      void Attack()
      {
            animator.SetTrigger("Attack");
      }
}
