﻿using Microsoft.Win32;
using Sinema.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
                string filename = Guid.NewGuid() + Path.GetExtension(openFileDialog.FileName);
                File.Copy(openFileDialog.FileName, $"{Path.GetDirectoryName(MainWindowViewModel.xmldatapath)}\\{filename}");
                dc.ResimYolu = filename;
            }
        }     
        
        public static void VideoEkle(Film dc)
        {
            OpenFileDialog openFileDialog = new() { Multiselect = false, Filter = "Video Dosyaları (*.mp4)|*.mp4" };
            if (openFileDialog.ShowDialog() == true)
            {
                dc.VideoYolu = openFileDialog.FileName;
            }
        }

        public static ObservableCollection<string> SalonHarfleri(int max)
        {
            ObservableCollection<string> list = new();
            for (int i = 0; i < max; i++)
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
    }
}