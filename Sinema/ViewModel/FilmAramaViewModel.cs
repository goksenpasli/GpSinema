using Sinema.Model;
using Sinema.View;
using System;

namespace Sinema.ViewModel
{
    public class FilmAramaViewModel : Film
    {
        public FilmAramaViewModel()
        {
            PropertyChanged += FilmAramaViewModel_PropertyChanged;
        }

        private void FilmAramaViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is "Adı")
            {
                try
                {
                    WebFilmArama.xmlDataProvider.Source = new Uri($"http://www.omdbapi.com/?t={Adı}&apikey=c30efe8&r=xml");
                }
                catch (Exception)
                {
                }
            }
        }
    }
}