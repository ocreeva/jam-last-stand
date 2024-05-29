using Moyba.Contracts;
using TMPro;
using UnityEngine;

namespace Moyba.Planet.UI
{
    public class TimePanel : MonoBehaviour
    {
        private const string _PauseText = "||";
        private const string _ResumeText = "►";

        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _day;
        [SerializeField] private TextMeshProUGUI _pause;

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
    }
}
