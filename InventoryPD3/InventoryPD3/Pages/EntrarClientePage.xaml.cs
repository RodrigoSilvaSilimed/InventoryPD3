using InventoryPD3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InventoryPD3.Servico.BLL;
using System.Text.RegularExpressions;

namespace Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EntrarClientePage : ContentPage
	{
        const int Cod_Cliente_Len = 6;
        User _login = new User();

        public EntrarClientePage()
        {
            InitializeComponent();
        }

            public EntrarClientePage(User Login)
        {
            InitializeComponent();
            entry_cod_cliente.MaxLength = Cod_Cliente_Len;
            _login = Login;
            NomeUsuario.Text = Login.GivenName + " " + Login.FamilyName;
            //email.Text = Login.Email;
            img_User.Source = Login.Picture;
            entry_cod_cliente.Focus();
        }

        private void btn_Img_Avancar_Clicked(object sender, EventArgs args)
        {
            if (Valida_Cliente(entry_cod_cliente.Text.Trim()))
            {
                _login.Cliente = entry_cod_cliente.Text.ToUpper();
                InventoryPD3.App.NavegarParaInciar(_login);
            }
        }

        private void OnEnterPressed(object sender, EventArgs args)
        {
            if (Valida_Cliente(entry_cod_cliente.Text.Trim()))
            {
                _login.Cliente = entry_cod_cliente.Text.ToUpper();
                InventoryPD3.App.NavegarParaInciar(_login);
            }
        }

            private void entry_cod_cliente_focused(object sender, EventArgs args)
        {
            entry_cod_cliente.Text = "";
        }

        private void entry_cod_cliente_TextChanged(object sender, EventArgs args)
        {

        }

        public bool Valida_Cliente(string cliente)
        {
            //var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
           
            if (!regexItem.IsMatch(cliente))
            {
               
                Mostrar_Alerta("O código de cliente inserido contém caracteres especiais!");
                return false;
            }
            else
            {
                if ((cliente.Length < Cod_Cliente_Len) || (cliente.Length > Cod_Cliente_Len))
                {
                    Mostrar_Alerta("O código de cliente precisa ter exatamente 6 caracteres!");
                    return false;
                }
                else
                {
                    if (cliente.Length == 0)
                    {
                        Mostrar_Alerta("Por favor, digite o código de cliente!");
                        return false;
                    }
                    else
                    {
                        if ((cliente.Length == Cod_Cliente_Len))
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }

        }
        private void Mostrar_Alerta(string _alerta)
        {
            DisplayAlert("Aviso", _alerta, "Ok");
        }

    }
}