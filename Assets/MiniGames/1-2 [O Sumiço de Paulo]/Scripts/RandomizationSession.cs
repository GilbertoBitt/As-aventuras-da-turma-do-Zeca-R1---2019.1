using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MiniGames.Scripts
{
    [Serializable]
    public class RandomizationSession
    {
        [BoxGroup("Configurações de Sessão")]
        public RandomizationType randomizationType;

        [ShowIf("randomizationType", RandomizationType.Sides)]
        [BoxGroup("Configurações de Sessão")]
        public int minSides = 3;
        [ShowIf("randomizationType", RandomizationType.Sides)]
        [BoxGroup("Configurações de Sessão")]
        public int maxSides = 6;


        [ShowIf("randomizationType", RandomizationType.Vertices)]
        [BoxGroup("Configurações de Sessão")]
        public int minVertices = 3;
        [ShowIf("randomizationType", RandomizationType.Vertices)]
        [BoxGroup("Configurações de Sessão")]
        public int maxVertices = 6;
        [HideIf("isPopulated")]
        public bool isPopulated;
        public List<GeometricPlaneObject> items;

        [ShowIf("isPopulated", true)]
        [TableList] public List<RandomizationItem> objectsOfSession;

        [HideIf("isPopulated")]
        [Button("Populate Objects")]
        public void PopulateAssets()
        {

            if (objectsOfSession == null)
            {
                objectsOfSession = new List<RandomizationItem>();
            }
            objectsOfSession.Clear();
            foreach (var item in items)
            {
                objectsOfSession.Add(new RandomizationItem
                {
                    Random = false,
                    itemObjectPlane = item
                });
            }

            isPopulated = true;

        }

        }

    [Serializable]
    public class RandomizationItem
    {
        [HideLabel]
        public bool Random;
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        public GeometricPlaneObject itemObjectPlane;


    }
}