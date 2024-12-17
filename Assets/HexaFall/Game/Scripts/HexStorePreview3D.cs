using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
    public class HexStorePreview3D : SkinPreview3D
    {
        [SerializeField] GameObject characterPrefab;

        private BaseCharacterBehaviour character;

        public override void Init()
        {
            base.Init();

            character = Instantiate(characterPrefab, PrefabParent).GetComponent<BaseCharacterBehaviour>();
            character.transform.localPosition = Vector3.zero;
            character.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            character.OnCharacterSkinSelected(SkinTab.Color, SkinStoreController.GetSelectedProductData(SkinTab.Color));
            character.OnCharacterSkinSelected(SkinTab.Hat, SkinStoreController.GetSelectedProductData(SkinTab.Hat));

            // disabling nickname
            character.transform.GetChild(1).gameObject.SetActive(false);
        }

        public override void SpawnProduct(SkinData data)
        {
            if (data.Tab == SkinTab.Color)
            {
                character.OnCharacterSkinSelected(SkinTab.Color, SkinStoreController.GetSelectedProductData(SkinTab.Color));
            }
            else if (data.Tab == SkinTab.Color)
            {
                character.OnCharacterSkinSelected(SkinTab.Hat, SkinStoreController.GetSelectedProductData(SkinTab.Hat));
            }
        }
    }
}