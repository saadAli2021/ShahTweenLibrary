
using UnityEngine;
using UnityEngine.UI;

public enum AdjustWidth
{
    inside = 0, outside = 1, center = 2

}
public  class ShahTween : MonoBehaviour
{
    public static ShahTween instance;


   
    public CanvasScaler canvasScaler;

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { 
        Destroy(instance );
        }
    }

    private void Start()
    {
        if (canvasScaler == null) { 
            Canvas canvas = FindObjectOfType<Canvas>();

            if (!canvas) {
            Debug.LogError("No Canvas component found in the scene.");
                return;
            }

            canvasScaler = canvas.GetComponent<CanvasScaler>();
            if (!canvasScaler) {
                Debug.LogError("Please Assing CanvasScalar component");
                return;
            }
        }

        

    }
    #endregion

    #region setter_Functions
    // Tested : set the image position at the left edge of the panel only horizontally
    public void SetItemAtLeftEdgeHorizontally(RectTransform container, RectTransform uiElementToPlace)
    {
        float halfWidth = (getScaledWidth(uiElementToPlace) / 2f);
        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);

        // Get the x-coordinate of the left edge of the panel
        float leftEdgeX = corners[0].x;

        // Set the image's position to the left edge of the panel
        Vector3 imagePosition = new Vector3(leftEdgeX + halfWidth, uiElementToPlace.position.y, uiElementToPlace.position.z);
        uiElementToPlace.position = imagePosition;
    }

    // TESTED : sets the uiElementToPlace at the vertical center of container
    public void SetItemAtVerticalCenter(RectTransform container, RectTransform uiElementToPlace)
    {

        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);

        // Calculate the vertical center of the panel
        float verticalCenterY = (corners[0].y + corners[1].y) / 2f;

        // Set the image's position to the vertical center of the panel
        Vector3 imagePosition = new Vector3(uiElementToPlace.position.x, verticalCenterY, uiElementToPlace.position.z);
        uiElementToPlace.position = imagePosition;
    }

    // TESTED : sets the image to the exact center of panel
   public  void setItemAtCenterOfContainer(RectTransform container, RectTransform elementToPlace)
    {
        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);
        Vector3 center = (corners[0] + corners[2]) / 2;
        elementToPlace.position = center;

    }
    // Tested : places the image at the center of left edge of conatiner
    public void SetItemAtCenterPointOfLeftEdge(RectTransform container, RectTransform elementToPlace)
    {
        float halfWidth = (getScaledWidth(elementToPlace) / 2f);
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
    // Tested : places the image at the center of right edge of conatiner
    public void SetItemAtCenterPointOfRightEdge(RectTransform container, RectTransform elementToPlace)
    {

        float halfWidth = (getScaledWidth(elementToPlace) / 2f);
        // Calculate the top right corner and bottom right corner
        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);

        Vector3 topRightCorner = corners[2]; // Top right corner
        Vector3 bottomRightCorner = corners[3]; // Bottom right corner

        // Calculate the center point
        Vector3 centerPoint = (topRightCorner + bottomRightCorner) / 2f;
        centerPoint.x -= halfWidth; // Adjust x-coordinate for right edge placement

        // Set the position of the new UI element to the center point
        elementToPlace.position = centerPoint;
    }

    // TESTED : SetItemsAtleftAndRightEdges of container
    public void SetItemsAtleftAndRightEdges(RectTransform container, RectTransform elementToPlaceLeft, RectTransform elementToPlaceRight, AdjustWidth adjustWidth = AdjustWidth.inside, bool adjustHeight = true)
    {

        float HalfWidthOfelementToPlaceLeft = (getScaledWidth(elementToPlaceLeft) / 2f);
        float HalfWidthOfelementToPlaceRight = (getScaledWidth(elementToPlaceRight) / 2f);

        float HalfHeightOfelementToPlaceLeft = (getScaledHeight(elementToPlaceLeft) / 2f);
        float HalfHeightOfelementToPlaceRight = (getScaledHeight(elementToPlaceRight) / 2f);


        Vector3 panelPosition = container.position;
        float panelWidth = getScaledWidth(container);

        // Calculate the positions for the left and right images
        float yPosition = panelPosition.y;
        Vector3 leftPosition = new Vector3(getLeftEdge(container), yPosition, panelPosition.z);
        Vector3 rightPosition = new Vector3(getRightEdge(container), yPosition, panelPosition.z);

        if (adjustWidth == AdjustWidth.inside)
        {
            // width adjustment inside
            leftPosition.x += HalfWidthOfelementToPlaceLeft;
            rightPosition.x -= HalfWidthOfelementToPlaceRight;
        }
        else if (adjustWidth == AdjustWidth.outside)
        {
            //width adjustment outside
            leftPosition.x -= HalfWidthOfelementToPlaceLeft;
            rightPosition.x += HalfWidthOfelementToPlaceRight;
        }

        //// Set the positions of the left and right images
        elementToPlaceLeft.position = leftPosition;
        elementToPlaceRight.position = rightPosition;

        if (adjustHeight)
        {

            // height adjustment 
            SetItemAtVerticalCenter(container, elementToPlaceRight);
            SetItemAtVerticalCenter(container, elementToPlaceLeft);
        }

    }

    #endregion

    #region Getter Functions

    // Tested : set the image position at the left edge of the panel only horizontally
    public Vector3 GetPositionForItemAtLeftEdgeHorizontally(RectTransform container, RectTransform uiElementToPlace)
    {
        float halfWidth = (getScaledWidth(uiElementToPlace) / 2f);
        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);

        // Get the x-coordinate of the left edge of the panel
        float leftEdgeX = corners[0].x;

        // Set the image's position to the left edge of the panel
        Vector3 imagePosition = new Vector3(leftEdgeX + halfWidth, uiElementToPlace.position.y, uiElementToPlace.position.z);
        return imagePosition;
    }
    
    // TESTED : sets the uiElementToPlace at the vertical center of container
    public Vector3 GetPositionForItemAtVerticalCenter(RectTransform container, RectTransform uiElementToPlace)
    {

        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);

        // Calculate the vertical center of the panel
        float verticalCenterY = (corners[0].y + corners[1].y) / 2f;

        // Set the image's position to the vertical center of the panel
        Vector3 imagePosition = new Vector3(uiElementToPlace.position.x, verticalCenterY, uiElementToPlace.position.z);
       return imagePosition;
    }

    // TESTED : sets the image to the exact center of panel
    public Vector3 GetPositionForItemAtCenterOfContainer(RectTransform container)
    {
        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);
        Vector3 center = (corners[0] + corners[2]) / 2;
        return center;

    }
    // Tested : places the image at the center of left edge of conatiner
    public Vector3 GetPositionForItemAtCenterPointOfLeftEdge(RectTransform container, RectTransform elementToPlace)
    {
        float halfWidth = (getScaledWidth(elementToPlace) / 2f);
        // Calculate the top left corner and bottom left corner
        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);

        Vector3 topLeftCorner = corners[0]; // Top left corner
        Vector3 bottomLeftCorner = corners[1]; // Bottom left corner

        // Calculate the center point
        Vector3 centerPoint = (topLeftCorner + bottomLeftCorner) / 2f;
        centerPoint.x += halfWidth;
        // Set the position of the new UI element to the center point
       return centerPoint;
    }

    // Tested : places the image at the center of right edge of conatiner
    public Vector3 GetPositionForItemAtCenterPointOfRightEdge(RectTransform container, RectTransform elementToPlace)
    {

        float halfWidth = (getScaledWidth(elementToPlace) / 2f);
        // Calculate the top right corner and bottom right corner
        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);

        Vector3 topRightCorner = corners[2]; // Top right corner
        Vector3 bottomRightCorner = corners[3]; // Bottom right corner

        // Calculate the center point
        Vector3 centerPoint = (topRightCorner + bottomRightCorner) / 2f;
        centerPoint.x -= halfWidth; // Adjust x-coordinate for right edge placement

        // Set the position of the new UI element to the center point
        return centerPoint;
    }

    // TESTED : ItemsAtleftAndRightEdges of container
    public Vector3[] GetPositionsForItemsAtleftAndRightEdges(RectTransform container, RectTransform elementToPlaceLeft, RectTransform elementToPlaceRight, AdjustWidth adjustWidth = AdjustWidth.inside, bool adjustHeight = true)
    {
        Vector3[] positions = new Vector3[2];

        float HalfWidthOfelementToPlaceLeft = (getScaledWidth(elementToPlaceLeft) / 2f);
        float HalfWidthOfelementToPlaceRight = (getScaledWidth(elementToPlaceRight) / 2f);

        float HalfHeightOfelementToPlaceLeft = (getScaledHeight(elementToPlaceLeft) / 2f);
        float HalfHeightOfelementToPlaceRight = (getScaledHeight(elementToPlaceRight) / 2f);


        Vector3 panelPosition = container.position;
        float panelWidth = getScaledWidth(container);

        // Calculate the positions for the left and right images
        float yPosition = panelPosition.y;
        Vector3 leftPosition = new Vector3(getLeftEdge(container), yPosition, panelPosition.z);
        Vector3 rightPosition = new Vector3(getRightEdge(container), yPosition, panelPosition.z);

        if (adjustWidth == AdjustWidth.inside)
        {
            // width adjustment inside
            leftPosition.x += HalfWidthOfelementToPlaceLeft;
            rightPosition.x -= HalfWidthOfelementToPlaceRight;
        }
        else if (adjustWidth == AdjustWidth.outside)
        {
            //width adjustment outside
            leftPosition.x -= HalfWidthOfelementToPlaceLeft;
            rightPosition.x += HalfWidthOfelementToPlaceRight;
        }

        // Set the positions 
        positions[0] = leftPosition;
        positions[1] = rightPosition;

        if (adjustHeight)
        {
            //getter
           positions[0].y =  GetPositionForItemAtVerticalCenter(container, elementToPlaceLeft).y;
           positions[1].y = GetPositionForItemAtVerticalCenter(container, elementToPlaceRight).y;
        }

        return positions;

    }

    //TESTED : 
    public float getLeftEdge(RectTransform uiElement)
    {

        // Get the position and scaledWidth of the uiElement
        Vector3 panelPosition = uiElement.position;
        float scaledWidth = getScaledWidth(uiElement);
        // calculate edge
        float leftEdge = panelPosition.x - (scaledWidth / 2f);
        return leftEdge;

    }
    //TESTED
    public float getRightEdge(RectTransform uiElement)
    {
        // Get the position and scaledWidth of the uiElement
        Vector3 panelPosition = uiElement.position;
        float scaledWidth = getScaledWidth(uiElement);
        // calculate edge
        float rightEdge = panelPosition.x + scaledWidth / 2f;
        return rightEdge;
    }
    //TESTED
    public float getScaledWidth(RectTransform uiElement)
    {

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
    //TESTED
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

    //TESTED
    public Vector2 getScaledWidthAndHeight(RectTransform uiElement)
    {

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
    // TESTED
    public Vector3 GetPositionForItemAtCenterPointOfTopEdge(RectTransform container, RectTransform elementToPlace)
    { 
        float halfWidth = (getScaledWidth(elementToPlace) / 2f);
        // Calculate the bottom left corner and bottom right corner
        Vector3[] corners = new Vector3[4];
        container.GetWorldCorners(corners);

        Vector3 bottomLeftCorner = corners[1]; // Bottom left corner
        Vector3 bottomRightCorner = corners[2]; // Bottom right corner

        // Calculate the center point at the bottom edge
        Vector3 centerPoint = (bottomLeftCorner + bottomRightCorner) / 2f;
        centerPoint.y -= halfWidth; 
      
        return centerPoint;

    }

    #endregion
}
