using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpBasic
{
    public partial class frmAgendaContatos : Form
    {
        private OperacaoEnum acao;
        public frmAgendaContatos()
        {
            InitializeComponent();
        }

        private void frmAgendaContatos_Shown(object sender, EventArgs e)
        {
            AlterarBotoesSalvarECancelar(false);
            AlterarBotoesIncluirAlterarEExcluir(true);
            CarregarListaContatos();
            AlterarEstadoCampos(false);
        }
        private void AlterarBotoesSalvarECancelar(bool estado)
        {
            btnSalvar.Enabled = estado;
            btnCancelar.Enabled = estado;
        }
        private void AlterarBotoesIncluirAlterarEExcluir(bool estado)
        {
            btnIncluir.Enabled = estado;
            btnAlterar.Enabled = estado;
            btnExcluir.Enabled = estado;
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            AlterarBotoesSalvarECancelar(true);
            AlterarBotoesIncluirAlterarEExcluir(false);
            AlterarEstadoCampos(true);
            acao = OperacaoEnum.INCLUIR;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            AlterarBotoesIncluirAlterarEExcluir(true);
            AlterarBotoesSalvarECancelar(false);
            AlterarEstadoCampos(false);
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            AlterarBotoesSalvarECancelar(true);
            AlterarBotoesIncluirAlterarEExcluir(false);
            AlterarEstadoCampos(true);
            acao = OperacaoEnum.ALTERAR;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
                Contato contato = new Contato
                {
                    Nome = txbNome.Text,
                    Email = txbEmail.Text,
                    NumeroTelefone = txbTelefone.Text
                };
                List < Contato > contatosList = new List<Contato>();
                foreach (Contato contatoDaLista in lbxContatos.Items)
                {
                    contatosList.Add(contatoDaLista);
                }

            if (acao == OperacaoEnum.ALTERAR)
            {
                int indice = lbxContatos.SelectedIndex;
                contatosList.RemoveAt(indice);
                contatosList.Insert(indice, contato);
            }
            else
            {
                contatosList.Add(contato);
            }

            ManipuladorArquivos.EscreverArquivo(contatosList);
            CarregarListaContatos();
            AlterarBotoesSalvarECancelar(false);
            AlterarBotoesIncluirAlterarEExcluir(true);
            LimparCampos();
            AlterarEstadoCampos(false);
        }

        private void CarregarListaContatos()
        {
            lbxContatos.Items.Clear();
            lbxContatos.Items.AddRange(ManipuladorArquivos.LerArquivo().ToArray());
        }

        private void LimparCampos()
        {
            txbTelefone.Text = "";
            txbEmail.Text = "";
            txbNome.Text = "";
        }

        private void AlterarEstadoCampos(bool estado)
        {
            txbNome.Enabled = estado;
            txbEmail.Enabled = estado;
            txbTelefone.Enabled = estado;
        }

        private void lbxContatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Contato contato = (Contato)lbxContatos.Items[lbxContatos.SelectedIndex];
            txbNome.Text = contato.Nome;
            txbEmail.Text = contato.Email;
            txbTelefone.Text = contato.NumeroTelefone;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Tem certeza?", "Pergunta", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int indiceExcluido = lbxContatos.SelectedIndex;
                    lbxContatos.SelectedIndex = 1;
                    lbxContatos.Items.RemoveAt(indiceExcluido);
                    List<Contato> contatosList = new List<Contato>();
                    foreach (Contato contato in lbxContatos.Items)
                    {
                        contatosList.Add(contato);
                    }
                    ManipuladorArquivos.EscreverArquivo(contatosList);
                    CarregarListaContatos();
                    LimparCampos();
                }
            }
            catch(ArgumentOutOfRangeException)
            {
                MessageBox.Show("Selecione um item para excluir.");
            }
        }
    }
}
