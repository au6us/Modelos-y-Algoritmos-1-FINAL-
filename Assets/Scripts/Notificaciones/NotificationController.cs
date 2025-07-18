using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Notifications;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif 
public class NotificationController : MonoBehaviour
{
    private void Start()
    {
#if UNITY_ANDROID
        StartCoroutine(PermisoNotificacion());
#endif
    }
    private void OnApplicationQuit()
    {
        ActivarNotificacion();
    }

    public void ActivarNotificacion()
    {
        DateTime fechaActivar = DateTime.Now.AddSeconds(5f); //5 segundos para notificar 
#if UNITY_ANDROID
        MakeNotification(fechaActivar);
#endif
    }

#if UNITY_ANDROID
    private const string idCanal = "Canal Notificacion";

    

    public void MakeNotification(DateTime fecha)
    {
        AndroidNotificationChannel androidNotificationChannel = new AndroidNotificationChannel
        {
            Id = idCanal,
            Name = "Canal Notificacion",
            Description = "Canal para notificaciones",
            Importance = Importance.Default
        };

        AndroidNotificationCenter.RegisterNotificationChannel(androidNotificationChannel);

        AndroidNotification androidNotification = new AndroidNotification
        {
            Title = "FROGGY QUIERE ENTRENAR",
            Text = "¡Vuelve! ¡Hay que ganar esa carrera!",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = fecha
        };

        AndroidNotificationCenter.SendNotification(androidNotification, idCanal);
    }

    IEnumerator PermisoNotificacion()
    {
        var request = new PermissionRequest();
        while(request.Status == PermissionStatus.RequestPending)
        {
            yield return null;
        }
    }

#endif
}
