using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Image tutorialBox,mainMenuBox, selectMapBox, inGameBox, gameOverBox, blindBorder;
    public int difficulty;

    public Image inGameGoSprite;
    public Text inGameGoText, inGameTimeText, debugText, gameOverText;

    private int _foundCount;
    private float _deadLineTime;

    public string currentCanvas;
    public bool nowTutorial;

    public ObjectLibrary ObjectLibrary;
    public bool nowFindingObjects;

    public Transform[] teleportPoints;
    public AudioClip[] SoundLibrary;

    public AudioSource audioSource;
    public float _debugTextAlpha;

    public Text diff;
    public int score;


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        GetComponent<Canvas>().worldCamera = FindObjectOfType<OVRScreenFade>().GetComponent<Camera>();
        ObjectLibrary = FindObjectOfType<ObjectLibrary>();
        LoadTpPoints();
        score = 0;
        if(!SceneManager.GetActiveScene().name.Equals("Menu_Tutorial")) Invoke("OnMapEnter",0.5f);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void Start()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(instance.gameObject);

        //difficulty = 0;
        _foundCount = 0;
        
        // 시작 시 띄울 창

        nowTutorial = true;
        nowFindingObjects = false;
        //ChangeCanvas("Tutorial");
        ChangeCanvas("MainMenu");
        //ChangeCanvas("InGame");
        //ChangeCanvas("SelectMap");

        _debugTextAlpha = 0;
        audioSource = GetComponent<AudioSource>();
        blindBorder.gameObject.SetActive(true);
        StartCoroutine(FadeIn());

    }

    // Update is called once per frame
    void Update()
    {
        if (_debugTextAlpha > 0) _debugTextAlpha -= Time.deltaTime * 300f;
        debugText.color = new Color(1, 1, 1, _debugTextAlpha/255f);
        if (currentCanvas.Equals("MainMenu"))
        {
            
        }
        else if (currentCanvas.Equals("SelectMap"))
        {
            
        }
        else if (currentCanvas.Equals("InGame"))
        {
            if (_deadLineTime > 0)
            {
                inGameTimeText.text = "남은시간 : "+_deadLineTime.ToString("F1");
                _deadLineTime -= Time.deltaTime;
            }
            else
            {
                StartCoroutine(ChangeScene("GameOver"));
                // Game Set, Disable Controller
            }
        }
        else if (currentCanvas.Equals("GameOver"))
        {
            
        }
        else if (currentCanvas.Equals("Tutorial"))
        {
            
        }
    }
    public void SetTutorialUI()
    {
        tutorialBox.gameObject.SetActive(true);
    }
    public void SetMainMenuUI()
    {
        mainMenuBox.gameObject.SetActive(true);
    }
    public void SetSelectMapUI()
    {
        selectMapBox.gameObject.SetActive(true);
    }
    public void SetGameOverUI()
    {
        gameOverBox.gameObject.SetActive(true);
        if (nowTutorial)
        {
            gameOverText.text = "튜토리얼 종료";
            nowTutorial = false;
        }
        else
        {
            gameOverText.text = "게임 종료!\n\n"+"점수:"+score;
        }
    }

    public void SetInGameUi()
    {
        if (_foundCount >= ObjectLibrary.gameObjects.Length && _foundCount!=0)
        {
            StartCoroutine(ChangeScene("GameOver"));
            _foundCount = 0;
            return;
        }
            
        inGameBox.gameObject.SetActive(true);
        
        var goScript = ObjectLibrary.gameObjects[_foundCount].GetComponent<GOscript>();
        if (difficulty==0 || difficulty==1)
        {
            inGameGoSprite.gameObject.SetActive(true);
            inGameGoText.gameObject.SetActive(false);
        }
        else if (difficulty == 2)
        {
            inGameGoSprite.gameObject.SetActive(false);
            inGameGoText.gameObject.SetActive(true);
        }
        switch (difficulty)
        {
            case 0:
                inGameGoSprite.sprite = goScript.goSprite;
                break;
            case 1:
                inGameGoSprite.sprite = goScript.goSpriteBW;
                break;
            case 2:
                inGameGoText.text = goScript.goExplanation;
                break;
            default:
                break;
        }

        StartCoroutine(GOAnimation());
    }

    public void FoundObject(string goname)
    {
        if (!nowFindingObjects)
        {
            return;
        }
        var _goName = ObjectLibrary.gameObjects[_foundCount].GetComponent<GOscript>().goName;
        if (goname.Equals(_goName))
        {
            _foundCount++;
            _deadLineTime += 30f;
            debugText.text = "찾았다!";
            score += (int)_deadLineTime/5 + 50;
            SetInGameUi();
        }
        else
        {
            debugText.text = "이건 "+goname+"이다.";
        }
    }

    private void SetTimer()
    {
        _deadLineTime = 60f;
    }

    public void ChangeCanvas(string canvasName)
    {
        nowFindingObjects = false;
        tutorialBox.gameObject.SetActive(false);
        mainMenuBox.gameObject.SetActive(false);
        selectMapBox.gameObject.SetActive(false);
        inGameBox.gameObject.SetActive(false);
        gameOverBox.gameObject.SetActive(false);
        if (canvasName.Equals("MainMenu"))
        {
            currentCanvas = "MainMenu";
            SetMainMenuUI();
        }
        else if (canvasName.Equals("SelectMap"))
        {
            currentCanvas = "SelectMap";
            SetSelectMapUI();
        }
        else if (canvasName.Equals("InGame"))
        {
            currentCanvas = "InGame";
            nowFindingObjects = true;
            SetTimer();
            SetInGameUi();
            
        }
        else if (canvasName.Equals("GameOver"))
        {
            currentCanvas = "GameOver";
            SetGameOverUI();
        }
        else if (canvasName.Equals("Tutorial"))
        {
            currentCanvas = "Tutorial";
            SetTutorialUI();
        }
    }

    public void InitCanvas()
    {
        
    }

    public IEnumerator ChangeScene(string sceneName)
    {
        blindBorder.enabled = true;
        for (float i = 0; i <= 255; i+=Time.deltaTime*255f)
        {
            blindBorder.color = new Color(0, 0, 0, i/255f);
            yield return null;
        }
        
        ChangeCanvas(sceneName);
        
        for (float i = 255; i > 0; i-=Time.deltaTime*150f)
        {
            blindBorder.color = new Color(0, 0, 0, i/255f);
            yield return null;
        }
        blindBorder.enabled = false;
    }

    public IEnumerator FadeOut()
    {
        blindBorder.enabled = true;
        for (float i = 0; i <= 255; i+=Time.deltaTime*255f)
        {
            blindBorder.color = new Color(0, 0, 0, i/255f);
            yield return null;
        }
    }
    
    public IEnumerator FadeIn()
    {
        blindBorder.enabled = true;
        for (float i = 255; i > 0; i-=Time.deltaTime*150f)
        {
            blindBorder.color = new Color(0, 0, 0, i/255f);
            yield return null;
        }
        blindBorder.enabled = false;
    }

    public IEnumerator GOAnimation()
    {
        for (float i = 0; i > -400; i-=Time.deltaTime*550f)
        {
            inGameBox.rectTransform.localPosition = new Vector3(i, i * -0.6f,0f);
            if (i == 0)
            {
                for (float j = 6; j > 1;)
                {
                    var speed = Time.deltaTime * (8 - j)*(8-j);
                    inGameBox.rectTransform.localScale = new Vector3(j,j,j);
                    if (j > 1.05f)
                    {
                        j -= speed;
                    }
                    else
                    {
                        j = 1;
                    }

                    //inGameBox.color = new Color();
                    float alpha = (6 - j) / 5f;
                    inGameGoSprite.color = new Color(1,1,1,alpha);
                    inGameGoText.color = new Color(1,1,1,alpha);
                    inGameTimeText.color = new Color(1,1,1,alpha);
                    yield return null;
                }
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
    }

    public IEnumerator ChangeMap(int mapCode)
    {
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(1f);
        // LoadScene, 플레이어 이동 , 맵 Init(라이브러리 설정, 스포너 배치)
        if (mapCode == 0)
        {
            SceneManager.LoadScene("SampleScene1");
        }
        else if (mapCode==1)
        {
            SceneManager.LoadScene("Map1");
        }
        else if (mapCode==2)
        {
            SceneManager.LoadScene("Map2");
        }
        else if (mapCode==3)
        {
            SceneManager.LoadScene("Map3");
        }
        
        //yield return new WaitForSeconds(1.5f);
    }

    public void OnMapEnter()
    {
        ChangeCanvas("InGame");
        //yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeIn());
    }

    public void LoadTpPoints()
    {
        var tpTransform = FindObjectOfType<TeleportPoint>().transform;
        teleportPoints = new Transform[tpTransform.childCount];
        for (int i = 0; i < tpTransform.childCount; i++)
        {
            teleportPoints[i] = tpTransform.GetChild(i);
        }
    }
    
    public Transform GetTPPoint()
    {
        return teleportPoints[Random.Range(0, teleportPoints.Length)];
    }
    
    public IEnumerator GreenPotion()
    {
        var playerStat = FindObjectOfType<OVRPlayerController>();
        AudioSource.PlayClipAtPoint(SoundLibrary[0],playerStat.transform.position);
        playerStat.Damping = 0.26f;
        yield return new WaitForSeconds(10f);
        playerStat.Damping = 0.5f;
    }

    public void ChangeDiff(int diff)
    {
        if (diff == 1)
        {
            this.diff.text = "★☆☆";
            difficulty = 0;
        }
        else if (diff == 2)
        {
            this.diff.text = "★★☆";
            difficulty = 2;
        }
        else if (diff == 3)
        {
            this.diff.text = "★★★";
            difficulty = 2;
        }
    }
}
