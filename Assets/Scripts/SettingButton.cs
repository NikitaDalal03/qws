using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton: MonoBehaviour
{
    public GameObject[] buttonsToAnimate;

    // Animation speed
    public float animationSpeed = 0.2f;

    //whether the buttons are expanded or collapsed
    private bool isExpanded = false;
    private Vector3[] originalPositions;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnSettingsButtonClick);
    }

    void Start()
    {
        //original positions of buttons
        originalPositions = new Vector3[buttonsToAnimate.Length];
        for (int i = 0; i < buttonsToAnimate.Length; i++)
        {
            originalPositions[i] = buttonsToAnimate[i].transform.localPosition;
        }

        //collapse the buttons
        CollapseButtons();
    }

    public void OnSettingsButtonClick()
    {
        if (isExpanded)
        {
            CollapseButtons();
        }
        else
        {
            ExpandButtons();
        }
    }


    private void ExpandButtons()
    {
        for (int i = 0; i < buttonsToAnimate.Length; i++)
        {
            Vector3 targetPosition = originalPositions[i];
            buttonsToAnimate[i].SetActive(true);
            StartCoroutine(MoveButton(buttonsToAnimate[i].transform, targetPosition));
        }
        isExpanded = true;
    }

    private void CollapseButtons()
    {
        for (int i = 0; i < buttonsToAnimate.Length; i++)
        {
            //Move Up
            Vector3 targetPosition = new Vector3(0f, 0f, 0f);
            StartCoroutine(MoveButton(buttonsToAnimate[i].transform, targetPosition, true));
        }
        isExpanded = false;

    }


    private System.Collections.IEnumerator MoveButton(Transform buttonTransform, Vector3 targetPosition, bool disableAfterMove = false)
    {
        Vector3 startPosition = buttonTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < animationSpeed)
        {
            buttonTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / animationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        buttonTransform.localPosition = targetPosition;

        if (disableAfterMove)
        {
            buttonTransform.gameObject.SetActive(false);
        }
    }
}

