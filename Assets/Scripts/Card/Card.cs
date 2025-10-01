using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatchGame
{
    public class Card : ACard
    {
        private static readonly WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
        
        [SerializeField] private Image mCardIcon;

        private Sprite mBackIcon;
        private Coroutine mFlipCOR;
       

        private void Awake()
        {
            mBackIcon = mCardIcon.sprite;
        }

        public override void InitCard(CardData data)
        {
            base.InitCard(data);
            Flip();
            Invoke("FlipBack", 2f);
        }

        /// <summary>
        /// To flip card to show choosen icon
        /// </summary>
        public override void Flip()
        {
            if (isFlipped || isMatched) return;
            isFlipped = true;
            if (mFlipCOR != null)
            {
                StopCoroutine(mFlipCOR);
            }
            mCardIcon.sprite = mBackIcon;
            
            mFlipCOR = StartCoroutine(FlipRotation(transform, flipTime, true));
        }

        

        /// <summary>
        /// To flip card to hide choosen icon
        /// </summary>
        public override void FlipBack()
        {
            if (mFlipCOR != null)
            {
                StopCoroutine(mFlipCOR);
            }
            mCardIcon.sprite = cardData.CardFrontIcon;
            mFlipCOR = StartCoroutine(FlipRotation(transform, flipTime, false));
        }

        /// <summary>
        /// play matched card animation
        /// </summary>
        public override void ShowMatched()
        {
            isMatched = true;            
            mFlipCOR = StartCoroutine(ScaleUpWithBounce(transform, Vector3.one * 1.2f, flipTime));           
        }

        IEnumerator ScaleUpDown(Transform target, float scaleFactor = 1.2f, float duration = 0.5f, int repeatCount = 1)
        {
            Vector3 originalScale = target.localScale;
            Vector3 targetScale = originalScale * scaleFactor;
            
            for (int i = 0; i < repeatCount; i++)
            {
                // Scale Up
                float t = 0f;
                while (t < duration)
                {
                    t += Time.deltaTime;
                    float progress = t / duration;
                    target.localScale = Vector3.Lerp(originalScale, targetScale, Mathf.SmoothStep(0, 1, progress));
                    yield return _waitForEndOfFrame;
                }

                // Scale Down
                t = 0f;
                while (t < duration)
                {
                    t += Time.deltaTime;
                    float progress = t / duration;
                    target.localScale = Vector3.Lerp(targetScale, Vector3.zero, Mathf.SmoothStep(0, 1, progress));
                    yield return _waitForEndOfFrame;
                }
            }

            // Ensure final scale is original
            target.localScale = Vector3.zero;
        }

        /// <summary>
        /// play mismatched card animation
        /// </summary>
        public override void ShowMisMatched()
        {
            isFlipped= true;
            mFlipCOR= StartCoroutine(DampedSwingRotation(transform));
        }
       
        IEnumerator FlipRotation(Transform targetObj, float flipTime, bool flipForward)
        {
            targetObj.localEulerAngles = Vector3.zero;
            Quaternion startRot = targetObj.rotation;
            Quaternion midRot = Quaternion.Euler(0, 90, 0);
            Quaternion endRot = Quaternion.Euler(0, flipForward ? 180 : 0, 0);

            float t = 0f;

            // First half (0° to 90°)
            while (t < flipTime / 2f)
            {
                t += Time.deltaTime;
                targetObj.rotation = Quaternion.Slerp(startRot, midRot, t / (flipTime / 2f));
                yield return _waitForEndOfFrame;
            }

            targetObj.rotation = midRot;
            mCardIcon.sprite = flipForward ? cardData.CardFrontIcon : mBackIcon;
            yield return _waitForEndOfFrame;
            t = 0f;

            // Second half (90° to 180° or back to 0°)
            while (t < flipTime / 2f)
            {
                t += Time.deltaTime;
                targetObj.rotation = Quaternion.Slerp(midRot, endRot, t / (flipTime / 2f));
                yield return _waitForEndOfFrame;
            }

            targetObj.rotation = endRot;
            
            isFlipped = flipForward; 
        }



        IEnumerator ScaleUpWithBounce(Transform target, Vector3 targetScale, float duration)
        {
            Vector3 startScale = target.localScale;
            float t = 0f;
            Debug.Log("start Scale : " + startScale);
            while (t < duration)
            {
                t += Time.deltaTime;
                float normalizedTime = t / duration;

                // Bounce easing: overshoot and settle
                float bounce = Mathf.Sin(normalizedTime * Mathf.PI) * (1f - normalizedTime);

                target.localScale = Vector3.LerpUnclamped(startScale, targetScale, normalizedTime + bounce);
                yield return _waitForEndOfFrame;
            }
            Debug.Log("start targetScale : " + targetScale);
            target.localScale = targetScale;

            // Scale Down
            t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float progress = t / duration;
                target.localScale = Vector3.Lerp(targetScale, Vector3.zero, Mathf.SmoothStep(0, 1, progress));
                yield return _waitForEndOfFrame;
            }
        }

        IEnumerator DampedSwingRotation(Transform target, float maxAngle = 30f, float speed = 20f, int swingCount = 2)
        {
            yield return _waitForEndOfFrame;
            Quaternion startRot = target.rotation;
            float elapsed = 0f;

            // Total time = full swings * swing period
            float totalDuration = (2f * Mathf.PI * swingCount) / speed;

            while (elapsed < totalDuration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / totalDuration;

                // Damping factor (1 at start → 0 at end)
                float damping = 1f - progress;

                // Sinusoidal swing with damping
                float swing = Mathf.Sin(Time.time * speed) * maxAngle * damping;

                target.rotation = startRot * Quaternion.Euler(0f, 0f, swing);
                yield return _waitForEndOfFrame;
            }

            // Smoothly ease back to original rotation if needed
            float settleTime = 0.3f;
            Quaternion currentRot = target.rotation;
            float t = 0f;

            while (t < settleTime)
            {
                t += Time.deltaTime;
                target.rotation = Quaternion.Slerp(currentRot, startRot, t / settleTime);
                yield return _waitForEndOfFrame;
            }

            target.rotation = startRot;
            yield return _waitForEndOfFrame;
            FlipBack();
        }

        private void OnDisable()
        {
            if (mFlipCOR != null)
            {
                StopCoroutine(mFlipCOR);
            }
        }  
    }
}

