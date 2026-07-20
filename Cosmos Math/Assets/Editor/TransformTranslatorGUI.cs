using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static AshVectors;

[CustomEditor(typeof(PlanetTransformer))]
[CanEditMultipleObjects]

public class TransformTranslatorGUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update Meshes"))
        {
            foreach (Transform selectedTransform in Selection.transforms)
            {

                if (selectedTransform.GetComponent<PlanetTransformer>())
                {

                    selectedTransform.GetComponent<PlanetTransformer>().userReadableRotation = AshMaths.EulerToQuat(selectedTransform.GetComponent<PlanetTransformer>().userReadableEulerAngles);
                    selectedTransform.GetComponent<PlanetTransformer>().UpdateMeshes();

                }

            }

        }

    }
}