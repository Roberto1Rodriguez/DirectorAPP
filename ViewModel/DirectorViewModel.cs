using DirectorAPP.Models;
using DirectorAPP.Services;
using DirectorAPP.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public Docentes Docente { get; set; } = null;
        public ObservableCollection<Docentes> Docentes { get; set; } = new ObservableCollection<Docentes>();
        public ObservableCollection<Usuario> Usuarios { get; set; } = new ObservableCollection<Usuario>();
        public string Errores { get; set; }
        readonly Service service = new Service();
        public ICommand LoginCommand { get; set; }
        public ICommand VerAgregar { get; set; }
        public ICommand GuardarUsuarioCommand { get; set; }
        public DirectorViewModel()
        {
            
            LoginCommand = new Command(Login);
            VerAgregar = new Command(NuevoUsuario);
            GuardarUsuarioCommand = new Command(GuardarUsuario);
                Usuario = new Usuario();
            service.Error += Service_Error;
            VerDocentes();
            VerUsuarios();
        }
        private void NuevoUsuario()
        {
            Usuario = new Usuario();
            AggUsuarioView usuarioview = new AggUsuarioView() { BindingContext = this };
            Application.Current.MainPage.Navigation.PushAsync(usuarioview);
            Errores = "";
            Actualizar(nameof(Errores));
        }
        async void GuardarUsuario()
        {
            Errores = "";
            if (Usuario!=null)
            {
                if (Usuario.Id==0)
                {
                    if (await service.InsertUsuario(Usuario))
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
            }
            VerUsuarios();

        }
      async void VerUsuarios()
        {
           
                Usuarios.Clear();
                var datos = await service.GetUsuarios();
                datos.ForEach(v => Usuarios.Add(v));
            
           
        }

        async void VerDocentes()
        {
            Docentes.Clear();
            var datos = await service.GetDocentes();
            datos.ForEach(v =>Docentes.Add(v));
        }
        private async void Login()
        {
            Errores = "";
            if (Usuario != null)
            {
                if (await service.Login(Usuario))
                {
                    App.Current.MainPage=new NavigationPage(new PrincipalView());
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
