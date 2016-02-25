﻿using UnityEngine;
using System.Collections;

public class InfoPanel : MonoBehaviour
{
    public bool isMoving
    {
        get;
        private set;
    }
    public bool isShowing
    {
        get;
        private set;
    }

    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void ShowPanel()
    {
        if (isMoving)
            return;
        if(isShowing)
        {
            Debug.Log("Rise");
            isShowing = false;
            animator.Play("Info Panel Rise", 0);
        }
        else
        {
            Debug.Log("Fall");
            isShowing = true;
            animator.Play("Info Panel Fall",0);
        }
        isMoving = true;
    }

    public void OnMoveEnd()
    {
        Debug.Log("Move End");
        isMoving = false;
    }
}