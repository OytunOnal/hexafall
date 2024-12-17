using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
        [System.Serializable]
        public class ProductSetuper
        {
            [SerializeField] SkinnedMeshRenderer skin;
            [SerializeField] Transform hatHolder;

            public Transform HatHolder => hatHolder;

            public void ApplyProduct(SkinTab tab, GameObject productPrefab)
            {
                if (tab == SkinTab.Color)
                {
                    ColorSkinBehavior skinObject = GameObject.Instantiate(productPrefab).GetComponent<ColorSkinBehavior>();
                    skin.material = skinObject.Material;
                    GameObject.Destroy(skinObject.gameObject);
                }
                if (tab == SkinTab.Hat)
                {
                    OnNewHatSelected(productPrefab);
                }
            }

            public void OnNewHatSelected(GameObject hatPrefab)
            {
                ClearHatHolder();

                var hat = GameObject.Instantiate(hatPrefab);

                hat.transform.SetParent(hatHolder);

                hat.transform.localScale = Vector3.one;
                hat.transform.localPosition = hat.GetComponent<HatSkinBehavior>().ModelPosition;
                hat.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }

            private void ClearHatHolder()
            {
                if (hatHolder.childCount == 0)
                    return;

                for (int i = hatHolder.childCount - 1; i >= 0; i--)
                {
                    GameObject.Destroy(hatHolder.GetChild(i).gameObject);
                }
            }
        }
    
}