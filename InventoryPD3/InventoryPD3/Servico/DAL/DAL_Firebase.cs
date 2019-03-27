using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using InventoryPD3.Servico.Entidade;
using Plugin.Media.Abstractions;

namespace InventoryPD3.Servico.DAL
{
    public class DAL_Firebase
    {
        public DAL_Firebase()
        {

        }
        public async Task<IReadOnlyCollection<FirebaseObject<Entidade_Leitura>>> BuscarLeitura(Entidade_Leitura leitura)
        {
            var firebase = new FirebaseClient("https://inventorypd3.firebaseio.com/");
            var leituras = await firebase
              .Child("Appv09Bucket")
              .Child("Inventario")
              .Child(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00"))
              .Child(leitura.Cliente)              
              .OrderByKey()
              .StartAt(leitura.Barcode)
              .LimitToFirst(1)
              .OnceAsync<Entidade_Leitura>();
            return leituras;
        }
        public async Task SendToFirebase(Entidade_Leitura leitura)
        {
            //Firebase https://rsamorim.azurewebsites.net/2017/11/07/reagindo-a-evento-com-xamarin-forms-e-firebase/
            //doc git https://github.com/step-up-labs/firebase-database-dotnet
            var firebase = new FirebaseClient("https://inventorypd3.firebaseio.com/");

            // add new item to list of data and let the client generate new key for you (done offline)
            /*var dino = await firebase
              .Child("inventorypd3")
              .PostAsync("nome:rodrigo");
              */
            /*var dinos = await firebase
              .Child("dinosaurs")
              .OrderByKey()
              .StartAt("pterodactyl")
              .LimitToFirst(2)
              .OnceAsync<Dinosaur>();

            foreach (var dino in dinos)
            {
                lb_Resultado.Text = lb_Resultado.Text + $"{dino.Key} is {dino.Object.Height}m high.";
            }*/

            //var cliente = 
            await firebase
                .Child("Appv09Bucket")
                .Child("Inventario")
                .Child(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00"))
                .Child(leitura.Cliente)
                .Child(leitura.Barcode)
                //.PostAsync(leitura);//Firebase cria a chave
                .PutAsync(leitura);// Eu crio a chave

            // note that there is another overload for the PostAsync method which delegates the new key generation to the firebase server
            //Console.WriteLine($"Key for the new dinosaur: {dino.Key}");

            // add new item directly to the specified location (this will overwrite whatever data already exists at that location)
            /* await firebase
               .Child("dinosaurs")
               .Child("t-rex")
               .PutAsync("com overwrite");

             // delete given child node
             /*await firebase
               .Child("dinosaurs")
               .Child("t-rex")
               .DeleteAsync();*/


            

        }
        public async void SendToFirebaseStorage(MediaFile file,Entidade_Leitura leitura)
        {
            //Envio para o FirebaseStorage - está sem lógica pois foi retirado da scanpage e colocado nessa classe apenas para documentar o código para o caso do processo voltar a ter foto
            // Get any Stream - it can be FileStream, MemoryStream or any other type of Stream
            //var stream = File.Open(@"C:\Users\you\file.png", FileMode.Open);
            var stream2 = file.GetStream();
            
            // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
            var task = new FirebaseStorage("inventorypd3.appspot.com")
                   .Child(leitura.Cliente)
                   .Child(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00"))
                   .Child(leitura.Barcode + ".jpg")
                   .PutAsync(stream2);

            file.Dispose();
            // Track progress of the upload
            //task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            // await the task to wait until upload completes and get the download url
            //task.Progress.ProgressChanged += (s, e) => DisplayAlert("Progresso", e.Percentage.ToString("00"), "Ok");
            var downloadUrl = await task;
        }
    }
}
