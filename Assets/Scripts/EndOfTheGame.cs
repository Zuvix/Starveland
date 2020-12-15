using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndOfTheGame : MonoBehaviour
{
    public TMP_Text gameOverText;
    public TMP_Text gameWonText;
    public TMP_Text totalDaysNumber;
    public TMP_Text humansSavedNumber;
    public TMP_Text grade1;
    public TMP_Text grade2;
    public TMP_Text finalGrade; 

    private void Awake()
    {
        if (PlayerPrefs.GetInt("GameOver") == 0)
        {
            this.gameWonText.gameObject.SetActive(false);
            this.finalGrade.text = "FX";
            this.grade1.text = "FX";
            this.grade2.text = "FX";
        }
        else
        {
            this.gameOverText.gameObject.SetActive(false);
            CalculateGrades();
        }
        totalDaysNumber.text = PlayerPrefs.GetInt("DaysPassed").ToString();
        humansSavedNumber.text = $"{PlayerPrefs.GetInt("PlayersSurvived")}/{PlayerPrefs.GetInt("MaxPlayers")}";
    }

    private void CalculateGrades()
    {
        string grade1;
        string grade2;
        string finalGrade;

        switch (Mathf.FloorToInt(Mathf.Abs((PlayerPrefs.GetInt("DaysPassed") - 1)) / 5))
        {
            case 0:
                {
                    grade1 = "A";
                    break;
                }
            case 1:
                {
                    grade1 = "B";
                    break;
                }
            case 2:
                {
                    grade1 = "C";
                    break;
                }
            case 3:
                {
                    grade1 = "D";
                    break;
                }
            default:
                grade1 = "E";
                break;
        }
        switch (PlayerPrefs.GetInt("MaxPlayers")-PlayerPrefs.GetInt("PlayersSurvived"))
        {
            case 0:
                {
                    grade2 = "A";
                    break;
                }
            case 1:
                {
                    grade2 = "B";
                    break;
                }
            case 2:
                {
                    grade2 = "C";
                    break;
                }
            case 3:
                {
                    grade2 = "D";
                    break;
                }
            default:
                grade2 = "E";
                break;
        }
        switch ((((float)grade1[0] + (float)grade2[0])/2f)-65f)
        {
            case 0f:
                {
                    finalGrade = "A";
                    break;
                }
            case 0.5f:
                {
                    finalGrade = "A-";
                    break;
                }
            case 1.0f:
                {
                    finalGrade = "B";
                    break;
                }
            case 1.5f:
                {
                    finalGrade = "B-";
                    break;
                }
            case 2.0f:
                {
                    finalGrade = "C";
                    break;
                }
            case 2.5f:
                {
                    finalGrade = "C-";
                    break;
                }
            case 3.0f:
                {
                    finalGrade = "D";
                    break;
                }
            case 3.5f:
                {
                    finalGrade = "D-";
                    break;
                }
            case 4.0f:
                {
                    finalGrade = "E";
                    break;
                }
            default:
                finalGrade = "E";
                break;
        }

        this.grade1.text = grade1;
        this.grade2.text = grade2;
        this.finalGrade.text = finalGrade;
    }

    public void ButtonBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
