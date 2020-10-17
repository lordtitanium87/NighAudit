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
using TesBE100;
using System.Linq.Expressions;
using System.Threading;
using System.Data.SqlClient;

namespace NightAudit
{
    public partial class frm_NightAudit : CustomForm
    {
        public static StdBSInterfPub Plataforma;
        public static ErpBS MotorLE = new ErpBS();
        private const string GridVersion = "01.00";
        private const string GridName = "GridDemo";
        private const string colEntidade = "tdu_caixasvsrdt.Cdu_caixas as Caixa";
        string colData = "'' as 'Data de Deposito'";
        private const string colNome = "sum(CabecTesouraria.TotalCredito)-sum(CabecTesouraria.TotalDebito) as TotApurado";
        private const string colTipoDoc = "TDU_MovimentosBancarios.CDU_tipoMovimento";
        private const string colTotApuradoNewHotel = "sum(CabecTesouraria.TotalCredito) as 'Total Apurado NewHotel'";
        //private const string colNome = "sum(cabecdoc.TotalDocumento) as TotApurado";
        //private const string colTipoDoc = "TDU_MovimentosBancarios.CDU_tipoMovimento";
        //private const string colTotApuradoNewHotel = "sum(cabecdoc.TotalDocumento) as 'Total Apurado NewHotel'";
        // Hidden column
        //            SELECT tdu_caixasvsrdt.Cdu_caixas as Caixa,'' as 'Data de Deposito',sum(CabecTesouraria.TotalCredito) as TotApurado,
        //sum(CabecTesouraria.TotalCredito) as TotApurado,TDU_MovimentosBancarios.CDU_tipoMovimento,sum(CabecTesouraria.TotalCredito) as 'Total Apurado NewHotel'
        private const string colModulo = "Modulo";
        private const string colTipoEntidade = "TipoEntidade";
        private const string colFilial = "Filial";
        private bool controlsInitialized;
        private string categoriaEntidade = "mntTabClientes";
        private string categoriaEntidadesExternas = "mntTabEntidadesExternas";
        public static string constString;
        public static string connectionString;
        public static decimal comissoes;
        public static bool templateOpened = false;

        public frm_NightAudit()
        {
            InitializeComponent();
        }

        private void frm_NightAudit_Load(object sender, EventArgs e)
        {
            PriSDKContext.Initialize(BSO, PSO);
            InitializeSDKControls();
            InitializeGrid();
            f4TabelaSQL1.Caption = "Documento: ";
            f4TabelaSQL2.Caption = "Conta Destino: ";
            Empresa.Text = "Empresa: " + BSO.Contexto.CodEmp;
            User.Text = "Utilizador: " + BSO.Contexto.UtilizadorActual;
            pictureBox1.Image = StdBrandingInfo100.Properties.RibbonResourcesVND.descontos_precos_16;
            Imprimir.Image = StdBrandingInfo100.Properties.RibbonResourcesVND.imprimir_16;
            btnNovo.Image = StdBrandingInfo100.Properties.RibbonResourcesVND.novo_16;

        }
        private void InitializeSDKControls()
        {
            //Initializes controls
            if (!controlsInitialized)
            {
                // Initialize the controls with the SDK context  
                f4TabelaSQL1.Inicializa(PriSDKContext.SdkContext);
                priGrelha1.Inicializa(PriSDKContext.SdkContext);
                f4TabelaSQL2.Inicializa(PriSDKContext.SdkContext);
                controlsInitialized = true;
            }
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
            StdBELista modosDeRecebimento = new StdBELista();
            string ConsultaModos = "";
            string textoModos = "";
            double grandTotal = 0;
            StringBuilder query = new StringBuilder();
            query.AppendLine(string.Format("SELECT {0}", priGrelha1.DaCamposBDSelect()));
            query.AppendLine("from cabecdoc inner join tdu_caixasvsrdt on cabecdoc.TipoDoc = tdu_caixasvsrdt.cdu_documento ");
            query.AppendLine(" inner join CabecTesouraria on CabecTesouraria.IdDocOriginal = cabecdoc.Id");
            query.AppendLine(" inner join LinhasTesouraria on CabecTesouraria.Id = LinhasTesouraria.IdCabecTesouraria");
            query.AppendLine(" inner join TDU_MovimentosBancarios on TDU_MovimentosBancarios.CDU_MovimentosBancarios = LinhasTesouraria.Movim");
            query.AppendLine("where cabecdoc.data >= '" + stdDatePicker1.Text + " 00:00:00.000' and cabecdoc.data <= '" + stdDatePicker1.Text + " 23:59:59.000'");
            query.AppendLine("group by tdu_caixasvsrdt.cdu_documento,tdu_caixasvsrdt.Cdu_caixas,TDU_MovimentosBancarios.cdu_TipoMovimento");
            query.AppendLine(" order by TDU_CaixasVsRDT.CDU_Caixas ");
            //query.AppendLine(string.Format("SELECT {0}", priGrelha1.DaCamposBDSelect()));
            //query.AppendLine("from cabecdoc inner join tdu_caixasvsrdt on cabecdoc.TipoDoc  = tdu_caixasvsrdt.cdu_documento ");
            //query.AppendLine(" inner join TDU_MovimentosBancarios on TDU_MovimentosBancarios.CDU_MovimentosBancarios = cabecdoc.ModoPag");
            //query.AppendLine("where data >= '" + stdDatePicker1.Text + " 00:00:00.000' and data <= '" + stdDatePicker1.Text + " 23:59:59.000'");
            //query.AppendLine("group by tdu_caixasvsrdt.cdu_documento,tdu_caixasvsrdt.Cdu_caixas,TDU_MovimentosBancarios.cdu_TipoMovimento");
            //query.AppendLine(" order by TDU_CaixasVsRDT.CDU_Caixas ");
            lista = new StdBELista();
            lista = PriSDKContext.SdkContext.BSO.Consulta(query.ToString());
            priGrelha1.DataBind(lista);

            //            SELECT tdu_caixasvsrdt.Cdu_caixas as Caixa,'' as 'Data de Deposito',sum(CabecTesouraria.TotalCredito) as TotApurado,
            //sum(CabecTesouraria.TotalCredito) as TotApurado,TDU_MovimentosBancarios.CDU_tipoMovimento,sum(CabecTesouraria.TotalCredito) as 'Total Apurado NewHotel'
            //from cabecdoc inner join tdu_caixasvsrdt on cabecdoc.TipoDoc = tdu_caixasvsrdt.cdu_documento
            //inner join CabecTesouraria on CabecTesouraria.IdDocOriginal = cabecdoc.Id
            //inner join LinhasTesouraria on CabecTesouraria.Id = LinhasTesouraria.IdCabecTesouraria

            //inner join TDU_MovimentosBancarios on TDU_MovimentosBancarios.CDU_MovimentosBancarios = LinhasTesouraria.Movim
            // where cabecdoc.Data >= '" + stdDatePicker1.Text + " 00:00:00.000' and cabecdoc.Data <= '" + stdDatePicker1.Text + " 23:59:59.000'
            // group by tdu_caixasvsrdt.cdu_documento,tdu_caixasvsrdt.Cdu_caixas,TDU_MovimentosBancarios.cdu_TipoMovimento
            // order by TDU_CaixasVsRDT.CDU_Caixas

            for (int i = 1; i <= priGrelha1.Grelha.DataRowCnt; i++)
            {
                //if (Convert.ToString(priGrelha1.GetGRID_GetValorCelula(i, colEntidade)) != "")
                //{
                //    priGrelha1.SetGRID_SetValorCelula(i, colTotApuradoNewHotel, 100.00);
                //    //MessageBox.Show(priGrelha1.GetGRID_GetValorCelula(i, colEntidade));
                //}
                modosDeRecebimento = new StdBELista();
                ConsultaModos = "  SELECT distinct y.CDU_Caixas,y.Movim from (select conta from contasbancarias where TipoConta = 4) as x, " +
                                " (select LinhasTesouraria.movim, TDU_CaixasVsRDT.CDU_Caixas, sum(LinhasTesouraria.Credito)-sum(LinhasTesouraria.Debito) as Total from LinhasTesouraria " +
                                " inner join CabecDoc on CabecDoc.Id = LinhasTesouraria.IdDocOriginal inner join TDU_CaixasVsRDT on CabecDoc.TipoDoc = TDU_CaixasVsRDT.CDU_Documento " +
                                " where TDU_CaixasVsRDT.CDU_ResumoTesouraria = '" + f4TabelaSQL1.Text + "' and LinhasTesouraria.DtValor >= '" + stdDatePicker1.Text + " 00:00:00.000' " +
                                " and LinhasTesouraria.DtValor <= '" + stdDatePicker1.Text + " 23:59:59.000' group by LinhasTesouraria.Movim, TDU_CaixasVsRDT.CDU_Caixas) as y " +
                                " inner join TDU_MovimentosBancarios on CDU_MovimentosBancarios = y.Movim where y.CDU_Caixas = '" + priGrelha1.GetGRID_GetValorCelula(i, colEntidade) + "' " +
                                " and CDU_TipoMovimento = 'E' order by y.CDU_Caixas";
                modosDeRecebimento = BSO.Consulta(ConsultaModos);
                if (priGrelha1.GetGRID_GetValorCelula(i, colTipoDoc) == "E")
                {
                    for (int x = 0; x <= modosDeRecebimento.NumLinhas() - 1; x++)
                    {
                        textoModos = textoModos + "/" + modosDeRecebimento.Valor(1);
                        modosDeRecebimento.Seguinte();
                    }
                    textoModos = textoModos.Remove(0, 1);
                    priGrelha1.SetGRID_SetValorCelula(i, colTipoDoc, textoModos);
                    //priGrelha1.SetGRID_SetValorCelula(i, colTipoDoc, "MB/CC/TRF");
                }
                if (priGrelha1.GetGRID_GetValorCelula(i, colTipoDoc) == "N")
                {
                    priGrelha1.SetGRID_SetValorCelula(i, colTipoDoc, "NUM");
                }
            }
            
            for (int i = 1; i <= priGrelha1.Grelha.DataRowCnt; i++)
            {
                //colNome
                if (string.IsNullOrEmpty(priGrelha1.GetGRID_GetValorCelula(i, colTipoDoc)) == false)
                {
                    
                    grandTotal = grandTotal + priGrelha1.GetGRID_GetValorCelula(i, colNome);
                }

            } 
            priGrelha1.SetGRID_SetValorCelula(priGrelha1.Grelha.DataRowCnt+2, colNome, grandTotal);
            priGrelha1.SetGRID_SetValorCelula(priGrelha1.Grelha.DataRowCnt, colTotApuradoNewHotel, grandTotal);
            priGrelha1.SetGRID_SetValorCelula(priGrelha1.Grelha.DataRowCnt, colEntidade, "Total");
             
        }
        private void InitializeGrid()
        {
            string substituiColData = "'" + stdDatePicker1.Text + "' as 'Data de Deposito'";
            colData = substituiColData;
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
            priGrelha1.AddColKey(colEntidade, FpCellType.CellTypeEdit, "Caixa", 17, true, strCamposBaseDados: colEntidade, blnDrillDown: true);
            priGrelha1.AddColKey(colTipoDoc, FpCellType.CellTypeEdit, "Modo de Recebimento", 17, true, strCamposBaseDados: colTipoDoc);
            priGrelha1.AddColKey(colNome, FpCellType.CellTypeCurrency, "Total Apurado Primavera", 17, true, strCamposBaseDados: colNome, blnColunaTotalizador: true);
            priGrelha1.AddColKey(colTotApuradoNewHotel, FpCellType.CellTypeCurrency, "Total Apurado NewHotel", 17, true, strCamposBaseDados: colTotApuradoNewHotel, blnColunaTotalizador: true);
            priGrelha1.AddColKey(colData, FpCellType.CellTypeDate, "Data de Deposito", 17, false, true, strCamposBaseDados: colData);
            //Default grouping
            priGrelha1.AdicionaAgrupamento(priGrelha1.Cols.GetEdita(colEntidade).Number);
            //pa riGrelha1.AdicionaAgrupamento(priGrelha1.Cols.GetEdita(colModuloDesc).Number);
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
            priGrelha1.Grelha.AutoCalc = true;

            // Read the last grid layout for the current user.
            if (!priGrelha1.LeXML("GRIDDEMO", BSO.Contexto.UtilizadorActual, GridName, GridName, GridVersion))
            {
                priGrelha1.FormataGrelha(true);
            }
            priGrelha1.LimpaGrelha();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void priGrelha1_DataFill(object Sender, PriGrelha.DataFillEventArgs e)
        {
            //SetModuloDesc(e.Row); 
        }
        private void SetModuloDesc(int row)
        {
            string modulo = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colModulo));
            string moduloDesc = modulo;

            switch (modulo)
            {
                case "V":
                    moduloDesc = "Vendas";
                    break;
                case "C":
                    moduloDesc = "Compras";
                    break;
                case "M":
                    moduloDesc = "C/Corentes";
                    break;
                default:
                    break;
            }

            // priGrelha1.SetGRID_SetValorCelula(row, colModuloDesc, moduloDesc);
        }

        private void priGrelha1_FormatacaoAlterada(object Sender, PriGrelha.FormatacaoAlteradaEventArgs e)
        {
            priGrelha1.LimpaGrelha();
        }

        private void priGrelha1_MenuContextoSeleccionado(object Sender, PriGrelha.MenuContextoSeleccionadoEventArgs e)
        {
            switch (e.Comando.ToUpper())
            {
                case "MNUSTDDRILLDOWN":
                    ExecuteDrillDown();
                    break;
                case "MNUCRIAENTIDADE":
                    PSO.Dialogos.MostraMensagem(
                            StdPlatBS100.StdBSTipos.TipoMsg.PRI_SimplesOk, "Create e new entity");
                    break;
                case "MNUEDITARENTIDADE":
                    PSO.Dialogos.MostraMensagem(
                        StdPlatBS100.StdBSTipos.TipoMsg.PRI_SimplesOk,
                        PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(priGrelha1.Grelha.ActiveRow, colEntidade)));
                    break;
                default:
                    break;
            }
        }

        private void ExecuteDrillDown()
        {

            try
            {
                using (var result = BSO.Extensibility.CreateCustomFormInstance(typeof(DetalheModoRecebimento)))
                {
                    if (result.IsSuccess())
                    {
                        DetalheModoRecebimento.Plataforma = PSO;
                        DetalheModoRecebimento.MotorLE = BSO;
                        DetalheModoRecebimento.resumoTesouraria = f4TabelaSQL1.Text;
                        DetalheModoRecebimento.dataReferencia = stdDatePicker1.Text;
                        string conString = PSO.BaseDados.DaConnectionStringNET(PSO.BaseDados.DaNomeBDdaEmpresa(BSO.Contexto.CodEmp), "DEFAULT");
                        DetalheModoRecebimento.connectionString = conString;
                        (result.Result as DetalheModoRecebimento).ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //int row = priGrelha1.Grelha.ActiveRowIndex;
            //int col = priGrelha1.Grelha.ActiveColumnIndex;

            //if (priGrelha1.Cols.GetEditaCol(col).ColKey == colEntidade)
            //{
            //    string entidade = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colEntidade));

            //    DrillDownManager.DrillDownEntidade(PSO, categoriaEntidade, entidade);

            //    return;
            //}

            //if (priGrelha1.Cols.GetEditaCol(col).ColKey == colNumDocInt)
            //{
            //    string modulo = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colModulo));
            //    string tipodoc = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colTipoDoc));
            //    string serie = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colSerie));
            //    int numdoc = PSO.Utils.FInt(priGrelha1.GetGRID_GetValorCelula(row, colNumDocInt));
            //    string filial = PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(row, colFilial));

            //    DrillDownManager.DrillDownDocumento(PSO, modulo, tipodoc, numdoc, serie, filial);

            //    return;
            //}
        }

        private void Converter_Click(object sender, EventArgs e)
        {
            string Avisos = "";
            string consultaDetalhes = "";
            string consultaModoPagamento;
            double comissoesApuradas = 0;
            StdBELista dataPermitida = new StdBELista();
            string consultaData;
            StdBECamposChave codigoComissao = new StdBECamposChave();
            StdBECamposChave codigoResumoDiario = new StdBECamposChave();
            StdBECamposChave tipoDeDocumento = new StdBECamposChave();
            StdBECamposChave ContaDestino = new StdBECamposChave();
            List<string> listaCaixas = new List<string>();
            StdBELista listaDetalhes = new StdBELista();
            StdBELista listaCDU = new StdBELista();
            //StdBELista contaDestino = new StdBELista();
            TesBEDocumentoTesouraria novoDocumentoTesouraria = new TesBEDocumentoTesouraria();
            TesBEDocumentoTesouraria DocumentoTesouraria = new TesBEDocumentoTesouraria();
            TesBEDocumentoTesouraria documentoGravado = new TesBEDocumentoTesouraria();
            TesBEMovimentoBancario novoMovimento = new TesBEMovimentoBancario();
            tipoDeDocumento.AddCampoChave("CDU_Codigo", f4TabelaSQL1.Text);

            try
            {
                if (string.IsNullOrEmpty(f4TabelaSQL2.Text) == true)
                {
                    PSO.Dialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, "A conta destino tem que estar preenchida", StdBSTipos.IconId.PRI_Critico);
                    return;
                }
                consultaData = "select max(Data) from CabecTesouraria where CDU_TipoDoc = '" + f4TabelaSQL1.Text + "' and CDU_Serie = '" + comboBox1.Text + "'";
                dataPermitida = BSO.Consulta(consultaData);
                if (stdDatePicker1.Value < Convert.ToDateTime(dataPermitida.Valor(0)))
                {
                    PSO.Dialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, "A data colocada é inferior ao permitida", StdBSTipos.IconId.PRI_Critico);
                    stdDatePicker1.Value = Convert.ToDateTime(dataPermitida.Valor(0)).AddDays(1);
                    stdDatePicker2.Value = Convert.ToDateTime(dataPermitida.Valor(0)).AddDays(1);
                    return;
                }
                for (int i = 1; i <= priGrelha1.Grelha.DataRowCnt; i++)
                {
                    if (string.IsNullOrEmpty(PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(i, colEntidade))) == false)
                    {
                        listaCaixas.Add(PSO.Utils.FStr(priGrelha1.GetGRID_GetValorCelula(i, colEntidade)));
                    }

                }
                List<string> distinct = listaCaixas.Distinct().ToList();
                for (int x = 0; x <= distinct.Count - 1; x++)
                {
                    criaDocumentoTesourariaMovimento(distinct[x]);

                }
                criaDocumentoTesourariaDeposito();
                //for (int x = 0; x <= distinct.Count - 1; x++)
                //{
                //    StdBEExecSql comandoSql = new StdBEExecSql();
                //    comandoSql.Tabela = "TDU_DetalheComissoes";
                //    comandoSql.AddCampo("CDU_caixaBanco", distinct[x], true);
                //    comandoSql.tpQuery = StdBETipos.EnumTpQuery.tpDELETE;
                //    BSO.DSO.Plat.ExecSql.Executa(comandoSql);
                //}
                PSO.Dialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, "Processo de fecho finalizado com sucesso", StdBSTipos.IconId.PRI_Informativo);
                criarNovoFecho();
                Atualizar_Click(sender, e);
            }
            catch (Exception ex)
            {
                PSO.Dialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, ex.Message, StdBSTipos.IconId.PRI_Informativo);
            }
        }

        private void criaDocumentoTesourariaDeposito()
        {
            string Avisos = "";
            string consultaDetalhes = "";
            string consultaModoPagamento;
            double valorTotal = 0;
            StdBECamposChave codigoComissao = new StdBECamposChave();
            StdBECamposChave codigoResumoDiario = new StdBECamposChave();
            StdBECamposChave tipoDeDocumento = new StdBECamposChave();
            StdBECamposChave ContaDestino = new StdBECamposChave();
            List<string> listaCaixas = new List<string>();
            StdBELista listaDetalhes = new StdBELista();
            StdBELista listaCDU = new StdBELista();
            StdBELista contaDestino = new StdBELista();
            TesBEDocumentoTesouraria novoDocumentoTesouraria = new TesBEDocumentoTesouraria();
            TesBEDocumentoTesouraria DocumentoTesouraria = new TesBEDocumentoTesouraria();
            TesBEDocumentoTesouraria documentoGravado = new TesBEDocumentoTesouraria();
            TesBEMovimentoBancario novoMovimento = new TesBEMovimentoBancario();
            tipoDeDocumento.AddCampoChave("CDU_Codigo", f4TabelaSQL1.Text);

            codigoResumoDiario = new StdBECamposChave();
            novoDocumentoTesouraria = new TesBEDocumentoTesouraria();
            novoDocumentoTesouraria.Tipodoc = BSO.TabelasUtilizador.DaValorAtributo("TDU_ResumoDiario", tipoDeDocumento, "CDU_DocumentoDeposito").ToString();
            novoDocumentoTesouraria.Serie = comboBox1.Text;
            novoDocumentoTesouraria.ContaDestino = f4TabelaSQL2.Text;
            novoDocumentoTesouraria.Data = stdDatePicker1.Value;
            novoDocumentoTesouraria.DataIntroducao = stdDatePicker1.Value;
            novoDocumentoTesouraria.ModuloOrigem = "B";
            novoDocumentoTesouraria.CamposUtil["CDU_TipoDoc"].Valor = f4TabelaSQL1.Text;
            novoDocumentoTesouraria.CamposUtil["CDU_Serie"].Valor = comboBox1.Text;
            novoDocumentoTesouraria.CamposUtil["CDU_NumDoc"].Valor = numericUpDown1.Value;
            novoDocumentoTesouraria.CamposUtil["CDU_DataFecho"].Valor = stdDatePicker1.Text;
            novoDocumentoTesouraria.CamposUtil["CDU_DataDoc"].Valor = stdDatePicker2.Text;
            novoDocumentoTesouraria.CamposUtil["CDU_ComissoesApuradas"].Valor = numericUpDown2.Value;
            novoDocumentoTesouraria.CamposUtil["CDU_ContaDestino"].Valor = f4TabelaSQL2.Text;
            codigoResumoDiario.AddCampoChave("CDU_Codigo", f4TabelaSQL1.Text);
            consultaDetalhes = "SELECT distinct y.CDU_Caixas as Caixa,'CDU_' + TDU_MovimentosBancarios.CDU_NewHotelMovimento,y.Total,TDU_MovimentosBancarios.CDU_TipoMovimento " +
                               " from(select conta from contasbancarias where TipoConta = 4) as x, (select LinhasTesouraria.Movim, TDU_CaixasVsRDT.CDU_Caixas, " +
                               " sum(LinhasTesouraria.Credito) - sum(LinhasTesouraria.Debito) as Total from cabecdoc inner join LinhasTesouraria on LinhasTesouraria.IdDocOriginal = cabecdoc.Id " +
                               " inner join TDU_CaixasVsRDT on cabecdoc.TipoDoc = TDU_CaixasVsRDT.CDU_Documento where TDU_CaixasVsRDT.CDU_ResumoTesouraria = '" + f4TabelaSQL1.Text + "' " +
                               " and cabecdoc.Data >= '" + stdDatePicker1.Text + " 00:00:00.000' and data <= '" + stdDatePicker1.Text + " 23:59:59.000' " +
                               " group by LinhasTesouraria.Movim, TDU_CaixasVsRDT.CDU_Caixas) as y " +
                               " inner join TDU_MovimentosBancarios on TDU_MovimentosBancarios.CDU_MovimentosBancarios = y.Movim where CDU_TipoMovimento = 'N' order by y.CDU_Caixas";
            listaDetalhes = BSO.Consulta(consultaDetalhes);

            if (listaDetalhes.Vazia() == false)
            {
                for (int linhasDetalhes = 0; linhasDetalhes <= listaDetalhes.NumLinhas() - 1; linhasDetalhes++)
                {
                    BSO.Tesouraria.Documentos.AdicionaLinha(novoDocumentoTesouraria, BSO.TabelasUtilizador.DaValorAtributo("TDU_ResumoDiario", codigoResumoDiario, "CDU_Debito").ToString(), listaDetalhes.Valor(0), "EUR", listaDetalhes.Valor(2));
                    listaDetalhes.Seguinte();
                }
                listaDetalhes.Inicio();
                for (int linhasCredito = 0; linhasCredito <= listaDetalhes.NumLinhas() - 1; linhasCredito++)
                {
                    valorTotal = valorTotal + listaDetalhes.Valor(2);
                    listaDetalhes.Seguinte();
                }
                BSO.Tesouraria.Documentos.AdicionaLinha(novoDocumentoTesouraria, BSO.TabelasUtilizador.DaValorAtributo("TDU_ResumoDiario", codigoResumoDiario, "CDU_Credito").ToString(),
                    f4TabelaSQL2.Text, "EUR", valorTotal);
                for (int linhas = 1; linhas <= novoDocumentoTesouraria.Linhas.NumItens; linhas++)
                {
                    novoDocumentoTesouraria.Linhas.GetEdita(linhas).Cambio = 1;
                    novoDocumentoTesouraria.Linhas.GetEdita(linhas).CambioMBase = 1;
                    novoDocumentoTesouraria.Linhas.GetEdita(linhas).CambioMAlt = 200.4820000;
                }
                BSO.Tesouraria.Documentos.ValidaActualizacao(novoDocumentoTesouraria, ref Avisos);
                if (string.IsNullOrEmpty(Avisos) == false)
                {
                    novoDocumentoTesouraria.ValorMovimento = novoDocumentoTesouraria.TotalDebito - novoDocumentoTesouraria.TotalCredito;
                    BSO.Tesouraria.Documentos.Actualiza(novoDocumentoTesouraria);
                    //documentoGravado = BSO.Tesouraria.Documentos.Edita(novoDocumentoTesouraria.Filial, novoDocumentoTesouraria.Tipodoc, novoDocumentoTesouraria.Serie, novoDocumentoTesouraria.NumDoc);
                    //lancaMovimentoContabilidade(documentoGravado);    
                }
            }

        }

        private void criarNovoFecho()
        {
            StdBELista NumeroMaximo = new StdBELista();
            StdBECamposChave campochave = new StdBECamposChave();
            string ConsultaNumeroMaximo;
            campochave.AddCampoChave("CDU_Codigo", f4TabelaSQL1.Text);
            ConsultaNumeroMaximo = "select max(CabecTesouraria.cdu_numDoc),max(numdoc),max(tipodoc),CASE WHEN  max(TDU_DocumentosFechoAnulado.cdu_numDoc) IS NULL THEN 0 ELSE max(TDU_DocumentosFechoAnulado.cdu_numDoc) END from CabecTesouraria " +
                                   " left outer join TDU_DocumentosFechoAnulado on TDU_DocumentosFechoAnulado.CDU_TipoDoc = '" + BSO.TabelasUtilizador.DaValorAtributo("TDU_ResumoDiario", campochave, "CDU_DocumentoTesouraria") + "' and " +
                                   " TDU_DocumentosFechoAnulado.CDU_Serie = '" + comboBox1.Text + "' " +
                                   " where CabecTesouraria.CDU_TipoDoc = '" + f4TabelaSQL1.Text + "' and CabecTesouraria.CDU_Serie = '" + comboBox1.Text + "'";
            NumeroMaximo = BSO.Consulta(ConsultaNumeroMaximo);
            if (string.IsNullOrEmpty(Convert.ToString(NumeroMaximo.Valor(0))) == false)
            {
                if (string.IsNullOrEmpty(Convert.ToString(NumeroMaximo.Valor(3))) == true)
                {
                    numericUpDown1.Maximum = Convert.ToInt32(NumeroMaximo.Valor(0)) + 1;
                    numericUpDown1.Value = Convert.ToInt32(NumeroMaximo.Valor(0)) + 1;
                    stdDatePicker1.Value = Convert.ToDateTime(BSO.Tesouraria.Documentos.DaValorAtributo("000", NumeroMaximo.Valor(2), comboBox1.Text, NumeroMaximo.Valor(1), "Data")).AddDays(1);
                    stdDatePicker2.Value = Convert.ToDateTime(BSO.Tesouraria.Documentos.DaValorAtributo("000", NumeroMaximo.Valor(2), comboBox1.Text, NumeroMaximo.Valor(1), "Data")).AddDays(1);
                }
                else if (NumeroMaximo.Valor(0) > NumeroMaximo.Valor(3))
                {
                    numericUpDown1.Maximum = Convert.ToInt32(NumeroMaximo.Valor(0)) + 1;
                    numericUpDown1.Value = Convert.ToInt32(NumeroMaximo.Valor(0)) + 1;
                    stdDatePicker1.Value = Convert.ToDateTime(BSO.Tesouraria.Documentos.DaValorAtributo("000", NumeroMaximo.Valor(2), comboBox1.Text, NumeroMaximo.Valor(1), "Data")).AddDays(1);
                    stdDatePicker2.Value = Convert.ToDateTime(BSO.Tesouraria.Documentos.DaValorAtributo("000", NumeroMaximo.Valor(2), comboBox1.Text, NumeroMaximo.Valor(1), "Data")).AddDays(1);
                }
                else
                {
                    numericUpDown1.Maximum = Convert.ToInt32(NumeroMaximo.Valor(3)) + 1;
                    numericUpDown1.Value = Convert.ToInt32(NumeroMaximo.Valor(3)) + 1;
                    stdDatePicker1.Value = Convert.ToDateTime(BSO.Tesouraria.Documentos.DaValorAtributo("000", NumeroMaximo.Valor(2), comboBox1.Text, NumeroMaximo.Valor(1), "Data")).AddDays(1);
                    stdDatePicker2.Value = Convert.ToDateTime(BSO.Tesouraria.Documentos.DaValorAtributo("000", NumeroMaximo.Valor(2), comboBox1.Text, NumeroMaximo.Valor(1), "Data")).AddDays(1);
                }
            }
            else
            {
                numericUpDown1.Maximum = 1;
                numericUpDown1.Value = 1;
            }
        }

        private void criaDocumentoTesourariaMovimento(string caixa)
        {
            string Avisos = "";
            string consultaDetalhes = "";
            string consultaModoPagamento;
            string consultaContaDestino = "";
            double comissoesApuradas = 0;
            double valorTotalEletronico = 0;
            StdBECamposChave codigoComissao = new StdBECamposChave();
            StdBECamposChave codigoResumoDiario = new StdBECamposChave();
            StdBECamposChave tipoDeDocumento = new StdBECamposChave();
            StdBECamposChave ContaDestino = new StdBECamposChave();
            List<string> listaCaixas = new List<string>();
            StdBELista listaDetalhes = new StdBELista();
            StdBELista listaCDU = new StdBELista();
            StdBELista ListacontaDestino = new StdBELista();
            TesBEDocumentoTesouraria novoDocumentoTesouraria = new TesBEDocumentoTesouraria();
            TesBEDocumentoTesouraria DocumentoTesouraria = new TesBEDocumentoTesouraria();
            TesBEDocumentoTesouraria documentoGravado = new TesBEDocumentoTesouraria();
            TesBEMovimentoBancario novoMovimento = new TesBEMovimentoBancario();
            tipoDeDocumento.AddCampoChave("CDU_Codigo", f4TabelaSQL1.Text);

            codigoResumoDiario = new StdBECamposChave();
            novoDocumentoTesouraria = new TesBEDocumentoTesouraria();
            novoDocumentoTesouraria.Tipodoc = BSO.TabelasUtilizador.DaValorAtributo("TDU_ResumoDiario", tipoDeDocumento, "CDU_DocumentoTesouraria").ToString();
            novoDocumentoTesouraria.Serie = comboBox1.Text;
            novoDocumentoTesouraria.ContaDestino = f4TabelaSQL2.Text;
            novoDocumentoTesouraria.Data = stdDatePicker1.Value;
            novoDocumentoTesouraria.DataIntroducao = stdDatePicker1.Value;
            novoDocumentoTesouraria.ModuloOrigem = "B";
            novoDocumentoTesouraria.CamposUtil["CDU_TipoDoc"].Valor = f4TabelaSQL1.Text;
            novoDocumentoTesouraria.CamposUtil["CDU_Serie"].Valor = comboBox1.Text;
            novoDocumentoTesouraria.CamposUtil["CDU_NumDoc"].Valor = numericUpDown1.Value;
            novoDocumentoTesouraria.CamposUtil["CDU_DataFecho"].Valor = stdDatePicker1.Text;
            novoDocumentoTesouraria.CamposUtil["CDU_DataDoc"].Valor = stdDatePicker2.Text;
            novoDocumentoTesouraria.CamposUtil["CDU_ComissoesApuradas"].Valor = numericUpDown2.Value;
            novoDocumentoTesouraria.CamposUtil["CDU_ContaDestino"].Valor = f4TabelaSQL2.Text;
            codigoResumoDiario.AddCampoChave("CDU_Codigo", f4TabelaSQL1.Text);
            consultaDetalhes = "SELECT distinct y.CDU_Caixas as Caixa,'CDU_' + TDU_MovimentosBancarios.CDU_NewHotelMovimento,y.Total,TDU_MovimentosBancarios.CDU_TipoMovimento " +
                               " from(select conta from contasbancarias where TipoConta = 4) as x, (select LinhasTesouraria.Movim, TDU_CaixasVsRDT.CDU_Caixas, " +
                               " sum(LinhasTesouraria.Credito) - sum(LinhasTesouraria.Debito) as Total from cabecdoc inner join LinhasTesouraria on LinhasTesouraria.IdDocOriginal = cabecdoc.Id " +
                               " inner join TDU_CaixasVsRDT on cabecdoc.TipoDoc = TDU_CaixasVsRDT.CDU_Documento where TDU_CaixasVsRDT.CDU_ResumoTesouraria = '" + f4TabelaSQL1.Text + "' " +
                               " and cabecdoc.Data >= '" + stdDatePicker1.Text + " 00:00:00.000' and data <= '" + stdDatePicker1.Text + " 23:59:59.000' " +
                               " group by LinhasTesouraria.Movim, TDU_CaixasVsRDT.CDU_Caixas) as y " +
                               " inner join TDU_MovimentosBancarios on TDU_MovimentosBancarios.CDU_MovimentosBancarios = y.Movim where CDU_TipoMovimento = 'E' order by y.CDU_Caixas";
            listaDetalhes = BSO.Consulta(consultaDetalhes);
            if (listaDetalhes.Vazia() == false)
            {
                consultaContaDestino = "select " + listaDetalhes.Valor(1) + " from TDU_NewHotelErpPrimavera inner join TDU_CaixasVsRDT on TDU_CaixasVsRDT.CDU_Documento = TDU_NewHotelErpPrimavera.CDU_DocPrimavera " +
                  " where CDU_Caixas = '" + caixa + "' and CDU_ResumoTesouraria = '" + f4TabelaSQL1.Text + "'";
                ListacontaDestino = BSO.Consulta(consultaContaDestino);
                for (int valorEletronico = 0; valorEletronico <= listaDetalhes.NumLinhas() - 1; valorEletronico++)
                {
                    valorTotalEletronico = valorTotalEletronico + listaDetalhes.Valor(2);
                    listaDetalhes.Seguinte();
                }
                BSO.Tesouraria.Documentos.AdicionaLinha(novoDocumentoTesouraria, BSO.TabelasUtilizador.DaValorAtributo("TDU_ResumoDiario", codigoResumoDiario, "CDU_Debito").ToString(), caixa, "EUR", valorTotalEletronico);

                listaDetalhes.Inicio();
                for (int linhasCredito = 0; linhasCredito <= listaDetalhes.NumLinhas() - 1; linhasCredito++)
                {
                    codigoComissao = new StdBECamposChave();
                    ListacontaDestino = BSO.Consulta("select " + listaDetalhes.Valor(1) + " from TDU_NewHotelErpPrimavera inner join " +
                  " TDU_CaixasVsRDT on TDU_CaixasVsRDT.CDU_Documento = TDU_NewHotelErpPrimavera.CDU_DocPrimavera " +
                  " where CDU_Caixas = '" + caixa + "' and CDU_ResumoTesouraria = '" + f4TabelaSQL1.Text + "'");
                    codigoComissao.AddCampoChave("CDU_caixaBanco", caixa + ListacontaDestino.Valor(0));
                    comissoesApuradas = PSO.Utils.FDbl(BSO.TabelasUtilizador.DaValorAtributo("TDU_DetalheComissoes", codigoComissao, "CDU_Valor"));
                    BSO.Tesouraria.Documentos.AdicionaLinha(novoDocumentoTesouraria, BSO.TabelasUtilizador.DaValorAtributo("TDU_ResumoDiario", codigoResumoDiario, "CDU_Credito").ToString(),
                    ListacontaDestino.Valor(0), "EUR", listaDetalhes.Valor(2) - comissoesApuradas);
                    listaDetalhes.Seguinte();
                }

                for (int linhas = 1; linhas <= novoDocumentoTesouraria.Linhas.NumItens; linhas++)
                {
                    novoDocumentoTesouraria.Linhas.GetEdita(linhas).Cambio = 1;
                    novoDocumentoTesouraria.Linhas.GetEdita(linhas).CambioMBase = 1;
                    novoDocumentoTesouraria.Linhas.GetEdita(linhas).CambioMAlt = 200.4820000;

                }
                BSO.Tesouraria.Documentos.ValidaActualizacao(novoDocumentoTesouraria, ref Avisos);
                if (string.IsNullOrEmpty(Avisos) == false)
                {
                    novoDocumentoTesouraria.ValorMovimento = novoDocumentoTesouraria.TotalDebito - novoDocumentoTesouraria.TotalCredito;
                    BSO.Tesouraria.Documentos.Actualiza(novoDocumentoTesouraria);
                    //documentoGravado = BSO.Tesouraria.Documentos.Edita(novoDocumentoTesouraria.Filial, novoDocumentoTesouraria.Tipodoc, novoDocumentoTesouraria.Serie, novoDocumentoTesouraria.NumDoc);
                    //lancaMovimentoContabilidade(documentoGravado);    
                }
            }

        }

        private void lancaMovimentoContabilidade(TesBEDocumentoTesouraria novoDocumentoTesouraria)
        {
            TesBEMovimentoBancario novoMovimento = new TesBEMovimentoBancario();
            for (int linhas = 1; linhas <= novoDocumentoTesouraria.Linhas.NumItens; linhas++)
            {
                novoMovimento = new TesBEMovimentoBancario();
                novoMovimento.Conta = novoDocumentoTesouraria.Linhas.GetEdita(linhas).Conta;
                novoMovimento.Movimento = novoDocumentoTesouraria.Linhas.GetEdita(linhas).MovimentoBancario;
                if (novoDocumentoTesouraria.Linhas.GetEdita(linhas).Natureza == "D")
                {
                    novoMovimento.Valor = novoDocumentoTesouraria.Linhas.GetEdita(linhas).Debito;
                    novoMovimento.Natureza = "D";
                }
                else
                {
                    novoMovimento.Valor = novoDocumentoTesouraria.Linhas.GetEdita(linhas).Credito;
                    novoMovimento.Natureza = "C";

                }
                novoMovimento.Modulo = "B";
                novoMovimento.FilialOriginal = "000";
                novoMovimento.SerieOriginal = novoDocumentoTesouraria.Serie;
                novoMovimento.TipoDocOriginal = novoDocumentoTesouraria.Tipodoc;
                novoMovimento.NumDocOriginal = novoDocumentoTesouraria.NumDoc;
                novoMovimento.DataMov = novoDocumentoTesouraria.Linhas.GetEdita(linhas).DataMovimento;
                novoMovimento.DataValor = novoDocumentoTesouraria.Linhas.GetEdita(linhas).DataMovimento;
                novoMovimento.Estado = 1;
                novoMovimento.Descricao = novoDocumentoTesouraria.Linhas.GetEdita(linhas).Descricao;
                BSO.Tesouraria.MovimentoBancario.Actualiza(novoMovimento);
            }
        }

        private string copiaLinhaDocumento(String tipoDoc, int numdoc, string serie)
        {
            IntBEDocumentoInterno DocumentoOrigem = new IntBEDocumentoInterno();
            dynamic DocumentoDestino = new VndBEDocumentoVenda();
            VndBEDocumentoVenda dados = new VndBEDocumentoVenda();
            StdBELista ListaCamposUtil = new StdBELista();
            string strSql;
            StdBELista result;
            StdBEExecSql comandosql = new StdBEExecSql();
            int preencheDados = 5;
            string modulodestino;
            modulodestino = "V";
            string queryListaCamposUtil;
            string strID;

            try
            {
                queryListaCamposUtil = "select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = N'CabecInternos' and LEFT(COLUMN_NAME,4) = 'CDU_'";
                ListaCamposUtil = BSO.Consulta(queryListaCamposUtil);

                strID = BSO.Internos.Documentos.DaValorAtributo(tipoDoc, numdoc, serie, "000", "Id");

                DocumentoOrigem = BSO.Internos.Documentos.EditaID(strID);

                DocumentoDestino.Entidade = DocumentoOrigem.Entidade;
                DocumentoDestino.TipoEntidade = DocumentoOrigem.TipoEntidade;
                DocumentoDestino.Serie = DocumentoOrigem.Serie;
                DocumentoDestino.Referencia = DocumentoOrigem.Referencia;
                DocumentoDestino.DataDoc = DateTime.Now;
                DocumentoDestino.Responsavel = DocumentoOrigem.CamposUtil["CDU_Vendedor"];
                for (int i = 0; i <= ListaCamposUtil.NumLinhas(); i++)
                {
                    DocumentoDestino.CamposUtil[ListaCamposUtil.Valor(0)].Valor = DocumentoOrigem.CamposUtil[ListaCamposUtil.Valor(0)];
                    ListaCamposUtil.Seguinte();
                }

                BSO.Vendas.Documentos.PreencheDadosRelacionados(DocumentoDestino, ref preencheDados);

                DocumentoDestino.Morada = DocumentoOrigem.Morada;
                DocumentoDestino.Morada2 = DocumentoOrigem.Morada2;
                DocumentoDestino.Localidade = DocumentoOrigem.Localidade;
                DocumentoDestino.LocalidadeCodigoPostal = DocumentoOrigem.CodPostalLocalidade;
                DocumentoDestino.DataDoc = DateTime.Now;

                BSO.Vendas.Documentos.PreencheDadosRelacionados(DocumentoDestino, ref preencheDados);

                if (DocumentoOrigem.UtilizaMoradaAltEntrega == true)
                {
                    DocumentoDestino.UtilizaMoradaAlternativaEntreg = true;
                    DocumentoDestino.MoradaAlternativaEntrega = DocumentoOrigem.MoradaAltEntrega;
                }

                for (int numLinhas = 1; numLinhas <= DocumentoOrigem.Linhas.NumItens; numLinhas++)
                {
                    BSO.Internos.Documentos.CopiaLinha("N", DocumentoOrigem, ref modulodestino, ref DocumentoDestino, numLinhas);
                }
                BSO.Vendas.Documentos.Actualiza(DocumentoDestino);
                BSO.Internos.Documentos.ActualizaValorAtributo(tipoDoc, numdoc, serie, "000", "Estado", "A");
                BSO.Internos.Documentos.ActualizaValorAtributo(tipoDoc, numdoc, serie, "000", "CDU_Fatura", DocumentoDestino.Tipodoc + DocumentoDestino.Serie + "/" + DocumentoDestino.NumDoc);
                BSO.Internos.Documentos.ActualizaValorAtributo(tipoDoc, numdoc, serie, "000", "Referencia", DocumentoDestino.Tipodoc + DocumentoDestino.Serie + "/" + DocumentoDestino.NumDoc);
                BSO.Vendas.Documentos.ActualizaValorAtributo("000", DocumentoDestino.Tipodoc, DocumentoDestino.Serie, DocumentoDestino.NumDoc, "Requisicao", tipoDoc + serie + "/" + numdoc);
                BSO.Internos.Documentos.ActualizaValorAtributo(tipoDoc, numdoc, serie, "000", "CDU_Adjud", "Sim");
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }

        private void Sair_Click(object sender, EventArgs e)
        {
            try
            {
                //Ensure that resources released.
                priGrelha1.Termina();

                controlsInitialized = false;
                this.Close();
            }
            catch { }
        }

        private void TransformacaoDocumentos_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                //Save the current configuration.
                priGrelha1.GravaXML();
                //Ensure that resources released.  
                priGrelha1.Termina();
                controlsInitialized = false;
            }
            catch { }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            templateOpened = false;
            try
            {
                using (var result = BSO.Extensibility.CreateCustomFormInstance(typeof(TemplateInterno)))
                {
                    if (result.IsSuccess())
                    {
                        TemplateInterno.Plataforma = Plataforma;
                        TemplateInterno.MotorLE = MotorLE;
                        TemplateInterno.connectionString = constString;
                        (result.Result as TemplateInterno).ShowDialog();
                    }
                }
                do
                {

                }
                while (templateOpened == false);
                numericUpDown2.Value = comissoes;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.BackColor = System.Drawing.Color.LightSkyBlue;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackColor = SystemColors.Control;
        }

        private void numericUpDown2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == Keys.F4.ToString())
            {
                templateOpened = false;
                try
                {
                    using (var result = BSO.Extensibility.CreateCustomFormInstance(typeof(TemplateInterno)))
                    {
                        if (result.IsSuccess())
                        {
                            TemplateInterno.Plataforma = Plataforma;
                            TemplateInterno.MotorLE = MotorLE;
                            TemplateInterno.connectionString = constString;
                            (result.Result as TemplateInterno).ShowDialog();
                        }
                    }
                    do
                    {

                    }
                    while (templateOpened == false);
                    numericUpDown2.Value = comissoes;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void f4TabelaSQL1_Change(object Sender, F4TabelaSQL.ChangeEventArgs e)
        {
            StdBELista ListaSeries = new StdBELista();
            string queryListaSeries = "Select CDU_Serie from TDU_SeriesResumoDiario where CDU_ResumoDiario='" + f4TabelaSQL1.Text + "'";
            ListaSeries = BSO.Consulta(queryListaSeries);
            if (ListaSeries.Vazia() == false)
            {
                for (int i = 0; i <= ListaSeries.NumLinhas() - 1; i++)
                {
                    comboBox1.Items.Add(ListaSeries.Valor(0));
                    ListaSeries.Seguinte();
                }
                comboBox1.SelectedIndex = ListaSeries.NumLinhas() - 1;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            StdBELista listaDocumento = new StdBELista();
            StdBELista listaDocumentoAnulado = new StdBELista();
            string consultaDocumentoAnulado;
            string consultaDocumentoTesouraria;
            StdBECamposChave campochave = new StdBECamposChave();
            campochave.AddCampoChave("CDU_Codigo", f4TabelaSQL1.Text);
            consultaDocumentoTesouraria = " select CDU_DataFecho,CDU_DataDoc,CDU_ComissoesApuradas,CDU_ContaDestino from CabecTesouraria where CDU_TipoDoc ='" + f4TabelaSQL1.Text + "' and CDU_Serie ='" + comboBox1.Text + "' and CDU_NumDoc =" + numericUpDown1.Value + "";
            listaDocumento = BSO.Consulta(consultaDocumentoTesouraria);
            consultaDocumentoAnulado = "select cdu_dataDoc,Cdu_datadoc,0,'' from TDU_DocumentosFechoAnulado where CDU_TipoDoc = '" + f4TabelaSQL1.Text + "' and " +
                " CDU_Serie = '" + comboBox1.Text + "' and cdu_numdoc = " + numericUpDown1.Value + "";
            listaDocumentoAnulado = BSO.Consulta(consultaDocumentoAnulado);
            if (listaDocumento.Vazia() == false)
            {
                stdDatePicker1.Value = listaDocumento.Valor(0);
                stdDatePicker2.Value = listaDocumento.Valor(1);
                numericUpDown2.Value = Convert.ToDecimal(listaDocumento.Valor(2));
                f4TabelaSQL2.Text = listaDocumento.Valor(3);
                Converter.Enabled = false;
                stdDatePicker1.Enabled = false;
                stdDatePicker2.Enabled = false;
                lblAnulado.Visible = false;
            }
            else
            {
                if (listaDocumentoAnulado.Vazia() == true)
                {
                    stdDatePicker1.Value = DateTime.Now;
                    stdDatePicker2.Value = DateTime.Now;
                    numericUpDown2.Value = 0;
                    Converter.Enabled = true;
                    stdDatePicker1.Enabled = true;
                    stdDatePicker2.Enabled = true;
                    lblAnulado.Visible = false;
                }
                else
                {
                    stdDatePicker1.Value = listaDocumentoAnulado.Valor(0);
                    stdDatePicker2.Value = listaDocumentoAnulado.Valor(1);
                    numericUpDown2.Value = Convert.ToDecimal(listaDocumentoAnulado.Valor(2));
                    Converter.Enabled = false;
                    stdDatePicker1.Enabled = false;
                    stdDatePicker2.Enabled = false;
                    lblAnulado.Visible = true;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            StdBELista NumeroMaximo = new StdBELista();
            StdBECamposChave campochave = new StdBECamposChave();
            string ConsultaNumeroMaximo;
            campochave.AddCampoChave("CDU_Codigo", f4TabelaSQL1.Text);
            ConsultaNumeroMaximo = "select max(CabecTesouraria.cdu_numDoc),max(numdoc),max(tipodoc),CASE WHEN  max(TDU_DocumentosFechoAnulado.cdu_numDoc) IS NULL THEN 0 ELSE max(TDU_DocumentosFechoAnulado.cdu_numDoc) END from CabecTesouraria " +
                                   " left outer join TDU_DocumentosFechoAnulado on TDU_DocumentosFechoAnulado.CDU_TipoDoc = '" + f4TabelaSQL1.Text + "' and " +
                                   " TDU_DocumentosFechoAnulado.CDU_Serie = '" + comboBox1.Text + "' " +
                                   " where CabecTesouraria.CDU_TipoDoc = '" + f4TabelaSQL1.Text + "' and CabecTesouraria.CDU_Serie = '" + comboBox1.Text + "'";
            NumeroMaximo = BSO.Consulta(ConsultaNumeroMaximo);


            if (string.IsNullOrEmpty(Convert.ToString(NumeroMaximo.Valor(0))) == false)
            {
                if (string.IsNullOrEmpty(Convert.ToString(NumeroMaximo.Valor(3))) == true)
                {
                    numericUpDown1.Maximum = Convert.ToInt32(NumeroMaximo.Valor(0)) + 1;
                    numericUpDown1.Value = Convert.ToInt32(NumeroMaximo.Valor(0)) + 1;
                    stdDatePicker1.Value = Convert.ToDateTime(BSO.Tesouraria.Documentos.DaValorAtributo("000", NumeroMaximo.Valor(2), comboBox1.Text, NumeroMaximo.Valor(1), "Data")).AddDays(1);
                    stdDatePicker2.Value = Convert.ToDateTime(BSO.Tesouraria.Documentos.DaValorAtributo("000", NumeroMaximo.Valor(2), comboBox1.Text, NumeroMaximo.Valor(1), "Data")).AddDays(1);
                }
                else if (NumeroMaximo.Valor(0) > NumeroMaximo.Valor(3))
                {
                    //MessageBox.Show(Convert.ToString(BSO.Tesouraria.Documentos.DaValorAtributo("000", NumeroMaximo.Valor(2), comboBox1.Text, NumeroMaximo.Valor(1), "Data")).ToString("yyyy/MM/dd"));
                    numericUpDown1.Maximum = Convert.ToInt32(NumeroMaximo.Valor(0)) + 1;
                    numericUpDown1.Value = Convert.ToInt32(NumeroMaximo.Valor(0)) + 1;
                    stdDatePicker1.Value = Convert.ToDateTime(BSO.Tesouraria.Documentos.DaValorAtributo("000", NumeroMaximo.Valor(2), comboBox1.Text, NumeroMaximo.Valor(1), "Data")).AddDays(1);
                    stdDatePicker2.Value = Convert.ToDateTime(BSO.Tesouraria.Documentos.DaValorAtributo("000", NumeroMaximo.Valor(2), comboBox1.Text, NumeroMaximo.Valor(1), "Data")).AddDays(1);
                }
                else
                {
                    numericUpDown1.Maximum = Convert.ToInt32(NumeroMaximo.Valor(3)) + 1;
                    numericUpDown1.Value = Convert.ToInt32(NumeroMaximo.Valor(3)) + 1;
                    stdDatePicker1.Value = Convert.ToDateTime(BSO.Tesouraria.Documentos.DaValorAtributo("000", NumeroMaximo.Valor(2), comboBox1.Text, NumeroMaximo.Valor(1), "Data")).AddDays(1);
                    stdDatePicker2.Value = Convert.ToDateTime(BSO.Tesouraria.Documentos.DaValorAtributo("000", NumeroMaximo.Valor(2), comboBox1.Text, NumeroMaximo.Valor(1), "Data")).AddDays(1);
                }
            }
            else
            {
                numericUpDown1.Maximum = 1;
                numericUpDown1.Value = 1;
            }

        }

        private void Anular_Click(object sender, EventArgs e)
        {
            string queryDocumentos = "";
            string strAvisos = "";
            StdBECamposChave campoChave = new StdBECamposChave();
            string chaveId = "";
            StdBELista consultaDocumentosRelacionados = new StdBELista();
            queryDocumentos = "select tipodoc,serie,numdoc from CabecTesouraria where CDU_TipoDoc = '" + f4TabelaSQL1.Text + "' and CDU_Serie = '" + comboBox1.Text + "' and CDU_numdoc = " + numericUpDown1.Value + "";
            consultaDocumentosRelacionados = BSO.Consulta(queryDocumentos);

            if (consultaDocumentosRelacionados.Vazia() == false)
            {
                for (int i = 0; i < consultaDocumentosRelacionados.NumLinhas(); i++)
                {
                    try
                    {
                        if (BSO.Tesouraria.Documentos.ValidaRemocao("000", consultaDocumentosRelacionados.Valor(0), consultaDocumentosRelacionados.Valor(1), consultaDocumentosRelacionados.Valor(2), ref strAvisos) == true)
                        {
                            BSO.Tesouraria.Documentos.Remove("000", consultaDocumentosRelacionados.Valor(0), consultaDocumentosRelacionados.Valor(1), consultaDocumentosRelacionados.Valor(2));
                            chaveId = System.Guid.NewGuid().ToString();
                            InsertNovaConta(chaveId);
                            campoChave.AddCampoChave("CDU_Id", chaveId);
                            BSO.TabelasUtilizador.ActualizaValorAtributo("TDU_DocumentosFechoAnulado", campoChave, "CDU_TipoDoc", f4TabelaSQL1.Text);
                            BSO.TabelasUtilizador.ActualizaValorAtributo("TDU_DocumentosFechoAnulado", campoChave, "CDU_Serie", comboBox1.Text);
                            BSO.TabelasUtilizador.ActualizaValorAtributo("TDU_DocumentosFechoAnulado", campoChave, "CDU_NumDoc", numericUpDown1.Value);
                            BSO.TabelasUtilizador.ActualizaValorAtributo("TDU_DocumentosFechoAnulado", campoChave, "CDU_DataDoc", stdDatePicker1.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        PSO.Dialogos.MostraMensagem(StdBSTipos.TipoMsg.PRI_SimplesOk, ex.Message, StdBSTipos.IconId.PRI_Critico);
                    }
                    consultaDocumentosRelacionados.Seguinte();
                }
            }
        }
        public static bool InsertNovaConta(string Nome)
        {
            string strSql;
            int novaConta;
            try
            {
                string[] parametrosNomes = new string[1];
                parametrosNomes[0] = "@CDU_Id";
                string[] parametrosValores = new string[1];
                parametrosValores[0] = Nome;
                strSql = "INSERT INTO TDU_DocumentosFechoAnulado (CDU_Id) values (@CDU_Id)";
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
                string conString = constString;//Plataforma.BaseDados.DaConnectionStringNET(Plataforma.BaseDados.DaNomeBDdaEmpresa(MotorLE.Contexto.CodEmp), "DEFAULT");
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



        private void btnNovo_Click(object sender, EventArgs e)
        {
            criarNovoFecho();
            Atualizar_Click(sender, e);
        }
    }
}
