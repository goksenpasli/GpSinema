using Microsoft.Win32;
using Sinema.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sinema.ViewModel
{
    public static class ExtensionMethods
    {
        public static T DeSerialize<T>(this string xmldatapath) where T : class, new()
        {
            XmlSerializer serializer = new(typeof(T));
            using StreamReader stream = new(xmldatapath);
            return serializer.Deserialize(stream) as T;
        }

        public static T DeSerialize<T>(this XElement xElement) where T : class, new()
        {
            XmlSerializer serializer = new(typeof(T));
            return serializer.Deserialize(xElement.CreateReader()) as T;
        }

        public static IEnumerable<int> Bölenler(this int x)
        {
            List<int> List = new();
            for (var i = 1; i <= x; i++)
            {
                if (x % i == 0)
                {
                    List.Add(i);
                    List.Add(x / i);
                }
            }
            return List;
        }

        public static IEnumerable<IGrouping<int, TSource>> GroupBy<TSource>(IEnumerable<TSource> source, int itemsPerGroup)
        {
            return source.Zip(Enumerable.Range(0, source.Count()), (s, r) => new
            {
                Group = r / itemsPerGroup,
                Item = s
            }).GroupBy(i => i.Group, g => g.Item);
        }

        public static string HarfeDöndür(int counter)
        {
            Stack<char> stack = new();
            if (counter == 0)
            {
                return "A";
            }
            while (counter > 0)
            {
                stack.Push((char)('A' + counter % 26));
                counter /= 26;
            }

            return new string(stack.ToArray());
        }

        public static void ResimEkle(Film dc)
        {
            OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Resim Dosyaları (*.jpg;*.jpeg;*.tif;*.tiff;*.png)|*.jpg;*.jpeg;*.tif;*.tiff;*.png" };
            if (openFileDialog.ShowDialog() == true)
            {
                var filename = Guid.NewGuid() + Path.GetExtension(openFileDialog.FileName);
                File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(MainWindowViewModel.xmldatapath)}\\{filename}");
                dc.ResimYolu = filename;
            }
        }

        public static ObservableCollection<string> SalonHarfleri(int max)
        {
            ObservableCollection<string> list = new();
            for (var i = 0; i < max; i++)
            {
                list.Add(HarfeDöndür(i));
            }
            return list;
        }

        public static ObservableCollection<Salon> SalonlarıYükle()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return null;
            }
            if (File.Exists(MainWindowViewModel.xmldatapath))
            {
                return MainWindowViewModel.xmldatapath.DeSerialize<Salonlar>().Salon;
            }
            _ = Directory.CreateDirectory(Path.GetDirectoryName(MainWindowViewModel.xmldatapath));
            return new ObservableCollection<Salon>();
        }

        public static void Serialize<T>(this T dataToSerialize) where T : class
        {
            XmlSerializer serializer = new(typeof(T));
            using TextWriter stream = new StreamWriter(MainWindowViewModel.xmldatapath);
            serializer.Serialize(stream, dataToSerialize);
        }

        public static void VideoEkle(Film dc)
        {
            OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Video Dosyaları (*.mp4;*.avi;*.mov;*.wmv;*.mpg)|*.mp4;*.avi;*.mov;*.wmv;*.mpg" };
            if (openFileDialog.ShowDialog() == true)
            {
                dc.VideoYolu = openFileDialog.FileName;
            }
        }
    }
}