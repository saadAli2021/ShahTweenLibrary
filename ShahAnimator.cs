using System.Collections;
using UnityEngine;

public class ShahAnimator : MonoBehaviour
{
    float animationDuration = 0.25f;
    Coroutine currentRoutine = null;
    public void StartAnimation(RectTransform imageRectTransform, Vector3 targetPosition, float offset) {

        float zRotation = imageRectTransform.localEulerAngles.z;
        zRotation = (zRotation > 180) ? zRotation - 360 : zRotation;
        
      if (Mathf.Approximately(zRotation, -90))
        currentRoutine =  StartCoroutine(AnimateImageLeft(imageRectTransform, targetPosition, offset));
      else
        currentRoutine =  StartCoroutine(AnimateImage(imageRectTransform, targetPosition, offset));
        
    }

 
    public void StopAnimation()
    {
        StopCoroutine(currentRoutine);
    }

   
    private IEnumerator AnimateImage(RectTransform imageRectTransform, Vector3 targetPosition, float offset)
    {
        Vector3 initialPosition = imageRectTransform.position;
        targetPosition.x += offset;

        float startTime = Time.time;

        while (Time.time - startTime < animationDuration)
        {
            float normalizedTime = (Time.time - startTime) / animationDuration;
            imageRectTransform.position = Vector3.Lerp(initialPosition, targetPosition, normalizedTime);
            yield return null;
        }

        // Ensure the final position is exactly at the center of the target position
        imageRectTransform.position = targetPosition;
        StopAnimation();

    }
    private IEnumerator AnimateImageLeft(RectTransform imageRectTransform, Vector3 targetPosition, float offset)
    {
       
        Vector3 initialPosition = imageRectTransform.position;
        targetPosition.y -= offset;

        float startTime = Time.time;

        while (Time.time - startTime < animationDuration)
        {
            float normalizedTime = (Time.time - startTime) / animationDuration;
            imageRectTransform.position = Vector3.Lerp(initialPosition, targetPosition, normalizedTime);
            yield return null;
        }

        // Ensure the final position is exactly at the center of the target position
        imageRectTransform.position = targetPosition;
       StopAnimation();

    }
}
