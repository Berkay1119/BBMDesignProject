using System;
using Backend.Components;

namespace Backend.EasyEvent
{
    public static class EventBus
    {
        // Add every event here
        public static event Action<BaseComponent, BaseComponent> OnCollision2D;
        public static event Action<BaseComponent> OnSpawn;
        public static event Action<BaseComponent> OnDestroy;

        public static void PublishCollision(BaseComponent a, BaseComponent b)
            => OnCollision2D?.Invoke(a, b);

        public static void PublishSpawn(BaseComponent comp)
            => OnSpawn?.Invoke(comp);

        public static void PublishDestroy(BaseComponent comp)
            => OnDestroy?.Invoke(comp);
    }
}