using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float currentScore;
    public int completedObjectives;
    public TMP_Text scoreText;
    public TMP_Text amountText;
    public int totalAmount;
    public int episode;
    [Header("Episode Text")]
    public GameObject wrapper;
    public TMP_Text textEpisode;
    void Start()
    {

        if (instance == null)
        {
            instance = this;
        }
        // totalAmount = SpawnManager.instance.maxObjectiveCount;
        EventManager.instance.OnObjectDropped += HandleObjectDropped;
        EventManager.instance.OnObjectiveCompleted += IncreaseAmountObjective;
        EventManager.instance.OnEpisodeCompleted += HandleEpisodeCompleted;
    }

    private void OnDisable()
    {
        EventManager.instance.OnObjectDropped -= HandleObjectDropped;
        EventManager.instance.OnObjectiveCompleted -= IncreaseAmountObjective;
        EventManager.instance.OnEpisodeCompleted -= HandleEpisodeCompleted;
    }

    private void HandleObjectDropped(float scoreFromEnemy)
    {
        Debug.Log("Score nambah nih");
        float startScore = currentScore;
        float targetScore = currentScore + scoreFromEnemy;

        DOTween.To(
            () => startScore,
            value =>
            {
                currentScore = value;
                UpdateScoreUI();
            },
            targetScore,
            0.5f
        );
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString("F0");
        }
    }
    private void IncreaseAmountObjective()
    {
        Debug.Log("Amoutn obj nambah nih");
        completedObjectives++;
        amountText.text = completedObjectives + " / " + totalAmount;
        if (completedObjectives >= totalAmount)
        {
            EventManager.instance.WhenEpisodeCompleted(episode);
        }
    }
    private void HandleEpisodeCompleted(int episodeCount)
    {
        StartCoroutine(TextEpisodeAnimation());
        Debug.Log("Episode " + episodeCount + "+selesai, mulai episode 2!");
    }

    private IEnumerator TextEpisodeAnimation()
    {
        wrapper.SetActive(true);
        textEpisode.enabled = true;
        textEpisode.text = "CHAPTER " + episode + " COMPLETE!!!";
        CanvasGroup cg = wrapper.GetComponent<CanvasGroup>();
        cg.DOFade(1, .5f);
        yield return new WaitForSeconds(.5f);
        textEpisode.DOFade(1, .5f);
        yield return new WaitForSeconds(.5f);
        textEpisode.DOFade(0, .5f);
        yield return new WaitForSeconds(.5f);
        cg.DOFade(0, .5f);

        textEpisode.enabled = true;
        wrapper.SetActive(false);
        yield return new WaitForSeconds(1f);
        if (episode == 1)
        {
            GameManagerNaya.instance.ChangeWinSceneEps1();
        }
        else if (episode == 2)
        {
            GameManagerNaya.instance.ChangeWinSceneEps2();
        }
    }
}
