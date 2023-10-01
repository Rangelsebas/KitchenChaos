using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private const string IS_WALKING = "isWalking";

    [SerializeField] private Player player;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool(IS_WALKING, player.IsWalking());
    }
}
