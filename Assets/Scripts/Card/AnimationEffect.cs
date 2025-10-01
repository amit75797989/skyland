using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffect : MonoBehaviour
{


    IEnumerator SwingRotation(Transform target, float angle = 15f, float speed = 10f, int repeatCount = 2)
    {
        Quaternion startRot = target.rotation;

        float swingDuration = (2f * Mathf.PI) / speed; // One full swing cycle
        float totalTime = swingDuration * repeatCount;
        float elapsed = 0f;

        while (elapsed < totalTime)
        {
            elapsed += Time.deltaTime;
            float swing = Mathf.Sin(Time.time * speed) * angle;
            target.rotation = startRot * Quaternion.Euler(0f, 0f, swing);
            yield return null;
        }

        // Reset rotation at the end
        target.rotation = startRot;
    }


    IEnumerator RotateAutoReverseBounce(Transform target, Vector3 angleOffset, float duration, float holdTime = 0f)
    {
        Quaternion startRot = target.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(angleOffset);

        float t = 0f;

        // Rotate to target (with bounce)
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;
            float bounce = Mathf.Sin(normalized * Mathf.PI) * (1f - normalized);
            target.rotation = Quaternion.SlerpUnclamped(startRot, endRot, normalized + bounce);
            yield return null;
        }

        target.rotation = endRot;

        // Hold at peak rotation (optional)
        if (holdTime > 0f)
            yield return new WaitForSeconds(holdTime);

        // Rotate back (with bounce)
        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;
            float bounce = Mathf.Sin(normalized * Mathf.PI) * (1f - normalized);
            target.rotation = Quaternion.SlerpUnclamped(endRot, startRot, normalized + bounce);
            yield return null;
        }

        target.rotation = startRot;
    }


    IEnumerator RotateWithBounce(Transform target, Vector3 targetEulerAngles, float duration)
    {
        Quaternion startRot = target.rotation;
        Quaternion endRot = Quaternion.Euler(targetEulerAngles);

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = t / duration;

            // Bounce curve: overshoots and settles
            float bounce = Mathf.Sin(normalizedTime * Mathf.PI) * (1f - normalizedTime);

            target.rotation = Quaternion.SlerpUnclamped(startRot, endRot, normalizedTime + bounce);
            yield return null;
        }

        target.rotation = endRot;
    }


    IEnumerator RotatePingPongBounce(Transform target, Vector3 angleOffset, float duration)
    {
        Quaternion originalRot = target.rotation;
        Quaternion targetRot = originalRot * Quaternion.Euler(angleOffset);

        float t = 0f;

        // Rotate to target with bounce
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = t / duration;

            float bounce = Mathf.Sin(normalizedTime * Mathf.PI) * (1f - normalizedTime);
            target.rotation = Quaternion.SlerpUnclamped(originalRot, targetRot, normalizedTime + bounce);
            yield return null;
        }

        target.rotation = targetRot;

        t = 0f;

        // Return to original with bounce
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = t / duration;

            float bounce = Mathf.Sin(normalizedTime * Mathf.PI) * (1f - normalizedTime);
            target.rotation = Quaternion.SlerpUnclamped(targetRot, originalRot, normalizedTime + bounce);
            yield return null;
        }

        target.rotation = originalRot;
    }
}
