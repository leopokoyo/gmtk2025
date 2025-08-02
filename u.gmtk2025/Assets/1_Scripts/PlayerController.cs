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
    void Start()
    {
        Sword.SetActive(false);
        animator = this.GetComponent<Animator>();
        overworldInput = GetComponent<PlayerInput>();
        InputSystem.actions.Disable();
        overworldInput.currentActionMap?.Enable();
        hashSwordAnimation = Animator.StringToHash("Base Layer.SwingSword");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerMove = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        animator.Play(hashSwordAnimation);
        StartCoroutine(loseSword());
    }

    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x + playerMove.x / playerSpeed, this.transform.position.y, this.transform.position.z + playerMove.y / playerSpeed);
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
