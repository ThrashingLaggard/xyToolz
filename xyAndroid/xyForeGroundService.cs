
using Android.Content;
using Android.Content.PM;
using Android.OS;

using AndroidX.Core.App;
using xyToolz;
using xyToolz.Helper.Logging;

namespace xyAndroid
{

      [Service(ForegroundServiceType = ForegroundService.TypeSpecialUse)]
      public class xyForeGroundService : Service
      {
            private string _notificationChannelID = "1000";
            private string _notificationChannelName = "notificator";
            private int _notificationlID = 1;
            public Context Context { get; set; }

            public xyForeGroundService( )
            {

            }

            public xyForeGroundService( Context context,string channelID, string channelName, int notificationID )
            {
                  Context = context;
                  _notificationChannelID = channelID;
                  _notificationChannelName = channelName;
                  _notificationlID = notificationID;
            }

            /// <summary>
            /// Start the service
            /// 
            /// 
            /// 
            /// Die Werte des Enums:
            ///StickyCompatibility: Dieser Wert ist veraltet und wird nicht mehr verwendet.
            ///Sticky:Lässt Service im Hintergrund weiterlaufen. Sinnvoll für Services, die über einen längeren Zeitraum im Hintergrund laufen sollen.
            ///NotSticky: Bedeutet, dass der Service beendet werden soll , wenn der Aufruf, der ihn gestartet hat , beendet wurde.
            ///RedeliverIntent: Bedeutet, dass der Service den Intent, der ihn gestartet hat , erneut ausführen soll, wenn er beendet wurde.
            ///ContinuationMask: Maske, die verwendet wird, um die Werte von Sticky, NotSticky und RedeliverIntent zu kombinieren.
            /// </summary>
            /// <param name="intent"></param>
            /// <param name="flags"></param>
            /// <param name="startId"></param>
            /// <returns></returns>
            public override StartCommandResult OnStartCommand( Intent? intent , StartCommandFlags flags , int startId )
            {
                  StartForeGroundService(_notificationChannelID,_notificationChannelName);

                  return StartCommandResult.Sticky;
            }

            public ComponentName GetCaller( Intent intent)
            {
                  ComponentName compontentName = StartForegroundService(intent);
                  return compontentName;
            }

            /// <summary>
            /// Muss noch angepasst/ implementiert werden
            /// </summary>
            /// <param name="intent"></param>
            /// <returns></returns>
            public override IBinder? OnBind( Intent? intent )
            {
                  return null;
            }

            /// <summary>
            /// Combine the given input into a new notification channel
            /// </summary>
            /// <param name="notificationManager"></param>
            /// <param name="notificationChannelID"></param>
            /// <param name="notificationChannelName"></param>
            public NotificationChannel CreateNotificationChannel( NotificationManager notificationManager , string notificationChannelID , string notificationChannelName )
            {
                  NotificationChannel? channel = new NotificationChannel(notificationChannelID , notificationChannelName , NotificationImportance.Max);
                  notificationManager.CreateNotificationChannel(channel);
                  return channel;
            }

            public Notification BuildNotification( string channelID , string title = "Alarm!" )
            {
                  Notification notification = new( );
                  try
                  {
#pragma warning disable CA1416 // Plattformkompatibilität überprüfen
                        NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(this , channelID)
                              .SetPriority(NotificationCompat.PriorityMax)
                              .SetCategory(NotificationCompat.CategoryAlarm)
                              .SetVisibility(1)
                              .SetOnlyAlertOnce(false)
                              .SetOngoing(true)
                              .SetContentTitle(title)
                              .SetVibrate(new long [ ] { 69L})
                              .SetChannelId(channelID)
                              .SetAutoCancel(false);
#pragma warning restore CA1416 // Plattformkompatibilität überprüfen

                        notification = notificationBuilder.Build( );
                  }
                  catch (Exception ex)
                  {
                        xyLog.ExLog(ex);
                  }

                  return notification;
            }

            private void StartForeGroundService(string notificationChannelID , string notificationChannelName )
            {
                  NotificationManager? notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
                  if (notificationManager != null)
                  {
                        if (Build.VERSION.SdkInt  >= BuildVersionCodes.O)
                        {
                              CreateNotificationChannel( notificationManager, notificationChannelID, notificationChannelName);
                        }
                        Notification notify =BuildNotification(notificationChannelID);
                        StartForeground(_notificationlID , notify);
                  }
            }












            public class ImplementForeGroundService( )
            {
                  // AndroidManifest:
                  //<uses-permission android:name="android.permission.FOREGROUND_SERVICE"/>

                  ///// <summary>
                  ///// Inhalt kann an Button angehängt werden oÄ, benötigt keine spezifischen EventDaten
                  ///// </summary>
                  ///// <param name="sender"></param>
                  ///// <param name="e"></param>
                  //private void OnStartServiceClicked( object sender , EventArgs e )
                  //{
                  //      Intent intent = new(Application.Context , typeof(ForeGroundService));
                  //      Application.Context.StartForegroundService(intent);
                  //}
            }

      }
}
