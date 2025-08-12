namespace sadklouds.ExcerciseTracker.DataInput;
public interface IUserInput
{
    public int GetIdInput();
    public DateTime GetStartDate();
    public DateTime GetEndDate(DateTime startDate);
    public string GetComment();
}
