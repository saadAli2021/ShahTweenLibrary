using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SingleAnimator : MonoBehaviour
{
    // Refrence to player Hands Container
    public RectTransform bottomHand;
    public RectTransform leftHand;
    public RectTransform topHand;
    public RectTransform rightHand;

    // Prefab to generate card for demonstration
    public GameObject cardPrefab;
 
    // List containing the cards for each player to distribute
    public List<GameObject> bottomPlayerCards;
    public List<GameObject> leftPlayerCards;
    public List<GameObject> topPlayerCards;
    public List<GameObject> rightPlayerCards;

   
    // cards to distrubute to each player in initial deal
    public int totalCardsToDistribute = 11;
    // delay between the deals of card
    public float delayBetweenDeals = 0.2f;
    // Adding padding to player Hands container
    public bool addPadding ;

    // coroutine refrencess to make them stop to avoid  issues
    private Coroutine rearrangeBottomHandRoutine = null;
    private Coroutine rearrangeTopHandRoutine = null;
    private Coroutine rearrangeLeftHandRoutine = null;
    private Coroutine rearrangeRightHandRoutine = null;

    private Coroutine addMultipleCardsBottomHand = null;

    private void Start()
    { 
        // Generate cards to distribute
        GenerateDeck();
        // start the animation of initial deal of cards to each player
        Invoke("StartCardDealingAnimation", 1f);

    }

    void StartCardDealingAnimation() { 
    StartCoroutine(StartDealingCards());
    }
    private void Update()
    {
       
        // ADD 3 NEW CARDS TO BOTTOM PLAYER HAND
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            print("pressed");
            // Generate a temporary card to add
            GameObject card = Instantiate(cardPrefab);
            card.AddComponent<ShahAnimator>();
            card.transform.SetParent(GameObject.Find("Canvas").transform, false);
          
            GameObject card2 = Instantiate(cardPrefab);
            card2.AddComponent<ShahAnimator>();
            card2.transform.SetParent(GameObject.Find("Canvas").transform, false);
           
            GameObject card3 = Instantiate(cardPrefab);
            card3.AddComponent<ShahAnimator>();
            card3.transform.SetParent(GameObject.Find("Canvas").transform, false);

            //add the card to bottom Hand
            AddNewCardToBottomHand(new List<GameObject>() { card,card2,card3 });
        }
        // ADD A NEW CARD TO TOP PLAYER HAND
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Generate a temporary card to add
            GameObject card = Instantiate(cardPrefab);
            card.AddComponent<ShahAnimator>();
            card.transform.SetParent(GameObject.Find("Canvas").transform, false);

            //add the card to bottom Hand
            AddNewCardToTopHand(new List<GameObject>() { card });
        }
        // ADD A NEW CARD TO left PLAYER HAND
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Generate a temporary card to add
            GameObject card = Instantiate(cardPrefab);
            card.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, -90);
            card.AddComponent<ShahAnimator>();
            card.transform.SetParent(GameObject.Find("Canvas").transform, false);

            //add the card to bottom Hand
            AddNewCardToLeftHand(new List<GameObject>() { card });
        }
        // ADD A NEW CARD TO Right PLAYER HAND
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Generate a temporary card to add
            GameObject card = Instantiate(cardPrefab);
            card.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, -90);
            card.AddComponent<ShahAnimator>();
            card.transform.SetParent(GameObject.Find("Canvas").transform, false);

            //add the card to bottom Hand
            AddNewCardToRightHand(new List<GameObject>() { card });
        }

        // Remove card bottom hand which is at index 0 in bottom hand
        if (Input.GetKeyDown(KeyCode.X)) {

            RemoveCardFromBottomHand(new List<int>() { 0 });
        }
        // Remove card bottom hand which is at index 0 in bottom hand
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RemoveCardFromTopHand(new List<int>() { 0 });
        }
        // Remove card Left hand which is at index 0 in bottom hand
        if (Input.GetKeyDown(KeyCode.C))
        {
            RemoveCardFromLeftHand(new List<int>() { 0 });
        }
        // Remove card Right hand which is at index 0 in bottom hand
        if (Input.GetKeyDown(KeyCode.V))
        {
            RemoveCardFromRightHand(new List<int>() { 0 });
        }

    }

    #region ADDING_DELETING_CARDS_IN_HANDS
    /*------------------------ ADD CARD ---------------------------*/
    public void AddNewCardToBottomHand(List<GameObject> cardsToAdd)
    {
        // Append new cards to BottomHand list
        bottomPlayerCards.AddRange(cardsToAdd);
        print("1: "+bottomPlayerCards.Count);
        // of coroutine is already running stop it first
        if (rearrangeBottomHandRoutine != null)
            StopCoroutine(rearrangeBottomHandRoutine);
        // starting the routine with updated list
        rearrangeBottomHandRoutine = StartCoroutine(RearrangeBottomHand());
    }
    public void AddNewCardToTopHand(List<GameObject> cardsToAdd)
    {
        // Append new cards to BottomHand list
        topPlayerCards.AddRange(cardsToAdd);

        // of coroutine is already running stop it first
        if (rearrangeTopHandRoutine != null)
            StopCoroutine(rearrangeTopHandRoutine);

        // starting the routine with updated list
        rearrangeTopHandRoutine = StartCoroutine(RearrangeTopHand());
    }
    public void AddNewCardToLeftHand(List<GameObject> cardsToAdd)
    {
        // Append new cards to BottomHand list
        leftPlayerCards.AddRange(cardsToAdd);

        // of coroutine is already running stop it first
        if (rearrangeLeftHandRoutine != null)
            StopCoroutine(rearrangeLeftHandRoutine);

        // starting the routine with updated list
        rearrangeLeftHandRoutine = StartCoroutine(RearrangeLeftHand());
    }
    public void AddNewCardToRightHand(List<GameObject> cardsToAdd)
    {
        // Append new cards to BottomHand list
        rightPlayerCards.AddRange(cardsToAdd);

        // of coroutine is already running stop it first
        if (rearrangeRightHandRoutine != null)
            StopCoroutine(rearrangeRightHandRoutine);

        // starting the routine with updated list
        rearrangeRightHandRoutine = StartCoroutine(RearrangeRightHand());
    }
    private List<GameObject> RemoveItemsByIndexes(List<int> indexesToRemove, List<GameObject> listOfElements)
    {
        List<GameObject> removedGameObjects = new List<GameObject>();

        // Iterate through the indexes to remove
        foreach (var index in indexesToRemove)
        {
            if (index >= 0 && index < listOfElements.Count)
            {
                // Remove the GameObject at the specified index and store the reference
                GameObject removedObject = listOfElements[index];
                removedGameObjects.Add(removedObject);
                listOfElements.RemoveAt(index);
                removedObject.SetActive(false);

            }
            else
            {
                Debug.LogWarning("Index out of range: " + index);
            }
        }

        return removedGameObjects;
    }

    /*------------------------ REMOVE CARD ---------------------------*/
    public void RemoveCardFromBottomHand(List<int> indexesOfcardsToRemove)
    {

        List<GameObject> removedCards = RemoveItemsByIndexes(indexesOfcardsToRemove, bottomPlayerCards);

        // of coroutine is already running stop it first
        if (rearrangeBottomHandRoutine != null)
            StopCoroutine(rearrangeBottomHandRoutine);



        // starting the routine with updated list
        rearrangeBottomHandRoutine = StartCoroutine(RearrangeBottomHand());
    }
    public void RemoveCardFromTopHand(List<int> indexesOfcardsToRemove)
    {

        List<GameObject> removedCards = RemoveItemsByIndexes(indexesOfcardsToRemove, topPlayerCards);

        // of coroutine is already running stop it first
        if (rearrangeTopHandRoutine != null)
            StopCoroutine(rearrangeTopHandRoutine);

        // starting the routine with updated list
        rearrangeTopHandRoutine = StartCoroutine(RearrangeTopHand());
    }
    public void RemoveCardFromLeftHand(List<int> indexesOfcardsToRemove)
    {

        List<GameObject> removedCards = RemoveItemsByIndexes(indexesOfcardsToRemove, leftPlayerCards);

        // of coroutine is already running stop it first
        if (rearrangeLeftHandRoutine != null)
            StopCoroutine(rearrangeLeftHandRoutine);

        // starting the routine with updated list
        rearrangeLeftHandRoutine = StartCoroutine(RearrangeLeftHand());
    }
    public void RemoveCardFromRightHand(List<int> indexesOfcardsToRemove)
    {

        List<GameObject> removedCards = RemoveItemsByIndexes(indexesOfcardsToRemove, rightPlayerCards);

        // of coroutine is already running stop it first
        if (rearrangeRightHandRoutine != null)
            StopCoroutine(rearrangeRightHandRoutine);

        // starting the routine with updated list
        rearrangeRightHandRoutine = StartCoroutine(RearrangeRightHand());
    }

    /*------------------------ Re-ARRANGE CARD -----------------------*/
    private IEnumerator RearrangeBottomHand()
    {
        float wait = 0f;
        Vector3 bottomTargetPosition = ShahTween.instance.GetPositionForItemAtCenterPointOfLeftEdge(bottomHand, bottomPlayerCards[0].GetComponent<RectTransform>());
        float BT_Offset = CalculateOffsetForBottomTop(bottomPlayerCards, bottomHand);
        float topBottomOffset = 0f;

        bottomTargetPosition.x += CalculateStartPositionForCard(bottomPlayerCards, bottomHand);


        if (addPadding)
        {
            float paddingValue = (getPadding(ShahTween.instance.getScaledWidth(bottomPlayerCards[0].GetComponent<RectTransform>())) / 2f);
            bottomTargetPosition.x += paddingValue;
        }
            foreach (GameObject card in bottomPlayerCards)
        {
            card.GetComponent<ShahAnimator>().StartAnimation(card.GetComponent<RectTransform>(), bottomTargetPosition, topBottomOffset);

            yield return new WaitForSeconds(wait);
            // making the cards child of Player Hand [optional]
            card.transform.SetParent(bottomHand, false);
            topBottomOffset += BT_Offset;

           
        }
    }
    private IEnumerator RearrangeTopHand()
    {
        float wait = 0f;
        Vector3 topTargetPosition = ShahTween.instance.GetPositionForItemAtCenterPointOfLeftEdge(topHand, topPlayerCards[0].GetComponent<RectTransform>());
        float BT_Offset = CalculateOffsetForBottomTop(topPlayerCards, topHand);
        float topBottomOffset = 0f;

        topTargetPosition.x += CalculateStartPositionForCard(topPlayerCards, topHand);
        //check if padding should be added
        if (addPadding)
        {
            float paddingValue = (getPadding(ShahTween.instance.getScaledWidth(cardPrefab.GetComponent<RectTransform>())) / 2f);
            topTargetPosition.x += paddingValue;
        }
        foreach (GameObject card in topPlayerCards)
        {
            card.GetComponent<ShahAnimator>().StartAnimation(card.GetComponent<RectTransform>(), topTargetPosition, topBottomOffset);

            yield return new WaitForSeconds(wait);
            card.transform.SetParent(topHand, false);
            topBottomOffset += BT_Offset;
        }
    }
    private IEnumerator RearrangeLeftHand()
    {
        float wait = 0f;
        Vector3 leftTargetPosition = ShahTween.instance.GetPositionForItemAtCenterPointOfTopEdge(leftHand, leftPlayerCards[0].GetComponent<RectTransform>());
        float LR_Offset = CalculateOffsetForLeftRight(leftPlayerCards, leftHand);
        float leftRightOffset = 0f;

        leftTargetPosition.y -= CalculateStartPositionForLeftRightCard(leftPlayerCards, leftHand);
        //check if padding should be added
        if (addPadding)
        {
            float paddingValue = (getPadding(ShahTween.instance.getScaledWidth(cardPrefab.GetComponent<RectTransform>())) / 2f);
            leftTargetPosition.y -= paddingValue;  
        }
        foreach (GameObject card in leftPlayerCards)
        {
            card.GetComponent<ShahAnimator>().StartAnimation(card.GetComponent<RectTransform>(), leftTargetPosition, leftRightOffset);

            yield return new WaitForSeconds(wait);
            card.transform.SetParent(leftHand, false);

            leftRightOffset += LR_Offset;
        }
    }
    private IEnumerator RearrangeRightHand()
    {
        float wait = 0f;
        Vector3 rightTargetPosition = ShahTween.instance.GetPositionForItemAtCenterPointOfTopEdge(rightHand, rightPlayerCards[0].GetComponent<RectTransform>());
        float LR_Offset = CalculateOffsetForLeftRight(rightPlayerCards, rightHand);
        float leftRightOffset = 0f;

        rightTargetPosition.y -= CalculateStartPositionForLeftRightCard(rightPlayerCards, rightHand);

        //check if padding should be added
        if (addPadding)
        {
            float paddingValue = (getPadding(ShahTween.instance.getScaledWidth(cardPrefab.GetComponent<RectTransform>())) / 2f);
            rightTargetPosition.y -= paddingValue;
        }
        foreach (GameObject card in rightPlayerCards)
        {
            card.GetComponent<ShahAnimator>().StartAnimation(card.GetComponent<RectTransform>(), rightTargetPosition, leftRightOffset);

            yield return new WaitForSeconds(wait);
            card.transform.SetParent(rightHand, false);

            leftRightOffset += LR_Offset;
        }
    }

    #endregion
    void GenerateDeck() 
    {
        // seperate list of cards in Hand for each hand
        bottomPlayerCards = new List<GameObject>();
        leftPlayerCards = new List<GameObject>();
        topPlayerCards = new List<GameObject>();
        rightPlayerCards = new List<GameObject>();

      // generating cards, Lists and adding ShahAnimator script for animation on each card
        for (int i = 1; i <= 4; ++i)
        {
            for (int j = 0; j < totalCardsToDistribute; ++j)
            { 
            GameObject card = Instantiate(cardPrefab);
            card.AddComponent<ShahAnimator>();
            card.transform.SetParent(GameObject.Find("Canvas").transform, false);

                switch (i)
                {
                    case 1:
                        bottomPlayerCards.Add(card);
                        break;

                    case 2:
                        card.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, -90);
                        leftPlayerCards.Add(card);
                        break;

                    case 3:
                        topPlayerCards.Add(card);
                        break;

                    case 4:
                        card.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, -90);
                        rightPlayerCards.Add(card);
                        break;

                }

            }

        }
        
    }

    // Initial Deal of cards
    private IEnumerator StartDealingCards()
    {
       // Calculated Starting Positions for all hands.
        Vector3 bottomTargetPosition = ShahTween.instance.GetPositionForItemAtCenterPointOfLeftEdge(bottomHand, bottomPlayerCards[0].GetComponent<RectTransform>());
        Vector3 leftTargetPosition = ShahTween.instance.GetPositionForItemAtCenterPointOfTopEdge(leftHand, leftPlayerCards[0].GetComponent<RectTransform>());
        Vector3 topTargetPosition = ShahTween.instance.GetPositionForItemAtCenterPointOfLeftEdge(topHand, topPlayerCards[0].GetComponent<RectTransform>());
        Vector3 rightTargetPosition = ShahTween.instance.GetPositionForItemAtCenterPointOfTopEdge(rightHand, rightPlayerCards[0].GetComponent<RectTransform>());

        //check if padding should be added
        if (addPadding) 
        {
            float paddingValue =( getPadding(ShahTween.instance.getScaledWidth(cardPrefab.GetComponent<RectTransform>()))/2f);
           
            bottomTargetPosition.x += paddingValue;
            topTargetPosition.x += paddingValue;
            leftTargetPosition.y -= paddingValue;
            rightTargetPosition.y -= paddingValue;
        }

        // Calculated Offset Value for Bottom  Hands.
        float BT_Offset = CalculateOffsetForBottomTop(bottomPlayerCards, bottomHand);
        // calculate offset for Top Hand
        float Top_Offset = CalculateOffsetForBottomTop(bottomPlayerCards, topHand);
        // Calculated Offset Value for Left and Right Hands.
        float LR_Offset = CalculateOffsetForLeftRight(leftPlayerCards, leftHand);
      
        // offset values
        float topBottomOffset = 0f;
        float topOffset = 0f;
        float leftRightOffset = 0f;

        // distributing cards And playing Animation
        for (int i = 0; i < totalCardsToDistribute; i++)
        {
            // starting the animation for each card in 'ShahAnimator.cs' Script
            bottomPlayerCards[i].GetComponent<ShahAnimator>().StartAnimation(bottomPlayerCards[i].GetComponent<RectTransform>(),bottomTargetPosition, topBottomOffset);
            topPlayerCards[i].GetComponent<ShahAnimator>().StartAnimation(topPlayerCards[i].GetComponent<RectTransform>(), topTargetPosition, topOffset);
            leftPlayerCards[i].GetComponent<ShahAnimator>().StartAnimation(leftPlayerCards[i].GetComponent<RectTransform>(), leftTargetPosition,leftRightOffset);
            rightPlayerCards[i].GetComponent<ShahAnimator>().StartAnimation(rightPlayerCards[i].GetComponent<RectTransform>(), rightTargetPosition,leftRightOffset);

            yield return new WaitForSeconds(delayBetweenDeals);
           
            // making the cards child of Player Hand [optional]
            bottomPlayerCards[i].transform.SetParent(bottomHand,false);
            topPlayerCards[i].transform.SetParent(topHand, false);
            leftPlayerCards[i].transform.SetParent(leftHand, false);
            rightPlayerCards[i].transform.SetParent(rightHand, false);

            // updating the offset values
            topBottomOffset += BT_Offset;
            leftRightOffset += LR_Offset;
            topOffset += Top_Offset;
        }
    }

    private float CalculateStartPositionForCard(List<GameObject> listOfElements, RectTransform container) {
        float startOffset = 0f;
        float elementWidth = ShahTween.instance.getScaledWidth(listOfElements[0].GetComponent<RectTransform>());
        float containerWidth = ShahTween.instance.getScaledWidth(container) - getPadding(elementWidth);

        int totalElements = listOfElements.Count;

        float totalOccupiedWidth = totalElements * elementWidth;
        float availableOverlappingWidth = containerWidth - totalOccupiedWidth;
        if (availableOverlappingWidth > 0) {
            startOffset = availableOverlappingWidth/2.0f;
        }
        return startOffset;
    }
    private float CalculateStartPositionForLeftRightCard(List<GameObject> listOfElements, RectTransform container)
    {
        float startOffset = 0f;
        float elementWidth = ShahTween.instance.getScaledWidth(listOfElements[0].GetComponent<RectTransform>());
       // float containerWidth = ShahTween.instance.getScaledWidth(container) - getPadding(elementWidth);
        float containerHeight = (ShahTween.instance.GetHeightUsingCorners(container) - getPadding(elementWidth));
        int totalElements = listOfElements.Count;

        float totalOccupiedWidth = totalElements * elementWidth;
        float availableOverlappingWidth = containerHeight - totalOccupiedWidth;
        if (availableOverlappingWidth > 0)
        {
            startOffset = availableOverlappingWidth / 2.0f;
        }
        return startOffset;
    }


    private float CalculateOffsetForBottomTop(List<GameObject> listOfElements, RectTransform container)
    {
        float offset = 0f;

        float elementWidth = ShahTween.instance.getScaledWidth(listOfElements[0].GetComponent<RectTransform>());
        float containerWidth = ShahTween.instance.getScaledWidth(container) - getPadding(elementWidth);
        print("BT : " + containerWidth);
        int totalElements = listOfElements.Count;

        float totalOccupiedWidth = totalElements * elementWidth;
        float availableOverlappingWidth = Mathf.Abs(containerWidth - totalOccupiedWidth);
        float offsetForOverlap = availableOverlappingWidth / (totalElements - 1);
     
        if ((totalOccupiedWidth < containerWidth))//*
        {
            offset = elementWidth;
        }
        else
        {
           
            offset = Mathf.Abs((elementWidth - offsetForOverlap));
        }
        return offset;

    }
    private float CalculateOffsetForLeftRight(List<GameObject> listOfElements, RectTransform container)
    {
        float offset = 0f;
        float elementHeight = ShahTween.instance.getScaledHeight(listOfElements[0].GetComponent<RectTransform>());
        float elementWidth = ShahTween.instance.getScaledWidth(listOfElements[0].GetComponent<RectTransform>());
        float containerHeight = (ShahTween.instance.GetHeightUsingCorners(container) - getPadding(elementWidth));
        
        int totalElements = listOfElements.Count;

        float totalOccupiedHeight = totalElements * elementWidth;//
        float availableOverlappingHeight = Mathf.Abs(containerHeight - totalOccupiedHeight);//
        float offsetForOverlap = availableOverlappingHeight / (totalElements - 1);//
      
        
        if ((totalOccupiedHeight < containerHeight) )//
        {
          
            offset = elementWidth;
        }
        else
        {
           
            offset = Mathf.Abs((elementWidth - offsetForOverlap));//

        }
        return offset;

    }
    
    private float getPadding(float elementWidth)
    {
        if (!addPadding)
            return 0f;

        float padding = (elementWidth);
        return padding;
    }
}
