using System;
using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Planet
{
    [CreateAssetMenu(fileName = "NewTimeData", menuName = "Data/Time")]
    public class TimeData : ScriptableObjectContract
    {
        [NonSerialized] private int _day;

        public int Day
        {
            get => _day;
            set => _Set(value, ref _day, changed: this.OnDayChanged);
        }

        public event ValueEventHandler<int> OnDayChanged;
    }
}
