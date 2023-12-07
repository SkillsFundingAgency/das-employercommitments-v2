namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

public class DataLockPriceChange
{
    public DateTime CurrentStartDate { get; set; }

    public DateTime? CurrentEndDate { get; set; }

    public DateTime IlrStartDate { get; set; }

    public DateTime? IlrEndDate { get; set; }

    public decimal CurrentCost { get; set; }

    public decimal IlrCost { get; set; }

    public bool MissingPriceHistory { get; set; }
}