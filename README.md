## Steps to reproduce

 1. ```sh
    msbuild -restore -p:Configuration=Release -t:Install AndroidAppSingleView1/AndroidAppSingleView1.csproj
    ```
 2. ```sh
    adb shell setprop debug.mono.log gref
    ```
 3. Run a script similar to the following to launch the app repeatedly until it crashes.
 
    ```sh
    #!/bin/bash
    
    while true 
    do
    	adb logcat -c -G 16M
    	adb shell am start -n com.contoso.androidappsingleview1/crc64ec9f16653b78c539.MainActivity
    	sleep 8
    	adb shell "ps | grep -q com.contoso.androidappsingleview1" || break;
    	adb shell am force-stop com.contoso.androidappsingleview1
    done
    adb logcat -d
    ```

## Version Information

  * Xamarin.Android SDK version 11.0.2.0 (xamarin-android/d16-7@025fde9)
      * Mono: 83105ba
      * Java.Interop: xamarin/java.interop/d16-7@1f3388a
      * Xamarin.Android Tools: xamarin/xamarin-android-tools/d16-7@017078f
  * Testing device: arm64-v8a Android 9.0 Pie (API level 28) Google Pixel 3
