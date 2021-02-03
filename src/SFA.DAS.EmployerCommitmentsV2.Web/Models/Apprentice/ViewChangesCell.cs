
namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice
{
    public class ViewChangesCell<T>
    {
        public T CurrentValue { get; set; }
        public T NewValue { get; set; }

        public ViewChangesCell(T currentValue, T newValue)
        {
            CurrentValue = currentValue;
            NewValue = newValue;
        }
    }
}