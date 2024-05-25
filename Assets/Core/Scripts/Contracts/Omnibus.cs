using UnityEngine;

namespace Moyba.Contracts
{
    [DefaultExecutionOrder(-99)]
    public class Omnibus : ContractBase
    {
        private static Omnibus _Instance;

        private void Awake()
        {
            if (_Instance)
            {
                if (_Instance != this) Object.Destroy(this.gameObject);
            }
            else
            {
                _Instance = this;
                Object.DontDestroyOnLoad(this.gameObject);
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {

        }
#endif
    }
}
