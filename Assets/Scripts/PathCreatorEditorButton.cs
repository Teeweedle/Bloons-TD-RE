using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BloonPathCreator))]
public class PathCreatorEditorButton : Editor
{    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BloonPathCreator lBloonPathCreator = (BloonPathCreator)target;
        if(GUILayout.Button("Create Point"))
        {
            lBloonPathCreator.CreateNewPoint();
        }

        if(GUILayout.Button("Delete Path"))
        {
            lBloonPathCreator.DeletePath();
        }
    }
}
