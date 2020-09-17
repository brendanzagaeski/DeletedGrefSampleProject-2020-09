## Steps to reproduce

 1. ```sh
    msbuild -restore -p:Configuration=Release -t:Install AndroidAppSingleView1/AndroidAppSingleView1.csproj
    ```
 2. ```sh
    adb shell setprop debug.mono.log gref
    ```
 3. ```sh
    adb shell am start -n com.contoso.androidappsingleview1/crc64ec9f16653b78c539.MainActivity
    ```
 4. After the app displays "Hello World!" on the screen, collect the GREFs log:

    ```sh
    adb shell run-as com.contoso.androidappsingleview1 cat files/.__override__/grefs.txt > grefs.txt
    ```

## Expected behavior

The GREFs log (_grefs.txt_) should show a new GREF created for the `Android.OS.IBinderInvoker` instance:

```
handle 0x26f6; key_handle 0xf865f0c: Java Type: `android/view/ViewRootImpl$W`; MCW type: `Android.OS.IBinderInvoker`
```

After that, the handle (`0x26f6` in this example) should not appear again in the log.

## Actual behavior

With arm64-v8a AOT, after the GREF is created for the `Android.OS.IBinderInvoker` instance, the log shows that it is later removed and replaced with a different GREF (`0x26f2` in this example):

```
*take_weak obj=0x7e6aa58270; handle=0x26f6
+w+ grefc 13 gwrefc 1 obj-handle 0x26f6/G -> new-handle 0x1b7/W from thread 'finalizer'(14580)
take_weak_global_ref_jni
-g- grefc 12 gwrefc 1 handle 0x26f6/G from thread 'finalizer'(14580)
take_weak_global_ref_jni
*try_take_global obj=0x7e6aa58270 -> wref=0x1b7 handle=0x26f2
+g+ grefc 13 gwrefc 1 obj-handle 0x1b7/W -> new-handle 0x26f2/G from thread 'finalizer'(14580)
take_global_ref_jni
-w- grefc 13 gwrefc 0 handle 0x1b7/W from thread 'finalizer'(14580)
take_global_ref_jni
```

This indicates that the managed garbage collector considers the `IBinderInvoker` instance to be eligible for garbage collection. That is unexpected because the current method is holding a reference to it (in the `windowToken` argument) when the garbage collector is called.

It appears that the use of `windowToken` in a ternary expression just before the garbage collector runs is somehow responsible for the behavior.

```csharp
static void TernaryThenCollect(IBinder windowToken)
{
    System.IntPtr ptr = windowToken == null ? System.IntPtr.Zero : System.IntPtr.Zero;
    // `windowToken` should not be eligible for collection during this GC, but it is.
    System.GC.Collect();
}
```

## Version Information

  * Xamarin.Android SDK version 11.0.2.0 (xamarin-android/d16-7@025fde9)
      * Mono: 83105ba
      * Java.Interop: xamarin/java.interop/d16-7@1f3388a
      * Xamarin.Android Tools: xamarin/xamarin-android-tools/d16-7@017078f
  * Testing device: arm64-v8a Android 9.0 Pie (API level 28) Google Pixel 3
