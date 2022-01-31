using Alerts.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alerts.Alarms
{
    public class AlarmUtil
    {
        Context context;
        AlarmManager alarmManager;

        public AlarmUtil(Context context)
        {
            this.context = context;
            alarmManager = context.GetSystemService(Context.AlarmService).JavaCast<AlarmManager>();
        }

        public void ScheduleAlarm(Alert alarm)
        {
            var intent = new Intent(context, typeof(AlarmIntentService));
            intent.PutExtra("_gid", alarm.id);
            intent.PutExtra("_Description", alarm.note);
            intent.PutExtra("_DateStart", alarm.start.ToString("dd.MM.yyyy HH:mm:ss"));
            var pendingIntent = PendingIntent.GetBroadcast(context, alarm.id, intent, PendingIntentFlags.UpdateCurrent);
            var triggerOffset = new DateTimeOffset(alarm.start);

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
            {
                alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup,
                        triggerOffset.ToUnixTimeMilliseconds(), pendingIntent);
            }
            else if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
            {
                alarmManager.SetExact(AlarmType.RtcWakeup,
                        triggerOffset.ToUnixTimeMilliseconds(), pendingIntent);
            }
            else
            {
                alarmManager.Set(AlarmType.RtcWakeup,
                        triggerOffset.ToUnixTimeMilliseconds(), pendingIntent);
            }
        }

        
        public void CancelAlarm(Alert alarm)
        {
            var intent = new Intent(context, typeof(AlarmIntentService));
            var pendingIntent = PendingIntent.GetBroadcast(context, alarm.id, intent, PendingIntentFlags.UpdateCurrent);
            alarmManager.Cancel(pendingIntent);
        }
    }
}