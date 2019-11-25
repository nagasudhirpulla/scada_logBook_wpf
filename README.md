# SCADA Logbook data fetching Application

* This app shows the max min values, time at which max min values occurred of frequency, WR demand and IR flows, for a given start and end times.
* When opened or on pressing the guess times button, the app automatically sets the appropriate start and end times based on the current time reducing the user effort to manually edit the start and end times.
* This app reduces the user effort to see plots and extract excel data for shift summary values.
* This app requires edna client which is able to access historian data to be installed in the SCADA PC as the only dependency.
* This is a WPF application and required .NET Framework >= 4.5.2 to be installed in a windows machine.