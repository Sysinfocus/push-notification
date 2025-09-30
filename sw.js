self.addEventListener('push', function (event) {  
  console.log(event.data);
  const data = event.data.json();
  const notificationTitle = data.notification.title;
  const notificationOptions = {
    body: data.notification.body,
    icon: data.notification.icon, // This will use the icon URL from the payload
    data: data.notification.data // Optional: Custom data for handling clicks
  };

  event.waitUntil(
    self.registration.showNotification(notificationTitle, notificationOptions)
  );
});

self.addEventListener('notificationclick', function (event) {
  event.notification.close();
  // Handle notification click, e.g., open a URL from event.notification.data.url
  if (event.notification.data && event.notification.data.url) {
    clients.openWindow(event.notification.data.url);
  }
});