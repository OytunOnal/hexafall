using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HexFall
{

    [CreateAssetMenu(fileName = "NicknamesDB", menuName = "Games/HexFall/Content/NicknamesDB")]
    public class NicknamesDatabase : ScriptableObject
    {
        [SerializeField]
        private List<string> nicknames;

        public void ParseNamesAndAddToDB(TextAsset textAsset)
        {
            int stringLength = textAsset.text.Length;
            StringBuilder nameBuilder = new StringBuilder();

            int namesAdded = 0;
            int namesSkiped = 0;

            for (int i = 0; i < stringLength; i++)
            {
                if (textAsset.text[i] != ';')
                {
                    nameBuilder.Append(textAsset.text[i]);
                }
                else
                {
                    string newName = nameBuilder.ToString();
                    nameBuilder.Clear();

                    if (newName != null)
                    {
                        if (nicknames.IndexOf(newName) == -1)
                        {
                            nicknames.Add(newName);
                            namesAdded++;
                        }
                        else
                        {
                            namesSkiped++;
                        }
                    }
                }
            }

            Debug.Log("Names added: " + namesAdded + ". Names skiped: " + namesSkiped);
        }

        public string GetRandomNick()
        {
            return nicknames[Random.Range(0, nicknames.Count)];
        }

        public List<string> GetRandomUniqueNicks(int nicksAmount)
        {
            if (nicksAmount >= nicknames.Count)
            {
                if (nicksAmount > nicknames.Count)
                {
                    Debug.LogError("[Nicknames DB] Amount of names in datatbase(" + nicknames.Count + ") is less than asked amount: " + nicksAmount);
                }

                return new List<string>(nicknames);
            }

            List<string> resultList = new List<string>();

            if (nicksAmount <= 30 && nicksAmount / nicknames.Count < 0.15f)
            {
                List<int> usedNickIndexes = new List<int>();

                for (int i = 0; i < nicksAmount; i++)
                {
                    int randomIndex;

                    do
                    {
                        randomIndex = Random.Range(0, nicknames.Count);
                    }
                    while (usedNickIndexes.IndexOf(randomIndex) != -1);

                    usedNickIndexes.Add(randomIndex);
                    resultList.Add(nicknames[randomIndex]);
                }
            }
            else
            {
                List<int> notUsedNicksIndexes = new List<int>();
                for (int i = 0; i < nicknames.Count; i++)
                {
                    notUsedNicksIndexes.Add(i);
                }

                for (int i = 0; i < nicksAmount; i++)
                {
                    int randomIndex = Random.Range(0, notUsedNicksIndexes.Count);
                    notUsedNicksIndexes.Remove(randomIndex);
                    resultList.Add(nicknames[randomIndex]);
                }
            }

            return resultList;
        }
    }
}