using Alerts.Models;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alerts.Alarms
{
    [BroadcastReceiver]
    public class AlarmIntentService : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Bundle bundle = intent.Extras;
            var alarm = new Alert
            {
                id = Convert.ToInt32(bundle.GetString("_gid")),
                note = bundle.GetString("_Description"),
                start = Convert.ToDateTime(bundle.GetString("_DateStart"), new System.Globalization.CultureInfo("ru-RU"))
            };

            var resultIntent = new Intent(context, typeof(MainActivity));

            PendingIntent pending = PendingIntent.GetActivity(context, 0,
                resultIntent,
                PendingIntentFlags.OneShot);

            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.O)
            {
                var manager = NotificationManager.FromContext(context);
                Android.Net.Uri soundUri = Android.Net.Uri.Parse(ContentResolver.SchemeAndroidResource + "://" + Application.Context.PackageName + "/" + Resource.Raw.Alarm);
                var builder = new NotificationCompat.Builder(context)
                                                    .SetAutoCancel(true)
                                                    .SetContentIntent(pending)
                                                    .SetSmallIcon(Resource.Drawable.Icon)
                                                    .SetCategory(Notification.CategoryAlarm)
                                                    .SetSound(soundUri)
                                                    .SetVibrate(new long[] { 1000, 1000, 1000, 1000, 1000 })
                                                    .SetLights(Android.Graphics.Color.Red, 3000, 3000)
                                                    .SetContentText(alarm.start.ToString("dd.MM.yyyy, HH:mm") + ": " + alarm.note)
                                                    .SetContentTitle(Application.Context.GetString(Resource.String.app_name));
                manager.Notify(1331, builder.Build());
            }
            else
            {
                using (var notificationManager = NotificationManager.FromContext(context))
                {
                    Notification notification;
                    // Настройте канал уведомлений.
                    var myUrgentChannel = context.PackageName;
                    const string channelName = "Срочное сообщение";

                    Android.Net.Uri soundUri = Android.Net.Uri.Parse(ContentResolver.SchemeAndroidResource + "://" + Application.Context.PackageName + "/" + Resource.Raw.Alarm);
                    NotificationChannel channel;
                    channel = notificationManager.GetNotificationChannel(myUrgentChannel);
                    if (channel == null)
                    {
                        channel = new NotificationChannel(myUrgentChannel, channelName, NotificationImportance.High);
                        channel.EnableVibration(true);
                        channel.EnableLights(true);
                        channel.SetSound(
                           soundUri,
                            new Android.Media.AudioAttributes.Builder().SetUsage(Android.Media.AudioUsageKind.Notification).Build()
                        );
                        channel.LockscreenVisibility = NotificationVisibility.Public;
                        notificationManager.CreateNotificationChannel(channel);
                    }
                    channel?.Dispose();

                    Notification.InboxStyle inboxStyle = new Notification.InboxStyle();

                    inboxStyle.AddLine(alarm.start.ToString("dd.MM.yyyy, HH:mm") + ": " + alarm.note);
                    notification = new Notification.Builder(context)
                                                .SetChannelId(myUrgentChannel)
                                                .SetContentTitle(Application.Context.GetString(Resource.String.app_name))
                                                .SetContentText(alarm.start.ToString("dd.MM.yyyy, HH:mm") + ": " + alarm.note)
                                                .SetAutoCancel(true)
                                                .SetSmallIcon(Resource.Drawable.Icon)
                                                .SetContentIntent(pending)
                                                .SetStyle(inboxStyle)
                                                .Build();
                    notificationManager.Notify(1331, notification);
                    notification.Dispose();
                }
            }
        }
    }
}