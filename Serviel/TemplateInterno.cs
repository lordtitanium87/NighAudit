using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ErpBS100;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomForm;
using StdPlatBS100;
using StdBE100;
using PRISDK100;
using UpgradeHelpers.Spread;
using VndBE100;
using Primavera.Extensibility.Integration.Modules.AccountingServices.Services;
using IntBE100;
using static BasBE100.BasBETiposGcp;
using Primavera.Extensibility.Extensions;
using System.Drawing;
using System.Data.SqlClient;

namespace NightAudit
{
    public partial class TemplateInterno : CustomForm
    {
        public static StdBSInterfPub Plataforma;
        public static ErpBS MotorLE = new ErpBS();
        private const string GridVersion = "01.00";
        private const string GridName = "GridDemo";
        private const string colCaixa = "x.Conta";
        private const string colBanco = "y.Conta";
        private const string colComissoes = "0 as 'Comissões apuradas'";
        // Hidden column
        private const string colModulo = "Modulo";
        private const string colTipoEntidade = "TipoEntidade";
        private const string colFilial = "Filial";
        private bool controlsInitialized;
        private string categoriaEntidade = "mntTabClientes";
        private string categoriaEntidadesExternas = "mntTabEntidadesExternas";
        public static string connectionString;
        public static decimal valorComissoesTotal = 0;
        public TemplateInterno()
        {
            InitializeComponent();
        }

        private void TemplateInterno_Load(object sender, EventArgs e)
        {
            PriSDKContext.Initialize(BSO, PSO);
            InitializeSDKControls();
            InitializeGrid();
        }

        private void InitializeSDKControls()
        {
            //Initializes controls
            if (!controlsInitialized)
            {
                // Initialize the controls with the SDK context   
                priGrelha1.Inicializa(PriSDKContext.SdkContext);
                controlsInitialized = true;
            }
        }

        private void InitializeGrid()
        {
            StdBECamposChave campoChave = new StdBECamposChave();
            priGrelha1.BandaMenuContexto = "PopupGrelhasStd";
            priGrelha1.IniciaDadosConfig();
            // Number of groupings allowed (maximum of 4)
            priGrelha1.AddColAgrupa();
            priGrelha1.AddColAgrupa();
            priGrelha1.AddColAgrupa();
            priGrelha1.AddColAgrupa();
            // Add a custom comand to the activebar.
            //priGrelha1.AddOpcaoActiveBar(0, "mnuCriaEntidade", "Novo", null, StdBrandingInfo100.Properties.RibbonResourcesVND.novo_16);
            //priGrelha1.AddOpcaoActiveBar(1, "mnuEditarEntidade", "Editar", null,
            //        StdBrandingInfo100.Properties.RibbonResourcesVND.clientes_16);

            // Normal columns 
            priGrelha1.AddColKey(colCaixa, FpCellType.CellTypeEdit, "Conta Caixa", 24, true, strCamposBaseDados: colCaixa, blnDrillDown: true);
            priGrelha1.AddColKey(colBanco, FpCellType.CellTypeEdit, "Banco", 24, true, strCamposBaseDados: colBanco);
            priGrelha1.AddColKey(colComissoes, FpCellType.CellTypeCurrency, "Comissões Apuradas", 10, false, true, strCamposBaseDados: colComissoes);
            //Default grouping
            priGrelha1.AdicionaAgrupamento(priGrelha1.Cols.GetEdita(colCaixa).Number);
            //priGrelha1.AdicionaAgrupamento(priGrelha1.Cols.GetEdita(colModuloDesc).Number);
            // Set greid default behavior
            priGrelha1.TituloGrelha = "Demo Grid 1";
            priGrelha1.PermiteAgrupamentosUser = true;
            priGrelha1.PermiteOrdenacao = true;
            priGrelha1.PermiteActualizar = true;
            priGrelha1.PermiteFiltros = true;
            priGrelha1.PermiteDetalhes = true;
            priGrelha1.PermiteStatusBar = true;
            priGrelha1.PermiteDataFill = true;
            priGrelha1.PermiteVistas = true;
            // Read the last grid layout for the current user.
            if (!priGrelha1.LeXML("GRIDDEMO", BSO.Contexto.UtilizadorActual, GridName, GridName, GridVersion))
            {
                priGrelha1.FormataGrelha(true);
            }
            priGrelha1.LimpaGrelha();

        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Atualizar_Click(object sender, EventArgs e)
        {
            try
            {
                LoadGrid();
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraErroSimples("", StdPlatBS100.StdBSTipos.IconId.PRI_Critico, ex.Message);
            }
        }
        private void LoadGrid()
        {
            StdBELista lista;
            StringBuilder query = new StringBuilder();
            StdBECamposChave campoChave = new StdBECamposChave();
            query.AppendLine(string.Format("SELECT {0}", priGrelha1.DaCamposBDSelect()));
            query.AppendLine("FROM (SELECT conta from ContasBancarias " +
                            " inner join tdu_caixasvsrdt on tdu_caixasvsrdt.cdu_caixas = ContasBancarias.conta " +
                            " where tipoconta = 4) as x, (SELECT conta from ContasBancarias where tipoconta = 0) as y");
            lista = new StdBELista();
            lista = PriSDKContext.SdkContext.BSO.Consulta(query.ToString());
            priGrelha1.DataBind(lista);
            for (int i = 1; i <= priGrelha1.Grelha.DataRowCnt; i++)
            {
                campoChave = new StdBECamposChave();
                if (string.IsNullOrEmpty(priGrelha1.GetGRID_GetValorCelula(i, colCaixa)) || string.IsNullOrEmpty(priGrelha1.GetGRID_GetValorCelula(i, colBanco)))
                {

                }
                else
                {
                    campoChave.AddCampoChave("CDU_caixaBanco", priGrelha1.GetGRID_GetValorCelula(i, colCaixa) + priGrelha1.GetGRID_GetValorCelula(i, colBanco));
                    if (MotorLE.TabelasUtilizador.Existe("TDU_DetalheComissoes", campoChave) == true)
                    {
                        priGrelha1.SetGRID_SetValorCelula(i, colComissoes, BSO.TabelasUtilizador.DaValorAtributo("TDU_DetalheComissoes", campoChave, "CDU_Valor"));
                    }
                }
            }
        }

        private void Converter_Click(object sender, EventArgs e)
        {
            StdBECamposChave campoChave = new StdBECamposChave();
            valorComissoesTotal = 0;


            for (int i = 1; i <= priGrelha1.Grelha.DataRowCnt; i++)
            {
                if (string.IsNullOrEmpty(priGrelha1.GetGRID_GetValorCelula(i, colBanco)))
                {

                }
                else
                {
                    campoChave = new StdBECamposChave();
                    campoChave.AddCampoChave("CDU_caixaBanco", Convert.ToString(priGrelha1.GetGRID_GetValorCelula(i, colCaixa)) + Convert.ToString(priGrelha1.GetGRID_GetValorCelula(i, colBanco)));
                    if (MotorLE.TabelasUtilizador.Existe("TDU_DetalheComissoes", campoChave) == true)
                    {
                        MotorLE.TabelasUtilizador.ActualizaValorAtributo("TDU_DetalheComissoes", campoChave, "CDU_Valor", priGrelha1.GetGRID_GetValorCelula(i, colComissoes));
                    }
                    else
                    {
                        InsertNovaConta(Convert.ToString(priGrelha1.GetGRID_GetValorCelula(i, colCaixa)) + Convert.ToString(priGrelha1.GetGRID_GetValorCelula(i, colBanco)));
                        MotorLE.TabelasUtilizador.ActualizaValorAtributo("TDU_DetalheComissoes", campoChave, "CDU_Valor", priGrelha1.GetGRID_GetValorCelula(i, colComissoes));

                    }
                    valorComissoesTotal = valorComissoesTotal + Convert.ToDecimal(priGrelha1.GetGRID_GetValorCelula(i, colComissoes));
                }
            }
            PSO.Dialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, "Gravado com sucesso", StdBSTipos.IconId.PRI_Informativo);

        }
        public static bool InsertNovaConta(string Nome)
        {
            string strSql;
            int novaConta;
            try
            {
                string[] parametrosNomes = new string[1];
                parametrosNomes[0] = "@CDU_caixaBanco";
                string[] parametrosValores = new string[1];
                parametrosValores[0] = Nome;
                strSql = "INSERT INTO TDU_DetalheComissoes (CDU_caixaBanco) values (@CDU_caixaBanco)";
                // strSql.AppendLine("VALUES")
                // strSql.AppendLine("(@CDU_Artigo,@CDU_ArtigoEquivalente, @CDU_ArtigoDesc, @CDU_ArtigoEquivalenteDesc)")
                novaConta = CRUD(strSql.ToString(), parametrosNomes, parametrosValores);
                if (novaConta == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        public static int CRUD(string sql, string[] parameterNames, string[] parameterVals)
        {
            try
            {
                using (SqlConnection connection = GetDbConnection())
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        FillParameters(command, parameterNames, parameterVals);
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void FillParameters(SqlCommand command, string[] parameterNames, string[] parameterVals)
        {
            try
            {
                if (parameterNames != null)
                {
                    for (var i = 0; i <= parameterNames.Length - 1; i++)
                        command.Parameters.AddWithValue(parameterNames[i], parameterVals[i]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SqlConnection GetDbConnection()
        {
            SqlConnection connection;
            try
            {
                string conString = connectionString;//Plataforma.BaseDados.DaConnectionStringNET(Plataforma.BaseDados.DaNomeBDdaEmpresa(MotorLE.Contexto.CodEmp), "DEFAULT");
                // ConnectionString()
                connection = new SqlConnection(conString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                return connection = null;
            }
            finally
            {
                connection = null;
            }
        }

        private void Sair_Click(object sender, EventArgs e)
        {
            StdBECamposChave campoChave = new StdBECamposChave();
            valorComissoesTotal = 0;


            for (int i = 1; i <= priGrelha1.Grelha.DataRowCnt; i++)
            {
                if (string.IsNullOrEmpty(priGrelha1.GetGRID_GetValorCelula(i, colCaixa)) || string.IsNullOrEmpty(priGrelha1.GetGRID_GetValorCelula(i, colBanco)))
                {

                }
                else
                {
                    campoChave = new StdBECamposChave();
                    campoChave.AddCampoChave("CDU_caixaBanco", priGrelha1.GetGRID_GetValorCelula(i, colCaixa) + priGrelha1.GetGRID_GetValorCelula(i, colBanco));
                    if (MotorLE.TabelasUtilizador.Existe("TDU_DetalheComissoes", campoChave) == true)
                    {
                        priGrelha1.SetGRID_SetValorCelula(i, colComissoes, BSO.TabelasUtilizador.DaValorAtributo("TDU_DetalheComissoes", campoChave, "CDU_Valor"));
                    }
                    else
                    {
                        InsertNovaConta(priGrelha1.GetGRID_GetValorCelula(i, colCaixa) + priGrelha1.GetGRID_GetValorCelula(i, colBanco));
                        MotorLE.TabelasUtilizador.ActualizaValorAtributo("TDU_DetalheComissoes", campoChave, "CDU_Valor", priGrelha1.GetGRID_GetValorCelula(i, colComissoes));
                    }
                    valorComissoesTotal = valorComissoesTotal + Convert.ToDecimal(priGrelha1.GetGRID_GetValorCelula(i, colComissoes));
                }
            }
            frm_NightAudit.comissoes = valorComissoesTotal;
            frm_NightAudit.templateOpened = true;
            this.Close();
        }

        private void TemplateInterno_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_NightAudit.comissoes = valorComissoesTotal;
            frm_NightAudit.templateOpened = true;
        }
    }
}
