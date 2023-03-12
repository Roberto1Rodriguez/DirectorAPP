using DirectorAPP.Models;
using DirectorAPP.Services;
using DirectorAPP.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace DirectorAPP.ViewModel
{
    public class DirectorViewModel : INotifyPropertyChanged

    {
        public Usuario Usuario { get; set; }

        public string Errores { get; set; }
        readonly Service service = new Service();
        public ICommand LoginCommand { get; set; }
        public DirectorViewModel()
        {
            LoginCommand = new Command(Login);
            Usuario = new Usuario();
            service.Error += Service_Error;
        }
        private async void Login()
        {
            Errores = "";
            if (Usuario != null)
            {
                if (await service.Login(Usuario))
                {
                    AggUsuarioView aggusu = new AggUsuarioView() { BindingContext = this };
                    await Application.Current.MainPage.Navigation.PushAsync(aggusu);
                }
               

            }
            Actualizar(nameof(Errores));
        }
        private void Service_Error(List<string> obj)
        {
            Errores = "";
            obj.ForEach(x => Errores += x + "\n");
            Actualizar(nameof(Errores));
        }

        public void Actualizar(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
