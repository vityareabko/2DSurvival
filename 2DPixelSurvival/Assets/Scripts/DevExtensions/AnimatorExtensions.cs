using UnityEngine;

namespace DevExtensions
{
    public static class AnimatorExtensions
    {
        public static float GetClipLength(this Animator animator, string clipName)
        {
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == clipName)
                {
                    return clip.length;
                }
            }

            Debug.LogWarning($"Clip '{clipName}' not found in Animator.");
            return 0f;
        }
    }
}