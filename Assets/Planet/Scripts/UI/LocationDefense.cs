using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Planet.UI
{
    public class LocationDefense : MonoBehaviour
    {
        public void Defend()
        {
            Omnibus.Instance.LoadSpaceCombatScene();
        }
    }
}
