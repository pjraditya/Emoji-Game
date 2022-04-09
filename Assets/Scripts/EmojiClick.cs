using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public class EmojiClick : MonoBehaviour
{
    public List<Sprite> options;
    public List<Sprite> correctList;
    public List<Image> optionBtn;
    public GameObject emojiIcon;
    public List<GameObject> wrongList;
    public List<Image> correctPos;
    public List<GameObject> correctCheckList;
    private bool _levelfail = false;
    public List<Image> wrongImage;
    private bool _levelComplete = false;
    public List<Image> hintPos;
    public int winCount;
    public List<Sprite> wrongColorlist;

    int index;
    public Sprite crossMark;

    // Winning Animation Components...
    public List<GameObject> winPanelAnim;
    public GameObject scorePanel;
    public List<GameObject> stars;
    public Text iqText;
    public int iq;
    public Image rays;
    public List<GameObject> starsParticleSystem;
    public GameObject poppers;
    public GameObject skipBtn, nextBtn;

    // Gamepanel Animation Components...
    public List<GameObject> gamePanel;

    public List<GameObject> halfandShuffle;

    public List<GameObject> halfandshuffleDummy;

    public GameObject hintBtn;

    public GameObject hintDummy;


    // Failed Panel Animations...
    public List<GameObject> failPanelAnim;
    public GameObject failPanel;
    public Image failRays;
    public GameObject retryBtn, skipBtn1;

    public Image failedImg;

    public Text iqfailedText;

    // Start is called before the first frame update
    void Start()
    {
        //   Sorting...
        for (int i = 0; i < options.Count; i++)
        {
            var temp = options[i];
            int randomTemp = Random.Range(0, options.Count);
            options[i] = options[randomTemp];
            options[randomTemp] = temp;
            optionBtn[i].rectTransform.DOScale(1.3f, 0.3f).SetEase(Ease.Flash).SetLoops(2, LoopType.Yoyo);
        }

        for (int i = 0; i < options.Count; i++)
        {
            optionBtn[i].GetComponent<Image>().sprite = options[i];
            optionBtn[i].gameObject.tag = "Wrong";
        }

        // Debug.Log(optionBtn.Count);
        PickRandomCorrect();


        // Win panel animation
        rays.gameObject.SetActive(false);
        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].SetActive(false);
            stars[i].transform.localScale = new Vector3(10, 10, 10);
        }

        scorePanel.transform.localScale = new Vector3(0, 0, 0);
        nextBtn.transform.localScale = Vector3.zero;
        skipBtn.transform.localScale = Vector3.zero;
        nextBtn.SetActive(false);
        skipBtn.SetActive(false);
        poppers.SetActive(false);

        // Gamepanel Animations...
        for (int i = 0; i < gamePanel.Count; i++)
        {
            gamePanel[i].transform.localScale = new Vector3(0, 0, 0);
            var j = i;
            gamePanel[i].transform.DOScale(Vector3.one, .8f).SetEase(Ease.Flash).OnComplete(() =>
            {
                for (int k = 0; k < halfandShuffle.Count; k++)
                {
                    var k1 = k;
                    halfandShuffle[k].transform.DOMoveX(halfandshuffleDummy[k].transform.position.x, .25f)
                        .SetEase(Ease.Linear).OnComplete(() =>
                        {
                            halfandShuffle[k1].transform.DOScale(Vector3.one * 1.2f, .15f).SetEase(Ease.Flash)
                                .SetLoops(2, LoopType.Yoyo);
                        });
                    hintBtn.transform.DOMoveY(hintDummy.transform.position.y, .25f).OnComplete(() =>
                    {
                        DOTween.Kill(hintBtn.transform);
                        hintBtn.transform.DOScale(Vector3.one * 1.2f, .15f).SetEase(Ease.Flash)
                            .SetLoops(2, LoopType.Yoyo);
                    });
                }
            });
        }


        // Fail Panel Animations...
        rays.gameObject.SetActive(false);
        failPanel.transform.localScale = new Vector3(0, 0, 0);
        retryBtn.transform.localScale = Vector3.zero;
        skipBtn1.transform.localScale = Vector3.zero;
        retryBtn.SetActive(false);
        skipBtn1.SetActive(false);
    }

    IEnumerator ThreeStarsActivation()
    {
        for (int i = 0; i < stars.Count; i++)
        {
            yield return new WaitForSeconds(.5f);
            stars[i].SetActive(true);
            var i1 = i;
            stars[i].transform.DOScale(Vector3.one, .5f).OnComplete(() => { starsParticleSystem[i1].SetActive(true); });
        }
    }

    IEnumerator TwoStarsActivation()
    {
        for (int i = 0; i < stars.Count - 1; i++)
        {
            yield return new WaitForSeconds(.5f);
            stars[i].SetActive(true);
            var i1 = i;
            stars[i].transform.DOScale(Vector3.one, .5f).OnComplete(() => { starsParticleSystem[i1].SetActive(true); });
        }
    }

    IEnumerator OneStarsActivation()
    {
        for (int i = 0; i < stars.Count - 2; i++)
        {
            yield return new WaitForSeconds(.5f);
            stars[i].SetActive(true);
            var i1 = i;
            stars[i].transform.DOScale(Vector3.one, .5f).OnComplete(() => { starsParticleSystem[i1].SetActive(true); });
        }
    }

    IEnumerator WinPanelActivation()
    {
        yield return new WaitForSeconds(0f);
        for (int i = 0; i < winPanelAnim.Count; i++)
        {
            winPanelAnim[i].SetActive(true);
        }
    }

    // IEnumerator DelayPanelOff()
    // {
    //     yield return new WaitForSeconds(.8f);
    //     for (int i = 0; i < gamePanel.Count; i++)
    //     {
    //         gamePanel[i].SetActive(false);
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        if (correctCheckList.Count == winCount)
        {
            if (_levelComplete == false)
            {
                StartCoroutine(WinPanelActivation());
                WonPanel();
                AudioManager.instance.Play("Won");
                _levelComplete = true;
                Debug.Log("Win");
                FindObjectOfType<Button>().enabled = false;
                UIManager.instance.WinPanel();
            }
        }

        if (wrongList.Count == 3)
        {
            if (_levelfail == false)
            {
                // StartCoroutine(DelayPanelOff());
                AudioManager.instance.Play("Lose");
                FailPanel();
                iq = 0;
                iqText.DOText(iq.ToString(), 2, true, ScrambleMode.Numerals);
                // GameObject.FindGameObjectsWithTag("Panel");
                _levelfail = true;
                Debug.Log("GameOver");
                for (int i = 0; i < optionBtn.Count; i++)
                {
                    optionBtn[i].GetComponent<Button>().enabled = false;
                }

                FindObjectOfType<Button>().enabled = false;
                UIManager.instance.LosePanel();
            }
        }
    }

    public void FailPanel()
    {
        for (int i = 0; i < failPanelAnim.Count; i++)
        {
            failPanelAnim[i].SetActive(true);
        }

        failedImg.rectTransform.DOScale(1.3f, .5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Flash);
        failPanel.transform.DOScale(Vector3.one, 1f).SetEase(Ease.Flash).OnComplete(() =>
        {
            // Background RayAlpha...
            failRays.gameObject.SetActive(true);
            Color color = rays.color;
            failRays.DOColor(new Color(color.r, color.g, color.b, 0f), 1f)
                .OnComplete(() => { failRays.DOColor(new Color(color.r, color.g, color.b, 1f), 1f); })
                .SetLoops(-1, LoopType.Yoyo);
            iqfailedText.DOText(iq.ToString(), 1.5f, true, ScrambleMode.Numerals).OnComplete(() =>
            {
                retryBtn.SetActive(true);
                skipBtn1.SetActive(true);
                retryBtn.transform.DOScale(Vector3.one, .5f).SetEase(Ease.Flash).OnComplete(() =>
                {
                    retryBtn.transform.DOScale(.95f, .5f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
                });
                skipBtn1.transform.DOScale(Vector3.one, .5f).SetEase(Ease.Flash).OnComplete(() =>
                {
                    skipBtn1.transform.DOScale(.95f, .5f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
                });
                // poppers.SetActive(true);
            });
        });
    }

    public void WonPanel()
    {
        scorePanel.transform.DOScale(Vector3.one, 1f).SetEase(Ease.Flash).OnComplete(() =>
        {
            // Background RayAlpha...
            rays.gameObject.SetActive(true);
            Color color = rays.color;
            rays.DOColor(new Color(color.r, color.g, color.b, 0f), 1f)
                .OnComplete(() => { rays.DOColor(new Color(color.r, color.g, color.b, 1f), 1f); })
                .SetLoops(-1, LoopType.Yoyo);

            // Stars...
            if (wrongList.Count == 0)
            {
                StartCoroutine(ThreeStarsActivation());
                iq = Random.Range(200, 300);
            }

            if (wrongList.Count == 1)
            {
                StartCoroutine(TwoStarsActivation());
                iq = Random.Range(100, 200);
            }

            if (wrongList.Count == 2)
            {
                StartCoroutine(OneStarsActivation());
                iq = Random.Range(50, 100);
            }

            iqText.DOText(iq.ToString(), 2, true, ScrambleMode.Numerals).OnComplete(() =>
            {
                nextBtn.SetActive(true);
                //  skipBtn.SetActive(true);
                nextBtn.transform.DOScale(Vector3.one, .5f).SetEase(Ease.Flash).OnComplete(() =>
                {
                    nextBtn.transform.DOScale(.95f, .5f).SetEase(Ease.Flash).SetLoops(-1, LoopType.Yoyo);
                });
                //   skipBtn.transform.DOScale(Vector3.one, .5f).SetEase(Ease.Flash);
                poppers.SetActive(true);
            });
        });
    }

    IEnumerator RemoveCrossMark(Image obj)
    {
        yield return new WaitForSeconds(.5f);
        obj.GetComponent<Image>().enabled = false;
    }

    public void SelectEmoji()
    {
        AudioManager.instance.Play("Tap");
        var temp = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        temp.GetComponent<Button>().enabled = false;
        // UIManager.instance.SmokeOn();
        if (temp.CompareTag("Wrong"))
        {
            temp.GetComponent<Image>().sprite = crossMark;
            temp.GetComponent<Image>().transform.DOScale(.5f, .25f).SetEase(Ease.Linear);
            StartCoroutine(RemoveCrossMark(temp.GetComponent<Image>()));
            // temp.GetComponent<Image>().enabled = false;
            wrongList.Add(temp.rectTransform.gameObject);

            wrongImage[index].sprite = wrongColorlist[index];
            wrongImage[index].rectTransform.DOScale(Vector3.one * 1.3f, .25f).SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.Flash);
            index++;
            Vibration.Vibrate(20);


        }

        if (temp.CompareTag("Correct"))
        {
            if (temp.GetComponent<Image>().sprite == correctList[0])
            {
                temp.GetComponent<Image>().rectTransform.DOMove(correctPos[0].rectTransform.position, .25f)
                    .OnComplete(() =>
                    {
                        correctPos[0].GetComponent<Image>().enabled = true;
                        correctPos[0].GetComponent<Image>().sprite = temp.GetComponent<Image>().sprite;
                        // correctPos[0].GetComponent<Image>().rectTransform.DOScale(1.3f, 0.25f).SetEase(Ease.Linear)
                        //     .SetLoops(2, LoopType.Yoyo);
                        temp.GetComponent<Image>().enabled = false;
                        var colorTemp = correctPos[0].GetComponent<Image>();
                        colorTemp.color = new Color(colorTemp.color.r, colorTemp.color.g, colorTemp.color.b, 1f);
                    });
            }

            else if (temp.GetComponent<Image>().sprite == correctList[1])
            {
                temp.GetComponent<Image>().rectTransform.DOMove(correctPos[1].rectTransform.position, .25f)
                    .OnComplete(() =>
                    {
                        correctPos[1].GetComponent<Image>().enabled = true;
                        correctPos[1].GetComponent<Image>().sprite = temp.GetComponent<Image>().sprite;
                        // correctPos[1].GetComponent<Image>().rectTransform.DOScale(1.3f, 0.25f).SetEase(Ease.Linear)
                        //     .SetLoops(2, LoopType.Yoyo);
                        temp.GetComponent<Image>().enabled = false;
                        var colorTemp = correctPos[1].GetComponent<Image>();
                        colorTemp.color = new Color(colorTemp.color.r, colorTemp.color.g, colorTemp.color.b, 1f);
                    });
            }
            else if (temp.GetComponent<Image>().sprite == correctList[2])
            {
                temp.GetComponent<Image>().rectTransform.DOMove(correctPos[2].rectTransform.position, .25f)
                    .OnComplete(() =>
                    {
                        correctPos[2].GetComponent<Image>().enabled = true;
                        correctPos[2].GetComponent<Image>().sprite = temp.GetComponent<Image>().sprite;
                        // correctPos[1].GetComponent<Image>().rectTransform.DOScale(1.3f, 0.25f).SetEase(Ease.Linear)
                        //     .SetLoops(2, LoopType.Yoyo);
                        temp.GetComponent<Image>().enabled = false;
                        var colorTemp = correctPos[2].GetComponent<Image>();
                        colorTemp.color = new Color(colorTemp.color.r, colorTemp.color.g, colorTemp.color.b, 1f);
                    });
            }


            correctCheckList.Add(temp.gameObject);           
            Vibration.Vibrate(20);

            /*for (int i = 0; i < correctList.Count; i++)
            {
                if (temp.GetComponent<Image>().sprite == correctList[i])
                {
                    var i1 = i;
                    temp.GetComponent<Image>().rectTransform.DOMove(correctPos[i].rectTransform.position, .25f)
                        .OnComplete(() =>
                        {
                            correctPos[i1].GetComponent<Image>().enabled = true;
                            correctPos[i1].GetComponent<Image>().sprite = temp.GetComponent<Image>().sprite;
                            correctPos[i1].GetComponent<Image>().rectTransform.DOScale(1.3f, 0.25f).SetEase(Ease.Linear)
                                .SetLoops(2, LoopType.Yoyo);
                            temp.GetComponent<Image>().enabled = false;
                            correctList.Remove(correctList[i1]);
                        });
                }
            }*/
        }
    }

    public void PickRandomCorrect()
    {
        for (int i = 0; i < correctList.Count; i++)
        {
            int ran = Random.Range(0, optionBtn.Count);
            optionBtn[ran].GetComponent<Image>().sprite = correctList[i];
            optionBtn[ran].gameObject.tag = "Correct";
            optionBtn.Remove(optionBtn[ran]);
        }

        // var temps = NonRepetitiveNums(correctList.Count, optionBtn.Count);
        // if (temps.Count < correctList.Count)
        // {
        //     
        // }

        // for (int i = 0; i < correctList.Count; i++)
        // {
        //     optionBtn[temps[i]].GetComponent<Image>().sprite = correctList[i];
        //     optionBtn[temps[i]].gameObject.tag = "Correct";
        // }
    }

    /*
    List<int> NonRepetitiveNums(int cap, int BtnsCap)
    {
        List<int> temp = new List<int>();
        
        for (int i = 0; i < cap; i++)
        {
            int val = Random.Range(0, BtnsCap);
            while (!temp.Contains(val))
            {
                temp.Add(val);
            }
        }
        return temp;
    }
    */

    public void Hint()
    {
        ISManager.instance.ShowRewadedVideo();

        //  int randNum = Random.Range(0, correctList.Count);
        for (int i = 0; i < hintPos.Count; i++)
        {
            var temp = hintPos[i].GetComponent<Image>();
            temp.color = new Color(temp.color.r, temp.color.g, temp.color.b, .5f);
            temp.enabled = true;
            temp.sprite = correctList[i];
            temp.rectTransform.DOScale(1.3f, 0.25f).SetEase(Ease.Linear)
                .SetLoops(2, LoopType.Yoyo);
        }
    }

    public void RemoveHalfEmoji()
    {
        ISManager.instance.ShowRewadedVideo();
        var j = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        j.interactable = false;
        int num = (optionBtn.Count / 2);
        for (int i = 0; i < num; i++)
        {
            optionBtn.RemoveAt(i);
            optionBtn[i].gameObject.SetActive(false);
        }

        // int spritenum = (options.Count / 2);
        // for (int i = 0; i < spritenum; i++)
        // {
        //     options.RemoveAt(i);
        // }

        // PickRandomCorrect();
    }

    public void Reshuffle()
    {
        // for (int i = 0; i < optionBtn.Count; i++) {
        //     var temp = optionBtn[i];
        //     int randomIndex = Random.Range(i, optionBtn.Count);
        //     optionBtn[i] = optionBtn[randomIndex];
        //     optionBtn[randomIndex] = temp;
        //     
        // }
        // for (int i = 0; i < options.Count; i++)
        // {
        //     var temp = options[i];
        //     int randomTemp = Random.Range(0, options.Count);
        //     options[i] = options[randomTemp];
        //     options[randomTemp] = temp;
        //     // optionBtn[i].rectTransform.DOScale(1.3f, 0.3f).SetEase(Ease.Flash).SetLoops(2, LoopType.Yoyo);
        // }
        //
        // for (int i = 0; i < optionBtn.Count; i++)
        // {
        //     optionBtn[i].GetComponent<Image>().sprite = options[i];
        //     optionBtn[i].gameObject.tag = "Wrong";
        //     optionBtn[i].rectTransform.DOScale(1.3f, 0.3f).SetEase(Ease.Flash).SetLoops(2, LoopType.Yoyo);
        // }


        //  PickRandomCorrect();
        ISManager.instance.ShowInterstitialAds();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}