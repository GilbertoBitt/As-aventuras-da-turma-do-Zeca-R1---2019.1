using System.Collections.Generic;
using UnityEngine;

namespace MiniGames.Memory.Scripts
{
    public class CardRandomizationGroup : ScriptableObject
    {
        public List<CardItem> GroupItemList = new List<CardItem>();

        public List<CardItem> SortCardGroup(int level)
        {
            var spritesChoosen = new List<CardItem>();
            spritesChoosen.Clear();
            switch (level)
            {
                case 0:
                    for (var i = 0; i < 5; i++)
                    {
                        spritesChoosen.Add(GroupItemList[i]);
                    }

                    break;
                case 1:
                    for (var i = 5; i < 10; i++)
                    {
                        spritesChoosen.Add(GroupItemList[i]);
                    }

                    break;
                case 2:
                    for (var i = 0; i < 11; i++)
                    {
                        if (i % 2 == 1)
                        {
                            spritesChoosen.Add(GroupItemList[i]);
                        }
                    }

                    break;
                default:
                    break;
            }

            return spritesChoosen;

        }

        private void OnValidate()
        {
            int tempCount = GroupItemList.Count;
            for (int i = 0; i < tempCount; i++)
            {
                GroupItemList[i].idItem = i;
            }
        }
    }
}