using System;
using Moyba.Contracts;

namespace Moyba.Planet.UI
{
    public class ActivityDialog : TraitBase<PlanetManager>
    {
        [NonSerialized] private LocationData _locationData;
        [NonSerialized] private Activity _selectedActivity;

        public Activity SelectedActivity
        {
            get => _selectedActivity;
            set => _Set(value, ref _selectedActivity, changed: this.OnSelectedActivityChanged);
        }

        public LocationData LocationData => _locationData;

        public event ValueEventHandler<Activity> OnSelectedActivityChanged;

        public void HandleAssign()
        {
            _locationData.Activity = this.SelectedActivity;
        }

        public void UpdateActivity(Activity activity)
        {
            this.SelectedActivity = activity;
        }

        private void OnEnable()
        {
            var location = Omnibus.Planet.Target.Location;
            _locationData = _manager.GetLocationData(location);

            this.SelectedActivity = _locationData.Activity;
        }
    }
}
