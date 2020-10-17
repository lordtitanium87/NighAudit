using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using NightAudit.Properties;
using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.Extensions;
using Primavera.Extensibility.Platform.Services;
using StdPlatBS100;

namespace NightAudit
{
    public class PrimaveraRibbon : Plataforma
    {
        const string cIDTAB = "10000";
        const string cIDGROUP = "100001";
        const string cIDBUTTON1 = "1000011";
        const string cIDBUTTON2 = "1000012";
        private StdBSPRibbon RibbonEvents;
        public override void DepoisDeCriarMenus(ExtensibilityEventArgs e)
        {
            RibbonEvents = this.PSO.Ribbon;
            RibbonEvents.Executa += RibbonEvents_Executa;
            // Create a new TAB.
            this.PSO.Ribbon.CriaRibbonTab("Módulo Night Audit", cIDTAB, 10);
            // Create a new Group.
            this.PSO.Ribbon.CriaRibbonGroup(cIDTAB, "Acesso", cIDGROUP);

            // Create a new 32x32 Button.
            this.PSO.Ribbon.CriaRibbonButton(cIDTAB, cIDGROUP, cIDBUTTON1, "Abrir Aplicação", true, Resources.processar);
        }
        ///
        /// Ribbon events.
        ///
        ///
        ///
        private void RibbonEvents_Executa(string Id, string Comando)
        {
            try
            {
                switch (Id)
                {
                    case cIDBUTTON1:
                        //Call action.
                        //StdBSTipos.ResultMsg resultadoPergunta = new StdBSTipos.ResultMsg();
                        //resultadoPergunta = PSO.Dialogos.MostraMensagem(StdPlatBS100.StdBSTipos.TipoMsg.PRI_SimNao, "Pretende aceder ao módulo de night Audit?", StdPlatBS100.StdBSTipos.IconId.PRI_Questiona);
                        //if (resultadoPergunta == StdBSTipos.ResultMsg.PRI_Sim)
                        //{
                            try
                            {
                                using (var result = BSO.Extensibility.CreateCustomFormInstance(typeof(frm_NightAudit)))
                                {
                                    if (result.IsSuccess())
                                    {
                                        frm_NightAudit.Plataforma = PSO;
                                        frm_NightAudit.MotorLE = BSO;
                                        string conString = PSO.BaseDados.DaConnectionStringNET(PSO.BaseDados.DaNomeBDdaEmpresa(BSO.Contexto.CodEmp), "DEFAULT");
                                        frm_NightAudit.constString = conString;
                                        (result.Result as frm_NightAudit).ShowDialog();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        //}
                        break;
                }
            }
            catch (System.Exception ex)
            {
                PSO.Dialogos.MostraAviso("Fail to execute the command.", StdBSTipos.IconId.PRI_Informativo, ex.Message);
            }
        }
    }
}
     
 
