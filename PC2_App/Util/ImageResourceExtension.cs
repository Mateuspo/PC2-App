using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PC2_App.Util
{
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            ImageSource imageSource;

            switch (Source)
            {
                case "PC2_App.Util.Src.WhatsApp.png":
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        imageSource = ImageSource.FromResource("PC2_App.Util.Src.WhatsApp2.png", typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                    }
                    else
                    {
                        imageSource = ImageSource.FromResource("PC2_App.Util.Src.WhatsApp.png", typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                    }
                    break;

                case "PC2_App.Util.Src.Atualizar.png":
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        imageSource = ImageSource.FromResource("PC2_App.Util.Src.Atualizar2.png", typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                    }
                    else
                    {
                        imageSource = ImageSource.FromResource("PC2_App.Util.Src.Atualizar.png", typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                    }
                    break;

                case "PC2_App.Util.Src.Sair.png":
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        imageSource = ImageSource.FromResource("PC2_App.Util.Src.Sair2.png", typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                    }
                    else
                    {
                        imageSource = ImageSource.FromResource("PC2_App.Util.Src.Sair.png", typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                    }
                    break;

                default:
                    imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                    break;
            }

            return imageSource;
        }
    }
}
