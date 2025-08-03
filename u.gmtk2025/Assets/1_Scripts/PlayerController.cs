using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    PlayerInput overworldInput;
    private Vector2 playerMove;
    Animator animator;
    [SerializeField] float playerSpeed; // The higher the number, the slower the player moves
    int hashSwordAnimation;
    [SerializeField] GameObject Sword;
    Rigidbody2D rb;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        Sword.SetActive(false);
        animator = this.GetComponent<Animator>();
        overworldInput = GetComponent<PlayerInput>();
        InputSystem.actions.Disable();
        overworldInput.currentActionMap?.Enable();
        hashSwordAnimation = Animator.StringToHash("Base Layer.SwingSword");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerMove = context.ReadValue<Vector2>().normalized;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        animator.Play(hashSwordAnimation);
        StartCoroutine(loseSword());
    }

    void FixedUpdate()
    {
        if (playerMove != Vector2.zero)
        {
            rb.linearVelocity = playerMove/playerSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        //this.transform.position = new Vector3(this.transform.position.x + playerMove.x / playerSpeed, this.transform.position.y + playerMove.y / playerSpeed, this.transform.position.z);
    }

    public void swordActive()
    {
        Sword.SetActive(true);
    }

    public void swordInactive()
    {
        Sword.SetActive(false);
    }

    IEnumerator loseSword()
    {
        yield return new WaitForSeconds(0.4f);
        Sword.SetActive(false);
    }
}
