using System.Collections.Generic;

public interface ITimeRecordable
{
    void RecordSnapshot();
    void ApplySnapshot(TimeSnapshot snapshot);
    List<TimeSnapshot> GetSnapshots();
    void ClearSnapshots();
}
