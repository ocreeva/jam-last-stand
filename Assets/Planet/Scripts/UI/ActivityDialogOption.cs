using Moyba.Contracts;
using UnityEngine;
using UnityEngine.UI;

namespace Moyba.Planet.UI
{
    public class ActivityDialogOption : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private ActivityDialog _activityDialog;
        [SerializeField] private Image _buttonBackground;

        [Header("Configuration")]
        [SerializeField] private Activity _activity;

        public void HandleClick()
        {
            _activityDialog.UpdateActivity(_activity);
        }

        private void HandleSelectedActivityChanged(UnityEngine.Object _, Activity selectedActivity)
        {
            if (selectedActivity == _activity)
            {
                _buttonBackground.color = Omnibus.ActiveButtonColor;
            }
            else
            {
                _buttonBackground.color = Omnibus.InactiveButtonColor;
            }
        }

        private void OnEnable()
        {
            _activityDialog.OnSelectedActivityChanged += this.HandleSelectedActivityChanged;
        }

        private void Start()
        {
            this.HandleSelectedActivityChanged(_activityDialog, _activityDialog.SelectedActivity);
        }
    }
}
