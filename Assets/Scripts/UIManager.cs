// Decompiled with JetBrains decompiler
// Type: UIManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 44AE776F-BEC0-4288-9A88-A12E287BA736
// Assembly location: C:\Users\jyoth\Desktop\EmojiGuess_base\assets\bin\Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public bool isSkipped;
    public GameObject winPanel;
    public GameObject losePanel;
    public TextMeshProUGUI levelNo;
    public string emojiName;
    public TextMeshProUGUI emojiNameText;
    public int failAds;

    private void Awake()
    {
        if ((bool) (Object) UIManager.instance)
            return;
        UIManager.instance = this;
    }

    private void Start()
    {
        if (GAScript.Instance)
            GAScript.Instance.LevelStart((PlayerPrefs.GetInt("lv ", 1).ToString()));
        this.levelNo.text = "lv " + PlayerPrefs.GetInt("levelnumber", 1).ToString();
        this.emojiNameText.DOText(this.emojiName, 0.0f);
    }

    public void LosePanel() => this.StartCoroutine(this.LoseDelay());

    private IEnumerator LoseDelay()
    {
        yield return (object) new WaitForSeconds(0.8f);
        this.losePanel.SetActive(true);
    }

    public void WinPanel() => this.StartCoroutine(this.WinDelay());

    private IEnumerator WinDelay()
    {
        yield return new WaitForSeconds(1.2f);
        winPanel.SetActive(true);
    }

    public void SkipBtnPressed()
    {
        // if(Application.isEditor)
        //     NextLevel();
        
        if(!ISManager.instance || isSkipped)
            return;
        
        isSkipped = true;
        ISManager.instance.ShowRewadedVideo();
    }

    public void NextLevel()
    {
        if(GAScript.Instance)
            GAScript.Instance.LevelCompleted((PlayerPrefs.GetInt("lv ", 1).ToString()));

        if (PlayerPrefs.GetInt("lv ", 1) % 2 == 0)
        {
            if(ISManager.instance)
                ISManager.instance.ShowInterstitialAds();
        }

        if (PlayerPrefs.GetInt("lv ", 1) >= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings - 1));
            PlayerPrefs.SetInt("lv ", PlayerPrefs.GetInt("lv ", 1) + 1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("lv ", PlayerPrefs.GetInt("lv ", 1) + 1);
        }

        PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
    }

    public void RetryLevel()
    {
        GAScript.Instance.LevelFail((PlayerPrefs.GetInt("lv ", 1).ToString()));

        if (failAds / 2 == 1)
        {
            ISManager.instance.ShowInterstitialAds();
            failAds = 0;
            Debug.Log("Showing Ad:");
        }

        failAds++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}