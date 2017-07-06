using Xamarin.UITest;

namespace Flowers.Tests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .ApkFile("../../../../Flowers.Forms/Flowers.Forms.Droid/bin/Debug/Flowers.Forms.Droid.apk")
                    .StartApp();
            }

            return ConfigureApp
                .iOS
                .StartApp();
        }
    }
}

