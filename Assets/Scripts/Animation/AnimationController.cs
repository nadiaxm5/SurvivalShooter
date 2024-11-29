using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GunCharacterAnimationController;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    //Change one bool to true and the others to false
    public void ChangeToAnimation(string animation)
    {
        foreach (var param in anim.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                anim.SetBool(param.name, param.name == animation);
            }
        }
    }
}
