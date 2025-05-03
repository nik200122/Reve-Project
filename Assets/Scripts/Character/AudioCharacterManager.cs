using UnityEngine;

public class AudioCharacterManager : MonoBehaviour
{
    [SerializeField] private AudioClip LandingAudioClip;
    [SerializeField] private AudioClip[] FootstepAudioClips;
    [SerializeField] [Range(0, 1)] private float FootstepAudioVolume = 0.5f;

    private CharacterController controller;

    private void Awake()
    {
        controller=GetComponent<CharacterController>();
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(controller.center), FootstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(controller.center), FootstepAudioVolume);
        }
    }
}
