# Simple_weather
Simple weather app crated in xamarin forms.
Only main folder, no folders of OS, have to add manually.

Android:
Open the AndroidManifest.xml file under the Properties folder and add the following inside of the manifest node:

<uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
<uses-feature android:name="android.hardware.location" android:required="false" />
<uses-feature android:name="android.hardware.location.gps" android:required="false" />
<uses-feature android:name="android.hardware.location.network" android:required="false" />

Or right-click on the Android project and open the project's properties. Under Android Manifest find the Required permissions: area and check the ACCESS_COARSE_LOCATION and ACCESS_FINE_LOCATION permissions. This will automatically update the AndroidManifest.xml file.

IOS:
Your app's Info.plist must contain the NSLocationWhenInUseUsageDescription key in order to access the device’s location.
Open the plist editor and add the Privacy - Location When In Use Usage Description property and fill in a value to display to the user.
Or manually edit the file and add the following and update the rationale:
<key>NSLocationWhenInUseUsageDescription</key>
<string>Fill in a reason why your app needs access to location.</string>
