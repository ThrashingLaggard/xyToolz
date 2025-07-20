using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;
using xyToolz.Helper.Logging;

namespace xyAndroid.StoragePermission
{
      public class DefaultStoragePermissionService : IStoragePermissionService
      {
            public string ShowPermissionsForAppManifest()
            {
                  string rules =
                      "<Capabilities>\r\n" +
                      "<uap:Capability Name=\"picturesLibrary\"/>\r\n    " +
                      "<uap:Capability Name=\"documentsLibrary\"/>\r\n    " +
                      "<uap:Capability Name=\"removableStorage\"/>\r\n" +
                      "</Capabilities>\r\n";
                  return rules;
            }
            public async Task GetStoragePermissionsAsync()
            {
                  await RequestStoragePermissionsAsync();
            }

            public static async Task<bool> RequestStoragePermissionsAsync()
            {
                  PermissionStatus statusWrite = PermissionStatus.Granted;
                  PermissionStatus statusRead = PermissionStatus.Granted;

#if ANDROID
                  // Ab Android 10 benötigt man "Scoped Storage" oder MANAGE_EXTERNAL_STORAGE für allgemeinen Zugriff
                  statusWrite = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                  if (statusWrite != PermissionStatus.Granted)
                  {
                        statusWrite = await Permissions.RequestAsync<Permissions.StorageWrite>();
                  }

                  statusRead = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                  if (statusRead != PermissionStatus.Granted)
                  {
                        statusRead = await Permissions.RequestAsync<Permissions.StorageRead>();
                  }

                  // Ab Android 11 (API 30) muss MANAGE_EXTERNAL_STORAGE gesondert angefordert werden
                  if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.R)
                  {
                        var hasManageStoragePermission = Android.Provider.Settings.CanDrawOverlays(Android.App.Application.Context);
                        if (!hasManageStoragePermission)
                        {
                              var intent = new Android.Content.Intent(Android.Provider.Settings.ActionManageAllFilesAccessPermission);
                              intent.SetFlags(Android.Content.ActivityFlags.NewTask);
                              Android.App.Application.Context.StartActivity(intent);
                        }
                  }

#elif IOS
                // iOS erlaubt keinen direkten Zugriff auf das Dateisystem außerhalb der Sandbox.
                // Falls Zugriff auf Dateien außerhalb der App benötigt wird, muss der Benutzer eine Datei über einen Picker auswählen.
                // Es gibt keine explizite Speicherberechtigung, sondern nur Zugriff über den UIDocumentPicker.
                return true;

#elif WINDOWS
            // Unter Windows sind keine Berechtigungen erforderlich, solange die App die Dateien lesen/schreiben kann.
            // Falls jedoch spezielle Ordner (z. B. Downloads) verwendet werden, muss dies in der App-Manifest-Datei definiert werden.
            return true;

#endif

                  var test = statusWrite == PermissionStatus.Granted && statusRead == PermissionStatus.Granted;
                  if (test)
                  {
                        xyLog.Log("OK");
                  }
                  else
                  {
                        xyLog.ExLog((new PermissionException("Insufficient permissions, cant work like this!")));
                  }
                  return test;
            }

      }
}