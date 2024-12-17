#pragma warning disable 0649

using UnityEngine;

namespace HexFall
{
    [RegisterModule("Tween", Core = true)]
    public class TweenInitModule : InitModule
    {
        [SerializeField] EasingSettings easingSettings;

        [Space]
        [SerializeField] int tweensUpdateCount = 300;
        [SerializeField] int tweensFixedUpdateCount = 30;
        [SerializeField] int tweensLateUpdateCount = 0;

        [Space]
        [SerializeField] bool verboseLogging;

        public override void CreateComponent(Initialiser Initialiser)
        {
            Tween tween = Initialiser.gameObject.AddComponent<Tween>();
            tween.Init(tweensUpdateCount, tweensFixedUpdateCount, tweensLateUpdateCount, verboseLogging);

            Ease.Initialise(easingSettings);
        }

        public TweenInitModule()
        {
            moduleName = "Tween";
        }
    }
}


// -----------------
// Tween v 1.3.1
// -----------------
