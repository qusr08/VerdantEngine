using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Enemy_Attack_SO", order = 1)]
public class EnemyAttack_SO : ScriptableObject
{
    public enum EnemyAttackType
    {
        Heal,
        Electric,
        Fire,
        Poison,
        Frost
    }

    public enum EnemyTargeting
    {
        Line,
        Shape,
        All,
        Allies,
        Self
    }

    [System.Serializable]
    public class Wrapper<T>
    {
        public T[] value;
    }
    public string attackName;

[HideInInspector]public Wrapper<bool>[] attackGrid;
    [HideInInspector] public int attackSize = 5; // Change from static to instance variable
    public EnemyTargeting targetType;
    public EnemyAttackType attackType;
    public int damage;
    public bool lineAttackIsHorizental;
    public int maxCoolDown;


    private void OnEnable()
    {
        if (attackGrid == null || attackGrid.Length != attackSize)
            ResetGrid();
    }

    public void ResetGrid()
    {
        attackGrid = new Wrapper<bool>[attackSize];
        for (int i = 0; i < attackSize; i++)
        {
            attackGrid[i] = new Wrapper<bool>();
            attackGrid[i].value = new bool[attackSize];
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyAttack_SO))]
public class EnemyAttackEditor : Editor
{
    SerializedProperty attackGrid;
    SerializedProperty attackSize;

    private void OnEnable()
    {
        attackGrid = serializedObject.FindProperty("attackGrid");
        attackSize = serializedObject.FindProperty("attackSize");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EnemyAttack_SO script = (EnemyAttack_SO)target;

        GUILayout.Label("Attack Grid Size");
        attackSize.intValue = EditorGUILayout.IntField(attackSize.intValue);

        if (GUILayout.Button("Reset Grid"))
            script.ResetGrid();

        DrawGrid();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawGrid()
    {
        try
        {
            if (attackGrid.arraySize != attackSize.intValue)
            {
                return; // Prevent accessing out-of-bounds elements
            }

            GUILayout.BeginVertical();
            for (int i = 0; i < attackSize.intValue; i++)
            {
                SerializedProperty row = attackGrid.GetArrayElementAtIndex(i).FindPropertyRelative("value");

                if (row.arraySize != attackSize.intValue)
                {
                    continue; // Prevent errors from uninitialized inner arrays
                }

                GUILayout.BeginHorizontal();
                for (int j = 0; j < attackSize.intValue; j++)
                {
                    SerializedProperty value = row.GetArrayElementAtIndex(j);
                    bool element = value.boolValue;
                    if (GUILayout.Button(element ? "X" : "O", GUILayout.MaxWidth(50)))
                    {
                        value.boolValue = !value.boolValue;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
        }
    }
}
#endif
