using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    //private void OnSceneGUI()
    //{
    //    var fow = (FieldOfView)target;
    //    Handles.color = Color.white;
    //    Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.viewRadius);
    //    //3D
    //    //Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
    //    var viewAngleA = fow.DirFromAngle2D(-fow.viewAngle / 2, false);
    //    var viewAngleB = fow.DirFromAngle2D(fow.viewAngle / 2, false);
    //    Handles.DrawLine(fow.transform.position, (Vector2)fow.transform.position + viewAngleA * fow.viewRadius);
    //    Handles.DrawLine(fow.transform.position, (Vector2)fow.transform.position + viewAngleB * fow.viewRadius);
    //    foreach (var visibleTarget in fow.visibleTargets)
    //    {
    //        Handles.color = Color.red;
    //        Handles.DrawLine(fow.transform.position, visibleTarget.position);
    //    }

    //}
}