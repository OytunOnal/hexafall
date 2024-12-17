using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexFall
{
    public class Hex
    {
        public Vector2Int GridHexCoords { get; private set; }
        public HexState State { get; private set; }

        private Vector3 worldPos;

        private PlatformBehaviour platform;
        private BrickBehavior brick;
        private List<BaseCharacterBehaviour> steppedCharacters = new List<BaseCharacterBehaviour>();

        private MapLayer mapLayer;
        public HexType HexType { get; private set; }

        public float lastTimeStepped;
        public bool HasBrick() => brick != null;

        public Hex(ActiveHex hexData, PlatformBehaviour platformBehaviour)
        {
            HexType = hexData.type;

            GridHexCoords = hexData.gridPosition;
            worldPos = hexData.position;

            platform = platformBehaviour;
            platform.ResetPlatfrom();
            platform.transform.position = GetWorldCoords();
            lastTimeStepped = Time.time;

            if (HexType == HexType.Ground)
                ChangeHexState(HexState.Active, true);
            else if (HexType == HexType.Hidden || HexType == HexType.Border)
                ChangeHexState(HexState.Disabled, true);            
        }

        public void SteppedOn(BaseCharacterBehaviour character)
        {
            switch (State)
            {
                case HexState.Active:

                    ChangeHexState(HexState.Active, false);
                    steppedCharacters.Add(character);

                    if (brick != null)
                    {
                        character.AddBrick(brick);

                        brick = null;
                    }

                    lastTimeStepped = Time.time;
                    platform.OnStep();

                    break;

                case HexState.Disabled:

                    if (character.HasBrick())
                    {
                        ChangeHexState(HexState.Active, false);

                        lastTimeStepped = Time.time;

                        character.RemoveBrick(platform.transform.position, platform.transform.rotation, immediately: true, onCompleted: delegate
                        {
                            steppedCharacters.Add(character);
                            platform.OnStep();
                        });
                    }
                    else
                    {
                        character.OnAutoJump();
                    }

                    break;
            }
        }

        public void SteppedOff(BaseCharacterBehaviour character, bool isGraphicStepOff = false)
        {
            //if (Time.time - lastTimeStepped < 1.5f ) return;
            if (steppedCharacters.Contains(character))
            {
                steppedCharacters.Remove(character);
            }

            if (steppedCharacters.Count == 0 && isGraphicStepOff)
            {
                platform.OnStepOff();
                return;
            }

            if (steppedCharacters.Count == 0)
            {
                platform.OnStepOff();
            }

            if (State == HexState.Active)
            {
                ChangeHexState(HexState.Disabled, false);
            }
        }


        public Vector3 GetWorldCoords()
        {
            return worldPos;
        }

        public void ChangeHexState(HexState state, bool immediately = false)
        {
            State = state;

            switch (State)
            {
                case HexState.Active:
                    platform.SetActiveState(true);

                    break;

                case HexState.Disabled:

                    //if (lastTimeStepped < 1) return;
                    foreach(BaseCharacterBehaviour character in steppedCharacters)
                    {
                        if (character.HasBrick())
                        {
                            ChangeHexState(HexState.Active, false);

                            lastTimeStepped = Time.time;

                            character.RemoveBrick(platform.transform.position, platform.transform.rotation, immediately: true, onCompleted: delegate
                            {
                                platform.OnStep();
                            });
                            return;
                        }
                    }

                    platform.SetDisabledState(true);

                    break;
            }
        }

        public void SetBrick(BrickBehavior brick)
        {
            this.brick = brick;

            brick.transform.position = GetWorldCoords();
            brick.transform.rotation = Quaternion.identity;
        }

        public void SetMap(MapLayer map)
        {
            mapLayer = map;
        }

        public void ForEachNeighbour(HexMap.HexDelegate func, bool onlyExisting = false)
        {
            for (int i = 0; i < 6; i++)
            {
                var neihgbourCoords = GridHexCoords + (GridHexCoords.y % 2 == 0 ? HexMap.NeighbourCoordsEven[i] : HexMap.NeighbourCoordsOdd[i]);

                var neighbour = onlyExisting ? HexMap.GetExistingHex(mapLayer, neihgbourCoords) : HexMap.GetHexStatic(mapLayer, neihgbourCoords);

                if (neighbour != null)
                {
                    func?.Invoke(neighbour);
                }
            }
        }
    }
}
