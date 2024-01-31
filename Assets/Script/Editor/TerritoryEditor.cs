using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Territory))]
public class TerritoryEditor : Editor
{
    private bool playerParametersFoldout = true;
    private bool uiFoldout = true;
    private bool invokeFoldout = true;
    private bool gameObjectFoldout = true;
    private bool soundFoldout = true;
    private bool privateVariablesFoldout = true;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Player Parameters
        playerParametersFoldout = EditorGUILayout.Foldout(playerParametersFoldout, "Player Parameters");
        if (playerParametersFoldout)
        {
            SerializedProperty moveSpeedProp = serializedObject.FindProperty("moveSpeed");
            if (moveSpeedProp != null)
            {
                EditorGUILayout.PropertyField(moveSpeedProp);
            }

            SerializedProperty recoveryTimeProp = serializedObject.FindProperty("recoveryTime");
            if (recoveryTimeProp != null)
            {
                EditorGUILayout.PropertyField(recoveryTimeProp);
            }

            SerializedProperty healthProp = serializedObject.FindProperty("health");
            if (healthProp != null)
            {
                EditorGUILayout.PropertyField(healthProp);
            }

            SerializedProperty maxHealthProp = serializedObject.FindProperty("maxHealth");
            if (maxHealthProp != null)
            {
                EditorGUILayout.PropertyField(maxHealthProp);
            }
        }

        // UI
        uiFoldout = EditorGUILayout.Foldout(uiFoldout, "UI");
        if (uiFoldout)
        {
            SerializedProperty healthBarProp = serializedObject.FindProperty("healthBar");
            if (healthBarProp != null)
            {
                EditorGUILayout.PropertyField(healthBarProp);
            }

            SerializedProperty productionBarProp = serializedObject.FindProperty("productionBar");
            if (productionBarProp != null)
            {
                EditorGUILayout.PropertyField(productionBarProp);
            }

            SerializedProperty recoveryBarProp = serializedObject.FindProperty("recoveryBar");
            if (recoveryBarProp != null)
            {
                EditorGUILayout.PropertyField(recoveryBarProp);
            }
        }

        // Invoke
        invokeFoldout = EditorGUILayout.Foldout(invokeFoldout, "Invoke");
        if (invokeFoldout)
        {
            SerializedProperty minionsProp = serializedObject.FindProperty("minions");
            if (minionsProp != null)
            {
                EditorGUILayout.PropertyField(minionsProp);
            }

            SerializedProperty spawnIntervalProp = serializedObject.FindProperty("spawnInterval");
            if (spawnIntervalProp != null)
            {
                EditorGUILayout.PropertyField(spawnIntervalProp);
            }
        }

        // Game Object
        gameObjectFoldout = EditorGUILayout.Foldout(gameObjectFoldout, "Game Object");
        if (gameObjectFoldout)
        {
            SerializedProperty territoryCircleProp = serializedObject.FindProperty("territoryCircle");
            if (territoryCircleProp != null)
            {
                EditorGUILayout.PropertyField(territoryCircleProp);
            }

            SerializedProperty innerRadiusProp = serializedObject.FindProperty("innerRadius");
            if (innerRadiusProp != null)
            {
                EditorGUILayout.PropertyField(innerRadiusProp);
            }
        }

        // Sound
        soundFoldout = EditorGUILayout.Foldout(soundFoldout, "Sound");
        if (soundFoldout)
        {
            SerializedProperty takingTerritoryProp = serializedObject.FindProperty("takingTerritory");
            if (takingTerritoryProp != null)
            {
                EditorGUILayout.PropertyField(takingTerritoryProp);
            }

            SerializedProperty territoryDeployementProp = serializedObject.FindProperty("territoryDeployement");
            if (territoryDeployementProp != null)
            {
                EditorGUILayout.PropertyField(territoryDeployementProp);
            }

            SerializedProperty territoryUnlayableProp = serializedObject.FindProperty("territoryUnlayable");
            if (territoryUnlayableProp != null)
            {
                EditorGUILayout.PropertyField(territoryUnlayableProp);
            }
        }

        // Private Variables
        privateVariablesFoldout = EditorGUILayout.Foldout(privateVariablesFoldout, "Private Variables");
        if (privateVariablesFoldout)
        {
            SerializedProperty audioSourceProp = serializedObject.FindProperty("audioSource");
            if (audioSourceProp != null)
            {
                EditorGUILayout.PropertyField(audioSourceProp);
            }

            SerializedProperty spawnRadiusProp = serializedObject.FindProperty("spawnRadius");
            if (spawnRadiusProp != null)
            {
                EditorGUILayout.PropertyField(spawnRadiusProp);
            }

            // Repeat the above pattern for other private variables...
        }

        serializedObject.ApplyModifiedProperties();
    }
}
