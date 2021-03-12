using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Assertions.Must;

public class AwardManager : MonoBehaviour
{
    public static AwardManager instance;
    public Button buyButton;
    public Button homeButton;
    public Button refreshButton;
    public Text buyButtonText;

    public GameObject popUp;
    public GameObject popUpPanel;
    public GameObject[] frames;

    public GameObject[] lockBoxes;
    public Image[] imageBoxes;
    public Sprite[] imageIcons;

    public Image imageSprite;

    public int imageCounts;
    private int boxCount;
    private int buycount;
    public int openedBoxNumber;

    private int kalan;
    private int cost;
    private int imageOrder;
    public int forLockBoxCount = 9;
    int pageNumber;

    private List<int> openedItems = new List<int>();
    private List<int> closeBoxNumbers = new List<int>();


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        openedItems = PlayerPref.GetInt("OpenItems");
        pageNumber = PlayerPrefs.GetInt("PageNumber");
       



        int count = imageIcons.Length - (pageNumber + 1) * 9;
        int forLockBoxCount = 9;
        if (count < 0)
        {
            foreach (GameObject box in lockBoxes)
            {
                box.SetActive(false);
            }

            forLockBoxCount = imageIcons.Length - pageNumber * 9;

            for (int i = 0; i < forLockBoxCount; i++)
            {
                lockBoxes[i].SetActive(true);
            }
        }

        for (int i = 0; i < openedItems.Count; i++)
        {
            imageBoxes[openedItems[i]].sprite = imageIcons[pageNumber * 9 + i];
            lockBoxes[openedItems[i]].SetActive(false);
            imageBoxes[openedItems[i]].gameObject.SetActive(true);
            frames[openedItems[i]].gameObject.SetActive(true);
        }

        for (int i = 0; i < forLockBoxCount; i++)
        {
            if (!openedItems.Contains(i))
            {
                closeBoxNumbers.Add(i);
            }
        }



        imageOrder = PlayerPrefs.GetInt("ImageOrder");
        cost = (imageOrder + 1) * 10;
        buyButtonText.text = cost.ToString();

        if (closeBoxNumbers.Count == 0 && forLockBoxCount == 9)
        {
            refreshButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);

        }
    }

    public void BuyEvents()
    {
        GameManager.instance.totalScore = PlayerPrefs.GetInt("Score");
        PlayerPrefs.SetInt("Score", GameManager.instance.totalScore);
        
        if (GameManager.instance.totalScore >= cost)
        {
            
            buycount =  cost;
            GameManager.instance.totalScore = PlayerPrefs.GetInt("Score")-buycount;
            PlayerPrefs.SetInt("Score", GameManager.instance.totalScore);
            GameManager.instance.lastScore.text = GameManager.instance.totalScore.ToString();
            StartCoroutine(Delay());
            buyButton.interactable = false;

        }
        else
        {
            StartCoroutine(Warning());
        }

    }


    IEnumerator Delay()
    {
        if (closeBoxNumbers.Count == 0)
        {
            yield break;
        }
        int temporaryRandom = 0;
        int randomSayi = 0;
        do
        {
            do
            {
                randomSayi = closeBoxNumbers[Random.Range(0, closeBoxNumbers.Count)];
                if (closeBoxNumbers.Count == 1)
                {
                    break;
                }
            } while (randomSayi == temporaryRandom);
            temporaryRandom = randomSayi;
            boxCount++;
            if (closeBoxNumbers.Count != 1)
            {

                yield return new WaitForSecondsRealtime(0.15f);
                StartCoroutine(ChangeColor(lockBoxes[randomSayi]));
            }
            yield return null;
        }
        while (boxCount != 8);
        boxCount = 0;
        if (closeBoxNumbers.Count != 1)
        {
            yield return new WaitForSecondsRealtime(0.4f);
        }

        openedBoxNumber = closeBoxNumbers[Random.Range(0, closeBoxNumbers.Count)];

        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(ChangeColor(lockBoxes[openedBoxNumber]));
            yield return new WaitForSecondsRealtime(0.15f);
        }


        PlayerPrefs.SetInt("firstControl", 1);
        openedItems.Add(openedBoxNumber);
        closeBoxNumbers.Remove(openedBoxNumber);
        PlayerPref.SetInts("OpenItems", openedItems);
        lockBoxes[openedBoxNumber].SetActive(false);
        imageBoxes[openedBoxNumber].gameObject.SetActive(true);
        frames[openedBoxNumber].gameObject.SetActive(true);
        imageBoxes[openedBoxNumber].sprite = imageIcons[imageOrder];


        popUp.SetActive(true);
        popUpPanel.SetActive(true);

        imageSprite.sprite = imageIcons[imageOrder];

        imageOrder++;
        PlayerPrefs.SetInt("ImageOrder", imageOrder);

        if (imageOrder > imageIcons.Length)
        {
            buyButton.GetComponent<Image>().color = new Color(212f, 213f, 213f, 181f);
            buyButton.interactable = false;
            yield break;
        }


        homeButton.interactable = false;
        buyButton.interactable = false;


        if (closeBoxNumbers.Count == 0)
        {
            refreshButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);

        }
        cost = (imageOrder + 1) * 10;
        buyButtonText.text = cost.ToString();
    }
        

    IEnumerator ChangeColor(GameObject lockBox)
    {
        lockBox.GetComponent<Image>().color = Color.gray;
        yield return new WaitForSecondsRealtime(0.2f);
        lockBox.GetComponent<Image>().color = Color.white;
    }

    public void ClosePopUp()
    {
        popUp.SetActive(false);
        popUpPanel.SetActive(false);
        homeButton.interactable = true;
        buyButton.interactable = true;


    }

    IEnumerator Warning()
    {
        buyButton.GetComponent<Image>().color = new Color(212f, 213f, 213f, 181f);
        buyButton.interactable = false;
        yield return new WaitForSecondsRealtime(1f);
        buyButton.GetComponent<Image>().color = Color.white;
        buyButton.interactable = true;
    }

    public void Refresh()
    {
        kalan = imageIcons.Length - imageOrder;
        int boxCountForCloseBox;
        if (kalan >= 9)
        {
            for (int i = 0; i < lockBoxes.Length; i++)
            {
                lockBoxes[i].SetActive(true);
            }

            boxCountForCloseBox = 9;
        }
        else
        {
            for (int k = 0; k < kalan; k++)
            {
                lockBoxes[k].SetActive(true);
            }


            boxCountForCloseBox = kalan;
        }
        for (int j = 0; j < imageBoxes.Length; j++)
        {
            imageBoxes[j].gameObject.SetActive(false);
        }
        for (int h = 0; h < frames.Length; h++)
        {
            frames[h].gameObject.SetActive(false);
        }

        openedItems.Clear();
        PlayerPref.SetInts("OpenItems", openedItems);
        buyButton.gameObject.SetActive(true);
        refreshButton.gameObject.SetActive(false);
        if (closeBoxNumbers.Count == 0)
        {
            refreshButton.GetComponent<Image>().color = new Color(212f, 213f, 213f, 181f);
            refreshButton.interactable = false;
        }


        for (int i = 0; i < boxCountForCloseBox; i++)
        {
            if (!openedItems.Contains(i))
            {
                closeBoxNumbers.Add(i);
            }
        }

        pageNumber++;
        PlayerPrefs.SetInt("PageNumber", pageNumber);
    }
}
