using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Player player;
    [SerializeField] PlayerCollisionDetection collision;
    [SerializeField] AudioSource footsteps;

    private bool stopAudio;

    private string currentState = "";

    const string IDLE = "Player_Idle";
    const string RUN = "Player_Run";
    const string JUMP = "Player_Jump";
    const string FALL = "Player_Fall";

    private void Update()
    {
        if (player.oldPosition == player.newPosition)
        {
            ChangeAnimationState(IDLE);
            StopAudio(footsteps);
            return;
        }
        if (Mathf.Abs(Vector2.Distance(player.oldPosition, player.newPosition)) > 0.03f)
        {
            transform.localScale = player.oldPosition.x < player.newPosition.x ? Vector3.one : new Vector3(-1, 1, 1);
        }

        if (collision.OnGround())
        {
            ChangeAnimationState(RUN);
            PlayAudio(footsteps);
        }
        else
        {
            StopAudio(footsteps);
            if (player.oldPosition.y < player.newPosition.y)
            {
                ChangeAnimationState(JUMP);
            }
            else
            {
                ChangeAnimationState(FALL);
            }
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    private void PlayAudio(AudioSource audio)
    {
        if (!audio.isPlaying)
        {
            audio.Play();
            stopAudio = true;
        }
    }

    private void StopAudio(AudioSource audio)
    {
        if (stopAudio)
        {
            audio.Stop();
            stopAudio = false;
        }
    }
}
