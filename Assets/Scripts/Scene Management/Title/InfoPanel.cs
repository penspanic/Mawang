using UnityEngine;

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

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowPanel()
    {
        if (isMoving)
            return;
        if (isShowing)
        {
            isShowing = false;
            animator.Play("Info Panel Rise", 0);
        }
        else
        {
            isShowing = true;
            animator.Play("Info Panel Fall", 0);
        }
        isMoving = true;
    }

    public void OnMoveEnd()
    {
        isMoving = false;
    }
}