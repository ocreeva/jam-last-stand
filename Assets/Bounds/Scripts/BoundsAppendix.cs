using Moyba.Contracts;
using UnityEngine;

namespace Moyba.Bounds
{
    public class BoundsAppendix : TraitBase<BoundsManager>
    {
        [Header("Components")]
        [SerializeField] private BoundsPanel _northBounds;
        [SerializeField] private BoundsPanel _westBounds;
        [SerializeField] private BoundsPanel _eastBounds;
        [SerializeField] private BoundsPanel _southBounds;

        private void Awake()
        {
            this._Assert(_manager.Appendix == null, "is replacing a non-stub instance.");

            _manager.Appendix = this;
        }

        private void OnDestroy()
        {
            this._Assert(ReferenceEquals(_manager.Appendix, this), "is stubbing a different instance.");

            _manager.Appendix = null;
        }

        private void Update()
        {
            var shipPosition = Omnibus.Ship.Position;
            this.Update_CheckBounds(shipPosition.y, _northBounds);
            this.Update_CheckBounds(-shipPosition.y, _southBounds);
            this.Update_CheckBounds(-shipPosition.x, _westBounds);
            this.Update_CheckBounds(shipPosition.x, _eastBounds);
        }

        private void Update_CheckBounds(float distance, BoundsPanel boundsPanel)
        {
            boundsPanel.ShouldShowAlert = distance > _manager.AlertDistance;
            boundsPanel.ShouldShowWarning = distance > _manager.WarningDistance;
        }
    }
}
