using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIImageAnimator : MonoBehaviour
{
    public Image leftImage; // Drag and drop the left UI image in the Inspector
    public Image rightImage;
    public RectTransform targetPosition; // The target position to animate towards
    public RectTransform playerHandPanel;
    public CanvasScaler canvasScaler;
    public float animationDuration = 0.5f; // Duration of the animation in seconds
    public float xOffset = 15; // Offset for each image
    public float delayBetweenImages = 0.3f; // Delay between image animations

    public List<Image> imagesToAnimate; // List of UI Images to animate

    private Vector3 targetCenter; // Center of the target position

    float scaledWidth;
    private void Start()
    {
        // Get the target position in world space
        targetCenter = targetPosition.position;
        // Calculate the scaled width based on the width scale factor
        scaledWidth = getScaledWidth(playerHandPanel);

        xOffset = scaledWidth / imagesToAnimate.Count;




        //   setImageToCenter(playerHandPanel, leftImage.rectTransform);
        //  SetItemAtCenterPointOfLeftEdge(playerHandPanel, leftImage.rectTransform);
        //  verticalCenter(playerHandPanel, rightImage);
        // SetImageToVerticalCenter(playerHandPanel, leftImage.rectTransform);
        //setImageToCenter(playerHandPanel, leftImage.rectTransform);
        //  SetImageToLeftEdgeHorizontally(playerHandPanel, leftImage.rectTransform);

        SetItemsAtleftAndRightEdges(playerHandPanel,leftImage.rectTransform,rightImage.rectTransform);
    }
    
   
    // Tested : set the image position at the left edge of the panel only horizontally
    void SetItemAtLeftEdgeHorizontally(RectTransform container, RectTransform uiElementToPlace)
    {
        float halfWidth =( getScaledWidth(uiElementToPlace) / 2f);
        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);

        // Get the x-coordinate of the left edge of the panel
        float leftEdgeX = corners[0].x;

        // Set the image's position to the left edge of the panel
        Vector3 imagePosition = new Vector3(leftEdgeX + halfWidth, uiElementToPlace.position.y, uiElementToPlace.position.z);
        uiElementToPlace.position = imagePosition;
    }

    // TESTED : sets the uiElementToPlace at the vertical center of container
    void SetItemAtVerticalCenter(RectTransform container, RectTransform uiElementToPlace)
    {
        
        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);

        // Calculate the vertical center of the panel
        float verticalCenterY = (corners[0].y + corners[1].y) / 2f;

        // Set the image's position to the vertical center of the panel
        Vector3 imagePosition = new Vector3(uiElementToPlace.position.x, verticalCenterY , uiElementToPlace.position.z);
        uiElementToPlace.position = imagePosition;
    }

    // TESTED : sets the image to the exact center of panel
    void setItemAtCenterOfPanel(RectTransform container, RectTransform elementToPlace)
    {
        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);
        Vector3 center = (corners[0] + corners[2]) / 2;
        elementToPlace.position = center;

    }
    // Tested : places the image at the center of conatiner
    public void SetItemAtCenterPointOfLeftEdge(RectTransform container, RectTransform elementToPlace)
    {
        float halfWidth = (getScaledWidth(elementToPlace)/2f);
        // Calculate the top left corner and bottom left corner
        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);

        Vector3 topLeftCorner = corners[0]; // Top left corner
        Vector3 bottomLeftCorner = corners[1]; // Bottom left corner

        // Calculate the center point
        Vector3 centerPoint = (topLeftCorner + bottomLeftCorner) / 2f;
        centerPoint.x += halfWidth;
        // Set the position of the new UI element to the center point
        elementToPlace.position = centerPoint;
    }


    private void verticalCenter(RectTransform uiElementRectTransform, Image imageToPlace)
    {
        // Get the corners of the UI element in local space
        Vector3[] corners = new Vector3[4];
        uiElementRectTransform.GetLocalCorners(corners);

        // Calculate the average y-coordinate of the top and bottom corners
        float centerY = (corners[1].y + corners[2].y) / 2f;

        // Set the image's position to the calculated vertical center
        Vector3 imagePosition = new Vector3(imageToPlace.rectTransform.position.x, centerY, imageToPlace.rectTransform.position.z);
        imageToPlace.rectTransform.position = imagePosition;
    }

    // TESTED : SetItemsAtleftAndRightEdges of container
    void SetItemsAtleftAndRightEdges(RectTransform container, RectTransform elementToPlaceLeft, RectTransform elementToPlaceRight) {

        float HalfWidthOfelementToPlaceLeft =( getScaledWidth(elementToPlaceLeft)/2f);
        float HalfWidthOfelementToPlaceRight = (getScaledWidth(elementToPlaceRight) / 2f);

        float HalfHeightOfelementToPlaceLeft = (getScaledHeight(elementToPlaceLeft) / 2f);
        float HalfHeightOfelementToPlaceRight = (getScaledHeight(elementToPlaceRight) / 2f);

        print("scaled height : " + getScaledHeight(elementToPlaceLeft));
        print("Normal height : " + (elementToPlaceLeft.rect.height));

        Vector3 panelPosition = container.position;
        float panelWidth = getScaledWidth(playerHandPanel);

        // Calculate the positions for the left and right images
        float yPosition = panelPosition.y; 
        Vector3 leftPosition = new Vector3(getLeftEdge(container), yPosition, panelPosition.z);
        Vector3 rightPosition = new Vector3(getRightEdge(container), yPosition, panelPosition.z);

        // width adjustment inside
        //leftPosition.x += HalfWidthOfelementToPlaceLeft;
        //rightPosition.x -= HalfWidthOfelementToPlaceRight;

        //width adjustment outside
        leftPosition.x -= HalfWidthOfelementToPlaceLeft;
        rightPosition.x += HalfWidthOfelementToPlaceRight;

        //// height adjustment 
        //leftPosition.y += HalfHeightOfelementToPlaceLeft;
        //rightPosition.y += HalfHeightOfelementToPlaceRight;


        //// Set the positions of the left and right images
        elementToPlaceLeft.position = leftPosition;
        elementToPlaceRight.position = rightPosition;

        // height adjustment 
        SetItemAtVerticalCenter(container, elementToPlaceRight);
        SetItemAtVerticalCenter(container, elementToPlaceLeft);

    }

    void SetItemsAtBottomleftAndBottomRightCorners(RectTransform container, RectTransform elementToPlaceLeft, RectTransform elementToPlaceRight)
    {

        Vector3 panelPosition = container.position;
        float panelWidth = getScaledWidth(playerHandPanel);

        // Calculate the positions for the left and right images
        float yPosition = panelPosition.y;
        Vector3 leftPosition = new Vector3(getLeftEdge(container), yPosition, panelPosition.z);
        Vector3 rightPosition = new Vector3(getRightEdge(container), yPosition, panelPosition.z);


        //// Set the positions of the left and right images
        elementToPlaceLeft.position = leftPosition;
        elementToPlaceRight.position = rightPosition;


    }

    Vector3 get2(RectTransform panelRectTransform) {

        // Get the position and dimensions of the panel
        Vector3 panelPosition = panelRectTransform.position;
        float panelWidth = panelRectTransform.rect.width;
        float panelHeight = panelRectTransform.rect.height;

        // Calculate the center position
        Vector3 centerPosition = new Vector3(panelPosition.x - panelWidth / 2f, panelPosition.y - panelHeight / 2f, panelPosition.z);
       
        Debug.Log(" [ get2] Center position of the panel: " + centerPosition);
        return centerPosition;
    }
    public Vector3 getCenterPositionOfUiElement(RectTransform uiElement) {
        // Get the position and dimensions of the panel
        Vector3 position = uiElement.position;
        Vector3 scaled = getScaledWidthAndHeight(uiElement);
        float width = scaled.x;
        float height = scaled.y;

        // Calculate the center position
        Vector3 centerPosition = new Vector3(position.x, position.y, position.z);
        centerPosition.x = width / 2f;
        centerPosition.y = height / 2f;
        print("center position : "+centerPosition);
        return centerPosition;
    }

    public Vector2 getHalfWidthAndHeight(RectTransform uiElement) {

        Vector2 widthAndHeight = Vector2.zero;
        widthAndHeight.x = (uiElement.rect.width / 2f);
        widthAndHeight.y = (uiElement.rect.height / 2f);
        return widthAndHeight;
    }

    public float getLeftEdge(RectTransform uiElement) {

        // Get the position and scaledWidth of the uiElement
        Vector3 panelPosition = uiElement.position;
        float scaledWidth = getScaledWidth(uiElement);
       // calculate edge
        float leftEdge = panelPosition.x - (scaledWidth / 2f);
        return leftEdge;


        /*
          // Get the position and dimensions of the UI element
            Vector3 elementPosition = uiElementRectTransform.position;
            float elementHeight = uiElementRectTransform.rect.height;

            // Calculate the center position of the left edge
            Vector3 leftEdgeCenterPosition = new Vector3(elementPosition.x, elementPosition.y + elementHeight / 2f, elementPosition.z);
         
         */

    }

    public float getRightEdge(RectTransform uiElement)
    {
        // Get the position and scaledWidth of the uiElement
        Vector3 panelPosition = uiElement.position;
        float scaledWidth = getScaledWidth(uiElement);
        // calculate edge
        float rightEdge = panelPosition.x + scaledWidth / 2f;
        return rightEdge;
    }
    public float getScaledWidth(RectTransform uiElement) {

        float width = 0;
        Vector2 referenceResolution = canvasScaler.referenceResolution;
        // Determine the scaling factor for the width
        float widthScaleFactor = Screen.width / referenceResolution.x;
        // Get the RectTransform width (size in local space)
        float rectWidth = uiElement.rect.width;
        // Calculate the scaled width based on the width scale factor
        width = rectWidth * widthScaleFactor;

        return width;
    }
    public float getScaledHeight(RectTransform uiElement)
    {

        float height = 0;
        Vector2 referenceResolution = canvasScaler.referenceResolution;
        // Determine the scaling factor for the width
        float heightScaleFactor = Screen.height / referenceResolution.y;
        // Get the RectTransform width (size in local space)
        float rectHeight = uiElement.rect.height;
        // Calculate the scaled width based on the width scale factor
        height = rectHeight * heightScaleFactor;

        return height;
    }

    public Vector2 getScaledWidthAndHeight(RectTransform uiElement) {

        Vector2 widthHeight = Vector2.zero;
        // Get the reference resolution of the CanvasScaler
        Vector2 referenceResolution = canvasScaler.referenceResolution;

        // Get the current screen resolution
        Vector2 screenResolution = new Vector2(Screen.width, Screen.height);

        // Calculate the scale factor based on the reference resolution and screen size
        float widthScaleFactor = screenResolution.x / referenceResolution.x;
        float heightScaleFactor = screenResolution.y / referenceResolution.y;

        // Get the original width and height of the UI element
        float originalWidth = uiElement.rect.width;
        float originalHeight = uiElement.rect.height;

        // Calculate the scaled width and height
        float scaledWidth = originalWidth * widthScaleFactor;
        float scaledHeight = originalHeight * heightScaleFactor;
        // vector2 x= width and y = height
        widthHeight.x = scaledWidth;
        widthHeight.y = scaledHeight;
        return widthHeight;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartAnimation();
    }

    public void StartAnimation()
    {
        StartCoroutine(AnimateImagesWithDelay());
       
    }

    private IEnumerator AnimateImagesWithDelay()
    {
        foreach (Image image in imagesToAnimate)
        {
            StartCoroutine(AnimateImage(image.rectTransform));
            yield return new WaitForSeconds(delayBetweenImages);
        }
    }
    private float offset;
    private IEnumerator AnimateImage(RectTransform imageRectTransform)
    {
        Vector3 initialPosition = imageRectTransform.position;
        Vector3 targetPosition = targetCenter;

        // Calculate the offset based on the image width
        float imageWidth = imageRectTransform.rect.width;
        offset += xOffset;

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
    }
}
