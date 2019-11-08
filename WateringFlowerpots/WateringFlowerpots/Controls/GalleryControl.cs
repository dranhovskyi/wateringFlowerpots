using System;
using System.Collections.Generic;
using DryIoc;
using FFImageLoading.Cache;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Services;
using Xamarin.Forms;

namespace WateringFlowerpots.Controls
{
    public class GalleryControl : StackLayout
    {
        public static readonly BindableProperty ImagePathProperty = BindableProperty.Create
            (nameof(ImagePath),
            typeof(string),
            typeof(GalleryControl),
            null,
            BindingMode.TwoWay);

        public string ImagePath
        {
            get
            {
                return (string)GetValue(ImagePathProperty);
            }
            set
            {
                SetValue(ImagePathProperty, value);
            }
        }

        private readonly CachedImage image;
        private readonly Button addPhotoButton;
        private readonly IPageDialogService dialogService;

        public GalleryControl()
        {
            this.dialogService = App.AppContainer.Resolve<IPageDialogService>();

            Orientation = StackOrientation.Horizontal;

            image = new CachedImage
            {
                HeightRequest = 70,
                WidthRequest = 70,
                CacheType = CacheType.None,
                Transformations = new List<ITransformation>
                {
                    new RoundedTransformation { Radius = 25 }
                }
            };

            addPhotoButton = new Button
            {
                Style = (Style)Application.Current.Resources["greenButton"],
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
                Text = " ADD PHOTO ",
                VerticalOptions = LayoutOptions.Center
            };
            addPhotoButton.Clicked += ButtonClicked;

            Children.Add(image);
            Children.Add(addPhotoButton);
        }

        private async void ButtonClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                storageStatus = results[Permission.Storage];
            }

            if (storageStatus == PermissionStatus.Granted)
            {
                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    CompressionQuality = 20,                    
                    SaveMetaData = false                   
                });

                if (file != null)
                {
                    image.Source = ImagePath = file.Path;                    
                }
            }
            else
            {
                await dialogService.DisplayAlertAsync("Alert", "Unable to take photos from gallery. Provide storage permissions", "OK");
                CrossPermissions.Current.OpenAppSettings();
            }
        }
    }
}
