using Microsoft.Extensions.Logging;
using Temporalio.Activities;

namespace MyWorkFlows.Activities;

public class MyActivities
{
    [Activity]
    public async Task<List<string>> LoadTrackingNumbers()
    {

        List<string> trackingNumbers = new List<string>();
        trackingNumbers.Add("tnbr001");
        trackingNumbers.Add("tnbr002");
        trackingNumbers.Add("tnbr003");
        trackingNumbers.Add("tnbr004");
        trackingNumbers.Add("tnbr005");

        return trackingNumbers;
    }

    [Activity]
    public async Task<List<string>> GetTrackingStatus(List<string> trackingNumbers)
    {
        var trackingStatus = new List<string>();
        foreach (var trackingNumber in trackingNumbers)
        {
            trackingStatus.Add(trackingNumber + " Parcel In Transit");
            Console.WriteLine(trackingStatus);
            ActivityExecutionContext.Current.Logger.LogInformation("Status recived for Tracking Number : " + trackingNumber);
        }
        return trackingStatus;
    }

    [Activity]
    public async Task<string> UpdateStatus(List<string> trackingStatus)
    {
        try
        {
            Console.WriteLine("Updating Status");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return "Failed";
        }

        return "Success";
    }


}