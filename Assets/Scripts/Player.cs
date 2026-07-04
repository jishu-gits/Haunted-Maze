using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Audio")]
    public AudioSource footsteps;
    public AudioClip screamSFX;

    private CharacterController controller;
    private Animator anim;
    public GameObject gameplayCamera;
    public GameObject deathCamera;

    private bool isMoving;
    private bool canMove = true;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!canMove)
            return;

        // Read Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Movement relative to player's facing direction
        Vector3 direction = transform.right * horizontal +
                            transform.forward * vertical;

        if (direction.sqrMagnitude > 1f)
            direction.Normalize();

        controller.Move(direction * speed * Time.deltaTime);

        // Animation & Footsteps
        bool moving = direction.sqrMagnitude > 0.01f;

        if (moving && !isMoving)
        {
            isMoving = true;
            footsteps.Play();
            anim.SetBool("isRunning", true);
        }
        else if (!moving && isMoving)
        {
            isMoving = false;
            footsteps.Stop();
            anim.SetBool("isRunning", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Zombie"))
            return;

        canMove = false;

        footsteps.Stop();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        AudioManager.instance.PlaySFX(screamSFX);

        anim.SetTrigger("isDead");
        gameplayCamera.SetActive(false);
        deathCamera.SetActive(true);

        UIManager.instance.ShowGameOver(false);
    }
}