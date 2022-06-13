using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryTextAnimation : MonoBehaviour
{
    public string[] storyTextList;
    private Dictionary<int, string> storyTextDictionary;
    public Text storyText;
    public Image storyBox,blindBorder;
    public AudioClip[] storyTextAudio;
    public GameObject soundPlayer;
    private bool _isTalking;

    public void Init()
    {
        storyBox.gameObject.SetActive(false);
        storyText.gameObject.SetActive(false);
        storyTextDictionary = new Dictionary<int, string>();
        for (int i = 0; i < storyTextList.Length; i++)
        {
            storyTextDictionary.Add(i,storyTextList[i]);
        }

        blindBorder.enabled = false;
        _isTalking = false;
    }


    public IEnumerator StartTextAnimation()
    {
        storyBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        storyText.gameObject.SetActive(true);
        for (int i = 0; i < storyTextList.Length; i++)
        {
            //if(storyTextAudio[i]!=null)
            //soundPlayer.GetComponent<AudioSource>().PlayOneShot(storyTextAudio[i]);
            storyText.text = "";
            for (int j = 0; j < storyTextList[i].Length; j++)
            {
                storyText.text += storyTextList[i][j];
                yield return new WaitForSeconds(0.04f);
            }
            yield return new WaitForSeconds(3f);
        }
        for (float i = 0; i < 255; i+=Time.deltaTime*255f)
        {
            var color = i / 255f;
            storyText.color = new Color(1,1,1,1-color);
            storyBox.color = new Color(0,0,0,1-color-(45/255f));
            yield return null;
        }

        StartCoroutine(FadeIn());
    }

    public void RefreshText()
    {
        
    }
    void Start()
    {
        Init();
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_isTalking)
        {
            _isTalking = true;
            StartCoroutine(StartTextAnimation());
        }
        
        throw new NotImplementedException();
    }
    
    public IEnumerator FadeOut()
    {
        for (float i = 0; i <= 255; i+=Time.deltaTime*255f)
        {
            blindBorder.color = new Color(0, 0, 0, i/255f);
            yield return null;
        }
    }
    
    public IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.5f);
        blindBorder.enabled = true;
        for (float i = 0; i <= 255; i+=Time.deltaTime*150f)
        {
            blindBorder.color = new Color(0, 0, 0, 1-i/255f);
            yield return null;
        }

        SceneManager.LoadScene("Menu_Tutorial");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
