using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    public class CosumableItem : ItemSO, IDestroyableItem, IItemAction
    {
        public string ActionName => "Consume";

        public AudioClip actionSFX { get; private set; }

        public bool PerformAction(GameObject character)
        {
            throw new System.NotImplementedException();
        }

    }

    public interface IDestroyableItem
    {
    }

    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character);
    }
}