using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform leftHand,rightHand;

    private RaycastHit rightHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log("검지손가락 트리거 눌림");
            if (Physics.Raycast(rightHand.position, rightHand.forward,out rightHit,10f))
            {
                var hitObject = rightHit.collider.gameObject;
                if(UIManager.instance.nowFindingObjects) UIManager.instance._debugTextAlpha = 600f;
                UIManager.instance.debugText.text = "이 물건은 아닌 것 같다.";
                Debug.Log(hitObject.name);
                if (hitObject.CompareTag("Button"))
                {
                    Debug.Log("is this Button?");
                }
                else if (hitObject.CompareTag("GO") || hitObject.CompareTag("Item"))
                {
                    hitObject.GetComponent<GOscript>().OnClickByPlayer();
                }
                else
                {
                    OnButtonClicked(hitObject);
                }
            }
        }
        if(OVRInput.Get(OVRInput.Button.Any))
        {
            //Debug.Log(EventSystem.current.name);
        }
    }

    public void CheckMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("마우스 좌클릭 눌림");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out rightHit,1000f))
            {
                Debug.DrawRay(ray.origin,ray.direction * 1000f, Color.red, 0.5f);
                var hitObject = rightHit.collider.gameObject;
                UIManager.instance.debugText.text = "찾고있는 물체가 아닌 것 같다.";
                Debug.Log(hitObject.name);
                if (hitObject.CompareTag("Button"))
                {
                    Debug.Log("is this Button?");
                }
                else if (hitObject.CompareTag("GO"))
                {
                    hitObject.GetComponent<GOscript>().OnClickByPlayer();
                }
                else
                {
                    OnButtonClicked(hitObject);
                }
            }
        }
    }
    public void OnButtonClicked(GameObject gameobject)
    {
        switch (gameobject.name)
        {
            case "BUT_MAINMENU":
                UIManager.instance.ChangeCanvas("MainMenu");
                break;
            case "BUT_TUTORIAL":
                UIManager.instance.ChangeCanvas("InGame");
                break;
            case "BUT_EXIT":
                Application.Quit();
                break;
            case "BUT_SELECTMAP":
                UIManager.instance.ChangeCanvas("SelectMap");
                break;
            case "BUT_GAMESTART":
                
                break;
            case "BUT_MAP1":
                StartCoroutine(UIManager.instance.ChangeMap(1));
                break;
            case "BUT_MAP2":
                StartCoroutine(UIManager.instance.ChangeMap(2));
                break;
            case "BUT_MAP3":
                StartCoroutine(UIManager.instance.ChangeMap(3));
                break;
            case "BUT_DIFF1":
                UIManager.instance.ChangeDiff(1);
                break;
            case "BUT_DIFF2":
                UIManager.instance.ChangeDiff(2);
                break;
            case "BUT_DIFF3":
                UIManager.instance.ChangeDiff(3);
                break;
            
        }
        Debug.Log(gameobject.name + "버튼이 눌렸습니다.");
    }
    
}
