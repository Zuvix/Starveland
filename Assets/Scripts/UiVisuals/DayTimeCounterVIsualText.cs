using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Assets.Scripts.Items
{
    class DayTimeCounterVIsualText : Singleton<DayTimeCounterVIsualText>
    {
        public TMP_Text timerTxt;

        void Start()
        {
            DaytimeCounter.Instance.OnTimeChanged.AddListener(UpdateTimerText);
        }
        private void UpdateTimerText(float TimeToDisplay)
        {
            float minutes = Mathf.FloorToInt(TimeToDisplay / 60);
            float seconds = Mathf.FloorToInt(TimeToDisplay % 60);

            timerTxt.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
