using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float currentScore;
    public int completedObjectives;
    public TMP_Text scoreText;
    public TMP_Text amountText;
    public int totalAmount;
    void Start()
    {
        // totalAmount = SpawnManager.instance.maxObjectiveCount;
        EventManager.instance.OnObjectDropped += HandleObjectDropped;
        EventManager.instance.OnObjectiveCompleted += IncreaseAmountObjective;
    }

    private void OnDisable()
    {
        EventManager.instance.OnObjectDropped -= HandleObjectDropped;
        EventManager.instance.OnObjectiveCompleted -= IncreaseAmountObjective;
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
        amountText.text = completedObjectives + " / " +  totalAmount;
    }
}
