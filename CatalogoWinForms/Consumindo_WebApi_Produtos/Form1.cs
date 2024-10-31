using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;

namespace Consumindo_WebApi_Produtos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string URI = "";
        int codigoProduto = 1;
        private static string _urlBase;
        private static AccessToken accessToken;
        private async void btnObterProdutos_Click(object sender, EventArgs e)
        {
            try
            {
                URI = txtURI.Text;
                var acessaAPI = new AcessaAPIService();
                List<Produto> produtos = await acessaAPI.GetAllProdutos(URI, accessToken);
                dgvDados.DataSource = produtos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private async void btnProdutosPorId_Click(object sender, EventArgs e)
        {
            BindingSource bsDados = new BindingSource();
            InputBox();
            if(codigoProduto !=-1)
            {
                try
                {
                    URI = txtURI.Text + "/" + codigoProduto;
                    var acessaAPI = new AcessaAPIService();
                    Produto produto = await acessaAPI.GetProdutoById(URI, accessToken);
                    bsDados.DataSource = produto;
                    dgvDados.DataSource = bsDados;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro : " + ex.Message);
                }
            }
        }

        private async void btnIncluirProduto_Click(object sender, EventArgs e)
        {
            Random randNum = new Random();
            Produto prod = new Produto();
            prod.Nome = "Novo Produto " + DateTime.Now.Second.ToString();
            prod.Descricao = "Novo Produto descricao " + DateTime.Now.Second.ToString();
            prod.CategoriaId = 1;
            prod.ImagemUrl = "novaImagem" + DateTime.Now.Second.ToString() + ".jpg";
            prod.Preco = randNum.Next(100);
            URI = txtURI.Text;
            try
            {
                var acessaAPI = new AcessaAPIService();
                var resultado = await acessaAPI.AddProduto(URI, accessToken, prod);
                MessageBox.Show(resultado.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }             
        }

        private async void btnAtualizaProduto_Click(object sender, EventArgs e)
        {
            Random randNum = new Random();
            Produto prod = new Produto();
            prod.Descricao = "Novo Produto descricao alterada " + DateTime.Now.Second.ToString();
            prod.Nome = "Novo Produto alterado" + DateTime.Now.Second.ToString();
            prod.CategoriaId = 1;
            prod.ImagemUrl = "novo alterado" + DateTime.Now.Second.ToString() + ".jpg";
            prod.Preco = randNum.Next(100); // atualizando o preço do produto
            InputBox();
            if (codigoProduto != -1)
            {
                prod.ProdutoId = codigoProduto;
                URI = txtURI.Text + "/" + prod.ProdutoId;
                try
                {
                    var acessaAPI = new AcessaAPIService();
                    var resultado = await acessaAPI.UpdateProduto(URI, accessToken, prod);
                    MessageBox.Show(resultado.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro : " + ex.Message);
                }
            }
        }

        private async void btnDeletarProduto_Click(object sender, EventArgs e)
        {
            URI = txtURI.Text;
            InputBox();
            if (codigoProduto != -1)
            {
                try
                {
                    var acessaAPI = new AcessaAPIService();
                    var resultado = await acessaAPI.DeleteProduto(URI, accessToken, codigoProduto);
                    MessageBox.Show(resultado.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro : " + ex.Message);
                }
            }
        }

        private void InputBox()
        {
            /* usando a função VB.Net para exibir um prompt para o usuário informar a senha */
            string Prompt = "Informe o código do Produto.";
            string Titulo = "www.macoratti.net";
            string Resultado = Microsoft.VisualBasic.Interaction.InputBox(Prompt, Titulo, "9", 600, 350);
            /* verifica se o resultado é uma string vazia o que indica que foi cancelado. */
            if (Resultado != "")
            {
                codigoProduto = Convert.ToInt32(Resultado);
            }
            else
            {
                codigoProduto = -1;
            }
        }

        private void BtnAutenticar_Click(object sender, EventArgs e)
        {
            _urlBase = ConfigurationManager.AppSettings["UrlBase"];
            var email = ConfigurationManager.AppSettings["UserID"];
            var password = ConfigurationManager.AppSettings["AccessKey"];
            var confirmPassword = password;

            var urlbase = _urlBase + "autoriza/login";

            using (var client = new HttpClient())
            {
                string conteudo = "";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // Envio da requisição a fim de autenticar
                // e obter o token de acesso
                HttpResponseMessage respToken =
                    client.PostAsync(urlbase, new StringContent(
                        JsonConvert.SerializeObject(new
                        {
                            email,
                            password,
                            confirmPassword
                        }), Encoding.UTF8, "application/json")).Result;

                try
                {
                    conteudo = respToken.Content.ReadAsStringAsync().Result;
                    btnProdutosPorId.Enabled = true;
                    btnObterProdutos.Enabled = true;
                    btnIncluirProduto.Enabled = true;
                    btnDeletarProduto.Enabled = true;
                    btnAtualizaProduto.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro " + ex.Message);
                    throw ex;
                }

                if (respToken.StatusCode == HttpStatusCode.OK)
                {
                    accessToken = JsonConvert.DeserializeObject<AccessToken>(conteudo);

                    if (accessToken.Authenticated)
                    {
                        // Associar o token aos headers do objeto
                        // do tipo HttpClient
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Token);

                        MessageBox.Show($"token JWT Autenticado ");
                    }
                    else
                    {
                        MessageBox.Show("Falha na autenticação");
                    }
                }
            }
        }
    }
}
