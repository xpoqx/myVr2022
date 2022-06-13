using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GOscript : MonoBehaviour
{
    public string goName;
    public Sprite goSprite;
    public Sprite goSpriteBW;

    public string goExplanation;
    public bool isObjectiveNow;
    public bool isItem;

    public GameObject effectPrefab,effectPrefabSub;
    public AudioClip drinkSound;

    private UIManager _uiManager;
    private Player player;
    private LayerMask _layerMask;
    
    // Start is called before the first frame update
    void Start()
    {
        isObjectiveNow = false;
        _uiManager = FindObjectOfType<UIManager>();
        if (isItem)
        {
            player = FindObjectOfType<Player>();
        }
        _layerMask = LayerMask.GetMask("GO");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickByPlayer()
    {
        if (!isItem)
        {
            _uiManager.FoundObject(goName);
        }
        else
        {
            var debugText = _uiManager.debugText;
            if (goName.Equals("Blue"))
            {
                BluePotion();
                var Effect1 = Instantiate(effectPrefab, player.transform);
                var Effect2 = Instantiate(effectPrefabSub, player.transform);
                Destroy(Effect1,2f);
                Destroy(Effect2,2f);
                debugText.text = "어디론가 순간이동했다.";
            }
            else if (goName.Equals("Red"))
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, _layerMask);
                for (int i = 0; i < colliders.Length; i++)
                {
                    var Effect1 = Instantiate(effectPrefab, colliders[i].transform);
                    var Effect2 = Instantiate(effectPrefabSub, colliders[i].transform);
                    Destroy(Effect1,10f);
                    Destroy(Effect2,10f);
                }
                debugText.text = "주변 물체들이 반짝이기 시작했다.";
            }
            else if (goName.Equals("Green"))
            {
                var Effect1 = Instantiate(effectPrefab, player.transform);
                var Effect2 = Instantiate(effectPrefabSub, player.transform);
                Destroy(Effect1,12f);
                Destroy(Effect2,12f);
                _uiManager.StartCoroutine(_uiManager.GreenPotion());
                debugText.text = "몸이 가벼워진 기분이다.";
            }
            gameObject.SetActive(false);
        }
        //gameObject.SetActive(false);
    }

    

    public void BluePotion()
    {
        var playerT = FindObjectOfType<OVRPlayerController>().gameObject.transform;
        playerT.position = _uiManager.GetTPPoint().position+Vector3.up;
    }
    
}
