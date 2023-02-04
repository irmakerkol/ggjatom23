using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICanvas : MonoBehaviour
{
    public RectTransform rectT => transform as RectTransform;
    private Animal currentAnimal;
    public static UICanvas instance { get; private set; }
    List<Animal> currentAnimalsOnScreen = new List<Animal>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void Prepare(Animal animal)
    {
        currentAnimal = animal;
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos;

            GameManager.instance.onClick();
            StartCoroutine(onObjectClicked());

            if (currentAnimal.animalData.neededClickToClone <= GameManager.instance.clickCounter)
            {
                if(currentAnimalsOnScreen.Count < currentAnimal.animalData.maxInstanceCount)
                {
                    Animal newAnimal = Instantiate(currentAnimal, rectT);
                    GameManager.instance.clickCounter = 0;
                    currentAnimalsOnScreen.Add(newAnimal);
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectT, Input.mousePosition, null, out mousePos))
                    {
                        newAnimal.gameObject.transform.position = rectT.TransformPoint(mousePos);
                    }
                }
            }
        }
       
        tweenAnimalImage();
    }

    IEnumerator onObjectClicked()
    {
        ObjectsOnClick ooc = Instantiate(currentAnimal.animalData.objectsOnClick_prefab, rectT);
        Vector2 mousePos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectT, Input.mousePosition, null, out mousePos))
        {
            ooc.gameObject.transform.position = rectT.TransformPoint(mousePos);
        }
        yield return new WaitForSeconds(0.8f);

        Destroy(ooc.gameObject);

    }


    private void tweenAnimalImage()
    {
            
        currentAnimal._animalRT.DOSizeDelta(currentAnimal._tweenSize,currentAnimal. _tweenDuration)
        .SetEase(Ease.OutCubic)
        .OnStepComplete(() => currentAnimal._animalRT.DOSizeDelta(currentAnimal._originalSize, currentAnimal._tweenDuration).SetEase(Ease.InCubic));
    }


}
