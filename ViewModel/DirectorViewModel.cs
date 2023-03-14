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
        public int IdTipo { get; set; }
        public ObservableCollection<Docentes> Docentes { get; set; } = new ObservableCollection<Docentes>();
        public ObservableCollection<Usuario> Usuarios { get; set; } = new ObservableCollection<Usuario>();
        public ObservableCollection<Grupo> Grupolista { get; set; } = new ObservableCollection<Grupo>();
        public ObservableCollection<Asignatura> AsignaturaLista { get; set; } = new ObservableCollection<Asignatura>();
        public ObservableCollection<Periodo> PeriodoLista { get; set; } = new ObservableCollection<Periodo>();
        public ObservableCollection<DocenteGrupo> DocenteGrupoLista { get; set; } = new ObservableCollection<DocenteGrupo>();
        public ObservableCollection<DocenteAsignatura> DocenteAsignaturaLista { get; set; } = new ObservableCollection<DocenteAsignatura>();
        public DocenteGrupo docgrupo { get; set; } = new DocenteGrupo();

        public string Errores { get; set; }
        readonly Service service = new Service();
        readonly AsignarService asignarserver = new();
        public ICommand LoginCommand { get; set; }
        public ICommand VerAgregarUsuario { get; set; }
        public ICommand VerAgregarDocente { get; set; }
        public ICommand VerEditarUsuarioCommand { get; set; }
        public ICommand VerEditarDocenteCommand { get; set; }
        public ICommand GuardarUsuarioCommand { get; set; }
        public ICommand ConfirmarUsuarioCommand { get; set; }
        public ICommand GuardarDocenteCommand { get; set; }
        public ICommand ConfirmarDocentesCommand { get; set; }
        public Command VerAsignarDocenteGruposCommand { get; set; }
        public Command AsignarGrupoCommand { get; set; }
        public DirectorViewModel()
        {
            
            LoginCommand = new Command(Login);
            VerAgregarUsuario = new Command(NuevoUsuario);
            VerAgregarDocente = new Command(NuevoDocente);
            GuardarUsuarioCommand = new Command(GuardarUsuario);
            GuardarDocenteCommand = new Command(GuardarDocente);
            VerEditarUsuarioCommand = new Command<Usuario>(EditarUsuario);
            ConfirmarUsuarioCommand = new Command<Usuario>(ConfirmarUsuarioAsync);
            ConfirmarDocentesCommand = new Command<Docentes>(ConfirmarDocenteAsync);
            VerEditarDocenteCommand = new Command<Docentes>(EditarDocente);
            VerAsignarDocenteGruposCommand = new Command(VerAsignarDocenteGrupos);
            AsignarGrupoCommand = new Command(AsignarDocenteGrupo);
            Usuario = new Usuario();
            Docente = new Docentes();
            service.Error += Service_Error;
            VerDocentes();
            VerUsuarios();
            CargarGrupo();
            CargarAsignatura();
            CargarPeriodo();
            CargarDocenteGrupos();
            CargarDocenteAsignatura();
            Actualizar(nameof(Usuarios));
        }
        private void NuevoUsuario()
        {
            Usuario = new Usuario();
            AggUsuarioView usuarioview = new AggUsuarioView() { BindingContext = this };
            Application.Current.MainPage.Navigation.PushAsync(usuarioview);
            Errores = "";
            Actualizar(nameof(Errores));
        }
        private void NuevoDocente()
        {
            Docente = new Docentes();
            AggDocenteView docenteview = new AggDocenteView() { BindingContext = this };
            Application.Current.MainPage.Navigation.PushAsync(docenteview);
            Errores = "";
            Actualizar(nameof(Errores));
            VerUsuarios();
        }
        async void GuardarUsuario()
        {
            Errores = "";
            if (Usuario!=null)
            {
                if (Usuario.Id==0)
                {
                    Usuario.Rol = 2;
                    if (await service.InsertUsuario(Usuario))
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
                else
                {
                    if (await service.UpdateUsuario(Usuario))
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();

                    }
                }

            }
            Actualizar(nameof(Usuarios));
            VerUsuarios();

        }
        void EditarUsuario(Usuario u)
        {
            Errores = "";
            Actualizar(nameof(Errores));
            Usuario = new Usuario
            {
                Id = u.Id,
                Usuario1 = u.Usuario1,
                Contraseña = u.Contraseña,
                Rol = u.Rol
            };
            Actualizar(nameof(Usuarios));
            EditarUsuarioView editarusuario = new EditarUsuarioView() { BindingContext = this };
            Application.Current.MainPage.Navigation.PushAsync(editarusuario);
        }
        void EditarDocente(Docentes d)
        {
            Errores = "";
            Actualizar(nameof(Errores));
            Docente = new Docentes
            {
                 Id=d.Id,
               Nombre=d.Nombre,
                ApellidoPaterno=d.ApellidoPaterno,
                 ApellidoMaterno=d.ApellidoMaterno,
                  Telefono= d.Telefono,
                   Edad= d.Edad,
                    Correo = d.Correo,
                    TipoDocente=d.TipoDocente
     
            };
            EditarDocenteView editardocente = new EditarDocenteView() { BindingContext = this };
            Application.Current.MainPage.Navigation.PushAsync(editardocente);
        }
        async void GuardarDocente()
        {
            Errores = "";
            if (Docente != null)
            {
                if (Docente.Id == 0)
                {
                    Docente.IdUsuario = Usuario.Id;
                    Docente.TipoDocente= IdTipo + 1 ;
                    if (await service.InsertDocente(Docente))
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
                else
                {
                    
                    if (await service.UpdateDocente(Docente))
                    {
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                }
            }
            VerDocentes();

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
        private async void ConfirmarUsuarioAsync(Usuario obj)
        {
            bool option = await Application.Current.MainPage.DisplayAlert("Eliminar", "Seguro de Eliminar este usuario?", "Si", "No");
            if (option == true)
            {
                Usuario = obj;
                EliminarUsuario(obj);
            }
        }
        private async void ConfirmarDocenteAsync(Docentes obj)
        {
            bool option = await Application.Current.MainPage.DisplayAlert("Eliminar", "Seguro de Eliminar este docente?", "Si", "No");
            if (option == true)
            {
                Docente = obj;
                EliminarDocentes(obj);
            }
        }
        async void EliminarUsuario(Usuario u)
        {
            Usuarios.Remove(u);
            await service.DeleteUsuario(u);
            VerUsuarios();

        }
        async void EliminarDocentes(Docentes d)
        {
            Docentes.Remove(d);
            await service.DeleteDocente(d);
            VerDocentes();

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

        //
        async void CargarGrupo()
        {
            Grupolista.Clear();
            var datos = await asignarserver.GetGrupo();
            datos.ForEach(x => Grupolista.Add(x));
            Actualizar(nameof(Grupolista));
        }
        async void CargarAsignatura()
        {
            AsignaturaLista.Clear();
            var datos = await asignarserver.GetAsignatura();
            datos.ForEach(x => AsignaturaLista.Add(x));
            Actualizar(nameof(AsignaturaLista));
        }
        async void CargarPeriodo()
        {
            PeriodoLista.Clear();
            var datos = await asignarserver.GetPeriodo();
            datos.ForEach(x => PeriodoLista.Add(x));
            Actualizar(nameof(PeriodoLista));
        }
        async void CargarDocenteGrupos()
        {
            DocenteGrupoLista.Clear();
            var datos = await asignarserver.GetDocenteGrupo();
            datos.ForEach(x => DocenteGrupoLista.Add(x));
            Actualizar(nameof(DocenteAsignaturaLista));
        }
        async void CargarDocenteAsignatura()
        {
            DocenteAsignaturaLista.Clear();
            var datos = await asignarserver.GetDocenteAsignatura();
            datos.ForEach(x => DocenteAsignaturaLista.Add(x));
            Actualizar(nameof(DocenteAsignaturaLista));
        }
        private async void VerAsignarDocenteGrupos()
        {
            docgrupo = new DocenteGrupo();
            GruposView gview= new GruposView() { BindingContext = this };
           

            await Application.Current.MainPage.Navigation.PushAsync(gview);
        }
        
        public Grupo grup { get; set; }
        public Periodo peri { get; set; }
        public async void AsignarDocenteGrupo()
        {
            docgrupo.IdDocente = Docente.Id;
            docgrupo.IdGrupo = grup.Id;
            docgrupo.IdPeriodo = peri.Id;
            DocenteGrupoLista.Add(docgrupo);
            Actualizar(nameof(DocenteGrupoLista));
            await asignarserver.InsertAsignarGrupo(docgrupo);
            await Application.Current.MainPage.Navigation.PopAsync();



        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
