using System.Collections;
using Moyba.Contracts;
using TMPro;
using UnityEngine;

namespace Moyba.Planet.UI
{
    public class TimePanel : MonoBehaviour
    {
        private const string _PauseText = "||";
        private const string _ResumeText = "►";

        [SerializeField] private PlanetTime _planetTime;

        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _day;
        [SerializeField] private TextMeshProUGUI _pause;
        [SerializeField] private GameObject _pauseButton;

        [Header("Configuration")]
        [SerializeField] private bool _autoAdvanceFirstDay;
        [SerializeField, Range(0f, 10f)] private float _firstDayDelay = 0f;
        [SerializeField, Range(0f, 10f)] private float _showPauseButtonDelay = 0f;

        private void HandleDayChanged(UnityEngine.Object _, int day)
        {
            _day.text = $"{day:N0}";
        }

        private void HandlePause(UnityEngine.Object _)
        {
            _pause.text = _ResumeText;
        }

        private void HandleResume(UnityEngine.Object _)
        {
            _pause.text = _PauseText;
        }

        private void OnEnable()
        {
            Omnibus.Planet.Time.OnDayChanged += this.HandleDayChanged;
            Omnibus.Planet.Time.OnPause += this.HandlePause;
            Omnibus.Planet.Time.OnResume += this.HandleResume;
        }

        private IEnumerator Start()
        {
            if (_autoAdvanceFirstDay)
            {
                if (_firstDayDelay > 0f) yield return new WaitForSeconds(_firstDayDelay);

                _planetTime.AdvanceDay();
            }

            if (_showPauseButtonDelay > 0f) yield return new WaitForSeconds(_showPauseButtonDelay);

            _pauseButton.SetActive(true);
        }
    }
}
