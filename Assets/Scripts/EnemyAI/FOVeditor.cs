using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyAiTutorial))]
public class FOVeditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyAiTutorial fov = (EnemyAiTutorial)target;

        // Draw detection radius
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        // Draw view angle boundaries
        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        // Draw line to player if in sight
        if (fov.playerInSightRange && fov.player != null)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.player.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        float angle = eulerY + angleInDegrees;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
