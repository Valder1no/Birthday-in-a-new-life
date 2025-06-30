using UnityEngine;

public interface ITimeRecordable
{
    void SaveCheckpoint();
    void LoadCheckpoint();
}

