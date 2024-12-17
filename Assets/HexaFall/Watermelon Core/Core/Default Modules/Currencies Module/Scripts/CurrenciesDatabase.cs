using UnityEngine;

namespace HexFall
{
    [CreateAssetMenu(fileName = "Currencies Database", menuName = "Games/HexFall/Content/Currencies/Currencies Database")]
    public class CurrenciesDatabase : ScriptableObject
    {
        [SerializeField] Currency[] currencies;
        public Currency[] Currencies => currencies;

        public void Initialise()
        {
            // Initialise currencies
            for(int i = 0; i < currencies.Length; i++)
            {
                currencies[i].Initialise();
            }
        }
    }
}