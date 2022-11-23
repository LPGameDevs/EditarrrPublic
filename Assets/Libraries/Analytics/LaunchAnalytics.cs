#if UNITY_PACKAGE_ANALYTICS

using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class LaunchAnalytics : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {

        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
            Debug.Log(e.Reason);
        }

    }
}
#endif
