using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpBS100;
using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomForm;
using StdBE100;
using StdPlatBS100;
using UpgradeHelpers.Spread;

namespace NightAudit
{
    public partial class DetalheModoRecebimento : CustomForm
    {
        public static StdBSInterfPub Plataforma;
        public static ErpBS MotorLE = new ErpBS();
        private const string GridVersion = "01.00";
        private const string GridName = "GridDemo";
        private const string colCaixa = "y.CDU_Caixas as Caixa";
        private const string colBanco = "y.Movim";
        private const string colComissoes = "y.Total";
        private const string colConta = "'' as Conta";
        // Hidden column
        private const string colModulo = "Modulo";
        private const string colTipoEntidade = "TipoEntidade";
        private const string colFilial = "Filial";
        private bool controlsInitialized;
        private string categoriaEntidade = "mntTabClientes";
        private string categoriaEntidadesExternas = "mntTabEntidadesExternas";
        public static string connectionString;
        public static decimal valorComissoesTotal = 0;
        public static string resumoTesouraria;
        public static string dataReferencia;
        public DetalheModoRecebimento()
        {
            InitializeComponent();
        }

        private void DetalheModoRecebimento_Load(object sender, EventArgs e)
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
            priGrelha1.AddColKey(colCaixa, FpCellType.CellTypeEdit, "Conta Caixa", 24, true, strCamposBaseDados: colCaixa);
            priGrelha1.AddColKey(colBanco, FpCellType.CellTypeEdit, "Modo De Pagamento", 24, true, strCamposBaseDados: colBanco);
            priGrelha1.AddColKey(colConta, FpCellType.CellTypeEdit, "Conta", 15, true, true, strCamposBaseDados: colConta);
            priGrelha1.AddColKey(colComissoes, FpCellType.CellTypeCurrency, "Valor Apurado Bruto", 10, false, true, strCamposBaseDados: colComissoes, blnColunaTotalizador: true);
            
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
            StdBELista listaConta = new StdBELista();
            string queryConta = "";
            StringBuilder query = new StringBuilder();
            StdBECamposChave campoChave = new StdBECamposChave();
            query.AppendLine(string.Format("SELECT distinct {0}", priGrelha1.DaCamposBDSelect()));
            query.AppendLine(" from (select conta from contasbancarias where TipoConta = 4) as x, " +
                            " (select LinhasTesouraria.Movim, TDU_CaixasVsRDT.CDU_Caixas, sum(CabecTesouraria.TotalCredito) - sum(CabecTesouraria.TotalDebito) as Total from CabecTesouraria " +
                            " inner join LinhasTesouraria on LinhasTesouraria.IdCabecTesouraria = CabecTesouraria.Id " +
                            " inner join cabecdoc on cabecdoc.Id = CabecTesouraria.IdDocOriginal inner join TDU_CaixasVsRDT on cabecdoc.TipoDoc = TDU_CaixasVsRDT.CDU_Documento " +
                            " where TDU_CaixasVsRDT.CDU_ResumoTesouraria = '" + resumoTesouraria + "' and DtValor >= '" + dataReferencia + " 00:00:00.000' and DtValor <= '" + dataReferencia + " 23:59:59.000' " +
                            " group by LinhasTesouraria.Movim, TDU_CaixasVsRDT.CDU_Caixas) as y order by y.CDU_Caixas");
            lista = new StdBELista();
            lista = PriSDKContext.SdkContext.BSO.Consulta(query.ToString());
            priGrelha1.DataBind(lista);
            for (int i = 1; i <= priGrelha1.Grelha.DataRowCnt; i++)
            {
                if (string.IsNullOrEmpty(priGrelha1.GetGRID_GetValorCelula(i, colCaixa)) == false)
                {
                    campoChave = new StdBECamposChave();
                    campoChave.AddCampoChave("CDU_MovimentosBancarios", priGrelha1.GetGRID_GetValorCelula(i, colBanco));
                    listaConta = new StdBELista();
                    queryConta = "select CDU_" + BSO.TabelasUtilizador.DaValorAtributo("TDU_MovimentosBancarios",campoChave,"CDU_NewHotelMovimento") + " from TDU_NewHotelErpPrimavera inner join " +
                                 " TDU_CaixasVsRDT on CDU_DocPrimavera = CDU_Documento and CDU_Caixas = '" + priGrelha1.GetGRID_GetValorCelula(i, colCaixa) + "' " +
                                 "inner join TDU_MovimentosBancarios on CDU_MovimentosBancarios = '" + priGrelha1.GetGRID_GetValorCelula(i, colBanco) + "' ";
                    listaConta = BSO.Consulta(queryConta);
                    priGrelha1.SetGRID_SetValorCelula(i, colConta, listaConta.Valor(0));
                }

            }


        }

        private void Sair_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
